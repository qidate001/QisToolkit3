using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Qi.QisToolkit3_Datas;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QisToolkit3.Forms
{
    public partial class ExtendedFeatures : Form
    {
        // 添加Shell32 API声明
        public class Shell32
        {
            [System.Runtime.InteropServices.DllImport("shell32.dll")]
            public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
        }

        private static readonly HttpClient _httpClient = new HttpClient();
        // 添加一个CancellationTokenSource用于支持取消操作（可选）
        private CancellationTokenSource _cancellationTokenSource;

        public ExtendedFeatures()
        {
            InitializeComponent();
            LoadText();

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void ExtendedFeatures_Load(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(Path.Combine(actualDirectory, "ElseTool"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误", $"第三方工具目录初始化失败，请尝试手动创建.\\ElseTool\n错误信息：{ex.Message}\n\n完整报错：{ex}");
            }
        }

        private void LoadText()
        {
            button_qicmd_delete.Enabled = File.Exists(@"C:\Windows\q.exe");
            button_mas_delete.Enabled = File.Exists(actualDirectory + @"\ElseTool\Mas.cmd");

            if (File.Exists(@"C:\Windows\q.exe"))
                button_qicmd_download.Text = GetBtnText(2);

            else if (File.Exists(actualDirectory + @"\ElseTool\QiCmd.exe"))
                button_qicmd_download.Text = GetBtnText(0);

            else button_qicmd_download.Text = GetBtnText(1);


            if (File.Exists(actualDirectory + @"\ElseTool\Mas.cmd"))
                button_mas_download.Text = GetBtnText(2);

            else button_mas_download.Text = GetBtnText(1);

            
        }

        private static string GetBtnText(int Type)
        {
            switch (Language)
            {
                case "en":
                    switch (Type)
                    {
                        case 0:
                            return "Install";
                        case 1:
                            return "Download";
                        case 2:
                            return "Re-download";
                    }
                    return "Download";

                case "zh-CN":
                default:
                    switch (Type)
                    {
                        case 0:
                            return "安装";
                        case 1:
                            return "下载";
                        case 2:
                            return "重新下载";
                    }
                    return "下载";
            }
        }

        // 可选：添加取消按钮的事件处理
        private void button_cancel_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }

        // 在窗体关闭时清理资源
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            base.OnFormClosing(e);
        }



        static async Task DownloadFileAsync(string url, string outputPath)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "QiCmd-Downloader");

            using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var totalBytes = response.Content.Headers.ContentLength ?? -1L;
            var canReportProgress = totalBytes != -1;

            using var stream = await response.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream(outputPath, FileMode.Create);

            var buffer = new byte[8192];
            var totalBytesRead = 0L;
            var bytesRead = 0;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await fileStream.WriteAsync(buffer, 0, bytesRead);
                totalBytesRead += bytesRead;

                if (canReportProgress)
                {
                    var percentage = (double)totalBytesRead / totalBytes * 100;
                    System.Console.Write($"\r下载进度: {percentage:F1}% ({totalBytesRead / 1024} KB)");
                }
            }

            System.Console.WriteLine(); // 换行
        }

        private void InstallQiCmd()
        {
            try
            {
                File.Copy(actualDirectory + @"\ElseTool\QiCmd.exe", @"C:\Windows\q.exe", true);
                RegisterQBatFileAssociation();

                MessageBox.Show($"安装完成！\n现在您在cmd或运行中输入 q 即可打开 QiCmd！", "QiCmd", MessageBoxButtons.OK, MessageBoxIcon.Question);

                /*if (tmp == DialogResult.OK)
                    Qi.RunNSudo(@"C:\Windows\q.exe"); */
            }
            catch (Exception ex)
            {
                MessageBox.Show($"安装失败: {ex.Message}", "报错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button_qicmd_download_Click(object sender, EventArgs e)
        {
            string downloadUrl = $"https://github.com/qidate/QiCmd/releases/download/{comboBox_QiCmd.Text}/QiCmd.exe";

            button_qicmd_download.Enabled = false;

            if (File.Exists(actualDirectory + @"\ElseTool\QiCmd.exe") && !File.Exists(@"C:\Windows\q.exe"))
            {
                InstallQiCmd();
                LoadText();
                button_qicmd_download.Enabled = true;
                return;
            }

            try
            {
                Qi.TryDeleteFile(actualDirectory + @"\ElseTool\QiCmd.exe");
                System.Console.WriteLine($"开始下载 QiCmd {comboBox_QiCmd.Text}...");
                await DownloadFileAsync(downloadUrl, actualDirectory + @"\ElseTool\QiCmd.exe");

                if (File.Exists(actualDirectory + @"\ElseTool\QiCmd.exe"))
                {
                    var tmp = MessageBox.Show("下载已完成，是否立即安装？", "QiCmd", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (tmp == DialogResult.OK)
                        InstallQiCmd();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"下载失败: {ex.Message}", "报错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            button_qicmd_download.Enabled = true;
            LoadText();
        }

        private void button_qicmd_delete_Click(object sender, EventArgs e)
        {
            Qi.TryDeleteFile(@"C:\Windows\q.exe");
            Qi.TryDeleteFile(actualDirectory + @"\ElseTool\QiCmd.exe");
            UnregisterQBatFileAssociation();
            MessageBox.Show("卸载完成", "QiCmd", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadText();
        }

        public static bool RegisterQBatFileAssociation()
        {
            try
            {
                // 1. 创建 .qbat 文件扩展名关联
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(".qbat"))
                {
                    key.SetValue("", "QiCmd.qbat.file");
                    key.SetValue("Content Type", "text/plain");
                    key.SetValue("PerceivedType", "text");

                    // 添加这些值来避免首次确认对话框
                    key.SetValue("QiCmdSelected", 1, RegistryValueKind.DWord);
                }

                // 2. 创建文件类型描述
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey("QiCmd.qbat.file"))
                {
                    key.SetValue("", "QiCmd Batch File");
                    key.SetValue("FriendlyTypeName", "QiCmd Batch File");
                    key.SetValue("EditFlags", 0x00000000, RegistryValueKind.DWord); // 避免首次确认

                    // 设置默认图标
                    using (RegistryKey defaultIconKey = key.CreateSubKey("DefaultIcon"))
                    {
                        defaultIconKey.SetValue("", "C:\\WINDOWS\\q.exe,0");
                    }
                }

                // 3. 创建打开命令
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@"QiCmd.qbat.file\shell\open\command"))
                {
                    key.SetValue("", "\"C:\\WINDOWS\\q.exe\" \"%1\" %*");
                }

                // 4. 设置为首选操作
                using (RegistryKey shellKey = Registry.ClassesRoot.CreateSubKey(@"QiCmd.qbat.file\shell"))
                {
                    shellKey.SetValue("", "open"); // 设置open为默认操作
                }

                // 5. 在CurrentUser中设置用户选择（重要！）
                SetUserChoice();

                System.Console.WriteLine("QiCmd .qbat 文件关联注册成功！");

                // 通知系统刷新关联
                RefreshSystem();

                return true;
            }
            catch (SecurityException ex)
            {
                System.Console.WriteLine($"权限不足，请以管理员身份运行: {ex.Message}");
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Console.WriteLine($"访问被拒绝: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"注册失败: {ex.Message}");
                return false;
            }
        }

        // 设置用户选择以避免首次确认对话框
        private static void SetUserChoice()
        {
            try
            {
                // 在HKCU中设置用户已经做出选择
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.qbat\UserChoice"))
                {
                    // 创建Progid值表示用户已选择
                    key.SetValue("Progid", "QiCmd.qbat.file");
                    key.SetValue("Hash", CalculateHash("QiCmd.qbat.file"));
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"设置用户选择失败: {ex.Message}");
            }
        }

        // 简单的哈希计算（实际Windows使用更复杂的算法）
        private static string CalculateHash(string progId)
        {
            // 这是一个简化的示例，实际应该使用Windows使用的算法
            return BitConverter.ToString(Encoding.UTF8.GetBytes(progId)).Replace("-", "");
        }

        // 刷新系统文件关联
        private static void RefreshSystem()
        {
            try
            {
                // 通知资源管理器刷新
                Shell32.SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
            }
            catch
            {
                // 忽略错误
            }
        }

        public static bool UnregisterQBatFileAssociation()
        {
            try
            {
                // 删除所有相关的注册表项
                string[] keysToDelete = {
                @".qbat",
                @"QiCmd.qbat.file"
            };

                foreach (string keyName in keysToDelete)
                {
                    Registry.ClassesRoot.DeleteSubKeyTree(keyName, false);
                }

                // 删除用户选择
                try
                {
                    Registry.CurrentUser.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.qbat", false);
                }
                catch { }

                System.Console.WriteLine("QiCmd .qbat 文件关联已卸载！");
                RefreshSystem();
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"卸载失败: {ex.Message}");
                return false;
            }
        }

        // 检查是否已注册
        public static bool IsQBatAssociationRegistered()
        {
            try
            {
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(".qbat"))
                {
                    if (key != null)
                    {
                        string value = key.GetValue("") as string;
                        return value == "QiCmd.qbat.file";
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // MAS 下载
        private async void button_mas_download_Click(object sender, EventArgs e)
        {
            const string downloadUrl = $"https://dev.azure.com/massgrave/Microsoft-Activation-Scripts/_apis/git/repositories/Microsoft-Activation-Scripts/items?path=/MAS/All-In-One-Version-KL/MAS_AIO.cmd&download=true";

            button_mas_download.Enabled = false;

            try
            {
                Qi.TryDeleteFile(actualDirectory + @"\ElseTool\Mas.cmd");
                System.Console.WriteLine($"开始下载 Mas...");
                await DownloadFileAsync(downloadUrl, actualDirectory + @"\ElseTool\Mas.cmd");

                if (File.Exists(actualDirectory + @"\ElseTool\Mas.cmd"))
                {
                    var tmp = MessageBox.Show("下载已完成，您可以在万能工具→第三方工具处运行了！请问是否立即运行？", "Mas", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (tmp == DialogResult.OK)
                        new Tools().buttonMAS_Click(null, null);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"下载失败: {ex.Message}", "报错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            button_mas_download.Enabled = true;
            LoadText();
        }

        private void button_mas_delete_Click(object sender, EventArgs e)
        {
            Qi.TryDeleteFile(actualDirectory + @"\ElseTool\Mas.cmd");
            MessageBox.Show("已删除", "Mas", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadText();
        }
    }
}