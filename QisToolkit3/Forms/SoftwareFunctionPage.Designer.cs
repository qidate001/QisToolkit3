namespace QisToolkit3.Forms
{
    partial class SoftwareFunctionPage
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            buttonQQ = new Button();
            button_WeChat = new Button();
            tabPage2 = new TabPage();
            tabPage4 = new TabPage();
            tabPage5 = new TabPage();
            tabPage6 = new TabPage();
            tabPage3 = new TabPage();
            buttonPCL = new Button();
            tabPage7 = new TabPage();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage3.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage6);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage7);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1004, 545);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(buttonQQ);
            tabPage1.Controls.Add(button_WeChat);
            tabPage1.Location = new Point(4, 36);
            tabPage1.Margin = new Padding(4);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(4);
            tabPage1.Size = new Size(996, 505);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "通讯软件";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonQQ
            // 
            buttonQQ.Enabled = false;
            buttonQQ.Font = new Font("Microsoft YaHei UI", 24F);
            buttonQQ.Location = new Point(8, 89);
            buttonQQ.Name = "buttonQQ";
            buttonQQ.Size = new Size(139, 76);
            buttonQQ.TabIndex = 2;
            buttonQQ.Text = "QQ";
            buttonQQ.UseVisualStyleBackColor = true;
            buttonQQ.Click += buttonQQ_Click;
            // 
            // button_WeChat
            // 
            button_WeChat.Font = new Font("Microsoft YaHei UI", 24F);
            button_WeChat.Location = new Point(8, 7);
            button_WeChat.Name = "button_WeChat";
            button_WeChat.Size = new Size(139, 76);
            button_WeChat.TabIndex = 1;
            button_WeChat.Text = "微信";
            button_WeChat.UseVisualStyleBackColor = true;
            button_WeChat.Click += button_WeChat_Click;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 29);
            tabPage2.Margin = new Padding(4);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(4);
            tabPage2.Size = new Size(996, 512);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "办公生产";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Location = new Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(996, 512);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "系统网络";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            tabPage5.Location = new Point(4, 29);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(996, 512);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "开发编程";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            tabPage6.Location = new Point(4, 29);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new Size(996, 512);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "设计创作";
            tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(buttonPCL);
            tabPage3.Location = new Point(4, 36);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(996, 505);
            tabPage3.TabIndex = 6;
            tabPage3.Text = "游戏娱乐";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // buttonPCL
            // 
            buttonPCL.Font = new Font("Microsoft YaHei UI", 24F);
            buttonPCL.Location = new Point(6, 6);
            buttonPCL.Name = "buttonPCL";
            buttonPCL.Size = new Size(139, 76);
            buttonPCL.TabIndex = 2;
            buttonPCL.Text = "PCL";
            buttonPCL.UseVisualStyleBackColor = true;
            buttonPCL.Click += buttonPCL_Click;
            // 
            // tabPage7
            // 
            tabPage7.Location = new Point(4, 29);
            tabPage7.Name = "tabPage7";
            tabPage7.Padding = new Padding(3);
            tabPage7.Size = new Size(996, 512);
            tabPage7.TabIndex = 7;
            tabPage7.Text = "影音创作";
            tabPage7.UseVisualStyleBackColor = true;
            // 
            // SoftwareFunctionPage
            // 
            AutoScaleDimensions = new SizeF(12F, 27F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1004, 545);
            Controls.Add(tabControl1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Margin = new Padding(4);
            Name = "SoftwareFunctionPage";
            Text = "SoftwareFunctionPage";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button button_WeChat;
        private TabPage tabPage2;
        private Button buttonQQ;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private TabPage tabPage3;
        private Button buttonPCL;
        private TabPage tabPage7;
    }
}