namespace QisToolkit3.Forms
{
    partial class StrangeFoods
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            panel1 = new Panel();
            button_add_egg = new Button();
            button_add_rise = new Button();
            button_add_flour = new Button();
            progressBar_MakeFood = new ProgressBar();
            label_MakeFoods_1 = new Label();
            label_MakeFoods_3 = new Label();
            label_MakeFoods_4 = new Label();
            label_MakeFoods_2 = new Label();
            label_MakeFoods_0 = new Label();
            button_MakeFood = new Button();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            tabPage4 = new TabPage();
            panel3 = new Panel();
            label2 = new Label();
            label1 = new Label();
            panel2 = new Panel();
            tabPage5 = new TabPage();
            label_Gold = new Label();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            panel_DevelopLogo = new Panel();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            panel1.SuspendLayout();
            tabPage4.SuspendLayout();
            panel3.SuspendLayout();
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
            splitContainer1.Panel1.Controls.Add(tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.AutoScroll = true;
            splitContainer1.Panel2.Controls.Add(label_Gold);
            splitContainer1.Size = new Size(1217, 859);
            splitContainer1.SplitterDistance = 902;
            splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Font = new Font("微软雅黑", 29F);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(902, 859);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(panel_DevelopLogo);
            tabPage1.Controls.Add(panel1);
            tabPage1.Controls.Add(progressBar_MakeFood);
            tabPage1.Controls.Add(label_MakeFoods_1);
            tabPage1.Controls.Add(label_MakeFoods_3);
            tabPage1.Controls.Add(label_MakeFoods_4);
            tabPage1.Controls.Add(label_MakeFoods_2);
            tabPage1.Controls.Add(label_MakeFoods_0);
            tabPage1.Controls.Add(button_MakeFood);
            tabPage1.Font = new Font("微软雅黑", 12.5F);
            tabPage1.Location = new Point(4, 71);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(894, 784);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "研制菜";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom;
            panel1.AutoScroll = true;
            panel1.Controls.Add(button_add_egg);
            panel1.Controls.Add(button_add_rise);
            panel1.Controls.Add(button_add_flour);
            panel1.Location = new Point(6, 482);
            panel1.Name = "panel1";
            panel1.Size = new Size(882, 294);
            panel1.TabIndex = 9;
            // 
            // button_add_egg
            // 
            button_add_egg.BackgroundImageLayout = ImageLayout.Stretch;
            button_add_egg.Font = new Font("微软雅黑", 8.5F);
            button_add_egg.Location = new Point(271, 3);
            button_add_egg.Name = "button_add_egg";
            button_add_egg.Size = new Size(128, 128);
            button_add_egg.TabIndex = 2;
            button_add_egg.TextAlign = ContentAlignment.BottomCenter;
            button_add_egg.TextImageRelation = TextImageRelation.TextAboveImage;
            button_add_egg.UseVisualStyleBackColor = true;
            button_add_egg.Click += button_add_egg_Click;
            // 
            // button_add_rise
            // 
            button_add_rise.BackgroundImageLayout = ImageLayout.Stretch;
            button_add_rise.Font = new Font("微软雅黑", 8.5F);
            button_add_rise.Location = new Point(137, 3);
            button_add_rise.Name = "button_add_rise";
            button_add_rise.Size = new Size(128, 128);
            button_add_rise.TabIndex = 1;
            button_add_rise.TextAlign = ContentAlignment.BottomCenter;
            button_add_rise.TextImageRelation = TextImageRelation.TextAboveImage;
            button_add_rise.UseVisualStyleBackColor = true;
            button_add_rise.Click += button_add_rise_Click;
            // 
            // button_add_flour
            // 
            button_add_flour.BackgroundImageLayout = ImageLayout.Stretch;
            button_add_flour.Font = new Font("微软雅黑", 8.5F);
            button_add_flour.Location = new Point(3, 3);
            button_add_flour.Name = "button_add_flour";
            button_add_flour.Size = new Size(128, 128);
            button_add_flour.TabIndex = 0;
            button_add_flour.TextAlign = ContentAlignment.BottomCenter;
            button_add_flour.TextImageRelation = TextImageRelation.TextAboveImage;
            button_add_flour.UseVisualStyleBackColor = true;
            button_add_flour.Click += button_flour_Click;
            // 
            // progressBar_MakeFood
            // 
            progressBar_MakeFood.Anchor = AnchorStyles.Bottom;
            progressBar_MakeFood.Location = new Point(221, 356);
            progressBar_MakeFood.Name = "progressBar_MakeFood";
            progressBar_MakeFood.Size = new Size(476, 29);
            progressBar_MakeFood.TabIndex = 8;
            progressBar_MakeFood.Visible = false;
            // 
            // label_MakeFoods_1
            // 
            label_MakeFoods_1.Anchor = AnchorStyles.Top;
            label_MakeFoods_1.BackColor = Color.LightGray;
            label_MakeFoods_1.Font = new Font("微软雅黑", 25F);
            label_MakeFoods_1.Location = new Point(568, 209);
            label_MakeFoods_1.Name = "label_MakeFoods_1";
            label_MakeFoods_1.Size = new Size(234, 52);
            label_MakeFoods_1.TabIndex = 7;
            label_MakeFoods_1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_MakeFoods_3
            // 
            label_MakeFoods_3.Anchor = AnchorStyles.Top;
            label_MakeFoods_3.BackColor = Color.LightGray;
            label_MakeFoods_3.Font = new Font("微软雅黑", 25F);
            label_MakeFoods_3.Location = new Point(488, 121);
            label_MakeFoods_3.Name = "label_MakeFoods_3";
            label_MakeFoods_3.Size = new Size(234, 52);
            label_MakeFoods_3.TabIndex = 6;
            label_MakeFoods_3.Text = "已损坏";
            label_MakeFoods_3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_MakeFoods_4
            // 
            label_MakeFoods_4.Anchor = AnchorStyles.Top;
            label_MakeFoods_4.BackColor = Color.LightGray;
            label_MakeFoods_4.Font = new Font("微软雅黑", 25F);
            label_MakeFoods_4.Location = new Point(345, 38);
            label_MakeFoods_4.Name = "label_MakeFoods_4";
            label_MakeFoods_4.Size = new Size(234, 52);
            label_MakeFoods_4.TabIndex = 5;
            label_MakeFoods_4.Text = "已损坏";
            label_MakeFoods_4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_MakeFoods_2
            // 
            label_MakeFoods_2.Anchor = AnchorStyles.Top;
            label_MakeFoods_2.BackColor = Color.LightGray;
            label_MakeFoods_2.Font = new Font("微软雅黑", 25F);
            label_MakeFoods_2.Location = new Point(197, 121);
            label_MakeFoods_2.Name = "label_MakeFoods_2";
            label_MakeFoods_2.Size = new Size(234, 52);
            label_MakeFoods_2.TabIndex = 4;
            label_MakeFoods_2.Text = "已损坏";
            label_MakeFoods_2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_MakeFoods_0
            // 
            label_MakeFoods_0.Anchor = AnchorStyles.Top;
            label_MakeFoods_0.BackColor = Color.LightGray;
            label_MakeFoods_0.Font = new Font("微软雅黑", 25F);
            label_MakeFoods_0.Location = new Point(121, 209);
            label_MakeFoods_0.Name = "label_MakeFoods_0";
            label_MakeFoods_0.Size = new Size(234, 52);
            label_MakeFoods_0.TabIndex = 3;
            label_MakeFoods_0.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button_MakeFood
            // 
            button_MakeFood.Anchor = AnchorStyles.Bottom;
            button_MakeFood.Enabled = false;
            button_MakeFood.Font = new Font("微软雅黑", 33F);
            button_MakeFood.Location = new Point(221, 391);
            button_MakeFood.Name = "button_MakeFood";
            button_MakeFood.Size = new Size(476, 85);
            button_MakeFood.TabIndex = 2;
            button_MakeFood.Text = "开始研制";
            button_MakeFood.UseVisualStyleBackColor = true;
            button_MakeFood.Click += button_MakeFood_Click;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 71);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(894, 784);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "小厨房";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 71);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(894, 784);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "怪物库";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(panel3);
            tabPage4.Location = new Point(4, 71);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(894, 784);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "去冒险";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            panel3.Controls.Add(label2);
            panel3.Controls.Add(label1);
            panel3.Controls.Add(panel2);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(888, 135);
            panel3.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("微软雅黑", 12F);
            label2.Location = new Point(137, 104);
            label2.Name = "label2";
            label2.Size = new Size(199, 27);
            label2.TabIndex = 2;
            label2.Text = "当前产出速度：0.1/s";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(137, 3);
            label1.Name = "label1";
            label1.Size = new Size(174, 64);
            label1.TabIndex = 1;
            label1.Text = "大米怪";
            // 
            // panel2
            // 
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(128, 128);
            panel2.TabIndex = 0;
            // 
            // tabPage5
            // 
            tabPage5.Location = new Point(4, 71);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(894, 784);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "图鉴集";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // label_Gold
            // 
            label_Gold.AutoSize = true;
            label_Gold.Font = new Font("Microsoft YaHei UI", 14F);
            label_Gold.Location = new Point(6, 6);
            label_Gold.Name = "label_Gold";
            label_Gold.Size = new Size(133, 31);
            label_Gold.TabIndex = 0;
            label_Gold.Text = "label_Gold";
            // 
            // panel_DevelopLogo
            // 
            panel_DevelopLogo.Anchor = AnchorStyles.Top;
            panel_DevelopLogo.BackgroundImageLayout = ImageLayout.Stretch;
            panel_DevelopLogo.Location = new Point(394, 194);
            panel_DevelopLogo.Name = "panel_DevelopLogo";
            panel_DevelopLogo.Size = new Size(128, 128);
            panel_DevelopLogo.TabIndex = 3;
            // 
            // StrangeFoods
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1217, 859);
            Controls.Add(splitContainer1);
            Name = "StrangeFoods";
            Text = "奇怪的食物";
            Load += MakeFoodsGame_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private Button button_MakeFood;
        private Label label_MakeFoods_0;
        private Label label_MakeFoods_1;
        private Label label_MakeFoods_3;
        private Label label_MakeFoods_4;
        private Label label_MakeFoods_2;
        private ProgressBar progressBar_MakeFood;
        private Panel panel1;
        private Button button_add_flour;
        private Label label_Gold;
        private Panel panel2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Panel panel3;
        private Label label2;
        private Label label1;
        private Button button_add_egg;
        private Button button_add_rise;
        private Panel panel_DevelopLogo;
    }
}