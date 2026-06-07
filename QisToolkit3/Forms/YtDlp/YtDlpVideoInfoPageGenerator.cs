using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace QisToolkit3.Forms.YtDlp
{
    public class VideoInfoPageGenerator
    {
        private readonly string _downloadPath;
        private readonly Action<string>? _logCallback;
        private bool _embedBilibiliPlayer = false;  // 添加字段

        public VideoInfoPageGenerator(string downloadPath, Action<string>? logCallback = null)
        {
            _downloadPath = downloadPath;
            _logCallback = logCallback;
        }

        private void mLog(string message)
        {
            _logCallback?.Invoke(message);
        }

        public async Task GenerateForVideo(string videoPath, bool embedBilibiliPlayer = false)
        {
            _embedBilibiliPlayer = embedBilibiliPlayer;  // 保存设置

            try
            {
                mLog($"📄 开始生成信息页...");
                mLog($"   视频文件: {videoPath}");
                mLog($"   嵌入视频播放器: {(embedBilibiliPlayer ? "是" : "否")}");

                string baseName = Path.GetFileNameWithoutExtension(videoPath);
                string jsonPath = Path.Combine(_downloadPath, $"{baseName}.信息.json");

                // 如果找不到 .信息.json，尝试 .info.json
                if (!File.Exists(jsonPath))
                {
                    string altJsonPath = Path.Combine(_downloadPath, $"{baseName}.info.json");
                    if (File.Exists(altJsonPath))
                    {
                        jsonPath = altJsonPath;
                        mLog($"   找到信息文件: {Path.GetFileName(jsonPath)}");
                    }
                    else
                    {
                        mLog($"   ⚠️ 未找到信息文件: {baseName}.信息.json 或 {baseName}.info.json");
                        return;
                    }
                }
                else
                {
                    mLog($"   找到信息文件: {Path.GetFileName(jsonPath)}");
                }

                // 读取 JSON
                mLog($"   读取 JSON 文件...");
                string jsonContent = await File.ReadAllTextAsync(jsonPath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
                };

                var videoInfo = JsonSerializer.Deserialize<VideoInfo>(jsonContent, options);

                if (videoInfo == null)
                {
                    mLog($"   ❌ 反序列化失败，视频信息为空");
                    return;
                }

                mLog($"   ✅ 解析成功: {videoInfo.Title}");
                mLog($"      评论数: {videoInfo.Comments.Count}");
                mLog($"      标签数: {videoInfo.Tags.Length}");

                // 生成 HTML
                mLog($"   生成 HTML 内容...");
                string html = GenerateHtml(videoInfo);
                string htmlPath = Path.Combine(_downloadPath, $"{baseName}.html");

                await File.WriteAllTextAsync(htmlPath, html, Encoding.UTF8);

                mLog($"   ✅ 信息页生成成功: {Path.GetFileName(htmlPath)}");
                mLog($"      文件大小: {new FileInfo(htmlPath).Length:N0} 字节");
            }
            catch (Exception ex)
            {
                mLog($"   ❌ 生成信息页失败: {ex.Message}");
            }
        }

        private string GenerateHtml(VideoInfo info)
        {
            // 根据设置决定是否获取嵌入URL
            string embedUrl = null;
            if (_embedBilibiliPlayer)
            {
                embedUrl = info.EmbedUrl;  // 使用 VideoInfo 中的便捷属性
            }

            return $@"<!DOCTYPE html>
<html lang='zh-CN'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{EscapeHtml(info.Title)} - 视频信息</title>
    <style>
        * {{ margin: 0; padding: 0; box-sizing: border-box; }}
        
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
            background: #0a0e1a;
            min-height: 100vh;
            padding: 40px 20px;
        }}
        
        .container {{
            max-width: 1000px;
            margin: 0 auto;
            background: linear-gradient(135deg, #1a1f2e 0%, #0f141f 100%);
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0,0,0,0.5);
            overflow: hidden;
            border: 1px solid rgba(102, 126, 234, 0.2);
        }}
        
        .header {{
            background: linear-gradient(135deg, #1a1f2e 0%, #0f141f 100%);
            border-bottom: 1px solid rgba(102, 126, 234, 0.3);
            color: white;
            padding: 40px;
            text-align: center;
        }}
        
        .header h1 {{
            font-size: 1.8rem;
            margin-bottom: 10px;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            -webkit-background-clip: text;
            background-clip: text;
            color: transparent;
        }}
        
        .header .video-id {{
            opacity: 0.7;
            font-size: 0.9rem;
            color: #8892b0;
        }}
        
        .content {{
            padding: 40px;
        }}
        
        /* 视频播放器容器 */
        .video-player {{
            margin-bottom: 30px;
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 8px 32px rgba(0,0,0,0.3);
            background: #000;
            position: relative;
            padding-bottom: 56.25%; /* 16:9 宽高比 */
            height: 0;
        }}
        
        .video-player iframe {{
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            border: none;
        }}
        
        .thumbnail {{
            width: 100%;
            max-height: 400px;
            object-fit: cover;
            border-radius: 10px;
            margin-bottom: 20px;
            border: 1px solid rgba(102, 126, 234, 0.3);
        }}
        
        /* 如果没有嵌入视频，显示提示 */
        .no-embed {{
            background: rgba(102, 126, 234, 0.1);
            border: 1px dashed rgba(102, 126, 234, 0.4);
            border-radius: 12px;
            padding: 40px;
            text-align: center;
            color: #8892b0;
            margin-bottom: 30px;
        }}
        
        .section {{
            margin-bottom: 30px;
        }}
        
        .section-title {{
            font-size: 1.3rem;
            font-weight: 600;
            color: #e6e6e6;
            border-left: 4px solid #667eea;
            padding-left: 15px;
            margin-bottom: 15px;
        }}
        
        .info-grid {{
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 15px;
        }}
        
        .info-card {{
            background: rgba(26, 31, 46, 0.8);
            backdrop-filter: blur(10px);
            padding: 15px;
            border-radius: 10px;
            border: 1px solid rgba(102, 126, 234, 0.2);
            transition: all 0.3s ease;
        }}
        
        .info-card:hover {{
            border-color: rgba(102, 126, 234, 0.5);
            transform: translateY(-2px);
        }}
        
        .info-label {{
            font-size: 0.8rem;
            color: #8892b0;
            margin-bottom: 5px;
        }}
        
        .info-value {{
            font-size: 1.1rem;
            font-weight: 500;
            color: #e6e6e6;
        }}
        
        .info-value a {{
            color: #667eea;
            text-decoration: none;
        }}
        
        .info-value a:hover {{
            text-decoration: underline;
        }}
        
        .tags {{
            display: flex;
            flex-wrap: wrap;
            gap: 8px;
        }}
        
        .tag {{
            background: rgba(102, 126, 234, 0.15);
            padding: 5px 12px;
            border-radius: 20px;
            font-size: 0.8rem;
            color: #a0aec0;
            border: 1px solid rgba(102, 126, 234, 0.3);
            transition: all 0.3s ease;
        }}
        
        .tag:hover {{
            background: rgba(102, 126, 234, 0.3);
            color: #e6e6e6;
        }}
        
        .description {{
            background: rgba(26, 31, 46, 0.8);
            padding: 20px;
            border-radius: 10px;
            line-height: 1.6;
            color: #a0aec0;
            border: 1px solid rgba(102, 126, 234, 0.2);
            white-space: pre-wrap;
        }}
        
        .comments-stats {{
            background: rgba(102, 126, 234, 0.15);
            padding: 8px 15px;
            border-radius: 20px;
            display: inline-block;
            margin-bottom: 15px;
            font-size: 0.85rem;
            color: #8892b0;
            border: 1px solid rgba(102, 126, 234, 0.3);
        }}
        
        .comments-list {{
            background: rgba(26, 31, 46, 0.6);
            border-radius: 10px;
            overflow: hidden;
            border: 1px solid rgba(102, 126, 234, 0.2);
        }}
        
        .comment {{
            padding: 15px 20px;
            border-bottom: 1px solid rgba(102, 126, 234, 0.1);
            transition: background 0.2s;
        }}
        
        .comment:hover {{
            background: rgba(102, 126, 234, 0.08);
        }}
        
        .comment:last-child {{
            border-bottom: none;
        }}
        
        .comment-author {{
            font-weight: 600;
            color: #667eea;
            margin-bottom: 5px;
            display: flex;
            align-items: center;
            gap: 10px;
            flex-wrap: wrap;
        }}
        
        .comment-author-id {{
            font-size: 0.7rem;
            color: #5a6e8a;
            font-weight: normal;
        }}
        
        .comment-text {{
            color: #cbd5e0;
            margin-bottom: 5px;
            line-height: 1.5;
        }}
        
        .comment-time {{
            font-size: 0.7rem;
            color: #5a6e8a;
        }}
        
        .footer {{
            background: rgba(15, 20, 31, 0.8);
            padding: 20px;
            text-align: center;
            color: #5a6e8a;
            font-size: 0.8rem;
            border-top: 1px solid rgba(102, 126, 234, 0.2);
        }}
        
        @media (max-width: 600px) {{
            .header {{ padding: 20px; }}
            .content {{ padding: 20px; }}
            .header h1 {{ font-size: 1.2rem; }}
        }}
        
        /* 滚动条样式 */
        ::-webkit-scrollbar {{
            width: 8px;
            height: 8px;
        }}
        
        ::-webkit-scrollbar-track {{
            background: #1a1f2e;
        }}
        
        ::-webkit-scrollbar-thumb {{
            background: #667eea;
            border-radius: 4px;
        }}
        
        ::-webkit-scrollbar-thumb:hover {{
            background: #764ba2;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>{EscapeHtml(info.Title)}</h1>
            <div class='video-id'>{info.Id}</div>
        </div>
        <div class='content'>
            {(!string.IsNullOrEmpty(embedUrl) ?
                $@"
            <div class='video-player'>
                <iframe 
                    src='{embedUrl}' 
                    frameborder='0' 
                    allowfullscreen='true'
                    allow='accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share'>
                </iframe>
            </div>" :
                (string.IsNullOrEmpty(info.Thumbnail) ? "" :
                $"<img class='thumbnail' src='{info.Thumbnail}' alt='缩略图' loading='lazy'>"))}
            
            <div class='section'>
                <div class='section-title'>基本信息</div>
                <div class='info-grid'>
                    <div class='info-card'><div class='info-label'>UP主</div><div class='info-value'>{EscapeHtml(info.Uploader)}</div></div>
                    <div class='info-card'><div class='info-label'>播放量</div><div class='info-value'>{FormatNumber(info.ViewCount)}</div></div>
                    <div class='info-card'><div class='info-label'>点赞数</div><div class='info-value'>{FormatNumber(info.LikeCount)}</div></div>
                    <div class='info-card'><div class='info-label'>评论数</div><div class='info-value'>{info.CommentCount}</div></div>
                    <div class='info-card'><div class='info-label'>时长</div><div class='info-value'>{FormatDuration(info.Duration)}</div></div>
                    <div class='info-card'><div class='info-label'>上传日期</div><div class='info-value'>{FormatDate(info.UploadDate)}</div></div>
                    {(string.IsNullOrEmpty(info.WebpageUrl) ? "" : $@"
                    <div class='info-card'>
                        <div class='info-label'>原视频链接</div>
                        <div class='info-value'><a href='{info.WebpageUrl}' target='_blank'>点击访问 ↗</a></div>
                    </div>")}
                </div>
            </div>
            
            {(info.Tags.Length > 0 ? $@"
            <div class='section'>
                <div class='section-title'>标签</div>
                <div class='tags'>
                    {string.Join("", info.Tags.Select(t => $"<span class='tag'>{EscapeHtml(t)}</span>"))}
                </div>
            </div>" : "")}
            
            {(!string.IsNullOrEmpty(info.Description) ? $@"
            <div class='section'>
                <div class='section-title'>简介</div>
                <div class='description'>{EscapeHtml(info.Description).Replace("\n", "<br>")}</div>
            </div>" : "")}
            
            {(info.Comments.Count > 0 ? $@"
            <div class='section'>
                <div class='section-title'>评论 ({info.Comments.Count})</div>
                <div class='comments-stats'>共 {info.Comments.Count} 条评论，已全部展示</div>
                <div class='comments-list'>
                    {string.Join("", info.Comments.Select(c => $@"
                    <div class='comment'>
                        <div class='comment-author'>
                            {EscapeHtml(c.Author)}
                            <span class='comment-author-id'>ID: {c.AuthorId}</span>
                        </div>
                        <div class='comment-text'>{EscapeHtml(c.Text)}</div>
                        <div class='comment-time'>{UnixTimeToDateTime(c.Timestamp):yyyy-MM-dd HH:mm:ss}</div>
                    </div>"))}
                </div>
            </div>" : "<div class='section'><div class='section-title'>评论</div><div class='description' style='text-align:center;color:#5a6e8a;'>暂无评论</div></div>")}
        </div>
        <div class='footer'>
            生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}<br>
            由 QisToolkit3 自动生成 | 数据来源: yt-dlp
            {(_embedBilibiliPlayer ? " | 已启用视频嵌入" : " | 缩略图模式")}
        </div>
    </div>
</body>
</html>";
        }

        private string EscapeHtml(string input) =>
            string.IsNullOrEmpty(input) ? "" :
            input.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");

        private string FormatNumber(int num) =>
            num >= 10000 ? $"{num / 10000.0:F1}万" : num.ToString();

        private string FormatDuration(double seconds)
        {
            var ts = TimeSpan.FromSeconds(seconds);
            if (ts.TotalHours >= 1)
                return ts.ToString(@"h\:mm\:ss");
            return ts.ToString(@"m\:ss");
        }

        private string FormatDate(string yyyymmdd) =>
            yyyymmdd.Length == 8 ? $"{yyyymmdd[..4]}-{yyyymmdd.Substring(4, 2)}-{yyyymmdd[6..]}" : yyyymmdd;

        private DateTime UnixTimeToDateTime(long unixTime) =>
            DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime;
    }
}