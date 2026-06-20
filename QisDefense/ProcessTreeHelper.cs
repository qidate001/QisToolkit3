using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QisDefense
{
    public static class ProcessTreeHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_BASIC_INFORMATION
        {
            public IntPtr ExitStatus;
            public IntPtr PebBaseAddress;
            public IntPtr AffinityMask;
            public IntPtr BasePriority;
            public UIntPtr UniqueProcessId;
            public IntPtr InheritedFromUniqueProcessId;  // 父进程 PID
        }

        [DllImport("ntdll.dll")]
        static extern int NtQueryInformationProcess(
            IntPtr processHandle,
            int processInformationClass,  // 0 = ProcessBasicInformation
            out PROCESS_BASIC_INFORMATION processInformation,
            int processInformationLength,
            out int returnLength);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        static extern bool CloseHandle(IntPtr hObject);

        const uint PROCESS_QUERY_INFORMATION = 0x0400;
        const uint PROCESS_VM_READ = 0x0010;

        /// <summary>
        /// 获取进程的父进程 PID
        /// </summary>
        public static int GetParentProcessId(int pid)
        {
            IntPtr hProcess = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, pid);
            if (hProcess == IntPtr.Zero)
                return -1;

            try
            {
                PROCESS_BASIC_INFORMATION pbi;
                int result = NtQueryInformationProcess(hProcess, 0, out pbi, Marshal.SizeOf(typeof(PROCESS_BASIC_INFORMATION)), out _);

                if (result == 0)  // STATUS_SUCCESS
                    return (int)pbi.InheritedFromUniqueProcessId;
            }
            finally
            {
                CloseHandle(hProcess);
            }

            return -1;
        }

        /// <summary>
        /// 查找所有直接子进程（通过遍历所有进程比对其父 PID）
        /// </summary>
        public static List<int> FindChildProcessIds(int parentPid)
        {
            var children = new List<int>();

            foreach (var proc in Process.GetProcesses())
            {
                try
                {
                    int ppid = GetParentProcessId(proc.Id);
                    if (ppid == parentPid)
                    {
                        children.Add(proc.Id);
                    }
                }
                catch
                {
                    // 进程退出或拒绝访问（TrustedInstaller 下基本没有拒绝访问）
                }
            }

            return children;
        }

        /// <summary>
        /// 递归获取所有子孙进程 PID
        /// </summary>
        public static List<int> GetAllDescendantProcessIds(int rootPid)
        {
            var allDescendants = new List<int>();
            var queue = new Queue<int>();
            queue.Enqueue(rootPid);

            while (queue.Count > 0)
            {
                int currentPid = queue.Dequeue();
                var children = FindChildProcessIds(currentPid);

                foreach (int childPid in children)
                {
                    allDescendants.Add(childPid);
                    queue.Enqueue(childPid);  // 继续查找子进程的子进程
                }
            }

            return allDescendants;
        }
    }
}
