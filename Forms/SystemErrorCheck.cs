using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Qi;

namespace QisToolkit3.Forms
{
    public partial class SystemErrorCheck : Form
    {
        private readonly List<ISystemCheck> _checks = new()
        {
            new CtfMonitorCheck(),
            new SystemProfileCheck(),
            new SystemRegPoliciesDisable()
        };

        public SystemErrorCheck()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            checkedListBox.Items.Clear();

            foreach (var check in _checks)
            {
                if (check.Check())
                {
                    checkedListBox.Items.Add(check.Name, true);
                }
            }

            buttonRepair.Enabled = checkedListBox.Items.Count > 0;

            if (checkedListBox.Items.Count == 0)
            {
                MessageBox.Show("恭喜您，您的电脑什么问题都没有！", "提示", MessageBoxButtons.OK);
            }
        }

        private void buttonRepair_Click(object sender, EventArgs e)
        {
            buttonRepair.Enabled = false;

            foreach (var check in _checks)
            {
                if (check.Check()) // 如果检查失败才修复
                {
                    check.Repair();
                }
            }

            RestartExplorer();
            MessageBox.Show("修复完成！", "提示", MessageBoxButtons.OK);
            checkedListBox.Items.Clear();
        }

        private void checkedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox.SelectedItem is string selectedName)
            {
                var check = _checks.FirstOrDefault(c => c.Name == selectedName);
                if (check != null)
                {
                    label_ProblemInterpretation.Text = $"{check.Name}：\n\n{check.Description}";
                }
            }
        }
    }
}
