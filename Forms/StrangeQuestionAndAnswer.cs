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
        public StrangeQuestionAndAnswer()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e) => new StrangeQuestionAndAnswerMain().Show();

        private void labelWinCount_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\QiAppDatas\Datas\QisToolkit3\StrangeuestionAndAnswer.qidata"))
                labelWinCount.Text = "您已通关了 " + File.ReadAllText(@"C:\QiAppDatas\Datas\QisToolkit3\StrangeuestionAndAnswer.qidata") + " 次";
        }

        private void StrangeQuestionAndAnswer_Load(object sender, EventArgs e)
        {
            Directory.CreateDirectory(@"C:\QiAppDatas\Datas\QisToolkit3");
            if (File.Exists(@"C:\QiAppDatas\Datas\QisToolkit3\StrangeuestionAndAnswer.qidata"))
                labelWinCount.Text = "您已通关了 " + File.ReadAllText(@"C:\QiAppDatas\Datas\QisToolkit3\StrangeuestionAndAnswer.qidata") + " 次";
        }
    }
}
