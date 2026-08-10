using QisToolkit3.Forms.YtDlp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Qi;
using static Qi.QisToolkit3_Datas;

namespace QisToolkit3.Forms
{
    public partial class YtDlpTool
    {
        private async void button_DoAnalysis_Click(object sender, EventArgs e)
        {
            ProcessingUserData();

            Log.Info($"[YtDlp工具] 开始执行解析 {comboBox_URL.Text}");

            if (NoST(comboBox_URL.Text) != string.Empty)
            {
                //string command = YtDlpPath;
                string command = string.Empty;

                // 使用 Cookies
                if (checkBox_UseCookies.Checked)
                    command += $" --cookies \"{CookiesFilePath}\"";

                // 字幕列表查看
                if (checkBox_ListSubs.Checked)
                    command += $@" --list-subs";

                // 缩略图列表查看
                if (checkBox_ListThumbnails.Checked)
                    command += $@" --list-thumbnails";

                command += $@" -F ""{comboBox_URL.Text}""";

                command = command.Trim();

                await ExecuteCommand(command);
            }
            else
            {
                Log.Warn($"[YtDlp工具] URL为空，解析失败");
                MessageBox.Show("URL不可为空", "错误");
            }
        }

        private async void button_DoDownload_Click(object sender, EventArgs e)
        {
            button_DoDownload.Enabled = false;
            button_AutoDownload.Enabled = false;
            button_DoAnalysis.Enabled = false;

            await DoDownload();

            button_DoDownload.Enabled = true;
            button_AutoDownload.Enabled = true;
            button_DoAnalysis.Enabled = true;
        }

        private async void button_AutoDownload_Click(object sender, EventArgs e)
        {
            await DoAutoDownload();
        }

        private async void button_RunRuleEngine_Click(object sender, EventArgs e)
        {
            // 规则引擎重命名
            if (checkBox_StringRuleEngine.Checked && !string.IsNullOrWhiteSpace(richTextBox_StringRuleEngine.Text))
            {
                string downloadPath = checkBox_Path_Home.Checked ?
                    comboBox_Path_Home.Text :
                    (checkBox_SetPaths.Checked ? textBox_Paths.Text : DefaultDownloadPath);

                // 确保路径是绝对路径
                if (!Path.IsPathRooted(downloadPath))
                {
                    downloadPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(YtDlpExePath), downloadPath));
                }

                await RenameFilesWithRuleEngine(downloadPath);
            }
        }

        private void button_DoCopyCommand_Click(object sender, EventArgs e)
        {
            ProcessingUserData();

            string command = $"\"{YtDlpExePath}\" {MakeCommand()}";
            Clipboard.SetText(command);
            Log.Info($"[YtDlp工具] 复制命令 {command}");
            MessageBox.Show($"已复制命令\n\n{command}");
        }









        private async void button_Update_Click(object sender, EventArgs e)
        {
            await ExecuteCommand("-U");
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            Stop();

            button_DoDownload.Enabled = true;
            button_AutoDownload.Enabled = true;
            button_DoAnalysis.Enabled = true;
        }

        private void button_AddHeaders_Save_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(HeadersFilePath, richTextBox_AddHeaders.Text);
            }
            catch (Exception ex)
            {
                Log.Err($"写入 文件头文件 时出现错误：{ex.Message}");
            }
        }

        private void button_Option_Save_Click(object sender, EventArgs e)
        {
            richTextBox.Text = string.Empty;
            YtDlpConfigManager.ExportConfig(this, DefaultConfigFilePath);
        }

        private void button_Option_Load_Click(object sender, EventArgs e)
        {
            if (File.Exists(DefaultConfigFilePath))
                YtDlpConfigManager.ImportConfig(this, DefaultConfigFilePath);
        }

        private void button_Option_ReSet_Click(object sender, EventArgs e)
        {
            TryDeleteFile(DefaultConfigFilePath);
        }

        private void button_YtDlpRunDir_Click(object sender, EventArgs e)
        {
            Qi.ExplorerStart(YtDlpWorkDirPath);
        }

        private async void button_IdNameMapper_Load_Click(object sender, EventArgs e)
        {
            await LoadIdNameMapper(AutoDownloadNameFilePath);
        }

        private void button_AddHeaders_Load_Click(object sender, EventArgs e)
        {
            if (File.Exists(HeadersFilePath))
            {
                try
                {
                    richTextBox_AddHeaders.Text = File.ReadAllText(HeadersFilePath);
                }
                catch (Exception ex)
                {
                    Log.Err($"读取 文件头文件 时出现错误：{ex.Message}");
                }
            }
        }


        #region 快速打开文件

        private void button_OpenArchiveFile_Click(object sender, EventArgs e)
        {
            string ArchivePath = checkBox_ArchivePlus.Checked ?
                $"{actualDirectory}\\yt-dlp\\Archive\\archive.txt" : $"{actualDirectory}\\yt-dlp\\archive.txt";

            //Log.Info($"[YtDlp工具] ArchivePath：{ArchivePath}");

            // 文件不存在就创建
            if (!File.Exists(ArchivePath))
                File.WriteAllText(ArchivePath, string.Empty);

            // 打开文件
            Log.Info($"[YtDlp工具] 打开文件 {ArchivePath}");
            StartFile($"{ArchivePath}");
        }

        private void button_OpenCookiesFile_Click(object sender, EventArgs e)
        {
            // 文件不存在就创建
            if (!File.Exists(CookiesFilePath))
            {
                File.WriteAllText(CookiesFilePath,
                    "# Netscape HTTP Cookie File\r\n" +
                    "# This file was Create by Yt-Dlp Tool\r\n");
            }

            // 打开文件
            Log.Info($"[YtDlp工具] 打开文件 {CookiesFilePath}");
            StartFile($"{CookiesFilePath}");
        }

        private void button_OpenDownloadPath_Click(object sender, EventArgs e)
        {
            string path = textBox_Paths.Text;
            Directory.CreateDirectory(path);
            ExplorerStart(path);
        }

        private void button_OpenAutoDownloadPath_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(DefaultDownloadPath);
            ExplorerStart(DefaultDownloadPath);
        }

        private void button_AutoDownloadFile_Click(object sender, EventArgs e)
        {
            // 文件不存在就创建
            if (!File.Exists(AutoDownloadConfigFilePath))
            {
                CreateDefaultAutoDownloadConfig(AutoDownloadConfigFilePath);
            }

            // 打开文件
            Log.Info($"[YtDlp工具] 打开文件 {AutoDownloadConfigFilePath}");
            StartFile(AutoDownloadConfigFilePath);
        }

        private void button_OpenAutoDownloadNameFile_Click(object sender, EventArgs e)
        {
            // 文件不存在就创建
            if (!File.Exists(AutoDownloadNameFilePath))
            {
                File.WriteAllText(AutoDownloadNameFilePath,
                    "# 此文件用于自动映射ID与名称，一行一个。\r\n" +
                    "# ID与名称以空格分隔，名称后可识别空格。\r\n" +
                    "# 以 '#' 开头为注释行，自动跳过空行\r\n" +
                    "# 格式：[ID] [名称]\r\n\r\n");
            }

            // 打开文件
            Log.Info($"[YtDlp工具] 打开文件 {AutoDownloadNameFilePath}");
            StartFile($"{AutoDownloadNameFilePath}");
        }

        #endregion



        private void checkBox_SetPaths_CheckedChanged(object sender, EventArgs e) =>
            textBox_Paths.Enabled = checkBox_SetPaths.Checked;

        private void checkBox_ReadOnClipboard_CheckedChanged(object sender, EventArgs e) =>
            comboBox_URL.Enabled = !checkBox_ReadOnClipboard.Checked;



        private void checkBox_FormatSort_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_FormatSortForce.Enabled = checkBox_FormatSort.Checked;

            if (!checkBox_FormatSort.Checked)
                checkBox_FormatSortForce.Checked = false;
        }

        private void checkBox_ExtractAudio_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_KeepVideo.Enabled = checkBox_ExtractAudio.Checked;

            if (!checkBox_ExtractAudio.Checked)
                checkBox_KeepVideo.Checked = false;
        }

        private void checkBox_RemuxVideo_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_RemuxVideo.Checked && comboBox_RemuxVideo.Text == "mkv" ||
                checkBox_RecodeVideo.Checked && comboBox_RecodeVideo.Text == "mkv")
            {
                checkBox_Embed_InfoJson.Enabled = true;
            }

            else
            {
                checkBox_Embed_InfoJson.Enabled = false;
                checkBox_Embed_InfoJson.Checked = false;
            }
        }

        private void checkBox_WriteThumbnail_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_WriteThumbnail.Checked)
            {
                checkBox_WriteAllThumbnails.Enabled = false;
                checkBox_WriteAllThumbnails.Checked = false;
            }
            else if (!checkBox_WriteAllThumbnails.Checked && !checkBox_WriteThumbnail.Checked)
            {
                checkBox_WriteAllThumbnails.Enabled = true;
            }
        }

        private void checkBox_WriteAllThumbnails_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_WriteAllThumbnails.Checked)
            {
                checkBox_WriteThumbnail.Enabled = false;
                checkBox_WriteThumbnail.Checked = false;
            }
            else if (!checkBox_WriteAllThumbnails.Checked && !checkBox_WriteThumbnail.Checked)
            {
                checkBox_WriteThumbnail.Enabled = true;
            }
        }



        private void comboBox_URL_TextChanged(object sender, EventArgs e)
        {
            button_AutoDownload.Enabled = File.Exists(AutoDownloadConfigFilePath) ||
                                          File.Exists(comboBox_URL.Text);
        }

        private void checkBox_ArchivePlus_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox_MatchFilters_CheckedChanged(object sender, EventArgs e)
        {
            //button_SetMatchFilters.Enabled = checkBox_MatchFilters.Checked;
        }

        private void button_SetMatchFilters_Click(object sender, EventArgs e)
        {
            new YtDlpToolMatchFilters().Show();
        }




        private void button_Text_Click(object sender, EventArgs e)
        {
            MessageBox.Show("** 下载BiliBili的1080P视频需要浏览器上先登录，4K同理，需先登录且账号上有大会员\r\n\r\n由于Chrome浏览器的数据保护 API（DPAPI），导致无法读取浏览器读取 Cookie ，因此Chrome浏览器部分视频无法获取（例如BiliBili的1080P视频）\r\n\r\n解决方法：\r\n1. 使用 Edge / Firefox 浏览器\r\n2. 使用 Linux 操作系统。\r\n3. 导出 Cookies，然后使用 --cookies 参数\r\n4. 使用企业规则 ApplicationBoundEncryptionEnabled\r\n", "小提示");
        }

        private void button_Help_ArchivePlus_Click(object sender, EventArgs e)
        {
            //richTextBox.Text = "什么是archive增强？\r\n现在archive会对每一个合集或作者单独创建一个archive.txt\r\n" +
            //                   "这样可以有效提升archive的实用性\r\n所有创建的archive.txt都在.\\yt-dlp\\Archive下\r\n" +
            //                   "合集：Archive\\BiliBili\\{作者ID}\\{合集ID}.txt\r\n主页：Archive\\BiliBili\\{作者ID}.txt\r\n" +
            //                   "作品：Archive\\BiliBili\\archive.txt\r\n注意：此功能暂仅支持BiliBili";
            MessageBox.Show(
                "什么是archive增强？\r\n\r\n同时下载多个合集的内容并为下载过的内容添加黑名单，\r\n" +
                "若全部放在同一个archive.txt会难以辨别且过于臃肿。\r\n此功能就是为解决此问题而生。\r\n\r\n" +
                "现在archive会对每一个合集或作者单独创建一个archive.txt，方便管理，提升性能。\r\n" +
                "这样可以有效提升archive的实用性\r\n所有创建的archive.txt都在.\\yt-dlp\\Archive下\r\n" +
                "合集：Archive\\BiliBili\\{作者ID}\\{合集ID}.txt\r\n" +
                "主页：Archive\\BiliBili\\{作者ID}.txt\r\n" +
                "作品：Archive\\BiliBili\\archive.txt\r\n\r\n" +
                "注意：此功能暂仅支持BiliBili", "介绍",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_AddHeaders_Preset_Click(object sender, EventArgs e)
        {
            richTextBox_AddHeaders.Text =
                "User-Agent:Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/120.0.0.0 Safari/537.36\r\n" +
                "Accept:text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8\r\n" +
                "Accept-Language:en-US,en;q=0.9\r\n" +
                "Accept-Encoding:gzip, deflate, br\r\n" +
                "Referer:https://www.google.com/\r\n" +
                "Sec-Ch-Ua:\"Not_A Brand\";v=\"8\", \"Chromium\";v=\"120\"\r\n" +
                "Sec-Ch-Ua-Mobile:?0\r\n" +
                "Sec-Ch-Ua-Platform:\"Windows\"\r\n" +
                "Sec-Fetch-Dest:document\r\n" +
                "Sec-Fetch-Mode:navigate\r\n" +
                "Sec-Fetch-Site:none\r\n" +
                "Sec-Fetch-User:?1\r\n" +
                "Upgrade-Insecure-Requests:1";
        }

        private void button_SuperFriendlyMode_Click(object sender, EventArgs e)
        {
            checkBox_SleepRequests.Checked = true;
            comboBox_SleepRequests.Text = "5";

            checkBox_SleepInterval.Checked = true;
            checkBox_MaxSleepInterval.Checked = true;
            comboBox_SleepInterval.Text = "30";
            comboBox_MaxSleepInterval.Text = "60";

            checkBox_SleepSubtitles.Checked = true;
            comboBox_SleepSubtitles.Text = "10";

            checkBox_RetrySleep.Checked = true;
            comboBox_RetrySleep.Text = "exp=10:120:2";
        }





        private void YtDlpTool_DragEnter(object sender, DragEventArgs e)
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

        private void YtDlpTool_DragDrop(object sender, DragEventArgs e)
        {
            // 恢复窗口外观
            this.BackColor = SystemColors.Control;

            // 获取拖入的文件路径数组
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // 处理文件
            ProcessFiles(files);
        }

        private void YtDlpTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (_matchFiltersWatcher != null)
            //{
            //    _matchFiltersWatcher.EnableRaisingEvents = false;
            //    _matchFiltersWatcher.Dispose();
            //    _matchFiltersWatcher = null;
            //}
            //_reloadDebounceTimer?.Stop();
            //_reloadDebounceTimer?.Dispose();
        }
    }
}
