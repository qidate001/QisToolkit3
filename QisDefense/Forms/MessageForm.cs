using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QisDefense.Forms
{
    public partial class MessageForm : Form
    {
        // 添加API声明
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOSIZE = 0x0001;

        private string Type = "None";

        public string UserChoice { get; private set; } // 存储用户选择

        public MessageForm(string type = "None", string Title = "Auto", string Text = "Auto")
        {
            InitializeComponent();
            TopMost = true;

            // 同步TYPE
            Type = type;

            
            switch (type)
            {
                case "Exit":
                    labelTitle.Text = "关闭齐之防御";
                    labelText.Text = "关闭后快捷键等将无法使用。";
                    break;

                default:
                    labelTitle.Text = Title;
                    labelText.Text = Text;
                    break;
            }
        }

        private void MessageForm_Load(object sender, EventArgs e)
        {
            base.OnShown(e);

            // 强制置顶（Windows API方式）
            SetWindowPos(Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);

            // 再次激活窗口（确保获得焦点）
            Activate();
            BringToFront();
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            UserChoice = "No"; // 用户点击"否"
            DialogResult = DialogResult.Cancel; // 关闭窗口并返回 Cancel
            Close();
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            UserChoice = "Yes"; // 用户点击"是"
            DialogResult = DialogResult.OK; // 关闭窗口并返回 OK


            switch (Type)
            {
                case "None":
                    Close();
                    break;

                case "Exit":
                    Environment.Exit(0);
                    break;

                default:
                    Close();
                    break;
            }
        }
    }
}


public enum UserResponse
{
    Yes,
    No,
    Cancel
}
