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
            if (string.IsNullOrEmpty(ruleString)) return input;
            var rules = ParseRules(ruleString);
            string current = input;
            foreach (var rule in rules)
                current = ApplyRule(current, rule);
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
            public enum CondType { PrefixText, LineHead, SuffixText } // 新增 SuffixText
            public CondType Type { get; set; }
            public string Text { get; set; }           // 前缀/后缀文本或正则
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
                    // ----- 新增后缀文本条件解析 -----
                    else if (cond.StartsWith("S=", StringComparison.OrdinalIgnoreCase) ||
                             cond.StartsWith("Suffix-Text=", StringComparison.OrdinalIgnoreCase))
                    {
                        string value;
                        if (cond.StartsWith("S=", StringComparison.OrdinalIgnoreCase))
                            value = cond.Substring(2).Trim();
                        else
                            value = cond.Substring("Suffix-Text=".Length).Trim();

                        bool isRegex = false;
                        if (value.StartsWith("<") && value.EndsWith(">"))
                        {
                            isRegex = true;
                            value = value.Substring(1, value.Length - 2);
                        }
                        conditions.Add(new Condition { Type = Condition.CondType.SuffixText, Text = value, IsRegex = isRegex });
                    }
                    else
                        throw new ArgumentException($"未知条件: {cond}");
                }

                var rule = new Rule { Conditions = conditions };

                // 按优先级识别基础规则（不变）
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

        private void AutoExtendRegexForOral(Rule rule)
        {
            if (!rule.IsRegex) return;
            string pattern = rule.LeftPattern;
            if (pattern == @"\d" || pattern == "[0-9]" || pattern == "[0-9]")
            {
                rule.LeftPattern = pattern + "+";
                return;
            }
            if (!Regex.IsMatch(pattern, @"[\*\+\?\{\}]"))
            {
                rule.LeftPattern = pattern + "+";
            }
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
                // ----- 新增后缀文本条件检查 -----
                else if (cond.Type == Condition.CondType.SuffixText)
                {
                    string suffix = input.Substring(match.Index + match.Length);
                    if (cond.IsRegex)
                    {
                        var regex = new Regex(cond.Text, RegexOptions.Compiled);
                        var suffixMatch = regex.Match(suffix);
                        // 要求匹配从后缀开头开始（即紧接匹配之后）
                        if (!suffixMatch.Success || suffixMatch.Index != 0)
                            return false;
                    }
                    else
                    {
                        if (!suffix.StartsWith(cond.Text, StringComparison.Ordinal))
                            return false;
                    }
                }
            }
            return true;
        }

        // ==================== 中文数字转换（完整实现，保持不变） ====================
        private static string ToChineseNum(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            var map = new char[] { '零', '一', '二', '三', '四', '五', '六', '七', '八', '九' };
            var sb = new StringBuilder(s.Length);
            foreach (char c in s)
                sb.Append(char.IsDigit(c) ? map[c - '0'] : c);
            return sb.ToString();
        }

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

            if (qian > 0)
            {
                sb.Append(qian == 2 ? "两" : digits[qian]);
                sb.Append(smallUnits[3]);
            }

            if (bai > 0)
            {
                sb.Append(digits[bai]);
                sb.Append(smallUnits[2]);
            }
            else if (qian > 0 && (shi > 0 || ge > 0))
            {
                sb.Append('零');
            }

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

            if (ge > 0)
                sb.Append(digits[ge]);

            return sb.ToString();
        }

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

            long result = 0;
            long section = 0;
            long temp = 0;

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (digitMap.TryGetValue(c, out long digit))
                {
                    temp = digit;
                }
                else if (unitMap.TryGetValue(c, out long unit))
                {
                    if (unit < 10000)
                    {
                        if (temp == 0) temp = 1;
                        section += temp * unit;
                        temp = 0;
                    }
                    else
                    {
                        if (temp == 0 && section == 0) section = 1;
                        result += (section + temp) * unit;
                        section = 0;
                        temp = 0;
                    }
                }
            }

            result += section + temp;
            return result.ToString();
        }
    }
}