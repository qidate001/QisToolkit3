using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QisToolkit3.Forms
{
    public partial class YtDlpTool
    {
        /// <summary>
        /// 从 MatchFilters.txt 加载规则到内存
        /// </summary>
        public void LoadMatchFilters()
        {
            _matchFiltersRules.Clear();
            if (File.Exists(MatchFiltersPath))
            {
                try
                {
                    var lines = File.ReadAllLines(MatchFiltersPath);
                    foreach (var line in lines)
                    {
                        string trimmed = line.Trim();
                        if (!string.IsNullOrEmpty(trimmed) && !trimmed.StartsWith("#"))
                            _matchFiltersRules.Add(trimmed);
                    }
                    Log.Info($"[YtDlp工具] 加载了 {_matchFiltersRules.Count} 条匹配过滤器规则");
                }
                catch (Exception ex)
                {
                    Log.Err($"[YtDlp工具] 加载 MatchFilters.txt 失败: {ex.Message}");
                }
            }
            else
            {
                Log.Warn($"[YtDlp工具] MatchFilters.txt 不存在，规则列表为空");
            }
        }

        public static void NotifyMatchFiltersSaved()
        {
            MatchFiltersSaved?.Invoke();
        }
    }
}
