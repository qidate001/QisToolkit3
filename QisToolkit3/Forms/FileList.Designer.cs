namespace QisToolkit3.Forms
{
    partial class FileList
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
            listBox1 = new ListBox();
            button_Next = new Button();
            button_Back = new Button();
            button1 = new Button();
            button2 = new Button();
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
            splitContainer1.Panel1.Controls.Add(listBox1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(button2);
            splitContainer1.Panel2.Controls.Add(button1);
            splitContainer1.Panel2.Controls.Add(button_Back);
            splitContainer1.Panel2.Controls.Add(button_Next);
            splitContainer1.Size = new Size(800, 851);
            splitContainer1.SplitterDistance = 345;
            splitContainer1.TabIndex = 0;
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.Font = new Font("Microsoft YaHei UI", 24F);
            listBox1.FormattingEnabled = true;
            listBox1.HorizontalScrollbar = true;
            listBox1.ItemHeight = 52;
            listBox1.Location = new Point(0, 0);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(345, 851);
            listBox1.TabIndex = 1;
            // 
            // button_Next
            // 
            button_Next.Font = new Font("Microsoft YaHei UI", 46F);
            button_Next.Location = new Point(12, 12);
            button_Next.Name = "button_Next";
            button_Next.Size = new Size(427, 134);
            button_Next.TabIndex = 0;
            button_Next.Text = "下一级";
            button_Next.UseVisualStyleBackColor = true;
            // 
            // button_Back
            // 
            button_Back.Font = new Font("Microsoft YaHei UI", 46F);
            button_Back.Location = new Point(12, 152);
            button_Back.Name = "button_Back";
            button_Back.Size = new Size(427, 134);
            button_Back.TabIndex = 1;
            button_Back.Text = "上一级";
            button_Back.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Font = new Font("Microsoft YaHei UI", 46F);
            button1.Location = new Point(12, 292);
            button1.Name = "button1";
            button1.Size = new Size(427, 134);
            button1.TabIndex = 2;
            button1.Text = "CMD";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Font = new Font("Microsoft YaHei UI", 46F);
            button2.Location = new Point(12, 432);
            button2.Name = "button2";
            button2.Size = new Size(427, 134);
            button2.TabIndex = 3;
            button2.Text = "路径";
            button2.UseVisualStyleBackColor = true;
            // 
            // FileList
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 851);
            Controls.Add(splitContainer1);
            Name = "FileList";
            Text = "FileList";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private ListBox listBox1;
        private Button button_Next;
        private Button button_Back;
        private Button button1;
        private Button button2;
    }
}