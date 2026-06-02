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
    public partial class StrangeQuestionAndAnswer : Form
    {
        private string DataDirPath = string.Empty;
        private string DataFilePath = string.Empty;

        public StrangeQuestionAndAnswer()
        {
            InitializeComponent();

            DataDirPath = Path.Combine(
                Qi.QisToolkit3_Datas.actualDirectory,
                "Datas",
                "StrangeQuestionAndAnswer"
            );

            DataFilePath = Path.Combine(
                DataDirPath,
                "data.qidata"
            );
        }

        private void StrangeQuestionAndAnswer_Load(object sender, EventArgs e)
        {
            Directory.CreateDirectory(DataDirPath);
            labelWinCount_Click(null, null);
        }

        private void buttonStart_Click(object sender, EventArgs e) => new StrangeQuestionAndAnswerMain().Show();

        private void labelWinCount_Click(object sender, EventArgs e)
        {
            if (File.Exists(DataFilePath))
            {
                labelWinCount.Text = Qi.GetLanguage() switch
                {
                    "en" => $"You have cleared {File.ReadAllText(DataFilePath)} times",
                    "zh-CN" => $"您已通关了 {File.ReadAllText(DataFilePath)} 次",
                    _ => $"您已通关了 {File.ReadAllText(DataFilePath)} 次"
                };
            }
        }
    }
}
