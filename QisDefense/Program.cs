using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace QisDefense
{
    internal class Program
    {
        #region Native Methods

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern SafeFileHandle CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetInformationProcess(
            IntPtr hProcess,
            int ProcessInformationClass,
            ref int ProcessInformation,
            int ProcessInformationLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        #endregion

        #region Constants

        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint GENERIC_EXECUTE = 0x20000000;
        private const uint FILE_SHARE_READ = 0x00000001;
        private const uint FILE_SHARE_WRITE = 0x00000002;
        private const uint OPEN_EXISTING = 3;

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int ProcessBreakOnTermination = 0x1D;

        #endregion

        #region Delegates & Fields

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr _hookID = IntPtr.Zero;
        private static LowLevelKeyboardProc _proc = HookCallback;

        private static SafeFileHandle _fileHandle;
        private static string _protectedFilePath;
        private static bool _isCriticalProcess = false;
        private static bool _isRunning = true;
        private static bool _isSystem = false;

        private static Mutex _mutex;
        
        // 全局字典：文件路径 → 文件句柄
        private static Dictionary<string, SafeFileHandle> _fileHandles = new Dictionary<string, SafeFileHandle>();

        #endregion

        #region Main Entry

        [STAThread]
        static void Main(string[] args)
        {
            // 防止多实例
            using (_mutex = new Mutex(true, "QisDefense_Protection_Mutex", out bool createdNew))
            {
                if (!createdNew)
                {
                    MessageBox.Show("齐之防御已经在运行中！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 设置目标文件路径（默认为同目录下的 QisToolkit3.exe）
                _protectedFilePath = args.Length > 0 ? args[0] :
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "QisToolkit3.exe");

                // 检查目标文件是否存在
                if (!File.Exists(_protectedFilePath))
                {
                    MessageBox.Show($"绝对依赖 齐的工具包3 未找到：\n{_protectedFilePath}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 获取当前进程信息
                int processId = Process.GetCurrentProcess().Id;
                string owner = GetProcessOwner(processId);
                _isSystem = owner.Equals(@"SYSTEM\NT AUTHORITY", StringComparison.OrdinalIgnoreCase);

                // 如果不是系统权限，尝试提升
                if (!_isSystem)
                {
                    TryElevatePrivilege();
                    //DialogResult result = MessageBox.Show(
                    //    "当前不是 SYSTEM 权限，无法提供完整保护！\n\n" +
                    //    "是否使用齐的工具包3自动提权？",
                    //    "权限不足",
                    //    MessageBoxButtons.YesNo,
                    //    MessageBoxIcon.Question);

                    //if (result == DialogResult.Yes)
                    //{
                    //    TryElevatePrivilege();
                    //    return;
                    //}
                    //else
                    //{
                    //    MessageBox.Show(
                    //        "未获得 SYSTEM 权限，防护功能将受限。\n" +
                    //        "文件仍会被锁定，但进程被杀不会蓝屏。",
                    //        "警告",
                    //        MessageBoxButtons.OK,
                    //        MessageBoxIcon.Warning);
                    //}
                }

                // 初始化保护
                if (!InitializeProtection())
                {
                    MessageBox.Show("初始化防护失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SystemEvents.SessionEnding += OnSessionEnding;
                SystemEvents.SessionEnded += OnSessionEnded;

                // 设置键盘钩子（用于退出热键）
                _hookID = SetHook(_proc);

                PipeServer pipeServer = new PipeServer("QisDefensePipe");
                pipeServer.CommandReceived += OnPipeCommand;
                pipeServer.Start();

                // 启动文件完整性监视
                Thread monitorThread = new Thread(MonitorFileIntegrity) { IsBackground = true };
                monitorThread.Start();

                //new ReadBookInjector().StartMonitoring();

                // 无界面消息循环
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ApplicationContext());

                // 清理资源（正常退出时执行）
                Cleanup();
            }
        }

        private static void OnPipeCommand(object sender, PipeCommandEventArgs e)
        {
            // 处理管道命令（如果需要在UI线程处理）
            // 这里可以直接调用 LockFile/UnlockFile 等方法
        }

        // 关机/注销即将发生时触发
        private static void OnSessionEnding(object sender, SessionEndingEventArgs e)
        {
            // 立即取消关键进程标记，防止蓝屏
            UnsetCriticalProcess();

            // 快速清理资源
            Cleanup();

            // 让系统继续关机流程
        }

        // 关机/注销完成时触发（保险）
        private static void OnSessionEnded(object sender, SessionEndedEventArgs e)
        {
            UnsetCriticalProcess();
            Cleanup();
        }

        #endregion

        #region Protection Methods

        private static bool InitializeProtection()
        {
            try
            {
                // 1. 锁定文件（独占句柄，禁止删除共享）
                if (!StartLockFiles())
                {
                    return false;
                }

                // 2. 如果是 SYSTEM 权限，标记为关键进程
                if (_isSystem)
                {
                    SetCriticalProcess();
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化防护失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool StartLockFiles()
        {
            try
            {
                // 齐的工具包主程序
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;

                LockFile(_protectedFilePath);
                LockFile(Path.Combine(baseDir, "Microsoft.Bcl.Cryptography.dll"));
                LockFile(Path.Combine(baseDir, "Microsoft.Win32.TaskScheduler.dll"));
                LockFile(Path.Combine(baseDir, "Newtonsoft.Json.dll"));
                LockFile(Path.Combine(baseDir, "NSudoAPI.dll"));
                LockFile(Path.Combine(baseDir, "QisToolkit3.deps.json"));
                LockFile(Path.Combine(baseDir, "QisToolkit3.dll"));
                LockFile(Path.Combine(baseDir, "QisToolkit3.exe"));
                LockFile(Path.Combine(baseDir, "QisToolkit3.pdb"));
                LockFile(Path.Combine(baseDir, "QisToolkit3.runtimeconfig.json"));
                LockFile(Path.Combine(baseDir, "System.CodeDom.dll"));
                LockFile(Path.Combine(baseDir, "System.Configuration.ConfigurationManager.dll"));
                LockFile(Path.Combine(baseDir, "System.Diagnostics.EventLog.dll"));
                LockFile(Path.Combine(baseDir, "System.DirectoryServices.AccountManagement.dll"));
                LockFile(Path.Combine(baseDir, "System.DirectoryServices.dll"));
                LockFile(Path.Combine(baseDir, "System.DirectoryServices.Protocols.dll"));
                LockFile(Path.Combine(baseDir, "System.Formats.Asn1.dll"));
                LockFile(Path.Combine(baseDir, "System.Management.dll"));
                LockFile(Path.Combine(baseDir, "System.Security.Cryptography.ProtectedData.dll"));
                LockFile(Path.Combine(baseDir, "Uninstall_QisToolkit3.exe"));

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动锁定文件异常：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool LockFile(string filePath, int mode = 0)
        {
            try
            {
                SafeFileHandle handle = null;

                switch (mode)
                {
                    // 可读 可写 可用 不可删
                    case 0:
                    default:
                        handle = CreateFile(
                            filePath,
                            GENERIC_READ,
                            FILE_SHARE_READ | FILE_SHARE_WRITE,
                            IntPtr.Zero,
                            OPEN_EXISTING,
                            0,
                            IntPtr.Zero
                        );
                        break;

                    // 可读 可写 不可用 不可删
                    case 1:
                        handle = CreateFile(
                            filePath,
                            GENERIC_READ | GENERIC_WRITE,
                            FILE_SHARE_READ | FILE_SHARE_WRITE,
                            IntPtr.Zero,
                            OPEN_EXISTING,
                            0,
                            IntPtr.Zero
                        );
                        break;

                    // 可读 不可写 不可用 不可删
                    case 2:
                        handle = CreateFile(
                            filePath,
                            GENERIC_READ | GENERIC_WRITE,
                            FILE_SHARE_READ,
                            IntPtr.Zero,
                            OPEN_EXISTING,
                            0,
                            IntPtr.Zero
                        );
                        break;

                    // 不可读 不可写 不可用 不可删
                    case 3:
                        handle = CreateFile(
                            filePath,
                            GENERIC_READ | GENERIC_WRITE,
                            0,
                            IntPtr.Zero,
                            OPEN_EXISTING,
                            0,
                            IntPtr.Zero
                        );
                        break;
                }

                if (handle == null || handle.IsInvalid)
                {
                    int error = Marshal.GetLastWin32Error();
                    return false;
                }

                if (_fileHandles.ContainsKey(filePath))
                {
                    _fileHandles[filePath]?.Dispose();
                    _fileHandles[filePath] = handle;
                }
                else
                {
                    _fileHandles.Add(filePath, handle);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool UnlockFile(string filePath)
        {
            try
            {
                if (!_fileHandles.ContainsKey(filePath))
                {
                    return false; // 该文件没有被锁定
                }

                // 获取句柄并释放
                SafeFileHandle handle = _fileHandles[filePath];
                if (handle != null && !handle.IsInvalid)
                {
                    handle.Dispose();
                }

                // 从字典中移除
                _fileHandles.Remove(filePath);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsFileLocked(string filePath)
        {
            return _fileHandles.ContainsKey(filePath) &&
                   _fileHandles[filePath] != null &&
                   !_fileHandles[filePath].IsInvalid;
        }

        public static int GetFileLockMode(string filePath)
        {
            // 如果你需要知道锁定模式，需要在字典中额外存储模式信息
            // 或者直接从句柄特性判断（比较麻烦）
            // 建议用另一个字典存储模式：Dictionary<string, int> _fileLockModes
            return -1; // 未知
        }

        private static void SetCriticalProcess()
        {
            try
            {
                int criticalFlag = 1;
                int result = NtSetInformationProcess(
                    GetCurrentProcess(),
                    ProcessBreakOnTermination,
                    ref criticalFlag,
                    sizeof(int));

                if (result == 0)
                {
                    _isCriticalProcess = true;
                }
            }
            catch
            {
                // 静默失败，不影响主要功能
            }
        }

        public static bool DeleteFile(string filepath)
        {
            try
            {
                System.IO.File.Delete(filepath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 取消关键进程标记（退出前必须调用，否则杀进程会蓝屏）
        /// </summary>
        private static void UnsetCriticalProcess()
        {
            if (!_isCriticalProcess)
                return;

            try
            {
                int criticalFlag = 0; // FALSE - 取消关键标记
                int result = NtSetInformationProcess(
                    GetCurrentProcess(),
                    ProcessBreakOnTermination,
                    ref criticalFlag,
                    sizeof(int));

                if (result == 0)
                {
                    _isCriticalProcess = false;
                }
            }
            catch
            {
                // 忽略异常
            }
        }

        private static void MonitorFileIntegrity()
        {
            while (_isRunning)
            {
                try
                {
                    // 检查文件是否存在
                    if (!File.Exists(_protectedFilePath))
                    {
                        MessageBox.Show(
                            $"检测到受保护文件已被删除！\n\n{_protectedFilePath}\n\n" +
                            "请从备份恢复文件，或重启程序重新锁定。",
                            "齐之防御 - 文件丢失警告",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }

                    // 验证文件句柄是否有效
                    if (_fileHandle == null || _fileHandle.IsInvalid)
                    {
                        RelockFile();
                    }
                }
                catch
                {
                    // 忽略异常，继续运行
                }

                Thread.Sleep(3000);
            }
        }

        private static void RelockFile()
        {
            try
            {
                if (_fileHandle != null && !_fileHandle.IsInvalid)
                {
                    _fileHandle.Dispose();
                    _fileHandle = null;
                }

                StartLockFiles();
            }
            catch
            {
                // 静默重试
            }
        }

        #endregion

        #region Privilege Management

        public static string GetProcessOwner(int processId)
        {
            try
            {
                string query = $"SELECT * FROM Win32_Process WHERE ProcessId = {processId}";
                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        object[] args = { null, null };
                        object result = obj.InvokeMethod("GetOwner", args);
                        if (result != null && (uint)result == 0)
                        {
                            string domain = args[0]?.ToString();
                            string user = args[1]?.ToString();
                            return $@"{domain}\{user}";
                        }
                    }
                }
                return "未知";
            }
            catch
            {
                return "未知";
            }
        }

        private static bool TryElevatePrivilege()
        {
            string regPath = @"HKEY_CURRENT_USER\Software\QisDefense";
            string valueName = "ElevateAttemptCount";
            string resetTimeName = "ElevateAttemptReset";

            // 读取尝试次数
            int attemptCount = (int)(Registry.GetValue(regPath, valueName, 0) ?? 0);
            string lastReset = (string)Registry.GetValue(regPath, resetTimeName, "");

            // 检查是否需要重置（每天重置一次）
            if (DateTime.Now.ToString("yyyy-MM-dd") != lastReset)
            {
                attemptCount = 0;
                Registry.SetValue(regPath, resetTimeName, DateTime.Now.ToString("yyyy-MM-dd"));
            }

            // 超过3次直接放弃
            if (attemptCount >= 3)
            {
                MessageBox.Show(
                    $"自动提权已失败 {attemptCount} 次，请手动以 SYSTEM 权限运行。",
                    "提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return false;
            }

            // 尝试提权
            try
            {
                string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "QisToolkit3.exe");
                string currentExe = Process.GetCurrentProcess().MainModule.FileName;

                if (!File.Exists(exePath))
                {
                    MessageBox.Show("未找到齐的工具包3，无法提权。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                Process.Start("QisToolkit3.exe", $"-SPL \"{currentExe}\" \"{_protectedFilePath}\"");

                // ✅ 提权成功，重置计数
                Registry.SetValue(regPath, valueName, 0);
                Registry.SetValue(regPath, resetTimeName, DateTime.Now.ToString("yyyy-MM-dd"));

                Environment.Exit(0);
                return true;
            }
            catch
            {
                // ❌ 提权失败，增加计数
                attemptCount++;
                Registry.SetValue(regPath, valueName, attemptCount);

                MessageBox.Show(
                    $"自动提权失败（第 {attemptCount} 次）",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
        }

        #endregion

        #region Keyboard Hook

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                // Ctrl+Alt+Esc 组合键退出
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                    (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                    (Keys)vkCode == Keys.Escape)
                {
                    OnExitRequest();
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(_hookID, nCode, (int)wParam, lParam);
        }

        private static void OnExitRequest()
        {
            DialogResult result = MessageBox.Show(
                "确定要退出齐之防御吗？\n\n" +
                "退出后文件将失去保护，可以被删除。",
                "确认退出",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                _isRunning = false;

                // ⚠️ 关键：先取消关键进程标记，再释放句柄
                // 顺序很重要！如果先释放句柄再取消标记，进程被终止时仍会蓝屏
                UnsetCriticalProcess();

                Cleanup();
                Application.Exit();
                Environment.Exit(0);
            }
        }

        #endregion

        #region Cleanup

        private static void Cleanup()
        {
            try
            {
                // 卸载键盘钩子
                if (_hookID != IntPtr.Zero)
                {
                    UnhookWindowsHookEx(_hookID);
                    _hookID = IntPtr.Zero;
                }

                // 释放文件句柄
                if (_fileHandle != null && !_fileHandle.IsInvalid)
                {
                    _fileHandle.Dispose();
                    _fileHandle = null;
                }
            }
            catch
            {
                // 忽略清理异常
            }
        }

        #endregion
    }
}