using System;
using System.Diagnostics;
using System.IO;
using QisToolkit3.Console.Services;

namespace QisToolkit3.Console.Commands
{
    public class SoftwareCommand : ICommand
    {
        private readonly MultiLevelAliasService _aliasService;

        public string Name => "software";
        public string Description => "软件管理 (启动/重启)";
        public string[] Aliases => new[] { "sw" };

        public SoftwareCommand()
        {
            _aliasService = new MultiLevelAliasService("software");
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

            string action = args[0].ToLower();
            string target = args[1];

            switch (action)
            {
                case "start":
                    return StartSoftware(target);

                case "restart":
                    return RestartSoftware(target);

                default:
                    return new CommandResult
                    {
                        Success = false,
                        Response = $"未知操作: {action}\n{GetUsageHelp()}"
                    };
            }
        }

        private CommandResult StartSoftware(string target)
        {
            // 解析别名
            string resolvedTarget = _aliasService.Resolve(target);

            if (resolvedTarget != target)
            {
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.WriteLine($"别名 '{target}' 解析为: {resolvedTarget}");
                System.Console.ResetColor();
            }

            // 检查是否已运行
            string processName = Path.GetFileNameWithoutExtension(resolvedTarget);
            var existingProcesses = Process.GetProcessesByName(processName);

            if (existingProcesses.Length > 0)
            {
                return CommandResult.Ok(
                    $"程序已在运行: {processName} (PID: {existingProcesses[0].Id})\n如需重启请使用: software restart",
                    resolvedTarget,                     // 数据0: 程序路径
                    "False",                            // 数据1: 是否成功
                    existingProcesses[0].Id.ToString()  // 数据2: PID
                );
            }

            // 启动程序
            try
            {
                if (!File.Exists(resolvedTarget))
                {
                    return CommandResult.Error($"文件不存在: {resolvedTarget}");
                }

                var process = Process.Start(resolvedTarget);

                // 等待一下让进程完全启动
                Thread.Sleep(500);

                // 获取新进程的PID
                int newPid = process?.Id ?? 0;

                // 返回数据: 程序路径, 是否成功(True), 新PID
                return CommandResult.Ok(
                    $"已启动: {Path.GetFileName(resolvedTarget)} (PID: {newPid})",
                    resolvedTarget,   // 数据0: 程序路径
                    "True",           // 数据1: 是否成功
                    newPid.ToString() // 数据2: PID
                );
            }
            catch (Exception ex)
            {
                return CommandResult.Error(
                    $"启动失败: {ex.Message}",
                    resolvedTarget,   // 数据0: 程序路径
                    "False",          // 数据1: 是否成功
                    ex.Message        // 数据2: 错误信息
                );
            }
        }

        private CommandResult RestartSoftware(string target)
        {
            // 解析别名
            string resolvedTarget = _aliasService.Resolve(target);

            if (resolvedTarget != target)
            {
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.WriteLine($"别名 '{target}' 解析为: {resolvedTarget}");
                System.Console.ResetColor();
            }

            string processName = Path.GetFileNameWithoutExtension(resolvedTarget);
            string exePath = null;
            int killedCount = 0;

            // 1. 先获取所有匹配的进程信息
            var processes = Process.GetProcessesByName(processName);

            if (processes.Length == 0)
            {
                // 如果没有找到进程，直接启动
                System.Console.WriteLine($"未找到运行中的进程: {processName}，直接启动");
                return StartSoftware(target);
            }

            // 2. 记录第一个进程的路径（用于重启）
            foreach (var process in processes)
            {
                try
                {
                    // 获取进程的可执行文件路径
                    string processPath = process.MainModule?.FileName;
                    if (!string.IsNullOrEmpty(processPath))
                    {
                        exePath = processPath;
                        System.Console.WriteLine($"检测到进程: {process.ProcessName} (PID: {process.Id})");
                        System.Console.WriteLine($"  路径: {exePath}");
                        break; // 记录第一个即可
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"无法获取进程 {process.Id} 的路径: {ex.Message}");
                }
            }

            // 3. 杀死所有进程
            foreach (var process in processes)
            {
                try
                {
                    System.Console.WriteLine($"正在终止: {process.ProcessName} (PID: {process.Id})");
                    process.Kill();
                    process.WaitForExit(5000);
                    killedCount++;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"终止失败: {ex.Message}");
                }
            }

            if (killedCount > 0)
            {
                System.Console.WriteLine($"已终止 {killedCount} 个进程");

                // 等待一下确保进程完全退出
                Thread.Sleep(1000);
            }

            // 4. 确定要启动的路径
            string startPath = null;

            // 优先使用获取到的进程路径
            if (!string.IsNullOrEmpty(exePath) && File.Exists(exePath))
            {
                startPath = exePath;
                System.Console.WriteLine($"使用进程路径重启: {startPath}");
            }
            // 其次使用用户指定的路径或别名
            else if (File.Exists(resolvedTarget))
            {
                startPath = resolvedTarget;
                System.Console.WriteLine($"使用指定路径启动: {startPath}");
            }
            else
            {
                return CommandResult.Error(
                    $"无法确定程序路径: {resolvedTarget}\n" +
                              "请确保:\n" +
                              "  1. 程序正在运行，或\n" +
                              "  2. 提供了正确的可执行文件路径",
                    resolvedTarget,   // 数据0: 程序路径
                    "False"           // 数据1: 是否成功
                );
            }

            // 5. 启动程序
            try
            {
                var process = Process.Start(startPath);
                int newPid = process?.Id ?? 0;

                return CommandResult.Ok(
                    killedCount > 0
                        ? $"已重启: {Path.GetFileName(startPath)} (新PID: {newPid})"
                        : $"已启动: {Path.GetFileName(startPath)} (PID: {newPid})",
                    resolvedTarget,   // 数据0: 程序路径
                    "True",           // 数据1: 是否成功
                    newPid.ToString() // 数据2: PID
                );
            }
            catch (Exception ex)
            {
                return CommandResult.Error(
                    $"启动失败: {ex.Message}",
                    resolvedTarget,   // 数据0: 程序路径
                    "False",          // 数据1: 是否成功
                    ex.Message        // 数据2: 错误信息
                );
            }
        }

        private string GetUsageHelp()
        {
            return "用法:\n" +
                   "  software start <别名/EXE路径>  - 启动程序\n" +
                   "  software restart <别名/EXE路径> - 重启程序\n" +
                   "  sw start notepad\n" +
                   "  sw restart myapp\n\n" +

                   "别名管理:\n" +
                   "  sw list   - 查看所有软件别名\n" +
                   "  sw add <别名> <路径> [描述] - 添加别名\n" +
                   "  sw remove <别名> - 删除别名\n" +
                   "  sw reload - 重新加载配置";
        }
    }
}