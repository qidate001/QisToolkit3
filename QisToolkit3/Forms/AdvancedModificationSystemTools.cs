using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Qi;
using static Qi.QisToolkit3_Datas;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QisToolkit3.Forms
{
    public partial class AdvancedModificationSystemTools : Form
    {
        private bool Loading = true;
        private RegistryKey RegPoliciesExplorer_registry = Registry.LocalMachine;
        private PasswordInfo passwordInfo = new PasswordInfo();
        private string _machineName;

        public AdvancedModificationSystemTools()
        {
            InitializeComponent();

            //if (!isSystem)
            //    MessageBox.Show(
            //        "您当前并非系统级权限使用此工具！\n推荐用系统级权限使用此工具，\n部分功能将不可用！",
            //        "警告",
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Information
            //    );

            // 初始化
            FormInitDo(this.Text, $"，系统级权限：{isSystem}");

            comboBox_RegPoliciesExplorer_Type.SelectedIndex = 0;
        }

        private void AdvancedModificationSystemTools_Load(object sender, EventArgs e)
        {
            LoadDatas();
        }

        // 加载数据
        private void LoadDatas()
        {
            try
            {
                Loading = true;

                List<string> users = GetLocalUsers();
                RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Command Processor");
                comboBox_cmd_autorun.Text = (registryKey.GetValue("autorun") ?? string.Empty).ToString();
                checkBox_cmd_autorun.Checked = registryKey.GetValue("autorun") != null;

                // OEM
                registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\OEMInformation");
                textBox_OEM_ManuFacturer.Text = (registryKey.GetValue("ManuFacturer") ?? string.Empty).ToString();
                textBox_OEM_Model.Text = (registryKey.GetValue("Model") ?? string.Empty).ToString();
                textBox_OEM_SupportHours.Text = (registryKey.GetValue("SupportHours") ?? string.Empty).ToString();
                textBox_OEM_SupportPhone.Text = (registryKey.GetValue("SupportPhone") ?? string.Empty).ToString();
                textBox_OEM_SupportURL.Text = (registryKey.GetValue("SupportURL") ?? string.Empty).ToString();
                checkBoxOEM.Checked = !(
                    registryKey.GetValue("ManuFacturer") == null ||
                    registryKey.GetValue("Model") == null ||
                    registryKey.GetValue("SupportHours") == null ||
                    registryKey.GetValue("SupportPhone") == null ||
                    registryKey.GetValue("SupportURL") == null
                );

                // CPU 名
                comboBoxCpuName.Text = Registry.LocalMachine
                    .CreateSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0")
                    .GetValue("ProcessorNameString")
                    .ToString();

                // GPU 设备路径
                comboBox_GPUPNPDeviceID.Text = GetGPUPNPDeviceID();

                // Policies\System
                registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
                textBox_legalnoticecaption.Text = (registryKey.GetValue("legalnoticecaption") ?? string.Empty).ToString();
                textBox_legalnoticetext.Text = (registryKey.GetValue("legalnoticetext") ?? string.Empty).ToString();

                checkBox_legalnoticecaption.Checked = registryKey.GetValue("legalnoticecaption") != null;
                checkBox_legalnoticetext.Checked = registryKey.GetValue("legalnoticetext") != null;


                // Windows 版本标识
                registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                comboBox_WindowsEditionID.Text = (registryKey.GetValue("EditionID") ?? "Windows 版本标识获取失败").ToString();


                // 密码
                comboBox_UserName.Text = Environment.UserName;
                comboBox_UserName.Items.AddRange(users.ToArray());


                button_RegWinDefender_NoSystem.Visible = !isSystem;

                button_UAC_Read_Click(null, null);

                // 获取电源选项相关数据
                LoadPowerData();


                Loading = false;
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
            }
        }

        private void LoadPowerData()
        {
            string guid = PowerManager.GetActiveScheme().ToString();

            switch (guid)
            {
                case "381b4222-f694-41f0-9685-ff5bb260df2e":
                    radioButton_ActiveScheme_0.Checked = true;
                    Log.Info($"[高级系统修改工具] [电源选项] 当前电源选项为 {guid}（平衡模式）");
                    break;

                case "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c":
                    radioButton_ActiveScheme_1.Checked = true;
                    Log.Info($"[高级系统修改工具] [电源选项] 当前电源选项为 {guid}（高性能模式）");
                    break;

                case "a1841308-3541-4fab-bc81-f71556f20b4a":
                    radioButton_ActiveScheme_2.Checked = true;
                    Log.Info($"[高级系统修改工具] [电源选项] 当前电源选项为 {guid}（节能模式）");
                    break;

                default:
                    radioButton_ActiveScheme_3.Checked = true;
                    Log.Info($"[高级系统修改工具] [电源选项] 当前电源选项为 {guid}");
                    break;
            }

            comboBox_PowerGUID.Text = guid;
        }

        private void LogEnable(string Name, bool Data)
        {
            if (Data) Log.Info("[高级系统修改工具] 已启用【CMD 启动时自动运行】");
            else Log.Info("[高级系统修改工具] 已禁用【CMD 启动时自动运行】");
        }

        private void MessageBoxSuccess(string Name, string Data = "True")
        {
            Log.Info($"[高级系统修改工具] 已将【{Name}】设置为【{Data}】");
            MessageBox.Show("执行成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MessageBoxERROR(Exception ex)
        {
            string text = ERROR_Text(ex);

            Log.Err(text);
            MessageBox.Show(text, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private string ERROR_Text(Exception ex) =>
            $"[高级系统修改工具] 程序出现错误，错误原因：{ex.Message}\n请联系开发人员，完整报错：\n{ex.ToString()}";

        private void checkBox_cmd_autorun_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_cmd_autorun.Enabled = checkBox_cmd_autorun.Checked;
            button_cmd_autorun.Enabled = checkBox_cmd_autorun.Checked;

            if (Loading) return;

            try
            {
                if (checkBox_cmd_autorun.Checked)
                {
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Command Processor").SetValue("autorun", string.Empty);
                }
                else
                {
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Command Processor").DeleteValue("autorun");
                }

                LogEnable("CMD 启动时自动运行", checkBox_cmd_autorun.Checked);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
            }
        }

        private void button_cmd_autorun_Click(object sender, EventArgs e)
        {
            try
            {
                Registry.
                    LocalMachine.
                        CreateSubKey(@"SOFTWARE\Microsoft\Command Processor").
                            SetValue("autorun", comboBox_cmd_autorun.Text);

                MessageBoxSuccess("CMD 启动时自动运行", comboBox_cmd_autorun.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
            }
        }

        private void checkBoxOEM_CheckedChanged(object sender, EventArgs e)
        {
            bool chk = checkBoxOEM.Checked;
            textBox_OEM_ManuFacturer.Enabled = chk;
            button_OEM_ManuFacturer.Enabled = chk;

            textBox_OEM_Model.Enabled = chk;
            button_OEM_Model.Enabled = chk;

            textBox_OEM_SupportHours.Enabled = chk;
            button_OEM_SupportHours.Enabled = chk;

            textBox_OEM_SupportPhone.Enabled = chk;
            button_OEM_SupportPhone.Enabled = chk;

            textBox_OEM_SupportURL.Enabled = chk;
            button_OEM_SupportURL.Enabled = chk;

            if (Loading) return;

            try
            {
                RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\OEMInformation");
                if (checkBoxOEM.Checked)
                {
                    key.SetValue("ManuFacturer", string.Empty);
                    key.SetValue("Model", string.Empty);
                    key.SetValue("SupportHours", string.Empty);
                    key.SetValue("SupportPhone", string.Empty);
                    key.SetValue("SupportURL", string.Empty);
                }
                else
                {
                    key.DeleteValue("ManuFacturer");
                    key.DeleteValue("Model");
                    key.DeleteValue("SupportHours");
                    key.DeleteValue("SupportPhone");
                    key.DeleteValue("SupportURL");
                }

                LogEnable("OEM支持商修改", checkBoxOEM.Checked);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
            }
        }

        #region OEM
        private void OEM_Set(string Name, string Value)
        {
            try
            {
                Registry.
                    LocalMachine
                        .CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\OEMInformation")
                            .SetValue(Name, Value);
                MessageBox.Show("设置成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
            }
        }

        private void button_OEM_ManuFacturer_Click(object sender, EventArgs e) => OEM_Set("ManuFacturer", textBox_OEM_ManuFacturer.Text);

        private void button_OEM_Model_Click(object sender, EventArgs e) => OEM_Set("Model", textBox_OEM_Model.Text);

        private void button_OEM_SupportHours_Click(object sender, EventArgs e) => OEM_Set("SupportHours", textBox_OEM_SupportHours.Text);

        private void button_OEM_SupportPhone_Click(object sender, EventArgs e) => OEM_Set("SupportPhone", textBox_OEM_SupportPhone.Text);

        private void button_OEM_SupportURL_Click(object sender, EventArgs e) => OEM_Set("SupportURL", textBox_OEM_SupportURL.Text);
        #endregion

        private void checkBox_legalnoticecaption_CheckedChanged(object sender, EventArgs e)
        {
            textBox_legalnoticecaption.Enabled = checkBox_legalnoticecaption.Checked;
            button_legalnoticecaption.Enabled = checkBox_legalnoticecaption.Checked;

            if (Loading) return;

            try
            {
                if (checkBox_legalnoticecaption.Checked)
                {
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System").SetValue("legalnoticecaption", string.Empty);
                }
                else
                {
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System").DeleteValue("legalnoticecaption");
                }
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
            }
        }

        private void checkBox_legalnoticetext_CheckedChanged(object sender, EventArgs e)
        {
            textBox_legalnoticetext.Enabled = checkBox_legalnoticetext.Checked;
            button_legalnoticetext.Enabled = checkBox_legalnoticetext.Checked;

            if (Loading) return;

            try
            {
                if (checkBox_legalnoticetext.Checked)
                {
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System").SetValue("legalnoticetext", string.Empty);
                }
                else
                {
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System").DeleteValue("legalnoticetext");
                }
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
            }
        }

        #region Windows 标识

        private void button_legalnoticecaption_Click(object sender, EventArgs e)
        {
            try
            {
                Registry.
                    LocalMachine.
                        CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System").
                            SetValue("legalnoticecaption", textBox_legalnoticecaption.Text);
                MessageBox.Show("设置成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
            }
        }

        private void button_legalnoticetext_Click(object sender, EventArgs e)
        {
            try
            {
                Registry.
                    LocalMachine.
                        CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System").
                            SetValue("legalnoticetext", textBox_legalnoticetext.Text);
                MessageBox.Show("设置成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
            }
        }


        private void button_registeredOwner_Click(object sender, EventArgs e)
        {
            try
            {
                Registry.
                    LocalMachine.
                        CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion").
                            SetValue("RegisteredOwner", comboBox_registeredOwner.Text);
                MessageBox.Show("设置成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
            }
        }

        private void button_registeredOrganization_Click(object sender, EventArgs e)
        {
            try
            {
                Registry.
                    LocalMachine.
                        CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion").
                            SetValue("RegisteredOrganization", comboBox_registeredOrganization.Text);
                MessageBox.Show("设置成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
            }
        }

        #endregion

        #region 系统功能禁用

        // 系统功能禁用 通用功能 应用
        private void button_RegPoliciesExplorer_Set_Click(object sender, EventArgs e)
        {
            SetRegPoliciesExplorer();
            RestartExplorer();
            MessageBox.Show("操作完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 系统功能禁用 通用功能 保存
        private void button_RegPoliciesExplorer_Save_Click(object sender, EventArgs e)
        {
            SetRegPoliciesExplorer();
            MessageBox.Show("操作完成\n\n温馨提示：\n您点击的是保存而非应用，\n因此，需要您手动进行刷新使其生效~\n若您想要立即生效，请点击应用而非保存哦~", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 系统功能禁用 通用功能 核心获取函数
        public void GetRegPoliciesExplorer()
        {
            try
            {
                // 定义两个注册表路径的映射
                var registryPaths = new Dictionary<string, Dictionary<CheckBox, string>>
                {
                    // Explorer
                    {
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer",
                        new Dictionary<CheckBox, string>
                        {
                            { checkBox_NoRun, "NoRun" },
                            { checkBox_Nofind, "Nofind" },
                            { checkBox_NoClose, "NoClose" },
                            { checkBox_NoLogOff, "NoLogOff" },
                            { checkBox_NoDesktop, "NoDesktop" },
                            { checkBox_NoSetFolders, "NoSetFolders" },
                            { checkBox_NoControlPanel, "NoControlPanel" },
                            { checkBox_NoSaveSettings, "NoSaveSettings" },
                            { checkBox_NoFavoritesMenu, "NoFavoritesMenu" },
                            { checkBox_NoWindowsUpdate, "NoWindowsUpdate" },
                            { checkBox_NofolderOptions, "NofolderOptions" },
                            { checkBox_NoRecentDocsMenu, "NoRecentDocsMenu" },
                            { checkBox_NoViewContextMenu, "NoViewContextMenu" },
                            { checkBox_NoTrayContextMenu, "NoTrayContextMenu" },
                            { checkBox_NoSetActiveDesktop, "NoSetActiveDesktop" },
                            { checkBox_EditLevel, "EditLevel" },
                            { checkBox_NoNetHood, "NoNetHood" },
                            { checkBox_NoFileMenu, "NoFileMenu" },
                            { checkBox_NoAddPrinter, "NoAddPrinter" },
                            { checkBox_NoSetTaskbar, "NoSetTaskbar" },
                            { checkBox_NoStartBanner, "NoStartBanner" },
                            { checkBox_NoPrinterTabs, "NoPrinterTabs" },
                            { checkBox_NoInternetIcon, "NoInternetIcon" },
                            { checkBox_NoActiveDesktop, "NoActiveDesktop" },
                            { checkBox_NoDeletePrinter, "NoDeletePrinter" },
                            { checkBox_NoChangeStartMenu, "NoChangeStartMenu" },
                            { checkBox_NoRecentDocsHistory, "NoRecentDocsHistory" },
                            { checkBox_ClearRecentDocsonExit, "ClearRecentDocsonExit" }
                        }
                    },

                    // System
                    {
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
                        new Dictionary<CheckBox, string>
                        {
                            { checkBox_DisableTaskMgr, "DisableTaskMgr" },
                            { checkBox_DisableRegistryTools, "DisableRegistryTools" }
                        }
                    },

                    // System 2
                    {
                        @"SOFTWARE\Policies\Microsoft\Windows\System",
                        new Dictionary<CheckBox, string>
                        {
                            { checkBox_DisableCMD, "DisableCMD" }
                        }
                    },

                    // Programs
                    {
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Programs",
                        new Dictionary<CheckBox, string>
                        {
                            { checkBox_NoProgramsAndFeatures, "NoProgramsAndFeatures" },
                            { checkBox_NoUninstallFromStart_And_NoUninstallFromPrograms, "NoUninstallFromStart" },
                            { checkBox_NoProgramInstall, "NoProgramInstall" },
                            { checkBox_NoWindowsUpdatesPage, "NoWindowsUpdatesPage" }
                        }
                    }
                };

                string str = "";
                foreach (var pathMapping in registryPaths)
                {
                    string registryPath = pathMapping.Key;
                    var checkBoxMappings = pathMapping.Value;

                    using (RegistryKey reg = RegPoliciesExplorer_registry.OpenSubKey(registryPath))
                    {
                        if (reg == null) continue;

                        foreach (var mapping in checkBoxMappings)
                        {
                            try
                            {
                                CheckBox checkBox = mapping.Key;
                                string valueName = mapping.Value;

                                if (!reg.GetValueNames().Contains(valueName))
                                    continue;

                                RegistryValueKind valueKind = reg.GetValueKind(valueName);
                                switch (valueKind)
                                {
                                    case RegistryValueKind.DWord:
                                        checkBox.Checked = Convert.ToInt32(reg.GetValue(valueName, 0)) == 1;
                                        break;

                                    case RegistryValueKind.Binary:
                                        byte[] bytes = (byte[])reg.GetValue(valueName, new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                        checkBox.Checked = BitConverter.ToInt32(bytes, 0) == 1;
                                        break;

                                    default:
                                        continue;
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"读取注册表值 {mapping.Value} 失败: {ex.Message}");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取注册表时发生错误。\n错误信息：{ex.Message}", "错误",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 系统功能禁用 通用功能 核心修改函数
        public void SetRegPoliciesExplorer()
        {
            try
            {
                // 定义常量值
                const int ENABLED = 1;
                const int DISABLED = 0;
                byte[] value_1000 = [0x01, 0x00, 0x00, 0x00];
                byte[] value_0000 = [0x00, 0x00, 0x00, 0x00];

                // 使用 using 语句确保资源释放
                using (RegistryKey explorerKey = RegPoliciesExplorer_registry.CreateSubKey
                    (@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"))
                {
                    if (explorerKey != null)
                    {
                        // 使用字典映射控件和注册表值名称
                        var explorerMappings = new Dictionary<CheckBox, (string Name, object EnabledValue, object DisabledValue)>
                        {
                            { checkBox_NoRun, ("NoRun", ENABLED, DISABLED) },
                            { checkBox_Nofind, ("Nofind", ENABLED, DISABLED) },
                            { checkBox_NoClose, ("NoClose", ENABLED, DISABLED) },
                            { checkBox_NoLogOff, ("NoLogOff", ENABLED, DISABLED) },
                            { checkBox_NoDesktop, ("NoDesktop", ENABLED, DISABLED) },
                            { checkBox_NoSetFolders, ("NoSetFolders", ENABLED, DISABLED) },
                            { checkBox_NoControlPanel, ("NoControlPanel", ENABLED, DISABLED) },
                            { checkBox_NoSaveSettings, ("NoSaveSettings", ENABLED, DISABLED) },
                            { checkBox_NoFavoritesMenu, ("NoFavoritesMenu", ENABLED, DISABLED) },
                            { checkBox_NoWindowsUpdate, ("NoWindowsUpdate", ENABLED, DISABLED) },
                            { checkBox_NofolderOptions, ("NofolderOptions", ENABLED, DISABLED) },
                            { checkBox_NoRecentDocsMenu, ("NoRecentDocsMenu", ENABLED, DISABLED) },
                            { checkBox_NoViewContextMenu, ("NoViewContextMenu", ENABLED, DISABLED) },
                            { checkBox_NoTrayContextMenu, ("NoTrayContextMenu", ENABLED, DISABLED) },
                            { checkBox_NoSetActiveDesktop, ("NoSetActiveDesktop", ENABLED, DISABLED) },
                            { checkBox_NoRecentDocsHistory, ("NoRecentDocsHistory", value_1000, value_0000) },
                            { checkBox_ClearRecentDocsonExit, ("ClearRecentDocsonExit", value_1000, value_0000) },
                            { checkBox_EditLevel, ("EditLevel", ENABLED, DISABLED) },
                            { checkBox_NoNetHood, ("NoNetHood", ENABLED, DISABLED) },
                            { checkBox_NoFileMenu, ("NoFileMenu", ENABLED, DISABLED) },
                            { checkBox_NoAddPrinter, ("NoAddPrinter", ENABLED, DISABLED) },
                            { checkBox_NoSetTaskbar, ("NoSetTaskbar", ENABLED, DISABLED) },
                            { checkBox_NoStartBanner, ("NoStartBanner", ENABLED, DISABLED) },
                            { checkBox_NoPrinterTabs, ("NoPrinterTabs", ENABLED, DISABLED) },
                            { checkBox_NoInternetIcon, ("NoInternetIcon", ENABLED, DISABLED) },
                            { checkBox_NoActiveDesktop, ("NoActiveDesktop", ENABLED, DISABLED) },
                            { checkBox_NoDeletePrinter, ("NoDeletePrinter", ENABLED, DISABLED) },
                            { checkBox_NoChangeStartMenu, ("NoChangeStartMenu", ENABLED, DISABLED) }
                        };

                        // 批量处理注册表设置
                        foreach (var mapping in explorerMappings)
                        {
                            object value = mapping.Key.Checked ? mapping.Value.EnabledValue : mapping.Value.DisabledValue;
                            explorerKey.SetValue(mapping.Value.Name, value);
                        }
                    }
                }

                // System 1 策略
                using (RegistryKey systemKey1 = RegPoliciesExplorer_registry.CreateSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"))
                {
                    if (systemKey1 != null)
                    {
                        var systemMappings1 = new Dictionary<CheckBox, string>
                        {
                            { checkBox_DisableTaskMgr, "DisableTaskMgr" },
                            { checkBox_DisableRegistryTools, "DisableRegistryTools" }
                        };

                        foreach (var mapping in systemMappings1)
                        {
                            systemKey1.SetValue(mapping.Value, mapping.Key.Checked ? ENABLED : DISABLED);
                        }
                    }
                }

                // System 2 策略
                using (RegistryKey systemKey2 = RegPoliciesExplorer_registry.CreateSubKey(
                    @"SOFTWARE\Policies\Microsoft\Windows\System"))
                {
                    if (systemKey2 != null)
                    {
                        systemKey2.SetValue("DisableCMD", checkBox_DisableCMD.Checked ? ENABLED : DISABLED);
                    }
                }

                // Programs 策略
                using (RegistryKey systemKey2 = RegPoliciesExplorer_registry.CreateSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Programs"))
                {
                    if (systemKey2 != null)
                    {
                        systemKey2.SetValue("NoProgramsAndFeatures", checkBox_NoProgramsAndFeatures.Checked ? ENABLED : DISABLED);
                        systemKey2.SetValue("NoUninstallFromStart", checkBox_NoUninstallFromStart_And_NoUninstallFromPrograms.Checked ? ENABLED : DISABLED);
                        systemKey2.SetValue("NoUninstallFromPrograms", checkBox_NoUninstallFromStart_And_NoUninstallFromPrograms.Checked ? ENABLED : DISABLED);
                        systemKey2.SetValue("NoProgramInstall", checkBox_NoProgramInstall.Checked ? ENABLED : DISABLED);
                        systemKey2.SetValue("NoWindowsUpdatesPage", checkBox_NoWindowsUpdatesPage.Checked ? ENABLED : DISABLED);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"写入注册表时发生错误。\n错误信息：{ex.Message}",
                    "齐的工具包3", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 系统功能禁用 通用功能 外部调用专用获取函数
        public static string Ext_GetRegPoliciesExplorer(int RegType = 0)
        {
            string DisableList = "";
            RegistryKey Reg = RegType == 0 ? Registry.LocalMachine : Registry.CurrentUser;

            try
            {
                // 定义注册表路径和对应的值名称
                var registryPaths = new Dictionary<string, List<string>>
                {
                    // Explorer
                    {
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer",
                        new List<string>
                        {
                            "NoRun", "Nofind", "NoClose", "NoLogOff", "NoDesktop", "NoSetFolders",
                            "NoControlPanel", "NoSaveSettings", "NoFavoritesMenu", "NoWindowsUpdate",
                            "NofolderOptions", "NoRecentDocsMenu", "NoViewContextMenu", "NoTrayContextMenu",
                            "NoSetActiveDesktop", "EditLevel", "NoNetHood", "NoFileMenu", "NoAddPrinter",
                            "NoSetTaskbar", "NoStartBanner", "NoPrinterTabs", "NoInternetIcon",
                            "NoActiveDesktop", "NoDeletePrinter", "NoChangeStartMenu", "NoRecentDocsHistory",
                            "ClearRecentDocsonExit"
                        }
                    },

                    // System
                    {
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
                        new List<string>
                        {
                            "DisableTaskMgr", "DisableRegistryTools"
                        }
                    },

                    // System 2
                    {
                        @"SOFTWARE\Policies\Microsoft\Windows\System",
                        new List<string>
                        {
                            "DisableCMD"
                        }
                    },

                    // Programs
                    {
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Programs",
                        new List<string>
                        {
                            "NoProgramsAndFeatures", "NoUninstallFromStart", "NoProgramInstall", "NoWindowsUpdatesPage"
                        }
                    }
                };

                foreach (var pathMapping in registryPaths)
                {
                    string registryPath = pathMapping.Key;
                    var valueNames = pathMapping.Value;

                    using (RegistryKey reg = Reg.OpenSubKey(registryPath))
                    {
                        if (reg == null) continue;

                        foreach (string valueName in valueNames)
                        {
                            try
                            {
                                if (!reg.GetValueNames().Contains(valueName))
                                    continue;

                                RegistryValueKind valueKind = reg.GetValueKind(valueName);
                                switch (valueKind)
                                {
                                    case RegistryValueKind.DWord:
                                        if (Convert.ToInt32(reg.GetValue(valueName, 0)) == 1)
                                        {
                                            DisableList += valueName + ' ';
                                        }
                                        break;

                                    case RegistryValueKind.Binary:
                                        byte[] bytes = (byte[])reg.GetValue(valueName, new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                        if (BitConverter.ToInt32(bytes, 0) == 1)
                                        {
                                            DisableList += valueName + ' ';
                                        }
                                        break;

                                    default:
                                        continue;
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"读取注册表值 {valueName} 失败: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 外部调用也可以选择不显示消息框，而是写入日志
                Debug.WriteLine($"读取注册表时发生错误。\n错误信息：{ex.Message}");

                // 如果您仍然需要消息框，可以保留
                MessageBox.Show($"读取注册表时发生错误。\n错误信息：{ex.Message}", "错误",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return DisableList.Trim(); // 去除末尾可能多余的空格
        }

        // 系统功能禁用 通用功能 外部调用专用修改函数
        public static void Ext_SetRegPoliciesExplorer(bool enable = false, int RegType = 0)
        {
            RegistryKey Reg = RegType == 0 ? Registry.LocalMachine : Registry.CurrentUser;

            try
            {
                // 定义常量值
                const int ENABLED = 1;
                const int DISABLED = 0;
                byte[] value_1000 = [0x01, 0x00, 0x00, 0x00];
                byte[] value_0000 = [0x00, 0x00, 0x00, 0x00];

                // 定义注册表路径和对应的值名称（与Get版本保持一致）
                var registryPaths = new Dictionary<string, List<string>>
                {
                    // Explorer
                    {
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer",
                        new List<string>
                        {
                            "NoRun", "Nofind", "NoClose", "NoLogOff", "NoDesktop", "NoSetFolders",
                            "NoControlPanel", "NoSaveSettings", "NoFavoritesMenu", "NoWindowsUpdate",
                            "NofolderOptions", "NoRecentDocsMenu", "NoViewContextMenu", "NoTrayContextMenu",
                            "NoSetActiveDesktop", "EditLevel", "NoNetHood", "NoFileMenu", "NoAddPrinter",
                            "NoSetTaskbar", "NoStartBanner", "NoPrinterTabs", "NoInternetIcon",
                            "NoActiveDesktop", "NoDeletePrinter", "NoChangeStartMenu"
                        }
                    },

                    // 需要特殊处理二进制值的项
                    {
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer_Binary",
                        new List<string>
                        {
                            "NoRecentDocsHistory", "ClearRecentDocsonExit"
                        }
                    },

                    // System
                    {
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
                        new List<string>
                        {
                            "DisableTaskMgr", "DisableRegistryTools"
                        }
                    },

                    // System 2
                    {
                        @"SOFTWARE\Policies\Microsoft\Windows\System",
                        new List<string>
                        {
                            "DisableCMD"
                        }
                    },

                    // Programs
                    {
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Programs",
                        new List<string>
                        {
                            "NoProgramsAndFeatures", "NoUninstallFromStart", "NoUninstallFromPrograms",
                            "NoProgramInstall", "NoWindowsUpdatesPage"
                        }
                    }
                };

                using (RegistryKey baseKey = Reg) // 或者 Registry.LocalMachine
                {
                    foreach (var pathMapping in registryPaths)
                    {
                        string registryPath = pathMapping.Key;
                        var valueNames = pathMapping.Value;

                        // 处理普通路径
                        if (!registryPath.EndsWith("_Binary"))
                        {
                            using (RegistryKey reg = baseKey.CreateSubKey(registryPath))
                            {
                                if (reg != null)
                                {
                                    foreach (string valueName in valueNames)
                                    {
                                        reg.SetValue(valueName, enable ? ENABLED : DISABLED);
                                        Debug.WriteLine($"设置: {registryPath}\\{valueName} = {(enable ? "启用" : "禁用")}");
                                    }
                                }
                            }
                        }
                        // 处理需要二进制值的项
                        else
                        {
                            string actualPath = registryPath.Replace("_Binary", "");
                            using (RegistryKey reg = baseKey.CreateSubKey(actualPath))
                            {
                                if (reg != null)
                                {
                                    foreach (string valueName in valueNames)
                                    {
                                        reg.SetValue(valueName, enable ? value_1000 : value_0000);
                                        Debug.WriteLine($"设置: {actualPath}\\{valueName} = {(enable ? "启用(二进制)" : "禁用(二进制)")}");
                                    }
                                }
                            }
                        }
                    }
                }

                Debug.WriteLine($"外部调用设置完成: 强制所有策略 {(enable ? "启用" : "禁用")}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"写入注册表时发生错误。\n错误信息：{ex.Message}");
                MessageBox.Show($"写入注册表时发生错误。\n错误信息：{ex.Message}",
                    "外部调用", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void button_RegPoliciesExplorer_Get_Click(object sender, EventArgs e)
        {
            GetRegPoliciesExplorer();
        }

        private void button_RegWinDefender_Get_Click(object sender, EventArgs e)
        {
            try
            {
                // 定义注册表路径与对应的CheckBox名前缀映射
                var registryPaths = new Dictionary<string, string>
                {
                    { @"SOFTWARE\Policies\Microsoft\Windows Defender", "checkBox_WinDefender_" },
                    { @"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "checkBox_WinDefender_" }
                };

                // 特殊处理DisableRealtimeMonitoring2
                var specialMappings = new Dictionary<string, string>
                {
                    { "DisableRealtimeMonitoring", "DisableRealtimeMonitoring2" }
                };

                using (RegistryKey localMachine = Registry.LocalMachine)
                {
                    foreach (var path in registryPaths)
                    {
                        using (RegistryKey defenderKey = localMachine.OpenSubKey(path.Key))
                        {
                            if (defenderKey == null) continue;

                            foreach (string valueName in defenderKey.GetValueNames())
                            {
                                // 跳过非DWORD值
                                if (defenderKey.GetValueKind(valueName) != RegistryValueKind.DWord)
                                    continue;

                                // 处理特殊映射
                                string controlName = path.Value +
                                    (specialMappings.ContainsKey(valueName) ?
                                    specialMappings[valueName] :
                                    valueName);

                                // 查找对应的CheckBox控件
                                Control[] controls = this.Controls.Find(controlName, true);
                                if (controls.Length > 0 && controls[0] is CheckBox checkBox)
                                {
                                    int? value = (int?)defenderKey.GetValue(valueName);
                                    checkBox.Checked = value == 1;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载Windows Defender设置时出错: {ex.Message}", "错误",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_RegWinDefender_Save_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender");

                // 主设置
                reg.SetValue("DisableAntiVirus", checkBox_WinDefender_DisableAntiVirus.Checked ? 1 : 0);
                reg.SetValue("DisableAntiSpyware", checkBox_WinDefender_DisableAntiSpyware.Checked ? 1 : 0);
                reg.SetValue("DisableRealtimeMonitoring", checkBox_WinDefender_DisableRealtimeMonitoring.Checked ? 1 : 0);
                reg.SetValue("DisableSpecialRunningModes", checkBox_WinDefender_DisableSpecialRunningModes.Checked ? 1 : 0);
                reg.SetValue("DisableRoutinelyTakingAction", checkBox_WinDefender_DisableRoutinelyTakingAction.Checked ? 1 : 0);

                // 实时保护
                reg.SetValue("DisableBehaviorMonitoring", checkBox_WinDefender_DisableBehaviorMonitoring.Checked ? 1 : 0);
                reg.SetValue("DisableRealtimeMonitoring", checkBox_WinDefender_DisableRealtimeMonitoring2.Checked ? 1 : 0);
                reg.SetValue("DisableOnAccessProtection", checkBox_WinDefender_DisableOnAccessProtection.Checked ? 1 : 0);
                reg.SetValue("DisableScanOnRealtimeEnable", checkBox_WinDefender_DisableScanOnRealtimeEnable.Checked ? 1 : 0);

                // SpyNet
                reg.SetValue("DisableBlockAtFirstSeen", checkBox_WinDefender_DisableBlockAtFirstSeen.Checked ? 1 : 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存设置时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MessageBox.Show("操作已完成！\n请重启电脑或注销使全部功能生效~", "齐的工具包3", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button_WindowsEditionID_Set_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_WindowsEditionID.Text))
                temp = MessageBox.Show
                (
                    "您要修改的Windows 版本标识的值为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;


            temp = MessageBox.Show
            (
                "修改 EditionID 风险较大\n潜在问题：" +
                "\n\n激活失效：Windows 激活服务（KMS/数字许可）会检测真实的系统版本，修改注册表可能导致激活失败。" +
                "\n\n功能异常：某些需要企业版组件的功能（如组策略高级选项）可能无法正常工作，因为系统实际并未安装对应的二进制文件。" +
                "\n\n软件兼容性：依赖 EditionID 的软件（如虚拟机工具、安全软件）可能报错或拒绝运行。" +
                "\n\n更新错误：Windows Update 可能尝试下载与企业版 LTSC 相关的更新，但无法安装（因系统文件不匹配）。" +
                "\n\n\n此工具推荐在特殊情况，例如您想要将您的Windows升级成LTSC，又想保留数据时，可以先将标识修改为LTSC的标识符（EnterpriseS），" +
                "这样就可以在保留数据的情况下进行系统升级了。",
                "风险警告",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information
            );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "最后再次警告，您确定要修改吗？",
                "风险警告",
                MessageBoxButtons.OKCancel
            );

            if (temp != DialogResult.OK)
                return;

            try
            {
                RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                registryKey.SetValue("EditionID", comboBox_WindowsEditionID.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show
                (
                    $"修改时发生错误！\n错误信息：{ex.Message}\n\n完整报错：{ex}",
                    "齐的工具包3",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                MessageBox.Show
                (
                    $"修改成功！",
                    "齐的工具包3",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void comboBox_RegPoliciesExplorer_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            RegPoliciesExplorer_registry = comboBox_RegPoliciesExplorer_Type.SelectedIndex == 0 ?
                Registry.CurrentUser : Registry.LocalMachine;

            // 获取数据
            button_RegPoliciesExplorer_Get_Click(null, null);
        }

        private void button_UAC_Read_Click(object sender, EventArgs e)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"))
                {
                    if (key != null)
                    {
                        comboBox_EnableLUA.SelectedIndex = GetUACRegData(key, "EnableLUA", 0, 1);
                        comboBox_ConsentPromptBehaviorAdmin.SelectedIndex = GetUACRegData(key, "ConsentPromptBehaviorAdmin", 0, 5);
                        comboBox_ConsentPromptBehaviorUser.SelectedIndex = GetUACRegData(key, "ConsentPromptBehaviorUser", 0, 3);
                        comboBox_PromptOnSecureDesktop.SelectedIndex = GetUACRegData(key, "PromptOnSecureDesktop", 0, 1);
                        comboBox_FilterAdministratorToken.SelectedIndex = GetUACRegData(key, "FilterAdministratorToken", 0, 1);
                    }
                    else
                    {
                        MessageBox.Show("未找到系统注册表项，请检查运行环境！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取注册表时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetUACRegData(RegistryKey key, string Value, int min, int max)
        {
            return Math.Clamp(StrToInt(key.GetValue(Value).ToString()), min, max);
        }

        private void button_UAC_Save_Click(object sender, EventArgs e)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", true))
                {
                    if (key != null)
                    {
                        key.SetValue("EnableLUA", comboBox_EnableLUA.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("ConsentPromptBehaviorAdmin", comboBox_ConsentPromptBehaviorAdmin.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("ConsentPromptBehaviorUser", comboBox_ConsentPromptBehaviorUser.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("PromptOnSecureDesktop", comboBox_PromptOnSecureDesktop.SelectedIndex, RegistryValueKind.DWord);
                        key.SetValue("FilterAdministratorToken", comboBox_FilterAdministratorToken.SelectedIndex, RegistryValueKind.DWord);
                        MessageBox.Show("保存成功。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("未找到系统注册表项，请检查运行环境！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"写入注册表时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_GoMyComputerNameSpaceTool_Click(object sender, EventArgs e) =>
            new MyComputerNameSpaceTool().Show();

        private void tabPageCMD_Click(object sender, EventArgs e)
        {

        }

        private void button_UltimatePerformancePowerPlan_Enable_Click(object sender, EventArgs e)
        {
            PowerManager.ShowUltimatePerformanceScheme();
        }

        private void button_UltimatePerformancePowerPlan_Disabled_Click(object sender, EventArgs e)
        {
            var items = PowerManager.GetAllUltimatePerformanceSchemes();
            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    PowerManager.DeleteScheme(item);
                }
                MessageBox.Show("卸载完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                MessageBox.Show(
                    "无法获取相关数据。\n\n" +
                    "请手动前往\"控制面板 → 电源选项\"进行删除\n" +
                    "或尝试使用下方手动删除功能进行删除。" +
                    "又或者索性点击重置按钮，完全重置电源计划", "提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }





        private void radioButton_ActiveScheme_0_CheckedChanged(object sender, EventArgs e)
        {
            if (Loading) return;

            PowerManager.SetActiveScheme(PowerSchemes.Balanced);
        }

        private void radioButton_ActiveScheme_1_CheckedChanged(object sender, EventArgs e)
        {
            if (Loading) return;

            PowerManager.SetActiveScheme(PowerSchemes.HighPerformance);
        }

        private void radioButton_ActiveScheme_2_CheckedChanged(object sender, EventArgs e)
        {
            if (Loading) return;

            PowerManager.SetActiveScheme(PowerSchemes.PowerSaver);
        }

        private void radioButton_ActiveScheme_3_CheckedChanged(object sender, EventArgs e)
        {
            //var msg = MessageBox.Show(
            //    "请注意！请", "警告"
            //);

            //if (msg == DialogResult.OK)
            //    PowerManager.SetActiveScheme(PowerSchemes.UltimatePerformance2);
        }

        private void button_PowerGUID_Set_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBox_PowerGUID.Text))
            {
                var IsDone = PowerManager.SetActiveScheme(new Guid(comboBox_PowerGUID.Text));

                LoadPowerData();
                if (IsDone) MessageBox.Show("已切换至该方案。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button_PowerGUID_Delete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBox_PowerGUID.Text))
            {
                var IsDone = PowerManager.DeleteScheme(new Guid(comboBox_PowerGUID.Text));

                LoadPowerData();
                if (IsDone) MessageBox.Show("已删除该方案。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button_PowerGUID_ReLoad_Click(object sender, EventArgs e)
        {
            LoadPowerData();
        }

        private void button_Power_Reset_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "powercfg.exe",
                    Arguments = $"-restoredefaultschemes",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                }
                Log.Info($"[高级系统修改工具] 完成调用 powercfg 重置电源计划");
                MessageBox.Show("已完成调用 powercfg 重置电源计划", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception ex)
            {
                Log.Err($"[高级系统修改工具] 调用 powercfg 时出现错误：{ex.Message}");
            }
        }

        #region 设备伪改名


        // 获取GPU设备实际路径
        public static string GetGPUPNPDeviceID()
        {
            try
            {
                // 过滤掉 ROOT 开头的虚拟设备路径
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    "SELECT PNPDeviceID FROM Win32_VideoController WHERE NOT PNPDeviceID LIKE 'ROOT%'"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string pnpDeviceId = obj["PNPDeviceID"]?.ToString();
                        if (!string.IsNullOrEmpty(pnpDeviceId))
                        {
                            // 返回第一个有效的物理设备路径
                            Log.Info($"[高级系统修改工具] [设备伪改名] 获取 GPU 设备路径: {pnpDeviceId}");
                            return pnpDeviceId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录异常以便调试
                Log.Err($"[高级系统修改工具] [设备伪改名] 获取 GPU 路径失败: {ex.Message}");
            }

            return null;
        }

        private void buttonGetCpuName_Click(object sender, EventArgs e)
        {
            try
            {
                comboBoxCpuName.Text = Registry.LocalMachine
                    .CreateSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0")
                    .GetValue("ProcessorNameString")
                    .ToString();
            }
            catch (Exception ex)
            {
                Log.Err($"[高级系统修改工具] [硬件伪改名] CPU名称 获取失败，错误原因：{ex.Message}");
                MessageBox.Show("获取失败！错误原因：" + ex.Message, "报错", MessageBoxButtons.OK);
            }
        }

        private void buttonSetCpuName_Click(object sender, EventArgs e)
        {
            try
            {
                Registry.LocalMachine
                    .CreateSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0")
                    .SetValue("ProcessorNameString", comboBoxCpuName.Text);

                Log.Info($"[高级系统修改工具] [硬件伪改名] CPU名已改为 {comboBoxCpuName.Text}");
                MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                Log.Err($"[高级系统修改工具] [硬件伪改名] CPU名称修改失败，错误原因：{ex.Message}");
                MessageBox.Show("修改失败！错误原因：" + ex.Message, "报错", MessageBoxButtons.OK);
            }
        }

        private void button_GpuName_Get_Click(object sender, EventArgs e)
        {
            try
            {
                string devicePath = comboBox_GPUPNPDeviceID.Text.Trim();

                using (RegistryKey deviceKey = Registry.LocalMachine
                    .OpenSubKey($@"SYSTEM\CurrentControlSet\Enum\{devicePath}", true))
                {
                    if (deviceKey != null)
                    {
                        if (deviceKey.GetValue("FriendlyName") != null)
                            comboBox_GpuName.Text = deviceKey.GetValue("FriendlyName").ToString();

                        else if (deviceKey.GetValue("DeviceDesc") != null)
                            comboBox_GpuName.Text = deviceKey.GetValue("DeviceDesc").ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Err($"[高级系统修改工具] [硬件伪改名] GPU名称 获取失败，错误原因：{ex.Message}");
                MessageBox.Show("获取失败！错误原因：" + ex.Message, "报错", MessageBoxButtons.OK);
            }
        }

        private void button_GpuName_SetFriendlyName_Click(object sender, EventArgs e)
        {
            try
            {
                string devicePath = comboBox_GPUPNPDeviceID.Text.Trim();

                using (RegistryKey deviceKey = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Enum\{devicePath}", true))
                {
                    if (deviceKey != null)
                    {
                        deviceKey.SetValue("FriendlyName", comboBox_GpuName.Text);
                        Log.Info($"[高级系统修改工具] [硬件伪改名] GPU FriendlyName 已修改为：{comboBox_GpuName.Text}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Err($"[高级系统修改工具] [硬件伪改名] 修改 GPU FriendlyName 失败，错误原因：{ex.Message}");
                MessageBox.Show("修改失败！错误原因：" + ex.Message, "报错", MessageBoxButtons.OK);
            }
        }

        private void button_GpuName_DeleteFriendlyName_Click(object sender, EventArgs e)
        {
            try
            {
                string devicePath = comboBox_GPUPNPDeviceID.Text.Trim();

                using (RegistryKey deviceKey = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Enum\{devicePath}", true))
                {
                    if (deviceKey != null)
                        if (deviceKey.GetValue("FriendlyName") != null)
                            deviceKey.DeleteValue("FriendlyName");
                    Log.Info($"[高级系统修改工具] [硬件伪改名] GPU FriendlyName 已被删除。");
                }
            }
            catch (Exception ex)
            {
                Log.Err($"[高级系统修改工具] [硬件伪改名] 删除 GPU FriendlyName 失败，错误原因：{ex.Message}");
                MessageBox.Show("删除失败！错误原因：" + ex.Message, "报错", MessageBoxButtons.OK);
            }
        }

        private void button_GpuName_SetDeviceDesc_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(
                    "警告！此操作会永久改变GPU描述且不可逆！千万谨慎使用！",
                    "警告！", MessageBoxButtons.YesNo, MessageBoxIcon.Warning
                ) == DialogResult.No
            ) return;

            if (
                MessageBox.Show(
                    "再次警告！此操作会永久改变GPU描述且不可逆！千万谨慎使用！",
                    "再次警告！", MessageBoxButtons.YesNo, MessageBoxIcon.Warning
                ) == DialogResult.No
            ) return;

            try
            {
                string devicePath = comboBox_GPUPNPDeviceID.Text.Trim();

                using (RegistryKey deviceKey = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Enum\{devicePath}", true))
                {
                    if (deviceKey != null)
                        deviceKey.SetValue("DeviceDesc", comboBox_GpuName.Text);
                    Log.Info($"[高级系统修改工具] [硬件伪改名] GPU DeviceDesc 已修改为：{comboBox_GpuName.Text}");
                }
            }
            catch (Exception ex)
            {
                Log.Err($"[高级系统修改工具] [硬件伪改名] 修改 GPU DeviceDesc 失败，错误原因：{ex.Message}");
                MessageBox.Show("修改失败！错误原因：" + ex.Message, "报错", MessageBoxButtons.OK);
            }
        }

        #endregion


        public class PasswordInfo
        {
            // 基本信息
            public string UserName { get; set; }
            public string FullName { get; set; }
            public string Description { get; set; }

            // 密码时间信息
            public DateTime? PasswordLastSet { get; set; }
            public DateTime? PasswordExpires { get; set; }
            public DateTime? LastLogin { get; set; }
            public DateTime? LastBadLoginAttempt { get; set; }
            public DateTime? AccountExpirationDate { get; set; }

            // 密码状态标志
            public bool PasswordNeverExpires { get; set; }
            public bool UserCannotChangePassword { get; set; }
            public bool PasswordExpired { get; set; }
            public bool PasswordRequired { get; set; }

            // 账户状态
            public bool AccountEnabled { get; set; }
            public bool AccountLocked { get; set; }
            public int BadPasswordCount { get; set; }
            public int BadLogonCount { get; set; }

            // 策略信息（全局）
            public int? MaxPasswordAge { get; set; }  // 密码最长使用期限（天）
            public int? MinPasswordAge { get; set; }   // 密码最短使用期限（天）
            public int? PasswordHistoryLength { get; set; }  // 密码历史记录长度
            public int? LockoutThreshold { get; set; }  // 锁定阈值
            public int? LockoutDuration { get; set; }   // 锁定持续时间（分钟）

            // 格式化的显示字符串
            public override string ToString()
            {
                return $@"
═══════════════════════════════════════════════════════════
用户密码详细信息 - {UserName}
═══════════════════════════════════════════════════════════

【基本信息】
  用户名: {UserName}
  全名: {FullName ?? "未设置"}
  描述: {Description ?? "未设置"}

【密码时间信息】
  密码最后设置时间: {(PasswordLastSet.HasValue ? PasswordLastSet.Value.ToString("yyyy-MM-dd HH:mm:ss") : "未知")}
  密码过期时间: {(PasswordExpires.HasValue ? PasswordExpires.Value.ToString("yyyy-MM-dd HH:mm:ss") : "未设置/永不过期")}
  最后登录时间: {(LastLogin.HasValue ? LastLogin.Value.ToString("yyyy-MM-dd HH:mm:ss") : "从未登录")}
  最后失败登录: {(LastBadLoginAttempt.HasValue ? LastBadLoginAttempt.Value.ToString("yyyy-MM-dd HH:mm:ss") : "无失败记录")}
  账户过期时间: {(AccountExpirationDate.HasValue ? AccountExpirationDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "永不过期")}

【密码状态】
  密码永不过期: {(PasswordNeverExpires ? "✅ 是" : "❌ 否")}
  用户不能改密码: {(UserCannotChangePassword ? "✅ 是" : "❌ 否")}
  密码已过期: {(PasswordExpired ? "✅ 是" : "❌ 否")}
  密码必须存在: {(PasswordRequired ? "✅ 是" : "❌ 否")}

【账户状态】
  账户已启用: {(AccountEnabled ? "✅ 是" : "❌ 否")}
  账户已锁定: {(AccountLocked ? "✅ 是" : "❌ 否")}
  错误密码次数: {BadPasswordCount}
  失败登录次数: {BadLogonCount}

【全局密码策略】
  密码最长有效期: {MaxPasswordAge?.ToString() ?? "未获取"} 天
  密码最短有效期: {MinPasswordAge?.ToString() ?? "未获取"} 天
  密码历史记录: {PasswordHistoryLength?.ToString() ?? "未获取"} 个
  账户锁定阈值: {LockoutThreshold?.ToString() ?? "未获取"} 次失败尝试
  账户锁定时间: {LockoutDuration?.ToString() ?? "未获取"} 分钟
═══════════════════════════════════════════════════════════";
            }
        }

        private static PrincipalContext GetContext(string machineName = null)
        {
            // 本地计算机上下文
            return new PrincipalContext(ContextType.Machine, machineName ?? Environment.MachineName);
        }

        /// <summary>
        /// 管理员强制重置密码（无需知道旧密码）
        /// </summary>
        public static bool ResetPassword(string userName, string newPassword)
        {
            try
            {
                using (var context = GetContext())
                {
                    using (var user = UserPrincipal.FindByIdentity(context, userName))
                    {
                        if (user == null)
                            return false;

                        // 直接设置新密码
                        user.SetPassword(newPassword);
                        user.Save();

                        if (newPassword == string.Empty)
                            Log.Info($"[高级系统修改工具] [PassWord] 修改密码成功！新密码为空。");
                        else
                            Log.Info($"[高级系统修改工具] [PassWord] 修改密码成功！新密码: {newPassword}");

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Err($"[高级系统修改工具] [PassWord] 修改密码失败: {ex.Message}");
                MessageBox.Show($"修改密码失败: {ex.Message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>
        /// 强制用户在下次登录时修改密码
        /// </summary>
        public static bool ForcePasswordChangeAtNextLogon(string userName)
        {
            using (var context = GetContext())
            {
                using (var user = UserPrincipal.FindByIdentity(context, userName))
                {
                    if (user == null)
                        return false;

                    // 强制密码过期
                    user.ExpirePasswordNow();
                    user.Save();
                    return true;
                }
            }
        }

        /// <summary>
        /// 设置密码永不过期
        /// </summary>
        public bool SetPasswordNeverExpires(string userName, bool neverExpires)
        {
            using (var context = GetContext())
            {
                using (var user = UserPrincipal.FindByIdentity(context, userName))
                {
                    if (user == null)
                        return false;

                    // PasswordNeverExpires 属性
                    user.PasswordNeverExpires = neverExpires;
                    user.Save();
                    return true;
                }
            }
        }

        /// <summary>
        /// 启用/禁用用户
        /// </summary>
        public bool SetUserEnabled(string userName, bool enabled)
        {
            using (var context = GetContext())
            {
                using (var user = UserPrincipal.FindByIdentity(context, userName))
                {
                    if (user == null)
                        return false;

                    user.Enabled = enabled;
                    user.Save();
                    return true;
                }
            }
        }

        /// <summary>
        /// 解锁被锁定的用户
        /// </summary>
        public bool UnlockUser(string userName)
        {
            using (var context = GetContext())
            {
                using (var user = UserPrincipal.FindByIdentity(context, userName))
                {
                    if (user == null)
                        return false;

                    user.UnlockAccount();
                    user.Save();
                    return true;
                }
            }
        }

        /// <summary>
        /// 获取密码信息
        /// </summary>
        public PasswordInfo GetPasswordInfo(string userName)
        {
            var info = new PasswordInfo();
            info.UserName = userName;

            // 方法1：使用 AccountManagement API（获取基础信息）
            GetInfoFromAccountManagement(userName, info);

            // 方法2：使用 DirectoryEntry API（获取更详细的属性）
            GetInfoFromDirectoryEntry(userName, info);

            // 方法3：获取全局密码策略
            GetPasswordPolicy(info);

            // 计算密码过期时间
            CalculatePasswordExpiry(info);

            return info;
        }

        /// <summary>
        /// 使用 AccountManagement API 获取信息
        /// </summary>
        private void GetInfoFromAccountManagement(string userName, PasswordInfo info)
        {
            try
            {
                using (var context = new PrincipalContext(ContextType.Machine, _machineName))
                {
                    using (var user = UserPrincipal.FindByIdentity(context, userName))
                    {
                        if (user == null) return;

                        // 基本信息
                        info.FullName = user.DisplayName;
                        info.Description = user.Description;

                        // 密码时间信息
                        info.PasswordLastSet = user.LastPasswordSet;
                        info.LastLogin = user.LastLogon;
                        info.AccountExpirationDate = user.AccountExpirationDate;

                        // 密码状态
                        info.PasswordNeverExpires = user.PasswordNeverExpires;
                        info.UserCannotChangePassword = user.UserCannotChangePassword;

                        // 账户状态
                        info.AccountEnabled = user.Enabled ?? false;

                        // 检查账户是否锁定
                        if (user.IsAccountLockedOut())
                        {
                            info.AccountLocked = true;
                        }

                        // 检查密码是否过期
                        if (user.LastPasswordSet.HasValue)
                        {
                            var daysSinceLastSet = (DateTime.Now - user.LastPasswordSet.Value).TotalDays;
                            // 如果没有设置永不过期，且超过42天（Windows默认），则认为过期
                            if (!info.PasswordNeverExpires && daysSinceLastSet > 42)
                            {
                                info.PasswordExpired = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Err($"[高级系统修改工具] [PassWord] AccountManagement 获取信息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 使用 DirectoryEntry API 获取更详细的信息
        /// </summary>
        private void GetInfoFromDirectoryEntry(string userName, PasswordInfo info)
        {
            try
            {
                string path = $"WinNT://{_machineName}/{userName},user";
                using (DirectoryEntry user = new DirectoryEntry(path))
                {
                    // 获取原生属性
                    var nativeUser = user.NativeObject;

                    // 密码相关属性
                    info.PasswordRequired = (int)user.Properties["PasswordRequired"].Value == 1;

                    // 失败登录信息
                    if (user.Properties["BadPasswordCount"].Value != null)
                        info.BadPasswordCount = (int)user.Properties["BadPasswordCount"].Value;

                    if (user.Properties["BadLoginCount"].Value != null)
                        info.BadLogonCount = (int)user.Properties["BadLoginCount"].Value;

                    // 最后失败登录时间
                    if (user.Properties["LastBadLoginAttempt"].Value != null)
                        info.LastBadLoginAttempt = (DateTime)user.Properties["LastBadLoginAttempt"].Value;

                    // 账户状态标志（UserFlags）
                    if (user.Properties["UserFlags"].Value != null)
                    {
                        int userFlags = (int)user.Properties["UserFlags"].Value;

                        // UF_DONT_EXPIRE_PASSWD = 0x10000
                        info.PasswordNeverExpires = (userFlags & 0x10000) != 0;

                        // UF_PASSWD_CANT_CHANGE = 0x40
                        info.UserCannotChangePassword = (userFlags & 0x40) != 0;

                        // UF_ACCOUNTDISABLE = 0x02
                        info.AccountEnabled = (userFlags & 0x02) == 0;

                        // UF_LOCKOUT = 0x10
                        info.AccountLocked = (userFlags & 0x10) != 0;

                        // UF_PASSWD_NOTREQD = 0x20
                        info.PasswordRequired = (userFlags & 0x20) == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Err($"[高级系统修改工具] [PassWord] DirectoryEntry 获取信息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取全局密码策略
        /// </summary>
        private void GetPasswordPolicy(PasswordInfo info)
        {
            try
            {
                string path = $"WinNT://{_machineName},computer";
                using (DirectoryEntry computer = new DirectoryEntry(path))
                {
                    // 密码最长使用期限（秒）
                    if (computer.Properties["MaxPasswordAge"].Value != null)
                    {
                        long maxAgeSeconds = (long)computer.Properties["MaxPasswordAge"].Value;
                        info.MaxPasswordAge = (int)(maxAgeSeconds / 86400); // 转换为天
                    }

                    // 密码最短使用期限（秒）
                    if (computer.Properties["MinPasswordAge"].Value != null)
                    {
                        long minAgeSeconds = (long)computer.Properties["MinPasswordAge"].Value;
                        info.MinPasswordAge = (int)(minAgeSeconds / 86400);
                    }

                    // 密码历史记录长度
                    if (computer.Properties["PasswordHistoryLength"].Value != null)
                    {
                        info.PasswordHistoryLength = (int)computer.Properties["PasswordHistoryLength"].Value;
                    }

                    // 账户锁定阈值
                    if (computer.Properties["LockoutThreshold"].Value != null)
                    {
                        info.LockoutThreshold = (int)computer.Properties["LockoutThreshold"].Value;
                    }

                    // 账户锁定持续时间（秒）
                    if (computer.Properties["LockoutDuration"].Value != null)
                    {
                        long lockoutSeconds = (long)computer.Properties["LockoutDuration"].Value;
                        info.LockoutDuration = (int)(lockoutSeconds / 60); // 转换为分钟
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Err($"[高级系统修改工具] [PassWord] 获取密码策略失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 计算密码过期时间
        /// </summary>
        private void CalculatePasswordExpiry(PasswordInfo info)
        {
            if (!info.PasswordNeverExpires && info.PasswordLastSet.HasValue && info.MaxPasswordAge.HasValue)
            {
                info.PasswordExpires = info.PasswordLastSet.Value.AddDays(info.MaxPasswordAge.Value);

                // 如果过期时间已过，标记为已过期
                if (info.PasswordExpires < DateTime.Now)
                {
                    info.PasswordExpired = true;
                }
            }
            else if (info.PasswordNeverExpires)
            {
                info.PasswordExpires = null;
            }
        }

        /// <summary>
        /// 批量获取所有用户信息
        /// </summary>
        public List<PasswordInfo> GetAllUsersPasswordInfo()
        {
            var allUsersInfo = new List<PasswordInfo>();

            try
            {
                using (var context = new PrincipalContext(ContextType.Machine, _machineName))
                {
                    using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                    {
                        foreach (var result in searcher.FindAll())
                        {
                            var user = (UserPrincipal)result;
                            var info = GetPasswordInfo(user.SamAccountName);
                            allUsersInfo.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Err($"[高级系统修改工具] [PassWord] 批量获取用户信息失败: {ex.Message}");
            }

            return allUsersInfo;
        }


        private void button_SetPassWord_Click(object sender, EventArgs e)
        {
            if (ResetPassword(comboBox_UserName.Text, textBox_PassWord.Text))
                MessageBox.Show("密码设置成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_ClearPassWord_Click(object sender, EventArgs e)
        {
            if (ResetPassword(comboBox_UserName.Text, string.Empty))
                MessageBox.Show("密码清除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_ExpirePasswordNow_Click(object sender, EventArgs e)
        {
            if (ForcePasswordChangeAtNextLogon(comboBox_UserName.Text))
                MessageBox.Show("密码已强制过期！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Password_UnlockUser_Click(object sender, EventArgs e)
        {
            if (UnlockUser(comboBox_UserName.Text))
                MessageBox.Show("账户已强制解锁！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void checkBox_PasswordNeverExpires_CheckedChanged(object sender, EventArgs e)
        {
            if (Loading) return;

            if (SetPasswordNeverExpires(comboBox_UserName.Text, checkBox_PasswordNeverExpires.Checked))
                MessageBox.Show(
                    $"成功设置永不过期为" +
                    $"{(checkBox_PasswordNeverExpires.Checked ? "开启" : "关闭")}！",
                    "提示", MessageBoxButtons.OK, MessageBoxIcon.Information
                );
        }

        private void checkBox_Password_UserEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (Loading) return;

            if (SetUserEnabled(comboBox_UserName.Text, checkBox_Password_UserEnabled.Checked))
                MessageBox.Show(
                    $"已成功将该用户启用状态设置为 " +
                    $"{(checkBox_Password_UserEnabled.Checked ? "启用" : "禁用")}！",
                    "提示", MessageBoxButtons.OK, MessageBoxIcon.Information
                );
        }



        private void button_GetPasswordInfo_Click(object sender, EventArgs e)
        {
            Loading = true;

            passwordInfo = GetPasswordInfo(comboBox_UserName.Text);
            checkBox_PasswordNeverExpires.Checked = passwordInfo.PasswordNeverExpires;
            checkBox_Password_UserEnabled.Checked = passwordInfo.AccountEnabled;

            UpdatePasswordInfoUI();

            Log.Info($"[高级系统修改工具] [PassWord] 读取信息，内容如下：\n{passwordInfo.ToString()}");

            Loading = false;
        }

        private void UpdatePasswordInfoUI()
        {
            string lang = GetLanguage();
            bool isEn = lang == "en";

            // ====== 辅助函数 ======
            string BoolText(bool value) => value ? (isEn ? "✅ Yes" : "✅ 是") : (isEn ? "❌ No" : "❌ 否");
            string DateText(DateTime? date, string zhDefault, string enDefault)
                => date?.ToString("yyyy-MM-dd HH:mm:ss") ?? (isEn ? enDefault : zhDefault);
            string IntText(int? value, string zhDefault, string enDefault, string zhUnit = "", string enUnit = "")
            {
                if (!value.HasValue) return isEn ? enDefault : zhDefault;
                return isEn ? $"{value}{enUnit}" : $"{value}{zhUnit}";
            }

            // ====== 定义映射：控件, 中文模板, 英文模板, 值获取器 ======
            var mappings = new (Label Label, string ZhTemplate, string EnTemplate, Func<string> ValueGetter)[]
            {
                (label_Password_UserName, "用户名：{0}", "Username: {0}", () => passwordInfo.UserName ?? (isEn ? "Unknown" : "未知")),
                (label_Password_FullName, "全名：{0}", "Full Name: {0}", () => passwordInfo.FullName ?? (isEn ? "Not set" : "未设置")),
                (label_Password_Description, "描述：{0}", "Description: {0}", () => passwordInfo.Description ?? (isEn ? "Not set" : "未设置")),

                (label_Password_PasswordNeverExpires, "密码永不过期：{0}", "Never Expires: {0}", () => BoolText(passwordInfo.PasswordNeverExpires)),
                (label_Password_UserCannotChangePassword, "用户不能改密码：{0}", "User Cannot Change: {0}", () => BoolText(passwordInfo.UserCannotChangePassword)),
                (label_Password_PasswordExpired, "密码已过期：{0}", "Password Expired: {0}", () => BoolText(passwordInfo.PasswordExpired)),
                (label_Password_PasswordRequired, "密码必须存在：{0}", "Password Required: {0}", () => BoolText(passwordInfo.PasswordRequired)),
                (label_Password_AccountEnabled, "账户已启用：{0}", "Account Enabled: {0}", () => BoolText(passwordInfo.AccountEnabled)),
                (label_Password_AccountLocked, "账户已锁定：{0}", "Account Locked: {0}", () => BoolText(passwordInfo.AccountLocked)),

                (label_Password_BadPasswordCount, "错误密码次数：{0}", "Bad Password Count: {0}", () => passwordInfo.BadPasswordCount.ToString()),
                (label_Password_BadLogonCount, "失败登录次数：{0}", "Failed Logon Count: {0}", () => passwordInfo.BadLogonCount.ToString()),

                (label_Password_MaxPasswordAge, "密码最长有效期：{0}天", "Max Age: {0} days", () => IntText(passwordInfo.MaxPasswordAge, "未获取", "Not retrieved", "天", " days")),
                (label_Password_MinPasswordAge, "密码最短有效期：{0}天", "Min Age: {0} days", () => IntText(passwordInfo.MinPasswordAge, "未获取", "Not retrieved", "天", " days")),
                (label_Password_PasswordHistoryLength, "密码历史记录：{0}个", "History Length: {0}", () => IntText(passwordInfo.PasswordHistoryLength, "未获取", "Not retrieved", "个", "")),
                (label_Password_LockoutThreshold, "账户锁定阈值：{0}", "Lockout Threshold: {0}", () => IntText(passwordInfo.LockoutThreshold, "未获取", "Not retrieved", "次失败尝试", " failed attempts")),
                (label_Password_LockoutDuration, "账户锁定时间：{0}分钟", "Lockout Duration: {0} min", () => IntText(passwordInfo.LockoutDuration, "未获取", "Not retrieved", "分钟", " min")),

                (label_Password_LastPasswordSet, "上次设置密码：{0}", "Last Set: {0}", () => DateText(passwordInfo.PasswordLastSet, "未知", "Unknown")),
                (label_Password_PasswordExpires, "密码过期时间：{0}", "Expires: {0}", () => DateText(passwordInfo.PasswordExpires, "未设置 / 永不过期", "Not set / Never")),
                (label_Password_LastLogin, "最后登录时间：{0}", "Last Login: {0}", () => DateText(passwordInfo.LastLogin, "从未登录", "Never logged in")),
                (label_Password_LastBadLoginAttempt, "最后失败登录：{0}", "Last Failed Login: {0}", () => DateText(passwordInfo.LastBadLoginAttempt, "无失败记录", "No failures")),
                (label_Password_AccountExpirationDate, "账户过期时间：{0}", "Account Expires: {0}", () => DateText(passwordInfo.AccountExpirationDate, "永不过期", "Never")),
            };

            // ====== 执行赋值 ======
            foreach (var (label, zhTemplate, enTemplate, valueGetter) in mappings)
            {
                string template = isEn ? enTemplate : zhTemplate;
                label.Text = string.Format(template, valueGetter());
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
