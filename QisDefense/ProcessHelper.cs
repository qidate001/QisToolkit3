using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace QisDefense
{
    public static class ProcessHelper
    {
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetInformationProcess(
            IntPtr hProcess,
            int ProcessInformationClass,
            ref int ProcessInformation,
            int ProcessInformationLength
        );

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtQueryInformationProcess(
            IntPtr hProcess,
            int ProcessInformationClass,
            ref int ProcessInformation,
            int ProcessInformationLength,
            out int ReturnLength
        );

        private const int ProcessBreakOnTermination = 0x1D;

        /// <summary>
        /// 给指定进程添加关键标记（杀进程会蓝屏）
        /// </summary>
        public static bool AddCriticalProcess(int processId)
        {
            try
            {
                using (var process = Process.GetProcessById(processId))
                {
                    int flag = 1;
                    int result = NtSetInformationProcess(
                        process.Handle,
                        ProcessBreakOnTermination,
                        ref flag,
                        sizeof(int)
                    );
                    return result == 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 移除进程的关键标记（正常退出）
        /// </summary>
        public static bool RemoveCriticalProcess(int processId)
        {
            try
            {
                using (var process = Process.GetProcessById(processId))
                {
                    int flag = 0;
                    int result = NtSetInformationProcess(
                        process.Handle,
                        ProcessBreakOnTermination,
                        ref flag,
                        sizeof(int)
                    );
                    return result == 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 检查进程是否为关键进程
        /// </summary>
        public static bool IsCriticalProcess(int processId)
        {
            try
            {
                using (var process = Process.GetProcessById(processId))
                {
                    int flag = 0;
                    int result = NtQueryInformationProcess(
                        process.Handle,
                        ProcessBreakOnTermination,
                        ref flag,
                        sizeof(int),
                        out _
                    );
                    return result == 0 && flag == 1;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
