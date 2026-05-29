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
    public partial class MinecraftTools : Form
    {
        public MinecraftTools()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);
        }
    }
}
