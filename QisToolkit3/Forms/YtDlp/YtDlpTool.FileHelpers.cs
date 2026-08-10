using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QisToolkit3.Forms
{
    public partial class YtDlpTool
    {
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
            string[] searchPaths = [homePath];

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
    }
}
