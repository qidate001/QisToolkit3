using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QisDefense
{
    public class JobKiller : IDisposable
    {
        private SafeFileHandle _jobHandle;

        // Job 信息结构体
        [StructLayout(LayoutKind.Sequential)]
        struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
        {
            public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
            // 后面字段省略，我们主要用 Basic
        }

        [StructLayout(LayoutKind.Sequential)]
        struct JOBOBJECT_BASIC_LIMIT_INFORMATION
        {
            public long PerProcessUserTimeLimit;
            public long PerJobUserTimeLimit;
            public uint LimitFlags;
            public UIntPtr MinimumWorkingSetSize;
            public UIntPtr MaximumWorkingSetSize;
            public uint ActiveProcessLimit;
            public long Affinity;
            public uint PriorityClass;
            public uint SchedulingClass;
        }

        const uint JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x2000;
        const int JobObjectExtendedLimitInformation = 9;

        [DllImport("kernel32.dll")]
        static extern SafeFileHandle CreateJobObject(IntPtr lpJobAttributes, string lpName);

        [DllImport("kernel32.dll")]
        static extern bool SetInformationJobObject(
            SafeFileHandle hJob,
            int JobObjectInfoClass,
            ref JOBOBJECT_EXTENDED_LIMIT_INFORMATION lpJobObjectInfo,
            uint cbJobObjectInfoLength);

        [DllImport("kernel32.dll")]
        static extern bool AssignProcessToJobObject(SafeFileHandle hJob, IntPtr hProcess);

        public JobKiller()
        {
            // 创建 Job Object（TrustedInstaller 下可以用全局名）
            _jobHandle = CreateJobObject(IntPtr.Zero, null);
            if (_jobHandle.IsInvalid)
                throw new Win32Exception();

            // 设置 KILL_ON_JOB_CLOSE：当句柄关闭时自动杀掉所有进程
            var info = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION();
            info.BasicLimitInformation.LimitFlags = JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE;

            SetInformationJobObject(
                _jobHandle,
                JobObjectExtendedLimitInformation,
                ref info,
                (uint)Marshal.SizeOf(typeof(JOBOBJECT_EXTENDED_LIMIT_INFORMATION))
            );
        }

        /// <summary>
        /// 将进程及其未来所有子进程加入 Job
        /// </summary>
        public bool AddProcess(int pid)
        {
            // TrustedInstaller + SeDebugPrivilege 下可以打开任何进程
            IntPtr hProcess = NativeMethods.OpenProcess(
                0x0400 | 0x0002,  // PROCESS_QUERY_INFORMATION | PROCESS_SET_INFORMATION
                false,
                pid
            );

            if (hProcess == IntPtr.Zero)
                return false;

            bool result = AssignProcessToJobObject(_jobHandle, hProcess);
            NativeMethods.CloseHandle(hProcess);
            return result;
        }

        /// <summary>
        /// 关闭 Job -> 清空里面所有进程（包括子进程）
        /// </summary>
        public void Dispose()
        {
            _jobHandle?.Dispose();  // 关闭句柄 = 触发 KILL_ON_JOB_CLOSE
        }
    }

    internal static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
    }
}
