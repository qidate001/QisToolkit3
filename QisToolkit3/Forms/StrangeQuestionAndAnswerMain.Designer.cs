namespace QisToolkit3.Forms
{
    partial class StrangeQuestionAndAnswerMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StrangeQuestionAndAnswerMain));
            button_A = new Button();
            button_B = new Button();
            button_C = new Button();
            button_D = new Button();
            label_Topic = new Label();
            labelQuestionCount = new Label();
            label_DeBug1 = new Label();
            label_DeBug2 = new Label();
            SuspendLayout();
            // 
            // button_A
            // 
            resources.ApplyResources(button_A, "button_A");
            button_A.Name = "button_A";
            button_A.UseVisualStyleBackColor = true;
            button_A.Click += button_A_Click;
            // 
            // button_B
            // 
            resources.ApplyResources(button_B, "button_B");
            button_B.Name = "button_B";
            button_B.UseVisualStyleBackColor = true;
            button_B.Click += button_B_Click;
            // 
            // button_C
            // 
            resources.ApplyResources(button_C, "button_C");
            button_C.Name = "button_C";
            button_C.UseVisualStyleBackColor = true;
            button_C.Click += button_C_Click;
            // 
            // button_D
            // 
            resources.ApplyResources(button_D, "button_D");
            button_D.Name = "button_D";
            button_D.UseVisualStyleBackColor = true;
            button_D.Click += button_D_Click;
            // 
            // label_Topic
            // 
            resources.ApplyResources(label_Topic, "label_Topic");
            label_Topic.BackColor = Color.FromArgb(224, 224, 224);
            label_Topic.Name = "label_Topic";
            label_Topic.Click += label_Topic_Click;
            // 
            // labelQuestionCount
            // 
            resources.ApplyResources(labelQuestionCount, "labelQuestionCount");
            labelQuestionCount.Name = "labelQuestionCount";
            // 
            // label_DeBug1
            // 
            resources.ApplyResources(label_DeBug1, "label_DeBug1");
            label_DeBug1.Name = "label_DeBug1";
            // 
            // label_DeBug2
            // 
            resources.ApplyResources(label_DeBug2, "label_DeBug2");
            label_DeBug2.Name = "label_DeBug2";
            // 
            // StrangeQuestionAndAnswerMain
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label_DeBug2);
            Controls.Add(label_DeBug1);
            Controls.Add(labelQuestionCount);
            Controls.Add(label_Topic);
            Controls.Add(button_D);
            Controls.Add(button_C);
            Controls.Add(button_B);
            Controls.Add(button_A);
            Name = "StrangeQuestionAndAnswerMain";
            Load += StrangeQuestionAndAnswerMain_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_A;
        private Button button_B;
        private Button button_C;
        private Button button_D;
        private Label label_Topic;
        private Label labelQuestionCount;
        private Label label_DeBug1;
        private Label label_DeBug2;
    }
}