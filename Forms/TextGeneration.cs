using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static Qi;

namespace QisToolkit3.Forms
{
    public partial class TextGeneration : Form
    {
        int I = 0, J = 0, K = 0;


        public TextGeneration()
        {
            InitializeComponent();
        }

        private void checkBox_Enabled_I_CheckedChanged(object sender, EventArgs e)
        {
            textBox_ReSet_I.Enabled = checkBox_Enabled_I.Checked;
            comboBox_I.Enabled = checkBox_Enabled_I.Checked;
            checkBox_Roman_I.Enabled = checkBox_Enabled_I.Checked;
            textBox_Do_I.Enabled = !(comboBox_I.Text == "自增一" || comboBox_I.Text == "自减一");
        }

        private void checkBox_Enabled_J_CheckedChanged(object sender, EventArgs e)
        {
            textBox_ReSet_J.Enabled = checkBox_Enabled_J.Checked;
            comboBox_J.Enabled = checkBox_Enabled_J.Checked;
            checkBox_Roman_J.Enabled = checkBox_Enabled_J.Checked;
            textBox_Do_J.Enabled = !(comboBox_J.Text == "自增一" || comboBox_J.Text == "自减一");
        }

        private void checkBox_Enabled_K_CheckedChanged(object sender, EventArgs e)
        {
            textBox_ReSet_K.Enabled = checkBox_Enabled_K.Checked;
            comboBox_K.Enabled = checkBox_Enabled_K.Checked;
            checkBox_Roman_K.Enabled = checkBox_Enabled_K.Checked;
            textBox_Do_K.Enabled = !(comboBox_K.Text == "自增一" || comboBox_K.Text == "自减一");
        }

        private void comboBox_I_SelectedIndexChanged(object sender, EventArgs e) =>
            textBox_Do_I.Enabled = !(comboBox_I.Text == "自增一" || comboBox_I.Text == "自减一");

        private void comboBox_J_SelectedIndexChanged(object sender, EventArgs e) =>
            textBox_Do_J.Enabled = !(comboBox_J.Text == "自增一" || comboBox_J.Text == "自减一");

        private void comboBox_K_SelectedIndexChanged(object sender, EventArgs e) =>
            textBox_Do_K.Enabled = !(comboBox_K.Text == "自增一" || comboBox_K.Text == "自减一");

        private void buttonRun_Click(object sender, EventArgs e)
        {
            richTextBoxMain.Text = richTextBox_Head.Text;
            I = StrToInt(textBox_ReSet_I.Text);
            J = StrToInt(textBox_ReSet_J.Text);
            K = StrToInt(textBox_ReSet_K.Text);

            for (int x = 0; x < StrToInt(textBox_ForCount.Text); ++x)
            {
                foreach (string item in richTextBox3000.Lines)
                    TextMain(NoRN(item));





                if (checkBox_Enabled_I.Checked)
                {
                    if (comboBox_I.Text == "自增一") ++I;
                    else if (comboBox_I.Text == "自减一") --I;
                    else if (comboBox_I.Text == "加等于") I += Qi.StrToInt(textBox_Do_I.Text);
                    else if (comboBox_I.Text == "减等于") I -= Qi.StrToInt(textBox_Do_I.Text);
                    else if (comboBox_I.Text == "乘等于") I *= Qi.StrToInt(textBox_Do_I.Text);
                    else if (comboBox_I.Text == "除等于") I /= Qi.StrToInt(textBox_Do_I.Text);
                }

                if (checkBox_Enabled_J.Checked)
                {
                    if (comboBox_J.Text == "自增一") ++J;
                    else if (comboBox_J.Text == "自减一") --J;
                    else if (comboBox_J.Text == "加等于") J += Qi.StrToInt(textBox_Do_J.Text);
                    else if (comboBox_J.Text == "减等于") J -= Qi.StrToInt(textBox_Do_J.Text);
                    else if (comboBox_J.Text == "乘等于") J *= Qi.StrToInt(textBox_Do_J.Text);
                    else if (comboBox_J.Text == "除等于") J /= Qi.StrToInt(textBox_Do_J.Text);
                }

                if (checkBox_Enabled_K.Checked)
                {
                    if (comboBox_K.Text == "自增一") ++K;
                    else if (comboBox_K.Text == "自减一") --K;
                    else if (comboBox_K.Text == "加等于") K += Qi.StrToInt(textBox_Do_K.Text);
                    else if (comboBox_K.Text == "减等于") K -= Qi.StrToInt(textBox_Do_K.Text);
                    else if (comboBox_K.Text == "乘等于") K *= Qi.StrToInt(textBox_Do_K.Text);
                    else if (comboBox_K.Text == "除等于") K /= Qi.StrToInt(textBox_Do_K.Text);
                }
            }

            richTextBoxMain.Text += richTextBox_End.Text;
        }

        private void TextMain(string Str)
        {
            if (Str.ToLower() == "<i>" && checkBox_Enabled_I.Checked)
            {
                if (checkBox_Roman_I.Checked)
                    richTextBoxMain.Text += Qi.IntToRoman(I).ToString();
                else
                    richTextBoxMain.Text += I.ToString();
            }
            else if (Str.ToLower() == "<j>" && checkBox_Enabled_J.Checked)
            {
                if (checkBox_Roman_J.Checked)
                    richTextBoxMain.Text += Qi.IntToRoman(J).ToString();
                else
                    richTextBoxMain.Text += J.ToString();
            }
            else if (Str.ToLower() == "<k>" && checkBox_Enabled_K.Checked)
            {
                if (checkBox_Roman_K.Checked)
                    richTextBoxMain.Text += Qi.IntToRoman(K).ToString();
                else
                    richTextBoxMain.Text += K.ToString();
            }
            else if (Str.ToLower() == "<r>") richTextBoxMain.Text += "\r";
            else if (Str.ToLower() == "<n>") richTextBoxMain.Text += "\n";
            else if (Str.ToLower() == "<rn>") richTextBoxMain.Text += "\r\n";
            else if (Str != String.Empty)
                richTextBoxMain.Text += Str;
        }

        private void buttonPresets_Click(object sender, EventArgs e)
        {
            richTextBox_Head.Text = "{\r\n    \"enchantment.level.1\": \"I\"";
            richTextBox3000.Text = ",\r\n<rn>\r\n    \"enchantment.level.\r\n<i>\r\n\": \"\r\n<j>\r\n\"";
            richTextBox_End.Text = "\r\n}";
            checkBox_Enabled_I.Checked = true;
            checkBox_Enabled_J.Checked = true;
            checkBox_Enabled_K.Checked = false;
            checkBox_Roman_I.Checked = false;
            checkBox_Roman_J.Checked = true;
            checkBox_Roman_K.Checked = false;
            textBox_ForCount.Text = "254";
            textBox_ReSet_I.Text = "2";
            textBox_ReSet_J.Text = "2";
            comboBox_I.Text = "自增一";
            comboBox_J.Text = "自增一";
            textBox_Do_I.Enabled = false;
            textBox_Do_J.Enabled = false;
            textBox_Do_K.Enabled = false;
        }

        private void button_PlusMode_Click(object sender, EventArgs e) => new TextGenerationPlus().Show();

        private void TextGeneration_Load(object sender, EventArgs e)
        {

        }
    }
}
