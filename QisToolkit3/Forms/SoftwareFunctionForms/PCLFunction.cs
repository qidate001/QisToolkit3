using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace QisToolkit3.Forms.SoftwareFunctionForms
{
    public partial class PCLFunction : Form
    {
        public PCLFunction()
        {
            InitializeComponent();
        }

        private void PCLFunction_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void LoadData()
        {
            try
            {
                // 打开PCL注册表项
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PCL"))
                {
                    if (key != null)
                    {
                        // 读取并设置各个注册表值到对应控件

                        // 字符串类型直接赋值
                        textBoxIdentify.Text = key.GetValue("Identify")?.ToString() ?? "";
                        textBoxSystemLastVersionReg.Text = key.GetValue("SystemLastVersionReg")?.ToString() ?? "";
                        textBoxSystemHighestBetaVersionReg.Text = key.GetValue("SystemHighestBetaVersionReg")?.ToString() ?? "";
                        textBoxUiLauncherThemeHide2.Text = key.GetValue("UiLauncherThemeHide2")?.ToString() ?? "";
                        textBoxCacheMsV2OAuthRefresh.Text = key.GetValue("CacheMsV2OAuthRefresh")?.ToString() ?? "";
                        textBoxCacheMsV2Access.Text = key.GetValue("CacheMsV2Access")?.ToString() ?? "";
                        textBoxCacheMsV2ProfileJson.Text = key.GetValue("CacheMsV2ProfileJson")?.ToString() ?? "";
                        textBoxCacheMsV2Uuid.Text = key.GetValue("CacheMsV2Uuid")?.ToString() ?? "";
                        textBoxCacheMsV2Name.Text = key.GetValue("CacheMsV2Name")?.ToString() ?? "";
                        textBoxLaunchFolders.Text = key.GetValue("LaunchFolders")?.ToString() ?? "";
                        textBoxCacheJavaListVersion.Text = key.GetValue("CacheJavaListVersion")?.ToString() ?? "";
                        textBoxSystemCount.Text = key.GetValue("SystemCount")?.ToString() ?? "";
                        textBoxLaunchArgumentJavaAll.Text = key.GetValue("LaunchArgumentJavaAll")?.ToString() ?? "";
                        textBoxToolUpdateReleaseLast.Text = key.GetValue("ToolUpdateReleaseLast")?.ToString() ?? "";
                        textBoxToolUpdateSnapshotLast.Text = key.GetValue("ToolUpdateSnapshotLast")?.ToString() ?? "";
                        textBoxLoginMsJson.Text = key.GetValue("LoginMsJson")?.ToString() ?? "";
                        textBoxSystemLaunchCount.Text = key.GetValue("SystemLaunchCount")?.ToString() ?? "";
                        textBoxSystemHighestSavedBetaVersionReg.Text = key.GetValue("SystemHighestSavedBetaVersionReg")?.ToString() ?? "";
                        textBoxSystemHelpVersion.Text = key.GetValue("SystemHelpVersion")?.ToString() ?? "";
                        textBoxLoginLegacyName.Text = key.GetValue("LoginLegacyName")?.ToString() ?? "";
                        textBoxCacheDownloadFolder.Text = key.GetValue("CacheDownloadFolder")?.ToString() ?? "";

                        // 布尔值类型使用ComboBox选择
                        comboBoxCacheMsV2Migrated.SelectedIndex = key.GetValue("CacheMsV2Migrated")?.ToString() == "True" ? 0 : 1;
                        comboBoxSystemEula.SelectedIndex = key.GetValue("SystemEula")?.ToString() == "True" ? 0 : 1;
                        comboBoxHintBuy.SelectedIndex = key.GetValue("HintBuy")?.ToString() == "True" ? 0 : 1;
                        comboBoxHintUpdateMod.SelectedIndex = key.GetValue("HintUpdateMod")?.ToString() == "True" ? 0 : 1;
                        comboBoxHintInstallBack.SelectedIndex = key.GetValue("HintInstallBack")?.ToString() == "True" ? 0 : 1;

                        // 枚举/数字类型使用ComboBox选择或直接设置文本
                        string loginType = key.GetValue("LoginType")?.ToString() ?? "5";
                        comboBoxLoginType.Text = loginType;

                        string hintDownload = key.GetValue("HintDownload")?.ToString() ?? "5";
                        comboBoxHintDownload.Text = hintDownload;

                        string hintNotice = key.GetValue("HintNotice")?.ToString() ?? "180";
                        comboBoxHintNotice.Text = hintNotice;
                    }
                    else
                    {
                        MessageBox.Show("未找到PCL注册表项，请确保运行过PCL。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取注册表时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveData()
        {
            try
            {
                // 打开或创建PCL注册表项
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PCL"))
                {
                    if (key != null)
                    {
                        // 保存字符串类型值
                        key.SetValue("Identify", textBoxIdentify.Text, RegistryValueKind.String);
                        key.SetValue("SystemLastVersionReg", textBoxSystemLastVersionReg.Text, RegistryValueKind.String);
                        key.SetValue("SystemHighestBetaVersionReg", textBoxSystemHighestBetaVersionReg.Text, RegistryValueKind.String);
                        key.SetValue("UiLauncherThemeHide2", textBoxUiLauncherThemeHide2.Text, RegistryValueKind.String);
                        key.SetValue("CacheMsV2OAuthRefresh", textBoxCacheMsV2OAuthRefresh.Text, RegistryValueKind.String);
                        key.SetValue("CacheMsV2Access", textBoxCacheMsV2Access.Text, RegistryValueKind.String);
                        key.SetValue("CacheMsV2ProfileJson", textBoxCacheMsV2ProfileJson.Text, RegistryValueKind.String);
                        key.SetValue("CacheMsV2Uuid", textBoxCacheMsV2Uuid.Text, RegistryValueKind.String);
                        key.SetValue("CacheMsV2Name", textBoxCacheMsV2Name.Text, RegistryValueKind.String);
                        key.SetValue("LaunchFolders", textBoxLaunchFolders.Text, RegistryValueKind.String);
                        key.SetValue("CacheJavaListVersion", textBoxCacheJavaListVersion.Text, RegistryValueKind.String);
                        key.SetValue("SystemCount", textBoxSystemCount.Text, RegistryValueKind.String);
                        key.SetValue("LaunchArgumentJavaAll", textBoxLaunchArgumentJavaAll.Text, RegistryValueKind.String);
                        key.SetValue("ToolUpdateReleaseLast", textBoxToolUpdateReleaseLast.Text, RegistryValueKind.String);
                        key.SetValue("ToolUpdateSnapshotLast", textBoxToolUpdateSnapshotLast.Text, RegistryValueKind.String);
                        key.SetValue("LoginMsJson", textBoxLoginMsJson.Text, RegistryValueKind.String);
                        key.SetValue("SystemLaunchCount", textBoxSystemLaunchCount.Text, RegistryValueKind.String);
                        key.SetValue("SystemHighestSavedBetaVersionReg", textBoxSystemHighestSavedBetaVersionReg.Text, RegistryValueKind.String);
                        key.SetValue("SystemHelpVersion", textBoxSystemHelpVersion.Text, RegistryValueKind.String);
                        key.SetValue("LoginLegacyName", textBoxLoginLegacyName.Text, RegistryValueKind.String);
                        key.SetValue("CacheDownloadFolder", textBoxCacheDownloadFolder.Text, RegistryValueKind.String);

                        // 保存布尔值类型
                        key.SetValue("CacheMsV2Migrated", comboBoxCacheMsV2Migrated.SelectedIndex == 0 ? "True" : "False", RegistryValueKind.String);
                        key.SetValue("SystemEula", comboBoxSystemEula.SelectedIndex == 0 ? "True" : "False", RegistryValueKind.String);
                        key.SetValue("HintBuy", comboBoxHintBuy.SelectedIndex == 0 ? "True" : "False", RegistryValueKind.String);
                        key.SetValue("HintUpdateMod", comboBoxHintUpdateMod.SelectedIndex == 0 ? "True" : "False", RegistryValueKind.String);
                        key.SetValue("HintInstallBack", comboBoxHintInstallBack.SelectedIndex == 0 ? "True" : "False", RegistryValueKind.String);

                        // 保存枚举/数字类型
                        key.SetValue("LoginType", comboBoxLoginType.Text, RegistryValueKind.String);
                        key.SetValue("HintDownload", comboBoxHintDownload.Text, RegistryValueKind.String);
                        key.SetValue("HintNotice", comboBoxHintNotice.Text, RegistryValueKind.String);

                        MessageBox.Show("PCL配置已成功保存到注册表！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("无法创建或打开PCL注册表项。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存注册表时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("此操作这将会永久清除这些信息！", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (MessageBox.Show("再次警告！！！\n此操作这将会永久清除这些信息！！！！！", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE"))
                    {
                        key.DeleteSubKeyTree("PCL");
                    }
                }
            }
        }
    }
}