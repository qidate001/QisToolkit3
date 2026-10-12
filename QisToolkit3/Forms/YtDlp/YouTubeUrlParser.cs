using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QisToolkit3.Forms.YtDlp
{
    internal class YouTubeUrlParser
    {
        /// <summary>
        /// YouTube URL类型枚举
        /// </summary>
        public enum YouTubeUrlType
        {
            Unknown,        // 未知类型
            SingleVideo,    // 单个视频
            Channel,        // 频道（含 @handle、/channel、/c、/user）
            Playlist        // 播放列表
        }

        /// <summary>
        /// 解析结果，包含完整的路径信息
        /// </summary>
        public class ParseResult
        {
            public bool IsValid { get; set; }
            public string Platform { get; set; } = "YouTube";
            public YouTubeUrlType UrlType { get; set; } = YouTubeUrlType.Unknown;

            // 提取的ID信息
            public string ChannelId { get; set; }   // UCxxx 或 @handle 或自定义名
            public string PlaylistId { get; set; }
            public string VideoId { get; set; }

            // 原始URL
            public string OriginalUrl { get; set; }

            // 路径信息
            public string BaseDirectory { get; set; }
            public string ArchiveDirectory { get; set; }      // 存档目录路径（不含文件名）
            public string ArchiveFileName { get; set; }        // 存档文件名
            public string FullArchivePath { get; set; }        // 完整路径（目录+文件名）

            /// <summary>
            /// 获取类型描述
            /// </summary>
            public string GetTypeDescription()
            {
                return UrlType switch
                {
                    YouTubeUrlType.SingleVideo => "单个视频",
                    YouTubeUrlType.Channel => "频道",
                    YouTubeUrlType.Playlist => "播放列表",
                    _ => "未知类型"
                };
            }

            /// <summary>
            /// 创建存档目录（如果不存在）
            /// </summary>
            /// <returns>创建的目录路径</returns>
            public string EnsureArchiveDirectoryExists()
            {
                if (!string.IsNullOrEmpty(ArchiveDirectory))
                {
                    Directory.CreateDirectory(ArchiveDirectory);
                }
                return ArchiveDirectory;
            }

            /// <summary>
            /// 获取目录信息，便于调试
            /// </summary>
            public string GetPathInfo()
            {
                return $"目录: {ArchiveDirectory}\n文件名: {ArchiveFileName}\n完整路径: {FullArchivePath}";
            }
        }

        // 视频ID：11位 [A-Za-z0-9_-]
        private const string VideoIdPattern = @"[A-Za-z0-9_\-]{11}";
        // 频道ID：UC + 22位
        private const string ChannelIdPattern = @"UC[A-Za-z0-9_\-]{22}";
        // 播放列表ID字符集
        private const string PlaylistIdPattern = @"[A-Za-z0-9_\-]+";

        /// <summary>
        /// 解析URL并返回存档信息（包含目录和文件名的分离）
        /// </summary>
        /// <param name="url">要解析的URL</param>
        /// <param name="baseDirectory">基础目录，默认为 {actualDirectory}</param>
        /// <returns>解析结果</returns>
        public static ParseResult ParseYouTubeUrl(string url, string baseDirectory = "{actualDirectory}")
        {
            var result = new ParseResult
            {
                OriginalUrl = url,
                BaseDirectory = baseDirectory,
                IsValid = false
            };

            if (string.IsNullOrEmpty(url))
                return result;

            try
            {
                // 补全协议头，便于统一处理
                string normalizedUrl = url;
                if (!normalizedUrl.Contains("://"))
                    normalizedUrl = "https://" + normalizedUrl;

                Uri uri = new Uri(normalizedUrl);
                string host = uri.Host.ToLowerInvariant();

                // 检查是否为YouTube域名
                bool isYouTube = host.Contains("youtube.com")
                              || host.Equals("youtu.be")
                              || host.Contains("youtube-nocookie.com");
                if (!isYouTube)
                    return result;

                string path = uri.AbsolutePath.TrimEnd('/');
                if (string.IsNullOrEmpty(path)) path = "/";

                // 解析 query 参数
                string listId = null;
                string videoIdFromQuery = null;

                string query = uri.Query.TrimStart('?');
                if (!string.IsNullOrEmpty(query))
                {
                    foreach (var pair in query.Split('&'))
                    {
                        int eqIndex = pair.IndexOf('=');
                        if (eqIndex < 0) continue;
                        string key = pair.Substring(0, eqIndex);
                        string val = Uri.UnescapeDataString(pair.Substring(eqIndex + 1));

                        if (key.Equals("list", StringComparison.OrdinalIgnoreCase))
                            listId = val;
                        else if (key.Equals("v", StringComparison.OrdinalIgnoreCase))
                            videoIdFromQuery = val;
                    }
                }

                // 1. 播放列表: youtube.com/playlist?list=XXX
                if (path.Equals("/playlist", StringComparison.OrdinalIgnoreCase)
                    && !string.IsNullOrEmpty(listId)
                    && Regex.IsMatch(listId, "^" + PlaylistIdPattern + "$"))
                {
                    result.PlaylistId = listId;
                    result.UrlType = YouTubeUrlType.Playlist;
                    result.IsValid = true;

                    SetArchivePath(result, baseDirectory, $"{SanitizeFileName(listId)}.txt");
                    return result;
                }

                // 2. 频道: /channel/UCxxxx
                Match channelMatch = Regex.Match(path, @"^/channel/(" + ChannelIdPattern + ")", RegexOptions.IgnoreCase);
                if (channelMatch.Success)
                {
                    result.ChannelId = channelMatch.Groups[1].Value;
                    result.UrlType = YouTubeUrlType.Channel;
                    result.IsValid = true;

                    SetArchivePath(result, baseDirectory, $"{SanitizeFileName(result.ChannelId)}.txt");
                    return result;
                }

                // 3. 频道: /@handle
                Match handleMatch = Regex.Match(path, @"^/@([A-Za-z0-9_\-\.]+)");
                if (handleMatch.Success)
                {
                    result.ChannelId = handleMatch.Groups[1].Value;
                    result.UrlType = YouTubeUrlType.Channel;
                    result.IsValid = true;

                    SetArchivePath(result, baseDirectory, $"{SanitizeFileName(result.ChannelId)}.txt");
                    return result;
                }

                // 4. 频道: /c/Name 或 /user/Name（旧版自定义URL）
                Match customMatch = Regex.Match(path, @"^/(c|user)/([A-Za-z0-9_\-\.]+)", RegexOptions.IgnoreCase);
                if (customMatch.Success)
                {
                    result.ChannelId = customMatch.Groups[2].Value;
                    result.UrlType = YouTubeUrlType.Channel;
                    result.IsValid = true;

                    SetArchivePath(result, baseDirectory, $"{SanitizeFileName(result.ChannelId)}.txt");
                    return result;
                }

                // 5. 单个视频: youtu.be/VIDEOID
                if (host.Equals("youtu.be"))
                {
                    Match shortMatch = Regex.Match(path, @"^/(" + VideoIdPattern + ")");
                    if (shortMatch.Success)
                    {
                        result.VideoId = shortMatch.Groups[1].Value;
                        result.UrlType = YouTubeUrlType.SingleVideo;
                        result.IsValid = true;

                        SetArchivePath(result, baseDirectory, "archive.txt");
                        return result;
                    }
                }

                // 6. 单个视频: /shorts/ID, /live/ID, /embed/ID, /v/ID
                Match videoPathMatch = Regex.Match(path, @"^/(shorts|live|embed|v)/(" + VideoIdPattern + ")", RegexOptions.IgnoreCase);
                if (videoPathMatch.Success)
                {
                    result.VideoId = videoPathMatch.Groups[2].Value;
                    result.UrlType = YouTubeUrlType.SingleVideo;
                    result.IsValid = true;

                    SetArchivePath(result, baseDirectory, "archive.txt");
                    return result;
                }

                // 7. 单个视频: /watch?v=VIDEOID（含 &list= 时按单个视频处理）
                if (!string.IsNullOrEmpty(videoIdFromQuery)
                    && Regex.IsMatch(videoIdFromQuery, "^" + VideoIdPattern + "$"))
                {
                    result.VideoId = videoIdFromQuery;
                    result.UrlType = YouTubeUrlType.SingleVideo;
                    result.IsValid = true;

                    SetArchivePath(result, baseDirectory, "archive.txt");
                    return result;
                }
            }
            catch (UriFormatException)
            {
                // URL格式错误
                return result;
            }

            return result;
        }

        /// <summary>
        /// 设置存档目录与文件名
        /// </summary>
        private static void SetArchivePath(ParseResult result, string baseDirectory, string fileName)
        {
            // 构建路径: {actualDirectory}\yt-dlp\Archive\YouTube\
            result.ArchiveDirectory = Path.Combine(
                baseDirectory,
                "yt-dlp",
                "Archive",
                "YouTube"
            );

            result.ArchiveFileName = fileName;
            result.FullArchivePath = Path.Combine(result.ArchiveDirectory, result.ArchiveFileName);
        }

        /// <summary>
        /// 清理文件名中的非法字符
        /// </summary>
        private static string SanitizeFileName(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            char[] invalid = Path.GetInvalidFileNameChars();
            return new string(name.Where(c => !invalid.Contains(c)).ToArray());
        }

        /// <summary>
        /// 简化的检查方法，只返回是否是需要存档的URL类型
        /// </summary>
        public static bool IsValidYouTubeUrl(string url)
        {
            var result = ParseYouTubeUrl(url, "");
            return result.IsValid;
        }

        /// <summary>
        /// 获取存档目录路径（不含文件名）
        /// </summary>
        public static string GetArchiveDirectory(string url, string baseDirectory = "{actualDirectory}")
        {
            var result = ParseYouTubeUrl(url, baseDirectory);
            return result.IsValid ? result.ArchiveDirectory : null;
        }

        /// <summary>
        /// 获取存档文件名
        /// </summary>
        public static string GetArchiveFileName(string url)
        {
            var result = ParseYouTubeUrl(url, "");
            return result.IsValid ? result.ArchiveFileName : null;
        }

        /// <summary>
        /// 获取完整存档路径
        /// </summary>
        public static string GetFullArchivePath(string url, string baseDirectory = "{actualDirectory}")
        {
            var result = ParseYouTubeUrl(url, baseDirectory);
            return result.IsValid ? result.FullArchivePath : null;
        }

        /// <summary>
        /// 获取URL类型
        /// </summary>
        public static YouTubeUrlType GetUrlType(string url)
        {
            var result = ParseYouTubeUrl(url, "");
            return result.UrlType;
        }
    }
}