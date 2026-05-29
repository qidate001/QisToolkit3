using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QisToolkit3.Forms.DCQAA;
using static Qi;
using static Qi.QisToolkit3_Datas;

namespace QisToolkit3.Forms
{
    public partial class DCQAA : Form
    {
        ConfigProgram configProgram = JsonReader.ReadJsonFile<ConfigProgram>(@"CQAA\Program.json");
        Topic topic;
        private bool DeBugMode = false;

        public DCQAA()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void DCQAA_Load(object sender, EventArgs e)
        {
            textBoxTitle.Text = configProgram.Title;
            textBoxIcon.Text = configProgram.Icon;
            textBoxWidth.Text = configProgram.Width.ToString();
            textBoxHeight.Text = configProgram.Height.ToString();
            textBoxBackColor_Red.Text = configProgram.BackColor_Red.ToString();
            textBoxBackColor_Green.Text = configProgram.BackColor_Green.ToString();
            textBoxBackColor_Blue.Text = configProgram.BackColor_Blue.ToString();
            textBoxBackgroundImage.Text = configProgram.BackgroundImage;
            textBoxBackgroundImageLayout.Text = configProgram.BackgroundImageLayout.ToString();
            textBoxbuttonA_Width.Text = configProgram.buttonA_Width.ToString();
            textBoxbuttonB_Width.Text = configProgram.buttonB_Width.ToString();
            textBoxbuttonC_Width.Text = configProgram.buttonC_Width.ToString();
            textBoxbuttonD_Width.Text = configProgram.buttonD_Width.ToString();
            textBoxbuttonA_Height.Text = configProgram.buttonA_Height.ToString();
            textBoxbuttonB_Height.Text = configProgram.buttonB_Height.ToString();
            textBoxbuttonC_Height.Text = configProgram.buttonC_Height.ToString();
            textBoxbuttonD_Height.Text = configProgram.buttonD_Height.ToString();
            checkBoxEnableClosingProgramAfterErrorAnswering.Checked = configProgram.EnableClosingProgramAfterErrorAnswering;
            textBoxFinishText.Text = configProgram.FinishText;
            textBoxFullText.Text = configProgram.FullText;
            textBoxFullErrorText.Text = configProgram.FullErrorText;

            comboBoxTopics.Items.Clear();
            comboBoxTopics.Items.AddRange(Directory.GetFiles(actualDirectory + @"\CQAA\topic"));
        }

        private void LoadTopicFile()
        {
            if (File.Exists(comboBoxTopics.Text))
            {
                topic = JsonReader.ReadJsonFile<Topic>(comboBoxTopics.Text);

                textBoxText.Text = TextRN(topic.Text);
                textBoxPx.Text = topic.Px.ToString();
                textBoxFamilyName.Text = topic.FamilyName;
                textBoxAnswer.Text = TextRN(topic.Answer);
                textBoxTextRunType.Text = topic.TextRunType;
                textBoxTextRunData.Text = TextRN(topic.TextRunData);
                textBoxOptionA.Text = TextRN(topic.OptionA);
                textBoxOptionB.Text = TextRN(topic.OptionB);
                textBoxOptionC.Text = TextRN(topic.OptionC);
                textBoxOptionD.Text = TextRN(topic.OptionD);
                checkBoxEnableOptionLeadingText.Checked = topic.EnableOptionLeadingText;
                checkBoxCorrectOptionsA.Checked = topic.CorrectOptionsA;
                checkBoxCorrectOptionsB.Checked = topic.CorrectOptionsB;
                checkBoxCorrectOptionsC.Checked = topic.CorrectOptionsC;
                checkBoxCorrectOptionsD.Checked = topic.CorrectOptionsD;

                buttonSave2.Text = "生成代码并导入 " + comboBoxTopics.Text;
            }
        }

        public class Topic
        {
            public string Text { get; set; } = string.Empty;
            public string FamilyName { get; set; } = "宋体";
            public float Px { get; set; } = -1;
            public string TextRunType { get; set; } = "NoRun";
            public string TextRunData { get; set; } = "QisToolkit3_CQAA.exe";
            public int AnswerTime { get; set; } = 1000;
            public int Type { get; set; } = 0;
            public string Answer { get; set; } = "NoAnswer";
            public string OptionA { get; set; } = "选项 A";
            public string OptionB { get; set; } = "选项 B";
            public string OptionC { get; set; } = "选项 C";
            public string OptionD { get; set; } = "选项 D";
            public bool EnableOptionLeadingText { get; set; } = true;
            public bool CorrectOptionsA { get; set; }
            public bool CorrectOptionsB { get; set; }
            public bool CorrectOptionsC { get; set; }
            public bool CorrectOptionsD { get; set; }
            public int OptionButtonA_X { get; set; } = -1;
            public int OptionButtonA_Y { get; set; } = -1;
            public int OptionButtonB_X { get; set; } = -1;
            public int OptionButtonB_Y { get; set; } = -1;
            public int OptionButtonC_X { get; set; } = -1;
            public int OptionButtonC_Y { get; set; } = -1;
            public int OptionButtonD_X { get; set; } = -1;
            public int OptionButtonD_Y { get; set; } = -1;
        }

        public class ConfigProgram
        {
            public string Title { get; set; } = string.Empty;
            public string Icon { get; set; } = "QiLogo";
            public int BackColor_Red { get; set; } = -1;
            public int BackColor_Green { get; set; } = -1;
            public int BackColor_Blue { get; set; } = -1;
            public string BackgroundImage { get; set; } = "None";
            public int BackgroundImageLayout { get; set; } = 3;
            public int Width { get; set; } = -1;
            public int Height { get; set; } = -1;
            public int buttonA_Width { get; set; } = -1;
            public int buttonA_Height { get; set; } = -1;
            public int buttonB_Width { get; set; } = -1;
            public int buttonB_Height { get; set; } = -1;
            public int buttonC_Width { get; set; } = -1;
            public int buttonC_Height { get; set; } = -1;
            public int buttonD_Width { get; set; } = -1;
            public int buttonD_Height { get; set; } = -1;
            public int BackColor_Option_Red { get; set; } = -1;
            public int BackColor_Option_Green { get; set; } = -1;
            public int BackColor_Option_Blue { get; set; } = -1;
            public int QuestionCount { get; set; } = 10;
            public bool EnableClosingProgramAfterErrorAnswering { get; set; } = false;
            public string FinishText { get; set; } = "恭喜你，完成题目！";
            public string FullText { get; set; } = "恭喜你，答题的题目全对！";
            public string FullErrorText { get; set; } = "恭喜你，完成题目！哪怕全错，依然鼓励！";
        }

        private void buttonSave1_Click(object sender, EventArgs e)
        {
            string StrTmp = "{\r\n    \"Title\": \"" + textBoxTitle.Text + "\",\r\n";
            StrTmp += "    \"Width\": " + textBoxWidth.Text + ",\r\n";
            StrTmp += "    \"Height\": " + textBoxHeight.Text + ",\r\n\r\n";
            StrTmp += "    \"Icon\": \"" + textBoxIcon.Text + "\",\r\n";
            StrTmp += "    \"BackColor_Red\": " + textBoxBackColor_Red.Text + ",\r\n";
            StrTmp += "    \"BackColor_Green\": " + textBoxBackColor_Green.Text + ",\r\n";
            StrTmp += "    \"BackColor_Blue\": " + textBoxBackColor_Blue.Text + ",\r\n";
            StrTmp += "    \"BackgroundImage\": \"" + textBoxBackgroundImage.Text + "\",\r\n";
            StrTmp += "    \"BackgroundImageLayout\": " + textBoxBackgroundImageLayout.Text + ",\r\n\r\n";
            StrTmp += "    \"buttonA_Width\": " + textBoxbuttonA_Width.Text + ",\r\n";
            StrTmp += "    \"buttonB_Width\": " + textBoxbuttonB_Width.Text + ",\r\n";
            StrTmp += "    \"buttonC_Width\": " + textBoxbuttonC_Width.Text + ",\r\n";
            StrTmp += "    \"buttonD_Width\": " + textBoxbuttonD_Width.Text + ",\r\n";
            StrTmp += "    \"buttonA_Height\": " + textBoxbuttonA_Height.Text + ",\r\n";
            StrTmp += "    \"buttonB_Height\": " + textBoxbuttonB_Height.Text + ",\r\n";
            StrTmp += "    \"buttonC_Height\": " + textBoxbuttonC_Height.Text + ",\r\n";
            StrTmp += "    \"buttonD_Height\": " + textBoxbuttonD_Height.Text + ",\r\n\r\n";
            StrTmp += "    \"BackColor_Option_Red\": " + textBoxBackColor_Option_Red.Text + ",\r\n";
            StrTmp += "    \"BackColor_Option_Green\": " + textBoxBackColor_Option_Green.Text + ",\r\n";
            StrTmp += "    \"BackColor_Option_Blue\": " + textBoxBackColor_Option_Blue.Text + ",\r\n\r\n";
            StrTmp += "    \"QuestionCount\": " + textBoxQuestionCount.Text + ",\r\n";
            StrTmp += "    \"EnableClosingProgramAfterErrorAnswering\": " + checkBoxEnableClosingProgramAfterErrorAnswering.Checked.ToString().ToLower() + ",\r\n\r\n";
            StrTmp += "    \"FinishText\": \"" + textBoxFinishText.Text + "\",\r\n";
            StrTmp += "    \"FullText\": \"" + textBoxFullText.Text + "\",\r\n";
            StrTmp += "    \"FullErrorText\": \"" + textBoxFullErrorText.Text + "\"\r\n";
            StrTmp += "}";

            if (DeBugMode)
                if (MessageBox.Show("代码已生成完成：\n" + StrTmp, "是否导出", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    File.WriteAllText(@"CQAA\Program.json", StrTmp);
                else
                    File.WriteAllText(@"CQAA\Program.json", StrTmp);
        }

        private void buttonSave2_Click(object sender, EventArgs e)
        {
            //if (File.Exists(comboBoxTopics.Text))
            //{
                string StrTmp = "{\r\n    \"Text\": \"" + textBoxText.Text + "\",\r\n\r\n";
                StrTmp += "    \"FamilyName\": \"" + textBoxFamilyName.Text + "\",\r\n";
                StrTmp += "    \"Px\": \"" + textBoxPx.Text + "\",\r\n\r\n";
                StrTmp += "    \"Answer\": \"" + textBoxAnswer.Text + "\",\r\n\r\n";
                StrTmp += "    \"TextRunType\": \"" + textBoxTextRunType.Text + "\",\r\n";
                StrTmp += "    \"TextRunData\": \"" + textBoxTextRunData.Text + "\",\r\n\r\n";
                if (checkBoxType0.Checked)
                {
                    StrTmp += "    \"Type\": 0,\r\n\r\n";
                    StrTmp += "    \"OptionA\": \"" + textBoxOptionA.Text + "\",\r\n";
                    StrTmp += "    \"OptionB\": \"" + textBoxOptionB.Text + "\",\r\n";
                    StrTmp += "    \"OptionC\": \"" + textBoxOptionC.Text + "\",\r\n";
                    StrTmp += "    \"OptionD\": \"" + textBoxOptionD.Text + "\",\r\n";
                    StrTmp += "    \"EnableOptionLeadingText\": " + checkBoxEnableOptionLeadingText.Checked.ToString().ToLower() + ",\r\n\r\n";
                    StrTmp += "    \"CorrectOptionsA\": " + checkBoxCorrectOptionsA.Checked.ToString().ToLower() + ",\r\n";
                    StrTmp += "    \"CorrectOptionsB\": " + checkBoxCorrectOptionsB.Checked.ToString().ToLower() + ",\r\n";
                    StrTmp += "    \"CorrectOptionsC\": " + checkBoxCorrectOptionsC.Checked.ToString().ToLower() + ",\r\n";
                    StrTmp += "    \"CorrectOptionsD\": " + checkBoxCorrectOptionsD.Checked.ToString().ToLower() + ",\r\n\r\n";
                    StrTmp += "    \"OptionButtonA_X\": " + textBoxOptionButtonA_X.Text + ",\r\n";
                    StrTmp += "    \"OptionButtonB_X\": " + textBoxOptionButtonB_X.Text + ",\r\n";
                    StrTmp += "    \"OptionButtonC_X\": " + textBoxOptionButtonC_X.Text + ",\r\n";
                    StrTmp += "    \"OptionButtonD_X\": " + textBoxOptionButtonD_X.Text + ",\r\n";
                    StrTmp += "    \"OptionButtonA_Y\": " + textBoxOptionButtonA_Y.Text + ",\r\n";
                    StrTmp += "    \"OptionButtonB_Y\": " + textBoxOptionButtonB_Y.Text + ",\r\n";
                    StrTmp += "    \"OptionButtonC_Y\": " + textBoxOptionButtonC_Y.Text + ",\r\n";
                    StrTmp += "    \"OptionButtonD_Y\": " + textBoxOptionButtonD_Y.Text + "\r\n";
                }
                StrTmp += "}";

                File.WriteAllText(comboBoxTopics.Text, StrTmp);
                if (DeBugMode)
                    if (MessageBox.Show("代码已生成完成：\n" + StrTmp, "是否导出", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        File.WriteAllText(comboBoxTopics.Text, StrTmp);
                    else
                        File.WriteAllText(comboBoxTopics.Text, StrTmp);



                comboBoxTopics.Items.Clear();
                comboBoxTopics.Items.AddRange(Directory.GetFiles(@"CQAA\topic"));
            //}
        }

        private void comboBoxTopics_SelectedIndexChanged(object sender, EventArgs e) => LoadTopicFile();

        private void buttonLoadFile_Click(object sender, EventArgs e) => LoadTopicFile();
    }

    public class JsonReader
    {
        public static T ReadJsonFile<T>(string filePath)
        {
            var jsonText = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(jsonText);
        }
    }
}
