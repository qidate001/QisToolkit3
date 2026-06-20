using QisToolkit3.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Qi;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace QisToolkit3.Forms
{
    public partial class SystemPermissionLauncher : Form
    {
        public SystemPermissionLauncher()
        {
            InitializeComponent();

            // 计算右上角位置：屏幕宽度 - 窗口宽度
            //int x = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) * 2 / 3;
            //int y = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) * 2 / 7;

            //this.Location = new Point(x, y);
        }

        private void SystemPermissionLauncher_Load(object sender, EventArgs e)
        {
            string windowsDir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            button_Py.Enabled = File.Exists(Path.Combine(windowsDir, "py.exe"));
            comboBox_RunMode.SelectedIndex = 0;
        }

        private void Run(string name, string arguments = "")
        {
            if (comboBox_RunMode.SelectedIndex == 0)
                RunNSudo($"{name}");
            else if (comboBox_RunMode.SelectedIndex == 1)
            {
                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = name,
                        Arguments = arguments,
                    };

                    process.Start();
                }
            }
            else
            {
                UnRunNSudo($"{name}");
            }
        }

        private void button_Taskmgr_Click(object sender, EventArgs e)
        {
            Run("Taskmgr.exe");
        }

        private void button_Cmd_Click(object sender, EventArgs e)
        {
            Run("cmd.exe");
        }

        private void button_PowerShell_Click(object sender, EventArgs e)
        {
            Run("powershell.exe");
        }

        private void button_Regedit_Click(object sender, EventArgs e)
        {
            Run("regedit.exe");
        }

        private void button_SystemInfo32_Click(object sender, EventArgs e)
        {
            Run("msinfo32.exe");
        }

        private void button_mmc_Click(object sender, EventArgs e)
        {
            Run("mmc.exe");
        }

        private void button_mmc_compmgmt_Click(object sender, EventArgs e)
        {
            Run("mmc.exe", "compmgmt.msc");
        }

        private void button_mmc_services_Click(object sender, EventArgs e)
        {
            Run("mmc.exe", "services.msc");
        }

        private void button_mmc_comexp_Click(object sender, EventArgs e)
        {
            Run("mmc.exe", "comexp.msc");
        }

        private void button_mmc_taskschd_Click(object sender, EventArgs e)
        {
            Run("mmc.exe", "taskschd.msc");
        }

        private void button_mmc_devmgmt_Click(object sender, EventArgs e)
        {
            Run("mmc.exe", "devmgmt.msc");
        }

        private void button_mmc_lusrmgr_Click(object sender, EventArgs e)
        {
            Run("mmc.exe", "lusrmgr.msc");
        }

        private void button_mmc_diskmgmt_Click(object sender, EventArgs e)
        {
            Run("mmc.exe", "diskmgmt.msc");
        }

        private void button_mmc_eventvwr_Click(object sender, EventArgs e)
        {
            Run("mmc.exe", "eventvwr.msc");
        }

        private void button_mmc_perfmon_Click(object sender, EventArgs e)
        {
            Run("mmc.exe", "perfmon.msc");
        }

        private void button_mmc_fsmgmt_Click(object sender, EventArgs e)
        {
            Run("mmc.exe", "fsmgmt.msc");
        }

        private void button_mmc_certlm_Click(object sender, EventArgs e)
        {
            Run("mmc.exe", "certlm.msc");
        }

        private void button_WMIC_Click(object sender, EventArgs e)
        {
            Run("wmic.exe");
        }

        private void button_Py_Click(object sender, EventArgs e)
        {
            Run("python.exe");
        }

        private void comboBox_RunMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SystemPermissionLauncher_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // 显示可拖放的光标
                e.Effect = DragDropEffects.Copy;

                // 可选：改变窗口外观提示用户
                this.BackColor = Color.LightBlue;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void SystemPermissionLauncher_DragDrop(object sender, DragEventArgs e)
        {
            // 恢复窗口外观
            this.BackColor = SystemColors.Control;

            // 获取拖入的文件路径数组
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // 处理文件
            ProcessFiles(files);
        }

        private void ProcessFiles(string[] files)
        {
            foreach (string file in files)
            {
                // 检查是否为文件（不是文件夹）
                if (File.Exists(file))
                {
                    string extension = Path.GetExtension(file);

                    switch (extension) 
                    {
                        case ".exe":
                            Run(file);
                            break;

                        case ".py":
                            Run("python.exe", file);
                            break;

                        case ".reg":
                            Run("regedit.exe", file);
                            break;

                        default:
                            Run(file);
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("目前仅支持拖入文件，不支持拖入文件夹或其他内容");
                    Log.Warn("[YtDlp工具] 用户拖入了非文件或不存在内容");
                }
            }
        }
    }
}
