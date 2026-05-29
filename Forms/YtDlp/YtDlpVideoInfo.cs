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
