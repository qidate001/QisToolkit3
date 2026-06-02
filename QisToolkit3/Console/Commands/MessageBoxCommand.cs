using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QisToolkit3.Console.Commands
{
    public class MessageBoxCommand : ICommand
    {
        public string Name => "messagebox";
        public string Description => "使用MessageBox输出内容";
        public string[] Aliases => new[] { "msgbox", "mbox", "mb" };

        public CommandResult Execute(string[] args)
        {
            if (args.Length > 0)
            {
                DialogResult result;

                // 不止1个参数
                if (args.Length > 1)
                {
                    // 高级自定义
                    if (args[0] == "." && args.Length >= 4)  // 至少需要 4 个参数：. + 按钮 + 图标 + 标题
                    {
                        MessageBoxButtons btn = GetMessageBoxButtons(args[1]);
                        MessageBoxIcon icon = GetMessageBoxIcon(args[2]);
                        string title = args[3];

                        // 正文：从索引 4 开始，到结束
                        string message = args.Length > 4
                            ? string.Join("\n", args, 4, args.Length - 4)
                            : "";

                        result = MessageBox.Show(message, title, btn, icon);
                    }

                    // 第一个参数 作为标题
                    else
                    {
                        result = MessageBox.Show(string.Join('\n', args, 1, args.Length - 1), args[0]);
                    }
                }

                // 仅一个参数 作为正文
                else
                {
                    result = MessageBox.Show(string.Join('\n', args));
                }

                return CommandResult.OkWithData(result.ToString());
            }

            return CommandResult.OkWithData(MessageBox.Show("").ToString());
        }

        private MessageBoxButtons GetMessageBoxButtons(string value) => value.ToLower() switch
        {
            "ok" => MessageBoxButtons.OK,
            "okc" => MessageBoxButtons.OKCancel,
            "okcancel" => MessageBoxButtons.OKCancel,
            "yesno" => MessageBoxButtons.YesNo,
            "yn" => MessageBoxButtons.YesNo,
            "yesnocancel" => MessageBoxButtons.YesNoCancel,
            "ync" => MessageBoxButtons.YesNoCancel,
            "retrycancel" => MessageBoxButtons.RetryCancel,
            "rc" => MessageBoxButtons.RetryCancel,
            _ => MessageBoxButtons.OK
        };

        private MessageBoxIcon GetMessageBoxIcon(string value) => value.ToLower() switch
        {
            "asterisk" => MessageBoxIcon.Asterisk,
            "ast" => MessageBoxIcon.Asterisk,
            "a" => MessageBoxIcon.Asterisk,
            "error" => MessageBoxIcon.Error,
            "err" => MessageBoxIcon.Error,
            "e" => MessageBoxIcon.Error,
            "exclamation" => MessageBoxIcon.Exclamation,
            "exc" => MessageBoxIcon.Exclamation,
            "hand" => MessageBoxIcon.Hand,
            "h" => MessageBoxIcon.Hand,
            "information" => MessageBoxIcon.Information,
            "info" => MessageBoxIcon.Information,
            "i" => MessageBoxIcon.Information,
            "question" => MessageBoxIcon.Question,
            "que" => MessageBoxIcon.Question,
            "q" => MessageBoxIcon.Question,
            "stop" => MessageBoxIcon.Stop,
            "s" => MessageBoxIcon.Stop,
            "warning" => MessageBoxIcon.Warning,
            "war" => MessageBoxIcon.Warning,
            "w" => MessageBoxIcon.Warning,
            "none" => MessageBoxIcon.None,
            "null" => MessageBoxIcon.None,
            "0" => MessageBoxIcon.None,
            "_" => MessageBoxIcon.None,
            _ => MessageBoxIcon.None
        };
    }
}
