using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TextProcessor
{
    public class RuleEngine
    {
        public string ProcessText(string input, string ruleString)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(ruleString))
                return input;

            var rules = ruleString.Split(';');
            var result = input;

            foreach (var rule in rules)
            {
                if (string.IsNullOrWhiteSpace(rule))
                    continue;

                result = ApplyRule(result, rule.Trim());
            }

            return result;
        }

        // 应用规则
        private string ApplyRule(string input, string rule)
        {
            var parts = rule.Split('+');
            var baseRule = parts[0].Trim();
            var conditions = new List<Condition>();

            // 解析附加条件
            for (int i = 1; i < parts.Length; i++)
            {
                var condition = ParseCondition(parts[i].Trim());
                if (condition != null)
                    conditions.Add(condition);
            }

            // 解析基础规则
            if (baseRule.Contains("→→"))
            {
                var insertParts = baseRule.Split(["→→"], StringSplitOptions.None);
                if (insertParts.Length == 2)
                {
                    return ApplyRightInsertRule(input, insertParts[0], insertParts[1], conditions);
                }
            }
            else if (baseRule.Contains("←←"))
            {
                var insertParts = baseRule.Split(["←←"], StringSplitOptions.None);
                if (insertParts.Length == 2)
                {
                    return ApplyLeftInsertRule(input, insertParts[0], insertParts[1], conditions);
                }
            }
            else if (baseRule.Contains("→←"))
            {
                var swapParts = baseRule.Split(["→←"], StringSplitOptions.None);
                if (swapParts.Length == 2)
                {
                    return ApplySwapRule(input, swapParts[0], swapParts[1], conditions);
                }
            }
            else if (baseRule.Contains("↑→")) // 新增凯撒密码向右
            {
                var caesarParts = baseRule.Split(new[] { "↑→" }, StringSplitOptions.None);
                if (caesarParts.Length == 2 && int.TryParse(caesarParts[1], out int shift))
                {
                    return ApplyCaesarRule(input, caesarParts[0], shift, CaesarDirection.Right, conditions);
                }
            }
            else if (baseRule.Contains("↑←")) // 新增凯撒密码向左
            {
                var caesarParts = baseRule.Split(new[] { "↑←" }, StringSplitOptions.None);
                if (caesarParts.Length == 2 && int.TryParse(caesarParts[1], out int shift))
                {
                    return ApplyCaesarRule(input, caesarParts[0], -shift, CaesarDirection.Left, conditions);
                }
            }
            else if (baseRule.EndsWith("↑↓"))
            {
                var target = baseRule.Substring(0, baseRule.Length - 2);
                return ApplyCaseInversionRule(input, target, conditions);
            }
            else if (baseRule.EndsWith("↓↑"))
            {
                var target = baseRule.Substring(0, baseRule.Length - 2);
                return ApplyStringReverseRule(input, target, conditions);
            }
            else if (baseRule.Contains("→"))
            {
                var replaceParts = baseRule.Split(["→"], StringSplitOptions.None);
                if (replaceParts.Length == 2)
                {
                    return ApplyReplaceRule(input, replaceParts[0], replaceParts[1], conditions);
                }
            }
            else if (baseRule.EndsWith("↑"))
            {
                var target = baseRule.Substring(0, baseRule.Length - 1);
                return ApplyCaseRule(input, target, CaseOperation.ToUpper, conditions);
            }
            else if (baseRule.EndsWith("↓"))
            {
                var target = baseRule.Substring(0, baseRule.Length - 1);
                return ApplyCaseRule(input, target, CaseOperation.ToLower, conditions);
            }

            return input;
        }

        private Condition ParseCondition(string conditionStr)
        {
            // 前缀文本
            if (conditionStr.StartsWith("P="))
            {
                return new Condition
                {
                    Type = ConditionType.Prefix,
                    Value = conditionStr.Substring(2)
                };
            }

            // 前缀文本
            else if (conditionStr.StartsWith("Prefix-Text="))
            {
                return new Condition
                {
                    Type = ConditionType.Prefix,
                    Value = conditionStr.Substring(12)
                };
            }

            // 行首文本
            else if (conditionStr.Equals("LH", StringComparison.OrdinalIgnoreCase) ||
                     conditionStr.Equals("Line-Head-Text", StringComparison.OrdinalIgnoreCase))
            {
                return new Condition
                {
                    Type = ConditionType.LineHead,
                    Value = null
                };
            }

            // 计数器跳过 - 简化格式
            else if (conditionStr.StartsWith("CS="))
            {
                return ParseCounterSkipCondition(conditionStr.Substring(3));
            }

            // 计数器跳过 - 完整格式
            else if (conditionStr.StartsWith("Counter-Skipping="))
            {
                return ParseCounterSkipCondition(conditionStr.Substring(17));
            }

            return null;
        }

        private Condition ParseCounterSkipCondition(string patternStr)
        {
            var condition = new Condition { Type = ConditionType.CounterSkipping };
            var patterns = patternStr.Split('&');

            foreach (var pattern in patterns)
            {
                if (pattern.Contains(':'))
                {
                    // 格式: 执行次数:跳过次数
                    var parts = pattern.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int execute) &&
                        int.TryParse(parts[1], out int skip))
                    {
                        condition.SkipPatterns.Add(new CounterSkipPattern
                        {
                            ExecuteCount = execute,
                            SkipCount = skip,
                            IsSinglePattern = false
                        });
                    }
                }
                else
                {
                    // 格式: 跳过次数 (单次执行)
                    if (int.TryParse(pattern, out int skip))
                    {
                        condition.SkipPatterns.Add(new CounterSkipPattern
                        {
                            ExecuteCount = 1,
                            SkipCount = skip,
                            IsSinglePattern = true
                        });
                    }
                }
            }

            return condition.SkipPatterns.Count > 0 ? condition : null;
        }


        #region 基础规则实现

        // 应用替换规则
        private string ApplyReplaceRule(string input, string search, string replace, List<Condition> conditions)
        {
            bool isRegex = IsRegexPattern(search);
            string pattern = isRegex ? GetRegexPattern(search) : search;

            if (!isRegex && conditions.Count == 0)
            {
                return input.Replace(pattern, replace);
            }

            if (isRegex)
            {
                return ApplyRegexReplaceRule(input, pattern, replace, conditions);
            }
            else
            {
                return ApplyStringReplaceRule(input, pattern, replace, conditions);
            }
        }

        // 应用大小写规则
        private string ApplyCaseRule(string input, string target, CaseOperation operation, List<Condition> conditions)
        {
            bool isRegex = IsRegexPattern(target);
            string pattern = isRegex ? GetRegexPattern(target) : target;

            if (isRegex)
            {
                return ApplyRegexCaseRule(input, pattern, operation, conditions);
            }
            else
            {
                return ApplyStringCaseRule(input, pattern, operation, conditions);
            }
        }

        // 应用向右插入规则
        private string ApplyRightInsertRule(string input, string search, string insertText, List<Condition> conditions)
        {
            bool isRegex = IsRegexPattern(search);
            string pattern = isRegex ? GetRegexPattern(search) : search;

            if (isRegex)
            {
                return ApplyRegexRightInsertRule(input, pattern, insertText, conditions);
            }
            else
            {
                return ApplyStringRightInsertRule(input, pattern, insertText, conditions);
            }
        }

        // 应用向左插入规则
        private string ApplyLeftInsertRule(string input, string search, string insertText, List<Condition> conditions)
        {
            bool isRegex = IsRegexPattern(search);
            string pattern = isRegex ? GetRegexPattern(search) : search;

            if (isRegex)
            {
                return ApplyRegexLeftInsertRule(input, pattern, insertText, conditions);
            }
            else
            {
                return ApplyStringLeftInsertRule(input, pattern, insertText, conditions);
            }
        }

        // 应用双向转换规则
        private string ApplySwapRule(string input, string textA, string textB, List<Condition> conditions)
        {
            // 双向转换不支持正则表达式
            if (IsRegexPattern(textA) || IsRegexPattern(textB))
            {
                return input;
            }

            var counterSkipConditions = conditions.Where(c => c.Type == ConditionType.CounterSkipping).ToList();
            var otherConditions = conditions.Where(c => c.Type != ConditionType.CounterSkipping).ToList();

            // 双向转换的特殊处理：先找到所有匹配位置，然后应用计数器跳过
            var matchesA = FindAllMatches(input, textA, otherConditions);
            var matchesB = FindAllMatches(input, textB, otherConditions);

            if (counterSkipConditions.Count > 0)
            {
                // 应用计数器跳过逻辑
                matchesA = ApplyCounterSkipToMatches(matchesA, counterSkipConditions);
                matchesB = ApplyCounterSkipToMatches(matchesB, counterSkipConditions);
            }

            // 使用临时占位符进行替换
            string tempPlaceholder = Guid.NewGuid().ToString();

            // 先替换A为临时占位符（只替换需要执行的）
            string step1 = ReplaceSelectedMatches(input, textA, tempPlaceholder, matchesA.Where(m => m.ShouldExecute).Select(m => m.Position).ToList());

            // 再替换B为A（只替换需要执行的）
            string step2 = ReplaceSelectedMatches(step1, textB, textA, matchesB.Where(m => m.ShouldExecute).Select(m => m.Position).ToList());

            // 最后将临时占位符替换为B（只替换需要执行的）
            string step3 = ReplaceSelectedMatches(step2, tempPlaceholder, textB, matchesA.Where(m => m.ShouldExecute).Select(m => m.Position).ToList());

            return step3;
        }

        // 查找所有匹配
        private List<MatchInfo> FindAllMatches(string input, string search, List<Condition> conditions)
        {
            var matches = new List<MatchInfo>();
            int currentIndex = 0;

            while (currentIndex <= input.Length - search.Length)
            {
                int foundIndex = input.IndexOf(search, currentIndex, StringComparison.Ordinal);
                if (foundIndex == -1) break;

                if (CheckAllConditions(input, foundIndex, search.Length, conditions))
                {
                    matches.Add(new MatchInfo { Position = foundIndex, ShouldExecute = true });
                }
                currentIndex = foundIndex + 1;
            }

            return matches;
        }

        // 应用计数器跳过到匹配
        private List<MatchInfo> ApplyCounterSkipToMatches(List<MatchInfo> matches, List<Condition> counterSkipConditions)
        {
            if (matches.Count == 0) return matches;

            var executionResults = new List<bool[]>();
            foreach (var condition in counterSkipConditions)
            {
                bool[] result = CalculateCounterSkipResult(matches.Count, condition.SkipPatterns);
                executionResults.Add(result);
            }

            bool[] finalExecution = MergeCounterSkipResults(executionResults, matches.Count);

            for (int i = 0; i < matches.Count; i++)
            {
                matches[i].ShouldExecute = finalExecution[i];
            }

            return matches;
        }

        // 替换所选匹配项
        private string ReplaceSelectedMatches(string input, string search, string replace, List<int> positions)
        {
            if (positions.Count == 0) return input;

            var result = new StringBuilder();
            int lastIndex = 0;
            var sortedPositions = positions.OrderBy(p => p).ToList();

            foreach (int pos in sortedPositions)
            {
                result.Append(input, lastIndex, pos - lastIndex);
                result.Append(replace);
                lastIndex = pos + search.Length;
            }

            result.Append(input, lastIndex, input.Length - lastIndex);
            return result.ToString();
        }

        // 应用凯撒密码规则
        private string ApplyCaesarRule(string input, string target, int shift, CaesarDirection direction, List<Condition> conditions)
        {
            bool isRegex = IsRegexPattern(target);
            string pattern = isRegex ? GetRegexPattern(target) : target;

            if (isRegex)
            {
                return ApplyRegexCaesarRule(input, pattern, shift, conditions);
            }
            else
            {
                return ApplyStringCaesarRule(input, pattern, shift, conditions);
            }
        }

        // 应用凯撒密码
        private string ApplyCaesarCipher(string text, int shift)
        {
            var result = new StringBuilder();

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char baseChar = char.IsUpper(c) ? 'A' : 'a';
                    int offset = (c - baseChar + shift) % 26;
                    if (offset < 0) offset += 26; // 处理负数
                    result.Append((char)(baseChar + offset));
                }
                else
                {
                    result.Append(c); // 非字母字符保持不变
                }
            }

            return result.ToString();
        }

        // 应用反转大小写规则
        private string ApplyCaseInversionRule(string input, string target, List<Condition> conditions)
        {
            bool isRegex = IsRegexPattern(target);
            string pattern = isRegex ? GetRegexPattern(target) : target;

            if (isRegex)
            {
                return ApplyRegexCaseInversionRule(input, pattern, conditions);
            }
            else
            {
                return ApplyStringCaseInversionRule(input, pattern, conditions);
            }
        }

        // 应用反转字符串规则
        private string ApplyStringReverseRule(string input, string target, List<Condition> conditions)
        {
            bool isRegex = IsRegexPattern(target);
            string pattern = isRegex ? GetRegexPattern(target) : target;

            if (isRegex)
            {
                return ApplyRegexStringReverseRule(input, pattern, conditions);
            }
            else
            {
                return ApplyStringStringReverseRule(input, pattern, conditions);
            }
        }

        #endregion

        #region 字符串操作实现

        // 应用字符串替换规则
        private string ApplyStringReplaceRule(string input, string search, string replace, List<Condition> conditions)
        {
            var counterSkipConditions = conditions.Where(c => c.Type == ConditionType.CounterSkipping).ToList();
            var otherConditions = conditions.Where(c => c.Type != ConditionType.CounterSkipping).ToList();

            if (counterSkipConditions.Count == 0)
            {
                return ApplyStringReplaceWithoutCounterSkip(input, search, replace, otherConditions);
            }

            return ApplyStringReplaceWithCounterSkip(input, search, replace, counterSkipConditions, otherConditions);
        }

        // 应用字符串替换：不跳过计数器
        private string ApplyStringReplaceWithoutCounterSkip(string input, string search, string replace, List<Condition> conditions)
        {
            var result = new StringBuilder();
            int lastIndex = 0;
            int currentIndex = 0;

            while (currentIndex < input.Length)
            {
                int foundIndex = input.IndexOf(search, currentIndex, StringComparison.Ordinal);
                if (foundIndex == -1)
                    break;

                if (CheckAllConditions(input, foundIndex, search.Length, conditions))
                {
                    result.Append(input, lastIndex, foundIndex - lastIndex);
                    result.Append(replace);
                    currentIndex = foundIndex + search.Length;
                    lastIndex = currentIndex;
                }
                else
                {
                    currentIndex = foundIndex + 1;
                }
            }

            result.Append(input, lastIndex, input.Length - lastIndex);
            return result.ToString();
        }

        // 应用字符串替换：跳过计数器
        private string ApplyStringReplaceWithCounterSkip(string input, string search, string replace,
            List<Condition> counterSkipConditions, List<Condition> otherConditions)
        {
            // 找到所有匹配的位置
            var matchPositions = new List<int>();
            int currentIndex = 0;

            while (currentIndex <= input.Length - search.Length)
            {
                int foundIndex = input.IndexOf(search, currentIndex, StringComparison.Ordinal);
                if (foundIndex == -1) break;

                if (CheckAllConditions(input, foundIndex, search.Length, otherConditions))
                {
                    matchPositions.Add(foundIndex);
                }
                currentIndex = foundIndex + 1;
            }

            if (matchPositions.Count == 0)
                return input;

            // 只保留计数器跳过条件，删除其他类型的条件处理
            var csConditions = counterSkipConditions.Where(c => c.Type == ConditionType.CounterSkipping).ToList();

            // 计算基础计数器跳过结果
            var executionResults = new List<bool[]>();
            foreach (var condition in csConditions)
            {
                bool[] result = CalculateCounterSkipResult(matchPositions.Count, condition.SkipPatterns);
                executionResults.Add(result);
            }

            bool[] finalExecution = MergeCounterSkipResults(executionResults, matchPositions.Count);

            // 构建最终结果
            var finalResult = new StringBuilder();
            int lastPos = 0;

            for (int i = 0; i < matchPositions.Count; i++)
            {
                int matchPos = matchPositions[i];

                finalResult.Append(input, lastPos, matchPos - lastPos);

                if (finalExecution[i])
                {
                    finalResult.Append(replace);
                }
                else
                {
                    finalResult.Append(search); // 跳过，保留原文本
                }

                lastPos = matchPos + search.Length;
            }

            finalResult.Append(input, lastPos, input.Length - lastPos);
            return finalResult.ToString();
        }



        // 应用字符串大小写规则
        private string ApplyStringCaseRule(string input, string search, CaseOperation operation, List<Condition> conditions)
        {
            var counterSkipConditions = conditions.Where(c => c.Type == ConditionType.CounterSkipping).ToList();
            var otherConditions = conditions.Where(c => c.Type != ConditionType.CounterSkipping).ToList();

            if (counterSkipConditions.Count == 0)
            {
                return ApplyStringCaseWithoutCounterSkip(input, search, operation, otherConditions);
            }

            return ApplyStringCaseWithCounterSkip(input, search, operation, counterSkipConditions, otherConditions);
        }

        // 应用字符串大小写：不跳过计数器
        private string ApplyStringCaseWithoutCounterSkip(string input, string search, CaseOperation operation, List<Condition> conditions)
        {
            var result = new StringBuilder();
            int lastIndex = 0;
            int currentIndex = 0;

            while (currentIndex <= input.Length - search.Length)
            {
                int foundIndex = input.IndexOf(search, currentIndex, StringComparison.Ordinal);
                if (foundIndex == -1) break;

                if (CheckAllConditions(input, foundIndex, search.Length, conditions))
                {
                    result.Append(input, lastIndex, foundIndex - lastIndex);

                    string matchedText = input.Substring(foundIndex, search.Length);
                    string transformedText = operation == CaseOperation.ToUpper ?
                        matchedText.ToUpper() : matchedText.ToLower();

                    result.Append(transformedText);
                    currentIndex = foundIndex + search.Length;
                    lastIndex = currentIndex;
                }
                else
                {
                    currentIndex = foundIndex + 1;
                }
            }

            result.Append(input, lastIndex, input.Length - lastIndex);
            return result.ToString();
        }

        // 应用字符串大小写：跳过计数器
        private string ApplyStringCaseWithCounterSkip(string input, string search, CaseOperation operation,
            List<Condition> counterSkipConditions, List<Condition> otherConditions)
        {
            // 找到所有匹配的位置
            var matchPositions = new List<int>();
            int currentIndex = 0;

            while (currentIndex <= input.Length - search.Length)
            {
                int foundIndex = input.IndexOf(search, currentIndex, StringComparison.Ordinal);
                if (foundIndex == -1) break;

                if (CheckAllConditions(input, foundIndex, search.Length, otherConditions))
                {
                    matchPositions.Add(foundIndex);
                }
                currentIndex = foundIndex + 1;
            }

            if (matchPositions.Count == 0)
                return input;

            // 计算计数器跳过结果
            var executionResults = new List<bool[]>();
            foreach (var condition in counterSkipConditions)
            {
                bool[] result = CalculateCounterSkipResult(matchPositions.Count, condition.SkipPatterns);
                executionResults.Add(result);
            }

            bool[] finalExecution = MergeCounterSkipResults(executionResults, matchPositions.Count);

            // 构建最终结果
            var finalResult = new StringBuilder();
            int lastPos = 0;

            for (int i = 0; i < matchPositions.Count; i++)
            {
                int matchPos = matchPositions[i];

                finalResult.Append(input, lastPos, matchPos - lastPos);

                if (finalExecution[i])
                {
                    string matchedText = input.Substring(matchPos, search.Length);
                    string transformedText = operation == CaseOperation.ToUpper ?
                        matchedText.ToUpper() : matchedText.ToLower();
                    finalResult.Append(transformedText);
                }
                else
                {
                    finalResult.Append(search); // 跳过，保留原文本
                }

                lastPos = matchPos + search.Length;
            }

            finalResult.Append(input, lastPos, input.Length - lastPos);
            return finalResult.ToString();
        }



        // 应用字符串向右插入规则
        private string ApplyStringRightInsertRule(string input, string search, string insertText, List<Condition> conditions)
        {
            var counterSkipConditions = conditions.Where(c => c.Type == ConditionType.CounterSkipping).ToList();
            var otherConditions = conditions.Where(c => c.Type != ConditionType.CounterSkipping).ToList();

            if (counterSkipConditions.Count == 0)
            {
                return ApplyStringRightInsertWithoutCounterSkip(input, search, insertText, otherConditions);
            }

            return ApplyStringRightInsertWithCounterSkip(input, search, insertText, counterSkipConditions, otherConditions);
        }

        // 应用字符串向右插入：不跳过计数器
        private string ApplyStringRightInsertWithoutCounterSkip(string input, string search, string insertText, List<Condition> conditions)
        {
            var result = new StringBuilder();
            int lastIndex = 0;
            int currentIndex = 0;

            while (currentIndex <= input.Length - search.Length)
            {
                int foundIndex = input.IndexOf(search, currentIndex, StringComparison.Ordinal);
                if (foundIndex == -1) break;

                if (CheckAllConditions(input, foundIndex, search.Length, conditions))
                {
                    result.Append(input, lastIndex, foundIndex - lastIndex + search.Length);
                    result.Append(insertText);
                    currentIndex = foundIndex + search.Length;
                    lastIndex = currentIndex;
                }
                else
                {
                    currentIndex = foundIndex + 1;
                }
            }

            result.Append(input, lastIndex, input.Length - lastIndex);
            return result.ToString();
        }

        // 应用字符串向右插入：跳过计数器
        private string ApplyStringRightInsertWithCounterSkip(string input, string search, string insertText,
            List<Condition> counterSkipConditions, List<Condition> otherConditions)
        {
            var matchPositions = new List<int>();
            int currentIndex = 0;

            while (currentIndex <= input.Length - search.Length)
            {
                int foundIndex = input.IndexOf(search, currentIndex, StringComparison.Ordinal);
                if (foundIndex == -1) break;

                if (CheckAllConditions(input, foundIndex, search.Length, otherConditions))
                {
                    matchPositions.Add(foundIndex);
                }
                currentIndex = foundIndex + 1;
            }

            if (matchPositions.Count == 0)
                return input;

            var executionResults = new List<bool[]>();
            foreach (var condition in counterSkipConditions)
            {
                bool[] result = CalculateCounterSkipResult(matchPositions.Count, condition.SkipPatterns);
                executionResults.Add(result);
            }

            bool[] finalExecution = MergeCounterSkipResults(executionResults, matchPositions.Count);

            var finalResult = new StringBuilder();
            int lastPos = 0;

            for (int i = 0; i < matchPositions.Count; i++)
            {
                int matchPos = matchPositions[i];

                finalResult.Append(input, lastPos, matchPos - lastPos + search.Length);

                if (finalExecution[i])
                {
                    finalResult.Append(insertText);
                }

                lastPos = matchPos + search.Length;
            }

            finalResult.Append(input, lastPos, input.Length - lastPos);
            return finalResult.ToString();
        }



        // 应用字符串向左插入规则
        private string ApplyStringLeftInsertRule(string input, string search, string insertText, List<Condition> conditions)
        {
            var counterSkipConditions = conditions.Where(c => c.Type == ConditionType.CounterSkipping).ToList();
            var otherConditions = conditions.Where(c => c.Type != ConditionType.CounterSkipping).ToList();

            if (counterSkipConditions.Count == 0)
            {
                return ApplyStringLeftInsertWithoutCounterSkip(input, search, insertText, otherConditions);
            }

            return ApplyStringLeftInsertWithCounterSkip(input, search, insertText, counterSkipConditions, otherConditions);
        }

        // 应用字符串向左插入：不跳过计数器
        private string ApplyStringLeftInsertWithoutCounterSkip(string input, string search, string insertText, List<Condition> conditions)
        {
            var result = new StringBuilder();
            int lastIndex = 0;
            int currentIndex = 0;

            while (currentIndex <= input.Length - search.Length)
            {
                int foundIndex = input.IndexOf(search, currentIndex, StringComparison.Ordinal);
                if (foundIndex == -1) break;

                if (CheckAllConditions(input, foundIndex, search.Length, conditions))
                {
                    result.Append(input, lastIndex, foundIndex - lastIndex);
                    result.Append(insertText);
                    result.Append(input, foundIndex, search.Length);
                    currentIndex = foundIndex + search.Length;
                    lastIndex = currentIndex;
                }
                else
                {
                    currentIndex = foundIndex + 1;
                }
            }

            result.Append(input, lastIndex, input.Length - lastIndex);
            return result.ToString();
        }

        // 应用字符串向左插入：跳过计数器
        private string ApplyStringLeftInsertWithCounterSkip(string input, string search, string insertText,
            List<Condition> counterSkipConditions, List<Condition> otherConditions)
        {
            var matchPositions = new List<int>();
            int currentIndex = 0;

            while (currentIndex <= input.Length - search.Length)
            {
                int foundIndex = input.IndexOf(search, currentIndex, StringComparison.Ordinal);
                if (foundIndex == -1) break;

                if (CheckAllConditions(input, foundIndex, search.Length, otherConditions))
                {
                    matchPositions.Add(foundIndex);
                }
                currentIndex = foundIndex + 1;
            }

            if (matchPositions.Count == 0)
                return input;

            var executionResults = new List<bool[]>();
            foreach (var condition in counterSkipConditions)
            {
                bool[] result = CalculateCounterSkipResult(matchPositions.Count, condition.SkipPatterns);
                executionResults.Add(result);
            }

            bool[] finalExecution = MergeCounterSkipResults(executionResults, matchPositions.Count);

            var finalResult = new StringBuilder();
            int lastPos = 0;

            for (int i = 0; i < matchPositions.Count; i++)
            {
                int matchPos = matchPositions[i];

                finalResult.Append(input, lastPos, matchPos - lastPos);

                if (finalExecution[i])
                {
                    finalResult.Append(insertText);
                }

                finalResult.Append(input, matchPos, search.Length);
                lastPos = matchPos + search.Length;
            }

            finalResult.Append(input, lastPos, input.Length - lastPos);
            return finalResult.ToString();
        }



        // 应用字符串凯撒密码规则
        private string ApplyStringCaesarRule(string input, string search, int shift, List<Condition> conditions)
        {
            var counterSkipConditions = conditions.Where(c => c.Type == ConditionType.CounterSkipping).ToList();
            var otherConditions = conditions.Where(c => c.Type != ConditionType.CounterSkipping).ToList();

            if (counterSkipConditions.Count == 0)
            {
                return ApplyStringCaesarWithoutCounterSkip(input, search, shift, otherConditions);
            }

            return ApplyStringCaesarWithCounterSkip(input, search, shift, counterSkipConditions, otherConditions);
        }

        // 应用字符串凯撒密码：不跳过计数器
        private string ApplyStringCaesarWithoutCounterSkip(string input, string search, int shift, List<Condition> conditions)
        {
            var result = new StringBuilder();
            int lastIndex = 0;
            int currentIndex = 0;

            while (currentIndex <= input.Length - search.Length)
            {
                int foundIndex = input.IndexOf(search, currentIndex, StringComparison.Ordinal);
                if (foundIndex == -1) break;

                if (CheckAllConditions(input, foundIndex, search.Length, conditions))
                {
                    result.Append(input, lastIndex, foundIndex - lastIndex);

                    string matchedText = input.Substring(foundIndex, search.Length);
                    string encryptedText = ApplyCaesarCipher(matchedText, shift);

                    result.Append(encryptedText);
                    currentIndex = foundIndex + search.Length;
                    lastIndex = currentIndex;
                }
                else
                {
                    currentIndex = foundIndex + 1;
                }
            }

            result.Append(input, lastIndex, input.Length - lastIndex);
            return result.ToString();
        }

        // 应用字符串凯撒密码：跳过计数器
        private string ApplyStringCaesarWithCounterSkip(string input, string search, int shift,
            List<Condition> counterSkipConditions, List<Condition> otherConditions)
        {
            var matchPositions = new List<int>();
            int currentIndex = 0;

            while (currentIndex <= input.Length - search.Length)
            {
                int foundIndex = input.IndexOf(search, currentIndex, StringComparison.Ordinal);
                if (foundIndex == -1) break;

                if (CheckAllConditions(input, foundIndex, search.Length, otherConditions))
                {
                    matchPositions.Add(foundIndex);
                }
                currentIndex = foundIndex + 1;
            }

            if (matchPositions.Count == 0)
                return input;

            var executionResults = new List<bool[]>();
            foreach (var condition in counterSkipConditions)
            {
                bool[] result = CalculateCounterSkipResult(matchPositions.Count, condition.SkipPatterns);
                executionResults.Add(result);
            }

            bool[] finalExecution = MergeCounterSkipResults(executionResults, matchPositions.Count);

            var finalResult = new StringBuilder();
            int lastPos = 0;

            for (int i = 0; i < matchPositions.Count; i++)
            {
                int matchPos = matchPositions[i];

                finalResult.Append(input, lastPos, matchPos - lastPos);

                if (finalExecution[i])
                {
                    string matchedText = input.Substring(matchPos, search.Length);
                    string encryptedText = ApplyCaesarCipher(matchedText, shift);
                    finalResult.Append(encryptedText);
                }
                else
                {
                    finalResult.Append(search); // 跳过，保留原文本
                }

                lastPos = matchPos + search.Length;
            }

            finalResult.Append(input, lastPos, input.Length - lastPos);
            return finalResult.ToString();
        }


        // 应用字符串大小写反转规则
        private string ApplyStringCaseInversionRule(string input, string search, List<Condition> conditions)
        {
            var counterSkipConditions = conditions.Where(c => c.Type == ConditionType.CounterSkipping).ToList();
            var otherConditions = conditions.Where(c => c.Type != ConditionType.CounterSkipping).ToList();

            if (counterSkipConditions.Count == 0)
            {
                return ApplyStringCaseInversionWithoutCounterSkip(input, search, otherConditions);
            }

            return ApplyStringCaseInversionWithCounterSkip(input, search, counterSkipConditions, otherConditions);
        }

        // 应用字符串大小写反转：不跳过计数器
        private string ApplyStringCaseInversionWithoutCounterSkip(string input, string search, List<Condition> conditions)
        {
            var result = new StringBuilder();
            int lastIndex = 0;
            int currentIndex = 0;

            while (currentIndex <= input.Length - search.Length)
            {
                int foundIndex = input.IndexOf(search, currentIndex, StringComparison.Ordinal);
                if (foundIndex == -1) break;

                if (CheckAllConditions(input, foundIndex, search.Length, conditions))
                {
                    result.Append(input, lastIndex, foundIndex - lastIndex);

                    string matchedText = input.Substring(foundIndex, search.Length);
                    string invertedText = InvertCase(matchedText);

                    result.Append(invertedText);
                    currentIndex = foundIndex + search.Length;
                    lastIndex = currentIndex;
                }
                else
                {
                    currentIndex = foundIndex + 1;
                }
            }

            result.Append(input, lastIndex, input.Length - lastIndex);
            return result.ToString();
        }

        // 应用字符串大小写反转：跳过计数器
        private string ApplyStringCaseInversionWithCounterSkip(string input, string search,
            List<Condition> counterSkipConditions, List<Condition> otherConditions)
        {
            var matchPositions = new List<int>();
            int currentIndex = 0;

            while (currentIndex <= input.Length - search.Length)
            {
                int foundIndex = input.IndexOf(search, currentIndex, StringComparison.Ordinal);
                if (foundIndex == -1) break;

                if (CheckAllConditions(input, foundIndex, search.Length, otherConditions))
                {
                    matchPositions.Add(foundIndex);
                }
                currentIndex = foundIndex + 1;
            }

            if (matchPositions.Count == 0)
                return input;

            var executionResults = new List<bool[]>();
            foreach (var condition in counterSkipConditions)
            {
                bool[] result = CalculateCounterSkipResult(matchPositions.Count, condition.SkipPatterns);
                executionResults.Add(result);
            }

            bool[] finalExecution = MergeCounterSkipResults(executionResults, matchPositions.Count);

            var finalResult = new StringBuilder();
            int lastPos = 0;

            for (int i = 0; i < matchPositions.Count; i++)
            {
                int matchPos = matchPositions[i];

                finalResult.Append(input, lastPos, matchPos - lastPos);

                if (finalExecution[i])
                {
                    string matchedText = input.Substring(matchPos, search.Length);
                    string invertedText = InvertCase(matchedText);
                    finalResult.Append(invertedText);
                }
                else
                {
                    finalResult.Append(search); // 跳过，保留原文本
                }

                lastPos = matchPos + search.Length;
            }

            finalResult.Append(input, lastPos, input.Length - lastPos);
            return finalResult.ToString();
        }

        private string InvertCase(string text)
        {
            var result = new StringBuilder();
            foreach (char c in text)
            {
                if (char.IsUpper(c))
                {
                    result.Append(char.ToLower(c));
                }
                else if (char.IsLower(c))
                {
                    result.Append(char.ToUpper(c));
                }
                else
                {
                    result.Append(c); // 非字母字符保持不变
                }
            }
            return result.ToString();
        }


        // 应用字符串的反转字符串规则
        private string ApplyStringStringReverseRule(string input, string search, List<Condition> conditions)
        {
            var counterSkipConditions = conditions.Where(c => c.Type == ConditionType.CounterSkipping).ToList();
            var otherConditions = conditions.Where(c => c.Type != ConditionType.CounterSkipping).ToList();

            if (counterSkipConditions.Count == 0)
            {
                return ApplyStringReverseWithoutCounterSkip(input, search, otherConditions);
            }

            return ApplyStringReverseWithCounterSkip(input, search, counterSkipConditions, otherConditions);
        }

        // 应用字符串的反转字符串：不跳过计数器
        private string ApplyStringReverseWithoutCounterSkip(string input, string search, List<Condition> conditions)
        {
            var result = new StringBuilder();
            int lastIndex = 0;
            int currentIndex = 0;

            while (currentIndex <= input.Length - search.Length)
            {
                int foundIndex = input.IndexOf(search, currentIndex, StringComparison.Ordinal);
                if (foundIndex == -1) break;

                if (CheckAllConditions(input, foundIndex, search.Length, conditions))
                {
                    result.Append(input, lastIndex, foundIndex - lastIndex);

                    string matchedText = input.Substring(foundIndex, search.Length);
                    string reversedText = ReverseString(matchedText);

                    result.Append(reversedText);
                    currentIndex = foundIndex + search.Length;
                    lastIndex = currentIndex;
                }
                else
                {
                    currentIndex = foundIndex + 1;
                }
            }

            result.Append(input, lastIndex, input.Length - lastIndex);
            return result.ToString();
        }

        // 应用字符串的反转字符串：跳过计数器
        private string ApplyStringReverseWithCounterSkip(string input, string search,
            List<Condition> counterSkipConditions, List<Condition> otherConditions)
        {
            var matchPositions = new List<int>();
            int currentIndex = 0;

            while (currentIndex <= input.Length - search.Length)
            {
                int foundIndex = input.IndexOf(search, currentIndex, StringComparison.Ordinal);
                if (foundIndex == -1) break;

                if (CheckAllConditions(input, foundIndex, search.Length, otherConditions))
                {
                    matchPositions.Add(foundIndex);
                }
                currentIndex = foundIndex + 1;
            }

            if (matchPositions.Count == 0)
                return input;

            var executionResults = new List<bool[]>();
            foreach (var condition in counterSkipConditions)
            {
                bool[] result = CalculateCounterSkipResult(matchPositions.Count, condition.SkipPatterns);
                executionResults.Add(result);
            }

            bool[] finalExecution = MergeCounterSkipResults(executionResults, matchPositions.Count);

            var finalResult = new StringBuilder();
            int lastPos = 0;

            for (int i = 0; i < matchPositions.Count; i++)
            {
                int matchPos = matchPositions[i];

                finalResult.Append(input, lastPos, matchPos - lastPos);

                if (finalExecution[i])
                {
                    string matchedText = input.Substring(matchPos, search.Length);
                    string reversedText = ReverseString(matchedText);
                    finalResult.Append(reversedText);
                }
                else
                {
                    finalResult.Append(search); // 跳过，保留原文本
                }

                lastPos = matchPos + search.Length;
            }

            finalResult.Append(input, lastPos, input.Length - lastPos);
            return finalResult.ToString();
        }




        // 计算计数器跳过结果
        private bool[] CalculateCounterSkipResult(int totalMatches, List<CounterSkipPattern> patterns)
        {
            bool[] result = new bool[totalMatches];
            int patternIndex = 0;
            int currentPatternCounter = 0;
            int executeRemaining = 0;
            int skipRemaining = 0;

            for (int i = 0; i < totalMatches; i++)
            {
                var currentPattern = patterns[patternIndex % patterns.Count];

                if (executeRemaining == 0 && skipRemaining == 0)
                {
                    // 开始新的模式周期
                    executeRemaining = currentPattern.ExecuteCount;
                    skipRemaining = currentPattern.SkipCount;
                }

                if (executeRemaining > 0)
                {
                    // 执行阶段
                    result[i] = true;
                    executeRemaining--;

                    if (executeRemaining == 0 && skipRemaining == 0)
                    {
                        patternIndex++;
                    }
                }
                else if (skipRemaining > 0)
                {
                    // 跳过阶段
                    result[i] = false;
                    skipRemaining--;

                    if (skipRemaining == 0)
                    {
                        patternIndex++;
                    }
                }
            }

            return result;
        }

        // 合并计数器跳过结果
        private bool[] MergeCounterSkipResults(List<bool[]> allResults, int totalMatches)
        {
            if (allResults.Count == 1)
                return allResults[0];

            bool[] finalResult = new bool[totalMatches];

            for (int i = 0; i < totalMatches; i++)
            {
                // 正常复合：只要有一个条件要求执行，就执行
                finalResult[i] = allResults.Any(result => result[i]);
            }

            return finalResult;
        }

        #endregion

        #region 正则表达式操作实现

        private string ApplyRegexReplaceRule(string input, string pattern, string replace, List<Condition> conditions)
        {
            return Regex.Replace(input, pattern, match =>
            {
                if (CheckAllConditions(input, match.Index, match.Length, conditions))
                    return replace;
                return match.Value;
            });
        }

        private string ApplyRegexCaseRule(string input, string pattern, CaseOperation operation, List<Condition> conditions)
        {
            return Regex.Replace(input, pattern, match =>
            {
                if (CheckAllConditions(input, match.Index, match.Length, conditions))
                    return operation == CaseOperation.ToUpper ? match.Value.ToUpper() : match.Value.ToLower();
                return match.Value;
            });
        }

        private string ApplyRegexRightInsertRule(string input, string pattern, string insertText, List<Condition> conditions)
        {
            return Regex.Replace(input, pattern, match =>
            {
                if (CheckAllConditions(input, match.Index, match.Length, conditions))
                    return match.Value + insertText;
                return match.Value;
            });
        }

        private string ApplyRegexLeftInsertRule(string input, string pattern, string insertText, List<Condition> conditions)
        {
            return Regex.Replace(input, pattern, match =>
            {
                if (CheckAllConditions(input, match.Index, match.Length, conditions))
                    return insertText + match.Value;
                return match.Value;
            });
        }

        private string ApplyRegexCaesarRule(string input, string pattern, int shift, List<Condition> conditions)
        {
            return Regex.Replace(input, pattern, match =>
            {
                if (CheckAllConditions(input, match.Index, match.Length, conditions))
                    return ApplyCaesarCipher(match.Value, shift);
                return match.Value;
            });
        }

        private string ApplyRegexCaseInversionRule(string input, string pattern, List<Condition> conditions)
        {
            var counterSkipConditions = conditions.Where(c => c.Type == ConditionType.CounterSkipping).ToList();
            var otherConditions = conditions.Where(c => c.Type != ConditionType.CounterSkipping).ToList();

            if (counterSkipConditions.Count == 0)
            {
                return Regex.Replace(input, pattern, match =>
                {
                    if (CheckAllConditions(input, match.Index, match.Length, otherConditions))
                        return InvertCase(match.Value);
                    return match.Value;
                });
            }

            // 对于正则表达式+计数器跳过，使用字符串方式处理
            var matches = new List<MatchInfo>();
            foreach (Match match in Regex.Matches(input, pattern))
            {
                if (CheckAllConditions(input, match.Index, match.Length, otherConditions))
                {
                    matches.Add(new MatchInfo { Position = match.Index, Length = match.Length, Value = match.Value });
                }
            }

            if (matches.Count == 0)
                return input;

            var executionResults = new List<bool[]>();
            foreach (var condition in counterSkipConditions)
            {
                bool[] result = CalculateCounterSkipResult(matches.Count, condition.SkipPatterns);
                executionResults.Add(result);
            }

            bool[] finalExecution = MergeCounterSkipResults(executionResults, matches.Count);

            var finalResult = new StringBuilder();
            int lastPos = 0;

            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];

                finalResult.Append(input, lastPos, match.Position - lastPos);

                if (finalExecution[i])
                {
                    finalResult.Append(InvertCase(match.Value));
                }
                else
                {
                    finalResult.Append(match.Value); // 跳过，保留原文本
                }

                lastPos = match.Position + match.Length;
            }

            finalResult.Append(input, lastPos, input.Length - lastPos);
            return finalResult.ToString();
        }

        private string ApplyRegexStringReverseRule(string input, string pattern, List<Condition> conditions)
        {
            var counterSkipConditions = conditions.Where(c => c.Type == ConditionType.CounterSkipping).ToList();
            var otherConditions = conditions.Where(c => c.Type != ConditionType.CounterSkipping).ToList();

            if (counterSkipConditions.Count == 0)
            {
                return Regex.Replace(input, pattern, match =>
                {
                    if (CheckAllConditions(input, match.Index, match.Length, otherConditions))
                        return ReverseString(match.Value);
                    return match.Value;
                });
            }

            // 对于正则表达式+计数器跳过，使用字符串方式处理
            var matches = new List<MatchInfo>();
            foreach (Match match in Regex.Matches(input, pattern))
            {
                if (CheckAllConditions(input, match.Index, match.Length, otherConditions))
                {
                    matches.Add(new MatchInfo { Position = match.Index, Length = match.Length, Value = match.Value });
                }
            }

            if (matches.Count == 0)
                return input;

            var executionResults = new List<bool[]>();
            foreach (var condition in counterSkipConditions)
            {
                bool[] result = CalculateCounterSkipResult(matches.Count, condition.SkipPatterns);
                executionResults.Add(result);
            }

            bool[] finalExecution = MergeCounterSkipResults(executionResults, matches.Count);

            var finalResult = new StringBuilder();
            int lastPos = 0;

            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];

                finalResult.Append(input, lastPos, match.Position - lastPos);

                if (finalExecution[i])
                {
                    finalResult.Append(ReverseString(match.Value));
                }
                else
                {
                    finalResult.Append(match.Value); // 跳过，保留原文本
                }

                lastPos = match.Position + match.Length;
            }

            finalResult.Append(input, lastPos, input.Length - lastPos);
            return finalResult.ToString();
        }

        #endregion

        #region 条件检查

        private bool CheckAllConditions(string input, int index, int length, List<Condition> conditions)
        {
            foreach (var condition in conditions)
            {
                if (!CheckCondition(input, index, length, condition))
                    return false;
            }
            return true;
        }

        private bool CheckCondition(string input, int index, int length, Condition condition)
        {
            switch (condition.Type)
            {
                case ConditionType.Prefix:
                    return CheckPrefixCondition(input, index, condition.Value);

                case ConditionType.LineHead:
                    return CheckLineHeadCondition(input, index);

                default:
                    return false;
            }
        }

        private bool CheckPrefixCondition(string input, int index, string requiredPrefix)
        {
            bool isRegex = IsRegexPattern(requiredPrefix);
            string pattern = isRegex ? GetRegexPattern(requiredPrefix) : requiredPrefix;

            if (index < pattern.Length && !isRegex)
                return false;

            if (isRegex)
            {
                if (index == 0) return false;

                string prefixText = input.Substring(0, index);
                return Regex.IsMatch(prefixText, pattern + "$");
            }
            else
            {
                var actualPrefix = input.Substring(index - pattern.Length, pattern.Length);
                return actualPrefix.Equals(pattern, StringComparison.Ordinal);
            }
        }

        private bool CheckLineHeadCondition(string input, int index)
        {
            // 检查是否是行首（开头或者前面是换行符）
            bool isLineStart = index == 0 ||
                              (index > 0 && input[index - 1] == '\n');

            // 处理Windows换行符 \r\n
            if (!isLineStart && index > 0 && input[index - 1] == '\r')
            {
                isLineStart = index == 1 || (index > 1 && input[index - 2] == '\n');
            }

            return isLineStart;
        }

        #endregion

        #region 工具方法

        private bool IsRegexPattern(string text)
        {
            return text.StartsWith("<") && text.EndsWith(">") && text.Length > 2;
        }

        private string GetRegexPattern(string text)
        {
            return text.Substring(1, text.Length - 2);
        }

        // 反转字符串
        private string ReverseString(string text)
        {
            char[] charArray = text.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        #endregion

        private class MatchInfo
        {
            public int Position { get; set; }
            public int Length { get; set; }
            public string Value { get; set; }
            public bool ShouldExecute { get; set; }
        }
    }

    public class Condition
    {
        public ConditionType Type { get; set; }
        public string Value { get; set; }
        public List<CounterSkipPattern> SkipPatterns { get; set; } = new List<CounterSkipPattern>();
    }

    // 计数器跳过
    public class CounterSkipPattern
    {
        public int ExecuteCount { get; set; }
        public int SkipCount { get; set; }
        public bool IsSinglePattern { get; set; }
    }

    public enum ConditionType
    {
        Prefix,
        LineHead,
        CounterSkipping
    }

    public enum CaseOperation
    {
        ToUpper,
        ToLower
    }

    public enum CaesarDirection
    {
        Right,
        Left
    }
}