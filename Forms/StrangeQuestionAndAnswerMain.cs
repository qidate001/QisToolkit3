using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;

namespace QisToolkit3.Forms
{
    public partial class StrangeQuestionAndAnswerMain : Form
    {
        private int QuestionCount = 1;
        private int QuestionTrueOption = 0;
        private bool DeBugMode = false, DeBugMode_ShowTheTrue = false, DeBugMode_SetLast = true, DeBugMode_NoElse = false;
        private Random random = new Random();

        // 存储所有题目类型
        private Dictionary<int, List<Topic>> topicGroups = new Dictionary<int, List<Topic>>();
        private HashSet<int> triggeredGroups = new HashSet<int>(); // 记录已触发过的特殊类型
        private Topic currentTopic;
        private List<string> currentOptions = new List<string>();
        private int currentGroupId = 0; // 当前题目所属的组ID


        public StrangeQuestionAndAnswerMain()
        {
            InitializeComponent();
            LoadAllTopics();
            label_DeBug1.Visible = DeBugMode;
            label_DeBug2.Visible = DeBugMode;

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void StrangeQuestionAndAnswerMain_Load(object sender, EventArgs e) => Main();

        // 加载所有题目类型
        private void LoadAllTopics()
        {
            try
            {
                string actualDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string dataDir = Path.Combine(actualDirectory, "Datas", "StrangeQuestionAndAnswer");

                if (!Directory.Exists(dataDir))
                {
                    MessageBox.Show($"题目数据目录未找到：{dataDir}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 加载所有JSON文件
                var jsonFiles = Directory.GetFiles(dataDir, "*.json");

                foreach (var file in jsonFiles)
                {
                    try
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        if (int.TryParse(fileName, out int groupId))
                        {
                            string jsonContent = File.ReadAllText(file);
                            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                            var topicsData = JsonSerializer.Deserialize<List<TopicJson>>(jsonContent, options);

                            var topicList = new List<Topic>();
                            foreach (var topicJson in topicsData)
                            {
                                topicList.Add(new Topic(
                                    topicJson.Text,
                                    topicJson.OptionA,
                                    topicJson.OptionB,
                                    topicJson.OptionC,
                                    topicJson.OptionD,
                                    topicJson.CorrectOptions,
                                    topicJson.Explanation
                                ));
                            }

                            topicGroups[groupId] = topicList;
                            if (DeBugMode)
                            {
                                System.Console.WriteLine($"已加载 {topicList.Count} 道题目到组 {groupId}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"加载文件 {file} 失败：{ex.Message}", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                if (DeBugMode)
                {
                    label_DeBug2.Text = $"已加载 {topicGroups.Count} 个题目组";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载题目失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 主函数
        private void Main()
        {
            if (!topicGroups.ContainsKey(0) || topicGroups[0].Count == 0)
            {
                MessageBox.Show("没有可用的题目数据（缺少主题目组0.json）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ShowTopic(GetTopic());
            if (DeBugMode)
            {
                label_DeBug1.Text = $"当前组: {currentGroupId}, 正确答案索引: {QuestionTrueOption}";
            }
        }

        // 获取题目
        private Topic GetTopic()
        {
            // 调试模式特殊处理
            if (DeBugMode && DeBugMode_SetLast)
            {
                currentGroupId = topicGroups.Keys.Max();
                var groupTopics = topicGroups[currentGroupId];
                currentTopic = groupTopics.Last();
                PrepareRandomizedOptions();
                return currentTopic;
            }

            // 计算随机值决定题目类型
            // 0.json（主类型）有85%概率，其他类型共享15%概率
            int randomValue = random.Next(1, 101);

            if (randomValue <= 85 || DeBugMode_NoElse) // 85%概率选择主类型
            {
                currentGroupId = 0;
            }
            else
            {
                // 从未触发过的特殊类型中选择
                var availableGroups = topicGroups.Keys
                    .Where(k => k != 0 && !triggeredGroups.Contains(k))
                    .ToList();

                if (availableGroups.Count == 0)
                {
                    // 所有特殊类型都已触发过，回退到主类型
                    currentGroupId = 0;
                }
                else
                {
                    currentGroupId = availableGroups[random.Next(availableGroups.Count)];
                    triggeredGroups.Add(currentGroupId); // 标记该类型已触发
                }
            }

            // 从选中的类型中随机选择一道题目
            var selectedGroup = topicGroups[currentGroupId];
            int topicIndex = random.Next(selectedGroup.Count);
            currentTopic = selectedGroup[topicIndex];

            PrepareRandomizedOptions();

            return currentTopic;
        }

        // 准备随机排序的选项
        private void PrepareRandomizedOptions()
        {
            currentOptions.Clear();

            // 添加有效选项
            if (!string.IsNullOrEmpty(currentTopic.OptionA) && currentTopic.OptionA != "/..None")
                currentOptions.Add(currentTopic.OptionA);
            if (!string.IsNullOrEmpty(currentTopic.OptionB) && currentTopic.OptionB != "/..None")
                currentOptions.Add(currentTopic.OptionB);
            if (!string.IsNullOrEmpty(currentTopic.OptionC) && currentTopic.OptionC != "/..None")
                currentOptions.Add(currentTopic.OptionC);
            if (!string.IsNullOrEmpty(currentTopic.OptionD) && currentTopic.OptionD != "/..None")
                currentOptions.Add(currentTopic.OptionD);

            // 获取原始正确答案
            string correctAnswer = "";
            switch (currentTopic.CorrectOptions)
            {
                case 1: correctAnswer = currentTopic.OptionA; break;
                case 2: correctAnswer = currentTopic.OptionB; break;
                case 3: correctAnswer = currentTopic.OptionC; break;
                case 4: correctAnswer = currentTopic.OptionD; break;
            }

            // 随机排序选项
            Shuffle(currentOptions);

            // 找到随机排序后正确答案的新位置
            QuestionTrueOption = currentOptions.IndexOf(correctAnswer) + 1;
        }

        // 随机打乱列表
        private void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void button_A_Click(object sender, EventArgs e) => JudgeX(1);
        private void button_B_Click(object sender, EventArgs e) => JudgeX(2);
        private void button_C_Click(object sender, EventArgs e) => JudgeX(3);
        private void button_D_Click(object sender, EventArgs e) => JudgeX(4);

        public void ShowTopic(Topic topic)
        {
            label_Topic.Text = topic.Text;

            // 显示随机排序后的选项
            button_A.Text = currentOptions.Count > 0 ? "A. " + currentOptions[0] : "";
            button_B.Text = currentOptions.Count > 1 ? "B. " + currentOptions[1] : "";
            button_C.Text = currentOptions.Count > 2 ? "C. " + currentOptions[2] : "";
            button_D.Text = currentOptions.Count > 3 ? "D. " + currentOptions[3] : "";

            // 重置按钮颜色
            ResetButtonColors();

            // 调试模式下的显示
            if (DeBugMode && DeBugMode_ShowTheTrue)
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

        private void ShowColor()
        {
            if (QuestionTrueOption == 1) button_A.BackColor = Color.Green;
            else button_A.BackColor = Color.Red;

            if (QuestionTrueOption == 2) button_B.BackColor = Color.Green;
            else button_B.BackColor = Color.Red;

            if (QuestionTrueOption == 3) button_C.BackColor = Color.Green;
            else button_C.BackColor = Color.Red;

            if (QuestionTrueOption == 4) button_D.BackColor = Color.Green;
            else button_D.BackColor = Color.Red;
        }

        private bool Judge(int Selected)
        {
            bool isCorrect = (Selected == QuestionTrueOption);

            if (!isCorrect)
            {
                // 显示正确答案的颜色
                ShowCorrectAnswer();

                // 获取正确答案的文本
                string correctAnswer = currentOptions[QuestionTrueOption - 1];
                string correctLetter = QuestionTrueOption switch
                {
                    1 => "A",
                    2 => "B",
                    3 => "C",
                    4 => "D",
                    _ => "?"
                };

                // 创建更详细的提示消息
                StringBuilder message = new StringBuilder();
                message.AppendLine("❌ 答案错误！");
                message.AppendLine();
                message.AppendLine($"✅ 正确答案：{correctLetter}. {correctAnswer}");

                if (!string.IsNullOrEmpty(currentTopic.Explanation))
                {
                    message.AppendLine();
                    message.AppendLine("📖 知识扩展：");
                    message.AppendLine(currentTopic.Explanation);
                }

                MessageBox.Show(message.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 如果是调试模式，也显示其他错误选项的颜色
                if (DeBugMode && DeBugMode_ShowTheTrue)
                {
                    ShowAllAnswerColors();
                }
            }
            else
            {
                // 答对时，只显示绿色正确答案（可选）
                if (DeBugMode && DeBugMode_ShowTheTrue)
                {
                    ShowAllAnswerColors();
                }
            }

            return isCorrect;
        }

        private void JudgeX(int Selected)
        {
            if (!Judge(Selected))
            {
                this.Close();
            }
            else
            {
                QuestionCount++;
                labelQuestionCount.Text = $"{QuestionCount}/10";

                if (QuestionCount >= 10)
                {
                    MessageBox.Show("你赢了！", "恭喜", MessageBoxButtons.OK);
                    SaveWinData();
                    this.Close();
                }
                else
                {
                    Main();
                }
            }
        }

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
                // 静默处理文件保存错误
                if (DeBugMode)
                {
                    MessageBox.Show($"保存数据失败：{ex.Message}", "调试信息", MessageBoxButtons.OK);
                }
            }
        }

        // 只显示正确答案的颜色（绿色）
        private void ShowCorrectAnswer()
        {
            ResetButtonColors();

            // 根据正确答案索引设置绿色
            switch (QuestionTrueOption)
            {
                case 1:
                    button_A.BackColor = Color.Green;
                    break;
                case 2:
                    button_B.BackColor = Color.Green;
                    break;
                case 3:
                    button_C.BackColor = Color.Green;
                    break;
                case 4:
                    button_D.BackColor = Color.Green;
                    break;
            }
        }

        // 显示所有选项的颜色（调试模式用）
        private void ShowAllAnswerColors()
        {
            ResetButtonColors();

            // 按钮A的颜色
            button_A.BackColor = (QuestionTrueOption == 1) ? Color.Green : Color.Red;

            // 按钮B的颜色
            button_B.BackColor = (QuestionTrueOption == 2) ? Color.Green : Color.Red;

            // 按钮C的颜色
            button_C.BackColor = (QuestionTrueOption == 3) ? Color.Green : Color.Red;

            // 按钮D的颜色
            button_D.BackColor = (QuestionTrueOption == 4) ? Color.Green : Color.Red;
        }

        private void label_Topic_Click(object sender, EventArgs e)
        {
            // SE.DoBaiDu(label_Topic.Text);
        }
    }

    public struct Topic
    {
        public string Text;
        public string OptionA;
        public string OptionB;
        public string OptionC;
        public string OptionD;
        public string Explanation;
        public int CorrectOptions;

        public Topic(string text, string optionA, string optionB, string optionC, string optionD, int correctOptions, string explanation = "")
        {
            Text = text;
            OptionA = optionA;
            OptionB = optionB;
            OptionC = optionC;
            OptionD = optionD;
            Explanation = explanation ?? "";
            CorrectOptions = correctOptions;
        }
    }

    // JSON反序列化用的类
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