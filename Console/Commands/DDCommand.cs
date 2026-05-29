using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using QisToolkit3.Console.Services;

namespace QisToolkit3.Console.Commands
{
    public class DDCommand : ICommand
    {
        private readonly Dictionary<string, ICommand> _handlers;
        private readonly MultiLevelAliasService _globalAliasService;
        private readonly MultiLevelAliasService _softwareAliasService;
        private readonly Func<List<string>> _getReturnData;

        public string Name => "dd";
        public string Description => "快速命令入口 (Do something quickly)";
        public string[] Aliases => Array.Empty<string>();

        public DDCommand(Dictionary<string, ICommand> handlers, Func<List<string>> getReturnData)
        {
            _handlers = handlers;
            _getReturnData = getReturnData;
            _globalAliasService = new MultiLevelAliasService();
            _softwareAliasService = new MultiLevelAliasService("software");
        }

        private List<string> GetReturnData()
        {
            return _getReturnData?.Invoke() ?? new List<string>();
        }

        public CommandResult Execute(string[] args)
        {
            if (args.Length == 0)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = GetHelpText()
                };
            }

            string firstArg = args[0];
            string[] remainingArgs = args.Length > 1 ? args.Skip(1).ToArray() : new string[0];

            // 特殊处理：dd & 将 & 中的所有数据作为命令执行
            if (firstArg == "@" && remainingArgs.Length == 0)
            {
                //System.Console.WriteLine(firstArg);
                return ExecuteAmpersandCommand();
            }

            // 检查操作符前缀
            string operation = GetOperation(firstArg, out string actualTarget);

            // 重新组合参数
            string[] finalArgs = new[] { actualTarget }.Concat(remainingArgs).ToArray();

            switch (operation)
            {
                // 新增：dd -进程 (杀死)
                case "kill":
                    return ExecuteKill(actualTarget);

                // 新增：dd /进程 (重启)
                case "restart":
                    return ExecuteRestart(actualTarget);

                // 新增：dd +文件 (强制打开)
                case "force_open":
                    return ExecuteOpen(actualTarget, forceOpen: true);

                // 原有：dd 内容 (智能识别 - 自动判断用哪个命令)
                case "smart_original":
                    return ExecuteSmartOriginal(actualTarget, remainingArgs);

                // 原有：dd 内容 (智能模式 - 未运行则打开，已运行则重启)
                case "smart_app":
                    return ExecuteSmartApp(actualTarget, remainingArgs);

                default:
                    return new CommandResult
                    {
                        Success = false,
                        Response = $"无法识别的操作: {firstArg}\n{GetHelpText()}"
                    };
            }
        }

        /// <summary>
        /// 执行 & 中存储的命令
        /// </summary>
        private CommandResult ExecuteAmpersandCommand()
        {
            var returnData = GetReturnData();

            if (returnData == null || returnData.Count == 0)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = "& 中没有数据"
                };
            }

            // 将所有数据组合成一个命令行
            string commandLine = string.Join(" ", returnData);

            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine($"[dd] 执行 & 中的命令: {commandLine}");
            System.Console.ResetColor();

            // 解析命令
            string[] parts = CommandLineParser.Parse(commandLine);
            if (parts.Length == 0)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = "& 中的命令为空"
                };
            }

            string cmdName = parts[0];
            string[] cmdArgs = parts.Length > 1 ? parts.Skip(1).ToArray() : new string[0];

            // 查找对应的命令
            if (_handlers.TryGetValue(cmdName, out var targetCmd))
            {
                return targetCmd.Execute(cmdArgs);
            }

            return new CommandResult
            {
                Success = false,
                Response = $"未知命令: {cmdName}"
            };
        }

        /// <summary>
        /// 解析操作符和实际目标
        /// </summary>
        private string GetOperation(string input, out string actualTarget)
        {
            actualTarget = input;

            // dd +文件 (强制打开)
            if (input.StartsWith("+"))
            {
                actualTarget = input.Substring(1);
                return "force_open";
            }

            // dd -进程 (杀死)
            if (input.StartsWith("-"))
            {
                actualTarget = input.Substring(1);
                return "kill";
            }

            // dd /进程 (重启)
            if (input.StartsWith("/"))
            {
                actualTarget = input.Substring(1);
                return "restart";
            }

            // 原有的智能识别逻辑
            return "smart_original";
        }

        /// <summary>
        /// 原有功能：智能识别用哪个命令（bilibili、ob等）
        /// </summary>
        private CommandResult ExecuteSmartOriginal(string input, string[] remainingArgs)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine($"[dd] 尝试自动识别: {input}");
            System.Console.ResetColor();

            // 1. 检查是否是通用别名
            if (_globalAliasService.IsAlias(input))
            {
                string resolved = _globalAliasService.Resolve(input);

                ICommand matchedHandler = null;

                if (IsUrl(resolved))
                {
                    _handlers.TryGetValue("openbrowser", out matchedHandler);
                }
                else if (IsAudioVideoFormat(resolved))
                {
                    if (remainingArgs.Length >= 1 && File.Exists(remainingArgs[0]))
                    {
                        _handlers.TryGetValue("convert", out matchedHandler);
                    }
                }
                else
                {
                    matchedHandler = FindHandlerForValue(resolved);
                }

                if (matchedHandler != null)
                {
                    string[] allArgs = new[] { resolved }.Concat(remainingArgs).ToArray();
                    return matchedHandler.Execute(allArgs);
                }
            }

            // 2. 检查是否能匹配到某个命令
            var possibleHandlers = FindPossibleHandlers(input);

            if (possibleHandlers.Count == 1)
            {
                var handler = possibleHandlers[0];
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.WriteLine($"[dd] 自动匹配到命令: {handler.Name}");
                System.Console.ResetColor();

                string[] allArgs = new[] { input }.Concat(remainingArgs).ToArray();
                return handler.Execute(allArgs);
            }
            else if (possibleHandlers.Count > 1)
            {
                return AskUserToChoose(possibleHandlers, input, remainingArgs);
            }

            // 3. 没有匹配到，尝试作为应用程序处理（智能打开/重启）
            return ExecuteSmartApp(input, remainingArgs);
        }

        /// <summary>
        /// 新功能：应用程序智能模式 - 未运行则打开，已运行则重启
        /// </summary>
        private CommandResult ExecuteSmartApp(string target, string[] remainingArgs)
        {
            // 解析别名
            string resolvedTarget = ResolveAlias(target);

            // 获取可执行文件路径
            string exePath = GetExecutablePath(resolvedTarget);
            if (string.IsNullOrEmpty(exePath))
            {
                return new CommandResult
                {
                    Success = false,
                    Response = $"无法确定程序路径: {target}\n" +
                              "请确保:\n" +
                              "  1. 文件存在\n" +
                              "  2. 程序在PATH中\n" +
                              "  3. 已配置软件别名"
                };
            }

            string processName = Path.GetFileNameWithoutExtension(exePath);

            // 检查是否正在运行
            var runningProcesses = Process.GetProcessesByName(processName);

            if (runningProcesses.Length > 0)
            {
                // 已运行，执行重启
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.WriteLine($"[dd] 程序已在运行，执行重启: {processName}");
                System.Console.ResetColor();
                return ExecuteRestart(target);
            }
            else
            {
                // 未运行，执行打开
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.WriteLine($"[dd] 程序未运行，执行启动: {processName}");
                System.Console.ResetColor();
                return ExecuteOpen(target, forceOpen: true);
            }
        }

        /// <summary>
        /// 执行打开操作（文件/程序/URL）
        /// </summary>
        private CommandResult ExecuteOpen(string target, bool forceOpen = false)
        {
            // 解析别名
            string resolvedTarget = ResolveAlias(target);

            try
            {
                // 检查是否是文件
                if (File.Exists(resolvedTarget))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = resolvedTarget,
                        UseShellExecute = true
                    });
                    return new CommandResult
                    {
                        Success = true,
                        Response = $"已打开: {Path.GetFileName(resolvedTarget)}"
                    };
                }

                // 检查是否是目录
                if (Directory.Exists(resolvedTarget))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = resolvedTarget,
                        UseShellExecute = true
                    });
                    return new CommandResult
                    {
                        Success = true,
                        Response = $"已打开目录: {resolvedTarget}"
                    };
                }

                // 检查是否是URL
                if (IsUrl(resolvedTarget))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = resolvedTarget,
                        UseShellExecute = true
                    });
                    return new CommandResult
                    {
                        Success = true,
                        Response = $"正在打开: {resolvedTarget}"
                    };
                }

                // 尝试作为可执行文件打开
                if (resolvedTarget.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) && File.Exists(resolvedTarget))
                {
                    Process.Start(resolvedTarget);
                    return new CommandResult
                    {
                        Success = true,
                        Response = $"已启动: {Path.GetFileName(resolvedTarget)}"
                    };
                }

                // 尝试在 PATH 中查找
                string pathExe = FindInPath(resolvedTarget);
                if (pathExe != null)
                {
                    Process.Start(pathExe);
                    return new CommandResult
                    {
                        Success = true,
                        Response = $"已启动: {resolvedTarget}"
                    };
                }

                return new CommandResult
                {
                    Success = false,
                    Response = $"无法打开: {target}\n" +
                              "请确保:\n" +
                              "  1. 文件/目录存在\n" +
                              "  2. 是有效的URL\n" +
                              "  3. 程序在PATH中"
                };
            }
            catch (Exception ex)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = $"打开失败: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// 执行杀死进程
        /// </summary>
        private CommandResult ExecuteKill(string target)
        {
            // 解析别名
            string resolvedTarget = ResolveAlias(target);

            // 获取进程名
            string processName = Path.GetFileNameWithoutExtension(resolvedTarget);

            // 调用 kill 命令
            if (_handlers.TryGetValue("kill", out var killCmd))
            {
                return killCmd.Execute(new[] { processName });
            }

            return new CommandResult
            {
                Success = false,
                Response = "kill 命令不可用"
            };
        }

        /// <summary>
        /// 执行重启进程
        /// </summary>
        private CommandResult ExecuteRestart(string target)
        {
            // 解析别名
            string resolvedTarget = ResolveAlias(target);

            // 调用 software restart 命令
            if (_handlers.TryGetValue("software", out var swCmd))
            {
                return swCmd.Execute(new[] { "restart", resolvedTarget });
            }

            return new CommandResult
            {
                Success = false,
                Response = "software 命令不可用"
            };
        }

        /// <summary>
        /// 解析别名（优先软件别名，再通用别名）
        /// </summary>
        private string ResolveAlias(string input)
        {
            if (_softwareAliasService.IsAlias(input))
            {
                string resolved = _softwareAliasService.Resolve(input);
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.WriteLine($"[dd] 软件别名 '{input}' -> {resolved}");
                System.Console.ResetColor();
                return resolved;
            }

            if (_globalAliasService.IsAlias(input))
            {
                string resolved = _globalAliasService.Resolve(input);
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.WriteLine($"[dd] 通用别名 '{input}' -> {resolved}");
                System.Console.ResetColor();
                return resolved;
            }

            return input;
        }

        /// <summary>
        /// 获取可执行文件路径
        /// </summary>
        private string GetExecutablePath(string target)
        {
            // 如果是文件路径
            if (File.Exists(target))
            {
                return target;
            }

            // 如果是 exe 文件名（不带路径）
            if (target.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                string pathExe = FindInPath(target);
                if (pathExe != null) return pathExe;
            }

            // 尝试作为程序名（不带.exe）
            string withExe = target + ".exe";
            string pathWithExe = FindInPath(withExe);
            if (pathWithExe != null) return pathWithExe;

            return null;
        }

        /// <summary>
        /// 找出可能匹配的处理器（原有功能）
        /// </summary>
        private List<ICommand> FindPossibleHandlers(string input)
        {
            var matches = new List<ICommand>();
            var seen = new HashSet<ICommand>();

            // 检查是否是URL
            if (IsUrl(input))
            {
                if (_handlers.TryGetValue("openbrowser", out var obCmd))
                {
                    if (!seen.Contains(obCmd))
                    {
                        seen.Add(obCmd);
                        matches.Add(obCmd);
                    }
                }
            }

            // 检查是否是音频/视频格式转换
            if (IsAudioVideoFormat(input))
            {
                if (_handlers.TryGetValue("convert", out var convertCmd))
                {
                    if (!seen.Contains(convertCmd))
                    {
                        seen.Add(convertCmd);
                        matches.Add(convertCmd);
                    }
                }
            }

            // 检查其他命令
            foreach (var handler in _handlers.Values)
            {
                if (handler.Name == "dd" || handler.Name == "openbrowser" || handler.Name == "convert")
                    continue;

                if (handler is IBilibiliCommand bCmd && bCmd.CanHandle(input))
                {
                    if (!seen.Contains(handler))
                    {
                        seen.Add(handler);
                        matches.Add(handler);
                    }
                }
            }

            return matches;
        }

        /// <summary>
        /// 根据值找到合适的处理器（原有功能）
        /// </summary>
        private ICommand FindHandlerForValue(string value)
        {
            // B站：数字或av/BV开头
            if (long.TryParse(value, out _) ||
                value.StartsWith("av", StringComparison.OrdinalIgnoreCase) ||
                value.StartsWith("BV", StringComparison.OrdinalIgnoreCase))
            {
                return _handlers.GetValueOrDefault("bilibili");
            }

            // GitHub：包含 "/" 的格式
            if (value.Contains("/") && !value.StartsWith("http"))
            {
                return _handlers.GetValueOrDefault("github");
            }

            return null;
        }

        /// <summary>
        /// 让用户选择用哪个命令（原有功能）
        /// </summary>
        private CommandResult AskUserToChoose(List<ICommand> handlers, string input, string[] remainingArgs)
        {
            var uniqueHandlers = new HashSet<ICommand>(handlers).ToList();

            System.Console.WriteLine($"\n发现多个命令可以处理 '{input}':");
            for (int i = 0; i < uniqueHandlers.Count; i++)
            {
                System.Console.WriteLine($"  [{i + 1}] {uniqueHandlers[i].Name} - {uniqueHandlers[i].Description}");
            }

            System.Console.Write("\n请选择 [1-{0}], 或按回车取消: ", uniqueHandlers.Count);

            string choice = System.Console.ReadLine()?.Trim();

            if (int.TryParse(choice, out int index) && index >= 1 && index <= uniqueHandlers.Count)
            {
                var handler = uniqueHandlers[index - 1];
                string[] allArgs = new[] { input }.Concat(remainingArgs).ToArray();
                return handler.Execute(allArgs);
            }

            return new CommandResult
            {
                Success = false,
                Response = "已取消"
            };
        }

        /// <summary>
        /// 判断是否是URL
        /// </summary>
        private bool IsUrl(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            return input.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                   input.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                   (input.Contains(".") && !input.Contains(" ") &&
                    !input.StartsWith("\\") && !Path.IsPathRooted(input));
        }

        /// <summary>
        /// 判断是否是音频/视频格式
        /// </summary>
        private bool IsAudioVideoFormat(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            string[] formats = { "mp3", "flac", "wav", "aac", "ogg", "m4a",
                                 "mp4", "mkv", "avi", "mov", "gif", "webm" };

            return formats.Contains(input.ToLower());
        }

        /// <summary>
        /// 在PATH中查找可执行文件
        /// </summary>
        private string FindInPath(string exeName)
        {
            string pathEnv = Environment.GetEnvironmentVariable("PATH");
            if (string.IsNullOrEmpty(pathEnv)) return null;

            foreach (string path in pathEnv.Split(';'))
            {
                try
                {
                    string fullPath = Path.Combine(path, exeName);
                    if (File.Exists(fullPath))
                        return fullPath;

                    fullPath = Path.Combine(path, exeName + ".exe");
                    if (File.Exists(fullPath))
                        return fullPath;
                }
                catch { continue; }
            }

            return null;
        }

        private string GetHelpText()
        {
            return
                "\n=== DD命令 - 快速命令入口 ===\n\n" +
                "【智能识别】\n" +
                "  dd av123456          - 打开B站视频\n" +
                "  dd bilibili.com      - 打开网页\n" +
                "  dd mp3 \"音乐.flac\"   - 格式转换\n\n" +
                "【应用管理】\n" +
                "  dd notepad           - 未运行则打开，已运行则重启\n" +
                "  dd +notepad          - 强制打开（新窗口）\n" +
                "  dd -notepad          - 终止进程\n" +
                "  dd /notepad          - 重启进程\n\n" +
                "【其他功能】\n" +
                "  dd +\"C:\\path\\app.exe\" - 打开指定程序\n" +
                "  dd \"D:\\文档.docx\"     - 打开文档\n" +
                "  dd https://github.com - 打开网页\n\n" +
                "【别名支持】\n" +
                "  dd vscode            - 使用软件别名\n" +
                "  dd - vscode          - 关闭软件\n" +
                "  dd / vscode          - 重启软件\n\n" +
                "提示: 操作符(+ - /)放在引号外面: +\"C:\\Program Files\\app.exe\"";
        }
    }
}