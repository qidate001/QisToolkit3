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
                LoadCurrentVersionRegistryValues();

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

        private void LoadCurrentVersionRegistryValues()
        {
            try
            {
                RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

                // ========== 第一组：系统路径 ==========
                comboBox_SystemRoot.Text = registryKey.GetValue("SystemRoot")?.ToString() ?? "";
                comboBox_PathName.Text = registryKey.GetValue("PathName")?.ToString() ?? "";

                // ========== 第二组：版本标识（EditionID 相关） ==========
                comboBox_WindowsEditionID.Text = registryKey.GetValue("EditionID")?.ToString() ?? "";
                comboBox_CompositionEditionID.Text = registryKey.GetValue("CompositionEditionID")?.ToString() ?? "";
                comboBox_ProductName.Text = registryKey.GetValue("ProductName")?.ToString() ?? "";

                // ========== 第三组：注册信息 ==========
                comboBox_registeredOwner.Text = registryKey.GetValue("RegisteredOwner")?.ToString() ?? "";
                comboBox_registeredOrganization.Text = registryKey.GetValue("RegisteredOrganization")?.ToString() ?? "";

                // ========== 第四组：软件类型与安装类型 ==========
                comboBox_SoftwareType.Text = registryKey.GetValue("SoftwareType")?.ToString() ?? "";
                comboBox_InstallationType.Text = registryKey.GetValue("InstallationType")?.ToString() ?? "";
                comboBox_CurrentType.Text = registryKey.GetValue("CurrentType")?.ToString() ?? "";

                // ========== 第五组：核心版本号（字符串类型） ==========
                comboBox_CurrentVersion.Text = registryKey.GetValue("CurrentVersion")?.ToString() ?? "";
                comboBox_CurrentBuild.Text = registryKey.GetValue("CurrentBuild")?.ToString() ?? "";
                //comboBox_CurrentBuildNumber.Text = registryKey.GetValue("CurrentBuildNumber")?.ToString() ?? "";
                comboBox_DisplayVersion.Text = registryKey.GetValue("DisplayVersion")?.ToString() ?? "";
                comboBox_ReleaseId.Text = registryKey.GetValue("ReleaseId")?.ToString() ?? "";

                // ========== 第六组：核心版本号（DWORD 类型，需要转十进制字符串） ==========
                // CurrentMajorVersionNumber 是 DWORD，需要读取为 int
                object majorVersionObj = registryKey.GetValue("CurrentMajorVersionNumber");
                comboBox_CurrentMajorVersionNumber.Text = majorVersionObj != null ? Convert.ToInt32(majorVersionObj).ToString() : "";

                object minorVersionObj = registryKey.GetValue("CurrentMinorVersionNumber");
                comboBox_CurrentMinorVersionNumber.Text = minorVersionObj != null ? Convert.ToInt32(minorVersionObj).ToString() : "";

                object ubrObj = registryKey.GetValue("UBR");
                comboBox_UBR.Text = ubrObj != null ? Convert.ToInt32(ubrObj).ToString() : "";

                object baseRevisionObj = registryKey.GetValue("BaseBuildRevisionNumber");
                comboBox_BaseBuildRevisionNumber.Text = baseRevisionObj != null ? Convert.ToInt32(baseRevisionObj).ToString() : "";

                // ========== 第七组：编译信息 ==========
                comboBox_BuildLab.Text = registryKey.GetValue("BuildLab")?.ToString() ?? "";
                comboBox_BuildLabEx.Text = registryKey.GetValue("BuildLabEx")?.ToString() ?? "";
                comboBox_BuildBranch.Text = registryKey.GetValue("BuildBranch")?.ToString() ?? "";
                comboBox_BuildGUID.Text = registryKey.GetValue("BuildGUID")?.ToString() ?? "";

                // ========== 第八组：恢复环境与产品ID ==========
                comboBox_WinREVersion.Text = registryKey.GetValue("WinREVersion")?.ToString() ?? "";
                comboBox_ProductId.Text = registryKey.GetValue("ProductId")?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                Log.Err(ERROR_Text(ex));
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

        #region Windows 标识

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
                MessageBoxERROR(ex);
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

        private void button_CompositionEditionID_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_CompositionEditionID.Text))
                temp = MessageBox.Show
                (
                    "您要修改的合成版本标识的值为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "修改 CompositionEditionID 风险较大\n潜在问题：" +
                "\n\n激活混乱：此值与 EditionID 共同决定系统版本身份，两者不一致时可能导致激活服务（KMS/数字许可）误判，造成激活失效。" +
                "\n\n系统身份冲突：某些系统组件（如 Windows 功能开关）会优先读取此值来判断可用的功能集，与实际 EditionID 不匹配可能导致功能开关错乱。" +
                "\n\nWindows 更新异常：更新程序可能根据此值推送不匹配的更新包（例如向专业版推送企业版专属更新），导致安装失败。" +
                "\n\n应用兼容性问题：部分 UWP 应用或商店应用会读取此值判断设备类型，不一致时可能拒绝运行或显示乱码。" +
                "\n\n\n此工具推荐在您需要临时伪装系统版本以绕过某些软件安装限制时使用，但请务必与 EditionID 保持一致，避免系统身份分裂。",
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
                registryKey.SetValue("CompositionEditionID", comboBox_CompositionEditionID.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
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

        private void button_SoftwareType_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_SoftwareType.Text))
                temp = MessageBox.Show
                (
                    "您要修改的软件类型的值为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "修改 SoftwareType 风险较大\n潜在问题：" +
                "\n\n系统身份欺骗：此值标识当前系统是客户端（Client）还是服务器（Server）操作系统，修改后可能让部分软件误判系统类型。" +
                "\n\n驱动程序兼容性：某些硬件驱动会检查此值决定是否安装，例如将 Client 改为 Server 后，某些消费级显卡驱动可能拒绝运行。" +
                "\n\n系统功能误判：部分 Windows 服务（如 Windows Update 策略）会参考此值选择更新频道，修改后可能收到不匹配的更新。" +
                "\n\n软件安装限制：某些企业级软件（如 SQL Server）会在安装时检查此值，若发现 Client 与 Server 不匹配可能直接拒绝安装。" +
                "\n\n\n此值一般不建议修改。仅在您需要伪装成服务器系统以测试某些软件兼容性时使用，测试完毕后请立即改回 Client。",
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
                registryKey.SetValue("SoftwareType", comboBox_SoftwareType.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
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

        private void button_ProductName_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_ProductName.Text))
                temp = MessageBox.Show
                (
                    "您要修改的产品名称为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "修改 ProductName 风险较大\n潜在问题：" +
                "\n\n界面显示异常：此值是系统在“关于 Windows”和“系统信息”中显示的名称，修改后仅影响表面文字，实际功能不受影响。" +
                "\n\n软件识别问题：某些软件（尤其是系统优化工具和杀毒软件）会读取此值来判断系统版本，修改后可能导致其误判（例如将 Win10 Pro 识别为 Win11 企业版）。" +
                "\n\n技术支持误导：如果您误将此值改为不存在的版本（如 Windows 100），后续在联系微软技术支持时可能造成沟通障碍。" +
                "\n\n系统升级检测：Windows Update 的推荐更新策略不会以此值为依据，但某些第三方更新程序可能会因此拒绝工作。" +
                "\n\n\n此值仅相当于系统的“显示名”标签，风险相对较低。但建议与实际版本保持一致（如 Windows 10 Pro），避免引起不必要的兼容性误会。",
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
                registryKey.SetValue("ProductName", comboBox_ProductName.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
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

        private void button_CurrentVersion_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_CurrentVersion.Text))
                temp = MessageBox.Show
                (
                    "您要修改的内核兼容版本号的值为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "修改 CurrentVersion 风险极高\n潜在问题：" +
                "\n\n系统内核识别异常：此值是 Windows NT 内核的兼容性版本号，大量系统底层驱动和内核模块在加载前会检查此值，修改后可能导致驱动拒绝加载，引发蓝屏（BSOD）。" +
                "\n\n软件兼容性崩塌：许多老旧软件（尤其是工业控制软件、财务软件）依赖此值判断系统是否为其支持的操作系统，修改后可能直接提示“系统不支持”而无法运行。" +
                "\n\n安全补丁失效：Windows 更新程序会验证此值，若与预期不符，安全补丁可能认为系统版本过高或过低而拒绝安装。" +
                "\n\n系统还原点损坏：系统还原功能可能因版本号异常而无法正常工作。" +
                "\n\n\n此值强烈不建议修改！当前 Win10/Win11 此值通常为 6.3（兼容 Win8.1）。除非您是系统开发人员且明确知道自己在做什么，否则请勿触碰。",
                "严重风险警告",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "最后再次警告，此操作极有可能导致系统无法启动，您确定要修改吗？",
                "严重风险警告",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            );

            if (temp != DialogResult.OK)
                return;

            try
            {
                RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                registryKey.SetValue("CurrentVersion", comboBox_CurrentVersion.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
            }
            finally
            {
                MessageBox.Show
                (
                    $"修改成功！请重启系统以生效（风险自负）。",
                    "齐的工具包3",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void button_CurrentMajorVersionNumber_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_CurrentMajorVersionNumber.Text))
                temp = MessageBox.Show
                (
                    "您要修改的主版本号的值为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            // 尝试转换为 DWORD
            if (!int.TryParse(comboBox_CurrentMajorVersionNumber.Text, out int majorVersion))
            {
                MessageBox.Show
                (
                    "主版本号必须为有效的整数（如 10），请检查输入！",
                    "输入错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            temp = MessageBox.Show
            (
                "修改 CurrentMajorVersionNumber 风险极高\n潜在问题：" +
                "\n\n系统版本身份错乱：此值决定了 Windows 的大版本号（如 10 代表 Win10，11 代表 Win11），修改后可能导致系统被识别为错误的版本。" +
                "\n\n驱动程序兼容性灾难：绝大多数硬件驱动在安装时会检查此值，若与预期不符（例如将 10 改为 11），驱动可能拒绝安装，导致硬件无法使用。" +
                "\n\nWindows 更新彻底失败：更新程序会严格验证此值，若发现不匹配，将直接报错 0x800f0922，再也无法接收安全更新。" +
                "\n\n应用程序大规模报错：大量现代应用（尤其是 UWP 和 .NET 应用）会在启动时检查此值，不一致将直接闪退。" +
                "\n\n\n此值是系统身份的基石，永远不建议修改！除非您想彻底破坏系统兼容性。",
                "严重风险警告",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "最后再次警告，修改此值将导致大量软件无法运行，您确定要修改吗？",
                "严重风险警告",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            );

            if (temp != DialogResult.OK)
                return;

            try
            {
                RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                registryKey.SetValue("CurrentMajorVersionNumber", majorVersion, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
            }
            finally
            {
                MessageBox.Show
                (
                    $"修改成功！请重启系统以生效（风险自负）。",
                    "齐的工具包3",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void button_CurrentMinorVersionNumber_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_CurrentMinorVersionNumber.Text))
                temp = MessageBox.Show
                (
                    "您要修改的次版本号的值为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            if (!int.TryParse(comboBox_CurrentMinorVersionNumber.Text, out int minorVersion))
            {
                MessageBox.Show
                (
                    "次版本号必须为有效的整数（通常为 0），请检查输入！",
                    "输入错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            temp = MessageBox.Show
            (
                "修改 CurrentMinorVersionNumber 风险较高\n潜在问题：" +
                "\n\n版本号异常：此值通常为 0，修改后可能导致系统版本显示为不存在的版本（如 Windows 10.1）。" +
                "\n\n部分软件识别异常：某些安全软件会同时检查主版本号和次版本号，若组合不匹配可能认为系统被篡改而拒绝运行。" +
                "\n\nAPI 调用失败：部分系统 API 会返回此值，修改后可能影响依赖版本号的应用程序逻辑。" +
                "\n\n\n此值通常保持为 0，不建议修改。仅当您需要模拟特定版本组合进行开发测试时使用。",
                "风险警告",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
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
                registryKey.SetValue("CurrentMinorVersionNumber", minorVersion, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
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

        private void button_CurrentBuild_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_CurrentBuild.Text))
                temp = MessageBox.Show
                (
                    "您要修改的当前内部版本号的值为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "修改 CurrentBuild 风险极高\n潜在问题：" +
                "\n\n系统版本号根本性篡改：此值是系统最核心的版本标识（如 19045），修改后将导致整个系统被识别为错误的版本号。" +
                "\n\n系统文件校验失败：Windows 文件保护（WFP）会检查此值与系统文件版本是否匹配，不匹配可能导致 SFC 扫描报错并尝试自动修复，但修复后会改回原值。" +
                "\n\n驱动程序拒绝加载：大量驱动程序会在安装时严格检查此值，若不一致将直接拒绝安装（尤其是显卡和芯片组驱动）。" +
                "\n\nWindows 更新彻底崩溃：更新服务会严格依赖此值来判断需要安装哪些补丁包，修改后将无法接收任何更新，Windows Update 会持续报错。" +
                "\n\n软件兼容性：某些软件的安装程序会读取此值来判断系统是否为受支持的版本（如 Win10 22H2），修改后可能无法安装。" +
                "\n\n\n此值是系统版本的核心，强烈不建议修改！仅推荐在极特殊的场景（如为了绕过某些软件版本限制）时临时修改，修改前请务必创建系统还原点。",
                "严重风险警告",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "最后再次警告，修改此值将严重影响系统更新和驱动安装，您确定要修改吗？",
                "严重风险警告",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            );

            if (temp != DialogResult.OK)
                return;

            try
            {
                RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                registryKey.SetValue("CurrentBuild", comboBox_CurrentBuild.Text);
                // 同步修改 CurrentBuildNumber
                registryKey.SetValue("CurrentBuildNumber", comboBox_CurrentBuild.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
            }
            finally
            {
                MessageBox.Show
                (
                    $"修改成功！CurrentBuild 和 CurrentBuildNumber 已同步修改，请重启系统以生效（风险自负）。",
                    "齐的工具包3",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void button_UBR_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_UBR.Text))
                temp = MessageBox.Show
                (
                    "您要修改的更新修订版本号的值为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            if (!int.TryParse(comboBox_UBR.Text, out int ubrValue) || ubrValue < 0)
            {
                MessageBox.Show
                (
                    "UBR 必须为有效的正整数（如 7417），请检查输入！",
                    "输入错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            temp = MessageBox.Show
            (
                "修改 UBR 风险较高\n潜在问题：" +
                "\n\n补丁版本号错乱：此值标识当前系统已安装的最新累积更新的修订号，修改后会让系统“谎报”自己的补丁级别。" +
                "\n\nWindows 更新冲突：更新程序会检测此值，若发现实际已安装的补丁版本与 UBR 不符，可能导致更新检查失败或反复提示安装已安装的补丁。" +
                "\n\n安全软件误报：部分安全软件会检查 UBR 来判断系统是否打了最新补丁，若不一致可能错误地认为系统存在漏洞。" +
                "\n\n系统完整性检查：DISM 和 SFC 工具在扫描时会参考此值，修改后可能导致扫描结果异常。" +
                "\n\n\n此值通常不建议手动修改，因为系统会在安装累积更新时自动更新它。仅用于开发测试环境模拟特定补丁版本。",
                "风险警告",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
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
                registryKey.SetValue("UBR", ubrValue, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
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

        private void button_BaseBuildRevisionNumber_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_BaseBuildRevisionNumber.Text))
                temp = MessageBox.Show
                (
                    "您要修改的基础编译修订号的值为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            if (!int.TryParse(comboBox_BaseBuildRevisionNumber.Text, out int baseRevision) || baseRevision < 0)
            {
                MessageBox.Show
                (
                    "基础编译修订号必须为有效的正整数（通常为 1），请检查输入！",
                    "输入错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            temp = MessageBox.Show
            (
                "修改 BaseBuildRevisionNumber 风险较高\n潜在问题：" +
                "\n\n编译版本标识篡改：此值表示该 Build 的首次编译修订号（通常是 1），修改后可能影响某些底层系统工具对版本一致性的判断。" +
                "\n\n系统文件版本校验异常：部分系统文件在版本资源中会包含此信息，不一致可能触发文件保护机制（SFC）的误报。" +
                "\n\n调试和日志混乱：系统日志和错误报告中会引用此值，修改后可能导致开发调试人员产生混淆。" +
                "\n\n\n此值一般由微软编译系统自动生成，永不需要手动修改。除非您是系统开发人员在进行版本号研究。",
                "风险警告",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
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
                registryKey.SetValue("BaseBuildRevisionNumber", baseRevision, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
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

        private void button_DisplayVersion_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_DisplayVersion.Text))
                temp = MessageBox.Show
                (
                    "您要修改的显示版本名称为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "修改 DisplayVersion 风险中等\n潜在问题：" +
                "\n\n用户界面显示混乱：此值会显示在“设置”->“系统”->“关于”页面以及 winver 命令中，修改后会让您自己都搞不清当前版本。" +
                "\n\n技术支持误导：当您联系微软或第三方技术支持时，他们可能基于这个显示名称做出错误的判断。" +
                "\n\n软件读取误判：某些软件（如系统信息工具）会读取此值来显示版本名称，修改后可能导致显示错误信息。" +
                "\n\n\n此值仅影响显示文字，不影响系统实际功能。可以修改为任意文字（如“Windows 10 超级版”），但建议与实际版本保持对应关系（如 22H2）。",
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
                registryKey.SetValue("DisplayVersion", comboBox_DisplayVersion.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
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

        private void button_ReleaseId_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_ReleaseId.Text))
                temp = MessageBox.Show
                (
                    "您要修改的功能更新发布标识为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "修改 ReleaseId 风险中等\n潜在问题：" +
                "\n\n版本标识错乱：此值表示功能更新的发布年份月份（如 2009 代表 2020年09月），修改后可能导致系统被识别为不同的功能更新版本。" +
                "\n\nWindows 更新影响：部分更新检测机制会参考此值，修改后可能导致补丁推送策略发生变化（例如收到旧版本的更新）。" +
                "\n\n软件兼容性：某些软件安装包会检查此值来判断系统版本是否满足要求（如要求 2004 及以上），修改后可以绕过检查。" +
                "\n\n\n此值在较新的 Windows 版本中已被 DisplayVersion 取代，但仍被部分旧软件使用。修改后不影响系统功能。",
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
                registryKey.SetValue("ReleaseId", comboBox_ReleaseId.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
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

        private void button_BuildLab_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_BuildLab.Text))
                temp = MessageBox.Show
                (
                    "您要修改的编译实验室标签为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "修改 BuildLab 风险中等\n潜在问题：" +
                "\n\n编译信息伪造：此值包含了编译分支和时间戳信息（如 19041.vb_release.191206-1406），修改后会让系统看起来像是从不同的分支编译的。" +
                "\n\n系统调试工具混淆：部分内核调试工具会参考此值，修改后可能导致调试信息不一致。" +
                "\n\n系统信息显示异常：此值会在某些系统信息工具中显示，修改后可能导致信息混乱。" +
                "\n\n\n此值仅用于标识编译来源，不影响系统功能。仅推荐在开发测试环境中修改以模拟不同构建场景。",
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
                registryKey.SetValue("BuildLab", comboBox_BuildLab.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
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

        private void button_BuildLabEx_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_BuildLabEx.Text))
                temp = MessageBox.Show
                (
                    "您要修改的编译实验室扩展标签为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "修改 BuildLabEx 风险中等\n潜在问题：" +
                "\n\n扩展编译信息伪造：此值是 BuildLab 的扩展版，包含了架构和编译号（如 19041.1.amd64fre.vb_release.191206-1406）。" +
                "\n\n系统信息显示异常：修改后会导致系统信息中显示的编译信息与实际不符。" +
                "\n\n驱动签名验证：部分驱动程序在签名时会记录此信息，若不一致可能触发驱动签名验证警告。" +
                "\n\n\n此值仅用于标识编译来源和架构，不影响系统功能。建议保持与实际系统一致。",
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
                registryKey.SetValue("BuildLabEx", comboBox_BuildLabEx.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
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

        private void button_BuildBranch_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_BuildBranch.Text))
                temp = MessageBox.Show
                (
                    "您要修改的编译分支名称为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "修改 BuildBranch 风险较低\n潜在问题：" +
                "\n\n编译分支标识篡改：此值仅用于标识系统是从哪个开发分支编译的（如 vb_release），修改后不影响系统功能。" +
                "\n\n调试信息混淆：仅对系统开发人员和调试工具有参考意义。" +
                "\n\n\n此值仅作为标识信息，可以安全修改。",
                "提示",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information
            );

            if (temp != DialogResult.OK)
                return;

            try
            {
                RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                registryKey.SetValue("BuildBranch", comboBox_BuildBranch.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
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

        private void button_BuildGUID_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_BuildGUID.Text))
                temp = MessageBox.Show
                (
                    "您要修改的编译 GUID 为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            // 简单验证 GUID 格式
            if (!Guid.TryParse(comboBox_BuildGUID.Text, out _))
            {
                MessageBox.Show
                (
                    "输入的 GUID 格式不正确！请使用标准 GUID 格式（如 ffffffff-ffff-ffff-ffff-ffffffffffff）。",
                    "输入错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            temp = MessageBox.Show
            (
                "修改 BuildGUID 风险极低\n潜在问题：" +
                "\n\n此值在大多数系统上为全 F 的占位符（ffffffff-ffff-ffff-ffff-ffffffffffff），实际未被微软使用。" +
                "\n\n修改此值不影响任何系统功能，仅用于某些第三方的自定义识别。" +
                "\n\n\n可以安全修改。",
                "提示",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information
            );

            if (temp != DialogResult.OK)
                return;

            try
            {
                RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                registryKey.SetValue("BuildGUID", comboBox_BuildGUID.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
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

        private void button_CurrentType_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_CurrentType.Text))
                temp = MessageBox.Show
                (
                    "您要修改的当前内核类型为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "修改 CurrentType 风险较低\n潜在问题：" +
                "\n\n此值通常为 \"Multiprocessor Free\"，表示系统支持多处理器且为零售版本。" +
                "\n\n修改此值仅影响部分系统信息工具的显示，不影响实际内核功能。" +
                "\n\n\n可以安全修改，但建议保持标准值 \"Multiprocessor Free\"。",
                "提示",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information
            );

            if (temp != DialogResult.OK)
                return;

            try
            {
                RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                registryKey.SetValue("CurrentType", comboBox_CurrentType.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
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

        private void button_InstallationType_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_InstallationType.Text))
                temp = MessageBox.Show
                (
                    "您要修改的安装类型为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "修改 InstallationType 风险中等\n潜在问题：" +
                "\n\n此值标识系统是 Client（客户端）还是 Server（服务器）安装类型。" +
                "\n\n修改后可能影响某些服务器软件的安装检测，以及 Windows 功能的可用性判断。" +
                "\n\n\n仅在需要模拟服务器环境进行软件测试时修改，测试完毕建议改回 Client。",
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
                registryKey.SetValue("InstallationType", comboBox_InstallationType.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
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

        private void button_WinREVersion_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_WinREVersion.Text))
                temp = MessageBox.Show
                (
                    "您要修改的 Windows 恢复环境版本为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "修改 WinREVersion 风险中等\n潜在问题：" +
                "\n\n此值标识 Windows 恢复环境（WinRE）的版本号，通常与主系统版本不同（如 10.0.19041.5363）。" +
                "\n\n修改此值仅影响 WinRE 的版本显示，不影响 WinRE 的实际功能。" +
                "\n\n但在系统恢复时，某些工具可能会检查此值来确认恢复环境是否兼容。" +
                "\n\n\n建议保持与实际 WinRE 版本一致，随意修改可能导致恢复环境识别异常。",
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
                registryKey.SetValue("WinREVersion", comboBox_WinREVersion.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
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

        private void button_ProductId_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_ProductId.Text))
                temp = MessageBox.Show
                (
                    "您要修改的产品安装标识符为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "修改 ProductId 风险极低\n潜在问题：" +
                "\n\n此值仅用于产品安装识别和电话激活的辅助标识，不包含密钥信息。" +
                "\n\n修改此值不影响系统激活状态（激活信息存储在 DigitalProductId 中）。" +
                "\n\n影响非常有限，仅某些系统信息工具会显示此值。" +
                "\n\n\n可以安全修改，但建议保留原值。",
                "提示",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information
            );

            if (temp != DialogResult.OK)
                return;

            try
            {
                RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                registryKey.SetValue("ProductId", comboBox_ProductId.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
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

        private void button_SystemRoot_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_SystemRoot.Text))
                temp = MessageBox.Show
                (
                    "您要修改的系统根目录为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "修改 SystemRoot 风险极高！\n潜在问题：" +
                "\n\n所有系统路径依赖崩溃：无数系统服务和应用程序通过读取此值来定位系统文件（如 %SystemRoot%\\System32）。" +
                "\n\n修改后将导致大量程序无法找到系统文件，直接导致蓝屏（BSOD）或系统无法启动。" +
                "\n\n\n除非您将系统安装到了非 C 盘（如 D:\\Windows），否则永远不要修改此值！" +
                "\n\n如果您确实需要修改，请同时修改 PathName，并确保指向的目录中存在完整的 Windows 系统文件。",
                "严重风险警告",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "最后再次警告，此操作极大概率导致系统崩溃，您确定要修改吗？",
                "严重风险警告",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            );

            if (temp != DialogResult.OK)
                return;

            try
            {
                RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                registryKey.SetValue("SystemRoot", comboBox_SystemRoot.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
            }
            finally
            {
                MessageBox.Show
                (
                    $"修改成功！请重启系统以生效（强烈建议您先备份重要数据）。",
                    "齐的工具包3",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void button_PathName_Click(object sender, EventArgs e)
        {
            DialogResult temp = DialogResult.OK;
            if (string.IsNullOrWhiteSpace(comboBox_PathName.Text))
                temp = MessageBox.Show
                (
                    "您要修改的系统路径为空，是否是故意为之？",
                    "疑似错误警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "修改 PathName 风险极高！\n潜在问题：" +
                "\n\n此值与 SystemRoot 功能类似，同样是系统路径的核心标识。" +
                "\n\n许多老旧应用程序会读取此值而非 SystemRoot，修改后可能导致这些程序无法找到系统文件。" +
                "\n\n\n如果修改，必须与 SystemRoot 保持一致。否则系统将陷入路径不一致的混乱状态。" +
                "\n\n仅当您将系统安装到了非 C 盘时需要同步修改此值。",
                "严重风险警告",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            );

            if (temp != DialogResult.OK)
                return;

            temp = MessageBox.Show
            (
                "最后再次警告，此操作极大概率导致系统崩溃，您确定要修改吗？",
                "严重风险警告",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            );

            if (temp != DialogResult.OK)
                return;

            try
            {
                RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                registryKey.SetValue("PathName", comboBox_PathName.Text);
            }
            catch (Exception ex)
            {
                MessageBoxERROR(ex);
            }
            finally
            {
                MessageBox.Show
                (
                    $"修改成功！请重启系统以生效（强烈建议您先备份重要数据）。",
                    "齐的工具包3",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
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

        private void button_WindowsEditionID_Set_Click_2(object sender, EventArgs e)
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
    }
}
