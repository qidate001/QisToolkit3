namespace QisToolkit3.Forms
{
    partial class ExtendedFeatures
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
            button_qicmd_download = new Button();
            label1 = new Label();
            button_qicmd_delete = new Button();
            comboBox_QiCmd = new ComboBox();
            label2 = new Label();
            comboBox1 = new ComboBox();
            button_mas_delete = new Button();
            button_mas_download = new Button();
            SuspendLayout();
            // 
            // button_qicmd_download
            // 
            button_qicmd_download.Font = new Font("Microsoft YaHei UI", 24F);
            button_qicmd_download.Location = new Point(26, 43);
            button_qicmd_download.Margin = new Padding(4);
            button_qicmd_download.Name = "button_qicmd_download";
            button_qicmd_download.Size = new Size(405, 88);
            button_qicmd_download.TabIndex = 0;
            button_qicmd_download.Text = "下载";
            button_qicmd_download.UseVisualStyleBackColor = true;
            button_qicmd_download.Click += button_qicmd_download_Click;
            // 
            // label1
            // 
            label1.Location = new Point(26, 12);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(405, 27);
            label1.TabIndex = 1;
            label1.Text = "QiCmd";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button_qicmd_delete
            // 
            button_qicmd_delete.Font = new Font("Microsoft YaHei UI", 24F);
            button_qicmd_delete.Location = new Point(26, 139);
            button_qicmd_delete.Margin = new Padding(4);
            button_qicmd_delete.Name = "button_qicmd_delete";
            button_qicmd_delete.Size = new Size(405, 88);
            button_qicmd_delete.TabIndex = 2;
            button_qicmd_delete.Text = "卸载";
            button_qicmd_delete.UseVisualStyleBackColor = true;
            button_qicmd_delete.Click += button_qicmd_delete_Click;
            // 
            // comboBox_QiCmd
            // 
            comboBox_QiCmd.FormattingEnabled = true;
            comboBox_QiCmd.Items.AddRange(new object[] { "v0.1", "v0.2", "v0.3", "v0.4" });
            comboBox_QiCmd.Location = new Point(26, 234);
            comboBox_QiCmd.Name = "comboBox_QiCmd";
            comboBox_QiCmd.Size = new Size(405, 35);
            comboBox_QiCmd.TabIndex = 3;
            comboBox_QiCmd.Text = "v0.4";
            // 
            // label2
            // 
            label2.Location = new Point(439, 12);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(405, 27);
            label2.TabIndex = 4;
            label2.Text = "MAS 激活器";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "v0.1", "v0.2", "v0.3", "v0.4" });
            comboBox1.Location = new Point(439, 234);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(405, 35);
            comboBox1.TabIndex = 7;
            comboBox1.Text = "v0.4";
            comboBox1.Visible = false;
            // 
            // button_mas_delete
            // 
            button_mas_delete.Font = new Font("Microsoft YaHei UI", 24F);
            button_mas_delete.Location = new Point(439, 139);
            button_mas_delete.Margin = new Padding(4);
            button_mas_delete.Name = "button_mas_delete";
            button_mas_delete.Size = new Size(405, 88);
            button_mas_delete.TabIndex = 6;
            button_mas_delete.Text = "删除";
            button_mas_delete.UseVisualStyleBackColor = true;
            button_mas_delete.Click += button_mas_delete_Click;
            // 
            // button_mas_download
            // 
            button_mas_download.Font = new Font("Microsoft YaHei UI", 24F);
            button_mas_download.Location = new Point(439, 43);
            button_mas_download.Margin = new Padding(4);
            button_mas_download.Name = "button_mas_download";
            button_mas_download.Size = new Size(405, 88);
            button_mas_download.TabIndex = 5;
            button_mas_download.Text = "下载";
            button_mas_download.UseVisualStyleBackColor = true;
            button_mas_download.Click += button_mas_download_Click;
            // 
            // ExtendedFeatures
            // 
            AutoScaleDimensions = new SizeF(12F, 27F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1467, 945);
            Controls.Add(comboBox1);
            Controls.Add(button_mas_delete);
            Controls.Add(button_mas_download);
            Controls.Add(label2);
            Controls.Add(comboBox_QiCmd);
            Controls.Add(button_qicmd_delete);
            Controls.Add(label1);
            Controls.Add(button_qicmd_download);
            Font = new Font("Microsoft YaHei UI", 12F);
            Margin = new Padding(4);
            Name = "ExtendedFeatures";
            Text = "ExtendedFeatures";
            Load += ExtendedFeatures_Load;
            ResumeLayout(false);
        }

        #endregion
        private Label label1;
        private Button button_qicmd_download;
        private Button button_qicmd_delete;
        private ComboBox comboBox_QiCmd;
        private Label label2;
        private ComboBox comboBox1;
        private Button button_mas_delete;
        private Button button_mas_download;
    }
}