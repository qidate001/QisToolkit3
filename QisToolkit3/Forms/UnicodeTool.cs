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
    public partial class UnicodeTool : Form
    {
        private string lastClipboardContent = "";


        public UnicodeTool()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void UnicodeTool_Load(object sender, EventArgs e)
        {

        }

        private void buttonGetClipboardData_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    string clipboardText = Clipboard.GetText();
                    labelClipboardUnicode.Text = clipboardText;

                    // 转换文本为Unicode代码
                    string unicodeCodes = ConvertTextToUnicode(clipboardText);
                    labelClipboardUnicode.Text = unicodeCodes;
                }
                else
                {
                    labelClipboardUnicode.Text = "";
                    MessageBox.Show("剪贴板中没有文本内容！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"程序出错了！\n报错信息: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ConvertTextToUnicode(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "无内容";

            StringBuilder unicodeBuilder = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                // 处理代理对（surrogate pairs）字符（如emoji）
                if (char.IsHighSurrogate(text[i]) && i < text.Length - 1 && char.IsLowSurrogate(text[i + 1]))
                {
                    int codePoint = char.ConvertToUtf32(text, i);
                    unicodeBuilder.Append($"U+{codePoint:X4} ");
                    i++; // 跳过代理对中的第二个字符
                }
                else
                {
                    unicodeBuilder.Append($"U+{(int)text[i]:X4} ");
                }
            }

            return unicodeBuilder.ToString().Trim();
        }

        private void labelClipboardUnicode_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(labelClipboardUnicode.Text);
            MessageBox.Show("已添加至剪切板！");
        }

        private void buttonGetUnicodeData_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Qi.GetUnicodeCharacter(richTextBoxClipboardUnicode.Text));
            MessageBox.Show("已添加至剪切板！");
        }

        private void richTextBoxClipboardUnicode_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

