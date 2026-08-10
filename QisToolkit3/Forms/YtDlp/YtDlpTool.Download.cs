using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Qi;
using static Qi.QisToolkit3_Datas;

namespace QisToolkit3.Forms
{
    public partial class YtDlpTool
    {
        private async Task DoDownload(string customMatchFiltersData = null)
        {
            ProcessingUserData();

            Log.Info($"[YtDlp工具] 开始执行下载 {comboBox_URL.Text}");
            if (Inspect())
            {
                await ExecuteCommand(MakeCommand(customMatchFiltersData));

                // 生成信息页
                if (checkBox_GenerateInfoPage.Checked)
                {
                    AppendText("开始生成信息页...", "QisToolkit");

                    // 获取所有视频文件
                    var videoFiles = GetAllVideoFiles();
                    AppendText($"找到 {videoFiles.Length} 个视频文件", "QisToolkit");

                    foreach (var videoPath in videoFiles)
                    {
                        AppendText($"处理视频: {Path.GetFileName(videoPath)}", "QisToolkit");

                        string infoJsonPath = GetInfoJsonPath(videoPath);

                        if (!string.IsNullOrEmpty(infoJsonPath) && File.Exists(infoJsonPath))
                        {
                            string infoDir = Path.GetDirectoryName(infoJsonPath);

                            var generator = new YtDlp.VideoInfoPageGenerator(infoDir, (msg) =>
                            {
                                AppendText(msg, "InfoPage");
                            });

                            await generator.GenerateForVideo(videoPath, checkBox_GenerateInfoPage_EmbedBilibiliVideoPlayer.Checked);

                            // 删除原 JSON 文件
                            if (checkBox_GenerateInfoPage_DeleteJsonFile.Checked)
                            {
                                TryDeleteFile(infoJsonPath);
                                AppendText($"🗑️ 已删除 JSON: {Path.GetFileName(infoJsonPath)}", "QisToolkit");

                                // 同时删除 description 文件
                                string descPath = infoJsonPath.Replace(".信息.json", ".description").Replace(".info.json", ".description");
                                if (File.Exists(descPath))
                                {
                                    TryDeleteFile(descPath);
                                }
                            }
                        }
                        else
                        {
                            AppendText($"⚠️ 未找到信息文件: {Path.GetFileNameWithoutExtension(videoPath)}", "QisToolkit");
                        }
                    }

                    AppendText("信息页生成完成", "QisToolkit");
                }

                // 将字幕文件 XML 转换成 ASS
                if (checkBox_DanmakuFactory_XML_To_ASS.Checked)
                {
                    // 等待文件写入完成
                    //await Task.Delay(1000);

                    foreach (var file in GetXmlFiles())
                    {
                        string assFile = Path.Combine(
                            Path.GetDirectoryName(file),
                            Path.GetFileNameWithoutExtension(file).Replace(".danmaku", "") + ".ass"
                        );

                        // 确保 ASS 目录存在
                        Directory.CreateDirectory(Path.GetDirectoryName(assFile));

                        // 调用 DanmakuFactory
                        //await WaitForFileUnlock(file, 3000); // 确保文件可访问
                        await RunDanmakuFactory(file, assFile);

                        // 删除原 xml
                        if (checkBox_DanmakuFactory_XML_To_ASS_DeleteXMLFile.Checked)
                            TryDeleteFile(file);
                    }
                }

                // 规则引擎重命名
                if (checkBox_StringRuleEngine.Checked && !string.IsNullOrWhiteSpace(richTextBox_StringRuleEngine.Text))
                {
                    // 等待文件写入完成
                    await Task.Delay(1000);

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

                if (checkBox_OpenF.Checked)
                {
                    string currentDir = checkBox_Path_Home.Checked ?
                        comboBox_Path_Home.Text : checkBox_SetPaths.Checked ?
                        textBox_Paths.Text : AppDomain.CurrentDomain.BaseDirectory;

                    Log.Info($"[YtDlp工具] 打开文件夹 {currentDir}");
                    ExplorerStart(currentDir);
                    checkBox_OpenF.Checked = false;
                }
            }
        }

        private string MakeCommand(string customMatchFiltersData = null)
        {
            //string command = YtDlpPath;
            string command = string.Empty;

            // 使用 Cookies
            if (checkBox_UseCookies.Checked)
                command += $" --cookies \"{CookiesFilePath}\"";

            // 使用 Archive
            if (checkBox_UseArchive.Checked)
            {
                // 增强 Archive
                if (checkBox_ArchivePlus.Checked)
                {
                    var result = BilibiliUrlParser.ParseBilibiliUrl(comboBox_URL.Text, actualDirectory);
                    if (result.IsValid)
                    {
                        // 创建目录
                        result.EnsureArchiveDirectoryExists();

                        command += $" --download-archive \"{result.FullArchivePath}\"";
                    }

                    // 回退到普通 archive
                    else
                    {
                        Log.Warn("[YtDlp工具] 增强 Archive 模块无效，已回退 普通 Archive 模式");

                        command += $" --download-archive \"{actualDirectory}\\yt-dlp\\archive.txt\"";
                    }
                }

                // 普通 Archive
                else
                {
                    command += $" --download-archive \"{actualDirectory}\\yt-dlp\\archive.txt\"";
                }
            }

            #region 视频格式策略

            // 允许多视频流合并
            if (checkBox_VideoMultistreams.Checked)
                command += $" --video-multistreams";

            // 允许多音频流合并
            if (checkBox_AudioMultistreams.Checked)
                command += $" --audio-multistreams";

            // 优先选择开放格式
            if (checkBox_PreferFreeFormats.Checked)
                command += $" --prefer-free-formats";

            // 指定格式
            if (checkBox_Format.Checked)
                command += $" -f {comboBox_Format.Text}";

            // 排序策略：用户排序覆盖所有
            if (checkBox_FormatSortForce.Checked)
                command += $" --format-sort-force";

            // 排序策略
            if (checkBox_FormatSort.Checked)
                command += $" -S \"{checkBox_FormatSort.Text}\"";

            #endregion

            // URL文件
            if (File.Exists(comboBox_URL.Text))
                command += $@" -a ""{comboBox_URL.Text}""";

            // URL
            else
                command += $@" {comboBox_URL.Text}";

            // 详细自定义路径
            if (checkBox_Path_Home.Checked || checkBox_Path_Temp.Checked ||
                checkBox_Path_Video.Checked || checkBox_Path_Audio.Checked ||
                checkBox_Path_SubTitle.Checked || checkBox_Path_Description.Checked ||
                checkBox_Path_Thumbnail.Checked || checkBox_Path_InfoJson.Checked)
            {
                if (checkBox_Path_Home.Checked) command += $@" -P home:""{comboBox_Path_Home.Text}""";
                if (checkBox_Path_Temp.Checked) command += $@" -P temp:""{comboBox_Path_Temp.Text}""";
                if (checkBox_Path_Video.Checked) command += $@" -P video:""{comboBox_Path_Video.Text}""";
                if (checkBox_Path_Audio.Checked) command += $@" -P audio:""{comboBox_Path_Audio.Text}""";
                if (checkBox_Path_SubTitle.Checked) command += $@" -P subtitle:""{comboBox_Path_SubTitle.Text}""";
                if (checkBox_Path_Description.Checked) command += $@" -P description:""{comboBox_Path_Description.Text}""";
                if (checkBox_Path_Thumbnail.Checked) command += $@" -P thumbnail:""{comboBox_Path_Thumbnail.Text}""";
                if (checkBox_Path_InfoJson.Checked) command += $@" -P infojson:""{comboBox_Path_InfoJson.Text}""";
            }

            // 自定义路径
            else if (checkBox_SetPaths.Checked)
            {
                Directory.CreateDirectory(textBox_Paths.Text);
                command += $" --paths \"{textBox_Paths.Text}\"";
            }

            // 指定要下载播放列表中的哪些项目
            if (checkBox_playlist_items.Checked && !string.IsNullOrWhiteSpace(textBox_playlist_items.Text))
                command += $" -I {textBox_playlist_items.Text.Trim()}";

            #region 基础下载过滤与限制

            // 最大大小
            if (checkBox_MaxFileSize.Checked)
                command += $" --max-filesize {comboBox_MaxFileSize.Text.Trim()}";

            // 最小大小
            if (checkBox_MinFileSize.Checked)
                command += $" --min-filesize {comboBox_MinFileSize.Text.Trim()}";

            // 只下载指定日期上传的视频
            if (checkBox_Date.Checked)
                command += $" --date {comboBox_Date.Text.Trim()}";

            // 只下载指定日期或之前上传的视频
            if (checkBox_DateBefore.Checked)
                command += $" --datebefore {comboBox_DateBefore.Text.Trim()}";

            // 只下载指定日期或之后上传的视频
            if (checkBox_DateAfter.Checked)
                command += $" --dateafter {comboBox_DateAfter.Text.Trim()}";

            // 最多下载数
            if (checkBox_MaxDownloads.Checked)
                command += $" --max-downloads {comboBox_MaxDownloads.Text.Trim()}";

            // 年龄限制
            if (checkBox_AgeLimit.Checked)
                command += $" --age-limit {comboBox_AgeLimit.Text.Trim()}";

            #endregion

            #region 网络、重试机制等下载选项设置

            // 并发下载片段数量
            if (checkBox_ConcurrentFragments.Checked)
                command += $" -N {comboBox_ConcurrentFragments.Text.Trim()}";

            // 最大下载速率（bit/s）
            if (checkBox_LimitRate.Checked)
                command += $" -r {comboBox_LimitRate.Text.Trim()}";

            // 最小下载速率阈值（bit/s）
            if (checkBox_ThrottledRate.Checked)
                command += $" --throttled-rate {comboBox_ThrottledRate.Text.Trim()}";

            // 网络重试次数
            if (checkBox_Retries.Checked)
                command += $" -R {comboBox_Retries.Text.Trim()}";

            // 文件重试次数
            if (checkBox_FileAccessRetries.Checked)
                command += $" --file-access-retries {comboBox_FileAccessRetries.Text.Trim()}";

            // 片段重试次数
            if (checkBox_FragmentRetries.Checked)
                command += $" --fragment-retries {comboBox_FragmentRetries.Text.Trim()}";

            // 重试之间的休眠时间
            if (checkBox_RetrySleep.Checked)
                command += $" --retry-sleep {comboBox_RetrySleep.Text.Trim()}";

            // 跳过不可用的片段（默认）
            if (checkBox_Fragments_SU_AO.Checked)
                command += $" --skip-unavailable-fragments";

            // 如果片段不可用则中止下载
            else
                command += $" --abort-on-unavailable-fragments";

            // 下载完成后保留磁盘上的片段文件
            if (checkBox_KeepFragments.Checked)
                command += $" --keep-fragments";

            // 下载完成后删除片段文件（默认）
            else
                command += $" --no-keep-fragments";

            // 下载缓冲区大小
            if (checkBox_BufferSize.Checked)
                command += $" --buffer-size {comboBox_BufferSize.Text.Trim()}";

            // 自动调整缓冲区大小（默认，从--buffer-size的初始值开始）
            if (checkBox_ResizeBuffer.Checked)
                command += $" --resize-buffer";

            // 不自动调整缓冲区大小
            else
                command += $" --no-resize-buffer";

            // 播放列表处理模式
            if (checkBox_PlayListMode.Checked)
            {
                command += comboBox_PlayListMode.SelectedIndex switch
                {
                    0 => " --no-lazy-playlist",
                    1 => " --playlist-random",
                    2 => " --lazy-playlist",
                    _ => " --no-lazy-playlist",
                };
            }

            #endregion

            #region 元数据文件系统

            // 导出视频描述文本
            if (checkBox_WriteDescription.Checked)
                command += $" --write-description";

            // 完整元数据（含个人信息）
            if (checkBox_WriteInfoJson.Checked)
                command += $" --write-info-json";

            // 导出所有评论
            if (checkBox_WriteComments.Checked)
                command += $" --write-comments";

            // 导出播放列表信息
            if (checkBox_WritePlayListMetaFiles.Checked)
                command += $" --write-playlist-metafiles";

            #endregion

            #region 文件参数

            // 文件覆盖策略
            if (checkBox_OverWritesMode.Checked)
            {
                command += comboBox_OverWritesMode.SelectedIndex switch
                {
                    0 => " --no-overwrites",
                    1 => " --no-force-overwrites",
                    2 => " --force-overwrites",
                    _ => " --no-force-overwrites",
                };
            }

            // 使用服务器时间
            if (checkBox_MTime.Checked)
                command += $" --mtime";

            // 禁用 续传
            if (checkBox_NoContinue.Checked)
                command += $" --no-continue";

            // 禁用 Part 文件
            if (checkBox_NoPart.Checked)
                command += $" --no-part";

            // 跳过下载视频
            if (checkBox_SkipDownload.Checked)
                command += $" --skip-download";

            #endregion

            #region 字幕

            // 下载手动字幕
            if (checkBox_WriteSubs.Checked)
                command += $" --write-subs";

            // 下载自动字幕
            if (checkBox_WriteAutoSubs.Checked)
                command += $" --write-auto-subs";

            // 字幕格式
            if (checkBox_SubFormat.Checked)
                command += $" --sub-format {comboBox_SubFormat.Text}";

            // 字幕格式
            if (checkBox_SubLangs.Checked)
                command += $" --sub-langs {comboBox_SubLangs.Text}";

            #endregion

            #region 认证

            // 账号密码登录
            if (checkBox_UsingUP.Checked)
                command += $" -u \"{textBox_UserName.Text}\" -p \"{textBox_PassWord.Text}\"";

            // 双因素认证（2FA）
            if (checkBox_2FA.Checked)
                command += $" -2 \"{textBox_2FA.Text}\"";

            // 视频特定密码
            if (checkBox_VideoPassWord.Checked)
                command += $" --video-password \"{textBox_VideoPassWord.Text}\"";

            // netrc
            if (checkBox_netrc.Checked)
            {
                command += $" --netrc";

                // 指定 netrc文件
                if (checkBox_netrc_Location.Checked)
                    command += $" --netrc-location \"{textBox_netrc_Location.Text}\"";
            }

            // Adobe Pass
            if (checkBox_AdobePass.Checked)
                command +=
                    $" --ap-mso {comboBox_AP_MSO.Text}" +
                    $" --ap-username \"{textBox_AP_UserName.Text}\"" +
                    $" --ap-password \"{textBox_AP_PassWord.Text}\"";

            // SSL 证书文件
            if (checkBox_ClientCertificate.Checked)
                command += $" --client-certificate \"{textBox_ClientCertificate.Text}\"";

            // SSL 私钥文件
            if (checkBox_ClientCertificate_Key.Checked)
                command += $" --client-certificate-key \"{textBox_ClientCertificate_Key.Text}\"";

            // SSL 私钥密码
            if (checkBox_ClientCertificate_PassWord.Checked)
                command += $" --client-certificate-password \"{textBox_ClientCertificate_PassWord.Text}\"";

            #endregion

            #region 音频提取 视频封装

            // 音频提取
            if (checkBox_AudioFormat.Checked)
                command += $" -x";

            // 音频格式
            if (checkBox_AudioFormat.Checked)
                command += $" --audio-format {comboBox_AudioFormat.Text}";

            // 音频质量
            if (checkBox_AudioQuality.Checked)
                command += $" --audio-quality {comboBox_AudioQuality.Text}";

            // 视频重新封装
            if (checkBox_RemuxVideo.Checked)
                command += $" --remux-video {comboBox_RemuxVideo.Text}";

            // 视频重新编码
            if (checkBox_RecodeVideo.Checked)
                command += $" --recode-video {comboBox_RecodeVideo.Text}";

            // 保留中间文件
            if (checkBox_KeepVideo.Checked)
                command += $" -k";

            // 跳过已存在的后处理文件
            if (checkBox_NoPostOverwrites.Checked)
                command += $" --no-post-overwrites";

            #endregion

            #region 嵌入内容 与 其他转换

            // 嵌入字幕
            if (checkBox_Embed_Subs.Checked)
                command += $" --embed-subs";

            // 嵌入缩略图
            if (checkBox_Embed_Thumbnail.Checked)
                command += $" --embed-thumbnail";

            // 嵌入元数据
            if (checkBox_Embed_Metadata.Checked)
                command += $" --embed-metadata";

            // 嵌入章节
            if (checkBox_Embed_Chapters.Checked)
                command += $" --embed-chapters";

            // 嵌入infojson
            if (checkBox_Embed_InfoJson.Checked)
                command += $" --embed-info-json";

            // 字幕转换
            if (checkBox_Convert_Subs.Checked)
                command += $" --convert-subs {comboBox_Convert_Subs.Text}";

            // 缩略图转换
            if (checkBox_Convert_Thumbnails.Checked)
                command += $" --convert-thumbnails {comboBox_Convert_Thumbnails.Text}";

            #endregion

            // 指定下载片段
            if (checkBox_DownloadSections_Start.Checked)
            {
                if (checkBox_DownloadSections_End.Checked)
                    command += $" --download-sections \"*{comboBox_DownloadSections_Start.Text}-{comboBox_DownloadSections_End.Text}\"";
                else
                    command += $" --download-sections \"*{comboBox_DownloadSections_Start.Text}-inf\"";
            }

            // 后期处理器参数传递
            if (checkBox_PostProcessorArgs.Checked)
                command += $" --ppa {comboBox_PostProcessorArgs.Text}";

            // 不使用列表
            if (checkBoxNoPlaylest.Checked)
                command += $" --no-playlist";

            // 详细输出模式/调试模式
            if (checkBox_Verbose.Checked)
                command += $" -v";

            // 安静模式
            if (checkBox_Quiet.Checked)
                command += $" -q";

            if (checkBox_MatchFilters.Checked || !string.IsNullOrEmpty(customMatchFiltersData))
            {
                List<string> rulesToUse;
                if (!string.IsNullOrEmpty(customMatchFiltersData))
                {
                    // 自定义规则（分号分隔多条）
                    rulesToUse = customMatchFiltersData
                        .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(r => r.Trim())
                        .Where(r => !string.IsNullOrEmpty(r))
                        .ToList();
                }
                else
                {
                    // 使用全局规则
                    rulesToUse = _matchFiltersRules;
                }

                foreach (string rule in rulesToUse)
                {
                    if (!string.IsNullOrEmpty(rule))
                        command += $" --match-filters \"{rule}\"";
                }
            }

            // Http头数据
            if (checkBox_AddHeaders.Checked)
            {
                try
                {
                    foreach (string line in richTextBox_AddHeaders.Lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line)) // 跳过空行
                        {
                            command += $" --add-headers \"{line}\"";
                        }
                    }
                }
                catch
                {

                }
            }

            // exec
            if (!string.IsNullOrWhiteSpace(richTextBox_exec.Text))
            {
                command += ProcessExecArguments(richTextBox_exec.Text);
            }

            // 去除头尾的空格
            command = command.Trim();

            Log.Info($"[YtDlp工具] 命令创建：{command}");
            return command;
        }

        private async Task ExecuteCommand(string command)
        {
            outputBox.Text = string.Empty;
            var tcs = new TaskCompletionSource<bool>();

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = YtDlpExePath,
                WorkingDirectory = YtDlpWorkDirPath,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = command
            };

            process = new Process
            {
                StartInfo = psi,
                EnableRaisingEvents = true
            };

            // 设置进程退出时通知
            process.Exited += (sender, e) =>
            {
                tcs.TrySetResult(true);
            };

            process.OutputDataReceived += (sender, e) => AppendText(e.Data);
            process.ErrorDataReceived += (sender, e) => AppendText(e.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // 等待进程退出
            await tcs.Task;
        }

        private async Task RunDanmakuFactory(string file, string assfile)
        {
            outputBox.Text = string.Empty;
            //string ytDlpPath = YtDlpPath.Contains(' ') ? $"\"{YtDlpPath}\"" : YtDlpPath;

            var tcs = new TaskCompletionSource<bool>();

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = DanmakuFactoryPath,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = $"-i \"{file}\" -o \"{assfile}\""
            };

            process = new Process
            {
                StartInfo = psi,
                EnableRaisingEvents = true
            };

            // 设置进程退出时通知
            process.Exited += (sender, e) =>
            {
                tcs.TrySetResult(true);
            };

            process.OutputDataReceived += (sender, e) => AppendText(e.Data, "DanmakuFactory");
            process.ErrorDataReceived += (sender, e) => AppendText(e.Data, "DanmakuFactory");

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // 等待进程退出
            await tcs.Task;
        }
    }
}
