using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Qi;

namespace QisToolkit3.Forms.SoftwareFunctionForms
{
    public partial class WeChatFunction : Form
    {
        bool NowGetData = true;
        public WeChatFunction()
        {
            NowGetData = true;
            InitializeComponent();
            checkBox_NoWeChat.Checked = EDIFEO("Weixin.exe");
            checkBox_NoWeChatAppEx.Checked = EDIFEO("WeChatAppEx.exe") || EDIFEO("WeixinExt.exe");
            checkBox_WeixinUpdate.Checked = EDIFEO("WeixinUpdate.exe");
            checkBox_NoWetypeInstaller.Checked = EDIFEO("WetypeInstaller.exe");
            NowGetData = false;
        }

        private void checkBox_NoWeChatAppEx_CheckedChanged(object sender, EventArgs e)
        {
            if (NowGetData)
                return;

            IFEO(checkBox_NoWeChatAppEx.Checked, "WeChatAppEx.exe");
            IFEO(checkBox_NoWeChatAppEx.Checked, "WeixinExt.exe");
        }

        private void checkBox_WeixinUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (NowGetData)
                return;

            IFEO(checkBox_WeixinUpdate.Checked, "WeixinUpdate.exe");
        }

        private void WeChatFunction_Load(object sender, EventArgs e)
        {

        }

        private void checkBox_NoWetypeInstaller_CheckedChanged(object sender, EventArgs e)
        {
            if (NowGetData)
                return;

            IFEO(checkBox_NoWetypeInstaller.Checked, "WetypeInstaller.exe");
        }

        private void checkBox_NoWeChat_CheckedChanged(object sender, EventArgs e)
        {
            if (NowGetData)
                return;

            IFEO(checkBox_NoWeChat.Checked, "Weixin.exe");
        }
    }
}
