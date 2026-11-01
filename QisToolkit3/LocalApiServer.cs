using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using QisToolkit3.Forms;
using QisToolkit3.Forms.YtDlp;
using static Qi.QisToolkit3_Datas;

namespace QisToolkit3
{
    public static class LocalApiServer
    {
        private const string AuthToken = "QisToolkit3_WP_2025";
        private static HttpListener _listener;
        private static bool _isRunning = false;
        private static readonly object _lock = new object();

        public static void Start()
        {
            lock (_lock)
            {
                if (_isRunning) return;
                _isRunning = true;
            }

            Task.Run(async () =>
            {
                _listener = new HttpListener();
                _listener.Prefixes.Add($"http://127.0.0.1:{LocalApiServerPort}/");
                try
                {
                    _listener.Start();
                    Log.Info($"[LocalAPI] HTTP 服务已启动，监听端口: {LocalApiServerPort}");

                    while (_isRunning)
                    {
                        var context = await _listener.GetContextAsync();
                        _ = ProcessRequestAsync(context);
                    }
                }
                catch (Exception ex)
                {
                    Log.Err($"[LocalAPI] 服务异常: {ex.Message}");
                }
                finally
                {
                    _listener?.Stop();
                    _listener?.Close();
                }
            });
        }

        public static void Stop()
        {
            lock (_lock)
            {
                if (!_isRunning) return;
                _isRunning = false;
                _listener?.Stop();
                _listener?.Close();
                Log.Info("[LocalAPI] HTTP 服务已停止");
            }
        }

        private static async Task ProcessRequestAsync(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            // 允许浏览器跨域访问
            response.AddHeader("Access-Control-Allow-Origin", "*");
            response.AddHeader("Access-Control-Allow-Headers", "Content-Type, X-Qis-Token");
            response.AddHeader("Access-Control-Allow-Methods", "POST, OPTIONS");

            // 处理浏览器的预检请求 (OPTIONS)
            if (request.HttpMethod == "OPTIONS")
            {
                response.StatusCode = 200;
                response.Close();
                return;
            }

            if (request.HttpMethod != "POST" || request.Url.AbsolutePath != "/download")
            {
                response.StatusCode = 404;
                response.Close();
                return;
            }

            if (request.Headers["X-Qis-Token"] != AuthToken)
            {
                response.StatusCode = 403;
                response.Close();
                return;
            }

            try
            {
                using var reader = new StreamReader(request.InputStream, Encoding.UTF8);
                string body = await reader.ReadToEndAsync();
                var data = JsonSerializer.Deserialize<DownloadRequest>(body);

                if (data != null && !string.IsNullOrEmpty(data.url))
                {
                    string cookiePath = null;
                    if (!string.IsNullOrEmpty(data.cookies))
                    {
                        cookiePath = Path.Combine(Path.GetTempPath(), $"qis_cookies_{Guid.NewGuid():N}.txt");
                        File.WriteAllText(cookiePath, data.cookies, new UTF8Encoding(false));
                        Log.Info($"[LocalAPI] 已写入临时 Cookies: {cookiePath}");
                    }

                    var invocation = new YtDlpInvocation
                    {
                        Url = data.url,
                        AutoDownload = true,
                        CookieFilePath = cookiePath
                    };

                    // 寻找已经打开的 YtDlpTool 窗口
                    YtDlpTool targetForm = Application.OpenForms.OfType<YtDlpTool>().FirstOrDefault();

                    if (targetForm != null)
                    {
                        // 找到了（由 start 协议唤起的空窗口），复用并塞入数据
                        targetForm.Invoke(new Action(() =>
                        {
                            targetForm.TriggerHttpDownload(invocation);
                        }));
                        Log.Info("[LocalAPI] 复用已打开的 YtDlpTool 窗口");
                    }
                    else
                    {
                        // 没找到（比如用户先开了主界面，没开工具窗口），新建一个
                        var mainForm = Application.OpenForms.Count > 0 ? Application.OpenForms[0] : null;
                        if (mainForm != null)
                        {
                            mainForm.Invoke(new Action(() =>
                            {
                                var form = new YtDlpTool();
                                form.TriggerHttpDownload(invocation);
                                form.Show();
                            }));
                            Log.Info("[LocalAPI] 未找到可用窗口，已新建 YtDlpTool");
                        }
                    }

                    response.StatusCode = 200;
                }
                else
                {
                    response.StatusCode = 400;
                }
            }
            catch (Exception ex)
            {
                Log.Err($"[LocalAPI] 处理请求失败: {ex.Message}");
                response.StatusCode = 500;
            }
            finally
            {
                response.Close();
            }
        }

        private class DownloadRequest
        {
            public string url { get; set; }
            public string cookies { get; set; }
        }
    }
}