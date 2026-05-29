using System;
using System.IO;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using QisToolkit3.Console.Services;

namespace QisToolkit3.Console.Commands
{
    public class GetGitHubCommand : ICommand
    {
        private readonly MultiLevelAliasService _aliasService;
        private readonly WebGetCommand _webGet;

        public string Name => "getgithub";
        public string Description => "从GitHub下载Release文件";
        public string[] Aliases => new[] { "ggh" };

        public GetGitHubCommand()
        {
            _aliasService = new MultiLevelAliasService("getgithub");
            _webGet = new WebGetCommand();
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

            // 检查是否使用别名 (单参数)
            if (args.Length == 1)
            {
                string alias = args[0];
                if (_aliasService.IsAlias(alias))
                {
                    string resolved = _aliasService.Resolve(alias);
                    System.Console.ForegroundColor = ConsoleColor.DarkGray;
                    System.Console.WriteLine($"[ggh] 别名 '{alias}' -> {resolved}");
                    System.Console.ResetColor();

                    // 解析别名格式: "owner/repo/tag/filename" 或 JSON
                    return DownloadFromAlias(resolved);
                }
                else
                {
                    return new CommandResult
                    {
                        Success = false,
                        Response = $"未知别名: {alias}\n使用 'ggh list' 查看所有别名"
                    };
                }
            }

            // 完整参数: owner repo tag filename
            if (args.Length >= 4)
            {
                string owner = args[0];
                string repo = args[1];
                string tag = args[2];
                string filename = args[3];
                string savePath = args.Length > 4 ? args[4] : null;

                return DownloadFromParts(owner, repo, tag, filename, savePath);
            }

            // 别名管理命令
            if (args[0].Equals("list", StringComparison.OrdinalIgnoreCase))
            {
                return ListAliases();
            }

            if (args[0].Equals("add", StringComparison.OrdinalIgnoreCase) && args.Length >= 3)
            {
                return AddAlias(args[1], string.Join(" ", args.Skip(2)));
            }

            if (args[0].Equals("remove", StringComparison.OrdinalIgnoreCase) && args.Length >= 2)
            {
                return RemoveAlias(args[1]);
            }

            if (args[0].Equals("reload", StringComparison.OrdinalIgnoreCase))
            {
                _aliasService.Reload();
                return new CommandResult
                {
                    Success = true,
                    Response = "GitHub别名配置已重新加载"
                };
            }

            return new CommandResult
            {
                Success = false,
                Response = GetUsageHelp()
            };
        }

        private CommandResult DownloadFromAlias(string aliasValue)
        {
            // 解析别名值，支持格式: "owner/repo/tag/filename"
            string[] parts = aliasValue.Split('/');
            if (parts.Length >= 4)
            {
                string owner = parts[0];
                string repo = parts[1];
                string tag = parts[2];
                string filename = string.Join("/", parts.Skip(3));
                return DownloadFromParts(owner, repo, tag, filename, null);
            }
            else
            {
                return new CommandResult
                {
                    Success = false,
                    Response = $"别名格式错误，应为: owner/repo/tag/filename\n当前值: {aliasValue}"
                };
            }
        }

        private CommandResult DownloadFromParts(string owner, string repo, string tag, string filename, string savePath)
        {
            // 构建GitHub下载URL
            string url = $"https://github.com/{owner}/{repo}/releases/download/{tag}/{filename}";

            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine($"GitHub: {owner}/{repo}");
            System.Console.WriteLine($"版本: {tag}");
            System.Console.WriteLine($"文件: {filename}");
            System.Console.ResetColor();

            // 调用 wget 下载
            if (string.IsNullOrEmpty(savePath))
            {
                return _webGet.Execute(new[] { url });
            }
            else
            {
                return _webGet.Execute(new[] { url, savePath });
            }
        }

        private CommandResult ListAliases()
        {
            var aliases = _aliasService.GetAllAliases();

            if (aliases.Count == 0)
            {
                return new CommandResult
                {
                    Success = true,
                    Response = "暂无GitHub别名配置\n\n" +
                               "添加别名示例:\n" +
                               "  ggh add nsudo M2TeamArchived/NSudo/8.2/NSudo_8.2_All_Components.zip\n" +
                               "  ggh add ffmpeg BtbN/FFmpeg-Builds/release/ffmpeg-master-latest-win64-gpl.zip"
                };
            }

            string response = "\nGitHub别名列表:\n";
            foreach (var kv in aliases)
            {
                response += $"  {kv.Key,-15} -> {kv.Value.Value}\n";
            }
            response += "\n使用 'ggh <别名>' 下载";

            return new CommandResult
            {
                Success = true,
                Response = response
            };
        }

        private CommandResult AddAlias(string alias, string value)
        {
            // 验证格式
            string[] parts = value.Split('/');
            if (parts.Length < 4)
            {
                return new CommandResult
                {
                    Success = false,
                    Response = $"格式错误，应为: owner/repo/tag/filename\n当前值: {value}"
                };
            }

            _aliasService.AddCommandAlias(alias, value);
            return new CommandResult
            {
                Success = true,
                Response = $"已添加GitHub别名: {alias} -> {value}"
            };
        }

        private CommandResult RemoveAlias(string alias)
        {
            if (_aliasService.RemoveCommandAlias(alias))
            {
                return new CommandResult
                {
                    Success = true,
                    Response = $"已删除GitHub别名: {alias}"
                };
            }

            return new CommandResult
            {
                Success = false,
                Response = $"别名不存在: {alias}"
            };
        }

        private string GetUsageHelp()
        {
            return "用法:\n" +
                   "  ggh <别名>                    - 使用别名下载\n" +
                   "  ggh <owner> <repo> <tag> <filename> [保存路径] - 直接指定\n" +
                   "  ggh list                      - 查看所有别名\n" +
                   "  ggh add <别名> <owner/repo/tag/filename> - 添加别名\n" +
                   "  ggh remove <别名>             - 删除别名\n" +
                   "  ggh reload                    - 重新加载配置\n\n" +
                   "示例:\n" +
                   "  ggh nsudo\n" +
                   "  ggh M2TeamArchived NSudo 8.2 NSudo_8.2_All_Components.zip\n" +
                   "  ggh add nsudo M2TeamArchived/NSudo/8.2/NSudo_8.2_All_Components.zip";
        }
    }
}