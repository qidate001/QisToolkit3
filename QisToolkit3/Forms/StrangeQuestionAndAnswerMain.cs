using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.Diagnostics;

namespace QisToolkit3.Forms
{
    public partial class StrangeQuestionAndAnswerMain : Form
    {
        // ============ 常量配置 ============
        public const int MAX_QUESTIONS = 10;
        public const int MAIN_TYPE_WEIGHT = 85;
        public const int TOTAL_WEIGHT = 100;
        public const string NONE_MARKER = "/..None";

        // ============ 状态变量 ============
        private int questionCount = 1;
        private int questionTrueOption = 0;
        private bool debugMode = false;
        private bool debugShowTrue = false;
        private bool debugSetLast = false;
        private bool debugNoElse = false;

        // ============ 核心组件 ============
        private readonly Random random = new Random();
        private QuestionBankService questionBank;
        private Topic currentTopic;
        private List<string> currentOptions = new List<string>();
        private string currentGroupName = "主题库";

        // ============ 性能监控 ============
        private readonly Stopwatch loadTimer = new Stopwatch();

        public StrangeQuestionAndAnswerMain()
        {
            InitializeComponent();

            // 初始化题库服务
            string dataDir = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Datas",
                "StrangeQuestionAndAnswer"
            );
            questionBank = new QuestionBankService(dataDir);

            label_DeBug1.Visible = debugMode;
            label_DeBug2.Visible = debugMode;

            Qi.FormInitDo(this.Text);
        }

        // ============ 异步加载 ============
        private async void StrangeQuestionAndAnswerMain_Load(object sender, EventArgs e)
        {
            await LoadQuestionBankAsync();
        }

        private async Task LoadQuestionBankAsync()
        {
            try
            {
                loadTimer.Restart();

                // 显示加载状态
                label_Topic.Text = "⏳ 正在加载题库...";
                button_A.Enabled = false;
                button_B.Enabled = false;
                button_C.Enabled = false;
                button_D.Enabled = false;

                await questionBank.LoadAllTopicsAsync();

                loadTimer.Stop();

                if (debugMode)
                {
                    label_DeBug2.Text = $"加载完成: {questionBank.TotalGroupCount}组, " +
                                        $"{questionBank.TotalQuestionCount}题, " +
                                        $"耗时{loadTimer.ElapsedMilliseconds}ms";
                }

                // 启用按钮并开始
                button_A.Enabled = true;
                button_B.Enabled = true;
                button_C.Enabled = true;
                button_D.Enabled = true;

                Main();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载题库失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        // ============ 主逻辑 ============
        private void Main()
        {
            if (!questionBank.HasQuestions())
            {
                MessageBox.Show("没有可用的题目数据（缺少主题目组0.json）", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ShowTopic(GetTopic());

            if (debugMode)
            {
                label_DeBug1.Text = $"组:{currentGroupName}, 答案:{questionTrueOption}";
            }
        }

        private Topic GetTopic()
        {
            // 调试模式：强制使用最后一个特殊题库的最后一题
            if (debugMode && debugSetLast)
            {
                var lastResult = questionBank.GetLastSpecialTopic();
                currentGroupName = lastResult.GroupName;
                currentTopic = lastResult.Topic;
                PrepareRandomizedOptions();
                return currentTopic;
            }

            // 正常获取题目
            var result = questionBank.GetNextQuestion();
            currentGroupName = result.GroupName;
            currentTopic = result.Topic;
            PrepareRandomizedOptions();
            return currentTopic;
        }

        private void PrepareRandomizedOptions()
        {
            currentOptions.Clear();

            var options = new[] {
                currentTopic.OptionA,
                currentTopic.OptionB,
                currentTopic.OptionC,
                currentTopic.OptionD
            };

            currentOptions.AddRange(options.Where(o =>
                !string.IsNullOrEmpty(o) && o != NONE_MARKER));

            if (currentOptions.Count == 0)
            {
                throw new InvalidOperationException("题目没有有效选项");
            }

            // 获取原始正确答案文本
            string correctAnswer = GetCorrectAnswerText(currentTopic);

            // 随机打乱
            Shuffle(currentOptions);

            // 找到正确答案的新位置
            questionTrueOption = currentOptions.IndexOf(correctAnswer) + 1;

            if (questionTrueOption == 0)
            {
                throw new InvalidOperationException("正确答案不在选项中");
            }
        }

        private string GetCorrectAnswerText(Topic topic)
        {
            return topic.CorrectOptions switch
            {
                1 => topic.OptionA,
                2 => topic.OptionB,
                3 => topic.OptionC,
                4 => topic.OptionD,
                _ => throw new InvalidOperationException("无效的正确答案索引")
            };
        }

        private void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        // ============ UI 显示 ============
        public void ShowTopic(Topic topic)
        {
            label_Topic.Text = topic.Text;

            var buttons = new[] { button_A, button_B, button_C, button_D };
            var letters = new[] { "A", "B", "C", "D" };

            for (int i = 0; i < buttons.Length; i++)
            {
                if (i < currentOptions.Count)
                {
                    buttons[i].Text = $"{letters[i]}. {currentOptions[i]}";
                    buttons[i].Visible = true;
                    buttons[i].Enabled = true;
                }
                else
                {
                    buttons[i].Visible = false;
                    buttons[i].Enabled = false;
                }
            }

            ResetButtonColors();

            if (debugMode && debugShowTrue)
            {
                ShowAllAnswerColors();
            }
        }

        private void ResetButtonColors()
        {
            button_A.BackColor = SystemColors.Control;
            button_B.BackColor = SystemColors.Control;
            button_C.BackColor = SystemColors.Control;
            button_D.BackColor = SystemColors.Control;
        }

        // ============ 答题逻辑 ============
        private void button_A_Click(object sender, EventArgs e) => JudgeX(1);
        private void button_B_Click(object sender, EventArgs e) => JudgeX(2);
        private void button_C_Click(object sender, EventArgs e) => JudgeX(3);
        private void button_D_Click(object sender, EventArgs e) => JudgeX(4);

        private bool Judge(int selected)
        {
            bool isCorrect = (selected == questionTrueOption);

            if (!isCorrect)
            {
                ShowCorrectAnswer();

                string correctAnswer = currentOptions[questionTrueOption - 1];
                string correctLetter = questionTrueOption switch
                {
                    1 => "A",
                    2 => "B",
                    3 => "C",
                    4 => "D",
                    _ => "?"
                };

                var message = new StringBuilder();
                message.AppendLine("❌ 答案错误！");
                message.AppendLine();
                message.AppendLine($"✅ 正确答案：{correctLetter}. {correctAnswer}");

                if (!string.IsNullOrEmpty(currentTopic.Explanation))
                {
                    message.AppendLine();
                    message.AppendLine("📖 知识扩展：");
                    message.AppendLine(currentTopic.Explanation);
                }

                MessageBox.Show(message.ToString(), "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (debugMode && debugShowTrue)
                {
                    ShowAllAnswerColors();
                }
            }
            else if (debugMode && debugShowTrue)
            {
                ShowAllAnswerColors();
            }

            return isCorrect;
        }

        private void JudgeX(int selected)
        {
            if (!Judge(selected))
            {
                this.Close();
            }
            else
            {
                questionCount++;
                labelQuestionCount.Text = $"{questionCount}/{MAX_QUESTIONS}";

                if (questionCount >= MAX_QUESTIONS)
                {
                    MessageBox.Show("🎉 恭喜你完成全部题目！", "恭喜",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SaveWinData();
                    this.Close();
                }
                else
                {
                    Main();
                }
            }
        }

        private void ShowCorrectAnswer()
        {
            ResetButtonColors();

            var buttons = new[] { button_A, button_B, button_C, button_D };
            if (questionTrueOption >= 1 && questionTrueOption <= buttons.Length)
            {
                buttons[questionTrueOption - 1].BackColor = Color.Green;
            }
        }

        private void ShowAllAnswerColors()
        {
            ResetButtonColors();

            var buttons = new[] { button_A, button_B, button_C, button_D };
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].BackColor = (i + 1 == questionTrueOption) ? Color.Green : Color.Red;
            }
        }

        // ============ 数据保存 ============
        private void SaveWinData()
        {
            try
            {
                string dataDir = @"C:\QiAppDatas\Datas\QisToolkit3";
                string filePath = Path.Combine(dataDir, "StrangeQuestionAndAnswer.qidata");
                Directory.CreateDirectory(dataDir);

                int currentWins = 0;
                if (File.Exists(filePath))
                {
                    string content = File.ReadAllText(filePath);
                    int.TryParse(content, out currentWins);
                }

                File.WriteAllText(filePath, (currentWins + 1).ToString());
            }
            catch (Exception ex)
            {
                if (debugMode)
                {
                    MessageBox.Show($"保存数据失败：{ex.Message}", "调试信息",
                        MessageBoxButtons.OK);
                }
            }
        }

        private void label_Topic_Click(object sender, EventArgs e)
        {
            // SE.DoBaiDu(label_Topic.Text);
        }
    }

    // ============ 题库服务 ============
    public class QuestionBankService
    {
        private readonly string dataDirectory;
        private readonly Random random = new Random();
        private readonly object loadLock = new object();

        // 主题库（0.json）- 可循环使用
        private List<Topic> mainTopics = new List<Topic>();
        private int mainTopicIndex = 0;

        // 特殊题库（任意名称.json）- 使用字符串作为组名
        private Dictionary<string, List<Topic>> specialGroups = new Dictionary<string, List<Topic>>();
        private HashSet<string> usedGroups = new HashSet<string>(); // 已使用的特殊题库

        private bool isLoaded = false;
        private int totalQuestionCount = 0;

        public int TotalGroupCount => specialGroups.Count + (mainTopics.Count > 0 ? 1 : 0);
        public int TotalQuestionCount => totalQuestionCount;

        public QuestionBankService(string dataDirectory)
        {
            this.dataDirectory = dataDirectory;
        }

        public async Task LoadAllTopicsAsync()
        {
            if (isLoaded) return;

            await Task.Run(() =>
            {
                lock (loadLock)
                {
                    if (isLoaded) return;

                    if (!Directory.Exists(dataDirectory))
                    {
                        throw new DirectoryNotFoundException($"题目数据目录未找到：{dataDirectory}");
                    }

                    var jsonFiles = Directory.GetFiles(dataDirectory, "*.json");

                    foreach (var file in jsonFiles)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);

                        try
                        {
                            string jsonContent = File.ReadAllText(file);
                            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                            var topicsData = JsonSerializer.Deserialize<List<TopicJson>>(jsonContent, options);

                            var topics = topicsData.Select(t => new Topic(
                                t.Text,
                                t.OptionA,
                                t.OptionB,
                                t.OptionC,
                                t.OptionD,
                                t.CorrectOptions,
                                t.Explanation
                            )).ToList();

                            // 判断是否为主题库
                            if (fileName == "0")
                            {
                                // 主题库 - 打乱顺序
                                mainTopics = topics.OrderBy(x => random.Next()).ToList();
                                mainTopicIndex = 0;
                                totalQuestionCount += topics.Count;
                            }
                            else
                            {
                                // 特殊题库 - 使用文件名作为组名
                                specialGroups[fileName] = topics;
                                totalQuestionCount += topics.Count;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"加载文件 {file} 失败：{ex.Message}");
                            throw;
                        }
                    }

                    if (mainTopics.Count == 0)
                    {
                        throw new InvalidOperationException("未找到主题库（0.json）");
                    }

                    isLoaded = true;
                }
            });
        }

        public (string GroupName, Topic Topic) GetNextQuestion()
        {
            if (!isLoaded)
                throw new InvalidOperationException("题库尚未加载");

            // 决定使用主题库还是特殊题库
            int randomValue = random.Next(1, StrangeQuestionAndAnswerMain.TOTAL_WEIGHT + 1);

            if (randomValue <= StrangeQuestionAndAnswerMain.MAIN_TYPE_WEIGHT) // 85%使用主题库
            {
                return GetFromMainTopics();
            }
            else // 15%使用特殊题库
            {
                return GetFromSpecialGroup();
            }
        }

        private (string GroupName, Topic Topic) GetFromMainTopics()
        {
            if (mainTopics.Count == 0)
                throw new InvalidOperationException("主题库为空");

            // 主题库循环使用
            if (mainTopicIndex >= mainTopics.Count)
            {
                // 所有主题库题目用完，重新打乱
                mainTopics = mainTopics.OrderBy(x => random.Next()).ToList();
                mainTopicIndex = 0;
            }

            var topic = mainTopics[mainTopicIndex];
            mainTopicIndex++;
            return ("主题库", topic);
        }

        private (string GroupName, Topic Topic) GetFromSpecialGroup()
        {
            // 找出所有未使用的特殊题库
            var availableGroups = specialGroups.Keys
                .Where(g => !usedGroups.Contains(g))
                .ToList();

            if (availableGroups.Count == 0)
            {
                // 所有特殊题库都用完了，回退到主题库
                return GetFromMainTopics();
            }

            // 随机选择一个未使用的特殊题库
            string selectedGroupName = availableGroups[random.Next(availableGroups.Count)];
            var topics = specialGroups[selectedGroupName];

            // 标记该题库已使用（整个题库不能再被使用）
            usedGroups.Add(selectedGroupName);

            // 从该题库中随机选一道题
            int topicIndex = random.Next(topics.Count);
            var topic = topics[topicIndex];

            return (selectedGroupName, topic);
        }

        public (string GroupName, Topic Topic) GetLastSpecialTopic()
        {
            if (!isLoaded)
                throw new InvalidOperationException("题库尚未加载");

            if (specialGroups.Count == 0)
                return GetFromMainTopics();

            // 获取最后一个特殊题库（按文件名排序）
            string lastGroupName = specialGroups.Keys.OrderBy(x => x).Last();
            var topics = specialGroups[lastGroupName];
            var topic = topics.Last();

            // 标记该题库已使用
            usedGroups.Add(lastGroupName);

            return (lastGroupName, topic);
        }

        public bool HasQuestions()
        {
            return isLoaded && mainTopics.Count > 0;
        }

        // 获取已使用的特殊题库数量
        public int GetUsedSpecialGroupCount()
        {
            return usedGroups.Count;
        }

        // 获取剩余的特殊题库数量
        public int GetRemainingSpecialGroupCount()
        {
            return specialGroups.Keys.Count - usedGroups.Count;
        }

        // 获取所有特殊题库名称
        public List<string> GetSpecialGroupNames()
        {
            return specialGroups.Keys.ToList();
        }
    }

    // ============ 数据类 ============
    public class Topic
    {
        public string Text { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string Explanation { get; set; }
        public int CorrectOptions { get; set; }

        public Topic() { }

        public Topic(string text, string optionA, string optionB, string optionC,
                     string optionD, int correctOptions, string explanation = "")
        {
            Text = text ?? string.Empty;
            OptionA = optionA ?? "/..None";
            OptionB = optionB ?? "/..None";
            OptionC = optionC ?? "/..None";
            OptionD = optionD ?? "/..None";
            Explanation = explanation ?? string.Empty;
            CorrectOptions = correctOptions;
        }
    }

    public class TopicJson
    {
        public string Text { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string Explanation { get; set; }
        public int CorrectOptions { get; set; }
    }
}