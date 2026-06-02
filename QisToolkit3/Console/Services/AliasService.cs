using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using QisToolkit3.Console.Models;

namespace QisToolkit3.Console.Services
{
    /// <summary>
    /// 别名服务
    /// </summary>
    public class AliasService
    {
        private Dictionary<string, BiliBiliAlias> _aliasMap;
        private readonly string _configPath;
        private readonly JsonSerializerOptions _jsonOptions;

        public AliasService()
        {
            _aliasMap = new Dictionary<string, BiliBiliAlias>(StringComparer.OrdinalIgnoreCase);
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,  // 不区分大小写
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            // 配置文件路径
            _configPath = Path.Combine(
                Qi.QisToolkit3_Datas.actualDirectory,
                "Datas",
                "Console",
                "BiliBiliAliases.json"
            );

            // 添加调试输出
            //System.Diagnostics.Debug.WriteLine($"配置文件路径: {_configPath}");
            //System.Console.ForegroundColor = ConsoleColor.DarkGray;
            //System.Console.WriteLine($"[调试] 配置文件路径: {_configPath}");
            //System.Console.ResetColor();
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        public void Load()
        {
            try
            {
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                //System.Console.WriteLine($"[调试] 尝试加载配置文件: {_configPath}");

                if (!File.Exists(_configPath))
                {
                    //System.Console.WriteLine($"[调试] 配置文件不存在，创建默认配置");
                    CreateDefaultConfig();
                    return;
                }

                //System.Console.WriteLine($"[调试] 配置文件存在，开始读取");
                string json = File.ReadAllText(_configPath);
                //System.Console.WriteLine($"[调试] 读取到内容: {json}");

                var config = JsonSerializer.Deserialize<BiliBiliAliasConfig>(json, _jsonOptions);

                if (config?.Aliases != null)
                {
                    _aliasMap.Clear();

                    // 添加调试信息，查看config的具体内容
                    //System.Console.WriteLine($"[调试] config 类型: {config.GetType()}");
                    //System.Console.WriteLine($"[调试] Aliases 是否为 null: {config.Aliases == null}");
                    if (config.Aliases != null)
                    {
                        //System.Console.WriteLine($"[调试] Aliases 数量: {config.Aliases.Count}");

                        foreach (var alias in config.Aliases)
                        {
                            // 添加调试信息，查看每个别名的属性
                            //System.Console.WriteLine($"[调试] 别名对象类型: {alias.GetType()}");
                            //System.Console.WriteLine($"[调试] Alias属性: '{alias.Alias}' (是否为null: {alias.Alias == null})");
                            //System.Console.WriteLine($"[调试] Value属性: '{alias.Value}' (是否为null: {alias.Value == null})");
                            //System.Console.WriteLine($"[调试] Description属性: '{alias.Description}'");

                            if (!string.IsNullOrEmpty(alias.Alias) && !string.IsNullOrEmpty(alias.Value))
                            {
                                _aliasMap[alias.Alias] = alias;
                                //System.Console.WriteLine($"[调试] 成功加载别名: {alias.Alias} -> {alias.Value}");
                            }
                            else
                            {
                                //System.Console.WriteLine($"[调试] 跳过无效别名: Alias={alias.Alias}, Value={alias.Value}");
                            }
                        }
                        //System.Console.WriteLine($"[调试] 最终加载 {_aliasMap.Count} 个别名");
                    }
                }
                else
                {
                    //System.Console.WriteLine($"[调试] 配置文件内容为空或格式错误");
                }
                System.Console.ResetColor();
            }
            catch (Exception ex)
            {
                //System.Console.ForegroundColor = ConsoleColor.Red;
                //System.Console.WriteLine($"[调试] 加载别名配置失败: {ex.Message}");
                //System.Console.WriteLine($"[调试] 堆栈: {ex.StackTrace}");
                //System.Console.ResetColor();

                _aliasMap.Clear();
            }
        }

        /// <summary>
        /// 创建默认配置文件
        /// </summary>
        private void CreateDefaultConfig()
        {
            try
            {
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                //System.Console.WriteLine($"[调试] 开始创建默认配置");

                var defaultAliases = new[]
                {
                    new BiliBiliAlias { Alias = "hua", Value = "697987209", Description = "花花椒" },
                    new BiliBiliAlias { Alias = "huahua", Value = "697987209", Description = "花花" },
                    new BiliBiliAlias { Alias = "花", Value = "697987209" },
                    new BiliBiliAlias { Alias = "花花", Value = "697987209" },
                    new BiliBiliAlias { Alias = "花花椒", Value = "697987209" },
                    new BiliBiliAlias { Alias = "花花椒咯", Value = "697987209" }
                };

                foreach (var alias in defaultAliases)
                {
                    _aliasMap[alias.Alias] = alias;
                    //System.Console.WriteLine($"[调试] 添加默认别名: {alias.Alias} -> {alias.Value}");
                }

                Save();
                //System.Console.WriteLine($"[调试] 默认配置创建完成");
                //System.Console.ResetColor();
            }
            catch (Exception ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine($"[调试] 创建默认配置失败: {ex.Message}");
                System.Console.ResetColor();
            }
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public void Save()
        {
            try
            {
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                //System.Console.WriteLine($"[调试] 开始保存配置文件到: {_configPath}");

                var config = new BiliBiliAliasConfig
                {
                    Aliases = new List<BiliBiliAlias>(_aliasMap.Values)
                };

                string json = JsonSerializer.Serialize(config, _jsonOptions);
                //System.Console.WriteLine($"[调试] 序列化后的JSON: {json}");

                // 确保目录存在
                string directory = Path.GetDirectoryName(_configPath);
                //System.Console.WriteLine($"[调试] 目录: {directory}");

                if (!Directory.Exists(directory))
                {
                    //System.Console.WriteLine($"[调试] 目录不存在，创建目录");
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(_configPath, json);
                //System.Console.WriteLine($"[调试] 文件保存成功");
                //System.Console.ResetColor();
            }
            catch (Exception ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine($"[AliasService] 保存别名配置失败: {ex.Message}");
                System.Console.ResetColor();
            }
        }

        /// <summary>
        /// 解析别名
        /// </summary>
        public string Resolve(string alias)
        {
            if (string.IsNullOrEmpty(alias))
                return alias;

            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            if (_aliasMap.TryGetValue(alias, out var item))
            {
                System.Console.WriteLine($"[调试] 找到别名: {alias} -> {item.Value}");
                System.Console.ResetColor();
                return item.Value;
            }
            else
            {
                System.Console.WriteLine($"[调试] 未找到别名: {alias}");
                System.Console.ResetColor();
                return alias;
            }
        }

        /// <summary>
        /// 判断是否是已注册的别名
        /// </summary>
        public bool IsAlias(string input)
        {
            bool result = _aliasMap.ContainsKey(input);
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine($"[调试] 检查别名 '{input}': {(result ? "是" : "否")}");
            System.Console.ResetColor();
            return result;
        }

        /// <summary>
        /// 获取所有别名
        /// </summary>
        public Dictionary<string, BiliBiliAlias> GetAllAliases()
        {
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine($"[调试] 获取所有别名，当前数量: {_aliasMap.Count}");
            System.Console.ResetColor();
            return new Dictionary<string, BiliBiliAlias>(_aliasMap);
        }

        /// <summary>
        /// 添加或更新别名
        /// </summary>
        public void AddOrUpdateAlias(string alias, string value, string description = "")
        {
            _aliasMap[alias] = new BiliBiliAlias
            {
                Alias = alias,
                Value = value,
                Description = description
            };
            Save();
        }

        /// <summary>
        /// 删除别名
        /// </summary>
        public bool RemoveAlias(string alias)
        {
            bool removed = _aliasMap.Remove(alias);
            if (removed)
            {
                Save();
            }
            return removed;
        }

        /// <summary>
        /// 重新加载配置
        /// </summary>
        public void Reload()
        {
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine($"[调试] 重新加载配置");
            System.Console.ResetColor();
            _aliasMap.Clear();
            Load();
        }
    }
}