using System;

namespace QisToolkit3.Console.Commands
{
    public class TimeCommand : ICommand
    {
        public string Name => "time";
        public string Description => "显示当前时间";
        public string[] Aliases => new[] { "t" };

        public CommandResult Execute(string[] args)
        {
            string timeStr = DateTime.Now.ToString("HH:mm:ss");
            return CommandResult.Ok($"当前时间: {timeStr}", timeStr);
        }
    }
}