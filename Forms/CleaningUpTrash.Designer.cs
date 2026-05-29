namespace QisToolkit3.Forms
{
    partial class CleaningUpTrash
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
            splitContainer1 = new SplitContainer();
            checkedListBox = new CheckedListBox();
            splitContainer2 = new SplitContainer();
            label_AllDeleteFileSize = new Label();
            label_AllDeleteFileCount = new Label();
            checkBoxKillProcess = new CheckBox();
            checkBoxUserCompatibilityMode = new CheckBox();
            label_AllFileSize = new Label();
            label_AllFileCount = new Label();
            buttonCleaningUp = new Button();
            buttonScanGarbage = new Button();
            richTextBox = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(2);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(checkedListBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(916, 658);
            splitContainer1.SplitterDistance = 242;
            splitContainer1.SplitterWidth = 3;
            splitContainer1.TabIndex = 0;
            // 
            // checkedListBox
            // 
            checkedListBox.Dock = DockStyle.Fill;
            checkedListBox.Font = new Font("微软雅黑", 12F);
            checkedListBox.FormattingEnabled = true;
            checkedListBox.Location = new Point(0, 0);
            checkedListBox.Margin = new Padding(2);
            checkedListBox.Name = "checkedListBox";
            checkedListBox.Size = new Size(242, 658);
            checkedListBox.TabIndex = 0;
            checkedListBox.SelectedIndexChanged += checkedListBox_SelectedIndexChanged;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Margin = new Padding(2);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(label_AllDeleteFileSize);
            splitContainer2.Panel1.Controls.Add(label_AllDeleteFileCount);
            splitContainer2.Panel1.Controls.Add(checkBoxKillProcess);
            splitContainer2.Panel1.Controls.Add(checkBoxUserCompatibilityMode);
            splitContainer2.Panel1.Controls.Add(label_AllFileSize);
            splitContainer2.Panel1.Controls.Add(label_AllFileCount);
            splitContainer2.Panel1.Controls.Add(buttonCleaningUp);
            splitContainer2.Panel1.Controls.Add(buttonScanGarbage);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(richTextBox);
            splitContainer2.Size = new Size(671, 658);
            splitContainer2.SplitterDistance = 299;
            splitContainer2.SplitterWidth = 3;
            splitContainer2.TabIndex = 0;
            // 
            // label_AllDeleteFileSize
            // 
            label_AllDeleteFileSize.AutoSize = true;
            label_AllDeleteFileSize.Font = new Font("微软雅黑", 14F);
            label_AllDeleteFileSize.Location = new Point(7, 98);
            label_AllDeleteFileSize.Margin = new Padding(2, 0, 2, 0);
            label_AllDeleteFileSize.Name = "label_AllDeleteFileSize";
            label_AllDeleteFileSize.Size = new Size(163, 31);
            label_AllDeleteFileSize.TabIndex = 8;
            label_AllDeleteFileSize.Text = "删除大小：0B";
            label_AllDeleteFileSize.Visible = false;
            label_AllDeleteFileSize.Click += label_AllDeleteFileSize_Click;
            // 
            // label_AllDeleteFileCount
            // 
            label_AllDeleteFileCount.AutoSize = true;
            label_AllDeleteFileCount.Font = new Font("微软雅黑", 14F);
            label_AllDeleteFileCount.Location = new Point(7, 68);
            label_AllDeleteFileCount.Margin = new Padding(2, 0, 2, 0);
            label_AllDeleteFileCount.Name = "label_AllDeleteFileCount";
            label_AllDeleteFileCount.Size = new Size(148, 31);
            label_AllDeleteFileCount.TabIndex = 7;
            label_AllDeleteFileCount.Text = "删除总数：0";
            label_AllDeleteFileCount.Visible = false;
            // 
            // checkBoxKillProcess
            // 
            checkBoxKillProcess.AutoSize = true;
            checkBoxKillProcess.Dock = DockStyle.Bottom;
            checkBoxKillProcess.Font = new Font("微软雅黑", 22F);
            checkBoxKillProcess.Location = new Point(0, 354);
            checkBoxKillProcess.Margin = new Padding(2);
            checkBoxKillProcess.Name = "checkBoxKillProcess";
            checkBoxKillProcess.Size = new Size(299, 52);
            checkBoxKillProcess.TabIndex = 6;
            checkBoxKillProcess.Text = "关闭有关进程";
            checkBoxKillProcess.UseVisualStyleBackColor = true;
            checkBoxKillProcess.CheckedChanged += checkBoxKillAllUserProcess_CheckedChanged;
            // 
            // checkBoxUserCompatibilityMode
            // 
            checkBoxUserCompatibilityMode.AutoSize = true;
            checkBoxUserCompatibilityMode.Dock = DockStyle.Bottom;
            checkBoxUserCompatibilityMode.Font = new Font("微软雅黑", 22F);
            checkBoxUserCompatibilityMode.Location = new Point(0, 406);
            checkBoxUserCompatibilityMode.Margin = new Padding(2);
            checkBoxUserCompatibilityMode.Name = "checkBoxUserCompatibilityMode";
            checkBoxUserCompatibilityMode.Size = new Size(299, 52);
            checkBoxUserCompatibilityMode.TabIndex = 5;
            checkBoxUserCompatibilityMode.Text = "用户兼容模式";
            checkBoxUserCompatibilityMode.UseVisualStyleBackColor = true;
            checkBoxUserCompatibilityMode.CheckedChanged += checkBoxUserCompatibilityMode_CheckedChanged;
            // 
            // label_AllFileSize
            // 
            label_AllFileSize.AutoSize = true;
            label_AllFileSize.Font = new Font("微软雅黑", 14F);
            label_AllFileSize.Location = new Point(7, 38);
            label_AllFileSize.Margin = new Padding(2, 0, 2, 0);
            label_AllFileSize.Name = "label_AllFileSize";
            label_AllFileSize.Size = new Size(115, 31);
            label_AllFileSize.TabIndex = 4;
            label_AllFileSize.Text = "大小：0B";
            label_AllFileSize.Visible = false;
            label_AllFileSize.Click += label_AllFileSize_Click;
            // 
            // label_AllFileCount
            // 
            label_AllFileCount.AutoSize = true;
            label_AllFileCount.Font = new Font("微软雅黑", 14F);
            label_AllFileCount.Location = new Point(7, 8);
            label_AllFileCount.Margin = new Padding(2, 0, 2, 0);
            label_AllFileCount.Name = "label_AllFileCount";
            label_AllFileCount.Size = new Size(100, 31);
            label_AllFileCount.TabIndex = 3;
            label_AllFileCount.Text = "总数：0";
            label_AllFileCount.Visible = false;
            // 
            // buttonCleaningUp
            // 
            buttonCleaningUp.Dock = DockStyle.Bottom;
            buttonCleaningUp.Enabled = false;
            buttonCleaningUp.Font = new Font("微软雅黑", 33F);
            buttonCleaningUp.Location = new Point(0, 458);
            buttonCleaningUp.Margin = new Padding(2);
            buttonCleaningUp.Name = "buttonCleaningUp";
            buttonCleaningUp.Size = new Size(299, 100);
            buttonCleaningUp.TabIndex = 2;
            buttonCleaningUp.Text = "清理垃圾";
            buttonCleaningUp.UseVisualStyleBackColor = true;
            buttonCleaningUp.Click += buttonCleaningUp_Click;
            // 
            // buttonScanGarbage
            // 
            buttonScanGarbage.Dock = DockStyle.Bottom;
            buttonScanGarbage.Font = new Font("微软雅黑", 33F);
            buttonScanGarbage.Location = new Point(0, 558);
            buttonScanGarbage.Margin = new Padding(2);
            buttonScanGarbage.Name = "buttonScanGarbage";
            buttonScanGarbage.Size = new Size(299, 100);
            buttonScanGarbage.TabIndex = 0;
            buttonScanGarbage.Text = "扫描垃圾";
            buttonScanGarbage.UseVisualStyleBackColor = true;
            buttonScanGarbage.Click += buttonScanGarbage_Click;
            // 
            // richTextBox
            // 
            richTextBox.Dock = DockStyle.Fill;
            richTextBox.Font = new Font("微软雅黑", 4F);
            richTextBox.Location = new Point(0, 0);
            richTextBox.Margin = new Padding(2);
            richTextBox.Name = "richTextBox";
            richTextBox.ReadOnly = true;
            richTextBox.Size = new Size(369, 658);
            richTextBox.TabIndex = 0;
            richTextBox.Text = "";
            richTextBox.WordWrap = false;
            // 
            // CleaningUpTrash
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(916, 658);
            Controls.Add(splitContainer1);
            HelpButton = true;
            Margin = new Padding(2);
            Name = "CleaningUpTrash";
            Text = "清理垃圾";
            HelpButtonClicked += CleaningUpTrash_HelpButtonClicked;
            Load += CleaningUpTrash_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel1.PerformLayout();
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private CheckedListBox checkedListBox;
        private SplitContainer splitContainer2;
        private RichTextBox richTextBox;
        private Button button2;
        private Button buttonScanGarbage;
        private Button buttonCleaningUp;
        private Label label_AllFileCount;
        private Label label_AllFileSize;
        private CheckBox checkBoxUserCompatibilityMode;
        private CheckBox checkBoxKillProcess;
        private Label label_AllDeleteFileSize;
        private Label label_AllDeleteFileCount;
    }
}