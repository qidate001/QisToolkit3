using System;

namespace QisToolkit3.Console.Commands
{
    public class DateCommand : ICommand
    {
        public string Name => "date";
        public string Description => "显示当前日期";
        public string[] Aliases => new[] { "d" };

        public CommandResult Execute(string[] args)
        {
            string dateStr = DateTime.Now.ToString("yyyy-MM-dd");
            return CommandResult.Ok($"当前日期: {dateStr}", dateStr);
        }
    }
}