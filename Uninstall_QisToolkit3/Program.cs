using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uninstall_QisToolkit3
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (IsProcessRunning("QisDefense"))
            {
                MessageBox.Show(
                    "齐之防御（QisDefense.exe） 正在运行！\n" +
                    "请先按退出Ctrl+Alt+Esc退出齐之防御后再执行卸载。",
                    "齐之防御正常运行",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop
                );
                return; // 或者 Environment.Exit(0);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        /// <summary>
        /// 检查指定名称的进程是否正在运行
        /// </summary>
        /// <param name="processName">进程名称（不含 .exe 后缀）</param>
        /// <returns>正在运行返回 true，否则返回 false</returns>
        static bool IsProcessRunning(string processName)
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(processName);
                return processes.Length > 0;
            }
            catch
            {
                // 如果访问被拒绝或其他异常，返回 false
                return false;
            }
        }
    }
}
