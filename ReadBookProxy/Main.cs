using EasyHook;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ReadBookProxy
{
    [Serializable]  // 重要：必须添加此特性
    public class HookEntry : IEntryPoint
    {
        private string _channelName;

        // 构造函数：可以接收多个参数
        public HookEntry(RemoteHooking.IContext context, string channelName)
        {
            _channelName = channelName;
            // 可以在这里初始化一些资源
        }

        // Run 方法：标准签名
        public void Run(RemoteHooking.IContext context, string channelName)
        {
            try
            {
                // 挂钩 CreateFileW
                IntPtr pCreateFile = LocalHook.GetProcAddress("kernel32.dll", "CreateFileW");
                LocalHook hook = LocalHook.Create(
                    pCreateFile,
                    new CreateFileDelegate(CreateFile_Hook),
                    this
                );

                // 允许在所有线程中生效
                hook.ThreadACL.SetExclusiveACL(new int[0]);

                // 通知注入器注入成功
                RemoteHooking.WakeUpProcess();

                // 保持线程运行（防止 GC 回收 hook 对象）
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Hook error: " + ex.Message);
                throw;
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Winapi, SetLastError = true)]
        private delegate IntPtr CreateFileDelegate(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );

        private static IntPtr CreateFile_Hook(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile)
        {
            // 你的钩子逻辑
            if (dwDesiredAccess == 0x80000000 &&
                lpFileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    if (File.Exists(lpFileName))
                    {
                        byte[] content = File.ReadAllBytes(lpFileName);

                        if (IsUtf8Encoding(content))
                        {
                            string text = Encoding.UTF8.GetString(content);
                            string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
                            File.WriteAllText(tempFile, text, Encoding.GetEncoding("GBK"));
                            lpFileName = tempFile;
                        }
                    }
                }
                catch
                {
                    // 转换失败则忽略
                }
            }

            return CreateFileW(
                lpFileName,
                dwDesiredAccess,
                dwShareMode,
                lpSecurityAttributes,
                dwCreationDisposition,
                dwFlagsAndAttributes,
                hTemplateFile
            );
        }

        private static bool IsUtf8Encoding(byte[] bytes)
        {
            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
                return true;

            try
            {
                string result = Encoding.UTF8.GetString(bytes);
                return !result.Contains('�');
            }
            catch
            {
                return false;
            }
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
    }
}