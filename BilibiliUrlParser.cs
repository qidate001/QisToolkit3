using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QisToolkit3
{
    internal class BilibiliUrlParser
    {
        /// <summary>
        /// B站URL类型枚举
        /// </summary>
        public enum BilibiliUrlType
        {
            Unknown,        // 未知类型
            SingleVideo,    // 单个视频
            UserSpace,      // 用户主页
            VideoList       // 视频合集/列表
        }

        /// <summary>
        /// 解析结果，包含完整的路径信息
        /// </summary>
        public class ParseResult
        {
            public bool IsValid { get; set; }
            public string Platform { get; set; } = "BiliBili";
            public BilibiliUrlType UrlType { get; set; } = BilibiliUrlType.Unknown;

            // 提取的ID信息
            public string UserId { get; set; }
            public string ListId { get; set; }
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
                    BilibiliUrlType.SingleVideo => "单个视频",
                    BilibiliUrlType.UserSpace => "用户主页",
                    BilibiliUrlType.VideoList => "视频合集",
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

        /// <summary>
        /// 解析URL并返回存档信息（包含目录和文件名的分离）
        /// </summary>
        /// <param name="url">要解析的URL</param>
        /// <param name="baseDirectory">基础目录，默认为 {actualDirectory}</param>
        /// <returns>解析结果</returns>
        public static ParseResult ParseBilibiliUrl(string url, string baseDirectory = "{actualDirectory}")
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
                // 规范化URL
                Uri uri = new Uri(url);

                // 检查是否为bilibili域名
                if (!uri.Host.Contains("bilibili.com"))
                    return result;

                string urlLower = url.ToLower();
                string urlOriginal = url;

                // 1. 匹配视频合集/列表: space.bilibili.com/[userid]/lists/[listid]
                string listPattern = @"space\.bilibili\.com/(\d+)/lists/(\d+)";
                Match listMatch = Regex.Match(urlLower, listPattern);

                if (listMatch.Success)
                {
                    result.UserId = listMatch.Groups[1].Value;
                    result.ListId = listMatch.Groups[2].Value;
                    result.UrlType = BilibiliUrlType.VideoList;
                    result.IsValid = true;

                    // 构建路径: {actualDirectory}\yt-dlp\Archive\BiliBili\{UserId}\
                    result.ArchiveDirectory = Path.Combine(
                        baseDirectory,
                        "yt-dlp",
                        "Archive",
                        "BiliBili",
                        result.UserId
                    );

                    // 文件名: {ListId}.txt
                    result.ArchiveFileName = $"{result.ListId}.txt";

                    // 完整路径
                    result.FullArchivePath = Path.Combine(result.ArchiveDirectory, result.ArchiveFileName);

                    return result;
                }

                // 2. 匹配用户主页: space.bilibili.com/[userid]
                string spacePattern = @"space\.bilibili\.com/(\d+)(?:/.*)?$";
                Match spaceMatch = Regex.Match(urlLower, spacePattern);

                if (spaceMatch.Success && !urlLower.Contains("/lists/"))
                {
                    result.UserId = spaceMatch.Groups[1].Value;
                    result.UrlType = BilibiliUrlType.UserSpace;
                    result.IsValid = true;

                    // 构建路径: {actualDirectory}\yt-dlp\Archive\BiliBili\
                    result.ArchiveDirectory = Path.Combine(
                        baseDirectory,
                        "yt-dlp",
                        "Archive",
                        "BiliBili"
                    );

                    // 文件名: {UserId}.txt
                    result.ArchiveFileName = $"{result.UserId}.txt";

                    // 完整路径
                    result.FullArchivePath = Path.Combine(result.ArchiveDirectory, result.ArchiveFileName);

                    return result;
                }

                // 3. 匹配单个视频: bilibili.com/video/BVxxxxx
                string videoPattern = @"bilibili\.com/video/(BV[\w]+)";
                Match videoMatch = Regex.Match(urlOriginal, videoPattern, RegexOptions.IgnoreCase);

                if (videoMatch.Success)
                {
                    result.VideoId = videoMatch.Groups[1].Value;
                    result.UrlType = BilibiliUrlType.SingleVideo;
                    result.IsValid = true;

                    // 构建路径: {actualDirectory}\yt-dlp\Archive\BiliBili\
                    result.ArchiveDirectory = Path.Combine(
                        baseDirectory,
                        "yt-dlp",
                        "Archive",
                        "BiliBili"
                    );

                    // 文件名: archive.txt
                    result.ArchiveFileName = "archive.txt";

                    // 完整路径
                    result.FullArchivePath = Path.Combine(result.ArchiveDirectory, result.ArchiveFileName);

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
        /// 简化的检查方法，只返回是否是需要存档的URL类型
        /// </summary>
        public static bool IsValidBilibiliUrl(string url)
        {
            var result = ParseBilibiliUrl(url, "");
            return result.IsValid;
        }

        /// <summary>
        /// 获取存档目录路径（不含文件名）
        /// </summary>
        public static string GetArchiveDirectory(string url, string baseDirectory = "{actualDirectory}")
        {
            var result = ParseBilibiliUrl(url, baseDirectory);
            return result.IsValid ? result.ArchiveDirectory : null;
        }

        /// <summary>
        /// 获取存档文件名
        /// </summary>
        public static string GetArchiveFileName(string url)
        {
            var result = ParseBilibiliUrl(url, "");
            return result.IsValid ? result.ArchiveFileName : null;
        }

        /// <summary>
        /// 获取完整存档路径
        /// </summary>
        public static string GetFullArchivePath(string url, string baseDirectory = "{actualDirectory}")
        {
            var result = ParseBilibiliUrl(url, baseDirectory);
            return result.IsValid ? result.FullArchivePath : null;
        }

        /// <summary>
        /// 获取URL类型
        /// </summary>
        public static BilibiliUrlType GetUrlType(string url)
        {
            var result = ParseBilibiliUrl(url, "");
            return result.UrlType;
        }
    }
}
