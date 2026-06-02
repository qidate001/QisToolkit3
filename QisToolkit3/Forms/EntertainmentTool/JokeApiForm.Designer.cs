namespace QisToolkit3.Forms.EntertainmentTool
{
    partial class JokeApiForm
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
            button_GetText = new Button();
            label = new Label();
            checkBox_Language = new CheckBox();
            comboBox_Language = new ComboBox();
            checkBox_BlackListFlags = new CheckBox();
            comboBox_BlackListFlags = new ComboBox();
            comboBox_Type = new ComboBox();
            SuspendLayout();
            // 
            // button_GetText
            // 
            button_GetText.Font = new Font("Microsoft YaHei UI", 48F);
            button_GetText.Location = new Point(13, 308);
            button_GetText.Margin = new Padding(4);
            button_GetText.Name = "button_GetText";
            button_GetText.Size = new Size(407, 192);
            button_GetText.TabIndex = 3;
            button_GetText.Text = "获取";
            button_GetText.UseVisualStyleBackColor = true;
            button_GetText.Click += button_GetText_Click;
            // 
            // label
            // 
            label.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label.BackColor = Color.FromArgb(224, 224, 224);
            label.Font = new Font("Microsoft YaHei UI", 24F);
            label.Location = new Point(13, 9);
            label.Margin = new Padding(4, 0, 4, 0);
            label.Name = "label";
            label.Size = new Size(1002, 294);
            label.TabIndex = 2;
            label.Text = "问：为什么程序员总是分不清万圣节和圣诞节？\r\n答：因为 Oct 31 = Dec 25";
            // 
            // checkBox_Language
            // 
            checkBox_Language.AutoSize = true;
            checkBox_Language.Enabled = false;
            checkBox_Language.Font = new Font("Microsoft YaHei UI", 24F);
            checkBox_Language.Location = new Point(427, 308);
            checkBox_Language.Name = "checkBox_Language";
            checkBox_Language.Size = new Size(164, 56);
            checkBox_Language.TabIndex = 4;
            checkBox_Language.Text = "语言：";
            checkBox_Language.UseVisualStyleBackColor = true;
            // 
            // comboBox_Language
            // 
            comboBox_Language.Enabled = false;
            comboBox_Language.Font = new Font("Microsoft YaHei UI", 24F);
            comboBox_Language.FormattingEnabled = true;
            comboBox_Language.Items.AddRange(new object[] { "zh", "de", "fr", "es" });
            comboBox_Language.Location = new Point(562, 306);
            comboBox_Language.Name = "comboBox_Language";
            comboBox_Language.Size = new Size(457, 60);
            comboBox_Language.TabIndex = 5;
            comboBox_Language.Text = "zh";
            // 
            // checkBox_BlackListFlags
            // 
            checkBox_BlackListFlags.AutoSize = true;
            checkBox_BlackListFlags.Checked = true;
            checkBox_BlackListFlags.CheckState = CheckState.Checked;
            checkBox_BlackListFlags.Font = new Font("Microsoft YaHei UI", 24F);
            checkBox_BlackListFlags.Location = new Point(427, 376);
            checkBox_BlackListFlags.Name = "checkBox_BlackListFlags";
            checkBox_BlackListFlags.Size = new Size(164, 56);
            checkBox_BlackListFlags.TabIndex = 6;
            checkBox_BlackListFlags.Text = "过滤：";
            checkBox_BlackListFlags.UseVisualStyleBackColor = true;
            // 
            // comboBox_BlackListFlags
            // 
            comboBox_BlackListFlags.Font = new Font("Microsoft YaHei UI", 24F);
            comboBox_BlackListFlags.FormattingEnabled = true;
            comboBox_BlackListFlags.Items.AddRange(new object[] { "nsfw", "racist", "sexist", "political", "nsfw,racist,sexist,political" });
            comboBox_BlackListFlags.Location = new Point(562, 374);
            comboBox_BlackListFlags.Name = "comboBox_BlackListFlags";
            comboBox_BlackListFlags.Size = new Size(457, 60);
            comboBox_BlackListFlags.TabIndex = 7;
            comboBox_BlackListFlags.Text = "nsfw,racist,sexist,political";
            // 
            // comboBox_Type
            // 
            comboBox_Type.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Type.Font = new Font("Microsoft YaHei UI", 24F);
            comboBox_Type.FormattingEnabled = true;
            comboBox_Type.Items.AddRange(new object[] { "任意", "程序员", "杂项笑话", "黑色幽默", "双关语", "万圣节", "圣诞节" });
            comboBox_Type.Location = new Point(427, 440);
            comboBox_Type.Name = "comboBox_Type";
            comboBox_Type.Size = new Size(592, 60);
            comboBox_Type.TabIndex = 8;
            // 
            // JokeApiForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1031, 505);
            Controls.Add(comboBox_Type);
            Controls.Add(comboBox_BlackListFlags);
            Controls.Add(checkBox_BlackListFlags);
            Controls.Add(comboBox_Language);
            Controls.Add(checkBox_Language);
            Controls.Add(button_GetText);
            Controls.Add(label);
            Name = "JokeApiForm";
            Text = "JokeAPI工具";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_GetText;
        private Label label;
        private CheckBox checkBox_Language;
        private ComboBox comboBox_Language;
        private CheckBox checkBox_BlackListFlags;
        private ComboBox comboBox_BlackListFlags;
        private ComboBox comboBox_Type;
    }
}