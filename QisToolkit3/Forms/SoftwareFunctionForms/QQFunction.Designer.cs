namespace QisToolkit3.Forms.SoftwareFunctionForms
{
    partial class QQFunction
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
            components = new System.ComponentModel.Container();
            checkBox_NoQQScreenshot = new CheckBox();
            toolTip = new ToolTip(components);
            checkBox_NoQQ = new CheckBox();
            SuspendLayout();
            // 
            // checkBox_NoQQScreenshot
            // 
            checkBox_NoQQScreenshot.AutoSize = true;
            checkBox_NoQQScreenshot.Location = new Point(15, 83);
            checkBox_NoQQScreenshot.Margin = new Padding(6);
            checkBox_NoQQScreenshot.Name = "checkBox_NoQQScreenshot";
            checkBox_NoQQScreenshot.Size = new Size(270, 56);
            checkBox_NoQQScreenshot.TabIndex = 1;
            checkBox_NoQQScreenshot.Text = "禁用QQ截图";
            toolTip.SetToolTip(checkBox_NoQQScreenshot, "暴力禁用QQ截图\r\n将会阻止 QQScreenshot.exe 的正常运行\r\n但恕我直言，此功能几乎用不到");
            checkBox_NoQQScreenshot.UseVisualStyleBackColor = true;
            checkBox_NoQQScreenshot.CheckedChanged += checkBox_NoQQScreenshot_CheckedChanged;
            // 
            // toolTip
            // 
            toolTip.AutoPopDelay = 500000;
            toolTip.InitialDelay = 50;
            toolTip.IsBalloon = true;
            toolTip.ReshowDelay = 100;
            // 
            // checkBox_NoQQ
            // 
            checkBox_NoQQ.AutoSize = true;
            checkBox_NoQQ.ForeColor = Color.Red;
            checkBox_NoQQ.Location = new Point(15, 15);
            checkBox_NoQQ.Margin = new Padding(6);
            checkBox_NoQQ.Name = "checkBox_NoQQ";
            checkBox_NoQQ.Size = new Size(190, 56);
            checkBox_NoQQ.TabIndex = 2;
            checkBox_NoQQ.Text = "禁用QQ";
            toolTip.SetToolTip(checkBox_NoQQ, "暴力禁用QQ截图\r\n将会阻止 QQScreenshot.exe 的正常运行\r\n但恕我直言，此功能几乎用不到");
            checkBox_NoQQ.UseVisualStyleBackColor = true;
            checkBox_NoQQ.CheckedChanged += checkBox_NoQQ_CheckedChanged;
            // 
            // QQFunction
            // 
            AutoScaleDimensions = new SizeF(24F, 52F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(402, 544);
            Controls.Add(checkBox_NoQQ);
            Controls.Add(checkBox_NoQQScreenshot);
            Font = new Font("Microsoft YaHei UI", 24F);
            Margin = new Padding(8);
            Name = "QQFunction";
            Text = "QQFunction";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox checkBox_NoQQScreenshot;
        private ToolTip toolTip;
        private CheckBox checkBox_NoQQ;
    }
}