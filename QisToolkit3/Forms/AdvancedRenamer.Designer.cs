namespace QisToolkit3.Forms
{
    partial class AdvancedRenamer
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
            listBoxInput = new ListBox();
            splitContainer2 = new SplitContainer();
            listBoxOutput = new ListBox();
            textBox1 = new TextBox();
            label1 = new Label();
            buttonStart = new Button();
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
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(listBoxInput);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(1860, 873);
            splitContainer1.SplitterDistance = 332;
            splitContainer1.TabIndex = 23;
            // 
            // listBoxInput
            // 
            listBoxInput.Dock = DockStyle.Fill;
            listBoxInput.Font = new Font("微软雅黑", 12F);
            listBoxInput.FormattingEnabled = true;
            listBoxInput.ItemHeight = 27;
            listBoxInput.Location = new Point(0, 0);
            listBoxInput.Name = "listBoxInput";
            listBoxInput.SelectionMode = SelectionMode.MultiExtended;
            listBoxInput.Size = new Size(332, 873);
            listBoxInput.TabIndex = 0;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(listBoxOutput);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(textBox1);
            splitContainer2.Panel2.Controls.Add(label1);
            splitContainer2.Panel2.Controls.Add(buttonStart);
            splitContainer2.Panel2.Font = new Font("Microsoft YaHei UI", 12F);
            splitContainer2.Size = new Size(1524, 873);
            splitContainer2.SplitterDistance = 1028;
            splitContainer2.TabIndex = 24;
            // 
            // listBoxOutput
            // 
            listBoxOutput.Dock = DockStyle.Fill;
            listBoxOutput.Font = new Font("微软雅黑", 12F);
            listBoxOutput.FormattingEnabled = true;
            listBoxOutput.ItemHeight = 27;
            listBoxOutput.Location = new Point(0, 0);
            listBoxOutput.Name = "listBoxOutput";
            listBoxOutput.SelectionMode = SelectionMode.MultiExtended;
            listBoxOutput.Size = new Size(1028, 873);
            listBoxOutput.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(76, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(413, 33);
            textBox1.TabIndex = 1;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(-1, 15);
            label1.Name = "label1";
            label1.Size = new Size(92, 27);
            label1.TabIndex = 2;
            label1.Text = "重命名：";
            // 
            // buttonStart
            // 
            buttonStart.Font = new Font("Microsoft YaHei UI", 24F);
            buttonStart.Location = new Point(11, 770);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new Size(469, 91);
            buttonStart.TabIndex = 0;
            buttonStart.Text = "开始";
            buttonStart.UseVisualStyleBackColor = true;
            buttonStart.Click += buttonStart_Click;
            // 
            // AdvancedRenamer
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1860, 873);
            Controls.Add(splitContainer1);
            Font = new Font("Microsoft YaHei UI", 9F);
            Name = "AdvancedRenamer";
            Text = "批量重命名工具";
            Load += AdvancedRenamer_Load;
            DragDrop += AdvancedRenamer_DragDrop;
            DragEnter += AdvancedRenamer_DragEnter;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private ListBox listBoxInput;
        private SplitContainer splitContainer2;
        private ListBox listBoxOutput;
        private Button buttonStart;
        private TextBox textBox1;
        private Label label1;
    }
}