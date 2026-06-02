using QisToolkit3.Forms.SoftwareFunctionForms;
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
    public partial class SoftwareFunctionPage : Form
    {
        public SoftwareFunctionPage()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void button_WeChat_Click(object sender, EventArgs e)
        {
            new WeChatFunction().Show();
        }

        private void buttonQQ_Click(object sender, EventArgs e)
        {
            new QQFunction().Show();
        }

        private void buttonPCL_Click(object sender, EventArgs e)
        {
            new PCLFunction().Show();
        }
    }
}
