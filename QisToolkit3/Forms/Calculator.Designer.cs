namespace QisToolkit3.Forms
{
    partial class Calculator
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
            textBox_expression = new TextBox();
            label = new Label();
            label1 = new Label();
            button_Add_Sigma = new Button();
            SuspendLayout();
            // 
            // textBox_expression
            // 
            textBox_expression.Dock = DockStyle.Top;
            textBox_expression.Font = new Font("Microsoft YaHei UI", 22.2F, FontStyle.Regular, GraphicsUnit.Point, 134);
            textBox_expression.Location = new Point(0, 0);
            textBox_expression.Name = "textBox_expression";
            textBox_expression.Size = new Size(800, 54);
            textBox_expression.TabIndex = 0;
            textBox_expression.TextChanged += textBox_expression_TextChanged;
            // 
            // label
            // 
            label.Dock = DockStyle.Top;
            label.Font = new Font("Microsoft YaHei UI", 14F);
            label.Location = new Point(0, 54);
            label.Name = "label";
            label.Size = new Size(800, 113);
            label.TabIndex = 1;
            label.Text = "算式 = ?";
            label.Click += label_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 12F);
            label1.Location = new Point(320, 167);
            label1.Name = "label1";
            label1.Size = new Size(112, 27);
            label1.TabIndex = 2;
            label1.Text = "插入表达式";
            // 
            // button_Add_Sigma
            // 
            button_Add_Sigma.Font = new Font("Microsoft YaHei UI", 22F);
            button_Add_Sigma.Location = new Point(12, 202);
            button_Add_Sigma.Name = "button_Add_Sigma";
            button_Add_Sigma.Size = new Size(64, 64);
            button_Add_Sigma.TabIndex = 3;
            button_Add_Sigma.Text = "Σ";
            button_Add_Sigma.UseVisualStyleBackColor = true;
            button_Add_Sigma.Click += button_Add_Sigma_Click;
            // 
            // Calculator
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button_Add_Sigma);
            Controls.Add(label1);
            Controls.Add(label);
            Controls.Add(textBox_expression);
            Name = "Calculator";
            Text = "Calculator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox_expression;
        private Label label;
        private Label label1;
        private Button button_Add_Sigma;
    }
}