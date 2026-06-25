using EasyHook;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace ReadBookProxy
{
    [Serializable]
    public class HookEntry : IEntryPoint
    {
        private static readonly string LogPath = Path.Combine(Path.GetTempPath(), "ReadBookProxy.log");

        // 记录目标程序的基础目录，用于精准拼接相对路径
        private static string AppBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public HookEntry(RemoteHooking.IContext context, string channelName) { }

        public void Run(RemoteHooking.IContext context, string channelName)
        {
            try
            {
                File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss.fff} [启动] 开始安装钩子...\n");

                // 获取当前被注入的进程信息，核对到底是谁被注入了
                string currentProcName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss.fff} [真实注入目标] 当前受害者进程名: {currentProcName}\n");

                // ===== 强力获取 kernelbase.dll 或 kernel32.dll 的真实 API 地址 =====
                // 很多时候 kernel32.dll 的 CreateFile 只是个转发壳，挂在 kernelbase.dll 上更稳
                IntPtr pCreateFileW = LocalHook.GetProcAddress("kernelbase.dll", "CreateFileW");
                if (pCreateFileW == IntPtr.Zero) pCreateFileW = LocalHook.GetProcAddress("kernel32.dll", "CreateFileW");

                IntPtr pCreateFileA = LocalHook.GetProcAddress("kernelbase.dll", "CreateFileA");
                if (pCreateFileA == IntPtr.Zero) pCreateFileA = LocalHook.GetProcAddress("kernel32.dll", "CreateFileA");

                File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss.fff} [地址] CreateFileW:{pCreateFileW}, CreateFileA:{pCreateFileA}\n");

                // ===== 安装 W 钩子 =====
                LocalHook hookW = LocalHook.Create(pCreateFileW, new CreateFileWDelegate(CreateFileW_Hook), this);
                // 关键改动：这里传入 0 代表对当前进程的所有线程（包括主线程、新线程）全部生效！
                hookW.ThreadACL.SetInclusiveACL(new int[] { 0 });

                // ===== 安装 A 钩子 =====
                LocalHook hookA = LocalHook.Create(pCreateFileA, new CreateFileADelegate(CreateFileA_Hook), this);
                hookA.ThreadACL.SetInclusiveACL(new int[] { 0 });

                File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss.fff} [启动] 强力钩子全部全局激活完成\n");

                // 解除目标进程暂停状态
                RemoteHooking.WakeUpProcess();

                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss.fff} [注入内部崩溃] {ex.Message}\n{ex.StackTrace}\n");
            }
        }

        // ================== CreateFileW 钩子 ==================
        [UnmanagedFunctionPointer(CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Unicode)]
        private delegate IntPtr CreateFileWDelegate(
            [MarshalAs(UnmanagedType.LPWStr)] string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );

        private static IntPtr CreateFileW_Hook(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile)
        {
            try
            {
                string newPath = ProcessFileOpen(lpFileName, dwDesiredAccess, "W");
                if (newPath != lpFileName)
                {
                    return CreateFileW(newPath, dwDesiredAccess, dwShareMode, lpSecurityAttributes, dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss.fff} [CreateFileW异常] {ex.Message}\n");
            }

            return CreateFileW(lpFileName, dwDesiredAccess, dwShareMode, lpSecurityAttributes, dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr CreateFileW(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );

        // ================== CreateFileA 钩子 ==================
        [UnmanagedFunctionPointer(CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Ansi)]
        private delegate IntPtr CreateFileADelegate(
            [MarshalAs(UnmanagedType.LPStr)] string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );

        private static IntPtr CreateFileA_Hook(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile)
        {
            try
            {
                string newPath = ProcessFileOpen(lpFileName, dwDesiredAccess, "A");
                if (newPath != lpFileName)
                {
                    // 注意：由于重定向生成的是绝对路径(Temp下的UTF-8/GBK新文件)，需要确保以 ANSI 传回
                    return CreateFileA(newPath, dwDesiredAccess, dwShareMode, lpSecurityAttributes, dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss.fff} [CreateFileA异常] {ex.Message}\n");
            }

            return CreateFileA(lpFileName, dwDesiredAccess, dwShareMode, lpSecurityAttributes, dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr CreateFileA(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );

        // ================== 核心公共处理逻辑 ==================
        private static string ProcessFileOpen(string lpFileName, uint dwDesiredAccess, string apiType)
        {
            if (string.IsNullOrEmpty(lpFileName)) return lpFileName;

            // 1. 放宽权限判定，包含读或者为0都算
            bool isRead = (dwDesiredAccess & 0x80000000) != 0 || dwDesiredAccess == 0;

            if (isRead && lpFileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss.fff} [命中{apiType}] 捕获到目标TXT: {lpFileName}\n");

                // 2. 智能化路径解析
                string realFilesystemPath = null;

                if (Path.IsPathRooted(lpFileName))
                {
                    // 如果传过来本身就是绝对路径，直接用
                    realFilesystemPath = lpFileName;
                }
                else
                {
                    // 如果是相对路径，提取出纯文件名
                    string pureFileName = Path.GetFileName(lpFileName);

                    // 穷举目标可能存在的位置
                    string[] guestPaths = new string[]
                    {
                        // 盲盒1：从你刚才图片里抓到的真实文档目录下找
                        Path.Combine(@"D:\UserDir\Documents", pureFileName),
                        // 盲盒2：系统的“我的文档”
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), pureFileName),
                        // 盲盒3：当前程序运行目录
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pureFileName),
                        // 盲盒4：当前工作目录
                        Path.Combine(Environment.CurrentDirectory, pureFileName)
                    }; 

                    foreach (var path in guestPaths)
                    {
                        if (File.Exists(path))
                        {
                            realFilesystemPath = path;
                            File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss.fff} [路径探测成功] 在此处找到真实文件: {realFilesystemPath}\n");
                            break;
                        }
                    }

                    // 【绝杀】如果全盘皆输，四个盲盒都没找到，说明是编码乱码导致的 File.Exists 失败
                    if (realFilesystemPath == null)
                    {
                        // 强制指定去你图片上的 D:\UserDir\Documents 目录下撞运气，不再用 File.Exists 拦截它
                        realFilesystemPath = Path.Combine(@"D:\UserDir\Documents", pureFileName);
                        File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss.fff} [路径兜底] 没找到物理文件，盲猜绝对路径为: {realFilesystemPath}\n");
                    }
                }

                // 3. 拦截、读取、转码、重定向
                try
                {
                    // 注意：如果文件确实存在，我们就读它。如果因为乱码读不到，我们打印异常
                    if (File.Exists(realFilesystemPath))
                    {
                        byte[] content = File.ReadAllBytes(realFilesystemPath);

                        if (IsUtf8Encoding(content))
                        {
                            File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss.fff} [开始转换] 检测到 UTF-8，正在转 GBK...\n");

                            string text = Encoding.UTF8.GetString(content);
                            string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".txt");

                            // 转成老程序认识的 GBK
                            File.WriteAllText(tempFile, text, Encoding.GetEncoding("GBK"));

                            File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss.fff} [重定向成功] 假文件已生成: {tempFile}\n");
                            return tempFile;
                        }
                        else
                        {
                            File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss.fff} [跳过] 文件本来就是 ANSI/GBK，无需转换\n");
                        }
                    }
                    else
                    {
                        File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss.fff} [致命] 即使兜底也无法在磁盘上定位该文件，请检查 D:\\UserDir\\Documents\\ 下是否存在该文件\n");
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss.fff} [转换异常] {ex.Message}\n");
                }
            }

            return lpFileName;
        }

        private static bool IsUtf8Encoding(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return false;

            // 检查 BOM (EF BB BF)
            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
                return true;

            // 无 BOM 高效流式检查 UTF-8 合法性
            int i = 0;
            while (i < bytes.Length)
            {
                if (bytes[i] <= 0x7F) { i++; continue; }
                if (bytes[i] >= 0xC2 && bytes[i] <= 0xDF && i + 1 < bytes.Length && bytes[i + 1] >= 0x80 && bytes[i + 1] <= 0xBF) { i += 2; continue; }
                if (bytes[i] >= 0xE0 && bytes[i] <= 0xEF && i + 2 < bytes.Length && bytes[i + 1] >= 0x80 && bytes[i + 1] <= 0xBF && bytes[i + 2] >= 0x80 && bytes[i + 2] <= 0xBF) { i += 3; continue; }
                if (bytes[i] >= 0xF0 && bytes[i] <= 0xF4 && i + 3 < bytes.Length && bytes[i + 1] >= 0x80 && bytes[i + 1] <= 0xBF && bytes[i + 2] >= 0x80 && bytes[i + 2] <= 0xBF && bytes[i + 3] >= 0x80 && bytes[i + 3] <= 0xBF) { i += 4; continue; }
                return false;
            }
            return true;
        }
    }
}