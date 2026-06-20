using Microsoft.Win32;
using QisToolkit3.Forms.EntertainmentTool;
using QisToolkit3.Forms.IndependentTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Qi;
using static Qi.QisToolkit3_Datas;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QisToolkit3.Forms
{
    public partial class Tools : Form
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public Tools()
        {
            InitializeComponent();

            //buttonGetCleanTempFilesLog.Enabled = File.Exists(@"C:\QiAppDatas\Temps\QisToolkit3_CleanTempFiles.log");

            if (ToolsOperation_UseChineseName)
            {
                buttonStartCmd.Text = "命令提示符";
                buttonStartWindowsPowerShell.Text = "Power Shell";
                buttonStartConsoleHostProcess.Text = "命令行程序宿主";
                buttonStartTaskManager.Text = "任务管理器";
                buttonStartExplorer.Text = "资源管理器";
                buttonStartRegedit.Text = "注册表编辑器";
            }

            if (ComputerNoviceMode)
            {
                buttonImageHijackingTool.Visible = false;
                buttonDCQAA.Visible = false;
                tabPageSystemTool.Text = "系统工具（小白勿用）";
            }

            //MessageBox.Show("程序真实路径: " + exePath);
            //MessageBox.Show("程序实际目录: " + actualDirectory);
            buttonYtDlp.Enabled = File.Exists(@$"{actualDirectory}\yt-dlp\yt-dlp.exe");
            buttonGeek.Enabled = File.Exists(@$"{actualDirectory}\ElseTool\geek.exe");
            buttonIDM.Enabled = File.Exists($@"{actualDirectory}\ElseTool\IDMActivateTool.cmd");
            buttonMAS.Enabled = File.Exists($@"{actualDirectory}\ElseTool\Mas.cmd");
            button_QisToolkit3_PersecutionSystem.Enabled = File.Exists($@"{actualDirectory}\QisToolkit3_PersecutionSystem.exe");


            textBoxProcessOwner.Text = owner;
        }

        private void buttonGetCpuName_Click(object sender, EventArgs e)
        {
            try
            {
                comboBoxCpuName.Text = Registry.LocalMachine.CreateSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0").GetValue("ProcessorNameString").ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取失败！错误原因：" + ex.Message, "报错", MessageBoxButtons.OK);
            }
        }

        private void buttonSetCpuName_Click(object sender, EventArgs e)
        {
            try
            {
                Registry.LocalMachine.CreateSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0").SetValue("ProcessorNameString", comboBoxCpuName.Text);
                MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK);

            }
            catch (Exception ex)
            {
                MessageBox.Show("修改失败！错误原因：" + ex.Message, "报错", MessageBoxButtons.OK);
            }
        }

        private void labelCpuName_Click(object sender, EventArgs e) =>
            MessageBox.Show(toolTip.GetToolTip(labelCpuName), "功能介绍");

        /*
        private void buttonGetSaveFolder_Click(object sender, EventArgs e)
        {
            comboBoxSaveFolder.Items.Clear();
            if (Directory.Exists(documentsPath + @"\Klei\DoNotStarveTogether"))
                comboBoxSaveFolder.Items.Add("饥荒联机版");
            if (Directory.Exists(documentsPath + @"\Klei\DoNotStarveTogetherBetaBranch"))
                comboBoxSaveFolder.Items.Add("饥荒联机版（Beta版分支）");

            comboBoxSaveFolder.Enabled = true;
        }

        private void buttonOpenSaveFolder_Click(object sender, EventArgs e)
        {
            switch (comboBoxSaveFolder.Text)
            {
                case "饥荒联机版":
                    OpenDirectory(documentsPath + @"\Klei\DoNotStarveTogether");
                    break;

                case "饥荒联机版（Beta版分支）":
                    OpenDirectory(documentsPath + @"\Klei\DoNotStarveTogetherBetaBranch");
                    break;
            }
        }


        private void comboBoxSaveFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonOpenSaveFolder.Enabled = true;
            buttonCleanTempFiles.Enabled = comboBoxSaveFolder.Text != "饥荒联机版（Beta版分支）";
        }
        */




        public static void OpenDirectory(string directoryPath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = directoryPath,
                FileName = "explorer.exe"
            };

            Process.Start(startInfo);
        }

        /*
        private void buttonCleanTempFiles_Click(object sender, EventArgs e)
        {
            string Str = "";
            switch (comboBoxSaveFolder.Text)
            {
                case "饥荒联机版":
                    Str += TryDeleteFile(documentsPath + @"\Klei\DoNotStarveTogether\caves_server_chat_log.txt");
                    Str += TryDeleteFile(documentsPath + @"\Klei\DoNotStarveTogether\caves_server_log.txt");
                    Str += TryDeleteFile(documentsPath + @"\Klei\DoNotStarveTogether\client_chat_log.txt");
                    Str += TryDeleteFile(documentsPath + @"\Klei\DoNotStarveTogether\client_log.txt");
                    Str += TryDeleteFile(documentsPath + @"\Klei\DoNotStarveTogether\master_server_chat_log.txt");
                    Str += TryDeleteFile(documentsPath + @"\Klei\DoNotStarveTogether\master_server_log.txt");
                    Str += TryDeleteDirectoryNd(documentsPath + @"\Klei\DoNotStarveTogether\backup\client_chat_log");
                    Str += TryDeleteDirectoryNd(documentsPath + @"\Klei\DoNotStarveTogether\backup\client_log");
                    break;
            }

            Directory.CreateDirectory(@"C:\QiAppDatas\Temps\");
            File.WriteAllText(@"C:\QiAppDatas\Temps\QisToolkit3_CleanTempFiles.log", Str);
            MessageBox.Show(Str + "\n清理完成。", "文件阅读");
            buttonGetCleanTempFilesLog.Enabled = true;
        }
        */

        private void buttonGetCleanTempFilesLog_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\QiAppDatas\Temps\QisToolkit3_CleanTempFiles.log"))
                MessageBox.Show(File.ReadAllText(@"C:\QiAppDatas\Temps\QisToolkit3_CleanTempFiles.log") + "\n清理完成。", "文件阅读");
        }

        private void button_WindowsUser_Password_set_Click(object sender, EventArgs e)
        {
            if (SetWindowsUserPassword(textBox_WindowsUser_Password.Text, textBox_UserName.Text))
                MessageBox.Show("密码修改成功", "提示", MessageBoxButtons.OK);
            else
                MessageBox.Show("程序异常：获取计算机实例失败\n\n请尝试以管理员身份运行此程序", "警告", MessageBoxButtons.OK);
        }

        private void button_WindowsUser_Password_clear_Click(object sender, EventArgs e)
        {
            if (SetWindowsUserPassword(string.Empty, textBox_UserName.Text))
                MessageBox.Show("密码清除成功", "提示", MessageBoxButtons.OK);
            else
                MessageBox.Show("程序异常：获取计算机实例失败\n\n请尝试以管理员身份运行此程序", "警告", MessageBoxButtons.OK);
        }

        private void buttonStartCmd_Click(object sender, EventArgs e) =>
            RunNSudo("cmd.exe");

        private void buttonStartWindowsPowerShell_Click(object sender, EventArgs e) =>
            RunNSudo("powershell.exe");

        private void buttonStartConsoleHostProcess_Click(object sender, EventArgs e) =>
            RunNSudo("conhost.exe");

        private void buttonStartTaskManager_Click(object sender, EventArgs e) =>
            RunNSudo("Taskmgr.exe");

        private void buttonStartExplorer_Click(object sender, EventArgs e) =>
            RunNSudo("explorer.exe");

        private void buttonStartRegedit_Click(object sender, EventArgs e) =>
            RunNSudo("regedit.exe");


        #region 工具窗口
        private void buttonTextProcessingTools_Click(object sender, EventArgs e) => new TextProcessorForm().Show();

        private void buttonWhatToEatToday_Click(object sender, EventArgs e) => new WhatToEatToday().Show();

        private void buttonSoftwareDownload_Click(object sender, EventArgs e) => new SoftwareDownload().Show();

        private void button_Calculator_Click(object sender, EventArgs e) => new Calculator().Show();

        private void buttonYtDlp_Click(object sender, EventArgs e) => new YtDlpTool().Show();

        private void buttonSystemErrorCheck_Click(object sender, EventArgs e) => new SystemErrorCheck().Show();

        private void buttonDCQAA_Click(object sender, EventArgs e) => new DCQAA().Show();

        private void buttonImageHijackingTool_Click(object sender, EventArgs e) => new ImageHijackingTool().Show();

        private void button_SendMessageToWindowsTool_Click(object sender, EventArgs e) => new SendMessageToWindowsTool().Show();

        private void button_CmdInQisToolkit3_Click(object sender, EventArgs e) => new CmdInQisToolkit3().Show();

        private void buttonMyComputerNameSpaceTool_Click(object sender, EventArgs e) => new MyComputerNameSpaceTool().Show();

        private void buttonUninstallRegistryKeysTool_Click(object sender, EventArgs e) => new UninstallRegistryKeysTool().Show();

        private void button_UnicodeTool_Click(object sender, EventArgs e) => new UnicodeTool().Show();

        private void button_ScanRogueSoftwareTool_Click(object sender, EventArgs e) => new ScanRogueSoftwareTool().Show();

        private void buttonAdvancedModificationSystemTools_Click(object sender, EventArgs e) => new AdvancedModificationSystemTools().Show();

        private void buttonMediumAutoStartTool_Click(object sender, EventArgs e) => new MediumAutoStartTool().Show();

        private void button_SPL_Click(object sender, EventArgs e) => new SystemPermissionLauncher().Show();

        private void button_HitokotoTool_Click(object sender, EventArgs e) => new HitokotoForm().Show();

        private void button_SoftwareFunctionPage_Click(object sender, EventArgs e) => new SoftwareFunctionPage().Show();

        private void button_FFmpeg_Click(object sender, EventArgs e) => new FFmpegTool().Show();

        private void buttonSystemServiceTools_Click(object sender, EventArgs e) => new SystemServiceTools().Show();

        private void buttonAdvancedRenamer_Click(object sender, EventArgs e) => new AdvancedRenamer().Show();

        private void button_JokeAPIForm_Click(object sender, EventArgs e) => new JokeApiForm().Show();

        private void buttonCommonFunctionalTools_Click(object sender, EventArgs e) => new CommonFunctionalTools().Show();

        #endregion

        private void checkBox_NoWindow_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox_ExitCmd.Checked)
                checkBox_ExitCmd.Checked = true;
        }

        private void checkBox_ExitCmd_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox_ExitCmd.Checked)
                checkBox_NewWindow.Checked = false;
        }



        private void RCC(string[] cmdline)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                //process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = !checkBox_NewWindow.Checked;
                process.Start();
                //process.StandardInput.AutoFlush = true;
                foreach (string line in cmdline)
                    process.StandardInput.WriteLine(line);
                if (checkBox_ExitCmd.Checked)
                    process.StandardInput.WriteLine("exit");

                //if (checkBox_NewWindow.Checked) output = process.StandardOutput.ReadToEnd();

                if (checkBox_ExitCmd.Checked)
                {
                    process.WaitForExit();
                    process.Close();
                }
            }
        }

        private void RCCL(string cmdline) => RCC([cmdline]);

        private void button_SystemInfo_Click(object sender, EventArgs e) =>
            RCCL("SYSTEMINFO");


        private void button_IpConfig_Click(object sender, EventArgs e) =>
            RCCL("ipconfig /all");

        private void button_PingBaidu_Click(object sender, EventArgs e) =>
            RCC(["ping -t -a 36.152.44.93", "ping -l 1024 -t -a 36.152.44.93"]);

        private void Tools_Load(object sender, EventArgs e)
        {
            SetbuttonQisDefense();
        }

        private void SetbuttonQisDefense()
        {
            if (!File.Exists(@$"{actualDirectory}\QisDefense.exe"))
            {
                buttonQisDefense.Enabled = false;
                return;
            }

            if (IsQisDefenseOn())
            {
                buttonQisDefense.ForeColor = Color.Green;
                buttonQisDefense.Text = "齐之防御 总开关 ON";
            }
            else
            {
                buttonQisDefense.ForeColor = Color.Red;
                buttonQisDefense.Text = "齐之防御 总开关 OFF";
            }
        }

        private void button_slmgr_dli_Click(object sender, EventArgs e) => ExecuteInCmd("slmgr /dli");

        private void checkBox_ShowRegPoliciesExplorer_CheckedChanged(object sender, EventArgs e) =>
            groupBox_RegPoliciesExplorer.Visible = checkBox_ShowRegPoliciesExplorer.Checked;

        private void checkBox_ShowHardwarePseudoRenaming_CheckedChanged(object sender, EventArgs e) =>
            groupBox_HardwarePseudoRenaming.Visible = checkBox_ShowHardwarePseudoRenaming.Checked;

        #region 系统开发者预留功能

        private void PESet_String(string Name, string Value) =>
            Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer").SetValue(Name, Value);

        private void PESet_Int(string Name, int Value) =>
            Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer").SetValue(Name, Value);

        private void PESet_Bytes(string Name, byte[] Value) =>
            Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer").SetValue(Name, Value);

        private void button_RegPoliciesExplorer_Set_Click(object sender, EventArgs e)
        {
            if (checkBox_NoRun.Checked) PESet_Int("NoRun", 1); else PESet_Int("NoRun", 0);
            if (checkBox_Nofind.Checked) PESet_Int("Nofind", 1); else PESet_Int("Nofind", 0);
            if (checkBox_NoClose.Checked) PESet_Int("NoClose", 1); else PESet_Int("NoClose", 0);
            if (checkBox_NoLogOff.Checked) PESet_Int("NoLogOff", 1); else PESet_Int("NoLogOff", 0);
            if (checkBox_NoDesktop.Checked) PESet_Int("NoDesktop", 1); else PESet_Int("NoDesktop", 0);
            if (checkBox_NoSetFolders.Checked) PESet_Int("NoSetFolders", 1); else PESet_Int("NoSetFolders", 0);
            if (checkBox_NoControlPanel.Checked) PESet_Int("NoControlPanel", 1); else PESet_Int("NoControlPanel", 0);
            if (checkBox_NoSaveSettings.Checked) PESet_Int("NoSaveSettings", 1); else PESet_Int("NoSaveSettings", 0);
            if (checkBox_NoFavoritesMenu.Checked) PESet_Int("NoFavoritesMenu", 1); else PESet_Int("NoFavoritesMenu", 0);
            if (checkBox_NoWindowsUpdate.Checked) PESet_Int("NoWindowsUpdate", 1); else PESet_Int("NoWindowsUpdate", 0);
            if (checkBox_NofolderOptions.Checked) PESet_Int("NofolderOptions", 1); else PESet_Int("NofolderOptions", 0);
            if (checkBox_NoWindowsUpdate.Checked) PESet_Int("NoWindowsUpdate", 1); else PESet_Int("NoWindowsUpdate", 0);
            if (checkBox_NoRecentDocsMenu.Checked) PESet_Int("NoRecentDocsMenu", 1); else PESet_Int("NoRecentDocsMenu", 0);
            if (checkBox_NoViewContextMenu.Checked) PESet_Int("NoViewContextMenu", 1); else PESet_Int("NoViewContextMenu", 0);
            if (checkBox_NoTrayContextMenu.Checked) PESet_Int("NoTrayContextMenu", 1); else PESet_Int("NoTrayContextMenu", 0);
            if (checkBox_NoSetActiveDesktop.Checked) PESet_Int("NoSetActiveDesktop", 1); else PESet_Int("NoSetActiveDesktop", 0);
            if (checkBox_NoRecentDocsHistory.Checked) PESet_Bytes("NoRecentDocsHistory", [0x01, 0x00, 0x00, 0x00]); else PESet_Bytes("NoRecentDocsHistory", [0x00, 0x00, 0x00, 0x00]);
            if (checkBox_ClearRecentDocsonExit.Checked) PESet_Bytes("ClearRecentDocsonExit", [0x01, 0x00, 0x00, 0x00]); else PESet_Bytes("ClearRecentDocsonExit", [0x00, 0x00, 0x00, 0x00]);

            if (checkBox_EditLevel.Checked) PESet_Int("EditLevel", 1); else PESet_Int("EditLevel", 0);
            if (checkBox_NoNetHood.Checked) PESet_Int("NoNetHood", 1); else PESet_Int("NoNetHood", 0);
            if (checkBox_NoFileMenu.Checked) PESet_Int("NoFileMenu", 1); else PESet_Int("NoFileMenu", 0);
            if (checkBox_NoAddPrinter.Checked) PESet_Int("NoAddPrinter", 1); else PESet_Int("NoAddPrinter", 0);
            if (checkBox_NoSetTaskbar.Checked) PESet_Int("NoSetTaskbar", 1); else PESet_Int("NoSetTaskbar", 0);
            if (checkBox_NoStartBanner.Checked) PESet_Int("NoStartBanner", 1); else PESet_Int("NoStartBanner", 0);
            if (checkBox_NoPrinterTabs.Checked) PESet_Int("NoPrinterTabs", 1); else PESet_Int("NoPrinterTabs", 0);
            if (checkBox_NoInternetIcon.Checked) PESet_Int("NoInternetIcon", 1); else PESet_Int("NoInternetIcon", 0);
            if (checkBox_NoActiveDesktop.Checked) PESet_Int("NoActiveDesktop", 1); else PESet_Int("NoActiveDesktop", 0);
            if (checkBox_NoDeletePrinter.Checked) PESet_Int("NoDeletePrinter", 1); else PESet_Int("NoDeletePrinter", 0);
            if (checkBox_NoChangeStartMenu.Checked) PESet_Int("NoChangeStartMenu", 1); else PESet_Int("NoChangeStartMenu", 0);

            RestartExplorer();
            MessageBox.Show("操作完成", "提示");
        }

        #endregion

        private void buttonGeek_Click(object sender, EventArgs e) => RunNSudo(@"ElseTool\geek.exe");

        private void buttonQisDefense_Click(object sender, EventArgs e)
        {
            try
            {
                // 关闭齐之防御
                if (IsQisDefenseOn())
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                    {
                        try
                        {
                            key.DeleteValue("00_QisDefense");
                        }
                        catch { }
                    }
                    SetbuttonQisDefense();
                    MessageBox.Show("齐之防御已禁用。\n部分功能可能需重启才可退出。", "齐的工具包3 & 齐之防御");
                }

                // 开启齐之防御
                else
                {
                    string nsudoPath = $"\"{actualDirectory}\\NSudoL.exe\"";
                    string qisDefensePath = @$"{actualDirectory}\QisDefense.exe";
                    string arguments = $"-U:T -P:E -Wait \"{qisDefensePath}\"";

                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                    {
                        key.SetValue("00_QisDefense", $"\"{qisDefensePath}\" SysReStart");
                    }

                    RunNSudo(qisDefensePath);
                    SetbuttonQisDefense();
                    MessageBox.Show("齐之防御已准备就绪。", "齐的工具包3 & 齐之防御");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"程序出现错误！\n报错信息：{ex.Message}", "齐的工具包3");
            }
        }

        private bool IsQisDefenseOn()
        {
            object _temp = false;
            try
            {
                _temp =
                    Registry
                        .LocalMachine
                            .CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true)
                                .GetValue("00_QisDefense");
            }
            catch { }

            return _temp != null;
        }

        public void buttonIDM_Click(object sender, EventArgs e)
        {
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = @"/c ElseTool\IDMActivateTool.cmd&exit",
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit();
        }

        public void buttonMAS_Click(object sender, EventArgs e)
        {
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = @"/c ElseTool\Mas.cmd&exit",
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit();
        }

        private void button_QisToolkit3_PersecutionSystem_Click(object sender, EventArgs e) => RunNSudo(@$"{actualDirectory}\QisToolkit3_PersecutionSystem.exe");


        private void buttonUnlockMusic_Click(object sender, EventArgs e)
        {
            OpenBrowser("https://unlock-music.lmb520.cn/");
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void buttonShellFolderSetPathTool_Click(object sender, EventArgs e)
        {
            new ShellFolderSetPathTool().Show();
        }

        private void button_PaywallBuster_Click(object sender, EventArgs e)
        {
            OpenBrowser("https://paywallbuster.me/");
        }

        private void buttonTaskManager_Click(object sender, EventArgs e)
        {
            new TaskManager().Show();
        }
    }
}
