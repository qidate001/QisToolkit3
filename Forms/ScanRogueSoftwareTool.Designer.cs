namespace QisToolkit3.Forms
{
    partial class ScanRogueSoftwareTool
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
            checkedListBox = new CheckedListBox();
            splitContainer1 = new SplitContainer();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            label_ProblemInterpretation = new Label();
            label = new Label();
            buttonDelete = new Button();
            buttonScan = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // checkedListBox
            // 
            checkedListBox.Dock = DockStyle.Fill;
            checkedListBox.Font = new Font("微软雅黑", 12F);
            checkedListBox.FormattingEnabled = true;
            checkedListBox.Location = new Point(0, 0);
            checkedListBox.Margin = new Padding(2);
            checkedListBox.Name = "checkedListBox";
            checkedListBox.Size = new Size(747, 736);
            checkedListBox.TabIndex = 1;
            checkedListBox.SelectedIndexChanged += checkedListBox_SelectedIndexChanged;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(lblStatus);
            splitContainer1.Panel1.Controls.Add(progressBar);
            splitContainer1.Panel1.Controls.Add(checkedListBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(label_ProblemInterpretation);
            splitContainer1.Panel2.Controls.Add(label);
            splitContainer1.Panel2.Controls.Add(buttonDelete);
            splitContainer1.Panel2.Controls.Add(buttonScan);
            splitContainer1.Size = new Size(1332, 736);
            splitContainer1.SplitterDistance = 747;
            splitContainer1.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Microsoft YaHei UI", 18F);
            lblStatus.Location = new Point(3, 631);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(227, 39);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "当前任务：暂无";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(3, 673);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(741, 60);
            progressBar.TabIndex = 4;
            // 
            // label_ProblemInterpretation
            // 
            label_ProblemInterpretation.BackColor = Color.FromArgb(224, 224, 224);
            label_ProblemInterpretation.Dock = DockStyle.Top;
            label_ProblemInterpretation.Font = new Font("黑体", 14F);
            label_ProblemInterpretation.Location = new Point(0, 180);
            label_ProblemInterpretation.Name = "label_ProblemInterpretation";
            label_ProblemInterpretation.Size = new Size(581, 306);
            label_ProblemInterpretation.TabIndex = 3;
            // 
            // label
            // 
            label.Dock = DockStyle.Top;
            label.Font = new Font("黑体", 14F);
            label.Location = new Point(0, 0);
            label.Name = "label";
            label.Size = new Size(581, 180);
            label.TabIndex = 2;
            label.Text = "关于本扫描工具的一下注意事项\r\n\r\n1.本工具是流氓软件扫描工具，可用于删除一些常见的流氓软件\r\n\r\n2. 如360、2345等软件都会被视为流氓软件，因此需要特别注意";
            // 
            // buttonDelete
            // 
            buttonDelete.Dock = DockStyle.Bottom;
            buttonDelete.Enabled = false;
            buttonDelete.Font = new Font("微软雅黑", 42F, FontStyle.Regular, GraphicsUnit.Point, 134);
            buttonDelete.Location = new Point(0, 464);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(581, 136);
            buttonDelete.TabIndex = 1;
            buttonDelete.Text = "删除";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // buttonScan
            // 
            buttonScan.Dock = DockStyle.Bottom;
            buttonScan.Font = new Font("微软雅黑", 42F, FontStyle.Regular, GraphicsUnit.Point, 134);
            buttonScan.Location = new Point(0, 600);
            buttonScan.Name = "buttonScan";
            buttonScan.Size = new Size(581, 136);
            buttonScan.TabIndex = 0;
            buttonScan.Text = "扫描";
            buttonScan.UseVisualStyleBackColor = true;
            buttonScan.Click += buttonScan_Click;
            // 
            // ScanRogueSoftwareTool
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1332, 736);
            Controls.Add(splitContainer1);
            Name = "ScanRogueSoftwareTool";
            Text = "ScanRogueSoftwareTool";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private CheckedListBox checkedListBox;
        private SplitContainer splitContainer1;
        private Label label_ProblemInterpretation;
        private Label label;
        private Button buttonDelete;
        private Button buttonScan;
        private ProgressBar progressBar;
        private Label lblStatus;
    }
}