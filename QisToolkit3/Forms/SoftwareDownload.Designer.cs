namespace QisToolkit3.Forms
{
    partial class SoftwareDownload
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
            comboBoxSearch = new ComboBox();
            textBoxSearch = new TextBox();
            buttonSearch = new Button();
            buttonElseDownload = new Button();
            buttonOffWeb = new Button();
            buttonOffWebDownloadA = new Button();
            buttonOffWebDownloadB = new Button();
            buttonOffWebDownloadC = new Button();
            SuspendLayout();
            // 
            // comboBoxSearch
            // 
            comboBoxSearch.Dock = DockStyle.Top;
            comboBoxSearch.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSearch.Enabled = false;
            comboBoxSearch.Font = new Font("微软雅黑", 42F, FontStyle.Regular, GraphicsUnit.Point, 134);
            comboBoxSearch.FormattingEnabled = true;
            comboBoxSearch.Location = new Point(0, 0);
            comboBoxSearch.Name = "comboBoxSearch";
            comboBoxSearch.Size = new Size(1338, 98);
            comboBoxSearch.TabIndex = 0;
            comboBoxSearch.SelectedIndexChanged += comboBoxSearch_SelectedIndexChanged;
            // 
            // textBoxSearch
            // 
            textBoxSearch.Dock = DockStyle.Top;
            textBoxSearch.Font = new Font("微软雅黑", 42F);
            textBoxSearch.Location = new Point(0, 98);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.Size = new Size(1338, 100);
            textBoxSearch.TabIndex = 1;
            textBoxSearch.TextChanged += textBoxSearch_TextChanged;
            // 
            // buttonSearch
            // 
            buttonSearch.Dock = DockStyle.Top;
            buttonSearch.Enabled = false;
            buttonSearch.Font = new Font("微软雅黑", 42F);
            buttonSearch.Location = new Point(0, 198);
            buttonSearch.Name = "buttonSearch";
            buttonSearch.Size = new Size(1338, 100);
            buttonSearch.TabIndex = 2;
            buttonSearch.Text = "搜索";
            buttonSearch.UseVisualStyleBackColor = true;
            buttonSearch.Click += buttonSearch_Click;
            // 
            // buttonElseDownload
            // 
            buttonElseDownload.Dock = DockStyle.Top;
            buttonElseDownload.Enabled = false;
            buttonElseDownload.Font = new Font("微软雅黑", 42F);
            buttonElseDownload.Location = new Point(0, 298);
            buttonElseDownload.Name = "buttonElseDownload";
            buttonElseDownload.Size = new Size(1338, 100);
            buttonElseDownload.TabIndex = 3;
            buttonElseDownload.Text = "第三方下载";
            buttonElseDownload.UseVisualStyleBackColor = true;
            buttonElseDownload.Click += buttonElseDownload_Click;
            // 
            // buttonOffWeb
            // 
            buttonOffWeb.Dock = DockStyle.Top;
            buttonOffWeb.Enabled = false;
            buttonOffWeb.Font = new Font("微软雅黑", 42F);
            buttonOffWeb.Location = new Point(0, 398);
            buttonOffWeb.Name = "buttonOffWeb";
            buttonOffWeb.Size = new Size(1338, 100);
            buttonOffWeb.TabIndex = 4;
            buttonOffWeb.Text = "官方网站";
            buttonOffWeb.UseVisualStyleBackColor = true;
            buttonOffWeb.Click += buttonOffWeb_Click;
            // 
            // buttonOffWebDownloadA
            // 
            buttonOffWebDownloadA.Dock = DockStyle.Top;
            buttonOffWebDownloadA.Enabled = false;
            buttonOffWebDownloadA.Font = new Font("微软雅黑", 42F);
            buttonOffWebDownloadA.Location = new Point(0, 498);
            buttonOffWebDownloadA.Name = "buttonOffWebDownloadA";
            buttonOffWebDownloadA.Size = new Size(1338, 100);
            buttonOffWebDownloadA.TabIndex = 5;
            buttonOffWebDownloadA.Text = "官方下载 A";
            buttonOffWebDownloadA.UseVisualStyleBackColor = true;
            buttonOffWebDownloadA.Click += buttonOffWebDownloadA_Click;
            // 
            // buttonOffWebDownloadB
            // 
            buttonOffWebDownloadB.Dock = DockStyle.Top;
            buttonOffWebDownloadB.Enabled = false;
            buttonOffWebDownloadB.Font = new Font("微软雅黑", 42F);
            buttonOffWebDownloadB.Location = new Point(0, 598);
            buttonOffWebDownloadB.Name = "buttonOffWebDownloadB";
            buttonOffWebDownloadB.Size = new Size(1338, 100);
            buttonOffWebDownloadB.TabIndex = 6;
            buttonOffWebDownloadB.Text = "官方下载 B";
            buttonOffWebDownloadB.UseVisualStyleBackColor = true;
            buttonOffWebDownloadB.Click += buttonOffWebDownloadB_Click;
            // 
            // buttonOffWebDownloadC
            // 
            buttonOffWebDownloadC.Dock = DockStyle.Top;
            buttonOffWebDownloadC.Enabled = false;
            buttonOffWebDownloadC.Font = new Font("微软雅黑", 42F);
            buttonOffWebDownloadC.Location = new Point(0, 698);
            buttonOffWebDownloadC.Name = "buttonOffWebDownloadC";
            buttonOffWebDownloadC.Size = new Size(1338, 100);
            buttonOffWebDownloadC.TabIndex = 7;
            buttonOffWebDownloadC.Text = "官方下载 C";
            buttonOffWebDownloadC.UseVisualStyleBackColor = true;
            buttonOffWebDownloadC.Click += buttonOffWebDownloadC_Click;
            // 
            // SoftwareDownload
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1338, 797);
            Controls.Add(buttonOffWebDownloadC);
            Controls.Add(buttonOffWebDownloadB);
            Controls.Add(buttonOffWebDownloadA);
            Controls.Add(buttonOffWeb);
            Controls.Add(buttonElseDownload);
            Controls.Add(buttonSearch);
            Controls.Add(textBoxSearch);
            Controls.Add(comboBoxSearch);
            Name = "SoftwareDownload";
            Text = "软件下载";
            Load += SoftwareDownload_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBoxSearch;
        private TextBox textBoxSearch;
        private Button buttonSearch;
        private Button buttonElseDownload;
        private Button buttonOffWeb;
        private Button buttonOffWebDownloadA;
        private Button buttonOffWebDownloadB;
        private Button buttonOffWebDownloadC;
    }
}