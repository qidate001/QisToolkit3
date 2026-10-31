using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QisToolkit3.Forms.YtDlp
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
        /// 反向解析结果
        /// </summary>
        public class ReverseResult
        {
            public bool IsValid { get; set; }
            public string Platform { get; set; } = "BiliBili";
            public BilibiliUrlType UrlType { get; set; } = BilibiliUrlType.Unknown;
            public string Url { get; set; }
            public string Description { get; set; }   // 反推成功时的类型说明，或失败原因
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
        /// 从 Archive 文件路径反推 BiliBili URL
        /// 支持层级：
        ///   Archive/BiliBili/archive.txt         -> 单视频累积，无法反推
        ///   Archive/BiliBili/{UserId}.txt        -> 用户主页
        ///   Archive/BiliBili/{UserId}/{ListId}.txt -> 视频合集
        /// </summary>
        /// <param name="archiveRootPath">Archive 根目录（不含 BiliBili）</param>
        /// <param name="fullArchivePath">具体归档文件完整路径</param>
        public static ReverseResult ReverseFromArchivePath(string archiveRootPath, string fullArchivePath)
        {
            var result = new ReverseResult();

            if (string.IsNullOrEmpty(archiveRootPath) || string.IsNullOrEmpty(fullArchivePath))
            {
                result.Description = "路径为空";
                return result;
            }

            if (!File.Exists(fullArchivePath))
            {
                result.Description = "文件不存在";
                return result;
            }

            string rootFull = Path.GetFullPath(archiveRootPath)
                                  .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string fileFull = Path.GetFullPath(fullArchivePath);

            if (!fileFull.StartsWith(rootFull + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            {
                result.Description = "文件不在 Archive 目录下";
                return result;
            }

            string relative = fileFull.Substring(rootFull.Length + 1);
            string[] parts = relative.Split(
                new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar },
                StringSplitOptions.RemoveEmptyEntries);

            // 第一段必须是 BiliBili
            if (parts.Length < 2 || !parts[0].Equals("BiliBili", StringComparison.OrdinalIgnoreCase))
            {
                result.Description = "不是 BiliBili 归档";
                return result;
            }

            // 两段：BiliBili\{fileName}.txt
            if (parts.Length == 2)
            {
                string fileName = parts[1];

                // 单视频累积存档
                if (fileName.Equals("archive.txt", StringComparison.OrdinalIgnoreCase))
                {
                    result.Description = "archive.txt 是单视频累积存档（可能包含多个 BV），无法反推出唯一 URL";
                    return result;
                }

                string userId = Path.GetFileNameWithoutExtension(fileName);
                if (string.IsNullOrEmpty(userId) || !userId.All(char.IsDigit))
                {
                    result.Description = $"文件名 {fileName} 不符合 BiliBili 用户 ID 规则（应为纯数字）";
                    return result;
                }

                result.Url = $"https://space.bilibili.com/{userId}";
                result.UrlType = BilibiliUrlType.UserSpace;
                result.IsValid = true;
                result.Description = "用户主页";
                return result;
            }

            // 三段：BiliBili\{UserId}\{ListId}.txt
            if (parts.Length == 3)
            {
                string userId = parts[1];
                string fileName = parts[2];
                string listId = Path.GetFileNameWithoutExtension(fileName);

                if (string.IsNullOrEmpty(userId) || !userId.All(char.IsDigit))
                {
                    result.Description = "用户目录名不符合规则（应为纯数字）";
                    return result;
                }

                if (string.IsNullOrEmpty(listId) || !listId.All(char.IsDigit))
                {
                    result.Description = $"文件名 {fileName} 不符合 BiliBili 视频合集 ID 规则（应为纯数字）";
                    return result;
                }

                result.Url = $"https://space.bilibili.com/{userId}/lists/{listId}";
                result.UrlType = BilibiliUrlType.VideoList;
                result.IsValid = true;
                result.Description = "视频合集";
                return result;
            }

            result.Description = $"路径层级不支持反推（{relative}）";
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
