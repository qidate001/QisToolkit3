using QisToolkit3.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Qi;
using static Qi.QisToolkit3_Datas;

namespace QisToolkit3
{
    public partial class Main : Form
    {
        private Button currentButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;
        //private Dictionary<Type, Form> cachedForms = new Dictionary<Type, Form>();

        public Main()
        {
            // 设置全局异常处理
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                Log.Fatal("未处理的异常 ", ex);
                BufferedLogger.Shutdown();
            };

            // 窗口事件记录
            this.Closed += (s, e) =>
            {
                Log.Info("主窗口已被关闭。");
                BufferedLogger.Shutdown();
            };

            // 本土化设置
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(GetLanguage());

            // 初始化控件
            InitializeComponent();
            Log.Info("主窗口已初始化。");
            OpenChildForm(new Home(), null);

            SetDarkMode();

            // 小白模式
            if (ComputerNoviceMode)
            {
                Log.Info($"小白模式已开启，部分功能已被隐藏。");
                buttonTextGeneration.Visible = false;
            }
        }

        private void SetDarkMode()
        {
            if (DarkMode)
            {
                panelDesktopPane.BackColor = Color.FromArgb(23, 23, 23);
                panelTitleBar.BackColor = Color.FromArgb(51, 51, 73);
                Log.Info($"设置深色调模式开启");
            }
        }

        private Color SelectThemeColor()
        {
            int index = random.Next(ThemeColor.ColorList.Count);
            while (tempIndex == index)
                random.Next(ThemeColor.ColorList.Count);

            tempIndex = index;
            string color = ThemeColor.ColorList[index];
            return ColorTranslator.FromHtml(color);
        }

        private void ActivateButton(object btnSender)
        {
            if (btnSender == null)
            {
                if (currentButton != (Button)btnSender)
                {
                    Color color = SelectThemeColor();
                    currentButton = (Button)btnSender;
                    currentButton.BackColor = color;
                    currentButton.ForeColor = Color.White;
                    currentButton.Font = new Font("Microsoft YaHei UI", 12.5F);
                }
            }
        }

        //private void DisableButton()
        //{
        //    foreach (Console previousBtn in panelMenu.Controls)
        //    {
        //        if (previousBtn.GetType() == typeof(Button))
        //        {
        //            previousBtn.BackColor = Color.FormArgb(51, 51, 76);
        //            previousBtn.ForeColor = Color.Gainsboro;
        //        }
        //    }
        //}

        private void OpenChildForm(Form childForm, object btnSender)
        {
            // 如果是要打开同一个窗体，不做处理
            if (activeForm != null && activeForm.GetType() == childForm.GetType())
                return;

            if (activeForm != null)
                activeForm.Close();

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktopPane.Controls.Add(childForm);
            panelDesktopPane.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            labelTitle.Text = childForm.Text;

            // 日志
            Log.Info($"已启动子窗口 {childForm.Text}");
        }
        //private void OpenChildForm(Form childForm, object btnSender)
        //{
        //    // 如果是要打开同一个窗体，不做处理
        //    if (activeForm != null && activeForm.GetType() == childForm.GetType())
        //        return;

        //    // 隐藏当前窗体而不是关闭
        //    if (activeForm != null)
        //    {
        //        //activeForm.Hide();
        //        activeForm.Close();
        //        activeForm.Dispose();
        //    }

        //    // 检查是否已缓存该窗体
        //    Type formType = childForm.GetType();
        //    if (cachedForms.ContainsKey(formType))
        //    {
        //        activeForm = cachedForms[formType];
        //        activeForm.Show();
        //    }
        //    else
        //    {
        //        childForm.TopLevel = false;
        //        childForm.FormBorderStyle = FormBorderStyle.None;
        //        childForm.Dock = DockStyle.Fill;
        //        panelDesktopPane.Controls.Add(childForm);
        //        panelDesktopPane.Tag = childForm;
        //        childForm.BringToFront();
        //        childForm.Show();
        //        activeForm = childForm;
        //        cachedForms[formType] = childForm;
        //    }

        //    labelTitle.Text = activeForm.Text;
        //    Log.Info($"已切换到子窗口 {activeForm.Text}");
        //}

        private void buttonDoFiles_Click(object sender, EventArgs e) => OpenChildForm(new FilesOperation(), sender);

        private void buttonTools_Click(object sender, EventArgs e) => OpenChildForm(new Tools(), sender);

        private void buttonTextGeneration_Click(object sender, EventArgs e) => OpenChildForm(new TextGeneration(), sender);

        private void buttonOptions_Click(object sender, EventArgs e) => OpenChildForm(new Options(), sender);

        private void buttonMinecraft_Click(object sender, EventArgs e) => OpenChildForm(new GameTools(), sender);

        private void buttonStrangeQuestionAndAnswer_Click(object sender, EventArgs e) => OpenChildForm(new StrangeQuestionAndAnswer(), sender);

        private void buttonCleaningUpTrash_Click(object sender, EventArgs e) => OpenChildForm(new CleaningUpTrash(), sender);

        private void label_Click(object sender, EventArgs e) => OpenChildForm(new Home(), sender);

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveDatas();
        }
    }
}