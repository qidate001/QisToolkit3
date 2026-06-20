using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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

        /// <summary>
        /// 检查进程是否为关键进程
        /// </summary>
        public static bool KillProcess(int processId)
        {
            try
            {
                using (var process = Process.GetProcessById(processId))
                {
                    process.Kill();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 把整个进程树（含保护进程）拉进 Job 然后全杀
        /// </summary>
        public static bool KillProcessTree(int rootPid)
        {
            try
            {
                using (var killer = new JobKiller())
                {
                    // 先加父进程
                    killer.AddProcess(rootPid);

                    // 递归加所有子进程（用上面任意一种实现）
                    foreach (int childPid in ProcessTreeHelper.FindChildProcessIds(rootPid))
                    {
                        AddProcessTree(childPid, killer);
                    }

                } // using 结束 -> 全部清理
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void AddProcessTree(int pid, JobKiller killer)
        {
            killer.AddProcess(pid);

            // 查找子进程（TrustedInstaller 能看到所有进程）
            foreach (var child in ProcessTreeHelper.FindChildProcessIds(pid))
            {
                AddProcessTree(child, killer);
            }
        }
    }
}
