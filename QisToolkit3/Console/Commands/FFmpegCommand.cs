using System;
using System.Diagnostics;
using System.Text;
using System.IO;
using QisToolkit3.Console.Services;

namespace QisToolkit3.Console.Commands
{
    public class FFmpegCommand : ICommand
    {
        private readonly string _ffmpegPath;

        public string Name => "ffmpeg";
        public string Description => "执行ffmpeg命令";
        public string[] Aliases => new[] { "ff" };

        public FFmpegCommand()
        {
            _ffmpegPath = System.IO.Path.Combine(
                Qi.QisToolkit3_Datas.actualDirectory,
                "yt-dlp",
                "ffmpeg.exe"
            );
        }

        public CommandResult Execute(string[] args)
        {
            if (args.Length == 0)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = "用法: ffmpeg <ffmpeg参数...>\n" +
                              "示例: ffmpeg -i input.mp3 output.flac"
                };
            }

            // 调试：显示ffmpeg路径
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine($"[调试] FFmpeg路径: {_ffmpegPath}");
            System.Console.WriteLine($"[调试] FFmpeg存在: {File.Exists(_ffmpegPath)}");
            System.Console.ResetColor();

            // 检查ffmpeg是否存在
            if (!System.IO.File.Exists(_ffmpegPath))
            {
                return CommandResult.Error(
                    $"找不到ffmpeg: {_ffmpegPath}\n请确保文件存在于正确位置。",
                    _ffmpegPath,                        // 数据0: FFmpeg路径
                    "False"                             // 数据1: 是否成功
                );
            }

            // 构建完整命令
            string arguments = string.Join(" ", args);

            // 调试：显示工作目录
            string workDir = Path.GetDirectoryName(_ffmpegPath);
            //System.Console.ForegroundColor = ConsoleColor.DarkGray;
            //System.Console.WriteLine($"[调试] 工作目录: {workDir}");
            //System.Console.WriteLine($"[调试] 完整命令: {_ffmpegPath} {arguments}");
            //System.Console.ResetColor();

            return ExecuteFFmpeg(arguments, workDir);
        }

        private CommandResult ExecuteFFmpeg(string arguments, string workDir)
        {
            try
            {
                var output = new StringBuilder();
                var error = new StringBuilder();

                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = _ffmpegPath,  // 直接调用ffmpeg，不用cmd
                        Arguments = arguments,
                        WorkingDirectory = workDir,  // 设置工作目录
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        StandardOutputEncoding = Encoding.UTF8,
                        StandardErrorEncoding = Encoding.UTF8
                    };

                    // 调试：显示启动信息
                    //System.Console.ForegroundColor = ConsoleColor.DarkGray;
                    //System.Console.WriteLine($"[调试] 启动进程: {_ffmpegPath} {arguments}");
                    //System.Console.ResetColor();

                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            output.AppendLine(e.Data);
                            System.Console.WriteLine(e.Data);
                        }
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            error.AppendLine(e.Data);
                            System.Console.ForegroundColor = ConsoleColor.Yellow;
                            System.Console.WriteLine(e.Data);
                            System.Console.ResetColor();
                        }
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    // 等待退出，设置超时
                    if (process.WaitForExit(30000)) // 30秒超时
                    {
                        if (process.ExitCode == 0)
                        {
                            return CommandResult.Ok(
                                "FFmpeg 执行完成",
                                _ffmpegPath,                        // 数据0: FFmpeg路径
                                "True",                             // 数据1: 是否成功
                                arguments,                          // 数据2: 参数
                                workDir                             // 数据3: 工作路径
                            );
                        }
                        else
                        {
                            return CommandResult.Error(
                                $"FFmpeg 执行失败，退出代码: {process.ExitCode}\n{error}",
                                process.ExitCode.ToString(),        // 数据0: 退出代码
                                "False",                            // 数据1: 是否成功
                                error.ToString(),                   // 数据2: 错误消息
                                arguments,                          // 数据3: 参数
                                workDir                             // 数据4: 工作路径
                            );
                        }
                    }
                    else
                    {
                        // 超时，强制结束
                        process.Kill();
                        return CommandResult.Error(
                            $"FFmpeg 执行超时 (30秒)",
                            process.ExitCode.ToString(),        // 数据0: 退出代码
                            "False",                            // 数据1: 是否成功
                            error.ToString(),                   // 数据2: 错误消息
                            arguments,                          // 数据3: 参数
                            workDir                             // 数据4: 工作路径
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                return CommandResult.Error(
                    $"执行FFmpeg时出错: {ex.Message}",
                    ex.Message,                         // 数据0: 错误消息
                    "False"                             // 数据1: 是否成功
                );
            }
        }
    }
}