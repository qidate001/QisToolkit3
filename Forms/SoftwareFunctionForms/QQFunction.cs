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

namespace QisToolkit3.Forms.SoftwareFunctionForms
{
    public partial class QQFunction : Form
    {
        bool NowGetData = true;
        public QQFunction()
        {
            NowGetData = true;
            InitializeComponent();
            checkBox_NoQQ.Checked = EDIFEO("QQ.exe");
            checkBox_NoQQScreenshot.Checked = EDIFEO("QQScreenshot.exe");
            NowGetData = false;
        }

        private void checkBox_NoQQScreenshot_CheckedChanged(object sender, EventArgs e)
        {
            if (NowGetData)
                return;

            IFEO(checkBox_NoQQScreenshot.Checked, "QQScreenshot.exe");
        }

        private void checkBox_NoQQ_CheckedChanged(object sender, EventArgs e)
        {
            if (NowGetData)
                return;

            IFEO(checkBox_NoQQ.Checked, "QQ.exe");
        }
    }
}
