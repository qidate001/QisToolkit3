using System;
using System.Text;

namespace QisToolkit3.Console.Commands
{
    /// <summary>
    /// 帮助命令
    /// </summary>
    public class HelpCommand : ICommand
    {
        private readonly CommandParser _parser;

        public HelpCommand(CommandParser parser)
        {
            _parser = parser;
        }

        public string Name => "help";
        public string Description => "显示帮助信息";
        public string[] Aliases => new[] { "?" };

        public CommandResult Execute(string[] args)
        {
            var sb = new StringBuilder();
            sb.AppendLine("\n可用命令:");

            foreach (var cmd in _parser.GetAllCommands())
            {
                string aliases = cmd.Aliases.Length > 0
                    ? $" (别名: {string.Join(", ", cmd.Aliases)})"
                    : "";
                sb.AppendLine($"  {cmd.Name,-10} - {cmd.Description}{aliases}");
            }

            return new CommandResult
            {
                Success = true,
                Response = sb.ToString()
            };
        }
    }
}