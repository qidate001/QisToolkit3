using System;
using System.IO;
using System.Text.RegularExpressions;
using QisToolkit3.Console.Services;

namespace QisToolkit3.Console.Commands
{
    public class ConvertCommand : ICommand
    {
        private readonly FFmpegCommand _ffmpeg;
        private readonly MultiLevelAliasService _aliasService;

        public string Name => "convert";
        public string Description => "音频/视频格式转换 (dd的转换功能)";
        public string[] Aliases => new[] { "cv" };

        public ConvertCommand()
        {
            _ffmpeg = new FFmpegCommand();
            _aliasService = new MultiLevelAliasService("convert");
        }

        public CommandResult Execute(string[] args)
        {
            if (args.Length < 2)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = GetUsageHelp()
                };
            }

            string format = args[0].ToLower();

            // 调试：显示原始参数
            //System.Console.ForegroundColor = ConsoleColor.DarkGray;
            //System.Console.WriteLine($"[调试] 原始路径参数: '{args[1]}'");

            // 重要：处理路径中的反斜杠
            string inputPath = Qi.FixPath(args[1]);

            //System.Console.WriteLine($"[调试] 修复后路径: '{inputPath}'");
            //System.Console.ResetColor();

            format = ResolveFormatAlias(format);

            // 检查输入文件是否存在
            if (!File.Exists(inputPath))
            {
                // 尝试不同的路径变体
                string[] attempts = new[]
                {
                    inputPath,
                    inputPath.Replace('\\', '/'),
                    inputPath.Replace('/', '\\'),
                    args[1].Trim('"'),  // 原始参数去掉引号
                    args[1].Trim('"').Replace('\\', '/'),
                };

                bool found = false;
                foreach (var attempt in attempts.Distinct())
                {
                    System.Console.ForegroundColor = ConsoleColor.DarkGray;
                    System.Console.WriteLine($"[调试] 尝试: '{attempt}'");
                    System.Console.ResetColor();

                    if (File.Exists(attempt))
                    {
                        inputPath = attempt;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return CommandResult.Error(
                        $"输入文件不存在: {inputPath}\n" +
                        $"请检查路径是否正确，或使用完整路径。",

                        inputPath,                          // 数据0: 输入路径
                        "False"                             // 数据1: 是否成功
                    );
                }
            }

            // 生成输出文件路径
            string outputPath = GenerateOutputPath(inputPath, format);

            // 构建ffmpeg参数
            string ffmpegArgs = $"-i \"{inputPath}\" \"{outputPath}\"";

            // 如果有额外参数
            if (args.Length > 2)
            {
                string extraArgs = string.Join(" ", args, 2, args.Length - 2);
                ffmpegArgs = $"-i \"{inputPath}\" {extraArgs} \"{outputPath}\"";
            }

            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine($"[转换] {Path.GetFileName(inputPath)} -> {format}");
            System.Console.WriteLine($"[命令] ffmpeg {ffmpegArgs}");
            System.Console.ResetColor();

            var result = _ffmpeg.Execute(new[] { ffmpegArgs });

            if (result.Success)
            {
                result.Response = $"转换完成: {outputPath}";
                result.ReturnData = [outputPath, "True"];
            }

            return result;
        }

        /// <summary>
        /// 解析格式别名
        /// </summary>
        private string ResolveFormatAlias(string format)
        {
            return format switch
            {
                "mp3" => "mp3",
                "flac" => "flac",
                "wav" => "wav",
                "aac" => "aac",
                "ogg" => "ogg",
                "m4a" => "m4a",
                "mp4" => "mp4",
                "mkv" => "mkv",
                "avi" => "avi",
                "mov" => "mov",
                "gif" => "gif",
                _ => format
            };
        }

        /// <summary>
        /// 生成输出文件路径
        /// </summary>
        private string GenerateOutputPath(string inputPath, string format)
        {
            string directory = Path.GetDirectoryName(inputPath);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(inputPath);

            if (string.IsNullOrEmpty(directory))
            {
                directory = Environment.CurrentDirectory;
            }

            return Path.Combine(directory, $"{fileNameWithoutExt}.{format}");
        }

        private string GetUsageHelp()
        {
            return "用法: convert <格式> <文件路径> [额外ffmpeg参数]\n" +
                   "示例:\n" +
                   "  convert mp3 \"E:\\KuGou\\音乐.flac\"\n" +
                   "  convert flac \"音频.mp3\"\n" +
                   "  convert mp4 \"视频.avi\"\n" +
                   "  convert mp3 \"音乐.flac\" -b:a 192k\n\n" +
                   "支持格式: mp3, flac, wav, aac, ogg, m4a, mp4, mkv, avi, mov, gif";
        }
    }
}