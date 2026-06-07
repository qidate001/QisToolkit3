using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QisToolkit3.Forms.YtDlp
{
    public class VideoInfo
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("uploader")]
        public string Uploader { get; set; } = "";

        [JsonPropertyName("uploader_id")]
        public string UploaderId { get; set; } = "";

        [JsonPropertyName("view_count")]
        public int ViewCount { get; set; }

        [JsonPropertyName("like_count")]
        public int LikeCount { get; set; }

        [JsonPropertyName("comment_count")]
        public int CommentCount { get; set; }

        [JsonPropertyName("duration")]
        public double Duration { get; set; }

        [JsonPropertyName("upload_date")]
        public string UploadDate { get; set; } = "";

        [JsonPropertyName("description")]
        public string Description { get; set; } = "";

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; } = "";

        [JsonPropertyName("tags")]
        public string[] Tags { get; set; } = Array.Empty<string>();

        [JsonPropertyName("comments")]
        public List<Comment> Comments { get; set; } = new();

        [JsonPropertyName("webpage_url")]
        public string WebpageUrl { get; set; } = "";

        /// <summary>
        /// B站视频BV号（如：BV1bJEn6REZE）
        /// </summary>
        [JsonPropertyName("bvid")]
        public string Bvid { get; set; } = "";

        /// <summary>
        /// B站视频AV号（如：123456）
        /// </summary>
        [JsonPropertyName("aid")]
        public long Aid { get; set; }

        /// <summary>
        /// B站视频CID号
        /// </summary>
        [JsonPropertyName("cid")]
        public long Cid { get; set; }

        /// <summary>
        /// 便捷属性：获取可用于嵌入的B站视频ID（优先使用BV号）
        /// </summary>
        [JsonIgnore]
        public string EmbedVideoId
        {
            get
            {
                if (!string.IsNullOrEmpty(Bvid))
                    return Bvid;
                if (Aid > 0)
                    return $"av{Aid}";
                return "";
            }
        }

        /// <summary>
        /// 便捷属性：获取B站嵌入播放器URL
        /// </summary>
        [JsonIgnore]
        public string EmbedUrl
        {
            get
            {
                string videoId = EmbedVideoId;
                if (string.IsNullOrEmpty(videoId))
                {
                    // 尝试从webpage_url中提取
                    videoId = ExtractVideoIdFromUrl(WebpageUrl);
                }

                if (string.IsNullOrEmpty(videoId))
                    return "";

                // 高画质、禁止自动播放
                return $"https://player.bilibili.com/player.html?{videoId}&page=1&high_quality=1&autoplay=0";
            }
        }

        /// <summary>
        /// 从URL中提取B站视频ID
        /// </summary>
        private string ExtractVideoIdFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return "";

            // 匹配 BV 号模式（BV开头后跟10个字母数字）
            var bvMatch = System.Text.RegularExpressions.Regex.Match(url, @"BV[a-zA-Z0-9]{10}");
            if (bvMatch.Success)
                return $"bvid={bvMatch.Value}";

            // 匹配 av 号模式
            var avMatch = System.Text.RegularExpressions.Regex.Match(url, @"av(\d+)");
            if (avMatch.Success)
                return $"aid={avMatch.Groups[1].Value}";

            // 匹配哔哩哔哩短链接格式
            var shortMatch = System.Text.RegularExpressions.Regex.Match(url, @"/video/(BV[a-zA-Z0-9]{10})");
            if (shortMatch.Success)
                return $"bvid={shortMatch.Groups[1].Value}";

            return "";
        }
    }

    public class Comment
    {
        [JsonPropertyName("author")]
        public string Author { get; set; } = "";

        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; } = "";

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; } = "";

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("parent")]
        public JsonElement Parent { get; set; }

        // 辅助方法：获取 parent 的字符串表示
        public string ParentString => Parent.ValueKind == JsonValueKind.String
            ? Parent.GetString()
            : Parent.ValueKind == JsonValueKind.Number
                ? Parent.GetRawText()
                : "";
    }
}