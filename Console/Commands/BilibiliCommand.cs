using QisToolkit3.Console.Services;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace QisToolkit3.Console.Commands
{
    /// <summary>
    /// 为B站命令添加判断能力
    /// </summary>
    public interface IBilibiliCommand
    {
        bool CanHandle(string input);
    }

    public class BilibiliCommand : ICommand, IBilibiliCommand
    {
        private readonly MultiLevelAliasService _aliasService;
        private readonly OpenBrowserCommand _browserCommand;  // 添加引用

        public string Name => "bilibili";
        public string Description => "打开B站视频或个人空间（支持别名）";
        public string[] Aliases => new[] { "b" };

        public BilibiliCommand()
        {
            _aliasService = new MultiLevelAliasService("bilibili");
            _browserCommand = new OpenBrowserCommand();  // 创建实例
        }

        public CommandResult Execute(string[] args)
        {
            if (args.Length == 0)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = GetUsageHelp()
                };
            }

            string target = args[0];

            // 别名管理命令
            if (target.Equals("list", StringComparison.OrdinalIgnoreCase))
            {
                return ListAliases();
            }

            if (target.Equals("reload", StringComparison.OrdinalIgnoreCase))
            {
                _aliasService.Reload();
                return new CommandResult
                {
                    Success = true,
                    Response = "别名配置已重新加载"
                };
            }

            // 解析别名
            string resolvedTarget = _aliasService.Resolve(target);

            // 构建B站URL
            string url;
            if (resolvedTarget.StartsWith("av", StringComparison.OrdinalIgnoreCase) ||
                resolvedTarget.StartsWith("BV", StringComparison.OrdinalIgnoreCase))
            {
                url = $"https://www.bilibili.com/video/{resolvedTarget}";
            }
            else if (long.TryParse(resolvedTarget, out _))
            {
                url = $"https://space.bilibili.com/{resolvedTarget}";
            }
            else
            {
                return CommandResult.Error(
                    $"无效的ID: {target}\n{GetUsageHelp()}",
                    resolvedTarget,                     // 数据0: ID
                    "False"                             // 数据1: 是否成功
                );
            }

            // 调用 OpenBrowserCommand 来打开URL
            return _browserCommand.Execute(new[] { url });
        }

        public bool CanHandle(string input)
        {
            if (long.TryParse(input, out _))
                return true;

            if (input.StartsWith("av", StringComparison.OrdinalIgnoreCase))
                return true;

            if (input.StartsWith("BV", StringComparison.OrdinalIgnoreCase))
                return true;

            if (_aliasService.IsAlias(input))
                return true;

            return false;
        }

        private CommandResult ListAliases()
        {
            var allAliases = _aliasService.GetAllAliases();
            var globalAliases = _aliasService.GetGlobalAliases();
            var commandAliases = _aliasService.GetCommandAliases();

            string response = "\n=== 别名配置 ===\n";

            if (commandAliases.Count > 0)
            {
                response += "\n[命令专用别名] (优先级高)\n";
                foreach (var kv in commandAliases)
                {
                    string desc = string.IsNullOrEmpty(kv.Value.Description) ? "" : $" ({kv.Value.Description})";
                    response += $"  {kv.Key,-10} -> {kv.Value.Value}{desc}\n";
                }
            }

            if (globalAliases.Count > 0)
            {
                response += "\n[通用别名] (优先级低)\n";
                foreach (var kv in globalAliases)
                {
                    if (commandAliases.ContainsKey(kv.Key))
                    {
                        response += $"  {kv.Key,-10} -> {kv.Value.Value} (被命令专用覆盖)\n";
                    }
                    else
                    {
                        string desc = string.IsNullOrEmpty(kv.Value.Description) ? "" : $" ({kv.Value.Description})";
                        response += $"  {kv.Key,-10} -> {kv.Value.Value}{desc}\n";
                    }
                }
            }

            if (allAliases.Count == 0)
            {
                response += "暂无已配置的别名\n";
            }

            response += $"\n使用方式:\n";
            response += $"  {Aliases[0]} <别名>          - 使用别名\n";
            response += $"  {Aliases[0]} list           - 查看所有别名\n";
            response += $"  {Aliases[0]} reload         - 重新加载配置\n";

            return new CommandResult
            {
                Success = true,
                Response = response
            };
        }

        private string GetUsageHelp()
        {
            return "用法: b <账号ID/视频ID/别名>\n" +
                   "示例: b av123456 (打开视频)\n" +
                   "      b BV1xx (打开视频)\n" +
                   "      b 697987209 (打开个人空间)\n" +
                   "      b hua (使用别名)\n" +
                   "      b list (查看所有别名)";
        }
    }
}