using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace QisToolkit3
{
    public enum MinSudoLevel
    {
        Standard = 0,
        System = 1,
        TrustedInstaller = 2
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MinSudoResult
    {
        [MarshalAs(UnmanagedType.Bool)]
        public bool Success;
        public uint ExitCode;
        public uint ErrorCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ErrorMessage;
    }

    public static class MinSudo
    {
        [DllImport("MinSudoDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern int MinSudoRun(
            string commandLine,
            MinSudoLevel level,
            bool privileged,
            string workingDirectory,
            out MinSudoResult result);

        public static bool RunElevated(
            string commandLine,
            MinSudoLevel level,
            bool privileged,
            string workingDirectory,
            out uint exitCode,
            out string errorMessage
        )
        {
            exitCode = 0;
            errorMessage = null;

            try
            {
                if (string.IsNullOrEmpty(commandLine))
                    commandLine = "cmd.exe";

                MinSudoResult result;
                int hr = MinSudoRun(commandLine, level, privileged, workingDirectory, out result);

                if (hr >= 0 && result.Success)
                {
                    exitCode = result.ExitCode;
                    return true;
                }
                else
                {
                    errorMessage = result.ErrorMessage ?? $"HRESULT: 0x{hr:X8}";
                    return false;
                }
            }
            catch (SEHException)
            {
                // DLL 内部清理时抛异常，但进程已启动，忽略
                // 尝试获取进程退出码（如果可能）
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"异常: {ex.Message}";
                return false;
            }
        }
    }
}
