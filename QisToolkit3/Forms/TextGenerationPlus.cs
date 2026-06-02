using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Qi;
using static System.Net.Mime.MediaTypeNames;

namespace QisToolkit3.Forms
{
    public partial class TextGenerationPlus : Form
    {
        int I = 0, J = 0, K = 0, X = 0, Y = 0;

        public TextGenerationPlus()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            richTextBoxMain.Text = string.Empty;

            I = StrToInt(textBox_ReSet_I.Text);
            J = StrToInt(textBox_ReSet_J.Text);
            K = StrToInt(textBox_ReSet_K.Text);
            X = 0; Y = 0;

            foreach (string item in richTextBox_Head.Lines)
                TextMain(NoRN(item));

            // ASCLL 防溢出
            if (checkBox_ASCLL_I.Checked && StrToInt(textBox_ReSet_I.Text) > 255)
                textBox_ReSet_I.Text = "255";

            if (checkBox_ASCLL_J.Checked && StrToInt(textBox_ReSet_J.Text) > 255)
                textBox_ReSet_J.Text = "255";

            if (checkBox_ASCLL_K.Checked && StrToInt(textBox_ReSet_K.Text) > 255)
                textBox_ReSet_K.Text = "255";

            for (; X < StrToInt(textBox_ForCount.Text); ++X)
            {
                foreach (string item in richTextBox3000.Lines)
                    TextMain(NoRN(item));





                if (checkBox_Enabled_I.Checked)
                {
                    if (comboBox_I.Text == "自增一") ++I;
                    else if (comboBox_I.Text == "自减一") --I;
                    else if (comboBox_I.Text == "加等于") I += StrToInt(textBox_Do_I.Text);
                    else if (comboBox_I.Text == "减等于") I -= StrToInt(textBox_Do_I.Text);
                    else if (comboBox_I.Text == "乘等于") I *= StrToInt(textBox_Do_I.Text);
                    else if (comboBox_I.Text == "除等于") I /= StrToInt(textBox_Do_I.Text);

                    if (checkBox_ASCLL_I.Checked && I > 255)
                        I = 255;
                }

                if (checkBox_Enabled_J.Checked)
                {
                    if (comboBox_J.Text == "自增一") ++J;
                    else if (comboBox_J.Text == "自减一") --J;
                    else if (comboBox_J.Text == "加等于") J += StrToInt(textBox_Do_J.Text);
                    else if (comboBox_J.Text == "减等于") J -= StrToInt(textBox_Do_J.Text);
                    else if (comboBox_J.Text == "乘等于") J *= StrToInt(textBox_Do_J.Text);
                    else if (comboBox_J.Text == "除等于") J /= StrToInt(textBox_Do_J.Text);

                    if (checkBox_ASCLL_J.Checked && J > 255)
                        J = 255;
                }

                if (checkBox_Enabled_K.Checked)
                {
                    if (comboBox_K.Text == "自增一") ++K;
                    else if (comboBox_K.Text == "自减一") --K;
                    else if (comboBox_K.Text == "加等于") K += StrToInt(textBox_Do_K.Text);
                    else if (comboBox_K.Text == "减等于") K -= StrToInt(textBox_Do_K.Text);
                    else if (comboBox_K.Text == "乘等于") K *= StrToInt(textBox_Do_K.Text);
                    else if (comboBox_K.Text == "除等于") K /= StrToInt(textBox_Do_K.Text);

                    if (checkBox_ASCLL_K.Checked && K > 255)
                        K = 255;
                }
            }

            foreach (string item in richTextBox_End.Lines)
                TextMain(NoRN(item));
        }

        private void TextMain(string Str)
        {
            if (Str.ToLower() == "<i>" && checkBox_Enabled_I.Checked)
            {
                if (checkBox_Roman_I.Checked)
                    richTextBoxMain.Text += IntToRoman(I).ToString();
                else if (checkBox_ASCLL_I.Checked)
                    richTextBoxMain.Text += ((Char)I).ToString();
                else
                    richTextBoxMain.Text += I.ToString();
            }
            else if (Str.ToLower() == "<j>" && checkBox_Enabled_J.Checked)
            {
                if (checkBox_Roman_J.Checked)
                    richTextBoxMain.Text += IntToRoman(J).ToString();
                else if (checkBox_ASCLL_J.Checked)
                    richTextBoxMain.Text += ((Char)J).ToString();
                else
                    richTextBoxMain.Text += J.ToString();
            }
            else if (Str.ToLower() == "<k>" && checkBox_Enabled_K.Checked)
            {
                if (checkBox_Roman_K.Checked)
                    richTextBoxMain.Text += IntToRoman(K).ToString();
                else if (checkBox_ASCLL_K.Checked)
                    richTextBoxMain.Text += ((Char)K).ToString();
                else
                    richTextBoxMain.Text += K.ToString();
            }
            else if (Str.ToLower() == "<x>" && checkBox_Enabled_X.Checked)
            {
                if (textBox_MultipleForm_X.Text.ToLower() == "<i>" && I % StrToInt(textBox_Multiple_X.Text) == 0)
                    foreach (string item in textBox_X_True.Lines)
                        TextMain(NoRN(item));
                else if (textBox_MultipleForm_X.Text.ToLower() == "<j>" && J % StrToInt(textBox_Multiple_X.Text) == 0)
                    foreach (string item in textBox_X_True.Lines)
                        TextMain(NoRN(item));
                else if (textBox_MultipleForm_X.Text.ToLower() == "<k>" && K % StrToInt(textBox_Multiple_X.Text) == 0)
                    foreach (string item in textBox_X_True.Lines)
                        TextMain(NoRN(item));
                else if (textBox_MultipleForm_X.Text.ToLower() == "<class>" && X % StrToInt(textBox_Multiple_X.Text) == 0)
                    foreach (string item in textBox_X_True.Lines)
                        TextMain(NoRN(item));
                else
                    foreach (string item in textBox_X_False.Lines)
                        TextMain(NoRN(item));
            }
            else if (Str.ToLower() == "<y>" && checkBox_Enabled_Y.Checked)
            {
                if (Y < richTextBox_Y.Lines.Count())
                    TextMain(NoRN(richTextBox_Y.Lines[Y++]));
            }
            else if (Str.ToLower() == "<r>") richTextBoxMain.Text += "\r";
            else if (Str.ToLower() == "<n>") richTextBoxMain.Text += "\n";
            else if (Str.ToLower() == "<rn>") richTextBoxMain.Text += "\r\n";
            else if (Str.ToLower() == "<t>") richTextBoxMain.Text += "\t";
            else if (Str != String.Empty)
                richTextBoxMain.Text += Str;















        }

        private void checkBox_Enabled_I_CheckedChanged(object sender, EventArgs e)
        {
            textBox_ReSet_I.Enabled = checkBox_Enabled_I.Checked;
            comboBox_I.Enabled = checkBox_Enabled_I.Checked;
            checkBox_Roman_I.Enabled = checkBox_Enabled_I.Checked;
            checkBox_Roman_J.Enabled = checkBox_Enabled_I.Checked;
            textBox_Do_I.Enabled = !(comboBox_I.Text == "自增一" || comboBox_I.Text == "自减一");
        }

        private void checkBox_Enabled_J_CheckedChanged(object sender, EventArgs e)
        {
            textBox_ReSet_J.Enabled = checkBox_Enabled_J.Checked;
            comboBox_J.Enabled = checkBox_Enabled_J.Checked;
            checkBox_Roman_J.Enabled = checkBox_Enabled_J.Checked;
            checkBox_ASCLL_J.Enabled = checkBox_Enabled_K.Checked;
            textBox_Do_J.Enabled = !(comboBox_J.Text == "自增一" || comboBox_J.Text == "自减一");
        }

        private void checkBox_Enabled_K_CheckedChanged(object sender, EventArgs e)
        {
            textBox_ReSet_K.Enabled = checkBox_Enabled_K.Checked;
            comboBox_K.Enabled = checkBox_Enabled_K.Checked;
            checkBox_Roman_K.Enabled = checkBox_Enabled_K.Checked;
            checkBox_ASCLL_K.Enabled = checkBox_Enabled_K.Checked;
            textBox_Do_K.Enabled = !(comboBox_K.Text == "自增一" || comboBox_K.Text == "自减一");
        }

        private void comboBox_I_SelectedIndexChanged(object sender, EventArgs e) =>
            textBox_Do_I.Enabled = !(comboBox_I.Text == "自增一" || comboBox_I.Text == "自减一");

        private void checkBox_Enabled_Y_CheckedChanged(object sender, EventArgs e) =>
            richTextBox_Y.Enabled = checkBox_Enabled_Y.Checked;

        private void checkBox_Enabled_X_CheckedChanged(object sender, EventArgs e)
        {
            textBox_Multiple_X.Enabled = checkBox_Enabled_X.Checked;
            textBox_MultipleForm_X.Enabled = checkBox_Enabled_X.Checked;
            textBox_X_True.Enabled = checkBox_Enabled_X.Checked;
            textBox_X_False.Enabled = checkBox_Enabled_X.Checked;
        }

        private void comboBox_J_SelectedIndexChanged(object sender, EventArgs e) =>
            textBox_Do_J.Enabled = !(comboBox_J.Text == "自增一" || comboBox_J.Text == "自减一");

        private void comboBox_K_SelectedIndexChanged(object sender, EventArgs e) =>
            textBox_Do_K.Enabled = !(comboBox_K.Text == "自增一" || comboBox_K.Text == "自减一");

        private void checkBox_Roman_I_CheckedChanged(object sender, EventArgs e) =>
            checkBox_ASCLL_I.Enabled = !checkBox_Roman_I.Checked;

        private void checkBox_Roman_J_CheckedChanged(object sender, EventArgs e) =>
            checkBox_ASCLL_J.Enabled = !checkBox_Roman_J.Checked;

        private void checkBox_Roman_K_CheckedChanged(object sender, EventArgs e) =>
            checkBox_ASCLL_K.Enabled = !checkBox_Roman_K.Checked;

        private void checkBox_ASCLL_I_CheckedChanged(object sender, EventArgs e) =>
            checkBox_Roman_I.Enabled = !checkBox_ASCLL_I.Checked;

        private void checkBox_ASCLL_J_CheckedChanged(object sender, EventArgs e) =>
            checkBox_Roman_J.Enabled = !checkBox_ASCLL_J.Checked;

        private void checkBox_ASCLL_K_CheckedChanged(object sender, EventArgs e) =>
            checkBox_Roman_K.Enabled = !checkBox_ASCLL_K.Checked;

        private void button_SaveFile_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 获取用户选择的文件路径
                string[] selectedFiles = saveFileDialog.FileNames;
                foreach (string file in selectedFiles)
                    File.WriteAllText(file, richTextBoxMain.Text);
            }
        }

        private void button_Presets1_Click(object sender, EventArgs e)
        {
            richTextBox_Head.Text = "----------------------------------------------------------------------------------\r\n<rn>";
            richTextBox_End.Text = "----------------------------------------------------------------------------------";
            checkBox_Enabled_I.Checked = true;
            checkBox_Enabled_J.Checked = true;
            checkBox_Enabled_K.Checked = false;
            checkBox_Enabled_X.Checked = true;
            checkBox_ASCLL_I.Checked = false;
            checkBox_ASCLL_J.Checked = false;
            checkBox_Roman_I.Checked = false;
            checkBox_Roman_J.Checked = false;
            textBox_ReSet_I.Text = "6";
            textBox_Multiple_X.Text = "5";
            textBox_MultipleForm_X.Text = "<i>";
            textBox_X_True.Text = "<rn>";
            textBox_X_False.Text = "<t>\r\n<t>";
            textBox_ForCount.Text = "100";
            richTextBox3000.Text = "<j>\r\n<x>";
        }

        private void button_Presets2_Click(object sender, EventArgs e)
        {
            checkBox_Enabled_I.Checked = true;
            checkBox_Enabled_J.Checked = true;
            checkBox_Enabled_K.Checked = false;
            checkBox_Enabled_X.Checked = false;
            textBox_ReSet_I.Text = "2";
            textBox_ReSet_J.Text = "2";
            checkBox_ASCLL_I.Checked = false;
            checkBox_ASCLL_J.Checked = false;
            checkBox_Roman_I.Checked = false;
            checkBox_Roman_J.Checked = true;
            textBox_ForCount.Text = "19";
            richTextBox_Head.Text = "{\r\n<rn>\r\n<t>\r\n\"Data1\": {\r\n<rn>\r\n<t>\r\n<t>\r\n\"Level\": 1,\r\n<rn>\r\n<t>\r\n<t>\r\n\"Text\": \"I\"\r\n<rn>\r\n<t>\r\n\r\n}";
            richTextBox_End.Text = "<rn>\r\n}";
            richTextBox3000.Text = ",\r\n<rn>\r\n<t>\r\n\"Data\r\n<i>\r\n\": {\r\n<rn>\r\n<t>\r\n<t>\r\n\"Level" +
                "" +
                "\": \r\n<i>\r\n,\r\n<rn>\r\n<t>\r\n<t>\r\n\"Text\": \"\r\n<j>\r\n\"\r\n<rn>\r\n<t>\r\n}";
        }

        private void button_Copy_Click(object sender, EventArgs e) =>
            Clipboard.SetText(richTextBoxMain.Text);
    }
}
