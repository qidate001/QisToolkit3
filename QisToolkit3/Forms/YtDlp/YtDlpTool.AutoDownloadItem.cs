using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QisToolkit3.Forms
{
    /// <summary>
    /// 自动下载配置项
    /// </summary>
    public class AutoDownloadItem
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
}
