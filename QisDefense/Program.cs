using QisDefense;
using QisDefense.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.XPath;

namespace QisDefense
{
    internal class Program
    {
        // 导入user32.dll用于设置键盘钩子
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static IntPtr _hookID = IntPtr.Zero;
        private static LowLevelKeyboardProc _proc = HookCallback; 

        private static string ProgramPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "QisDefense.exe"); 
        private static string ProgramDirPath = AppDomain.CurrentDomain.BaseDirectory; 
        
        public static bool MessageReporting;
        public static bool AutoCloseLauncher;
        public static bool NoGameMode;
        public static bool SysReStart;
        public static bool AutoCheck;
        public static bool CuttingBoardMonitoring;

        public static bool CBM_PhoneNumber, CBM_AutoNumber, CBM_AutoStringReplacer;
        public static string CBM_PhoneNumber_TargetType, CBM_AutoNumber_Str;
        public static Dictionary<int, string> CBM_PhoneNumber_Type = new Dictionary<int, string>();

        public static int processId = Process.GetCurrentProcess().Id;
        public static string owner = GetProcessOwner(processId);
        public static bool isSystem = owner.Equals(@"SYSTEM\NT AUTHORITY", StringComparison.OrdinalIgnoreCase);

        // 使用示例
        public static ConfigReader config = new ConfigReader("QisDefenseConfig.ini");


        // 侦测程序
        public static DateTime? lastSteamStartTime = null;
        public static string lastClipboardText = "";




        private static Form _controllerForm;






        static void Main(string[] args)
        {
            MessageBox.Show("由于齐的工具包3更新，齐之防御暂不可用，请等待兼容最新SPL启动接口。", "齐之防御");
            Environment.Exit(0);




            if (IsAlreadyRunning())
            {
                MessageBox.Show("请勿重复运行齐之防御！", "齐之防御", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(0);
            }

            else if (MessageReporting)
                MessageBox.Show("齐之防御已启动", "齐之防御");

            if (args.Length > 0)
            {
                foreach (string arg in args)
                {
                    if (arg == "SysReStart")
                    {
                        SysReStart = true;
                    }
                }
            }


            // 配置文件
            string INIPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".", "QisDefenseConfig.ini");
            if (File.Exists(INIPath))
            {
                try
                {
                    MessageReporting = config.GetBool("MessageReporting");
                    SysReStart = config.GetBool("AutoSysReStart") || SysReStart;
                    AutoCloseLauncher = config.GetBool("AutoCloseLauncher");
                    NoGameMode = config.GetBool("NoGameMode");
                    AutoCheck = config.GetBool("AutoCheck");
                    CuttingBoardMonitoring = config.GetBool("CuttingBoardMonitoring");
                    CBM_PhoneNumber = config.GetBool("CBM.PhoneNumber");
                    CBM_AutoNumber = config.GetBool("CBM.AutoNumber");
                    CBM_AutoStringReplacer = config.GetBool("CBM.AutoStringReplacer");


                    if (CBM_PhoneNumber)
                    {
                        // 获取支持的格式
                        for (int i = 0; config.GetIsValueHave($"CBM.PhoneNumberType.{i}"); ++i)
                            CBM_PhoneNumber_Type.Add(i, config.GetString($"CBM.PhoneNumberType.{i}"));

                        // 获取转换目标格式
                        CBM_AutoNumber_Str = config.GetString("CBM.PhoneNumberType.Target");
                    }

                    if (CBM_AutoNumber)
                    {
                        CBM_AutoNumber_Str = "$1" + config.GetString("CBM.AutoNumber.Str");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"读取配置文件时出错\n报错信息：{ex.Message}", "齐之防御", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }



            // 非启动级权限自动重启
            if (SysReStart && !isSystem)
            {
                RunNSudo(ProgramPath);
                Environment.Exit(0);
            }


            // Warframe 检测
            else if (IsProcessRunning("Launcher") && AutoCloseLauncher)
            {
                MessageBox.Show("Warframe 启动器或其他游戏启动器已在运行，齐之防御不再启动。", "提示");
                Environment.Exit(0);
            }


            // 运行自检查
            else if (AutoCheck)
            {
                try
                {
                    // 获取所有进程列表
                    Process[] processes = Process.GetProcesses();

                    // 将进程列表存储在变量中
                    var processList = processes.ToList();

                }
                catch
                {

                }
            }




            // 设置键盘钩子
            _hookID = SetHook(_proc);

            // 启动 WMI 监视
            if (AutoCloseLauncher) new WmiProcessMonitor("Launcher");

            if (NoGameMode)
            {
                // steam
                new WmiProcessMonitor("steam.exe", "KillProcess");
                new WmiProcessMonitor("steamwebhelper", "KillProcess");
            }
            else
            {
                new WmiProcessMonitor("steam.exe", "Steam");
            }

            if (CuttingBoardMonitoring)
            {
                Task.Run(() => MonitorClipboard());
            }

            _controllerForm = new Form() { ShowInTaskbar = false, WindowState = FormWindowState.Minimized };
            Application.Run(_controllerForm);

            // 程序退出时卸载钩子
            UnhookWindowsHookEx(_hookID);
        }

        private static bool IsInProcessList(List<Process> processList, string targetProcess)
        {
            return processList.Any(p =>
                p.ProcessName.Equals(targetProcess, StringComparison.OrdinalIgnoreCase) ||
                p.ProcessName.Equals($"{targetProcess}.exe", StringComparison.OrdinalIgnoreCase));
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        // 组合键
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                // CMD（Ctrl+Alt+F1组合键）
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                    (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                    (Keys)vkCode == Keys.F1)
                {
                    RunNSudo("cmd.exe");
                }

                // PowerShell（Ctrl+Alt+F2组合键）
                else if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.F2)
                {
                    RunNSudo("powershell.exe", true);
                }

                // 任务管理器（Ctrl+Alt+F3组合键）
                else if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.F3)
                {
                    RunNSudo("Taskmgr.exe");
                }

                // 注册表编辑器（Ctrl+Alt+F4组合键）
                else if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.F4)
                {
                    RunNSudo("regedit.exe", true);
                }

                // 系统配置工具（Ctrl+Alt+F5组合键）
                else if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.F5)
                {
                    RunNSudo("msconfig.exe");
                }

                // 资源监视器（Ctrl+Alt+F6组合键）
                else if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.F6)
                {
                    RunNSudo("resmon.exe");
                }

                // 系统信息（Ctrl+Alt+F7组合键）
                else if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.F7)
                {
                    RunNSudo("msinfo32.exe");
                }

                // DirectX 诊断工具（Ctrl+Alt+F8组合键）
                else if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.F8)
                {
                    RunNSudo("dxdiag.exe");
                }

                // UAC调整工具（Ctrl+Alt+F9组合键）
                else if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.F9)
                {
                    RunNSudo("UserAccountControlSettings.exe", true);
                    new MessageForm().ShowDialog();
                }


                // 齐的工具包3（Ctrl+Alt+F12组合键）
                else if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.F12)
                {
                    string exePath = Path.GetFullPath(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".", "QisToolkit3.exe"));

                    if (exePath != null && File.Exists(exePath))
                    {
                        RunNSudo(exePath);
                        //Process.Start(exePath);
                    }
                    else
                    {
                        MessageBox.Show("未找到齐的工具包3，请检查环境相关问题。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                // 退出（Ctrl+Alt+ESC组合键）
                else if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.Escape)
                {
                    new MessageForm("Exit").Show();
                }

                // 用户自定义按键
                /*else 
                {
                    if (File.Exists(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad0.bat")))
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.NumPad0)
                            ExecuteInCmd(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad0.bat"));
                    }

                    else if (File.Exists(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad1.bat")))
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.NumPad1)
                            ExecuteInCmd(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad1.bat"));
                    }

                    else if (File.Exists(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad2.bat")))
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.NumPad2)
                            ExecuteInCmd(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad2.bat"));
                    }

                    else if (File.Exists(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad3.bat")))
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.NumPad3)
                            ExecuteInCmd(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad3.bat"));
                    }

                    else if (File.Exists(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad4.bat")))
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.NumPad4)
                            ExecuteInCmd(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad4.bat"));
                    }

                    else if (File.Exists(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad5.bat")))
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.NumPad5)
                            ExecuteInCmd(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad5.bat"));
                    }

                    else if (File.Exists(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad6.bat")))
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.NumPad6)
                            ExecuteInCmd(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad6.bat"));
                    }

                    else if (File.Exists(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad7.bat")))
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.NumPad7)
                            ExecuteInCmd(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad7.bat"));
                    }

                    else if (File.Exists(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad8.bat")))
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.NumPad8)
                            ExecuteInCmd(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad8.bat"));
                    }

                    else if (File.Exists(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad9.bat")))
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                         (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                         (Keys)vkCode == Keys.NumPad9)
                            ExecuteInCmd(Path.Combine(ProgramDirPath, @"QisDefenseRUN\Numpad9.bat"));
                    }
                }*/
            }
            return CallNextHookEx(_hookID, nCode, (int)wParam, lParam);
        }



        // 运行 Cmd 命令
        public static string ExecuteInCmd(string cmdline)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.StandardInput.AutoFlush = true;
                process.StandardInput.WriteLine(cmdline + "&exit");

                string output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
                process.Close();
                return output;
            }
        }

        // 使用 NSudo 运行程序
        public static void RunNSudo(string exefile, bool CMDReLoad = false, string arguments = "-U:T -P:E -M:S -Priority:RealTime")
        {
            string nsudoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NSudoL.exe");

            if (CMDReLoad)
                ExecuteInCmd($"{nsudoPath} {arguments} {exefile}");

            else
            {
                string log = string.Empty;

                try
                {
                    // 构建完整命令参数
                    string nsudoArgs = $"-U:T -P:E -M:S -Priority:RealTime \"{exefile}\"";

                    if (!string.IsNullOrEmpty(arguments))
                    {
                        nsudoArgs += $" {arguments}";
                    }

                    // 配置进程启动信息
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = nsudoPath,
                        Arguments = nsudoArgs,
                        UseShellExecute = false, // 重定向输出需要设为 false
                        CreateNoWindow = true,   // 不创建新窗口
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    // 启动进程
                    using (var process = new Process { StartInfo = startInfo })
                    {
                        // 捕获输出事件
                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                                log += $"[NSudo] {e.Data}";
                        };

                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                                log += $"[NSudo Error] {e.Data}";
                        };

                        process.Start();

                        // 开始异步读取输出
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        // 等待进程退出（可选）
                        // process.WaitForExit();

                        // 获取退出代码（如果需要）
                        // int exitCode = process.ExitCode;
                    }
                }
                catch
                {

                }
            }
        }

        static bool IsAlreadyRunning()
        {
            Process currentProcess = Process.GetCurrentProcess();
            string currentProcessName = currentProcess.ProcessName;

            // 获取相同名称的进程数量
            int runningCount = Process.GetProcesses()
                                      .Count(p => p.ProcessName == currentProcessName);

            return runningCount > 1;
        }

        private static bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }



        public static string GetProcessOwner(int processId)
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

        private static void MonitorClipboard()
        {
            // 创建 STA 线程来监控剪贴板
            Thread clipboardThread = new Thread(() =>
            {
                while (CuttingBoardMonitoring)
                {
                    try
                    {
                        // 需要在 STA 线程中访问剪贴板
                        if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }

                        // 使用 Application.DoEvents() 确保消息循环处理
                        Application.DoEvents();

                        string clipboardText = null;
                        bool hasText = false;

                        // 在 STA 线程中安全地访问剪贴板
                        hasText = Clipboard.ContainsText();

                        if (hasText)
                        {
                            clipboardText = Clipboard.GetText();
                            // 避免重复处理同一内容
                            if (!string.IsNullOrEmpty(clipboardText) && clipboardText != lastClipboardText)
                            {
                                lastClipboardText = clipboardText;

                                // 检测并修正电话号码格式
                                string fixedText = clipboardText;
                                
                                if (CBM_PhoneNumber)
                                    fixedText = PhoneNumberFormatConversion(fixedText);
                                
                                if (CBM_AutoNumber)
                                    fixedText = AutoNumber(fixedText);

                                if (CBM_AutoStringReplacer)
                                    fixedText = new StringReplacer().Replace(fixedText);

                                if (fixedText != clipboardText)
                                {
                                    // 更新剪贴板
                                    Clipboard.SetText(fixedText);

                                    if (MessageReporting)
                                    {
                                        _controllerForm.Invoke(new Action(() =>
                                        {
                                            var msgForm = new MessageForm("None", "粘贴板监视器",
                                                $"已自动修正剪贴板中的内容\n\n{clipboardText}\n↓\n{fixedText}");
                                            msgForm.Show();
                                        }));
                                    }
                                    //new MessageForm("None", "粘贴板监视器", $"已自动修正剪贴板中的引号\n\n{clipboardText}\n↓\n{fixedText}").Show();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"剪贴板访问错误: {ex.Message}");
                        // 如果是权限问题，可以尝试重新获取剪贴板访问权
                        Thread.Sleep(2000);
                    }

                    Thread.Sleep(1000); // 每秒检查一次
                }
            });

            // 设置线程为 STA（单线程单元）
            clipboardThread.SetApartmentState(ApartmentState.STA);
            clipboardThread.IsBackground = true;
            clipboardThread.Start();
        }

        private static string PhoneNumberFormatConversion(string input)
        {
            // 如果输入为空或未启用电话号码监控，直接返回
            if (string.IsNullOrEmpty(input) || !CBM_PhoneNumber || CBM_PhoneNumber_Type.Count == 0 || string.IsNullOrEmpty(CBM_PhoneNumber_TargetType))
                return input;

            // 去除输入字符串两端的空白字符
            string trimmedInput = input.Trim();

            // 检查输入是否匹配任何支持的电话号码格式
            foreach (var formatPattern in CBM_PhoneNumber_Type.Values)
            {
                try
                {
                    // 使用正则表达式进行匹配
                    Regex regex = new Regex(formatPattern);
                    Match match = regex.Match(trimmedInput);

                    if (match.Success)
                    {
                        // 提取纯数字（去除所有非数字字符）
                        string digitsOnly = Regex.Replace(trimmedInput, @"[^\d]", "");

                        // 检查是否为有效的手机号（11位，以1开头，第二位在3-9之间）
                        if (digitsOnly.Length == 11 && digitsOnly[0] == '1' && digitsOnly[1] >= '3' && digitsOnly[1] <= '9')
                        {
                            // 根据目标正则表达式格式进行转换
                            return FormatAccordingToTargetPattern(digitsOnly, CBM_PhoneNumber_TargetType);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 记录正则表达式错误
                    Debug.WriteLine($"正则表达式错误: {formatPattern} - {ex.Message}");
                }
            }

            // 如果不匹配任何支持格式，返回原始输入
            return input;
        }

        private static string FormatAccordingToTargetPattern(string digitsOnly, string targetPattern)
        {
            try
            {
                // 根据目标正则表达式模式构建格式化字符串
                switch (targetPattern)
                {
                    case "^1[3-9]\\d{9}$": // 无分隔符：12345678901
                        return digitsOnly;

                    case "^1[3-9]\\d{2}\\s\\d{4}\\s\\d{4}$": // 空格分隔：123 4567 8901
                        return $"{digitsOnly.Substring(0, 3)} {digitsOnly.Substring(3, 4)} {digitsOnly.Substring(7, 4)}";

                    case "^1[3-9]\\d{2}-\\d{4}-\\d{4}$": // 连字符分隔：123-4567-8901
                        return $"{digitsOnly.Substring(0, 3)}-{digitsOnly.Substring(3, 4)}-{digitsOnly.Substring(7, 4)}";

                    case "^1[3-9]\\d{2}\\.\\d{4}\\.\\d{4}$": // 点号分隔：123.4567.8901
                        return $"{digitsOnly.Substring(0, 3)}.{digitsOnly.Substring(3, 4)}.{digitsOnly.Substring(7, 4)}";

                    case "^1[3-9]\\d{2}/\\d{4}/\\d{4}$": // 斜杠分隔：123/4567/8901
                        return $"{digitsOnly.Substring(0, 3)}/{digitsOnly.Substring(3, 4)}/{digitsOnly.Substring(7, 4)}";

                    case "^\\(\\d{3}\\)\\s?\\d{4}-\\d{4}$": // 括号格式：(123) 4567-8901 或 (123)4567-8901
                        return $"({digitsOnly.Substring(0, 3)}) {digitsOnly.Substring(3, 4)}-{digitsOnly.Substring(7, 4)}";

                    case "^\\+86\\s1[3-9]\\d{2}\\s\\d{4}\\s\\d{4}$": // 国际格式：+86 123 4567 8901
                        return $"+86 {digitsOnly.Substring(0, 3)} {digitsOnly.Substring(3, 4)} {digitsOnly.Substring(7, 4)}";

                    case "^1[3-9]\\d{2}_\\d{4}_\\d{4}$": // 下划线分隔：123_4567_8901
                        return $"{digitsOnly.Substring(0, 3)}_{digitsOnly.Substring(3, 4)}_{digitsOnly.Substring(7, 4)}";

                    default:
                        // 对于未知的目标格式，尝试解析模式并构建相应的格式
                        return ParseAndBuildCustomFormat(digitsOnly, targetPattern);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"目标格式转换错误: {targetPattern} - {ex.Message}");
                // 如果转换失败，返回原始数字
                return digitsOnly;
            }
        }

        private static string ParseAndBuildCustomFormat(string digitsOnly, string targetPattern)
        {
            try
            {
                // 使用正则表达式匹配目标模式中的数字部分和非数字部分
                var patternParts = new List<string>();
                var currentPart = new StringBuilder();
                bool inEscape = false;

                foreach (char c in targetPattern)
                {
                    if (c == '\\' && !inEscape)
                    {
                        inEscape = true;
                        continue;
                    }

                    if (inEscape)
                    {
                        currentPart.Append(c);
                        inEscape = false;
                    }
                    else if (c == 'd' || c == '[' || c == ']' || c == '^' || c == '$' || c == '(' || c == ')' || c == '+' || c == '*' || c == '?' || c == '{' || c == '}' || c == '|')
                    {
                        // 跳过正则表达式特殊字符
                        continue;
                    }
                    else if (char.IsDigit(c))
                    {
                        currentPart.Append(c);
                    }
                    else
                    {
                        // 非数字字符作为分隔符
                        if (currentPart.Length > 0)
                        {
                            patternParts.Add(currentPart.ToString());
                            currentPart.Clear();
                        }
                        patternParts.Add(c.ToString());
                    }
                }

                if (currentPart.Length > 0)
                {
                    patternParts.Add(currentPart.ToString());
                }

                // 构建格式化字符串
                var result = new StringBuilder();
                int digitIndex = 0;

                foreach (var part in patternParts)
                {
                    if (part.All(char.IsDigit))
                    {
                        // 数字部分，提取相应位数的数字
                        int digitCount = part.Length;
                        if (digitIndex + digitCount <= digitsOnly.Length)
                        {
                            result.Append(digitsOnly.Substring(digitIndex, digitCount));
                            digitIndex += digitCount;
                        }
                        else
                        {
                            // 数字不足，直接追加剩余数字
                            result.Append(digitsOnly.Substring(digitIndex));
                            break;
                        }
                    }
                    else
                    {
                        // 分隔符部分
                        result.Append(part);
                    }
                }

                // 如果还有剩余数字，追加到末尾
                if (digitIndex < digitsOnly.Length)
                {
                    result.Append(digitsOnly.Substring(digitIndex));
                }

                return result.ToString();
            }
            catch
            {
                // 如果解析失败，使用默认的空格分隔
                return $"{digitsOnly.Substring(0, 3)} {digitsOnly.Substring(3, 4)} {digitsOnly.Substring(7, 4)}";
            }
        }

        private static string AutoNumber(string input)
        {
            // 使用正则表达式匹配所有连续的数字
            return Regex.Replace(input, @"\d+", match =>
            {
                string number = match.Value;
                // 从右向左每4位插入一个逗号
                string formatted = Regex.Replace(number, @"(\d)(?=(\d{4})+$)", CBM_AutoNumber_Str);
                return formatted;
            });
        }
    }
}


// WMI 进程监视器
class WmiProcessMonitor : IDisposable
{
    private ManagementEventWatcher processStartWatcher;
    private string targetProcessName;
    private string[] AdditionalProcessName;
    private string DoWhat;

    public WmiProcessMonitor(string processName, string doWhat = "Exit", string[] additionalProcessName = null)
    {
        targetProcessName = processName.EndsWith(".exe") ? processName : processName + ".exe";
        AdditionalProcessName = additionalProcessName;
        DoWhat = doWhat;
        SetupWmiWatcher();
    }

    private void SetupWmiWatcher()
    {
        try
        {
            string queryString = $@"SELECT * FROM Win32_ProcessStartTrace 
                                  WHERE ProcessName='{targetProcessName}'";

            var query = new WqlEventQuery(queryString);
            processStartWatcher = new ManagementEventWatcher(query);
            processStartWatcher.EventArrived += ProcessStarted;
            processStartWatcher.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"WMI 监视器初始化失败: {ex.Message}");
        }
    }

    private void ProcessStarted(object sender, EventArrivedEventArgs e)
    {
        switch (DoWhat)
        {
            // 退出
            case "Exit":
                if (Application.MessageLoop) Application.Exit();
                else Environment.Exit(0);
                break;

            // 关闭
            case "KillProcess":
                KillProcess(targetProcessName);
                break;

            // Steam
            case "Steam":
                DateTime currentTime = DateTime.Now;

                if (Program.lastSteamStartTime.HasValue)
                {
                    TimeSpan timeDifference = currentTime - Program.lastSteamStartTime.Value;

                    if (timeDifference.TotalSeconds < 30)
                    {
                        using (MessageForm msgForm = new MessageForm(
                            "None",
                            "Steam异常",
                            "检测到您在短时间运行了两次Steam，\n是否是Steam出现了异常？\n\n" +
                            "是否需要尝试进行进程修复？"
                        ))
                        {
                            // 显示模态对话框，主程序会在此等待
                            DialogResult result = msgForm.ShowDialog();

                            // 检查用户选择
                            if (result == DialogResult.OK && msgForm.UserChoice == "Yes")
                            {
                                KillProcess(targetProcessName);
                                KillProcess(@"steamerrorreporter64.exe");
                                new MessageForm(
                                    "None",
                                    "Steam异常",
                                    "已完成进程修复，请您再次运行Steam尝试。"
                                ).ShowDialog();
                            }
                            // 如果点击"否"，什么都不做（窗口已关闭）
                        }

                        // 清空变量
                        Program.lastSteamStartTime = null;
                    }
                    else
                        // 更新记录的时间
                        Program.lastSteamStartTime = currentTime;
                }
                else
                    // 更新记录的时间
                    Program.lastSteamStartTime = currentTime;

                    
                break;

            // 测试
            case "Debug":
                MessageBox.Show($"侦测到{targetProcessName}启动");
                break;
        }
    }

    private void KillProcess(string ProcessName)
    {
        try
        {
            Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(ProcessName));
            string logs = string.Empty;

            if (processes.Length == 0)
                return;

            foreach (Process process in processes)
            {
                process.Kill();
                logs += $"成功强制关闭进程: {process.ProcessName} (ID: {process.Id})";
            }
            if (Program.MessageReporting)
                MessageBox.Show($"WMI监视器检查到订阅进程 {ProcessName}\n{logs}", "齐之防御", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            if (Program.MessageReporting)
                MessageBox.Show($"关闭进程时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public void Dispose()
    {
        processStartWatcher?.Stop();
        processStartWatcher?.Dispose();
    }
}


public class ConfigReader
{
    private readonly Dictionary<string, string> _config = new Dictionary<string, string>();

    public ConfigReader(string filePath)
    {
        foreach (var line in File.ReadAllLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#") || line.StartsWith(";"))
                continue;

            var parts = line.Split(new[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                _config[parts[0].Trim()] = parts[1].Trim();
            }
        }
    }

    public string GetString(string key) => _config.TryGetValue(key, out var value) ? value : null;
    public bool GetBool(string key) => bool.Parse(_config[key]);
    public int GetInt(string key) => int.Parse(_config[key]);
    public double GetDouble(string key) => double.Parse(_config[key]);
    public bool GetIsValueHave(string key) => _config.ContainsKey(key);
}

public class StringReplacer
{
    private readonly Dictionary<string, string> _replacements;

    public StringReplacer()
    {
        _replacements = new Dictionary<string, string>
        {
            { ">=", "≥" }, { "->", "→" },
            { "<=", "≤" }, { "<-", "←" },
            { "!=", "≠" }, { "^^", "↑" },
            { "&&", "∧" }, { "-^^", "↓" },
            { "||", "∨" }, { "==", "≡" },
            { "alpha", "α" }, { "beta", "β" },
            { "gamma", "γ" }, { "delta", "δ" },
            { "pi", "π" },

            { @"\a", "\a" }, { @"\b", "\b" }, { @"\f", "\f" },
            { @"\n", "\n" }, { @"\r", "\r" }, { @"\t", "\t" },
            { @"\v", "\v" }, { @"\\", "\\" }, { @"\0", "\0" },
            { @"\'", "\'" }, { "\\\"", "\"" }
        };
    }

    public StringReplacer(Dictionary<string, string> customReplacements)
    {
        _replacements = customReplacements;
    }

    public string Replace(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        StringBuilder sb = new StringBuilder(input);
        foreach (var replacement in _replacements)
        {
            sb.Replace(replacement.Key, replacement.Value);
        }
        return sb.ToString();
    }

    // 添加新的替换规则
    public void AddReplacement(string oldValue, string newValue)
    {
        _replacements[oldValue] = newValue;
    }
}
