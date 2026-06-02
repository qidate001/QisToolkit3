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
            resources.ApplyResources(buttonStart, "buttonStart");
            buttonStart.ForeColor = Color.Black;
            buttonStart.Name = "buttonStart";
            buttonStart.UseVisualStyleBackColor = true;
            buttonStart.Click += buttonStart_Click;
            // 
            // labelWinCount
            // 
            resources.ApplyResources(labelWinCount, "labelWinCount");
            labelWinCount.ForeColor = Color.Black;
            labelWinCount.Name = "labelWinCount";
            labelWinCount.Click += labelWinCount_Click;
            // 
            // labelTitle
            // 
            resources.ApplyResources(labelTitle, "labelTitle");
            labelTitle.ForeColor = Color.Black;
            labelTitle.Name = "labelTitle";
            // 
            // labelIntroduce
            // 
            resources.ApplyResources(labelIntroduce, "labelIntroduce");
            labelIntroduce.ForeColor = Color.Black;
            labelIntroduce.Name = "labelIntroduce";
            // 
            // StrangeQuestionAndAnswer
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(labelIntroduce);
            Controls.Add(labelTitle);
            Controls.Add(labelWinCount);
            Controls.Add(buttonStart);
            Name = "StrangeQuestionAndAnswer";
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