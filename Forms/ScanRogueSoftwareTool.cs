using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using static Qi;
using static Qi.QisToolkit3_Datas;

namespace QisToolkit3.Forms
{
    public partial class ScanRogueSoftwareTool : Form
    {
        private int[] IdList = new int[1024];
        private int lid = 0;

        private static string userName = Environment.UserName;
        private static string LocalDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static string RoamingDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string LocalLowDir = @"C:\Users\" + Environment.UserName + @"\AppData\LocalLow";

        //Dictionary<int, Action> actionMap = new Dictionary<int, Action>
        //{
        //    { 0001, Del360Universal}
        //};

        //// 用于存储任务列表
        //private List<Action> tasks = new List<Action>();

        //// 当前执行的任务索引
        //private int currentTaskIndex = 0;

        private EC[] ecs = {
            // 360 系列
            new("360 安全卫士", "360 安全卫士，我们难以强行删除，最好您先手动卸载，再进行删除", null, [@"C:\Program Files (x86)\360\360Safe", $@"{RoamingDir}\360Safe", $@"{LocalLowDir}\360WD"]),
            new("360 浏览器", "360 浏览器，我们难以强行删除，最好您先手动卸载，再进行删除", null, [$@"{RoamingDir}\360browser"]),
            new("360 安全浏览器", "360 浏览器，我们难以强行删除，最好您先手动卸载，再进行删除", null, [$@"{RoamingDir}\360se6"]),
            new("360 桌面精简版", "难崩", null, [$@"{RoamingDir}\360DesktopLite"]),
            new("360 游戏5", "难崩", null, [$@"{RoamingDir}\360Game5"]),
            new("360 壁纸", "难崩", null, [$@"{RoamingDir}\360huabao"]),
            new("360 登录", "难崩", null, [$@"{RoamingDir}\360Login"]),
            new("360 通用模块", "这些模块总是会作为残留留下……", null, [@"C:\360Downloads", @"C:\Program Files (x86)\360"]),
            new("Windows 重装大师", "98元才能进系统？招笑", null, [@"C:\WindowSysReset", @"C:\WindowSysResetData"]),
        };
        


        public ScanRogueSoftwareTool()
        {
            InitializeComponent();
            //InitializeTasks();

            // 初始化
            Qi.FormInitDo(this.Text);

            if (!isSystem)
                MessageBox.Show(
                    "您当前并非系统级权限使用此工具！\n推荐用系统级权限使用此工具，\n否则部分内容难以删除！", 
                    "警告", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information
                );
        }

        //private void btnStart_Click(object sender, EventArgs e)
        //{
        //    if (!backgroundWorker.IsBusy)
        //    {
        //        // 重置状态
        //        currentTaskIndex = 0;
        //        progressBar.Value = 0;
        //        lblStatus.Text = "准备开始任务...";
        //        lblTask.Text = "";

        //        // 开始执行
        //        backgroundWorker.RunWorkerAsync();
        //    }
        //}

        /*
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
                btnCancel.Enabled = false;
                lblStatus.Text = "正在取消任务...";
            }
        }
        */

        /*
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;

            // 总任务数
            int totalTasks = tasks.Count;

            for (int i = 0; i < totalTasks; i++)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                currentTaskIndex = i;

                // 报告当前任务进度 (0表示任务开始)
                worker.ReportProgress(0, $"开始执行任务 {i + 1}/{totalTasks}");

                // 执行任务
                try
                {
                    tasks[i].Invoke();
                }
                catch (Exception ex)
                {
                    worker.ReportProgress(0, $"任务 {i + 1} 执行失败: {ex.Message}");
                    // 可以选择继续执行后续任务或中断
                    continue;
                }

                // 计算整体进度
                int overallProgress = (int)((i + 1) * 100.0 / totalTasks);

                // 报告进度
                worker.ReportProgress(overallProgress, $"任务 {i + 1} 完成");

                // 模拟任务间隔
                Thread.Sleep(500);
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // 更新进度条
            if (e.ProgressPercentage > progressBar.Value)
            {
                progressBar.Value = e.ProgressPercentage;
            }

            // 更新状态标签
            if (e.UserState != null)
            {
                lblStatus.Text = e.UserState.ToString();
            }

            // 更新当前任务信息
            lblTask.Text = $"当前任务: {currentTaskIndex + 1}/{tasks.Count}";
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                lblStatus.Text = "任务已取消";
            }
            else if (e.Error != null)
            {
                lblStatus.Text = $"发生错误: {e.Error.Message}";
            }
            else
            {
                lblStatus.Text = "所有任务已完成!";
                progressBar.Value = 100;
            }
        }
        */

        private void buttonScan_Click(object sender, EventArgs e)
        {
            // 重置数据
            IdList = new int[1024];
            checkedListBox.Items.Clear();

            // ID计数器（id负责总id，lid负责内列表）
            int id = 0;
            foreach (EC ec in ecs)
            {
                bool have = false;

                if (ec.FilePaths != null)
                    have = ScanFile(ec) || have ? true : false;

                if (ec.DirPaths != null)
                    have = ScanDir(ec) || have ? true : false;

                // 若软件存在，则添加至内外列表
                if (have)
                {
                    IdList[lid] = id; lid++;
                    checkedListBox.Items.Add(ec.Text, true);
                }

                ++id;
            }

            if (checkedListBox.Items.Count <= 0)
                MessageBox.Show("恭喜！您的电脑没有流氓软件！", "提示");

            else
                buttonDelete.Enabled = true;

            static bool ScanFile(EC ec)
            {
                foreach (string path in ec.FilePaths)
                {
                    if (File.Exists(path))
                        return true;
                }
                return false;
            }

            static bool ScanDir(EC ec)
            {
                foreach (string path in ec.DirPaths)
                {
                    if (Directory.Exists(path))
                        return true;
                }
                return false;
            }
        }

        private async void buttonDelete_Click(object sender, EventArgs e)
        {
            // 禁用按钮防止重复点击
            buttonDelete.Enabled = false;

            try
            {
                // 在后台线程执行删除操作
                await Task.Run(() =>
                {
                    foreach (int id in IdList.Take(lid))
                    {
                        if (checkedListBox.CheckedItems.Contains(ecs[id].Text))
                        {
                            if (ecs[id].FilePaths != null)
                                foreach (string path in ecs[id].FilePaths)
                                    TryDeleteFile(path);

                            if (ecs[id].DirPaths != null)
                                foreach (string path in ecs[id].DirPaths)
                                    TryDeleteDirectoryNd(path, true, true);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                // 跨线程更新UI需要使用Invoke
                Invoke(() =>
                {
                    MessageBox.Show($"删除失败: {ex.Message}");
                });
            }
            finally
            {
                // 重新启用按钮
                buttonDelete.Enabled = true;
                checkedListBox.Items.Clear();
            }
        }

        private void checkedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = checkedListBox.SelectedItem.ToString();
            if (checkedListBox.SelectedItem != null)
                label_ProblemInterpretation.Text = $"{str}：\n\n{GetECPI(str)}";
        }

        private string GetECPI(string text)
        {
            foreach (EC ec in ecs)
                if (ec.Text == text)
                    return ec.PI;
            return "ERROR 未找到 PI 值";
        }

        private struct EC
        {
            public string Text; // 显示内容
            public string PI; // 简介
            public string[] FilePaths;
            public string[] DirPaths;

            public EC(string text, string pi, string[] filePaths, string[] dirPaths)
            {
                Text = text;
                PI = pi;
                FilePaths = filePaths;
                DirPaths = dirPaths;
            }
        }
    }
}
