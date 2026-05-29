namespace QisToolkit3.Forms
{
    partial class CmdInQisToolkit3
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
            outputBox = new RichTextBox();
            splitContainer2 = new SplitContainer();
            sendButton = new Button();
            inputBox = new RichTextBox();
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
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(outputBox);
            splitContainer1.Size = new Size(1290, 660);
            splitContainer1.SplitterDistance = 430;
            splitContainer1.TabIndex = 0;
            // 
            // outputBox
            // 
            outputBox.Dock = DockStyle.Fill;
            outputBox.Location = new Point(0, 0);
            outputBox.Name = "outputBox";
            outputBox.Size = new Size(856, 660);
            outputBox.TabIndex = 0;
            outputBox.Text = "";
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(inputBox);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(sendButton);
            splitContainer2.Size = new Size(430, 660);
            splitContainer2.SplitterDistance = 520;
            splitContainer2.TabIndex = 1;
            // 
            // sendButton
            // 
            sendButton.Dock = DockStyle.Fill;
            sendButton.Font = new Font("Microsoft YaHei UI", 24F);
            sendButton.Location = new Point(0, 0);
            sendButton.Name = "sendButton";
            sendButton.Size = new Size(430, 136);
            sendButton.TabIndex = 1;
            sendButton.Text = "执行";
            sendButton.UseVisualStyleBackColor = true;
            // 
            // inputBox
            // 
            inputBox.Dock = DockStyle.Fill;
            inputBox.Location = new Point(0, 0);
            inputBox.Name = "inputBox";
            inputBox.Size = new Size(430, 520);
            inputBox.TabIndex = 2;
            inputBox.Text = "";
            // 
            // CmdInQisToolkit3
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1290, 660);
            Controls.Add(splitContainer1);
            Name = "CmdInQisToolkit3";
            Text = "CmdInQisToolkit3";
            Load += CmdInQisToolkit3_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private RichTextBox outputBox;
        private SplitContainer splitContainer2;
        private RichTextBox inputBox;
        private Button sendButton;
    }
}