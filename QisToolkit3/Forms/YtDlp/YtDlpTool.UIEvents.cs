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





        private void button_ArchiveEnhanced_Load_Click(object sender, EventArgs e)
        {
            comboBox_ArchiveEnhanced_Root.Items.Clear();
            comboBox_ArchiveEnhanced_Users.Items.Clear();
            comboBox_ArchiveEnhanced_PlayList.Items.Clear();

            if (!Directory.Exists(YtDlpPathArchivePlusPath))
            {
                MessageBox.Show("ArchivePlus目录不存在：" + YtDlpPathArchivePlusPath);
                return;
            }

            LoadArchiveEnhancedRoots();
        }

        private void button_ArchiveEnhanced_Save_Click(object sender, EventArgs e)
        {
            string fullPath = GetSelectedArchiveFilePath();
            if (string.IsNullOrEmpty(fullPath))
            {
                MessageBox.Show("请先选择一个归档文件。");
                return;
            }

            if (!File.Exists(fullPath))
            {
                MessageBox.Show("文件不存在：" + fullPath);
                return;
            }

            try
            {
                string text = richTextBox_ArchiveEnhanced.Text ?? string.Empty;

                // 统一换行符为 \r\n
                text = text.Replace("\r\n", "\n")
                           .Replace("\r", "\n")
                           .Replace("\n", "\r\n");

                // 可选：保存前备份
                // File.Copy(fullPath, fullPath + ".bak", true);

                // UTF-8 无 BOM 写入
                File.WriteAllText(fullPath, text, new UTF8Encoding(false));

                MessageBox.Show("保存成功。");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败：" + ex.Message);
            }
        }

        private void button_ArchiveEnhanced_Resolve_Click(object sender, EventArgs e)
        {
            string url = comboBox_URL.Text?.Trim();
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("请先输入或选择 URL。");
                return;
            }

            string archivePath = TryResolveEnhancedArchivePath(url, actualDirectory);
            if (string.IsNullOrEmpty(archivePath) || !File.Exists(archivePath))
            {
                MessageBox.Show("未找到对应的归档文件。\nURL: " + url);
                return;
            }

            if (string.IsNullOrEmpty(YtDlpPathArchivePlusPath))
                YtDlpPathArchivePlusPath = Path.Combine(actualDirectory, "yt-dlp", "Archive");

            if (!Directory.Exists(YtDlpPathArchivePlusPath))
            {
                MessageBox.Show("归档目录不存在：" + YtDlpPathArchivePlusPath);
                return;
            }

            if (comboBox_ArchiveEnhanced_Root.Items.Count == 0)
                LoadArchiveEnhancedRoots();

            string rootFull = Path.GetFullPath(YtDlpPathArchivePlusPath)
                                  .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string fileFull = Path.GetFullPath(archivePath);

            if (!fileFull.StartsWith(rootFull + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("文件不在 Archive 目录下：\n" + archivePath);
                return;
            }

            string relative = fileFull.Substring(rootFull.Length + 1);
            string[] parts = relative.Split(
                new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar },
                StringSplitOptions.RemoveEmptyEntries);

            // 支持两种层级：
            //   Root/xxx.txt          -> parts.Length == 2
            //   Root/User/yyy.txt     -> parts.Length == 3
            if (parts.Length < 2)
            {
                MessageBox.Show("归档文件层级不符合预期：\n" + relative);
                return;
            }

            string rootName = parts[0];
            string userName = parts[1];
            string playListName = parts.Length >= 3 ? parts[2] : null;

            if (!SelectComboBoxItem(comboBox_ArchiveEnhanced_Root, rootName))
            {
                MessageBox.Show("未在 Archive 中找到根目录：" + rootName);
                return;
            }

            if (!SelectComboBoxItem(comboBox_ArchiveEnhanced_Users, userName))
            {
                MessageBox.Show("未在 " + rootName + " 中找到用户项：" + userName);
                return;
            }

            // 用户级文件：PlayList 为空，内容已由 Users 的 SelectedIndexChanged 加载
            if (playListName == null)
                return;

            if (!SelectComboBoxItem(comboBox_ArchiveEnhanced_PlayList, playListName))
            {
                MessageBox.Show("未在 " + rootName + "\\" + userName + " 中找到 PlayList 文件：" + playListName);
                return;
            }
        }

        private void button_ArchiveEnhanced_Reverse_Click(object sender, EventArgs e)
        {
            string fullPath = GetSelectedArchiveFilePath();
            if (string.IsNullOrEmpty(fullPath))
            {
                MessageBox.Show("请先在三层下拉框中选好一个归档文件。");
                return;
            }

            if (!File.Exists(fullPath))
            {
                MessageBox.Show("文件不存在：\n" + fullPath);
                return;
            }

            if (string.IsNullOrEmpty(YtDlpPathArchivePlusPath))
                YtDlpPathArchivePlusPath = Path.Combine(actualDirectory, "yt-dlp", "Archive");

            string rootName = comboBox_ArchiveEnhanced_Root.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(rootName))
            {
                MessageBox.Show("请先选择一个 Archive 根目录。");
                return;
            }

            try
            {
                if (rootName.Equals("BiliBili", StringComparison.OrdinalIgnoreCase))
                {
                    var r = BilibiliUrlParser.ReverseFromArchivePath(YtDlpPathArchivePlusPath, fullPath);
                    if (r.IsValid)
                    {
                        comboBox_URL.Text = r.Url;
                        Log.Info($"[YtDlp工具] 反向解析 BiliBili {r.Description} => {r.Url}");
                    }
                    else
                    {
                        MessageBox.Show("无法反推出 URL：\n" + r.Description);
                    }
                }
                else if (rootName.Equals("YouTube", StringComparison.OrdinalIgnoreCase))
                {
                    var r = YouTubeUrlParser.ReverseFromArchivePath(YtDlpPathArchivePlusPath, fullPath);
                    if (r.IsValid)
                    {
                        comboBox_URL.Text = r.Url;
                        Log.Info($"[YtDlp工具] 反向解析 YouTube {r.Description} => {r.Url}");
                    }
                    else
                    {
                        MessageBox.Show("无法反推出 URL：\n" + r.Description);
                    }
                }
                else
                {
                    MessageBox.Show("未知的 Archive 根目录：" + rootName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("反推失败：" + ex.Message);
            }
        }

        private void button_ArchiveEnhanced_Delete_Click(object sender, EventArgs e)
        {
            string fullPath = GetSelectedArchiveFilePath();
            if (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath))
            {
                MessageBox.Show("请先选择一个存在的归档文件。");
                return;
            }

            if (MessageBox.Show("确定删除以下归档文件吗？\n\n" + fullPath,
                    "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                string currentRoot = comboBox_ArchiveEnhanced_Root.SelectedItem?.ToString();

                // 1) 删除文件本身
                File.Delete(fullPath);

                // 2) 如果是「用户目录」下的文件，且该用户目录删完后为空，则删除用户目录
                string parentDir = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrEmpty(currentRoot)
                    && !string.IsNullOrEmpty(parentDir)
                    && Directory.Exists(parentDir))
                {
                    string rootPath = Path.Combine(YtDlpPathArchivePlusPath, currentRoot);

                    // 父目录必须比 Root 更深（即它是用户目录，不是 Root 目录）
                    bool isUserDir = parentDir.Length > rootPath.Length
                                  && parentDir.StartsWith(
                                        rootPath + Path.DirectorySeparatorChar,
                                        StringComparison.OrdinalIgnoreCase);

                    if (isUserDir && Directory.GetFileSystemEntries(parentDir).Length == 0)
                    {
                        Directory.Delete(parentDir, false);
                        Log.Info($"[YtDlp工具] 用户目录已空，删除：{parentDir}");
                    }
                }

                // 3) 清空显示 + 刷新下拉框（尽量保持 Root 选中位置）
                richTextBox_ArchiveEnhanced.Clear();
                ReloadArchiveEnhancedCombos(currentRoot);
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败：" + ex.Message);
            }
        }

        private static bool SelectComboBoxItem(ComboBox cb, string value)
        {
            if (cb == null || string.IsNullOrEmpty(value))
                return false;

            for (int i = 0; i < cb.Items.Count; i++)
            {
                if (string.Equals(cb.Items[i]?.ToString(), value, StringComparison.OrdinalIgnoreCase))
                {
                    cb.SelectedIndex = i;
                    return true;
                }
            }
            return false;
        }

        private void LoadArchiveEnhancedRoots()
        {
            comboBox_ArchiveEnhanced_Root.Items.Clear();
            comboBox_ArchiveEnhanced_Users.Items.Clear();
            comboBox_ArchiveEnhanced_PlayList.Items.Clear();

            if (!Directory.Exists(YtDlpPathArchivePlusPath))
                return;

            string[] dirs = Directory.GetDirectories(YtDlpPathArchivePlusPath);
            Array.Sort(dirs, StringComparer.OrdinalIgnoreCase);

            foreach (string dir in dirs)
            {
                string name = Path.GetFileName(dir);
                if (!string.IsNullOrEmpty(name))
                    comboBox_ArchiveEnhanced_Root.Items.Add(name);
            }

            if (comboBox_ArchiveEnhanced_Root.Items.Count > 0)
                comboBox_ArchiveEnhanced_Root.SelectedIndex = 0;
        }

        private void comboBox_ArchiveEnhanced_Root_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_ArchiveEnhanced_Users.Items.Clear();
            comboBox_ArchiveEnhanced_PlayList.Items.Clear();

            if (string.IsNullOrEmpty(YtDlpPathArchivePlusPath) ||
                comboBox_ArchiveEnhanced_Root.SelectedItem == null)
                return;

            string rootName = comboBox_ArchiveEnhanced_Root.SelectedItem.ToString();
            string rootPath = Path.Combine(YtDlpPathArchivePlusPath, rootName);

            if (!Directory.Exists(rootPath))
                return;

            // 1) 子目录（用户目录）
            var dirNames = new List<string>();
            foreach (string dir in Directory.GetDirectories(rootPath))
            {
                string name = Path.GetFileName(dir);
                if (!string.IsNullOrEmpty(name))
                    dirNames.Add(name);
            }
            dirNames.Sort(StringComparer.OrdinalIgnoreCase);

            // 2) root 级直接存在的 txt 文件（archive.txt、8299159.txt 等）
            var fileNames = new List<string>();
            foreach (string file in Directory.GetFiles(rootPath, "*.txt", SearchOption.TopDirectoryOnly))
            {
                string name = Path.GetFileName(file);
                if (!string.IsNullOrEmpty(name))
                    fileNames.Add(name);
            }
            fileNames.Sort(StringComparer.OrdinalIgnoreCase);

            // 目录在前，文件在后
            foreach (string name in dirNames)
                comboBox_ArchiveEnhanced_Users.Items.Add(name);
            foreach (string name in fileNames)
                comboBox_ArchiveEnhanced_Users.Items.Add(name);

            if (comboBox_ArchiveEnhanced_Users.Items.Count > 0)
                comboBox_ArchiveEnhanced_Users.SelectedIndex = 0;
        }

        private void comboBox_ArchiveEnhanced_Users_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_ArchiveEnhanced_PlayList.Items.Clear();

            if (string.IsNullOrEmpty(YtDlpPathArchivePlusPath) ||
                comboBox_ArchiveEnhanced_Root.SelectedItem == null ||
                comboBox_ArchiveEnhanced_Users.SelectedItem == null)
            {
                richTextBox_ArchiveEnhanced.Clear();
                return;
            }

            string rootName = comboBox_ArchiveEnhanced_Root.SelectedItem.ToString();
            string userName = comboBox_ArchiveEnhanced_Users.SelectedItem.ToString();

            string rootPath = Path.Combine(YtDlpPathArchivePlusPath, rootName);
            string userPath = Path.Combine(rootPath, userName);

            // 情况 A：Users 选中的是 root 级 txt 文件
            if (File.Exists(userPath))
            {
                try
                {
                    richTextBox_ArchiveEnhanced.Text = ReadTextFileAutoEncoding(userPath);
                }
                catch (Exception ex)
                {
                    richTextBox_ArchiveEnhanced.Text = "读取失败：" + ex.Message;
                }
                return;
            }

            // 情况 B：Users 选中的是用户目录
            if (!Directory.Exists(userPath))
                return;

            string[] files = Directory.GetFiles(userPath, "*.txt", SearchOption.TopDirectoryOnly);
            Array.Sort(files, StringComparer.OrdinalIgnoreCase);

            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                if (!string.IsNullOrEmpty(name))
                    comboBox_ArchiveEnhanced_PlayList.Items.Add(name);
            }

            if (comboBox_ArchiveEnhanced_PlayList.Items.Count > 0)
                comboBox_ArchiveEnhanced_PlayList.SelectedIndex = 0;
            else
                richTextBox_ArchiveEnhanced.Clear();
        }

        private void comboBox_ArchiveEnhanced_PlayList_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox_ArchiveEnhanced.Clear();

            if (comboBox_ArchiveEnhanced_PlayList.SelectedItem == null)
                return;

            string fullPath = GetSelectedArchiveFilePath();
            if (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath))
                return;

            try
            {
                richTextBox_ArchiveEnhanced.Text = ReadTextFileAutoEncoding(fullPath);
            }
            catch (Exception ex)
            {
                richTextBox_ArchiveEnhanced.Text = "读取失败：" + ex.Message;
                Log.Err("[Archive] ", ex);
            }
        }

        // - Users 是文件  -> 返回该文件路径
        // - Users 是目录 + PlayList 有选中 -> 返回目录下 PlayList 路径
        // - 其它 -> null
        private string GetSelectedArchiveFilePath()
        {
            if (string.IsNullOrEmpty(YtDlpPathArchivePlusPath) ||
                comboBox_ArchiveEnhanced_Root.SelectedItem == null ||
                comboBox_ArchiveEnhanced_Users.SelectedItem == null)
                return null;

            string rootName = comboBox_ArchiveEnhanced_Root.SelectedItem.ToString();
            string userName = comboBox_ArchiveEnhanced_Users.SelectedItem.ToString();

            string rootPath = Path.Combine(YtDlpPathArchivePlusPath, rootName);
            string userPath = Path.Combine(rootPath, userName);

            // 用户级文件（archive.txt / 8299159.txt 等）
            if (File.Exists(userPath))
                return userPath;

            // 目录：需要 PlayList 有选中项
            if (comboBox_ArchiveEnhanced_PlayList.SelectedItem == null)
                return null;

            string playListName = comboBox_ArchiveEnhanced_PlayList.SelectedItem.ToString();
            return Path.Combine(userPath, playListName);
        }

        private static string ReadTextFileAutoEncoding(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);

            // 1) 带 BOM
            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
                return Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);

            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
                return Encoding.Unicode.GetString(bytes, 2, bytes.Length - 2);

            if (bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF)
                return Encoding.BigEndianUnicode.GetString(bytes, 2, bytes.Length - 2);

            // 2) 无 BOM：先严格按 UTF-8 解码，失败再回退 GBK
            try
            {
                var utf8Strict = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false,
                                                  throwOnInvalidBytes: true);
                return utf8Strict.GetString(bytes);
            }
            catch (DecoderFallbackException)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                return Encoding.GetEncoding("GBK").GetString(bytes);
            }
        }

        /// <summary>
        /// 重新加载 ArchiveEnhanced 三层下拉框，尽量保持原来选中的 Root
        /// </summary>
        private void ReloadArchiveEnhancedCombos(string preferRoot)
        {
            LoadArchiveEnhancedRoots();   // 内部会选中第一个 Root 并触发 Users / PlayList 联动

            if (!string.IsNullOrEmpty(preferRoot)
                && SelectComboBoxItem(comboBox_ArchiveEnhanced_Root, preferRoot))
            {
                // SelectComboBoxItem 已经切换 Root，触发 Users / PlayList 刷新
                return;
            }

            if (comboBox_ArchiveEnhanced_Root.Items.Count > 0)
                comboBox_ArchiveEnhanced_Root.SelectedIndex = 0;
            else
                richTextBox_ArchiveEnhanced.Clear();
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
