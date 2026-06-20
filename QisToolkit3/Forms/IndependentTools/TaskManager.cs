using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace QisToolkit3.Forms
{
    public partial class TaskManager : Form
    {
        private Process[] processes;
        private string[] names;
        private RegistryKey registry = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace");

        // 添加 Win32 API 声明
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr GetTopWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        // 消息常量定义
        private const int WM_CLOSE = 0x0010;
        private const int WM_QUERYENDSESSION = 0x0011;
        private const int WM_ENDSESSION = 0x0016;
        private const int CTRL_SHUTDOWN_EVENT = 0x00000006; // 系统关机事件

        private const uint GW_HWNDNEXT = 2;

        public TaskManager()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);

            //comboBoxType.SelectedIndex = 0;
        }

        private void TaskManager_Load(object sender, EventArgs e)
        {
            LoadDatas();
        }

        private void LoadDatas()
        {
            // 获取 names
            try
            {
                processes = Process.GetProcesses();
                //names = processes.Select(p => p.ProcessName).ToArray();
                listBox.Items.Clear();
                foreach (Process process in processes)
                {
                    listBox.Items.Add($"{process.Id} : {process.ProcessName}");
                }
            }
            catch (Exception ex)
            {
                Log.Err($"[TaskManager] 数据获取失败，错误信息：{ex.Message}");
                MessageBox.Show(
                    $"数据获取失败！\n请您检查相关权限，联系开发者\n错误原因：{ex.Message}\n完整报错：\n{ex}",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enabled = listBox.SelectedItem != null;

            button_Kill.Enabled = enabled;
            button_JobKill.Enabled = enabled;
            button_WM_CLOSE.Enabled = enabled;
            button_WM_ENDSESSION.Enabled = enabled;
            button_WM_QUERYENDSESSION.Enabled = enabled;
            button_CTRL_SHUTDOWN_EVENT.Enabled = enabled;

            button_AddCriticalProcess.Enabled = enabled;
            button_RemoveCriticalProcess.Enabled = enabled;
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// 从选中的项中解析 PID
        /// </summary>
        private bool TryGetPidFromSelectedItem(object item, out int pid)
        {
            pid = 0;
            string itemText = item.ToString();
            string[] parts = itemText.Split(new[] { " : " }, StringSplitOptions.None);

            if (parts.Length != 2)
            {
                Log.Err($"[TaskManager] 无法解析项目：{itemText}");
                return false;
            }

            if (!int.TryParse(parts[0], out pid))
            {
                Log.Err($"[TaskManager] 无法解析 PID：{parts[0]}");
                return false;
            }

            return true;
        }


        private void button_WM_CLOSE_Click(object sender, EventArgs e)
        {
            foreach (object item in listBox.SelectedItems)
            {
                string itemText = item.ToString();
                try
                {
                    if (!TryGetPidFromSelectedItem(item, out int pid))
                        continue;

                    Process process = Process.GetProcessById(pid);

                    // 发送 WM_CLOSE 消息
                    if (!process.CloseMainWindow())
                    {
                        Log.Warn($"[TaskManager] 无法向进程 {itemText} 发送关闭消息");
                        // 可选：使用 Win32 API 发送 WM_CLOSE
                        // SendWM_CLOSE(process.MainWindowHandle);
                    }
                    else
                    {
                        Log.Info($"[TaskManager] 已向进程 {itemText} 发送关闭消息");
                    }
                }
                catch (ArgumentException)
                {
                    // 进程可能已经退出
                    Log.Warn($"[TaskManager] 进程已不存在：{itemText}");
                    MessageBox.Show($"进程 {itemText} 已经不存在了", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Log.Err($"执行出现错误，信息：{ex.Message}");
                    MessageBox.Show($"执行出现错误，请联系开发者\n错误原因：{ex.Message}\n\n完整报错：\n{ex}", "报错");
                }
            }

            // 刷新列表（可选）
            LoadDatas();
        }

        /// <summary>
        /// 发送消息到进程的所有窗口
        /// </summary>
        private void SendMessageToAllWindows(Process process, int msg, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                // 发送到主窗口
                if (process.MainWindowHandle != IntPtr.Zero)
                {
                    SendMessage(process.MainWindowHandle, msg, wParam, lParam);
                    Log.Info($"[TaskManager] 已发送消息 0x{msg:X4} 到主窗口 (PID: {process.Id})");
                }

                // 发送到所有子窗口（有些应用可能有多个窗口）
                foreach (ProcessThread thread in process.Threads)
                {
                    // 这里可以通过 EnumThreadWindows 枚举线程的所有窗口
                    // 简化版：只处理主窗口
                }
            }
            catch (Exception ex)
            {
                Log.Err($"发送消息失败：{ex.Message}");
            }
        }

        private void button_WM_QUERYENDSESSION_Click(object sender, EventArgs e)
        {
            foreach (object item in listBox.SelectedItems)
            {
                try
                {
                    if (!TryGetPidFromSelectedItem(item, out int pid))
                        continue;

                    Process process = Process.GetProcessById(pid);

                    // WM_QUERYENDSESSION: 询问进程是否可以结束会话
                    // wParam = 0, lParam = 0 表示系统正在关机
                    IntPtr result = (IntPtr)SendMessage(process.MainWindowHandle, WM_QUERYENDSESSION, IntPtr.Zero, IntPtr.Zero);

                    if (result.ToInt32() != 0)
                    {
                        Log.Info($"[TaskManager] 进程 {pid} 同意结束会话");
                        MessageBox.Show($"进程 {item} 同意结束会话", "查询结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        Log.Warn($"[TaskManager] 进程 {pid} 拒绝结束会话");
                        MessageBox.Show($"进程 {item} 拒绝结束会话", "查询结果", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (ArgumentException)
                {
                    Log.Warn($"[TaskManager] 进程已不存在：{item}");
                }
                catch (Exception ex)
                {
                    Log.Err($"执行出现错误，信息：{ex.Message}");
                    MessageBox.Show($"执行出现错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_WM_ENDSESSION_Click(object sender, EventArgs e)
        {
            foreach (object item in listBox.SelectedItems)
            {
                try
                {
                    if (!TryGetPidFromSelectedItem(item, out int pid))
                        continue;

                    Process process = Process.GetProcessById(pid);

                    // WM_ENDSESSION: 通知进程会话结束（系统正在关机）
                    // wParam = 1 表示会话结束，lParam = 0
                    SendMessage(process.MainWindowHandle, WM_ENDSESSION, (IntPtr)1, IntPtr.Zero);
                    Log.Info($"[TaskManager] 已发送 WM_ENDSESSION 到进程 {pid}");

                    // 等待进程退出（可选）
                    if (process.WaitForExit(3000))
                    {
                        Log.Info($"[TaskManager] 进程 {pid} 已退出");
                    }
                    else
                    {
                        Log.Warn($"[TaskManager] 进程 {pid} 未在3秒内退出");
                    }
                }
                catch (ArgumentException)
                {
                    Log.Warn($"[TaskManager] 进程已不存在：{item}");
                }
                catch (Exception ex)
                {
                    Log.Err($"执行出现错误，信息：{ex.Message}");
                    MessageBox.Show($"执行出现错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // 刷新列表
            LoadDatas();
        }

        private void button_CTRL_SHUTDOWN_EVENT_Click(object sender, EventArgs e)
        {
            // CTRL_SHUTDOWN_EVENT 是控制台事件，主要用于控制台程序
            // 对于普通 GUI 程序，这个方法效果有限

            foreach (object item in listBox.SelectedItems)
            {
                try
                {
                    if (!TryGetPidFromSelectedItem(item, out int pid))
                        continue;

                    Process process = Process.GetProcessById(pid);

                    // 方法1: 如果是控制台程序，可以发送 Ctrl+C 或 Ctrl+Break
                    // 但 .NET 没有直接的方法发送控制台事件到其他进程

                    // 方法2: 使用 GenerateConsoleCtrlEvent API
                    // 注意：这只能用于同一个控制台会话的进程
                    // 对于独立的 GUI 程序，这个方法无效

                    // 方法3: 发送 WM_CLOSE 作为替代（等同于点击关闭按钮）
                    // 因为 CTRL_SHUTDOWN_EVENT 在实际应用中很少直接使用

                    Log.Warn($"[TaskManager] CTRL_SHUTDOWN_EVENT 对 GUI 程序效果有限，使用 WM_CLOSE 替代");

                    // 使用 WM_CLOSE 作为替代方案
                    if (!process.CloseMainWindow())
                    {
                        Log.Warn($"[TaskManager] 无法向进程 {item} 发送关闭消息");
                    }
                    else
                    {
                        Log.Info($"[TaskManager] 已向进程 {item} 发送 WM_CLOSE 消息");
                    }
                }
                catch (ArgumentException)
                {
                    Log.Warn($"[TaskManager] 进程已不存在：{item}");
                }
                catch (Exception ex)
                {
                    Log.Err($"执行出现错误，信息：{ex.Message}");
                    MessageBox.Show($"执行出现错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_Kill_Click(object sender, EventArgs e)
        {
            if (!PipeClient.CheckServiceRunning()) return;

            foreach (object item in listBox.SelectedItems)
            {
                try
                {
                    string itemText = item.ToString();
                    if (!TryGetPidFromSelectedItem(item, out int pid))
                        continue;

                    bool ok = PipeClient.KillProcess(pid);

                    if (ok)
                    {
                        Log.Info($"[TaskManager] 向齐之防御请求杀死进程成功，进程：{itemText}");
                    }
                    else
                    {
                        Log.Info($"[TaskManager] 向齐之防御请求杀死进程失败，进程：{itemText}");
                    }
                }
                catch (ArgumentException)
                {
                    Log.Warn($"[TaskManager] 进程已不存在：{item}");
                }
                catch (Exception ex)
                {
                    Log.Err($"执行出现错误，信息：{ex.Message}");
                    MessageBox.Show($"执行出现错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            MessageBox.Show("操作完成！", "齐之防御", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDatas();
        }

        private void button_JobKill_Click(object sender, EventArgs e)
        {
            if (!PipeClient.CheckServiceRunning()) return;

            foreach (object item in listBox.SelectedItems)
            {
                try
                {
                    string itemText = item.ToString();
                    if (!TryGetPidFromSelectedItem(item, out int pid))
                        continue;

                    bool ok = PipeClient.JobKillProcess(pid);

                    if (ok)
                    {
                        Log.Info($"[TaskManager] 向齐之防御请求杀死进程树成功，进程：{itemText}");
                    }
                    else
                    {
                        Log.Info($"[TaskManager] 向齐之防御请求杀死进程树失败，进程：{itemText}");
                    }
                }
                catch (ArgumentException)
                {
                    Log.Warn($"[TaskManager] 进程已不存在：{item}");
                }
                catch (Exception ex)
                {
                    Log.Err($"执行出现错误，信息：{ex.Message}");
                    MessageBox.Show($"执行出现错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            LoadDatas();
            MessageBox.Show("操作完成！", "齐之防御", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_AddCriticalProcess_Click(object sender, EventArgs e)
        {

            if (!PipeClient.CheckServiceRunning()) return;

            foreach (object item in listBox.SelectedItems)
            {
                try
                {
                    string itemText = item.ToString();
                    if (!TryGetPidFromSelectedItem(item, out int pid))
                        continue;

                    bool ok = PipeClient.AddCriticalProcess(pid);

                    if (ok)
                    {
                        Log.Info($"[TaskManager] 向齐之防御请求添加关键进程成功，进程：{itemText}");
                    }
                    else
                    {
                        Log.Info($"[TaskManager] 向齐之防御请求添加关键进程失败，进程：{itemText}");
                    }
                }
                catch (ArgumentException)
                {
                    Log.Warn($"[TaskManager] 进程已不存在：{item}");
                }
                catch (Exception ex)
                {
                    Log.Err($"执行出现错误，信息：{ex.Message}");
                    MessageBox.Show($"执行出现错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            MessageBox.Show("操作完成！", "齐之防御", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_RemoveCriticalProcess_Click(object sender, EventArgs e)
        {

            if (!PipeClient.CheckServiceRunning()) return;

            foreach (object item in listBox.SelectedItems)
            {
                try
                {
                    string itemText = item.ToString();
                    if (!TryGetPidFromSelectedItem(item, out int pid))
                        continue;

                    bool ok = PipeClient.RemoveCriticalProcess(pid);

                    if (ok)
                    {
                        Log.Info($"[TaskManager] 向齐之防御请求移除关键进程成功，进程：{itemText}");
                    }
                    else
                    {
                        Log.Info($"[TaskManager] 向齐之防御请求移除关键进程失败，进程：{itemText}");
                    }
                }
                catch (ArgumentException)
                {
                    Log.Warn($"[TaskManager] 进程已不存在：{item}");
                }
                catch (Exception ex)
                {
                    Log.Err($"执行出现错误，信息：{ex.Message}");
                    MessageBox.Show($"执行出现错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            MessageBox.Show("操作完成！", "齐之防御", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
