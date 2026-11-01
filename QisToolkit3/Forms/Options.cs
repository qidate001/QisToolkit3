using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Qi; 
using static Qi.QisToolkit3_Datas; 

namespace QisToolkit3.Forms
{
    public partial class Options : Form
    {

        public Options()
        {
            InitializeComponent();
            SetDarkMode();

            if (ComputerNoviceMode)
            {
                button_System.Visible = false;
                buttonInstall.Text = "安装";
                buttonUninstall.Text = "卸载";
            }
        }

        private void SetDarkMode()
        {
            Color BackColor = Color.FromArgb(23, 23, 23);
            Color BtnBackColor = Color.FromArgb(34, 34, 34);

            if (DarkMode)
            {
                this.BackColor = BackColor;
                tabPageHead.BackColor = BackColor;
                buttonInstall.BackColor = BtnBackColor;
                buttonInstall.FlatStyle = FlatStyle.Flat;

                buttonUninstall.BackColor = BtnBackColor;
                buttonUninstall.FlatStyle = FlatStyle.Flat;

                tabControlOptions.ForeColor = Color.FromArgb(177, 177, 177);
                //panelTitleBar.BackColor = Color.FromArgb(51, 51, 73);
            }
        }

        private void Options_Load(object sender, EventArgs e)
        {
            #region
            ReSetSystemEnvironmentBtn();
            checkBox_DarkMode.Checked = DarkMode;
            checkBox_ComputerNoviceMode.Checked = ComputerNoviceMode;

            // 打开文件操作栏自动跳出“打开文件”窗口
            checkBoxFilesOperation_AutomaticallyPopUpTheOpenFileWindow.Checked = FilesOperation_AutomaticallyPopUpTheOpenFileWindow;

            // 系统工具 → 系统程序：程序使用中文名称
            checkBoxToolsOperation_UseChineseName.Checked = ToolsOperation_UseChineseName;

            // 文本处理工具：窗口置顶
            checkBoxToolsProcessingTools_TopMost.Checked = ToolsProcessingTools_TopMost;

            // System 权限备用方案启动
            checkBox_MinSudo.Checked = UseMinSudoDLL;

            #endregion

            comboBoxLanguage.SelectedIndex = GetLanguageId(GetLanguage());
        }

        private void ReSetSystemEnvironmentBtn()
        {
            bool ev = EnvironmentVariableHelper.IsPathInSystemEnvironment(actualDirectory);
            buttonInstall.Enabled = !ev;
            buttonUninstall.Enabled = ev;
        }

        private void checkBoxFilesOperation_AutomaticallyPopUpTheOpenFileWindow_CheckedChanged(object sender, EventArgs e) =>
            FilesOperation_AutomaticallyPopUpTheOpenFileWindow = checkBoxFilesOperation_AutomaticallyPopUpTheOpenFileWindow.Checked;

        private void button_System_Click(object sender, EventArgs e)
        {
            RunNSudo(AppDomain.CurrentDomain.BaseDirectory + "QisToolkit3.exe");
            // MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory + "QisToolkit3.exe");
            Application.Exit();
        }

        private void checkBoxToolsOperation_UseChineseName_CheckedChanged(object sender, EventArgs e) =>
            ToolsOperation_UseChineseName = checkBoxToolsOperation_UseChineseName.Checked;

        private void checkBoxToolsProcessingTools_TopMost_CheckedChanged(object sender, EventArgs e) =>
            ToolsProcessingTools_TopMost = checkBoxToolsProcessingTools_TopMost.Checked;

        private void OutIWshShortcut(WshShell shell, string filename, string path, string name, string args = "")
        {
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut($@"{path}\{name}.lnk");
            shortcut.TargetPath = filename;
            shortcut.Arguments = args;
            shortcut.WorkingDirectory = actualDirectory;
            shortcut.WindowStyle = 1;
            shortcut.Description = "齐的工具包3 快捷启动";
            shortcut.IconLocation = filename + ", 0";
            shortcut.Save();
        }

        private void buttonInstall_Click(object sender, EventArgs e)
        {
            try
            {
                string FileName = $@"{actualDirectory}\QisToolkit3.exe";
                string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string commonPrograms = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms);

                // 创建桌面快捷方式
                WshShell shell = new WshShell();
                OutIWshShortcut(shell, FileName, DesktopPath, "齐的工具包3");
                OutIWshShortcut(shell, FileName, DesktopPath, "齐的工具包：命令行", "-command");
                OutIWshShortcut(shell, FileName, DesktopPath, "齐的工具包：Yt-Dlp", "-o YtDlpTool");
                OutIWshShortcut(shell, FileName, DesktopPath, "齐的工具包：SPL", "-o SystemPermissionLauncher");
                OutIWshShortcut(shell, FileName, commonPrograms, "齐的工具包3");
                OutIWshShortcut(shell, FileName, commonPrograms, "齐的工具包：命令行", "-command");
                OutIWshShortcut(shell, FileName, commonPrograms, "齐的工具包：Yt-Dlp", "-o YtDlpTool");
                OutIWshShortcut(shell, FileName, commonPrograms, "齐的工具包：SPL", "-o SystemPermissionLauncher");
                OutIWshShortcut(shell, FileName, actualDirectory, "qt");
                OutIWshShortcut(shell, FileName, actualDirectory, "q", "-command");

                // 软件信息
                RegistryKey registry = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\QisToolkit3", true);
                registry.SetValue("DisplayName", "齐的工具包3");
                registry.SetValue("DisplayIcon", actualDirectory + @"\QisToolkit3.exe");
                registry.SetValue("InstallLocation", actualDirectory);
                registry.SetValue("UninstallString", actualDirectory + @"\Uninstall_QisToolkit3.exe");
                registry.SetValue("Version", QisToolkit3_Datas.Version);

                EnvironmentVariableHelper.AddPathsToSystemEnvironment(actualDirectory);

                // ===== 注册自定义协议 qistoolkit3:// =====
                const string protocolName = "qistoolkit3";
                try
                {
                    // 1) 协议主键
                    using (RegistryKey protocolKey = Registry.ClassesRoot.CreateSubKey(protocolName, true))
                    {
                        if (protocolKey != null)
                        {
                            protocolKey.SetValue("", $"URL:{protocolName} Protocol");
                            protocolKey.SetValue("URL Protocol", "");

                            // 2) 命令键：指向 QisToolkit3.exe，并接收浏览器传入的完整 URL
                            using (RegistryKey commandKey = protocolKey.CreateSubKey(@"shell\open\command", true))
                            {
                                if (commandKey != null)
                                {
                                    // 注意：这里用引号包裹 exe 路径，并保留 "%1" 作为完整 URL
                                    commandKey.SetValue("", $"\"{FileName}\" \"%1\"");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("协议注册失败（可继续使用其他功能）：\n" + ex.Message, "警告");
                    Log.Err("[软件设置] 协议注册失败！", ex);
                }

                MessageBox.Show(
                    "操作已完成！\n\n" +
                    "已向桌面释放快捷方式！\n" +
                    "已向系统环境变量释放路径\n" +
                    "已向系统注册自定义协议 qistoolkit3://\n" +
                    "你现在可以通过在命令提示符中输入 “QisToolkit3” 来启动齐的工具包3",
                    "提示"
                );

                ReSetSystemEnvironmentBtn();
            }
            catch (Exception ex)
            {
                MessageBox.Show("代码执行时出现错误！\n报错信息：" + ex.Message, "ERROR");
                Log.Err("[软件设置] 代码执行时出现错误！", ex);
            }
        }

        private void buttonUninstall_Click(object sender, EventArgs e)
        {
            try
            {
                string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string commonPrograms = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms);

                // 删除桌面快捷方式
                TryDeleteFile(Path.Combine(DesktopPath, "齐的工具包3.lnk"));
                TryDeleteFile(Path.Combine(DesktopPath, "齐的工具包：命令行.lnk"));
                TryDeleteFile(Path.Combine(DesktopPath, "齐的工具包：Yt-Dlp.lnk"));
                TryDeleteFile(Path.Combine(DesktopPath, "齐的工具包：SLP.lnk"));
                TryDeleteFile(Path.Combine(commonPrograms, "齐的工具包3.lnk"));
                TryDeleteFile(Path.Combine(commonPrograms, "齐的工具包：命令行.lnk"));
                TryDeleteFile(Path.Combine(commonPrograms, "齐的工具包：Yt-Dlp.lnk"));
                TryDeleteFile(Path.Combine(commonPrograms, "齐的工具包：SLP.lnk"));
                TryDeleteFile(Path.Combine(actualDirectory, "q.lnk"));
                TryDeleteFile(Path.Combine(actualDirectory, "qt.lnk"));

                HkImTryDeleteTree(Registry.LocalMachine.CreateSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall", true), "QisToolkit3");
                EnvironmentVariableHelper.RemovePathFromSystemEnvironment(actualDirectory);

                // ===== 删除自定义协议 qistoolkit:// =====
                try
                {
                    Registry.ClassesRoot.DeleteSubKeyTree("qistoolkit3", false);
                }
                catch (Exception ex)
                {
                    // 键不存在时忽略；其他异常记录日志
                    Log.Err("[软件设置] 删除协议注册表失败", ex);
                }

                MessageBox.Show("操作已完成！\n\n所有释放的注册表、文件、环境变量等均已删除", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show("代码执行时出现错误！\n报错信息：" + ex.Message, "错误");
                Log.Err("[软件设置] 代码执行时出现错误！", ex);
            }

            ReSetSystemEnvironmentBtn();
        }

        private void checkBox_DarkMode_CheckedChanged(object sender, EventArgs e)
        {
            DarkMode = checkBox_DarkMode.Checked;
        }

        private void checkBox_ComputerNoviceMode_CheckedChanged(object sender, EventArgs e)
        {
            ComputerNoviceMode = checkBox_ComputerNoviceMode.Checked;
        }

        private void button_ExtendedFeatures_Click(object sender, EventArgs e)
        {
            new ExtendedFeatures().Show();
        }

        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.ini");
            System.IO.File.WriteAllText(filePath, GetLanguageName(comboBoxLanguage.SelectedIndex));
            Language = GetLanguageName(comboBoxLanguage.SelectedIndex);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(GetLanguage());

            checkBoxToolsOperation_UseChineseName.Checked = Language == "zh-CN";
        }

        private void button_Command_Click(object sender, EventArgs e)
        {
            RunNSudo(AppDomain.CurrentDomain.BaseDirectory + "QisToolkit3.exe");
            // MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory + "QisToolkit3.exe");
            Application.Exit();
        }

        private void button_SystemCommandMode_Click(object sender, EventArgs e)
        {
            //RunNSudo($"{AppDomain.CurrentDomain.BaseDirectory + "QisToolkit3.exe"} -command", "-U:T -P:E -M:S -Priority:RealTime", true);
            RunNSudo($"{AppDomain.CurrentDomain.BaseDirectory + "QisToolkit3.exe"} -command");
            //UnRunNSudo($"{AppDomain.CurrentDomain.BaseDirectory + "QisToolkit3.exe"} -command");
            //MessageBox.Show($"start {AppDomain.CurrentDomain.BaseDirectory + "QisToolkit3.exe"} -command");
        }

        private void button_CommandMode_Click(object sender, EventArgs e)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "QisToolkit3.exe"),
                    Arguments = $"-command",
                };

                process.Start();
            }
        }

        private void checkBox_MinSudo_CheckedChanged(object sender, EventArgs e)
        {
            UseMinSudoDLL = checkBox_MinSudo.Checked;
        }

        private void button_GitHub_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/qidate001/QisToolkit3") { UseShellExecute = true });
        }

        private void button_DownloadForGitHub_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/qidate001/QisToolkit3/releases") { UseShellExecute = true });
        }

        private void button_DownloadForBaidu_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://pan.baidu.com/s/1XTg3X1aTYdIP1Q11YZ0fiQ?pwd=qinb") { UseShellExecute = true });
            Clipboard.SetText("qinb");
            //MessageBox.Show("提取码: qinb", "提取码");
        }

        private void button_button_QisWeb_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.qidate001.com/qistoolkit3/") { UseShellExecute = true });
        }

        private void button_DownloadForQQ_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://qm.qq.com/q/x5V5tMYMA8") { UseShellExecute = true });
        }

        private void button_QQEmail_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("3563532971@qq.com");
            MessageBox.Show("3563532971@qq.com\n已复制至剪切板。", "邮箱");
        }

        private void button_Gmail_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("qidate001@gmail.com");
            MessageBox.Show("qidate001@gmail.com\n已复制至剪切板。", "邮箱");
        }
    }
}
