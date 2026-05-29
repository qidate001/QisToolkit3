using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QisToolkit3.Forms
{
    public partial class TextProcessingTools : Form
    {
        string BoxText = string.Empty;

        public TextProcessingTools()
        {
            InitializeComponent();
            this.TopMost = Qi.QisToolkit3_Datas.ToolsProcessingTools_TopMost;

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void buttonToUppercase_Click(object sender, EventArgs e)
        {
            BoxText = richTextBox.Text;
            string str = string.Empty;

            foreach (char ch in BoxText)
            {
                if (ch >= 'a' && ch <= 'z')
                    str += (char)(ch - 32);
                else
                    str += ch;
            }

            richTextBox.Text = str;
        }

        private void buttonToLowercase_Click(object sender, EventArgs e)
        {
            BoxText = richTextBox.Text;
            string str = string.Empty;

            foreach (char ch in BoxText)
            {
                if (ch >= 'A' && ch <= 'Z')
                    str += (char)(ch + 32);
                else
                    str += ch;
            }

            richTextBox.Text = str;
        }

        private void buttonClearAir_Click(object sender, EventArgs e)
        {
            BoxText = richTextBox.Text;
            string str = string.Empty;

            foreach (char ch in BoxText)
                if (ch != ' ')
                    str += ch;

            richTextBox.Text = str;
        }

        private void buttonClearUnderline_Click(object sender, EventArgs e)
        {
            BoxText = richTextBox.Text;
            string str = string.Empty;

            foreach (char ch in BoxText)
                if (ch != '_')
                    str += ch;

            richTextBox.Text = str;
        }

        private void buttonSpacesToUnderline_Click(object sender, EventArgs e)
        {
            BoxText = richTextBox.Text;
            string str = string.Empty;

            foreach (char ch in BoxText)
            {
                if (ch == ' ')
                    str += '_';
                else
                    str += ch;
            }

            richTextBox.Text = str;
        }

        private void buttonUnderlineToSpaces_Click(object sender, EventArgs e)
        {
            BoxText = richTextBox.Text;
            string str = string.Empty;

            foreach (char ch in BoxText)
            {
                if (ch == '_')
                    str += ' ';
                else
                    str += ch;
            }

            richTextBox.Text = str;
        }

        private void buttonWithdraw_Click(object sender, EventArgs e)
        {
            string str = richTextBox.Text;
            richTextBox.Text = BoxText;
            BoxText = str;
        }

        private void buttonRestore_Click(object sender, EventArgs e)
        {

        }

        private void buttonSpecialCharacterConversion_Click(object sender, EventArgs e)
        {
            BoxText = richTextBox.Text;
            string str = string.Empty;

            for (int i = 0; i < BoxText.Length; ++i)
            {
                // 防止超过长度
                if (i + 1 < BoxText.Length)
                {
                    // 特殊字符前置
                    if (BoxText[i] == '.' && BoxText[i + 1] == '.')
                    {
                        // 特殊字符名
                        string sc = string.Empty;

                        // 获取特殊字符名
                        for (int j = i + 2; j < BoxText.Length; ++j)
                            if (BoxText[j] != '.')
                                sc += BoxText[j];
                            else
                                break;

                        switch (sc.ToLower())
                        {
                            case "n":
                                str += '\n';
                                break;
                            case "r":
                                str += '\r';
                                break;
                            case "t":
                                str += '\t';
                                break;


                            #region 直角引号
                            case "\"":
                                str += "『』";
                                break;
                            case "\'":
                                str += "「」";
                                break;
                            case "‘":
                                str += "「";
                                break;
                            case "’":
                                str += "」";
                                break;
                            case "“":
                                str += "『";
                                break;
                            case "”":
                                str += "』";
                                break;
                            case "-\"":
                                str += "﹃\n﹄";
                                break;
                            case "-\'":
                                str += "﹁\n﹂";
                                break;
                            case "-‘":
                                str += "﹃";
                                break;
                            case "-’":
                                str += "﹄";
                                break;
                            case "-“":
                                str += "﹁";
                                break;
                            case "-”":
                                str += "﹂";
                                break;
                            #endregion


                            case "#":
                                str += '♯';
                                break;
                            case "mn0":
                                str += '♩';
                                break;
                            case "mn1":
                                str += '♪';
                                break;
                            case "mn2":
                                str += '♫';
                                break;
                            case "mn3":
                                str += '♬';
                                break;
                            case "mndm":
                                str += '♭';
                                break;
                            case "mnrs":
                                str += '♮';
                                break;
                            case "seg":
                                str += '§';
                                break;
                            case "pnd":
                                str += '℡';
                                break;
                            case "tel":
                                str += '℡';
                                break;
                            case "em ph":
                                str += '☎';
                                break;
                            case "em ph2":
                                str += '☏';
                                break;
                            case "em ms":
                                str += '☪';
                                break;
                            case "em hs":
                                str += '♨';
                                break;

                            #region 罗马数字 小写
                            case "rs1":
                                str += 'ⅰ';
                                break;
                            case "rs2":
                                str += 'ⅱ';
                                break;
                            case "rs3":
                                str += 'ⅲ';
                                break;
                            case "rs4":
                                str += 'ⅳ';
                                break;
                            case "rs5":
                                str += 'ⅴ';
                                break;
                            case "rs6":
                                str += 'ⅵ';
                                break;
                            case "rs7":
                                str += 'ⅶ';
                                break;
                            case "rs8":
                                str += 'ⅷ';
                                break;
                            case "rs9":
                                str += 'ⅸ';
                                break;
                            case "rs10":
                                str += 'ⅹ';
                                break;
                            case "rs11":
                                str += 'ⅺ';
                                break;
                            case "rs12":
                                str += 'ⅻ';
                                break;
                            #endregion

                            #region 罗马数字 大写
                            case "r1":
                                str += 'Ⅰ';
                                break;
                            case "r2":
                                str += 'Ⅱ';
                                break;
                            case "r3":
                                str += 'Ⅲ';
                                break;
                            case "r4":
                                str += 'Ⅳ';
                                break;
                            case "r5":
                                str += 'Ⅴ';
                                break;
                            case "r6":
                                str += 'Ⅵ';
                                break;
                            case "r7":
                                str += 'Ⅶ';
                                break;
                            case "r8":
                                str += 'Ⅷ';
                                break;
                            case "r9":
                                str += 'Ⅸ';
                                break;
                            case "r10":
                                str += 'Ⅹ';
                                break;
                            case "r11":
                                str += 'Ⅺ';
                                break;
                            case "r12":
                                str += 'Ⅻ';
                                break;
                                #endregion
                        }

                        if (sc == string.Empty)
                            i += 1;
                        else
                            i += sc.Length + 2;
                    }
                    else str += BoxText[i];
                }
                else str += BoxText[i];
            }

            richTextBox.Text = str;
        }

        private void buttonStandingInitialToUppercase_Click(object sender, EventArgs e)
        {
            BoxText = richTextBox.Text;
            string str = string.Empty;
            bool IsHead = true;

            foreach (char ch in BoxText)
            {
                if (ch == ' ' || ch == '_' || ch == '\t' || ch == '\r' || ch == '\n')
                    IsHead = true;
                else if (IsHead && ch >= 'A' && ch <= 'Z')
                    IsHead = false;

                if (IsHead && ch >= 'a' && ch <= 'z')
                {
                    str += (char)(ch - 32);
                    IsHead = false;
                }
                else
                    str += ch;
            }

            richTextBox.Text = str;
        }
    }
}
