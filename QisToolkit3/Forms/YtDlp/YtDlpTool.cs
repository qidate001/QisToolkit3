using QisToolkit3.Forms.YtDlp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static Qi;
using static Qi.QisToolkit3_Datas;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QisToolkit3.Forms
{
    public partial class YtDlpTool : Form
    {
        private Process process;
        private RichTextBox outputBox;
        private IdNameMapper _idNameMapper;
        private List<string> _matchFiltersRules = new List<string>();
        public static event Action MatchFiltersSaved;
        //private FileSystemWatcher _matchFiltersWatcher;
        //private System.Windows.Forms.Timer _reloadDebounceTimer;  // 用于防抖
        //private bool _isReloading = false;  // 防止并发重载

        // 文件路径
        private static string YtDlpPath = Path.Combine(actualDirectory, "yt-dlp");
        private static string DefaultDownloadPath = Path.Combine(YtDlpPath, "Downloads");
        private static string YtDlpExePath = Path.Combine(YtDlpPath, "yt-dlp.exe");
        private static string DanmakuFactoryPath = Path.Combine(YtDlpPath, "DanmakuFactory.exe");
        private static string CookiesFilePath = Path.Combine(YtDlpPath, "cookies.txt");
        private static string AutoDownloadFilePath = Path.Combine(YtDlpPath, "AutoDownload.txt");
        private static string AutoDownloadNameFilePath = Path.Combine(YtDlpPath, "AutoDownloadName.txt");
        private static string HeadersFilePath = Path.Combine(YtDlpPath, "Headers.txt");
        private static string DefaultConfigFilePath = Path.Combine(YtDlpPath, "DefaultConfig.xml");
        private static string AutoDownloadConfigFilePath = Path.Combine(YtDlpPath, "AutoDownloadConfig.txt");
        public static string MatchFiltersPath = Path.Combine(YtDlpPath, "MatchFilters.txt");
        public static string YtDlpWorkDirPath = YtDlpPath;


        public YtDlpTool()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);
            comboBox_StringRuleEngine.SelectedIndex = 0;

            //if (YtDlpPath.Contains(' '))
            //{
            //    Log.Err($"[YtDlp工具] 齐的工具包3路径包含空格或特殊符号，无法运行YtDlp。");
            //    MessageBox.Show("齐的工具包3路径包含空格或特殊符号，无法运行YtDlp。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    button_DoDownload.Enabled = false;
            //    button_AutoDownload.Enabled = false;
            //    button_DoAnalysis.Enabled = false;
            //    button_Update.Enabled = false;
            //    button_Stop.Enabled = false;
            //}

            if (!File.Exists(actualDirectory + @"\yt-dlp\ffmpeg.exe"))
            {
                Log.Warn($"[YtDlp工具] 工具环境缺失！这可能会导致一些问题。");
                MessageBox.Show("环境缺失！这可能会导致一些问题", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            outputBox = richTextBox;
        }

        private async void YtDlpTool_Load(object sender, EventArgs e)
        {
            // 下载存放路径
            //textBox_Paths.Text = DefaultDownloadPath;
            //comboBox_Path_Home.Text = DefaultDownloadPath;
            //comboBox_Path_Temp.Text = DefaultDownloadPath;
            //comboBox_Path_Video.Text = DefaultDownloadPath;
            //comboBox_Path_Audio.Text = DefaultDownloadPath;
            //comboBox_Path_SubTitle.Text = DefaultDownloadPath;
            //comboBox_Path_Description.Text = DefaultDownloadPath;
            //comboBox_Path_Thumbnail.Text = DefaultDownloadPath;
            //comboBox_Path_InfoJson.Text = DefaultDownloadPath;

            // .netrc 文件
            textBox_netrc_Location.Text = Path.Combine(actualDirectory, @$"yt-dlp") + '\\';

            button_AutoDownload.Enabled = File.Exists(comboBox_URL.Text) || File.Exists(AutoDownloadFilePath);
            checkBox_IdNameMapper.Checked = File.Exists(AutoDownloadNameFilePath);

            int IsCookies = IsCookieFile(CookiesFilePath);
            checkBox_UseCookies.Checked = IsCookies == 1 || IsCookies == 2;

            comboBox_PlayListMode.SelectedIndex = 0;
            comboBox_OverWritesMode.SelectedIndex = 1;

            if (File.Exists(DefaultConfigFilePath))
                YtDlpConfigManager.ImportConfig(this, DefaultConfigFilePath);

            // 加载自动下载名称映射
            await LoadIdNameMapper(AutoDownloadNameFilePath);

            if (string.IsNullOrWhiteSpace(textBox_Paths.Text))
                textBox_Paths.Text = Path.Combine(actualDirectory, "yt-dlp", "Downloads");

            LoadMatchFilters();

            MatchFiltersSaved += () =>
            {
                if (this.InvokeRequired)
                    this.Invoke(new Action(LoadMatchFilters));
                else
                    LoadMatchFilters();
                Log.Info("[YtDlp工具] 收到规则已保存通知，已重新加载");
            };
        }

        // 解析
        private void button_DoAnalysis_Click(object sender, EventArgs e)
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

                ExecuteCommand(command);
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

        /// <summary>
        /// 获取所有视频文件
        /// </summary>
        private string[] GetAllVideoFiles()
        {
            string ytDlpDir = Path.GetDirectoryName(YtDlpExePath);
            var allVideos = new List<string>();

            // 获取 home 路径
            string homePath = GetHomePath();

            // 搜索所有可能的目录
            string[] searchPaths = new[] { homePath };

            // 如果有 Video 路径，也加入
            if (checkBox_Path_Video.Checked && !string.IsNullOrEmpty(comboBox_Path_Video.Text))
            {
                string videoPath = comboBox_Path_Video.Text;
                if (!Path.IsPathRooted(videoPath))
                {
                    videoPath = Path.GetFullPath(Path.Combine(ytDlpDir, videoPath));
                }
                searchPaths = searchPaths.Append(videoPath).ToArray();
            }

            foreach (var path in searchPaths.Distinct())
            {
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path, "*.mp4", SearchOption.AllDirectories);
                    allVideos.AddRange(files);
                    AppendText($"在 {path} 找到 {files.Length} 个视频", "QisToolkit");
                }
            }

            return allVideos.Distinct().ToArray();
        }

        /// <summary>
        /// 获取所有下载文件
        /// </summary>
        private string[] GetAllDownloadFiles()
        {
            string ytDlpDir = Path.GetDirectoryName(YtDlpExePath);
            var allVideos = new List<string>();

            // 获取 home 路径
            string homePath = GetHomePath();

            // 搜索所有可能的目录
            string[] searchPaths = new[] { homePath };

            // 如果有 Video 路径，也加入
            if (checkBox_Path_Video.Checked && !string.IsNullOrEmpty(comboBox_Path_Video.Text))
            {
                string videoPath = comboBox_Path_Video.Text;
                if (!Path.IsPathRooted(videoPath))
                {
                    videoPath = Path.GetFullPath(Path.Combine(ytDlpDir, videoPath));
                }
                searchPaths = searchPaths.Append(videoPath).ToArray();
            }

            foreach (var path in searchPaths.Distinct())
            {
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                    allVideos.AddRange(files);
                    AppendText($"在 {path} 找到 {files.Length} 个文件", "QisToolkit");
                }
            }

            return allVideos.Distinct().ToArray();
        }

        /// <summary>
        /// 获取 Home 路径（基础下载目录）
        /// </summary>
        private string GetHomePath()
        {
            string ytDlpDir = Path.GetDirectoryName(YtDlpExePath);

            // 优先使用 Home 路径
            if (checkBox_Path_Home.Checked && !string.IsNullOrEmpty(comboBox_Path_Home.Text))
            {
                string homePath = comboBox_Path_Home.Text;
                if (!Path.IsPathRooted(homePath))
                {
                    homePath = Path.GetFullPath(Path.Combine(ytDlpDir, homePath));
                }
                return homePath;
            }

            // 使用默认路径
            string defaultPath = textBox_Paths.Text;
            if (string.IsNullOrEmpty(defaultPath))
            {
                defaultPath = Path.Combine(ytDlpDir, "Downloads");
            }
            else if (!Path.IsPathRooted(defaultPath))
            {
                defaultPath = Path.GetFullPath(Path.Combine(ytDlpDir, defaultPath));
            }

            return defaultPath;
        }

        /// <summary>
        /// 获取弹幕文件列表
        /// </summary>
        private string[] GetXmlFiles()
        {
            string ytDlpDir = Path.GetDirectoryName(YtDlpExePath);
            var allXmlFiles = new List<string>();

            // 获取 home 路径
            string homePath = "";
            if (checkBox_Path_Home.Checked && !string.IsNullOrEmpty(comboBox_Path_Home.Text))
            {
                homePath = comboBox_Path_Home.Text;
                if (!Path.IsPathRooted(homePath))
                {
                    homePath = Path.GetFullPath(Path.Combine(ytDlpDir, homePath));
                }
            }
            else
            {
                homePath = textBox_Paths.Text;
                if (string.IsNullOrEmpty(homePath))
                {
                    homePath = Path.Combine(ytDlpDir, "Downloads");
                }
                else if (!Path.IsPathRooted(homePath))
                {
                    homePath = Path.GetFullPath(Path.Combine(ytDlpDir, homePath));
                }
            }

            // 获取 subtitle 相对路径
            if (checkBox_Path_SubTitle.Checked && !string.IsNullOrEmpty(comboBox_Path_SubTitle.Text))
            {
                string subRelPath = comboBox_Path_SubTitle.Text;
                string subPath = Path.Combine(homePath, subRelPath);

                if (Directory.Exists(subPath))
                {
                    var files = Directory.GetFiles(subPath, "*.danmaku.xml", SearchOption.AllDirectories);
                    allXmlFiles.AddRange(files);
                    Log.Info($"从 SubTitle 路径找到 {files.Length} 个弹幕文件: {subPath}");
                }
            }

            // 也在 home 根目录找一下
            if (Directory.Exists(homePath))
            {
                var files = Directory.GetFiles(homePath, "*.danmaku.xml", SearchOption.TopDirectoryOnly);
                allXmlFiles.AddRange(files);
                Log.Info($"从 Home 路径找到 {files.Length} 个弹幕文件: {homePath}");
            }

            var result = allXmlFiles.Distinct().ToArray();
            Log.Info($"总共找到 {result.Length} 个弹幕文件");
            return result;
        }

        /// <summary>
        /// 获取 InfoJson 文件路径
        /// </summary>
        private string GetInfoJsonPath(string videoPath)
        {
            string videoName = Path.GetFileNameWithoutExtension(videoPath);
            string ytDlpDir = Path.GetDirectoryName(YtDlpExePath);

            // 获取 home 路径（基础目录）
            string homePath = "";
            if (checkBox_Path_Home.Checked && !string.IsNullOrEmpty(comboBox_Path_Home.Text))
            {
                homePath = comboBox_Path_Home.Text;
                if (!Path.IsPathRooted(homePath))
                {
                    homePath = Path.GetFullPath(Path.Combine(ytDlpDir, homePath));
                }
            }
            else
            {
                // 默认 home 路径
                homePath = textBox_Paths.Text;
                if (string.IsNullOrEmpty(homePath))
                {
                    homePath = Path.Combine(ytDlpDir, "Downloads");
                }
                else if (!Path.IsPathRooted(homePath))
                {
                    homePath = Path.GetFullPath(Path.Combine(ytDlpDir, homePath));
                }
            }

            // 获取 infojson 相对路径
            string infoJsonRelPath = "";
            if (checkBox_Path_InfoJson.Checked && !string.IsNullOrEmpty(comboBox_Path_InfoJson.Text))
            {
                infoJsonRelPath = comboBox_Path_InfoJson.Text;
            }

            // 组合完整路径
            string infoJsonDir = string.IsNullOrEmpty(infoJsonRelPath)
                ? homePath
                : Path.Combine(homePath, infoJsonRelPath);

            // 尝试文件名（支持 .info.json 和 .信息.json）
            string[] possibleExtensions = { ".info.json", ".信息.json" };

            foreach (var ext in possibleExtensions)
            {
                string jsonPath = Path.Combine(infoJsonDir, videoName + ext);
                if (File.Exists(jsonPath))
                {
                    Log.Info($"找到信息文件: {jsonPath}");
                    return jsonPath;
                }
            }

            Log.Warn($"未找到信息文件，尝试路径: {infoJsonDir}\\{videoName}.info.json 或 .信息.json");
            return "";
        }

        private void button_DoCopyCommand_Click(object sender, EventArgs e)
        {
            ProcessingUserData();

            string command = $"\"{YtDlpExePath}\" {MakeCommand()}";
            Clipboard.SetText(command);
            Log.Info($"[YtDlp工具] 复制命令 {command}");
            MessageBox.Show($"已复制命令\n\n{command}");
        }

        private async Task RenameFilesWithRuleEngine(string directoryPath)
        {
            if (!checkBox_StringRuleEngine.Checked ||
                string.IsNullOrWhiteSpace(richTextBox_StringRuleEngine.Text))
                return;

            AppendText("开始应用规则引擎重命名文件...", "QisToolkit");

            var ruleEngine = new RuleEngine();
            var rules = richTextBox_StringRuleEngine.Text;

            // 获取所有文件
            var videoFiles = GetAllDownloadFiles();
            int renamedCount = 0;

            foreach (var videoPath in videoFiles)
            {
                try
                {
                    string fileName = Path.GetFileNameWithoutExtension(videoPath);
                    string extension = Path.GetExtension(videoPath);
                    string directory = Path.GetDirectoryName(videoPath);

                    // 应用规则引擎处理文件名
                    string newFileName = ruleEngine.ProcessText(fileName, rules);

                    // 清理非法字符
                    newFileName = IdNameMapper.SanitizeFileName(newFileName);

                    // 如果新文件名与旧文件名不同，则重命名
                    if (!string.Equals(newFileName, fileName, StringComparison.OrdinalIgnoreCase))
                    {
                        string newPath = Path.Combine(directory, newFileName + extension);

                        // 处理重名情况
                        if (File.Exists(newPath))
                        {
                            if (comboBox_StringRuleEngine.SelectedIndex == 0)
                            {
                                AppendText($"✅ 保护模式跳过文件: {fileName}{extension}", "QisToolkit");
                            }

                            else if (comboBox_StringRuleEngine.SelectedIndex == 1)
                            {
                                AppendText($"✅ 强制模式删除文件: {fileName}{extension}", "QisToolkit");
                                TryDeleteFile(newPath);
                                File.Move(videoPath, newPath);
                                renamedCount++;
                            }
                        }

                        else
                        {
                            File.Move(videoPath, newPath);
                            renamedCount++;
                        }


                        AppendText($"✅ 重命名: {fileName}{extension} → {newFileName}{extension}", "QisToolkit");

                        // 同时重命名关联的 .info.json 和 .description 文件
                        RenameAssociatedFiles(directory, fileName, newFileName);
                    }
                }
                catch (Exception ex)
                {
                    AppendText($"❌ 重命名失败 {Path.GetFileName(videoPath)}: {ex.Message}", "QisToolkit");
                    Log.Err($"重命名文件失败: {ex.Message}");
                }
            }

            AppendText($"规则引擎重命名完成，共重命名 {renamedCount} 个文件", "QisToolkit");
        }

        /// <summary>
        /// 重命名关联文件（.info.json, .description 等）
        /// </summary>
        private void RenameAssociatedFiles(string directory, string oldName, string newName)
        {
            string[] extensions = { ".info.json", ".信息.json", ".description", ".danmaku.xml", ".ass", ".html" };

            foreach (var ext in extensions)
            {
                string oldPath = Path.Combine(directory, oldName + ext);
                string newPath = Path.Combine(directory, newName + ext);
                Log.Info(oldPath + " - " + oldPath);

                if (File.Exists(oldPath))
                {
                    try
                    {
                        // 处理重名情况
                        if (File.Exists(newPath))
                        {
                            if (comboBox_StringRuleEngine.SelectedIndex == 0)
                            {
                                File.Move(oldPath, newPath);
                                AppendText($"   📎 跳过重命名关联: {oldName}{ext}", "QisToolkit");
                                TryDeleteFile(newPath);
                            }

                            else if (comboBox_StringRuleEngine.SelectedIndex == 1)
                            {
                                AppendText($"   📎 覆盖重命名关联: {oldName}{ext}", "QisToolkit");
                                TryDeleteFile(newPath);
                                File.Move(oldPath, newPath);
                            }
                        }

                        else
                        {
                            File.Move(oldPath, newPath);
                            AppendText($"   📎 重命名关联: {oldName}{ext} → {newName}{ext}", "QisToolkit");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Warn($"[YtDlp工具] 重命名关联文件失败 {oldName}{ext}: {ex.Message}");
                    }
                }
            }
        }

        

        private string ProcessExecArguments(string lines)
        {
            var result = new StringBuilder();
            // 按行分割，移除空行
            var lineArray = lines.Split(new[] { Environment.NewLine, "\n", "\r\n" },
                                         StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lineArray)
            {
                var trimmedLine = line.Trim();

                // 跳过空行
                if (string.IsNullOrEmpty(trimmedLine))
                    continue;

                // 跳过注释行
                if (trimmedLine.StartsWith('#') || trimmedLine.StartsWith("//"))
                    continue;

                // 检查是否已经被引号包裹
                bool isQuoted = trimmedLine.Length >= 2 &&
                                trimmedLine.StartsWith("\"") &&
                                trimmedLine.EndsWith("\"");

                trimmedLine = trimmedLine.Replace("%(Qi:ActualDirectory)", actualDirectory);
                trimmedLine = trimmedLine.Replace("%(Qi.YtDlpTool:Paths)", textBox_Paths.Text);
                trimmedLine = trimmedLine.Replace("\"", "\\\"");

                if (isQuoted)
                {
                    result.Append($" --exec {trimmedLine}");
                }
                else if (trimmedLine.Contains(' '))
                {
                    // 包含空格，需要加引号
                    result.Append($" --exec \"{trimmedLine}\"");
                }
                else
                {
                    result.Append($" --exec {trimmedLine}");
                }
            }

            return result.ToString();
        }

        // 
        public async Task LoadIdNameMapper(string mapFilePath)
        {
            _idNameMapper = new IdNameMapper(mapFilePath);
            try
            {
                await _idNameMapper.LoadMapAsync();
                Log.Info($"成功加载 {_idNameMapper.GetMapCount()} 个ID映射");
            }
            catch (Exception ex)
            {
                Log.Err($"加载映射文件失败: {ex.Message}");
                // 如果映射文件加载失败，仍然继续运行，只是不使用名称替换
                _idNameMapper = null;
            }
        }

        

        // 处理用户数据
        private void ProcessingUserData()
        {
            string containsText = string.Empty;

            if (checkBox_ReadOnClipboard.Checked)
            {
                containsText = GetContainsText();


                if (!string.IsNullOrWhiteSpace(containsText))
                    comboBox_URL.Text = containsText;
            }

            if (checkBox_ClearURLData.Checked)
                comboBox_URL.Text = ClearURLData(comboBox_URL.Text);
        }

        private string ClearURLData(string url)
        {
            string _url = string.Empty;

            if (url != string.Empty && url.Contains('&'))
                for (int i = 0; i < url.Length; i++)
                    if (url[i] == '&')
                        break;
                    else
                        _url += url[i];
            else
                return url;

            return _url;
        }

        private string GetContainsText()
        {
            try
            {
                if (!Clipboard.ContainsText())
                {
                    Log.Warn("[YtDlp工具] 剪贴板中没有文本内容。");
                    MessageBox.Show("剪贴板中没有文本内容。");
                    return null;
                }

                string text = Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(text))
                {
                    Log.Warn("[YtDlp工具] 剪贴板文本为空。");
                    MessageBox.Show("剪贴板文本为空。");
                    return null;
                }

                // 匹配 URL（支持 http/https，可扩展其他协议）
                const string urlPattern = @"https?://[^\s]+";
                Match match = Regex.Match(text, urlPattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    string url = match.Value;
                    Log.Info($"[YtDlp工具] 剪贴板文本成功提取 URL: {url}");
                    return url;
                }
                else
                {
                    Log.Warn("[YtDlp工具] 剪贴板中未找到有效的 URL。");
                    MessageBox.Show("剪贴板中未找到有效的 URL。");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Err($"[YtDlp工具] 访问剪贴板失败: {ex.Message}");
                MessageBox.Show($"访问剪贴板失败: {ex.Message}");
                return null;
            }
        }

        private bool Inspect()
        {
            if (NoST(comboBox_URL.Text) == string.Empty)
            {
                MessageBox.Show("URL不可为空", "错误");
                return false;
            }
            return true;
        }

        private void checkBox_SetPaths_CheckedChanged(object sender, EventArgs e) =>
            textBox_Paths.Enabled = checkBox_SetPaths.Checked;

        private void button_Text_Click(object sender, EventArgs e)
        {
            MessageBox.Show("** 下载BiliBili的1080P视频需要浏览器上先登录，4K同理，需先登录且账号上有大会员\r\n\r\n由于Chrome浏览器的数据保护 API（DPAPI），导致无法读取浏览器读取 Cookie ，因此Chrome浏览器部分视频无法获取（例如BiliBili的1080P视频）\r\n\r\n解决方法：\r\n1. 使用 Edge / Firefox 浏览器\r\n2. 使用 Linux 操作系统。\r\n3. 导出 Cookies，然后使用 --cookies 参数\r\n4. 使用企业规则 ApplicationBoundEncryptionEnabled\r\n", "小提示");
        }

        

        

        private void AppendText(string text, string form = "YtDlp")
        {
            if (string.IsNullOrEmpty(text)) return;


            //汉化提示
            if (checkBox_ChineseTip.Checked)
            {
                text = text.Replace("Destination", "目标");
                text = text.Replace("Danmaku Pool: ", "弹幕池：");
                text = text.Replace("Read timed out", "读取超时");
                text = text.Replace("Language Formats", "语言格式");
                text = text.Replace("Extracting URL", "正在提取URL");
                text = text.Replace("Writting file", "正在写入文件");
                text = text.Replace("Downloading item", "正在下载项目");
                text = text.Replace("Available formats for", "可用格式");
                text = text.Replace("HTTPSConnectionPool", "HTTPS连接池");
                text = text.Replace("Available subtitles for", "可用语言");
                text = text.Replace("Downloading webpage", "正在下载网页");
                text = text.Replace("Merging formats into", "将格式合并到");
                text = text.Replace("Adding thumbnail to", "正在添加缩略图");
                text = text.Replace("has already been downloaded", "已下载");
                text = text.Replace("EXT RESOLUTION FPS", "外部分辨率 帧率");
                text = text.Replace("File Loading Complete", "文件加载完成");
                text = text.Replace("Deleting original file", "删除原始文件");
                text = text.Replace("Downloading wbi sign", "正在下载WIB标志");
                text = text.Replace("Available thumbnails for ", "可用缩略图 ");
                text = text.Replace("Downloading playlist", "正在下载播放列表");
                text = text.Replace("Downloading playlist:", "正在下载播放列表：");
                text = text.Replace("Finished downloading playlist: ", "完成列表下载");
                text = text.Replace("Extracting videos in anthology", "正在选集中提取视频");
                text = text.Replace("Downloading video formats for cid", "正在下载cid视频格式");
                text = text.Replace("for how to manually pass cookies", "了解如何手动传递Cookie");
                text = text.Replace("has already been recorded in the archive", "已经记录在档案（黑名单）中");
                text = text.Replace("add --no-playlist to download just the video", "添加参数--no-playlist，将不再下载播放列表，只下载视频");
                text = text.Replace("are missing; you have to become a premium member to download them. Use --cookies-from-browser or --cookies for the authentication. See", "未寻获；你必须成为高级会员下载它们。使用浏览器中的Cookie或用于身份验证的Cookie。见");

                text = text.Replace("VBR ACODEC", "可变比特率音频编解码器");
                text = text.Replace("Extracting chapters", "提取章节");
                text = text.Replace("Loading files", "加载文件列表");
                text = text.Replace("video thumbnail", "视频缩略图");
                text = text.Replace("EmbedThumbnail", "嵌入缩略图");
                text = text.Replace("Loading file", "加载文件");
                text = text.Replace("Downloading", "正在下载");
                text = text.Replace("Output file", "输出文件");
                text = text.Replace("Input file", "输入文件");
                text = text.Replace("Playlist ", "播放列表");
                text = text.Replace("TBR PROTO", "TBR 协议");
                text = text.Replace("TimeShift", "时间偏移");
                text = text.Replace("Got error", "出现错误");
                text = text.Replace("VCODEC", "视频解码器");
                text = text.Replace("video only", "仅视频");
                text = text.Replace("audio only", "仅音频");
                text = text.Replace("FILESIZE", "文件大小");
                text = text.Replace("Exec", "外部程序调用");
                text = text.Replace("FileName ", "文件名");
                text = text.Replace("items of", "个项目");
                text = text.Replace("ABR", "可用比特率");
                text = text.Replace("download", "下载");
                text = text.Replace("Template", "模板");
                text = text.Replace("Retrying", "重试");
                text = text.Replace("Sorting", "排序");
                text = text.Replace("Unknown", "未知");
                text = text.Replace("unknown", "未知");
                text = text.Replace("Writing", "写入");
                text = text.Replace("Format", "格式");
                text = text.Replace("format", "格式");
                text = text.Replace("Merger", "合并");
                text = text.Replace("Height", "高度");
                text = text.Replace("Number", "编号");
                text = text.Replace("Width", "宽度");
                text = text.Replace("ERROR", "错误");
                text = text.Replace("info", "信息");
                text = text.Replace("Done", "完成");
                text = text.Replace("host", "主机");
                text = text.Replace("port", "端口");
                text = text.Replace("of", "总计");

                text = text.Replace("Unable to obtain version", "无法获取版本");
                text = text.Replace("certificate verify failed", "证书验证失败");
                text = text.Replace("unable to get local issuer certificate", "无法获取本地颁发者证书");
                text = text.Replace("Please try again later or visit", "请稍后再试或直接访问");
            }



            // 跨线程更新UI控件
            if (outputBox.InvokeRequired)
            {
                outputBox.Invoke(new Action<string, string>(AppendText), text, form);
            }
            else
            {
                outputBox.AppendText(text + Environment.NewLine);
                outputBox.ScrollToCaret(); // 自动滚动到底部

                // 添加到日志
                Log.Info($"[{form}] {text}");
            }
        }

        public void Stop()
        {
            process.Close();
            richTextBox.Text += "\n已终止进程。";
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            Stop();

            button_DoDownload.Enabled = true;
            button_AutoDownload.Enabled = true;
            button_DoAnalysis.Enabled = true;
        }

        private void checkBox_ReadOnClipboard_CheckedChanged(object sender, EventArgs e) =>
            comboBox_URL.Enabled = !checkBox_ReadOnClipboard.Checked;

        private void button_Update_Click(object sender, EventArgs e)
        {
            ExecuteCommand("-U");
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

        private void ProcessFiles(string[] files)
        {
            foreach (string file in files)
            {
                // 检查是否为文件（不是文件夹）
                if (File.Exists(file))
                {
                    Log.Info($"[YtDlp工具] 开始处理拖入的文件：{file}");

                    int type = IsCookieFile(file);
                    switch (type)
                    {
                        // 发生错误
                        case -1:
                            Log.Err($"[YtDlp工具] 处理拖入的文件 {file} 时出现意料之外的异常");
                            break;

                        // 文件不存在（按理来说不可能，已经验证过一次了）
                        case 0:
                            Log.Warn($"[YtDlp工具] 处理拖入的文件，因未知原因该文件不存在：{file}");
                            break;

                        // 文件头为 cookie 文件
                        case 1:
                            SetCookieFile(file);
                            break;

                        // 文件名为 cookie.txt
                        case 2:
                            Log.Info($"[YtDlp工具] 该文件的头行文本并非标准 # Netscape HTTP Cookie File");
                            var MsgBoxTmp = MessageBox.Show("该文件的头行并非标准的 # Netscape HTTP Cookie File，" +
                                                            "是否将其作为 Cookie 导入？", "YtDlp工具",
                                                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                            if (MsgBoxTmp == DialogResult.OK)
                                SetCookieFile(file);

                            else
                                Log.Info($"[YtDlp工具] 用户取消导入。");

                            break;

                        // 其他文件
                        case 3:
                            comboBox_URL.Text = file;
                            break;

                        default:
                            break;
                    }
                    Log.Info($"[YtDlp工具] 处理文件 {file}");
                }
                else
                {
                    MessageBox.Show("目前仅支持拖入文件，不支持拖入文件夹或其他内容");
                    Log.Warn("[YtDlp工具] 用户拖入了非文件或不存在内容");
                }
            }
        }

        private void SetCookieFile(string file)
        {
            try
            {
                File.WriteAllText(CookiesFilePath, File.ReadAllText(file));
                checkBox_UseCookies.Checked = true;

                Log.Info($"[YtDlp工具] 成功将Cookies文件 {file} 的内容导入 cookie.txt");
                MessageBox.Show($"成功将Cookies文件 {file} 的内容导入 cookie.txt", "YtDlp工具",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log.Err($"[YtDlp工具] 处理Cookies文件 {file} 时出现意料之外的异常，错误信息：{ex.Message}");
                MessageBox.Show($"文件 {file} 导入 cookie.txt 时出现异常\n\n错误信息：{ex.Message}",
                                 "YtDlp工具", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*
         * 检查是否为 Cookie 文件
         * -1：异常
         *  0：文件不存在
         *  1：文件头行正确
         *  2：文件名正确
         *  3：其他文件
         */
        public int IsCookieFile(string filePath)
        {
            // 检查文件是否存在
            if (!File.Exists(filePath))
                return 0;

            // 检查文件名是否为 cookie.txt
            string fileName = Path.GetFileName(filePath);
            bool isCorrectName = fileName.Equals("cookie.txt", StringComparison.OrdinalIgnoreCase);

            // 检查第一行内容
            bool hasCorrectHeader = false;

            try
            {
                // 只读取第一行，不读取整个文件
                string firstLine = File.ReadLines(filePath).FirstOrDefault();
                hasCorrectHeader = firstLine?.Trim() == "# Netscape HTTP Cookie File";
            }
            catch (Exception ex)
            {
                // 处理读取异常
                Log.Err($"[YtDlp工具] 处理文件 {filePath} 时出现意料之外的异常，错误信息：{ex.Message}");
                return -1;
            }

            // 文件头行正确
            if (hasCorrectHeader)
                return 1;

            // 文件名正确
            else if (isCorrectName)
                return 2;

            // 其他文件
            return 3;
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

        private async void button_AutoDownload_Click(object sender, EventArgs e)
        {
            // 检查配置文件是否存在
            if (!File.Exists(AutoDownloadConfigFilePath))
            {
                CreateDefaultAutoDownloadConfig(AutoDownloadConfigFilePath);
                MessageBox.Show("已创建默认配置文件，请编辑后再执行自动下载。\n" +
                                $"文件位置：{AutoDownloadConfigFilePath}",
                                "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 解析配置文件
            var downloadItems = ParseAutoDownloadConfig(AutoDownloadConfigFilePath);

            if (downloadItems.Count == 0)
            {
                MessageBox.Show("配置文件中没有有效的下载项，请检查格式。", "提示",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 禁用按钮防止重复点击
            button_AutoDownload.Enabled = false;
            button_DoDownload.Enabled = false;
            button_DoAnalysis.Enabled = false;

            // 保存原始设置，以便下载完成后恢复
            var originalSettings = new
            {
                UseArchive = checkBox_UseArchive.Checked,
                ArchivePlus = checkBox_ArchivePlus.Checked,
                ClearURLData = checkBox_ClearURLData.Checked,
                OpenF = checkBox_OpenF.Checked,
                MatchFilters = checkBox_MatchFilters.Checked,
                PathHome = checkBox_Path_Home.Checked,
                HomePath = comboBox_Path_Home.Text,
                SetPaths = checkBox_SetPaths.Checked,
                Paths = textBox_Paths.Text
            };

            // 设置自动下载需要的配置
            checkBox_UseArchive.Checked = true;
            checkBox_ArchivePlus.Checked = true;
            checkBox_ClearURLData.Checked = true;
            checkBox_OpenF.Checked = false;

            int successCount = 0;
            int failCount = 0;
            bool UsePlayListItems = false;

            try
            {
                foreach (var item in downloadItems)
                {
                    Log.Info($"[YtDlp工具] 开始处理: {item.Url}");
                    Log.Info($"  名称: {(item.HasCustomName ? item.Name : "(自动解析)")}");
                    Log.Info($"  MatchFilters: {item.MatchFilters}");
                    Log.Info($"  Playlist: {item.Playlist}");

                    UsePlayListItems =
                        !string.IsNullOrWhiteSpace(item.Playlist) &&
                        item.Playlist.ToLower() != "false" &&
                        item.Playlist.ToLower() != "no";

                    // 设置启用状态
                    checkBox_MatchFilters.Checked = item.MatchFilters;

                    // 确定是否传递自定义规则数据
                    string customMatchFiltersData = null;
                    if (item.MatchFilters && item.HasCustomMatchFilters)
                    {
                        customMatchFiltersData = item.MatchFiltersData;
                    }

                    // 执行下载，传递自定义规则（如果有）
                    try
                    {
                        await DoDownload(customMatchFiltersData);
                        successCount++;
                        Log.Info($"[YtDlpTool] 完成: {item.Url}");
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        Log.Err($"[YtDlpTool] 下载失败: {item.Url}, 错误: {ex.Message}");
                    }

                    checkBox_playlist_items.Checked = UsePlayListItems;

                    if (UsePlayListItems)
                        textBox_playlist_items.Text = item.Playlist;

                    // 设置 URL
                    comboBox_URL.Text = item.Url;

                    // 获取目标目录名称
                    string directoryName = item.GetDefaultName();

                    // 清理名称中的非法字符
                    directoryName = IdNameMapper.SanitizeFileName(directoryName);

                    // 构建下载路径
                    string downloadPath = Path.Combine(actualDirectory, @"yt-dlp\Downloads", directoryName);

                    Log.Info($"下载目录: {downloadPath}");

                    // 设置路径
                    if (checkBox_Path_Home.Checked)
                        comboBox_Path_Home.Text = downloadPath;
                    else
                        textBox_Paths.Text = downloadPath;

                    // 确保目录存在
                    Directory.CreateDirectory(downloadPath);

                    // 执行下载
                    try
                    {
                        await DoDownload();
                        successCount++;
                        Log.Info($"[YtDlp工具] 完成: {item.Url}");
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        Log.Err($"[YtDlp工具] 下载失败: {item.Url}, 错误: {ex.Message}");
                    }
                }

                // 显示完成信息
                string resultMsg = $"自动下载完成！\n成功: {successCount} 个\n失败: {failCount} 个";
                Log.Info($"[YtDlp工具] {resultMsg}");
                MessageBox.Show(resultMsg, "下载完成", MessageBoxButtons.OK,
                                failCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log.Err($"[YtDlp工具] 自动下载过程中出现错误：{ex.Message}");
                MessageBox.Show($"下载过程中出现错误：{ex.Message}", "错误",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 恢复原始设置
                checkBox_UseArchive.Checked = originalSettings.UseArchive;
                checkBox_ArchivePlus.Checked = originalSettings.ArchivePlus;
                checkBox_ClearURLData.Checked = originalSettings.ClearURLData;
                checkBox_OpenF.Checked = originalSettings.OpenF;
                checkBox_MatchFilters.Checked = originalSettings.MatchFilters;

                if (originalSettings.PathHome)
                {
                    checkBox_Path_Home.Checked = true;
                    comboBox_Path_Home.Text = originalSettings.HomePath;
                }
                else if (originalSettings.SetPaths)
                {
                    checkBox_SetPaths.Checked = true;
                    textBox_Paths.Text = originalSettings.Paths;
                }

                // 恢复按钮状态
                button_AutoDownload.Enabled = true;
                button_DoDownload.Enabled = true;
                button_DoAnalysis.Enabled = true;
            }
        }

        /// <summary>
        /// 自动下载配置项
        /// </summary>
        private class AutoDownloadItem
        {
            public string Url { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public bool MatchFilters { get; set; } = false;
            public string MatchFiltersData { get; set; } = string.Empty;
            public string Playlist { get; set; } = string.Empty;

            public bool HasCustomName => !string.IsNullOrEmpty(Name);
            public bool HasCustomMatchFilters => !string.IsNullOrEmpty(MatchFiltersData);
            public bool HasPlaylistSetting => !string.IsNullOrEmpty(Playlist);

            /// <summary>
            /// 获取默认名称
            /// </summary>
            public string GetDefaultName()
            {
                if (HasCustomName)
                    return Name;

                return ParseDefaultNameFromUrl(Url);
            }

            /// <summary>
            /// 从 URL 解析默认名称
            /// </summary>
            private string ParseDefaultNameFromUrl(string url)
            {
                // 合集: https://space.bilibili.com/697987209/lists/8033161 -> 697987209_8033161
                if (url.Contains("/lists/"))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(url, @"space\.bilibili\.com/(\d+)/lists/(\d+)");
                    if (match.Success)
                        return $"{match.Groups[1]}_{match.Groups[2]}";
                }

                // 视频: https://www.bilibili.com/video/BV19EJJ68EK8 -> BV19EJJ68EK8
                if (url.Contains("/video/"))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(url, @"/video/([a-zA-Z0-9]+)");
                    if (match.Success)
                        return match.Groups[1].Value;
                }

                // 频道: https://space.bilibili.com/647267811 -> 647267811
                if (url.Contains("space.bilibili.com/"))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(url, @"space\.bilibili\.com/(\d+)");
                    if (match.Success)
                        return match.Groups[1].Value;
                }

                // 通用 fallback：使用 URL 的最后一段
                var uri = new Uri(url);
                return SanitizeFileName(uri.Segments.LastOrDefault()?.Trim('/') ?? "unknown");
            }

            private string SanitizeFileName(string name)
            {
                if (string.IsNullOrEmpty(name))
                    return "unknown";

                var invalidChars = Path.GetInvalidFileNameChars();
                return string.Join("_", name.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
            }

            /// <summary>
            /// 检查 Playlist 是否等于 false（布尔值）
            /// </summary>
            public bool IsPlaylistFalse()
            {
                if (string.IsNullOrEmpty(Playlist))
                    return false;

                return Playlist.Equals("false", StringComparison.OrdinalIgnoreCase);
            }

            /// <summary>
            /// 获取播放列表参数（用于命令行）
            /// </summary>
            public string GetPlaylistArgument()
            {
                if (string.IsNullOrEmpty(Playlist))
                    return string.Empty;

                // 如果是 false，返回 --no-playlist
                if (IsPlaylistFalse())
                    return " --no-playlist";

                // 否则返回 --playlist-items 参数
                return $" --playlist-items {Playlist.Trim()}";
            }
        }

        /// <summary>
        /// 解析自动下载配置文件
        /// </summary>
        private List<AutoDownloadItem> ParseAutoDownloadConfig(string configPath)
        {
            var items = new List<AutoDownloadItem>();

            if (!File.Exists(configPath))
            {
                CreateDefaultAutoDownloadConfig(configPath);
                return items;
            }

            var lines = File.ReadAllLines(configPath);
            AutoDownloadItem currentItem = null;

            foreach (var rawLine in lines)
            {
                var line = rawLine.Trim();

                // 跳过空行
                if (string.IsNullOrEmpty(line))
                    continue;

                // 注释行（以 # 开头）
                if (line.StartsWith("#"))
                    continue;

                // 检查是否是 URL
                bool isUrl = line.StartsWith("http://") || line.StartsWith("https://") ||
                             (line.Contains("bilibili.com") && !line.Contains(':'));

                if (isUrl)
                {
                    // 如果有当前项，先添加到列表
                    if (currentItem != null && !string.IsNullOrEmpty(currentItem.Url))
                    {
                        items.Add(currentItem);
                    }

                    // 创建新项
                    currentItem = new AutoDownloadItem { Url = line };
                    continue;
                }

                // 解析属性（仅当有当前项时）
                if (currentItem != null && line.Contains(':'))
                {
                    var colonIndex = line.IndexOf(':');
                    var key = line.Substring(0, colonIndex).Trim().ToLowerInvariant();
                    var value = line.Substring(colonIndex + 1).Trim();

                    switch (key)
                    {
                        case "name":
                            currentItem.Name = value;
                            break;
                        case "matchfilters":
                            currentItem.MatchFilters = value.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                                                       value.Equals("1") ||
                                                       value.Equals("yes");
                            break;
                        case "playlist":
                            currentItem.Playlist = value;
                            break;
                        case "matchfiltersdata":
                            currentItem.MatchFiltersData = value;
                            break;
                    }
                }
            }

            // 添加最后一个项
            if (currentItem != null && !string.IsNullOrEmpty(currentItem.Url))
            {
                items.Add(currentItem);
            }

            // 输出解析结果日志
            Log.Info($"[YtDlp工具] 解析到 {items.Count} 个下载项");
            foreach (var item in items)
            {
                Log.Info($"  - {item.Url}");
                Log.Info($"    名称: {(string.IsNullOrEmpty(item.Name) ? "(自动解析)" : item.Name)}");
                Log.Info($"    MatchFilters: {item.MatchFilters}");
                Log.Info($"    Playlist: {(string.IsNullOrEmpty(item.Playlist) ? "(未设置)" : item.Playlist)}");
            }

            return items;
        }

        /// <summary>
        /// 创建默认的自动下载配置文件
        /// </summary>
        private void CreateDefaultAutoDownloadConfig(string configPath)
        {
            var defaultContent = @"# ============================================
# 自动下载配置文件
# ============================================
# 格式说明：
# 1. URL 单独一行
# 2. 可选参数：
#    Name: 自定义名称（不指定则自动从 URL 解析）
#    MatchFilters: true/false（是否启用匹配过滤器，默认 false）
#    MatchFiltersData: 指定规则数据（需要先启用匹配过滤器）
#       - 逻辑与: 使用 '&' 符连接
#       - 逻辑或: 使用 ';' 符分隔
#    Playlist: 下载指定播放列表（默认不设置）
#       - false: 不指定下载播放列表索引，下载完整列表。
#       - 数字: 指定索引，如 ""5"" 只下载第5个
#       - 范围: 如 ""1-10"" 下载第1到第10个
#       - 倒序: 如 ""-1:-1"" 只下载最后一个
#       - 复杂: 如 ""1,3,5-7,10"" 或 ""1:-1:2""（起始:结束:步进）
#       - 末尾: 如 ""-5:"" 最后5个，""::-2"" 所有偶数项倒序
#
# ============================================

# 师傅你变了
https://space.bilibili.com/3494379457087582/lists/4459703
Name: 《师傅你变了》
MatchFilters: false

# 甜橙盛夏
https://space.bilibili.com/697987209/lists/8033161
Name: 《甜橙盛夏》
MatchFilters: false
Playlist: false

# 姜糖恋语
https://space.bilibili.com/1411920158/lists/6668649
Name: 《姜糖恋语》

# 逆徒与逆师
https://space.bilibili.com/5538/lists/6715609
Name: 《逆徒与逆师》

# 师兄绝非反派
https://space.bilibili.com/3546830767917809/lists/4728895
Name: 《师兄绝非反派》

# 熙玥救赎
https://space.bilibili.com/3690975687870895/lists/7035241
Name: 《熙玥救赎》

# 我竟是伟大存在
https://space.bilibili.com/32160535/lists/5578590
Name: 《我竟是伟大存在》

# 不当人的选手
https://space.bilibili.com/589953538/lists/7712111
Name: 《不当人的选手》

# 御兽老大
https://space.bilibili.com/429403980/lists/2639451
Name: 《御兽老大》

# 我的灵根是系统
https://space.bilibili.com/3494369006979716/lists/6714915
Name: 《我的灵根是系统》

# 摆烂也能无敌第二季
https://space.bilibili.com/3546743408953776/lists/7706348
Name: 《摆烂也能无敌第二季》

# 无免之门（已注释，如需下载请取消注释）
# https://space.bilibili.com/3546697831549463/lists/5684545
# Name: 《无免之门》

# 遗忘世间
https://space.bilibili.com/71130413/lists/5525218
Name: 《遗忘世间》
";
            File.WriteAllText(configPath, defaultContent, Encoding.UTF8);
        }

        private void comboBox_URL_TextChanged(object sender, EventArgs e)
        {
            button_AutoDownload.Enabled = File.Exists(AutoDownloadConfigFilePath) ||
                                          File.Exists(comboBox_URL.Text);
        }

        private void checkBox_ArchivePlus_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox_Date_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_DateBefore.Enabled = !checkBox_Date.Checked;
            checkBox_DateAfter.Enabled = !checkBox_Date.Checked;

            if (checkBox_Date.Checked)
            {
                checkBox_DateBefore.Checked = false;
                checkBox_DateAfter.Checked = false;
            }
        }

        private void checkBox_MatchFilters_CheckedChanged(object sender, EventArgs e)
        {
            //button_SetMatchFilters.Enabled = checkBox_MatchFilters.Checked;
        }

        private void button_SetMatchFilters_Click(object sender, EventArgs e)
        {
            new YtDlpToolMatchFilters().Show();
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

        private class IdNameMapper
        {
            private Dictionary<string, string> _idNameMap;
            private readonly string _mapFilePath;

            public IdNameMapper(string mapFilePath)
            {
                _mapFilePath = mapFilePath;
                _idNameMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            /// <summary>
            /// 读取映射文件
            /// 文件格式：每行一个 [ID] [名称]，用空格或制表符分隔
            /// </summary>
            public async Task LoadMapAsync()
            {
                if (!File.Exists(_mapFilePath))
                {
                    throw new FileNotFoundException($"映射文件不存在: {_mapFilePath}");
                }

                _idNameMap.Clear();

                var lines = await File.ReadAllLinesAsync(_mapFilePath);

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue; // 跳过空行和注释行

                    var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length >= 2)
                    {
                        string id = parts[0].Trim();
                        string name = string.Join(" ", parts.Skip(1)).Trim(); // 处理名称中包含空格的情况

                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name))
                        {
                            _idNameMap[id] = name;
                        }
                    }
                }
            }

            /// <summary>
            /// 根据ID获取名称，如果不存在则返回ID本身
            /// </summary>
            public string GetNameOrDefault(string id)
            {
                if (string.IsNullOrEmpty(id))
                    return id;

                return _idNameMap.TryGetValue(id, out var name) ? name : id;
            }

            /// <summary>
            /// 清理文件名中的非法字符
            /// </summary>
            public static string SanitizeFileName(string fileName)
            {
                if (string.IsNullOrEmpty(fileName))
                    return fileName;

                var invalidChars = Path.GetInvalidFileNameChars();
                return string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
            }

            // 添加到IdNameMapper类中
            public int GetMapCount() => _idNameMap.Count;

            /// <summary>
            /// 重新加载映射文件
            /// </summary>
            public async Task ReloadMapAsync()
            {
                await LoadMapAsync();
            }

            /// <summary>
            /// 获取所有ID列表
            /// </summary>
            public IEnumerable<string> GetAllIds() => _idNameMap.Keys;

            /// <summary>
            /// 获取所有映射
            /// </summary>
            public IReadOnlyDictionary<string, string> GetAllMappings() => _idNameMap;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await LoadIdNameMapper(AutoDownloadNameFilePath);
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

        private void button_YtDlpRunDir_Click(object sender, EventArgs e)
        {
            Qi.ExplorerStart(YtDlpWorkDirPath);
        }

        private async void button_RunRuleEngine_ClickAsync(object sender, EventArgs e)
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



