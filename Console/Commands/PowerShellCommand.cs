using System;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace QisToolkit3.Console.Commands
{
    public class PowerShellCommand : ICommand
    {
        public string Name => "ps";
        public string Description => "直接执行PowerShell命令";
        public string[] Aliases => new[] { "!!" };

        public CommandResult Execute(string[] args)
        {
            if (args.Length == 0)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = "用法: ps <命令>\n" +
                               "示例:\n" +
                               "  ps dir\n" +
                               "  ps ipconfig\n" +
                               "  ps ping 8.8.8.8\n" +
                               "  !! echo hello world  (使用别名)"
                };
            }

            // 将参数重新组合成命令
            string command = string.Join(" ", args);

            return ExecuteCmd(command);
        }

        private CommandResult ExecuteCmd(string command)
        {
            try
            {
                var output = new StringBuilder();
                var error = new StringBuilder();

                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"/c {command}",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        StandardOutputEncoding = Encoding.GetEncoding("GB2312"), // CMD默认用GBK
                        StandardErrorEncoding = Encoding.GetEncoding("GB2312")
                    };

                    // 显示执行命令
                    //System.Console.ForegroundColor = ConsoleColor.DarkGray;
                    //System.Console.WriteLine($"[CMD] {command}");
                    //System.Console.ResetColor();

                    // 实时输出
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            System.Console.ForegroundColor = ConsoleColor.Yellow;
                            System.Console.WriteLine(e.Data);
                            System.Console.ResetColor();
                            output.AppendLine(e.Data);
                        }
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            System.Console.ForegroundColor = ConsoleColor.DarkRed;
                            System.Console.WriteLine(e.Data);
                            System.Console.ResetColor();
                            error.AppendLine(e.Data);
                        }
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        return new CommandResult
                        {
                            Success = true//,
                            //Response = $"\n命令执行完成，退出代码: {process.ExitCode}"
                        };
                    }
                    else
                    {
                        return new CommandResult
                        {
                            Success = false//,
                            //Response = $"\n命令执行失败，退出代码: {process.ExitCode}\n{error}"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = $"执行CMD命令时出错: {ex.Message}"
                };
            }
        }
    }
}