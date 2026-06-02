using QisToolkit3.Console;

public class EchoCommand : ICommand
{
    public string Name => "echo";
    public string Description => "回显输入内容";
    public string[] Aliases => new[] { "print" };

    public CommandResult Execute(string[] args)
    {
        string message = string.Join(" ", args);

        // 注意：& 已经在 CommandHandler 中被替换了
        // 所以这里直接输出即可
        if (string.IsNullOrEmpty(message))
        {
            return CommandResult.Ok("");
        }

        return CommandResult.Ok(message);
    }
}