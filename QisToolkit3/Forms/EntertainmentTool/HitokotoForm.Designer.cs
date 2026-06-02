namespace QisToolkit3.Forms
{
    partial class HitokotoForm
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
            label = new Label();
            button_GetText = new Button();
            SuspendLayout();
            // 
            // label
            // 
            label.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label.BackColor = Color.FromArgb(224, 224, 224);
            label.Font = new Font("Microsoft YaHei UI", 24F);
            label.Location = new Point(16, 12);
            label.Margin = new Padding(4, 0, 4, 0);
            label.Name = "label";
            label.Size = new Size(1002, 294);
            label.TabIndex = 0;
            label.Text = "用代码表达言语的魅力，用代码书写山河的壮丽。\r\n—— 一言 开发者中心\r\n\r\n\r\n（点击获取按钮随机显示一条短语）";
            // 
            // button_GetText
            // 
            button_GetText.Font = new Font("Microsoft YaHei UI", 48F);
            button_GetText.Location = new Point(16, 325);
            button_GetText.Margin = new Padding(4);
            button_GetText.Name = "button_GetText";
            button_GetText.Size = new Size(1002, 160);
            button_GetText.TabIndex = 1;
            button_GetText.Text = "获取";
            button_GetText.UseVisualStyleBackColor = true;
            button_GetText.Click += button_GetText_Click;
            // 
            // HitokotoForm
            // 
            AutoScaleDimensions = new SizeF(12F, 27F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1031, 498);
            Controls.Add(button_GetText);
            Controls.Add(label);
            Font = new Font("Microsoft YaHei UI", 12F);
            Margin = new Padding(4);
            Name = "HitokotoForm";
            Text = "一言：用代码表达言语的魅力，用代码书写山河的壮丽。";
            Load += HitokotoForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private Label label;
        private Button button_GetText;
    }
}