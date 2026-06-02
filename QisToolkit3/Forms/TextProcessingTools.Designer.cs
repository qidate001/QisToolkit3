namespace QisToolkit3.Forms
{
    partial class TextProcessingTools
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
            splitContainer = new SplitContainer();
            richTextBox = new RichTextBox();
            buttonRestore = new Button();
            buttonStandingInitialToUppercase = new Button();
            buttonSpecialCharacterConversion = new Button();
            buttonWithdraw = new Button();
            buttonUnderlineToSpaces = new Button();
            buttonSpacesToUnderline = new Button();
            buttonClearUnderline = new Button();
            buttonClearSpaces = new Button();
            buttonToLowercase = new Button();
            buttonToUppercase = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 0);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(richTextBox);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(buttonRestore);
            splitContainer.Panel2.Controls.Add(buttonStandingInitialToUppercase);
            splitContainer.Panel2.Controls.Add(buttonSpecialCharacterConversion);
            splitContainer.Panel2.Controls.Add(buttonWithdraw);
            splitContainer.Panel2.Controls.Add(buttonUnderlineToSpaces);
            splitContainer.Panel2.Controls.Add(buttonSpacesToUnderline);
            splitContainer.Panel2.Controls.Add(buttonClearUnderline);
            splitContainer.Panel2.Controls.Add(buttonClearSpaces);
            splitContainer.Panel2.Controls.Add(buttonToLowercase);
            splitContainer.Panel2.Controls.Add(buttonToUppercase);
            splitContainer.Size = new Size(978, 658);
            splitContainer.SplitterDistance = 444;
            splitContainer.TabIndex = 0;
            // 
            // richTextBox
            // 
            richTextBox.Dock = DockStyle.Fill;
            richTextBox.Font = new Font("微软雅黑", 14F);
            richTextBox.Location = new Point(0, 0);
            richTextBox.Name = "richTextBox";
            richTextBox.Size = new Size(444, 658);
            richTextBox.TabIndex = 1;
            richTextBox.Text = "";
            // 
            // buttonRestore
            // 
            buttonRestore.Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point, 134);
            buttonRestore.Location = new Point(264, 225);
            buttonRestore.Name = "buttonRestore";
            buttonRestore.Size = new Size(257, 68);
            buttonRestore.TabIndex = 13;
            buttonRestore.Text = "还原";
            buttonRestore.UseVisualStyleBackColor = true;
            buttonRestore.Click += buttonRestore_Click;
            // 
            // buttonStandingInitialToUppercase
            // 
            buttonStandingInitialToUppercase.Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point, 134);
            buttonStandingInitialToUppercase.Location = new Point(266, 299);
            buttonStandingInitialToUppercase.Name = "buttonStandingInitialToUppercase";
            buttonStandingInitialToUppercase.Size = new Size(257, 68);
            buttonStandingInitialToUppercase.TabIndex = 12;
            buttonStandingInitialToUppercase.Text = "英文首字母大写";
            buttonStandingInitialToUppercase.UseVisualStyleBackColor = true;
            buttonStandingInitialToUppercase.Click += buttonStandingInitialToUppercase_Click;
            // 
            // buttonSpecialCharacterConversion
            // 
            buttonSpecialCharacterConversion.Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point, 134);
            buttonSpecialCharacterConversion.Location = new Point(3, 299);
            buttonSpecialCharacterConversion.Name = "buttonSpecialCharacterConversion";
            buttonSpecialCharacterConversion.Size = new Size(257, 68);
            buttonSpecialCharacterConversion.TabIndex = 11;
            buttonSpecialCharacterConversion.Text = "特殊字符转换";
            buttonSpecialCharacterConversion.UseVisualStyleBackColor = true;
            buttonSpecialCharacterConversion.Click += buttonSpecialCharacterConversion_Click;
            // 
            // buttonWithdraw
            // 
            buttonWithdraw.Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point, 134);
            buttonWithdraw.Location = new Point(3, 225);
            buttonWithdraw.Name = "buttonWithdraw";
            buttonWithdraw.Size = new Size(257, 68);
            buttonWithdraw.TabIndex = 10;
            buttonWithdraw.Text = "撤回 / 还原";
            buttonWithdraw.UseVisualStyleBackColor = true;
            buttonWithdraw.Click += buttonWithdraw_Click;
            // 
            // buttonUnderlineToSpaces
            // 
            buttonUnderlineToSpaces.Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point, 134);
            buttonUnderlineToSpaces.Location = new Point(266, 151);
            buttonUnderlineToSpaces.Name = "buttonUnderlineToSpaces";
            buttonUnderlineToSpaces.Size = new Size(257, 68);
            buttonUnderlineToSpaces.TabIndex = 9;
            buttonUnderlineToSpaces.Text = "下划线转空格";
            buttonUnderlineToSpaces.UseVisualStyleBackColor = true;
            buttonUnderlineToSpaces.Click += buttonUnderlineToSpaces_Click;
            // 
            // buttonSpacesToUnderline
            // 
            buttonSpacesToUnderline.Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point, 134);
            buttonSpacesToUnderline.Location = new Point(3, 151);
            buttonSpacesToUnderline.Name = "buttonSpacesToUnderline";
            buttonSpacesToUnderline.Size = new Size(257, 68);
            buttonSpacesToUnderline.TabIndex = 8;
            buttonSpacesToUnderline.Text = "空格转下划线";
            buttonSpacesToUnderline.UseVisualStyleBackColor = true;
            buttonSpacesToUnderline.Click += buttonSpacesToUnderline_Click;
            // 
            // buttonClearUnderline
            // 
            buttonClearUnderline.Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point, 134);
            buttonClearUnderline.Location = new Point(266, 77);
            buttonClearUnderline.Name = "buttonClearUnderline";
            buttonClearUnderline.Size = new Size(257, 68);
            buttonClearUnderline.TabIndex = 7;
            buttonClearUnderline.Text = "去除下划线";
            buttonClearUnderline.UseVisualStyleBackColor = true;
            buttonClearUnderline.Click += buttonClearUnderline_Click;
            // 
            // buttonClearSpaces
            // 
            buttonClearSpaces.Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point, 134);
            buttonClearSpaces.Location = new Point(3, 77);
            buttonClearSpaces.Name = "buttonClearSpaces";
            buttonClearSpaces.Size = new Size(257, 68);
            buttonClearSpaces.TabIndex = 6;
            buttonClearSpaces.Text = "去除空格";
            buttonClearSpaces.UseVisualStyleBackColor = true;
            buttonClearSpaces.Click += buttonClearAir_Click;
            // 
            // buttonToLowercase
            // 
            buttonToLowercase.Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point, 134);
            buttonToLowercase.Location = new Point(266, 3);
            buttonToLowercase.Name = "buttonToLowercase";
            buttonToLowercase.Size = new Size(257, 68);
            buttonToLowercase.TabIndex = 1;
            buttonToLowercase.Text = "转小写";
            buttonToLowercase.UseVisualStyleBackColor = true;
            buttonToLowercase.Click += buttonToLowercase_Click;
            // 
            // buttonToUppercase
            // 
            buttonToUppercase.Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point, 134);
            buttonToUppercase.Location = new Point(2, 3);
            buttonToUppercase.Name = "buttonToUppercase";
            buttonToUppercase.Size = new Size(257, 68);
            buttonToUppercase.TabIndex = 0;
            buttonToUppercase.Text = "转大写";
            buttonToUppercase.UseVisualStyleBackColor = true;
            buttonToUppercase.Click += buttonToUppercase_Click;
            // 
            // TextProcessingTools
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(978, 658);
            Controls.Add(splitContainer);
            Name = "TextProcessingTools";
            Text = "齐的工具包3：文本处理工具";
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer;
        private Button buttonUnderlineToAir;
        private Button buttonAirToUnderline;
        private Button button2;
        private Button button3;
        private Button buttonToLowercase;
        private Button buttonToUppercase;
        private Button buttonClearUnderline;
        private Button buttonClearSpaces;
        private Button buttonUnderlineToSpaces;
        private Button buttonSpacesToUnderline;
        private RichTextBox richTextBox;
        private Button buttonWithdraw;
        private Button buttonSpecialCharacterConversion;
        private Button buttonStandingInitialToUppercase;
        private Button buttonRestore;
    }
}