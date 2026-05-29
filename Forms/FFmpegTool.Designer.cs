namespace QisToolkit3.Forms
{
    partial class FFmpegTool
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FFmpegTool));
            splitContainer1 = new SplitContainer();
            button1 = new Button();
            button_Main = new Button();
            button_CopyCommand = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            groupBox_ssto = new GroupBox();
            radioButton_ssto_best = new RadioButton();
            radioButton_ssto_precise = new RadioButton();
            radioButton_ssto_fast = new RadioButton();
            comboBox_to = new ComboBox();
            checkBox_to = new CheckBox();
            comboBox_ss = new ComboBox();
            checkBox_ss = new CheckBox();
            comboBox1 = new ComboBox();
            checkBox1 = new CheckBox();
            tabPage3 = new TabPage();
            comboBox_r = new ComboBox();
            comboBox_c_v = new ComboBox();
            comboBox_b_v = new ComboBox();
            comboBox_s = new ComboBox();
            checkBox_c_v = new CheckBox();
            checkBox_b_v = new CheckBox();
            checkBox_r = new CheckBox();
            checkBox_s = new CheckBox();
            checkBox_crf = new CheckBox();
            comboBox_crf = new ComboBox();
            trackBar_crf = new TrackBar();
            tabPage4 = new TabPage();
            checkBox_c_a = new CheckBox();
            comboBox_c_a = new ComboBox();
            checkBox_b_a = new CheckBox();
            comboBox_b_a = new ComboBox();
            comboBox_ac = new ComboBox();
            checkBox_ac = new CheckBox();
            tabPage5 = new TabPage();
            checkBox_c_copy = new CheckBox();
            groupBox1 = new GroupBox();
            checkBox_vn = new CheckBox();
            checkBox_sn = new CheckBox();
            checkBox_an = new CheckBox();
            tabPage6 = new TabPage();
            button_Save_File = new Button();
            button_Open_File = new Button();
            comboBox_y_or_n_or_null = new ComboBox();
            comboBox_Output = new ComboBox();
            label2 = new Label();
            comboBox_Input = new ComboBox();
            label1 = new Label();
            button_Stop = new Button();
            button_Text = new Button();
            panel1 = new Panel();
            button__DoCopyCommand = new Button();
            richTextBox = new RichTextBox();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            toolTip = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox_ssto.SuspendLayout();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar_crf).BeginInit();
            tabPage4.SuspendLayout();
            tabPage5.SuspendLayout();
            groupBox1.SuspendLayout();
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
            splitContainer1.Panel1.Controls.Add(button1);
            splitContainer1.Panel1.Controls.Add(button_Main);
            splitContainer1.Panel1.Controls.Add(button_CopyCommand);
            splitContainer1.Panel1.Controls.Add(tabControl1);
            splitContainer1.Panel1.Controls.Add(button_Save_File);
            splitContainer1.Panel1.Controls.Add(button_Open_File);
            splitContainer1.Panel1.Controls.Add(comboBox_y_or_n_or_null);
            splitContainer1.Panel1.Controls.Add(comboBox_Output);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(comboBox_Input);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(button_Stop);
            splitContainer1.Panel1.Controls.Add(button_Text);
            splitContainer1.Panel1.Controls.Add(panel1);
            splitContainer1.Panel1.Controls.Add(button__DoCopyCommand);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(richTextBox);
            splitContainer1.Size = new Size(1553, 803);
            splitContainer1.SplitterDistance = 681;
            splitContainer1.TabIndex = 1;
            // 
            // button1
            // 
            button1.BackColor = Color.Red;
            button1.Location = new Point(526, 707);
            button1.Name = "button1";
            button1.Size = new Size(150, 29);
            button1.TabIndex = 53;
            button1.Text = "停止";
            button1.UseVisualStyleBackColor = false;
            // 
            // button_Main
            // 
            button_Main.Font = new Font("微软雅黑", 16.2F);
            button_Main.Location = new Point(12, 742);
            button_Main.Name = "button_Main";
            button_Main.Size = new Size(508, 55);
            button_Main.TabIndex = 52;
            button_Main.Text = "执行命令";
            button_Main.UseVisualStyleBackColor = true;
            button_Main.Click += button_Main_Click;
            // 
            // button_CopyCommand
            // 
            button_CopyCommand.Font = new Font("微软雅黑", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button_CopyCommand.Location = new Point(526, 742);
            button_CopyCommand.Name = "button_CopyCommand";
            button_CopyCommand.Size = new Size(150, 55);
            button_CopyCommand.TabIndex = 50;
            button_CopyCommand.Text = "复制命令";
            button_CopyCommand.UseVisualStyleBackColor = true;
            button_CopyCommand.Click += button_CopyCommand_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage6);
            tabControl1.Location = new Point(12, 106);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(664, 581);
            tabControl1.TabIndex = 49;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(groupBox_ssto);
            tabPage1.Controls.Add(comboBox1);
            tabPage1.Controls.Add(checkBox1);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(656, 548);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "通用选项";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox_ssto
            // 
            groupBox_ssto.Controls.Add(radioButton_ssto_best);
            groupBox_ssto.Controls.Add(radioButton_ssto_precise);
            groupBox_ssto.Controls.Add(radioButton_ssto_fast);
            groupBox_ssto.Controls.Add(comboBox_to);
            groupBox_ssto.Controls.Add(checkBox_to);
            groupBox_ssto.Controls.Add(comboBox_ss);
            groupBox_ssto.Controls.Add(checkBox_ss);
            groupBox_ssto.Font = new Font("Microsoft YaHei UI", 12F);
            groupBox_ssto.Location = new Point(6, 55);
            groupBox_ssto.Name = "groupBox_ssto";
            groupBox_ssto.Size = new Size(644, 272);
            groupBox_ssto.TabIndex = 52;
            groupBox_ssto.TabStop = false;
            groupBox_ssto.Text = "截取片段";
            // 
            // radioButton_ssto_best
            // 
            radioButton_ssto_best.AutoSize = true;
            radioButton_ssto_best.Checked = true;
            radioButton_ssto_best.Font = new Font("Microsoft YaHei UI", 16.2F);
            radioButton_ssto_best.Location = new Point(6, 219);
            radioButton_ssto_best.Name = "radioButton_ssto_best";
            radioButton_ssto_best.Size = new Size(148, 40);
            radioButton_ssto_best.TabIndex = 59;
            radioButton_ssto_best.TabStop = true;
            radioButton_ssto_best.Text = "最佳模式";
            radioButton_ssto_best.UseVisualStyleBackColor = true;
            radioButton_ssto_best.CheckedChanged += radioButton_ssto_best_CheckedChanged;
            // 
            // radioButton_ssto_precise
            // 
            radioButton_ssto_precise.AutoSize = true;
            radioButton_ssto_precise.Font = new Font("Microsoft YaHei UI", 16.2F);
            radioButton_ssto_precise.Location = new Point(6, 173);
            radioButton_ssto_precise.Name = "radioButton_ssto_precise";
            radioButton_ssto_precise.Size = new Size(148, 40);
            radioButton_ssto_precise.TabIndex = 58;
            radioButton_ssto_precise.Text = "精确模式";
            radioButton_ssto_precise.UseVisualStyleBackColor = true;
            radioButton_ssto_precise.CheckedChanged += radioButton_ssto_precise_CheckedChanged;
            // 
            // radioButton_ssto_fast
            // 
            radioButton_ssto_fast.AutoSize = true;
            radioButton_ssto_fast.Font = new Font("Microsoft YaHei UI", 16.2F);
            radioButton_ssto_fast.Location = new Point(6, 124);
            radioButton_ssto_fast.Name = "radioButton_ssto_fast";
            radioButton_ssto_fast.Size = new Size(148, 40);
            radioButton_ssto_fast.TabIndex = 57;
            radioButton_ssto_fast.TabStop = true;
            radioButton_ssto_fast.Text = "快速模式";
            radioButton_ssto_fast.UseVisualStyleBackColor = true;
            radioButton_ssto_fast.CheckedChanged += radioButton_ssto_fast_CheckedChanged;
            // 
            // comboBox_to
            // 
            comboBox_to.Font = new Font("微软雅黑", 16.2F);
            comboBox_to.FormattingEnabled = true;
            comboBox_to.Items.AddRange(new object[] { "00:00:00", "00:00:01", "00:01:00", "00:01:30", "00:05:00", "00:10:00", "00:20:00", "00:30:00", "01:00:00", "10", "20", "30", "60", "100", "120" });
            comboBox_to.Location = new Point(262, 77);
            comboBox_to.Name = "comboBox_to";
            comboBox_to.Size = new Size(198, 43);
            comboBox_to.TabIndex = 56;
            comboBox_to.Text = "00:01:30";
            // 
            // checkBox_to
            // 
            checkBox_to.AutoSize = true;
            checkBox_to.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_to.Location = new Point(6, 78);
            checkBox_to.Name = "checkBox_to";
            checkBox_to.Size = new Size(261, 40);
            checkBox_to.TabIndex = 55;
            checkBox_to.Text = "到指定时间点结束";
            toolTip.SetToolTip(checkBox_to, resources.GetString("checkBox_to.ToolTip"));
            checkBox_to.UseVisualStyleBackColor = true;
            // 
            // comboBox_ss
            // 
            comboBox_ss.Font = new Font("微软雅黑", 16.2F);
            comboBox_ss.FormattingEnabled = true;
            comboBox_ss.Items.AddRange(new object[] { "00:00:00", "00:00:01", "00:01:00", "00:01:30", "00:05:00", "00:10:00", "00:20:00", "00:30:00", "01:00:00" });
            comboBox_ss.Location = new Point(262, 31);
            comboBox_ss.Name = "comboBox_ss";
            comboBox_ss.Size = new Size(198, 43);
            comboBox_ss.TabIndex = 53;
            comboBox_ss.Text = "00:01:00";
            // 
            // checkBox_ss
            // 
            checkBox_ss.AutoSize = true;
            checkBox_ss.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_ss.Location = new Point(6, 32);
            checkBox_ss.Name = "checkBox_ss";
            checkBox_ss.Size = new Size(261, 40);
            checkBox_ss.TabIndex = 52;
            checkBox_ss.Text = "从指定时间点开始";
            toolTip.SetToolTip(checkBox_ss, resources.GetString("checkBox_ss.ToolTip"));
            checkBox_ss.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Font = new Font("微软雅黑", 16.2F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "安静模式（-8）", "恐慌模式（0）", "致命模式（8）", "错误模式（16）", "警告模式（24）", "信息模式（32）", "冗长模式（40）", "调试模式（48）" });
            comboBox1.Location = new Point(208, 6);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(247, 43);
            comboBox1.TabIndex = 51;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox1.Location = new Point(6, 7);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(205, 40);
            checkBox1.TabIndex = 50;
            checkBox1.Text = "日志详细程度";
            toolTip.SetToolTip(checkBox1, resources.GetString("checkBox1.ToolTip"));
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(comboBox_r);
            tabPage3.Controls.Add(comboBox_c_v);
            tabPage3.Controls.Add(comboBox_b_v);
            tabPage3.Controls.Add(comboBox_s);
            tabPage3.Controls.Add(checkBox_c_v);
            tabPage3.Controls.Add(checkBox_b_v);
            tabPage3.Controls.Add(checkBox_r);
            tabPage3.Controls.Add(checkBox_s);
            tabPage3.Controls.Add(checkBox_crf);
            tabPage3.Controls.Add(comboBox_crf);
            tabPage3.Controls.Add(trackBar_crf);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(656, 548);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "视频选项";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // comboBox_r
            // 
            comboBox_r.Enabled = false;
            comboBox_r.Font = new Font("微软雅黑", 16.2F);
            comboBox_r.FormattingEnabled = true;
            comboBox_r.Items.AddRange(new object[] { "180", "120", "60", "30", "15" });
            comboBox_r.Location = new Point(95, 141);
            comboBox_r.Name = "comboBox_r";
            comboBox_r.Size = new Size(78, 43);
            comboBox_r.TabIndex = 35;
            comboBox_r.Text = "60";
            // 
            // comboBox_c_v
            // 
            comboBox_c_v.Enabled = false;
            comboBox_c_v.Font = new Font("微软雅黑", 16.2F);
            comboBox_c_v.FormattingEnabled = true;
            comboBox_c_v.Items.AddRange(new object[] { "H.264", "H.265", "复制" });
            comboBox_c_v.Location = new Point(178, 2);
            comboBox_c_v.Name = "comboBox_c_v";
            comboBox_c_v.Size = new Size(116, 43);
            comboBox_c_v.TabIndex = 31;
            comboBox_c_v.Text = "H.265";
            // 
            // comboBox_b_v
            // 
            comboBox_b_v.Enabled = false;
            comboBox_b_v.Font = new Font("微软雅黑", 16.2F);
            comboBox_b_v.FormattingEnabled = true;
            comboBox_b_v.Items.AddRange(new object[] { "1M", "500K" });
            comboBox_b_v.Location = new Point(178, 50);
            comboBox_b_v.Name = "comboBox_b_v";
            comboBox_b_v.Size = new Size(116, 43);
            comboBox_b_v.TabIndex = 33;
            comboBox_b_v.Text = "1M";
            // 
            // comboBox_s
            // 
            comboBox_s.Enabled = false;
            comboBox_s.Font = new Font("微软雅黑", 16.2F);
            comboBox_s.FormattingEnabled = true;
            comboBox_s.Items.AddRange(new object[] { "180", "120", "60", "30", "15" });
            comboBox_s.Location = new Point(122, 96);
            comboBox_s.Name = "comboBox_s";
            comboBox_s.Size = new Size(199, 43);
            comboBox_s.TabIndex = 37;
            comboBox_s.Text = "1280x720";
            // 
            // checkBox_c_v
            // 
            checkBox_c_v.AutoSize = true;
            checkBox_c_v.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_c_v.Location = new Point(6, 3);
            checkBox_c_v.Name = "checkBox_c_v";
            checkBox_c_v.Size = new Size(177, 40);
            checkBox_c_v.TabIndex = 30;
            checkBox_c_v.Text = "视频编码器";
            checkBox_c_v.UseVisualStyleBackColor = true;
            checkBox_c_v.CheckedChanged += checkBox_c_v_CheckedChanged;
            // 
            // checkBox_b_v
            // 
            checkBox_b_v.AutoSize = true;
            checkBox_b_v.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_b_v.Location = new Point(6, 51);
            checkBox_b_v.Name = "checkBox_b_v";
            checkBox_b_v.Size = new Size(177, 40);
            checkBox_b_v.TabIndex = 32;
            checkBox_b_v.Text = "视频比特率";
            checkBox_b_v.UseVisualStyleBackColor = true;
            checkBox_b_v.CheckedChanged += checkBox_b_v_CheckedChanged;
            // 
            // checkBox_r
            // 
            checkBox_r.AutoSize = true;
            checkBox_r.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_r.Location = new Point(6, 143);
            checkBox_r.Name = "checkBox_r";
            checkBox_r.Size = new Size(93, 40);
            checkBox_r.TabIndex = 34;
            checkBox_r.Text = "帧率";
            checkBox_r.UseVisualStyleBackColor = true;
            checkBox_r.CheckedChanged += checkBox_r_CheckedChanged;
            // 
            // checkBox_s
            // 
            checkBox_s.AutoSize = true;
            checkBox_s.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_s.Location = new Point(6, 97);
            checkBox_s.Name = "checkBox_s";
            checkBox_s.Size = new Size(121, 40);
            checkBox_s.TabIndex = 36;
            checkBox_s.Text = "分辨率";
            checkBox_s.UseVisualStyleBackColor = true;
            checkBox_s.CheckedChanged += checkBox_s_CheckedChanged;
            // 
            // checkBox_crf
            // 
            checkBox_crf.AutoSize = true;
            checkBox_crf.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_crf.Location = new Point(131, 279);
            checkBox_crf.Name = "checkBox_crf";
            checkBox_crf.Size = new Size(205, 40);
            checkBox_crf.TabIndex = 27;
            checkBox_crf.Text = "恒定速率因子";
            checkBox_crf.UseVisualStyleBackColor = true;
            checkBox_crf.CheckedChanged += checkBox_crf_CheckedChanged;
            // 
            // comboBox_crf
            // 
            comboBox_crf.Enabled = false;
            comboBox_crf.Font = new Font("微软雅黑", 16.2F);
            comboBox_crf.FormattingEnabled = true;
            comboBox_crf.Items.AddRange(new object[] { "极佳", "优秀", "良好", "一般", "较差" });
            comboBox_crf.Location = new Point(522, 278);
            comboBox_crf.Name = "comboBox_crf";
            comboBox_crf.Size = new Size(86, 43);
            comboBox_crf.TabIndex = 28;
            comboBox_crf.Text = "23";
            comboBox_crf.SelectedIndexChanged += comboBox_crf_SelectedIndexChanged;
            comboBox_crf.TextChanged += comboBox_crf_SelectedIndexChanged;
            // 
            // trackBar_crf
            // 
            trackBar_crf.Enabled = false;
            trackBar_crf.Location = new Point(331, 279);
            trackBar_crf.Maximum = 51;
            trackBar_crf.Name = "trackBar_crf";
            trackBar_crf.Size = new Size(185, 56);
            trackBar_crf.TabIndex = 29;
            trackBar_crf.Value = 23;
            trackBar_crf.Scroll += trackBar_crf_Scroll;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(checkBox_c_a);
            tabPage4.Controls.Add(comboBox_c_a);
            tabPage4.Controls.Add(checkBox_b_a);
            tabPage4.Controls.Add(comboBox_b_a);
            tabPage4.Controls.Add(comboBox_ac);
            tabPage4.Controls.Add(checkBox_ac);
            tabPage4.Location = new Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(656, 548);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "音频选项";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // checkBox_c_a
            // 
            checkBox_c_a.AutoSize = true;
            checkBox_c_a.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_c_a.Location = new Point(178, 17);
            checkBox_c_a.Name = "checkBox_c_a";
            checkBox_c_a.Size = new Size(177, 40);
            checkBox_c_a.TabIndex = 38;
            checkBox_c_a.Text = "音频编码器";
            checkBox_c_a.UseVisualStyleBackColor = true;
            checkBox_c_a.CheckedChanged += checkBox_c_a_CheckedChanged;
            // 
            // comboBox_c_a
            // 
            comboBox_c_a.Enabled = false;
            comboBox_c_a.Font = new Font("微软雅黑", 16.2F);
            comboBox_c_a.FormattingEnabled = true;
            comboBox_c_a.Items.AddRange(new object[] { "MP3编码器", "AAC编码器", "OGG编码器", "FLAC编码器", "OPUS编码器", "AC3编码器", "复制" });
            comboBox_c_a.Location = new Point(350, 16);
            comboBox_c_a.Name = "comboBox_c_a";
            comboBox_c_a.Size = new Size(185, 43);
            comboBox_c_a.TabIndex = 39;
            comboBox_c_a.Text = "MP3编码器";
            // 
            // checkBox_b_a
            // 
            checkBox_b_a.AutoSize = true;
            checkBox_b_a.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_b_a.Location = new Point(178, 65);
            checkBox_b_a.Name = "checkBox_b_a";
            checkBox_b_a.Size = new Size(177, 40);
            checkBox_b_a.TabIndex = 40;
            checkBox_b_a.Text = "音频比特率";
            checkBox_b_a.UseVisualStyleBackColor = true;
            checkBox_b_a.CheckedChanged += checkBox_b_a_CheckedChanged;
            // 
            // comboBox_b_a
            // 
            comboBox_b_a.Enabled = false;
            comboBox_b_a.Font = new Font("微软雅黑", 16.2F);
            comboBox_b_a.FormattingEnabled = true;
            comboBox_b_a.Items.AddRange(new object[] { "128k", "198k" });
            comboBox_b_a.Location = new Point(350, 64);
            comboBox_b_a.Name = "comboBox_b_a";
            comboBox_b_a.Size = new Size(185, 43);
            comboBox_b_a.TabIndex = 41;
            comboBox_b_a.Text = "198k";
            // 
            // comboBox_ac
            // 
            comboBox_ac.Enabled = false;
            comboBox_ac.Font = new Font("微软雅黑", 16.2F);
            comboBox_ac.FormattingEnabled = true;
            comboBox_ac.Items.AddRange(new object[] { "1", "2" });
            comboBox_ac.Location = new Point(350, 112);
            comboBox_ac.Name = "comboBox_ac";
            comboBox_ac.Size = new Size(78, 43);
            comboBox_ac.TabIndex = 43;
            comboBox_ac.Text = "2";
            // 
            // checkBox_ac
            // 
            checkBox_ac.AutoSize = true;
            checkBox_ac.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_ac.Location = new Point(234, 113);
            checkBox_ac.Name = "checkBox_ac";
            checkBox_ac.Size = new Size(121, 40);
            checkBox_ac.TabIndex = 42;
            checkBox_ac.Text = "声道数";
            checkBox_ac.UseVisualStyleBackColor = true;
            checkBox_ac.CheckedChanged += checkBox_ac_CheckedChanged;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(checkBox_c_copy);
            tabPage5.Controls.Add(groupBox1);
            tabPage5.Location = new Point(4, 29);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(656, 548);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "流映射项";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // checkBox_c_copy
            // 
            checkBox_c_copy.AutoSize = true;
            checkBox_c_copy.Checked = true;
            checkBox_c_copy.CheckState = CheckState.Checked;
            checkBox_c_copy.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_c_copy.Location = new Point(201, 6);
            checkBox_c_copy.Name = "checkBox_c_copy";
            checkBox_c_copy.Size = new Size(177, 40);
            checkBox_c_copy.TabIndex = 54;
            checkBox_c_copy.Text = "流拷贝模式";
            checkBox_c_copy.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(checkBox_vn);
            groupBox1.Controls.Add(checkBox_sn);
            groupBox1.Controls.Add(checkBox_an);
            groupBox1.Font = new Font("Microsoft YaHei UI", 12F);
            groupBox1.Location = new Point(6, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(189, 182);
            groupBox1.TabIndex = 53;
            groupBox1.TabStop = false;
            groupBox1.Text = "禁用流";
            // 
            // checkBox_vn
            // 
            checkBox_vn.AutoSize = true;
            checkBox_vn.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_vn.Location = new Point(6, 32);
            checkBox_vn.Name = "checkBox_vn";
            checkBox_vn.Size = new Size(177, 40);
            checkBox_vn.TabIndex = 24;
            checkBox_vn.Text = "禁用视频流";
            checkBox_vn.UseVisualStyleBackColor = true;
            // 
            // checkBox_sn
            // 
            checkBox_sn.AutoSize = true;
            checkBox_sn.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_sn.Location = new Point(6, 124);
            checkBox_sn.Name = "checkBox_sn";
            checkBox_sn.Size = new Size(177, 40);
            checkBox_sn.TabIndex = 26;
            checkBox_sn.Text = "禁用字幕流";
            checkBox_sn.UseVisualStyleBackColor = true;
            // 
            // checkBox_an
            // 
            checkBox_an.AutoSize = true;
            checkBox_an.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_an.Location = new Point(6, 78);
            checkBox_an.Name = "checkBox_an";
            checkBox_an.Size = new Size(177, 40);
            checkBox_an.TabIndex = 25;
            checkBox_an.Text = "禁用音频流";
            checkBox_an.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            tabPage6.Location = new Point(4, 29);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new Size(656, 548);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "tabPage6";
            tabPage6.UseVisualStyleBackColor = true;
            // 
            // button_Save_File
            // 
            button_Save_File.Location = new Point(623, 57);
            button_Save_File.Name = "button_Save_File";
            button_Save_File.Size = new Size(53, 43);
            button_Save_File.TabIndex = 48;
            button_Save_File.Text = "保存";
            button_Save_File.UseVisualStyleBackColor = true;
            button_Save_File.Click += button_Save_File_Click;
            // 
            // button_Open_File
            // 
            button_Open_File.Location = new Point(623, 8);
            button_Open_File.Name = "button_Open_File";
            button_Open_File.Size = new Size(53, 43);
            button_Open_File.TabIndex = 47;
            button_Open_File.Text = "打开";
            button_Open_File.UseVisualStyleBackColor = true;
            button_Open_File.Click += button_Open_File_Click;
            // 
            // comboBox_y_or_n_or_null
            // 
            comboBox_y_or_n_or_null.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_y_or_n_or_null.Font = new Font("微软雅黑", 16.2F);
            comboBox_y_or_n_or_null.FormattingEnabled = true;
            comboBox_y_or_n_or_null.Items.AddRange(new object[] { "自动覆盖已存在文件", "不覆盖已存在文件", "默认" });
            comboBox_y_or_n_or_null.Location = new Point(12, 693);
            comboBox_y_or_n_or_null.Name = "comboBox_y_or_n_or_null";
            comboBox_y_or_n_or_null.Size = new Size(275, 43);
            comboBox_y_or_n_or_null.TabIndex = 45;
            // 
            // comboBox_Output
            // 
            comboBox_Output.Font = new Font("微软雅黑", 16.2F);
            comboBox_Output.FormattingEnabled = true;
            comboBox_Output.Items.AddRange(new object[] { "\\yt-dlp\\output.mp3" });
            comboBox_Output.Location = new Point(143, 57);
            comboBox_Output.Name = "comboBox_Output";
            comboBox_Output.Size = new Size(474, 43);
            comboBox_Output.TabIndex = 21;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("微软雅黑", 16.2F);
            label2.Location = new Point(3, 60);
            label2.Name = "label2";
            label2.Size = new Size(155, 36);
            label2.TabIndex = 20;
            label2.Text = "输出文件：";
            label2.Click += label2_Click;
            // 
            // comboBox_Input
            // 
            comboBox_Input.Font = new Font("微软雅黑", 16.2F);
            comboBox_Input.FormattingEnabled = true;
            comboBox_Input.Items.AddRange(new object[] { "\\yt-dlp\\input.mp3" });
            comboBox_Input.Location = new Point(143, 8);
            comboBox_Input.Name = "comboBox_Input";
            comboBox_Input.Size = new Size(474, 43);
            comboBox_Input.TabIndex = 19;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("微软雅黑", 16.2F);
            label1.Location = new Point(3, 11);
            label1.Name = "label1";
            label1.Size = new Size(155, 36);
            label1.TabIndex = 18;
            label1.Text = "输入文件：";
            label1.Click += label1_Click;
            // 
            // button_Stop
            // 
            button_Stop.Anchor = AnchorStyles.Bottom;
            button_Stop.BackColor = Color.Red;
            button_Stop.Location = new Point(1077, 1372);
            button_Stop.Name = "button_Stop";
            button_Stop.Size = new Size(94, 29);
            button_Stop.TabIndex = 15;
            button_Stop.Text = "停止";
            button_Stop.UseVisualStyleBackColor = false;
            // 
            // button_Text
            // 
            button_Text.Anchor = AnchorStyles.Bottom;
            button_Text.Location = new Point(1177, 1372);
            button_Text.Name = "button_Text";
            button_Text.Size = new Size(94, 29);
            button_Text.TabIndex = 13;
            button_Text.Text = "小提示";
            button_Text.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Location = new Point(1470, 637);
            panel1.Name = "panel1";
            panel1.Size = new Size(8, 8);
            panel1.TabIndex = 9;
            // 
            // button__DoCopyCommand
            // 
            button__DoCopyCommand.Anchor = AnchorStyles.Bottom;
            button__DoCopyCommand.Font = new Font("微软雅黑", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button__DoCopyCommand.Location = new Point(1020, 1407);
            button__DoCopyCommand.Name = "button__DoCopyCommand";
            button__DoCopyCommand.Size = new Size(251, 55);
            button__DoCopyCommand.TabIndex = 8;
            button__DoCopyCommand.Text = "复制命令";
            button__DoCopyCommand.UseVisualStyleBackColor = true;
            // 
            // richTextBox
            // 
            richTextBox.Dock = DockStyle.Fill;
            richTextBox.Font = new Font("Microsoft YaHei UI", 12F);
            richTextBox.Location = new Point(0, 0);
            richTextBox.Name = "richTextBox";
            richTextBox.ReadOnly = true;
            richTextBox.Size = new Size(868, 803);
            richTextBox.TabIndex = 0;
            richTextBox.Text = "";
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = "所有文件 (*.*)|*.*";
            // 
            // saveFileDialog
            // 
            saveFileDialog.FileName = "Output.mp3";
            saveFileDialog.Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
            // 
            // toolTip
            // 
            toolTip.AutomaticDelay = 200;
            toolTip.AutoPopDelay = 200000;
            toolTip.InitialDelay = 200;
            toolTip.IsBalloon = true;
            toolTip.ReshowDelay = 40;
            // 
            // FFmpegTool
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1553, 803);
            Controls.Add(splitContainer1);
            Name = "FFmpegTool";
            Text = "FFmpeg 工具";
            Load += FFmpegTool_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            groupBox_ssto.ResumeLayout(false);
            groupBox_ssto.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar_crf).EndInit();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            tabPage5.ResumeLayout(false);
            tabPage5.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Button button_Stop;
        private Button button_Text;
        private Panel panel1;
        private Button button__DoCopyCommand;
        private RichTextBox richTextBox;
        private ComboBox comboBox_Input;
        private Label label1;
        private ComboBox comboBox_Output;
        private Label label2;
        private CheckBox checkBox_vn;
        private CheckBox checkBox_sn;
        private CheckBox checkBox_an;
        private CheckBox checkBox_crf;
        private TrackBar trackBar_crf;
        private ComboBox comboBox_crf;
        private ComboBox comboBox_c_v;
        private CheckBox checkBox_c_v;
        private ComboBox comboBox_b_v;
        private CheckBox checkBox_b_v;
        private ComboBox comboBox_r;
        private CheckBox checkBox_r;
        private ComboBox comboBox_s;
        private CheckBox checkBox_s;
        private ComboBox comboBox_b_a;
        private CheckBox checkBox_b_a;
        private ComboBox comboBox_c_a;
        private CheckBox checkBox_c_a;
        private ComboBox comboBox_ac;
        private CheckBox checkBox_ac;
        private ComboBox comboBox_y_or_n_or_null;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private Button button_Save_File;
        private Button button_Open_File;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private ComboBox comboBox1;
        private CheckBox checkBox1;
        private ToolTip toolTip;
        private GroupBox groupBox_ssto;
        private CheckBox checkBox_ss;
        private ComboBox comboBox_ss;
        private ComboBox comboBox_to;
        private CheckBox checkBox_to;
        private Button button_Main;
        private Button button_CopyCommand;
        private Button button1;
        private RadioButton radioButton_ssto_best;
        private RadioButton radioButton_ssto_precise;
        private RadioButton radioButton_ssto_fast;
        private GroupBox groupBox1;
        private CheckBox checkBox_c_copy;
    }
}