namespace QisToolkit3.Forms
{
    partial class WhatToEatToday
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
            comboBox_RamType = new ComboBox();
            button_DoRam = new Button();
            label_FoodName = new Label();
            button_Url = new Button();
            SuspendLayout();
            // 
            // comboBox_RamType
            // 
            comboBox_RamType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_RamType.Font = new Font("Microsoft YaHei UI", 32F);
            comboBox_RamType.FormattingEnabled = true;
            comboBox_RamType.Location = new Point(12, 12);
            comboBox_RamType.Name = "comboBox_RamType";
            comboBox_RamType.Size = new Size(384, 76);
            comboBox_RamType.TabIndex = 0;
            comboBox_RamType.SelectedIndexChanged += comboBox_RamType_SelectedIndexChanged;
            // 
            // button_DoRam
            // 
            button_DoRam.Font = new Font("Microsoft YaHei UI", 32F);
            button_DoRam.Location = new Point(402, 11);
            button_DoRam.Name = "button_DoRam";
            button_DoRam.Size = new Size(275, 76);
            button_DoRam.TabIndex = 1;
            button_DoRam.Text = "抽取";
            button_DoRam.UseVisualStyleBackColor = true;
            button_DoRam.Click += button_DoRam_Click;
            // 
            // label_FoodName
            // 
            label_FoodName.Font = new Font("Microsoft YaHei UI", 64F);
            label_FoodName.Location = new Point(12, 129);
            label_FoodName.Name = "label_FoodName";
            label_FoodName.Size = new Size(1201, 129);
            label_FoodName.TabIndex = 2;
            label_FoodName.Text = "FoodName";
            label_FoodName.TextAlign = ContentAlignment.MiddleCenter;
            label_FoodName.Visible = false;
            // 
            // button_Url
            // 
            button_Url.Font = new Font("Microsoft YaHei UI", 32F);
            button_Url.Location = new Point(915, 682);
            button_Url.Name = "button_Url";
            button_Url.Size = new Size(275, 76);
            button_Url.TabIndex = 3;
            button_Url.Text = "相关资源";
            button_Url.UseVisualStyleBackColor = true;
            button_Url.Click += button_Url_Click;
            // 
            // WhatToEatToday
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1225, 796);
            Controls.Add(button_Url);
            Controls.Add(label_FoodName);
            Controls.Add(button_DoRam);
            Controls.Add(comboBox_RamType);
            Margin = new Padding(2);
            Name = "WhatToEatToday";
            Text = "齐的工具包3：今天吃什么";
            Load += WhatToEatToday_Load;
            ResumeLayout(false);
        }

        #endregion

        private ComboBox comboBox_RamType;
        private Button button_DoRam;
        private Label label_FoodName;
        private Button button_Url;
    }
}