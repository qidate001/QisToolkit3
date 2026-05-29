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
            button_A.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button_A.Font = new Font("微软雅黑", 25F);
            button_A.Location = new Point(224, 375);
            button_A.Margin = new Padding(2);
            button_A.Name = "button_A";
            button_A.Size = new Size(758, 91);
            button_A.TabIndex = 0;
            button_A.Text = "A. 等待初始化";
            button_A.TextAlign = ContentAlignment.MiddleLeft;
            button_A.UseVisualStyleBackColor = true;
            button_A.Click += button_A_Click;
            // 
            // button_B
            // 
            button_B.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button_B.Font = new Font("微软雅黑", 25F);
            button_B.Location = new Point(224, 471);
            button_B.Margin = new Padding(2);
            button_B.Name = "button_B";
            button_B.Size = new Size(758, 91);
            button_B.TabIndex = 1;
            button_B.Text = "B. 等待初始化";
            button_B.TextAlign = ContentAlignment.MiddleLeft;
            button_B.UseVisualStyleBackColor = true;
            button_B.Click += button_B_Click;
            // 
            // button_C
            // 
            button_C.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button_C.Font = new Font("微软雅黑", 25F);
            button_C.Location = new Point(224, 567);
            button_C.Margin = new Padding(2);
            button_C.Name = "button_C";
            button_C.Size = new Size(758, 91);
            button_C.TabIndex = 3;
            button_C.Text = "C. 等待初始化";
            button_C.TextAlign = ContentAlignment.MiddleLeft;
            button_C.UseVisualStyleBackColor = true;
            button_C.Click += button_C_Click;
            // 
            // button_D
            // 
            button_D.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button_D.Font = new Font("微软雅黑", 25F);
            button_D.Location = new Point(224, 662);
            button_D.Margin = new Padding(2);
            button_D.Name = "button_D";
            button_D.Size = new Size(758, 91);
            button_D.TabIndex = 4;
            button_D.Text = "D. 等待初始化";
            button_D.TextAlign = ContentAlignment.MiddleLeft;
            button_D.UseVisualStyleBackColor = true;
            button_D.Click += button_D_Click;
            // 
            // label_Topic
            // 
            label_Topic.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label_Topic.BackColor = Color.FromArgb(224, 224, 224);
            label_Topic.Font = new Font("微软雅黑", 30F);
            label_Topic.Location = new Point(10, 17);
            label_Topic.Margin = new Padding(2, 0, 2, 0);
            label_Topic.Name = "label_Topic";
            label_Topic.Size = new Size(1198, 345);
            label_Topic.TabIndex = 5;
            label_Topic.Text = "题目";
            label_Topic.Click += label_Topic_Click;
            // 
            // labelQuestionCount
            // 
            labelQuestionCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labelQuestionCount.AutoSize = true;
            labelQuestionCount.Font = new Font("微软雅黑", 20F);
            labelQuestionCount.Location = new Point(11, 716);
            labelQuestionCount.Margin = new Padding(2, 0, 2, 0);
            labelQuestionCount.Name = "labelQuestionCount";
            labelQuestionCount.Size = new Size(95, 45);
            labelQuestionCount.TabIndex = 6;
            labelQuestionCount.Text = "1/10";
            // 
            // label_DeBug1
            // 
            label_DeBug1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label_DeBug1.AutoSize = true;
            label_DeBug1.Font = new Font("黑体", 12F);
            label_DeBug1.Location = new Point(1006, 738);
            label_DeBug1.Name = "label_DeBug1";
            label_DeBug1.Size = new Size(89, 20);
            label_DeBug1.TabIndex = 7;
            label_DeBug1.Text = "题目Id: ";
            label_DeBug1.Visible = false;
            // 
            // label_DeBug2
            // 
            label_DeBug2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label_DeBug2.AutoSize = true;
            label_DeBug2.Font = new Font("黑体", 12F);
            label_DeBug2.Location = new Point(1006, 707);
            label_DeBug2.Name = "label_DeBug2";
            label_DeBug2.Size = new Size(179, 20);
            label_DeBug2.TabIndex = 8;
            label_DeBug2.Text = "0 | 0 | 0 | 0 | 0";
            label_DeBug2.Visible = false;
            // 
            // StrangeQuestionAndAnswerMain
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1219, 770);
            Controls.Add(label_DeBug2);
            Controls.Add(label_DeBug1);
            Controls.Add(labelQuestionCount);
            Controls.Add(label_Topic);
            Controls.Add(button_D);
            Controls.Add(button_C);
            Controls.Add(button_B);
            Controls.Add(button_A);
            Margin = new Padding(2);
            Name = "StrangeQuestionAndAnswerMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "奇葩问答";
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