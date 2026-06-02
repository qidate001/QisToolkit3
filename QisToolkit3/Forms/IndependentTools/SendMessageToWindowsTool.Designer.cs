namespace QisToolkit3.Forms
{
    partial class SendMessageToWindowsTool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            button_SendMessage = new Button();
            comboBox_FindWindowhWnd = new ComboBox();
            comboBox_Msg = new ComboBox();
            label2 = new Label();
            splitContainer1 = new SplitContainer();
            button_GetNoVisibleWindows = new Button();
            button_GetOnEnumWindowsAPI = new Button();
            button_FindWindowOnText = new Button();
            comboBox_FindWindowText = new ComboBox();
            label3 = new Label();
            button_FindWindow = new Button();
            richTextBox_hWnd = new RichTextBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            richTextBox_wParam = new RichTextBox();
            tabPage2 = new TabPage();
            richTextBox_lParam = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 16F);
            label1.Location = new Point(12, 57);
            label1.Name = "label1";
            label1.Size = new Size(285, 35);
            label1.TabIndex = 1;
            label1.Text = "由窗口类名获取句柄：";
            // 
            // button_SendMessage
            // 
            button_SendMessage.Anchor = AnchorStyles.Bottom;
            button_SendMessage.Font = new Font("Microsoft YaHei UI", 24F);
            button_SendMessage.Location = new Point(12, 785);
            button_SendMessage.Name = "button_SendMessage";
            button_SendMessage.Size = new Size(228, 65);
            button_SendMessage.TabIndex = 2;
            button_SendMessage.Text = "发送消息";
            button_SendMessage.UseVisualStyleBackColor = true;
            button_SendMessage.Click += button_SendMessage_Click;
            // 
            // comboBox_FindWindowhWnd
            // 
            comboBox_FindWindowhWnd.Font = new Font("Microsoft YaHei UI", 16F);
            comboBox_FindWindowhWnd.FormattingEnabled = true;
            comboBox_FindWindowhWnd.Items.AddRange(new object[] { "Notepad", "ConsoleWindowClass" });
            comboBox_FindWindowhWnd.Location = new Point(277, 55);
            comboBox_FindWindowhWnd.Name = "comboBox_FindWindowhWnd";
            comboBox_FindWindowhWnd.Size = new Size(179, 43);
            comboBox_FindWindowhWnd.TabIndex = 3;
            comboBox_FindWindowhWnd.Text = "Notepad";
            // 
            // comboBox_Msg
            // 
            comboBox_Msg.Font = new Font("Microsoft YaHei UI", 16F);
            comboBox_Msg.FormattingEnabled = true;
            comboBox_Msg.Items.AddRange(new object[] { "0x0010 (WM_CLOSE)", "0x0011 (WM_QUERYENDSESSION)", "0x0016 (WM_ENDSESSION)", "0x000C (WM_SETTEXT)", "0x0111 (WM_COMMAND)" });
            comboBox_Msg.Location = new Point(152, 6);
            comboBox_Msg.Name = "comboBox_Msg";
            comboBox_Msg.Size = new Size(377, 43);
            comboBox_Msg.TabIndex = 5;
            comboBox_Msg.Text = "0x0010 (WM_CLOSE)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 16F);
            label2.Location = new Point(12, 9);
            label2.Name = "label2";
            label2.Size = new Size(150, 35);
            label2.TabIndex = 4;
            label2.Text = "消息类型：";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(button_GetNoVisibleWindows);
            splitContainer1.Panel1.Controls.Add(button_GetOnEnumWindowsAPI);
            splitContainer1.Panel1.Controls.Add(button_FindWindowOnText);
            splitContainer1.Panel1.Controls.Add(comboBox_FindWindowText);
            splitContainer1.Panel1.Controls.Add(label3);
            splitContainer1.Panel1.Controls.Add(button_FindWindow);
            splitContainer1.Panel1.Controls.Add(richTextBox_hWnd);
            splitContainer1.Panel1.Controls.Add(comboBox_Msg);
            splitContainer1.Panel1.Controls.Add(comboBox_FindWindowhWnd);
            splitContainer1.Panel1.Controls.Add(button_SendMessage);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(label2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tabControl1);
            splitContainer1.Size = new Size(1376, 862);
            splitContainer1.SplitterDistance = 542;
            splitContainer1.TabIndex = 10;
            // 
            // button_GetNoVisibleWindows
            // 
            button_GetNoVisibleWindows.Font = new Font("Microsoft YaHei UI", 12F);
            button_GetNoVisibleWindows.Location = new Point(269, 156);
            button_GetNoVisibleWindows.Name = "button_GetNoVisibleWindows";
            button_GetNoVisibleWindows.Size = new Size(260, 42);
            button_GetNoVisibleWindows.TabIndex = 12;
            button_GetNoVisibleWindows.Text = "获取所有可见窗口句柄";
            button_GetNoVisibleWindows.UseVisualStyleBackColor = true;
            button_GetNoVisibleWindows.Click += button_GetNoVisibleWindows_Click;
            // 
            // button_GetOnEnumWindowsAPI
            // 
            button_GetOnEnumWindowsAPI.Font = new Font("Microsoft YaHei UI", 12F);
            button_GetOnEnumWindowsAPI.Location = new Point(12, 156);
            button_GetOnEnumWindowsAPI.Name = "button_GetOnEnumWindowsAPI";
            button_GetOnEnumWindowsAPI.Size = new Size(251, 42);
            button_GetOnEnumWindowsAPI.TabIndex = 11;
            button_GetOnEnumWindowsAPI.Text = "获取所有顶层窗口句柄";
            button_GetOnEnumWindowsAPI.UseVisualStyleBackColor = true;
            button_GetOnEnumWindowsAPI.Click += button_GetOnEnumWindowsAPI_Click;
            // 
            // button_FindWindowOnText
            // 
            button_FindWindowOnText.Font = new Font("Microsoft YaHei UI", 12F);
            button_FindWindowOnText.Location = new Point(462, 106);
            button_FindWindowOnText.Name = "button_FindWindowOnText";
            button_FindWindowOnText.Size = new Size(67, 42);
            button_FindWindowOnText.TabIndex = 10;
            button_FindWindowOnText.Text = "获取";
            button_FindWindowOnText.UseVisualStyleBackColor = true;
            button_FindWindowOnText.Click += button_FindWindowOnText_Click;
            // 
            // comboBox_FindWindowText
            // 
            comboBox_FindWindowText.Font = new Font("Microsoft YaHei UI", 16F);
            comboBox_FindWindowText.FormattingEnabled = true;
            comboBox_FindWindowText.Items.AddRange(new object[] { "命令提示符", "C:\\WINDOWS\\system32\\cmd.exe", "选择 C:\\WINDOWS\\system32\\cmd.exe" });
            comboBox_FindWindowText.Location = new Point(277, 107);
            comboBox_FindWindowText.Name = "comboBox_FindWindowText";
            comboBox_FindWindowText.Size = new Size(179, 43);
            comboBox_FindWindowText.TabIndex = 8;
            comboBox_FindWindowText.Text = "命令提示符";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft YaHei UI", 16F);
            label3.Location = new Point(12, 107);
            label3.Name = "label3";
            label3.Size = new Size(285, 35);
            label3.TabIndex = 9;
            label3.Text = "由窗口文本获取句柄：";
            // 
            // button_FindWindow
            // 
            button_FindWindow.Font = new Font("Microsoft YaHei UI", 12F);
            button_FindWindow.Location = new Point(462, 56);
            button_FindWindow.Name = "button_FindWindow";
            button_FindWindow.Size = new Size(67, 42);
            button_FindWindow.TabIndex = 7;
            button_FindWindow.Text = "获取";
            button_FindWindow.UseVisualStyleBackColor = true;
            button_FindWindow.Click += button_FindWindow_Click;
            // 
            // richTextBox_hWnd
            // 
            richTextBox_hWnd.Font = new Font("Microsoft YaHei UI", 36F);
            richTextBox_hWnd.Location = new Point(12, 204);
            richTextBox_hWnd.Name = "richTextBox_hWnd";
            richTextBox_hWnd.Size = new Size(517, 575);
            richTextBox_hWnd.TabIndex = 6;
            richTextBox_hWnd.Text = "";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Font = new Font("Microsoft YaHei UI", 18F);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(830, 862);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(richTextBox_wParam);
            tabPage1.Location = new Point(4, 48);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(822, 810);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "wParam";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // richTextBox_wParam
            // 
            richTextBox_wParam.Dock = DockStyle.Fill;
            richTextBox_wParam.Location = new Point(3, 3);
            richTextBox_wParam.Name = "richTextBox_wParam";
            richTextBox_wParam.Size = new Size(816, 804);
            richTextBox_wParam.TabIndex = 0;
            richTextBox_wParam.Text = "";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(richTextBox_lParam);
            tabPage2.Location = new Point(4, 48);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(822, 810);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "lParam";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBox_lParam
            // 
            richTextBox_lParam.Dock = DockStyle.Fill;
            richTextBox_lParam.Location = new Point(3, 3);
            richTextBox_lParam.Name = "richTextBox_lParam";
            richTextBox_lParam.Size = new Size(816, 804);
            richTextBox_lParam.TabIndex = 1;
            richTextBox_lParam.Text = "";
            // 
            // SendMessageToWindowsTool
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1376, 862);
            Controls.Add(splitContainer1);
            Name = "SendMessageToWindowsTool";
            Text = "发送Windows消息工具";
            Load += SendMessageToWindowsTool_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Label label1;
        private Button button_SendMessage;
        private ComboBox comboBox_FindWindowhWnd;
        private ComboBox comboBox_Msg;
        private Label label2;
        private SplitContainer splitContainer1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private RichTextBox richTextBox_wParam;
        private RichTextBox richTextBox_lParam;
        private RichTextBox richTextBox_hWnd;
        private Button button_FindWindow;
        private Button button_FindWindowOnText;
        private ComboBox comboBox_FindWindowText;
        private Label label3;
        private Button button_GetNoVisibleWindows;
        private Button button_GetOnEnumWindowsAPI;
    }
}