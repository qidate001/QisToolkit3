using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace QisToolkit3.Console.Services
{
    /// <summary>
    /// 别名项
    /// </summary>
    public class AliasItem
    {
        public string Alias { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Source { get; set; } // 用于调试：记录别名来源
    }

    /// <summary>
    /// 别名配置文件
    /// </summary>
    public class AliasConfig
    {
        public string ConfigName { get; set; }
        public List<AliasItem> Aliases { get; set; } = new();
    }

    /// <summary>
    /// 多级别名服务 - 支持通用别名和命令专用别名
    /// </summary>
    public class MultiLevelAliasService
    {
        private readonly string _globalConfigPath;
        private readonly string _commandConfigPath;
        private readonly string _commandName;

        // 存储所有别名（已按优先级合并）
        private Dictionary<string, AliasItem> _mergedAliases;

        // 分开存储用于调试
        private Dictionary<string, AliasItem> _globalAliases;
        private Dictionary<string, AliasItem> _commandAliases;

        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandName">命令名称，null表示只加载通用配置</param>
        public MultiLevelAliasService(string commandName = null)
        {
            _commandName = commandName;
            _mergedAliases = new Dictionary<string, AliasItem>(StringComparer.OrdinalIgnoreCase);
            _globalAliases = new Dictionary<string, AliasItem>(StringComparer.OrdinalIgnoreCase);
            _commandAliases = new Dictionary<string, AliasItem>(StringComparer.OrdinalIgnoreCase);

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            // 通用配置文件路径: {actualDirectory}\Datas\Console\Aliases\global.json
            _globalConfigPath = Path.Combine(
                Qi.QisToolkit3_Datas.actualDirectory,
                "Datas",
                "Console",
                "Aliases",
                "global.json"
            );

            // 命令专用配置文件路径: {actualDirectory}\Datas\Console\Aliases\{commandName}.json
            if (!string.IsNullOrEmpty(commandName))
            {
                _commandConfigPath = Path.Combine(
                    Qi.QisToolkit3_Datas.actualDirectory,
                    "Datas",
                    "Console",
                    "Aliases",
                    $"{commandName}.json"
                );
            }

            LoadAll();
        }

        /// <summary>
        /// 加载所有配置文件并按优先级合并
        /// </summary>
        private void LoadAll()
        {
            // 1. 加载通用配置
            LoadGlobalConfig();

            // 2. 加载命令专用配置
            if (!string.IsNullOrEmpty(_commandName))
            {
                LoadCommandConfig();
            }

            // 3. 合并别名（命令专用覆盖通用）
            MergeAliases();
        }

        /// <summary>
        /// 加载通用配置文件
        /// </summary>
        private void LoadGlobalConfig()
        {
            try
            {
                if (!File.Exists(_globalConfigPath))
                {
                    // 创建默认空配置
                    SaveGlobalConfig();
                    return;
                }

                string json = File.ReadAllText(_globalConfigPath);
                var config = JsonSerializer.Deserialize<AliasConfig>(json, _jsonOptions);

                if (config?.Aliases != null)
                {
                    _globalAliases.Clear();
                    foreach (var alias in config.Aliases)
                    {
                        alias.Source = "global";
                        _globalAliases[alias.Alias] = alias;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载通用别名配置失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 加载命令专用配置文件
        /// </summary>
        private void LoadCommandConfig()
        {
            try
            {
                if (!File.Exists(_commandConfigPath))
                {
                    // 创建默认空配置
                    SaveCommandConfig();
                    return;
                }

                string json = File.ReadAllText(_commandConfigPath);
                var config = JsonSerializer.Deserialize<AliasConfig>(json, _jsonOptions);

                if (config?.Aliases != null)
                {
                    _commandAliases.Clear();
                    foreach (var alias in config.Aliases)
                    {
                        alias.Source = $"command-{_commandName}";
                        _commandAliases[alias.Alias] = alias;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载命令专用别名配置失败 [{_commandName}]: {ex.Message}");
            }
        }

        /// <summary>
        /// 合并别名（命令专用覆盖通用）
        /// </summary>
        private void MergeAliases()
        {
            _mergedAliases.Clear();

            // 1. 先添加所有通用别名
            foreach (var kv in _globalAliases)
            {
                _mergedAliases[kv.Key] = kv.Value;
            }

            // 2. 命令专用别名覆盖通用别名
            foreach (var kv in _commandAliases)
            {
                _mergedAliases[kv.Key] = kv.Value;
            }
        }

        /// <summary>
        /// 保存通用配置文件
        /// </summary>
        public void SaveGlobalConfig()
        {
            try
            {
                var config = new AliasConfig
                {
                    ConfigName = "global",
                    Aliases = new List<AliasItem>(_globalAliases.Values)
                };

                string json = JsonSerializer.Serialize(config, _jsonOptions);

                string directory = Path.GetDirectoryName(_globalConfigPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(_globalConfigPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"保存通用别名配置失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 保存命令专用配置文件
        /// </summary>
        public void SaveCommandConfig()
        {
            if (string.IsNullOrEmpty(_commandName)) return;

            try
            {
                var config = new AliasConfig
                {
                    ConfigName = _commandName,
                    Aliases = new List<AliasItem>(_commandAliases.Values)
                };

                string json = JsonSerializer.Serialize(config, _jsonOptions);

                string directory = Path.GetDirectoryName(_commandConfigPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(_commandConfigPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"保存命令专用别名配置失败 [{_commandName}]: {ex.Message}");
            }
        }

        /// <summary>
        /// 解析别名（按优先级：命令专用 > 通用）
        /// </summary>
        public string Resolve(string alias)
        {
            if (string.IsNullOrEmpty(alias))
                return alias;

            if (_mergedAliases.TryGetValue(alias, out var item))
            {
                // 调试信息：显示别名来源
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.WriteLine($"[别名] '{alias}' 来自 {item.Source}, 解析为: {item.Value}");
                System.Console.ResetColor();
                return item.Value;
            }

            return alias;
        }

        /// <summary>
        /// 判断是否是别名
        /// </summary>
        public bool IsAlias(string input)
        {
            return _mergedAliases.ContainsKey(input);
        }

        /// <summary>
        /// 获取别名来源信息（用于调试）
        /// </summary>
        public string GetAliasSource(string alias)
        {
            return _mergedAliases.TryGetValue(alias, out var item) ? item.Source : null;
        }

        /// <summary>
        /// 获取所有合并后的别名
        /// </summary>
        public Dictionary<string, AliasItem> GetAllAliases()
        {
            return new Dictionary<string, AliasItem>(_mergedAliases);
        }

        /// <summary>
        /// 获取通用别名
        /// </summary>
        public Dictionary<string, AliasItem> GetGlobalAliases()
        {
            return new Dictionary<string, AliasItem>(_globalAliases);
        }

        /// <summary>
        /// 获取命令专用别名
        /// </summary>
        public Dictionary<string, AliasItem> GetCommandAliases()
        {
            return new Dictionary<string, AliasItem>(_commandAliases);
        }

        /// <summary>
        /// 添加通用别名
        /// </summary>
        public void AddGlobalAlias(string alias, string value, string description = "")
        {
            var item = new AliasItem
            {
                Alias = alias,
                Value = value,
                Description = description,
                Source = "global"
            };
            _globalAliases[alias] = item;
            MergeAliases();
            SaveGlobalConfig();
        }

        /// <summary>
        /// 添加命令专用别名
        /// </summary>
        public void AddCommandAlias(string alias, string value, string description = "")
        {
            if (string.IsNullOrEmpty(_commandName)) return;

            var item = new AliasItem
            {
                Alias = alias,
                Value = value,
                Description = description,
                Source = $"command-{_commandName}"
            };
            _commandAliases[alias] = item;
            MergeAliases();
            SaveCommandConfig();
        }

        /// <summary>
        /// 删除通用别名
        /// </summary>
        public bool RemoveGlobalAlias(string alias)
        {
            bool removed = _globalAliases.Remove(alias);
            if (removed)
            {
                MergeAliases();
                SaveGlobalConfig();
            }
            return removed;
        }

        /// <summary>
        /// 删除命令专用别名
        /// </summary>
        public bool RemoveCommandAlias(string alias)
        {
            if (string.IsNullOrEmpty(_commandName)) return false;

            bool removed = _commandAliases.Remove(alias);
            if (removed)
            {
                MergeAliases();
                SaveCommandConfig();
            }
            return removed;
        }

        /// <summary>
        /// 重新加载所有配置
        /// </summary>
        public void Reload()
        {
            _globalAliases.Clear();
            _commandAliases.Clear();
            _mergedAliases.Clear();
            LoadAll();
        }
    }
}