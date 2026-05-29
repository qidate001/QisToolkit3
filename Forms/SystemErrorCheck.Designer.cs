namespace QisToolkit3.Forms
{
    partial class SystemErrorCheck
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
            label_ProblemInterpretation = new Label();
            label1 = new Label();
            buttonRepair = new Button();
            buttonScan = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(checkedListBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(label_ProblemInterpretation);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Panel2.Controls.Add(buttonRepair);
            splitContainer1.Panel2.Controls.Add(buttonScan);
            splitContainer1.Size = new Size(1186, 761);
            splitContainer1.SplitterDistance = 666;
            splitContainer1.TabIndex = 0;
            //splitContainer1.SplitterMoved += this.splitContainer1_SplitterMoved;
            // 
            // checkedListBox
            // 
            checkedListBox.Dock = DockStyle.Fill;
            checkedListBox.Font = new Font("微软雅黑", 12F);
            checkedListBox.FormattingEnabled = true;
            checkedListBox.Location = new Point(0, 0);
            checkedListBox.Margin = new Padding(2);
            checkedListBox.Name = "checkedListBox";
            checkedListBox.Size = new Size(666, 761);
            checkedListBox.TabIndex = 1;
            checkedListBox.SelectedIndexChanged += checkedListBox_SelectedIndexChanged;
            // 
            // label_ProblemInterpretation
            // 
            label_ProblemInterpretation.BackColor = Color.FromArgb(224, 224, 224);
            label_ProblemInterpretation.Dock = DockStyle.Top;
            label_ProblemInterpretation.Font = new Font("黑体", 14F);
            label_ProblemInterpretation.Location = new Point(0, 180);
            label_ProblemInterpretation.Name = "label_ProblemInterpretation";
            label_ProblemInterpretation.Size = new Size(516, 306);
            label_ProblemInterpretation.TabIndex = 3;
            //label_ProblemInterpretation.Click += this.label_ProblemInterpretation_Click;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("黑体", 14F);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(516, 180);
            label1.TabIndex = 2;
            label1.Text = "关于本扫描工具的一下注意事项\r\n\r\n1.本工具是系统错误检查工具，可用于检查一些常见的系统错误（例如输入法消失等等）\r\n\r\n2.本工具不是流氓软件删除工具，更不是病毒扫描工具！！！";
            // 
            // buttonRepair
            // 
            buttonRepair.Dock = DockStyle.Bottom;
            buttonRepair.Enabled = false;
            buttonRepair.Font = new Font("微软雅黑", 42F, FontStyle.Regular, GraphicsUnit.Point, 134);
            buttonRepair.Location = new Point(0, 489);
            buttonRepair.Name = "buttonRepair";
            buttonRepair.Size = new Size(516, 136);
            buttonRepair.TabIndex = 1;
            buttonRepair.Text = "修复";
            buttonRepair.UseVisualStyleBackColor = true;
            buttonRepair.Click += buttonRepair_Click;
            // 
            // buttonScan
            // 
            buttonScan.Dock = DockStyle.Bottom;
            buttonScan.Font = new Font("微软雅黑", 42F, FontStyle.Regular, GraphicsUnit.Point, 134);
            buttonScan.Location = new Point(0, 625);
            buttonScan.Name = "buttonScan";
            buttonScan.Size = new Size(516, 136);
            buttonScan.TabIndex = 0;
            buttonScan.Text = "扫描";
            buttonScan.UseVisualStyleBackColor = true;
            buttonScan.Click += buttonScan_Click;
            // 
            // SystemErrorCheck
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1186, 761);
            Controls.Add(splitContainer1);
            Name = "SystemErrorCheck";
            Text = "系统错误检查工具";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private CheckedListBox checkedListBox;
        private Button buttonRepair;
        private Button buttonScan;
        private Label label1;
        private Label label_ProblemInterpretation;
    }
}