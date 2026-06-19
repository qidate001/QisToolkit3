using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace QisToolkit3
{
    public class RuleEngine
    {
        // ---------- 公共入口 ----------
        public string ProcessText(string input, string ruleString)
        {
            //Log.Info($"[RuleEngine] 输入：{input}");
            //Log.Info($"[RuleEngine] 规则：{ruleString}");
            if (string.IsNullOrEmpty(ruleString)) return input;
            var rules = ParseRules(ruleString);
            string current = input;
            foreach (var rule in rules)
                current = ApplyRule(current, rule);
            //Log.Info($"[RuleEngine] 输出：{current}");
            return current;
        }

        // ---------- 内部数据结构 ----------
        private class Rule
        {
            public enum OperationType
            {
                Replace, ToUpper, ToLower, ToChineseNum, ToChineseNumOral,
                FromChineseNumOral, InsertRight, InsertLeft, Swap
            }
            public OperationType Op { get; set; }
            public string LeftPattern { get; set; }    // 匹配模式（正则或普通字符串）
            public bool IsRegex { get; set; }
            public string RightOperand { get; set; }   // 替换/插入/交换的右操作数
            public List<Condition> Conditions { get; set; } = new List<Condition>();
        }

        private class Condition
        {
            public enum CondType { PrefixText, LineHead }
            public CondType Type { get; set; }
            public string Text { get; set; }           // 前缀文本或正则
            public bool IsRegex { get; set; }
        }

        // ---------- 规则解析 ----------
        private List<Rule> ParseRules(string ruleString)
        {
            var rules = new List<Rule>();
            var ruleParts = ruleString.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in ruleParts)
            {
                var trimmed = part.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;

                var plusParts = trimmed.Split('+');
                string basePart = plusParts[0].Trim();
                var conditions = new List<Condition>();

                for (int i = 1; i < plusParts.Length; i++)
                {
                    var cond = plusParts[i].Trim();
                    if (cond == "LH" || cond == "Line-Head-Text")
                        conditions.Add(new Condition { Type = Condition.CondType.LineHead });
                    else if (cond.StartsWith("P=", StringComparison.OrdinalIgnoreCase) ||
                             cond.StartsWith("Prefix-Text=", StringComparison.OrdinalIgnoreCase))
                    {
                        string value;
                        if (cond.StartsWith("P=", StringComparison.OrdinalIgnoreCase))
                            value = cond.Substring(2).Trim();
                        else
                            value = cond.Substring("Prefix-Text=".Length).Trim();

                        bool isRegex = false;
                        if (value.StartsWith("<") && value.EndsWith(">"))
                        {
                            isRegex = true;
                            value = value.Substring(1, value.Length - 2);
                        }
                        conditions.Add(new Condition { Type = Condition.CondType.PrefixText, Text = value, IsRegex = isRegex });
                    }
                    else
                        throw new ArgumentException($"未知条件: {cond}");
                }

                var rule = new Rule { Conditions = conditions };

                // 按优先级识别基础规则
                if (basePart.Contains("→→"))
                {
                    var parts = basePart.Split(new[] { "→→" }, StringSplitOptions.None);
                    if (parts.Length != 2) throw new ArgumentException("向右插入格式错误");
                    rule.Op = Rule.OperationType.InsertRight;
                    ParseLeftRight(parts[0].Trim(), parts[1].Trim(), rule);
                }
                else if (basePart.Contains("←←"))
                {
                    var parts = basePart.Split(new[] { "←←" }, StringSplitOptions.None);
                    if (parts.Length != 2) throw new ArgumentException("向左插入格式错误");
                    rule.Op = Rule.OperationType.InsertLeft;
                    ParseLeftRight(parts[0].Trim(), parts[1].Trim(), rule);
                }
                else if (basePart.Contains("→←"))
                {
                    var parts = basePart.Split(new[] { "→←" }, StringSplitOptions.None);
                    if (parts.Length != 2) throw new ArgumentException("双向转换格式错误");
                    rule.Op = Rule.OperationType.Swap;
                    rule.LeftPattern = parts[0].Trim();
                    rule.IsRegex = false;
                    rule.RightOperand = parts[1].Trim();
                }
                else if (basePart.Contains("↑CN2"))
                {
                    int idx = basePart.IndexOf("↑CN2");
                    string left = basePart.Substring(0, idx).Trim();
                    if (string.IsNullOrEmpty(left)) throw new ArgumentException("↑CN2 缺少左操作数");
                    rule.Op = Rule.OperationType.ToChineseNumOral;
                    ParseLeftPattern(left, rule);
                    // 自动扩展正则量词，使口语转换作用于整个数字串
                    AutoExtendRegexForOral(rule);
                }
                else if (basePart.Contains("↑CN"))
                {
                    int idx = basePart.IndexOf("↑CN");
                    string left = basePart.Substring(0, idx).Trim();
                    if (string.IsNullOrEmpty(left)) throw new ArgumentException("↑CN 缺少左操作数");
                    rule.Op = Rule.OperationType.ToChineseNum;
                    ParseLeftPattern(left, rule);
                }
                else if (basePart.Contains("↓CN2"))
                {
                    int idx = basePart.IndexOf("↓CN2");
                    string left = basePart.Substring(0, idx).Trim();
                    if (string.IsNullOrEmpty(left))
                    {
                        // 无左操作数 → 匹配整个文本
                        rule.Op = Rule.OperationType.FromChineseNumOral;
                        rule.LeftPattern = ".*";
                        rule.IsRegex = true;
                    }
                    else
                    {
                        rule.Op = Rule.OperationType.FromChineseNumOral;
                        ParseLeftPattern(left, rule);
                        AutoExtendRegexForOral(rule);
                    }
                }
                else if (basePart.Contains("↑"))
                {
                    int idx = basePart.IndexOf("↑");
                    string left = basePart.Substring(0, idx).Trim();
                    if (string.IsNullOrEmpty(left)) throw new ArgumentException("↑ 缺少左操作数");
                    rule.Op = Rule.OperationType.ToUpper;
                    ParseLeftPattern(left, rule);
                }
                else if (basePart.Contains("↓"))
                {
                    int idx = basePart.IndexOf("↓");
                    string left = basePart.Substring(0, idx).Trim();
                    if (string.IsNullOrEmpty(left)) throw new ArgumentException("↓ 缺少左操作数");
                    rule.Op = Rule.OperationType.ToLower;
                    ParseLeftPattern(left, rule);
                }
                else if (basePart.Contains("→"))
                {
                    var parts = basePart.Split(new[] { "→" }, StringSplitOptions.None);
                    if (parts.Length != 2) throw new ArgumentException("替换格式错误");
                    rule.Op = Rule.OperationType.Replace;
                    ParseLeftRight(parts[0].Trim(), parts[1].Trim(), rule);
                }
                else
                    throw new ArgumentException($"无法识别的基础规则: {basePart}");

                rules.Add(rule);
            }
            return rules;
        }

        private void ParseLeftPattern(string left, Rule rule)
        {
            if (left.StartsWith("<") && left.EndsWith(">"))
            {
                rule.IsRegex = true;
                rule.LeftPattern = left.Substring(1, left.Length - 2);
            }
            else
            {
                rule.IsRegex = false;
                rule.LeftPattern = left;
            }
        }

        private void ParseLeftRight(string left, string right, Rule rule)
        {
            ParseLeftPattern(left, rule);
            rule.RightOperand = right;
        }

        /// <summary> 对口语数字转换的正则自动添加 + 量词（若缺失）以匹配连续数字串 </summary>
        private void AutoExtendRegexForOral(Rule rule)
        {
            if (!rule.IsRegex) return;
            string pattern = rule.LeftPattern;
            // 检查是否为简单的数字匹配模式且不含量词
            // 匹配常见数字字符类：\d, [0-9], [0-9a-fA-F] 等，但我们只关心纯数字
            // 简单判断：模式为 \d 或 [0-9] 或 [0-9] 带范围，且末尾不是量词
            if (pattern == @"\d" || pattern == "[0-9]" || pattern == "[0-9]")
            {
                rule.LeftPattern = pattern + "+";
                return;
            }
            // 更通用的检测：如果模式不包含 *, +, ?, {，则添加 +
            if (!Regex.IsMatch(pattern, @"[\*\+\?\{\}]"))
            {
                rule.LeftPattern = pattern + "+";
            }
            // 否则保持原样（用户已明确量词）
        }

        // ---------- 规则应用 ----------
        private string ApplyRule(string input, Rule rule)
        {
            switch (rule.Op)
            {
                case Rule.OperationType.Replace:
                    return ApplyReplace(input, rule);
                case Rule.OperationType.ToUpper:
                    return ApplyTransform(input, rule, s => s.ToUpperInvariant());
                case Rule.OperationType.ToLower:
                    return ApplyTransform(input, rule, s => s.ToLowerInvariant());
                case Rule.OperationType.ToChineseNum:
                    return ApplyTransform(input, rule, s => ToChineseNum(s));
                case Rule.OperationType.ToChineseNumOral:
                    return ApplyTransform(input, rule, s => ToChineseNumOral(s));
                case Rule.OperationType.FromChineseNumOral:
                    return ApplyTransform(input, rule, s => FromChineseNumOral(s));
                case Rule.OperationType.InsertRight:
                    return ApplyInsert(input, rule, true);
                case Rule.OperationType.InsertLeft:
                    return ApplyInsert(input, rule, false);
                case Rule.OperationType.Swap:
                    return ApplySwap(input, rule);
                default:
                    return input;
            }
        }

        private string ApplyTransform(string input, Rule rule, Func<string, string> transform)
        {
            return ApplyReplacement(input, rule, match => transform(match.Value));
        }

        private string ApplyReplace(string input, Rule rule)
        {
            return ApplyReplacement(input, rule, match => rule.RightOperand);
        }

        private string ApplyInsert(string input, Rule rule, bool insertAfter)
        {
            return ApplyReplacement(input, rule,
                match => insertAfter ? match.Value + rule.RightOperand : rule.RightOperand + match.Value);
        }

        private string ApplySwap(string input, Rule rule)
        {
            string a = Regex.Escape(rule.LeftPattern);
            string b = Regex.Escape(rule.RightOperand);
            string pattern = $"({a}|{b})";
            return ApplyReplacement(input, rule, match =>
            {
                if (match.Value == rule.LeftPattern)
                    return rule.RightOperand;
                if (match.Value == rule.RightOperand)
                    return rule.LeftPattern;
                return match.Value;
            }, useRegex: true, pattern: pattern);
        }

        private string ApplyReplacement(string input, Rule rule, Func<Match, string> replacement,
                                        bool useRegex = false, string pattern = null)
        {
            string pat;
            if (useRegex)
                pat = pattern;
            else if (rule.IsRegex)
                pat = rule.LeftPattern;
            else
                pat = Regex.Escape(rule.LeftPattern);

            var regex = new Regex(pat, RegexOptions.Compiled);
            var matches = regex.Matches(input);
            var sb = new StringBuilder(input);

            // 从后往前替换，避免索引偏移
            for (int i = matches.Count - 1; i >= 0; i--)
            {
                var match = matches[i];
                if (CheckConditions(input, match, rule.Conditions))
                {
                    string replace = replacement(match);
                    sb.Remove(match.Index, match.Length);
                    sb.Insert(match.Index, replace);
                }
            }
            return sb.ToString();
        }

        private bool CheckConditions(string input, Match match, List<Condition> conditions)
        {
            foreach (var cond in conditions)
            {
                if (cond.Type == Condition.CondType.LineHead)
                {
                    if (match.Index != 0 &&
                        !(match.Index > 0 && (input[match.Index - 1] == '\n' || input[match.Index - 1] == '\r')))
                        return false;
                }
                else if (cond.Type == Condition.CondType.PrefixText)
                {
                    string prefix = input.Substring(0, match.Index);
                    if (cond.IsRegex)
                    {
                        var regex = new Regex(cond.Text, RegexOptions.Compiled);
                        var prefixMatch = regex.Match(prefix);
                        if (!prefixMatch.Success || prefixMatch.Index + prefixMatch.Length != prefix.Length)
                            return false;
                    }
                    else
                    {
                        if (!prefix.EndsWith(cond.Text, StringComparison.Ordinal))
                            return false;
                    }
                }
            }
            return true;
        }

        // ==================== 中文数字转换（完整实现） ====================

        /// <summary> 逐字转中文数字（0-9 → 零~九）</summary>
        private static string ToChineseNum(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            var map = new char[] { '零', '一', '二', '三', '四', '五', '六', '七', '八', '九' };
            var sb = new StringBuilder(s.Length);
            foreach (char c in s)
                sb.Append(char.IsDigit(c) ? map[c - '0'] : c);
            return sb.ToString();
        }

        /// <summary> 口语中文数字（将数字串转为中文口语读法）</summary>
        private static string ToChineseNumOral(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            if (!long.TryParse(s, out long num)) return s;

            if (num == 0) return "零";

            string[] digitChars = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            string[] smallUnits = { "", "十", "百", "千" };
            string[] bigUnits = { "", "万", "亿", "兆", "京" };

            var groups = new List<int>();
            long n = num;
            while (n > 0)
            {
                groups.Add((int)(n % 10000));
                n /= 10000;
            }

            var result = new StringBuilder();
            bool needZero = false;

            for (int i = groups.Count - 1; i >= 0; i--)
            {
                int val = groups[i];
                if (val == 0)
                {
                    if (i > 0) needZero = true;
                    continue;
                }

                string part = BuildGroup(val, digitChars, smallUnits, i == groups.Count - 1);
                if (needZero)
                {
                    result.Append('零');
                    needZero = false;
                }
                result.Append(part);
                result.Append(bigUnits[i]);
            }

            return result.ToString();
        }

        private static string BuildGroup(int val, string[] digits, string[] smallUnits, bool isHighest)
        {
            if (val == 0) return "";

            int qian = val / 1000;
            int bai = (val % 1000) / 100;
            int shi = (val % 100) / 10;
            int ge = val % 10;

            var sb = new StringBuilder();

            // 千位
            if (qian > 0)
            {
                sb.Append(qian == 2 ? "两" : digits[qian]);
                sb.Append(smallUnits[3]);
            }

            // 百位
            if (bai > 0)
            {
                sb.Append(digits[bai]);
                sb.Append(smallUnits[2]);
            }
            else if (qian > 0 && (shi > 0 || ge > 0))
            {
                sb.Append('零');
            }

            // 十位
            if (shi > 0)
            {
                bool isFirst = isHighest && qian == 0 && bai == 0 && shi == 1 && ge == 0;
                if (isFirst)
                    sb.Append('十');
                else if (shi == 1 && qian == 0 && bai == 0 && isHighest)
                    sb.Append('十');
                else
                {
                    sb.Append(digits[shi]);
                    sb.Append(smallUnits[1]);
                }
            }
            else if ((qian > 0 || bai > 0) && ge > 0)
            {
                sb.Append('零');
            }

            // 个位
            if (ge > 0)
                sb.Append(digits[ge]);

            return sb.ToString();
        }

        /// <summary> 逆向口语中文数字（将中文口语读法转为阿拉伯数字字符串）</summary>
        /// <summary>
        /// 逆向口语中文数字：将中文数字串转为阿拉伯数字字符串
        /// 支持：零一二三四五六七八九十百千万亿兆京，以及“两”
        /// </summary>
        private static string FromChineseNumOral(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;

            var digitMap = new Dictionary<char, long>
    {
        {'零', 0}, {'一', 1}, {'二', 2}, {'两', 2}, {'三', 3}, {'四', 4},
        {'五', 5}, {'六', 6}, {'七', 7}, {'八', 8}, {'九', 9}
    };

            var unitMap = new Dictionary<char, long>
    {
        {'十', 10}, {'百', 100}, {'千', 1000},
        {'万', 10000}, {'亿', 100000000},
        {'兆', 1000000000000}, {'京', 10000000000000000}
    };

            long result = 0;      // 最终结果
            long section = 0;     // 当前小节值（万以下）
            long temp = 0;        // 当前暂存的数字（用于与单位结合）

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (digitMap.TryGetValue(c, out long digit))
                {
                    temp = digit;
                }
                else if (unitMap.TryGetValue(c, out long unit))
                {
                    if (unit < 10000) // 小单位（十、百、千）
                    {
                        if (temp == 0) temp = 1; // 处理“十”单独出现等
                        section += temp * unit;
                        temp = 0;
                    }
                    else // 大单位（万、亿、兆、京）
                    {
                        if (temp == 0 && section == 0) section = 1; // 如“万”单独出现
                        result += (section + temp) * unit;
                        section = 0;
                        temp = 0;
                    }
                }
                // 遇到 '零' 忽略，其他字符也忽略
            }

            // 处理剩余未结算部分
            result += section + temp;
            return result.ToString();
        }
    }
}