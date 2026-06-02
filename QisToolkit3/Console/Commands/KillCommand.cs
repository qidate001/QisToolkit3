using System;
using System.Diagnostics;
using System.Linq;

namespace QisToolkit3.Console.Commands
{
    public class KillCommand : ICommand
    {
        public string Name => "kill";
        public string Description => "终止进程";
        public string[] Aliases => new[] { "k" };

        public CommandResult Execute(string[] args)
        {
            if (args.Length == 0)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = "用法: kill <进程名/PID>\n" +
                               "示例:\n" +
                               "  kill notepad\n" +
                               "  kill 12345\n" +
                               "  k chrome"
                };
            }

            string target = args[0];

            try
            {
                // 尝试按 PID 终止
                if (int.TryParse(target, out int pid))
                {
                    try
                    {
                        var process = Process.GetProcessById(pid);
                        string processName = process.ProcessName;
                        process.Kill();
                        process.WaitForExit(5000);

                        return new CommandResult
                        {
                            Success = true,
                            Response = $"已终止进程: {processName} (PID: {pid})"
                        };
                    }
                    catch (ArgumentException)
                    {
                        return new CommandResult
                        {
                            Success = false,
                            Response = $"未找到 PID 为 {pid} 的进程"
                        };
                    }
                }

                // 按进程名终止（杀死所有匹配的进程）
                var processes = Process.GetProcessesByName(target);

                if (processes.Length == 0)
                {
                    return new CommandResult
                    {
                        Success = false,
                        Response = $"未找到进程: {target}"
                    };
                }

                int killedCount = 0;
                foreach (var process in processes)
                {
                    try
                    {
                        string processName = process.ProcessName;
                        int processId = process.Id;
                        process.Kill();
                        process.WaitForExit(3000);
                        killedCount++;
                        System.Console.WriteLine($"已终止: {processName} (PID: {processId})");
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"终止失败: {process.ProcessName} - {ex.Message}");
                    }
                }

                return new CommandResult
                {
                    Success = killedCount > 0,
                    Response = killedCount > 0
                        ? $"共终止 {killedCount} 个进程"
                        : "未能终止任何进程"
                };
            }
            catch (Exception ex)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = $"终止进程失败: {ex.Message}"
                };
            }
        }
    }
}