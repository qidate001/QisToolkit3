using QisToolkit3.Console;
using System.Diagnostics;
using System.Security.Policy;

public class CmdCommand : ICommand
{
    public string Name => "cmd";
    public string Description => "直接执行CMD命令";
    public string[] Aliases => new[] { "!" };

    public CommandResult Execute(string[] args)
    {
        if (args.Length == 0)
        {
            return new CommandResult
            {
                Success = false,
                Response = "用法: cmd <命令>"
            };
        }

        string command = string.Join(" ", args);

        //Log.Info($"[命令行模式] [CMD] 命令：{command}");

        try
        {
            //System.Console.ForegroundColor = ConsoleColor.DarkGray;
            //System.Console.WriteLine($"[CMD] {command}");
            //System.Console.ResetColor();

            // 使用 System.Diagnostics.Process 但让输出继承
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = System.Text.Encoding.GetEncoding("GB2312"),
                    StandardErrorEncoding = System.Text.Encoding.GetEncoding("GB2312")
                };

                // 同步读取并实时输出
                process.Start();

                // 实时输出标准输出
                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    if (line != null)
                    {
                        Console.WriteLine(line);
                    }
                }

                // 实时输出错误输出
                while (!process.StandardError.EndOfStream)
                {
                    string line = process.StandardError.ReadLine();
                    if (line != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(line);
                        Console.ResetColor();
                    }
                }

                process.WaitForExit();
            }

            return CommandResult.OkWithData(
                command,                            // 数据0: 命令
                "True"                              // 数据1: 是否成功
            );
        }
        catch (Exception ex)
        {
            return CommandResult.Error(
                $"执行命令失败: {ex.Message}",
                command,                            // 数据0: 命令
                "False",                            // 数据1: 是否成功
                ex.Message                          // 数据2: 错误消息
            );
        }
    }
}