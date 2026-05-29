namespace QisToolkit3.Forms
{
    partial class StrangeQuestionAndAnswer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StrangeQuestionAndAnswer));
            buttonStart = new Button();
            labelWinCount = new Label();
            labelTitle = new Label();
            labelIntroduce = new Label();
            SuspendLayout();
            // 
            // buttonStart
            // 
            buttonStart.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonStart.Font = new Font("黑体", 66F);
            buttonStart.ForeColor = Color.Black;
            buttonStart.Location = new Point(138, 335);
            buttonStart.Margin = new Padding(2);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new Size(513, 194);
            buttonStart.TabIndex = 0;
            buttonStart.Text = "开始挑战";
            buttonStart.UseVisualStyleBackColor = true;
            buttonStart.Click += buttonStart_Click;
            // 
            // labelWinCount
            // 
            labelWinCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labelWinCount.AutoSize = true;
            labelWinCount.Font = new Font("黑体", 14F);
            labelWinCount.ForeColor = Color.Black;
            labelWinCount.Location = new Point(9, 556);
            labelWinCount.Margin = new Padding(2, 0, 2, 0);
            labelWinCount.Name = "labelWinCount";
            labelWinCount.Size = new Size(190, 24);
            labelWinCount.TabIndex = 1;
            labelWinCount.Text = "您已通关了 0 次";
            labelWinCount.Click += labelWinCount_Click;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("楷体", 28F, FontStyle.Regular, GraphicsUnit.Point, 134);
            labelTitle.ForeColor = Color.Black;
            labelTitle.Location = new Point(296, 8);
            labelTitle.Margin = new Padding(2, 0, 2, 0);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(212, 47);
            labelTitle.TabIndex = 2;
            labelTitle.Text = "奇葩问答";
            // 
            // labelIntroduce
            // 
            labelIntroduce.AutoSize = true;
            labelIntroduce.Font = new Font("Microsoft YaHei UI", 14F);
            labelIntroduce.ForeColor = Color.Black;
            labelIntroduce.Location = new Point(85, 54);
            labelIntroduce.Margin = new Padding(2, 0, 2, 0);
            labelIntroduce.Name = "labelIntroduce";
            labelIntroduce.Size = new Size(663, 248);
            labelIntroduce.TabIndex = 3;
            labelIntroduce.Text = resources.GetString("labelIntroduce.Text");
            // 
            // StrangeQuestionAndAnswer
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(793, 587);
            Controls.Add(labelIntroduce);
            Controls.Add(labelTitle);
            Controls.Add(labelWinCount);
            Controls.Add(buttonStart);
            Margin = new Padding(2);
            Name = "StrangeQuestionAndAnswer";
            Text = "奇葩问答";
            Load += StrangeQuestionAndAnswer_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonStart;
        private Label labelWinCount;
        private Label labelTitle;
        private Label labelIntroduce;
    }
}