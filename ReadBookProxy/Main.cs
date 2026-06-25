using EasyHook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ReadBookProxy
{
    using System;
    using System.IO;
    using System.Text;
    using System.Runtime.InteropServices;
    using EasyHook;

    public class Main : IEntryPoint
    {
        public Main(RemoteHooking.IContext context) { }

        public void Run(RemoteHooking.IContext context)
        {
            try
            {
                // 获取 CreateFileW 的函数指针
                IntPtr pCreateFile = LocalHook.GetProcAddress("kernel32.dll", "CreateFileW");

                // 创建钩子
                LocalHook hook = LocalHook.Create(
                    pCreateFile,
                    new CreateFileDelegate(CreateFile_Hook),
                    this
                );

                // 设置钩子线程权限（允许在所有线程中生效）
                hook.ThreadACL.SetExclusiveACL(new int[0]);

                // 保持进程存活
                RemoteHooking.WakeUpProcess();
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                // 输出错误信息（可通过 DebugView 查看）
                System.Diagnostics.Debug.WriteLine("Hook error: " + ex.Message);
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
            // ========== 在此处编写你的文件拦截逻辑 ==========
            // 例：如果文件名以 .txt 结尾，且为读取操作，则进行转换

            if (dwDesiredAccess == 0x80000000 && lpFileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    if (File.Exists(lpFileName))
                    {
                        byte[] content = File.ReadAllBytes(lpFileName);
                        if (IsUtf8Encoding(content))
                        {
                            // 转换为 ANSI（GBK）
                            string text = Encoding.UTF8.GetString(content);
                            string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
                            File.WriteAllText(tempFile, text, Encoding.GetEncoding("GBK"));
                            lpFileName = tempFile;  // 重定向到临时文件
                        }
                    }
                }
                catch { /* 转换失败则忽略，继续执行原文件打开 */ }
            }

            // 调用原始 API
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

        // 简单 UTF-8 检测（含 BOM 检测）
        private static bool IsUtf8Encoding(byte[] bytes)
        {
            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
                return true;

            // 没有 BOM 时，采用宽松检测：尝试解码，看是否包含非 ASCII 且无异常
            try
            {
                string result = Encoding.UTF8.GetString(bytes);
                // 如果文本包含高位字符且解码后无明显替换字符（�），可视为 UTF-8
                return result.Contains('�') == false;
            }
            catch
            {
                return false;
            }
        }

        // 声明原始 API 函数指针（用于调用原函数）
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
