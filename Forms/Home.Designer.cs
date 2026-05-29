namespace QisToolkit3.Forms
{
    partial class Home
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
            labelIntroduce1 = new Label();
            labelTitle = new Label();
            labelIntroduce2 = new Label();
            SuspendLayout();
            // 
            // labelIntroduce1
            // 
            labelIntroduce1.BackColor = Color.FromArgb(233, 233, 233);
            labelIntroduce1.Font = new Font("Microsoft YaHei UI", 14F);
            labelIntroduce1.ForeColor = Color.Black;
            labelIntroduce1.Location = new Point(11, 70);
            labelIntroduce1.Margin = new Padding(2, 0, 2, 0);
            labelIntroduce1.Name = "labelIntroduce1";
            labelIntroduce1.Size = new Size(365, 556);
            labelIntroduce1.TabIndex = 5;
            labelIntroduce1.Text = "Ctrl+1：命令提示符\r\nCtrl+2：PowerShell\r\nCtrl+3：任务管理器\r\nCtrl+4：\r\nCtrl+5：\r\nCtrl+6：\r\nCtrl+7：\r\nCtrl+8：\r\nCtrl+9：\r\nCtrl+0：\r\nCtrl+S：SPL启动器\r\nCtrl+空格：显示此列表";
            labelIntroduce1.Visible = false;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("楷体", 28F, FontStyle.Regular, GraphicsUnit.Point, 134);
            labelTitle.ForeColor = Color.Black;
            labelTitle.Location = new Point(280, 9);
            labelTitle.Margin = new Padding(2, 0, 2, 0);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(212, 47);
            labelTitle.TabIndex = 4;
            labelTitle.Text = "热键功能";
            labelTitle.Visible = false;
            // 
            // labelIntroduce2
            // 
            labelIntroduce2.BackColor = Color.FromArgb(233, 233, 233);
            labelIntroduce2.Font = new Font("Microsoft YaHei UI", 14F);
            labelIntroduce2.ForeColor = Color.Black;
            labelIntroduce2.Location = new Point(396, 70);
            labelIntroduce2.Margin = new Padding(2, 0, 2, 0);
            labelIntroduce2.Name = "labelIntroduce2";
            labelIntroduce2.Size = new Size(386, 556);
            labelIntroduce2.TabIndex = 6;
            labelIntroduce2.Text = "Alt+1：System权限命令提示符\r\nAlt+2：System权限PowerShell\r\nAlt+3：System权限任务管理器\r\n";
            labelIntroduce2.Visible = false;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(793, 643);
            Controls.Add(labelIntroduce2);
            Controls.Add(labelIntroduce1);
            Controls.Add(labelTitle);
            Name = "Home";
            Text = "首页";
            FormClosing += Home_FormClosing;
            Load += Home_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelIntroduce1;
        private Label labelTitle;
        private Label labelIntroduce2;
    }
}