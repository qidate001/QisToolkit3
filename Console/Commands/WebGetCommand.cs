using System;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace QisToolkit3.Console.Commands
{
    public class WebGetCommand : ICommand
    {
        public string Name => "webget";
        public string Description => "下载文件 (支持多线程断点续传)";
        public string[] Aliases => new[] { "wg", "wget" };

        // 配置参数
        private const int BUFFER_SIZE = 1024 * 1024; // 1MB
        private const int MAX_RETRIES = 3;
        private const int RETRY_DELAY_MS = 1000;
        private const int DEFAULT_THREADS = 4; // 默认线程数

        public CommandResult Execute(string[] args)
        {
            if (args.Length == 0)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = GetUsageHelp()
                };
            }

            string url = args[0];
            string savePath = args.Length > 1 ? args[1] : null;
            int threadCount = DEFAULT_THREADS;

            // 检查是否指定了线程数 (可选参数: -t 4)
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-t" && i + 1 < args.Length)
                {
                    if (int.TryParse(args[i + 1], out int threads) && threads > 0 && threads <= 16)
                    {
                        threadCount = threads;
                    }
                    else
                    {
                        System.Console.WriteLine($"无效线程数，使用默认值: {DEFAULT_THREADS}");
                    }
                }
            }

            // 验证URL
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return new CommandResult
                {
                    Success = false,
                    Response = $"无效的URL: {url}"
                };
            }

            // 如果没有指定保存路径，使用当前目录 + URL文件名
            if (string.IsNullOrEmpty(savePath))
            {
                string fileName = GetFileNameFromUrl(url);
                savePath = Path.Combine(Environment.CurrentDirectory, fileName);
            }
            else
            {
                // 如果是目录，则使用目录 + 文件名
                if (Directory.Exists(savePath) || savePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    string fileName = GetFileNameFromUrl(url);
                    savePath = Path.Combine(savePath, fileName);
                }
            }

            // 确保目录存在
            string directory = Path.GetDirectoryName(savePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine($"下载: {url}");
            System.Console.WriteLine($"保存到: {savePath}");
            System.Console.WriteLine($"线程数: {threadCount}");
            System.Console.ResetColor();

            // 多线程下载
            return DownloadMultiThread(url, savePath, threadCount).Result;
        }

        /// <summary>
        /// 多线程分块下载
        /// </summary>
        private async Task<CommandResult> DownloadMultiThread(string url, string savePath, int threadCount)
        {
            try
            {
                // 先检查服务器是否支持 Range 请求
                if (!await SupportsRange(url))
                {
                    System.Console.WriteLine("服务器不支持断点续传，使用单线程下载");
                    return await DownloadFile(url, savePath);
                }

                // 获取文件大小
                long fileSize = await GetFileSize(url);
                if (fileSize <= 0)
                {
                    return await DownloadFile(url, savePath);
                }

                System.Console.WriteLine($"文件大小: {FormatBytes(fileSize)}");
                System.Console.WriteLine($"使用 {threadCount} 个线程下载...");

                // 计算每个线程下载的块大小
                long chunkSize = fileSize / threadCount;
                var tasks = new List<Task<(int id, bool success)>>();
                var tempFiles = new List<string>();

                // 进度跟踪
                var downloadedBytes = new long[threadCount];
                var progressLock = new object();
                DateTime lastUpdate = DateTime.Now;
                long lastTotal = 0;

                // 启动进度显示线程
                var progressTask = Task.Run(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(500);
                        lock (progressLock)
                        {
                            long totalDownloaded = downloadedBytes.Sum();
                            if (totalDownloaded >= fileSize)
                                break;

                            double percent = (double)totalDownloaded / fileSize * 100;
                            long speed = (totalDownloaded - lastTotal) * 2; // 估算速度

                            System.Console.Write($"\r进度: {percent:F1}% ({FormatBytes(totalDownloaded)} / {FormatBytes(fileSize)}) - {FormatSpeed(speed)}");
                            lastTotal = totalDownloaded;
                        }
                    }
                });

                // 启动下载任务
                for (int i = 0; i < threadCount; i++)
                {
                    int threadId = i;
                    long start = i * chunkSize;
                    long end = (i == threadCount - 1) ? fileSize - 1 : (i + 1) * chunkSize - 1;
                    string tempFile = savePath + $".part{threadId}";
                    tempFiles.Add(tempFile);

                    tasks.Add(DownloadChunk(url, tempFile, start, end, threadId, downloadedBytes, progressLock));
                }

                // 等待所有下载完成
                var results = await Task.WhenAll(tasks);

                // 检查是否全部成功
                if (results.All(r => r.success))
                {
                    System.Console.WriteLine("\n下载完成，正在合并文件...");

                    // 合并文件
                    using (var output = new FileStream(savePath, FileMode.Create))
                    {
                        foreach (var tempFile in tempFiles)
                        {
                            using (var _input = new FileStream(tempFile, FileMode.Open))
                            {
                                await _input.CopyToAsync(output, BUFFER_SIZE);
                            }
                        }
                    }

                    // 清理临时文件
                    foreach (var tempFile in tempFiles)
                    {
                        try { File.Delete(tempFile); } catch { }
                    }

                    System.Console.WriteLine("合并完成！");

                    // 询问是否打开文件夹
                    System.Console.Write("是否打开文件所在文件夹？[y/N]: ");
                    string input = System.Console.ReadLine()?.Trim().ToLower();
                    if (input == "y" || input == "yes")
                    {
                        string directory = Path.GetDirectoryName(savePath);
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = directory,
                            UseShellExecute = true
                        });
                    }

                    return new CommandResult
                    {
                        Success = true,
                        Response = $"文件已保存到: {savePath}"
                    };
                }
                else
                {
                    // 清理临时文件
                    foreach (var tempFile in tempFiles)
                    {
                        try { File.Delete(tempFile); } catch { }
                    }

                    return new CommandResult
                    {
                        Success = false,
                        Response = "下载失败: 部分分块下载失败"
                    };
                }
            }
            catch (Exception ex)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = $"下载失败: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// 下载单个分块
        /// </summary>
        private async Task<(int id, bool success)> DownloadChunk(string url, string savePath, long start, long end, int threadId, long[] downloadedBytes, object progressLock)
        {
            for (int retry = 0; retry < MAX_RETRIES; retry++)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.Timeout = TimeSpan.FromMinutes(30);
                        httpClient.DefaultRequestHeaders.Add("User-Agent",
                            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
                        httpClient.DefaultRequestHeaders.Add("Range", $"bytes={start}-{end}");

                        using (var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCode();

                            using (var stream = await response.Content.ReadAsStreamAsync())
                            using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None, BUFFER_SIZE, true))
                            {
                                byte[] buffer = new byte[BUFFER_SIZE];
                                int bytesRead;
                                long chunkDownloaded = 0;

                                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                {
                                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                                    chunkDownloaded += bytesRead;

                                    // 更新进度
                                    lock (progressLock)
                                    {
                                        downloadedBytes[threadId] = chunkDownloaded;
                                    }
                                }
                            }
                        }
                    }

                    return (threadId, true);
                }
                catch (Exception ex)
                {
                    if (retry < MAX_RETRIES - 1)
                    {
                        System.Console.WriteLine($"\n线程 {threadId} 下载失败: {ex.Message}，重试中...");
                        await Task.Delay(RETRY_DELAY_MS);
                    }
                    else
                    {
                        System.Console.WriteLine($"\n线程 {threadId} 下载失败: {ex.Message}");
                        return (threadId, false);
                    }
                }
            }

            return (threadId, false);
        }

        /// <summary>
        /// 检查服务器是否支持 Range 请求
        /// </summary>
        private async Task<bool> SupportsRange(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Head, url);
                    var response = await httpClient.SendAsync(request);
                    return response.Headers.Contains("Accept-Ranges") ||
                           response.Content.Headers.Contains("Content-Range");
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        private async Task<long> GetFileSize(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Head, url);
                    var response = await httpClient.SendAsync(request);
                    return response.Content.Headers.ContentLength ?? -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 单线程下载（回退方案）
        /// </summary>
        private async Task<CommandResult> DownloadFile(string url, string savePath)
        {
            for (int retry = 0; retry < MAX_RETRIES; retry++)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.Timeout = TimeSpan.FromMinutes(30);
                        httpClient.DefaultRequestHeaders.Add("User-Agent",
                            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 Chrome/120.0.0.0 Safari/537.36");
                        httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                        httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");

                        System.Console.WriteLine("正在连接...");

                        using (var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCode();

                            long? totalBytes = response.Content.Headers.ContentLength;
                            if (totalBytes.HasValue)
                            {
                                System.Console.WriteLine($"文件大小: {FormatBytes(totalBytes.Value)}");
                            }

                            using (var stream = await response.Content.ReadAsStreamAsync())
                            using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None, BUFFER_SIZE, true))
                            {
                                byte[] buffer = new byte[BUFFER_SIZE];
                                long totalRead = 0;
                                int bytesRead;
                                DateTime lastUpdate = DateTime.Now;
                                long lastBytes = 0;

                                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                {
                                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                                    totalRead += bytesRead;

                                    if (DateTime.Now - lastUpdate > TimeSpan.FromMilliseconds(500))
                                    {
                                        if (totalBytes.HasValue)
                                        {
                                            double percent = (double)totalRead / totalBytes.Value * 100;
                                            long bytesPerSecond = (totalRead - lastBytes) * 2;
                                            string speed = FormatSpeed(bytesPerSecond);

                                            System.Console.Write($"\r进度: {percent:F1}% ({FormatBytes(totalRead)} / {FormatBytes(totalBytes.Value)}) - {speed}");
                                        }
                                        else
                                        {
                                            System.Console.Write($"\r已下载: {FormatBytes(totalRead)}");
                                        }
                                        lastUpdate = DateTime.Now;
                                        lastBytes = totalRead;
                                    }
                                }
                            }
                        }

                        System.Console.WriteLine("\n下载完成！");

                        System.Console.Write("是否打开文件所在文件夹？[y/N]: ");
                        string input = System.Console.ReadLine()?.Trim().ToLower();
                        if (input == "y" || input == "yes")
                        {
                            string directory = Path.GetDirectoryName(savePath);
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = directory,
                                UseShellExecute = true
                            });
                        }

                        return new CommandResult
                        {
                            Success = true,
                            Response = $"文件已保存到: {savePath}"
                        };
                    }
                }
                catch (Exception ex)
                {
                    if (File.Exists(savePath))
                    {
                        try { File.Delete(savePath); } catch { }
                    }

                    if (retry < MAX_RETRIES - 1)
                    {
                        System.Console.ForegroundColor = ConsoleColor.Yellow;
                        System.Console.WriteLine($"\n下载失败: {ex.Message}");
                        System.Console.WriteLine($"正在重试 ({retry + 1}/{MAX_RETRIES})...");
                        System.Console.ResetColor();
                        await Task.Delay(RETRY_DELAY_MS);
                    }
                    else
                    {
                        return new CommandResult
                        {
                            Success = false,
                            Response = $"下载失败: {ex.Message}"
                        };
                    }
                }
            }

            return new CommandResult
            {
                Success = false,
                Response = "下载失败: 已达到最大重试次数"
            };
        }

        private string GetFileNameFromUrl(string url)
        {
            try
            {
                string fileName = Path.GetFileName(new Uri(url).LocalPath);
                if (!string.IsNullOrEmpty(fileName))
                {
                    return fileName;
                }
            }
            catch { }

            return "download_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
        }

        private string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private string FormatSpeed(long bytesPerSecond)
        {
            if (bytesPerSecond < 1024)
                return $"{bytesPerSecond} B/s";
            if (bytesPerSecond < 1024 * 1024)
                return $"{bytesPerSecond / 1024.0:F1} KB/s";
            return $"{bytesPerSecond / 1024.0 / 1024.0:F1} MB/s";
        }

        private string GetUsageHelp()
        {
            return "用法: wget <URL> [保存路径] [-t 线程数]\n" +
                   "示例:\n" +
                   "  wget https://github.com/xxx/file.zip\n" +
                   "  wget https://xxx.com/file.exe D:\\Downloads\\\n" +
                   "  wget https://xxx.com/file.exe D:\\Downloads\\file.exe -t 8\n\n" +
                   "参数:\n" +
                   "  -t <线程数>  指定下载线程数 (默认4，最大16)\n\n" +
                   "提示:\n" +
                   "  • 支持多线程下载，速度更快\n" +
                   "  • 自动检测服务器是否支持断点续传\n" +
                   "  • 失败自动重试最多3次\n" +
                   "  • 使用1MB缓冲区提升速度";
        }
    }
}