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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FFmpegTool));
            splitContainer1 = new SplitContainer();
            button_OpenDownloadPath = new Button();
            comboBox_Output_FileType = new ComboBox();
            comboBox_Output_FilePath = new ComboBox();
            comboBox_Output_FileName = new ComboBox();
            button_Save_File = new Button();
            button_Open_File = new Button();
            button1 = new Button();
            button_Main = new Button();
            button_CopyCommand = new Button();
            tabControl1 = new TabControl();
            tabPage2 = new TabPage();
            dataGridView_InputFiles = new DataGridView();
            ColumnFileName = new DataGridViewTextBoxColumn();
            ColumnFileType = new DataGridViewTextBoxColumn();
            ColumnDeleteFile = new DataGridViewButtonColumn();
            ColumnDirectory = new DataGridViewTextBoxColumn();
            ColumnFullPath = new DataGridViewTextBoxColumn();
            ColumnFileNameWithExt = new DataGridViewTextBoxColumn();
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
            groupBox3 = new GroupBox();
            comboBox_preset = new ComboBox();
            trackBar_preset = new TrackBar();
            checkBox_preset = new CheckBox();
            checkBox_crf = new CheckBox();
            comboBox_r = new ComboBox();
            trackBar_crf = new TrackBar();
            comboBox_c_v = new ComboBox();
            comboBox_crf = new ComboBox();
            comboBox_b_v = new ComboBox();
            checkBox_c_v = new CheckBox();
            comboBox_s = new ComboBox();
            checkBox_s = new CheckBox();
            checkBox_r = new CheckBox();
            checkBox_b_v = new CheckBox();
            tabPage4 = new TabPage();
            groupBox4 = new GroupBox();
            comboBox_af_volume = new ComboBox();
            checkBox_af_volume = new CheckBox();
            comboBox_ar = new ComboBox();
            checkBox_ar = new CheckBox();
            comboBox_q_a = new ComboBox();
            checkBox_q_a = new CheckBox();
            trackBar_q_a = new TrackBar();
            comboBox_c_a = new ComboBox();
            comboBox_b_a = new ComboBox();
            comboBox_ac = new ComboBox();
            checkBox_c_a = new CheckBox();
            checkBox_ac = new CheckBox();
            checkBox_b_a = new CheckBox();
            tabPage5 = new TabPage();
            comboBox_disposition_additionalImages = new ComboBox();
            checkBox_disposition_additionalImages = new CheckBox();
            groupBox2 = new GroupBox();
            checkBox_cd_copy = new CheckBox();
            checkBox_ca_copy = new CheckBox();
            checkBox_cs_copy = new CheckBox();
            checkBox_cv_copy = new CheckBox();
            checkBox_c_copy = new CheckBox();
            groupBox1 = new GroupBox();
            checkBox_vn = new CheckBox();
            checkBox_sn = new CheckBox();
            checkBox_an = new CheckBox();
            comboBox_y_or_n_or_null = new ComboBox();
            button_Stop = new Button();
            button_Text = new Button();
            panel1 = new Panel();
            button__DoCopyCommand = new Button();
            richTextBox = new RichTextBox();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            toolTip = new ToolTip(components);
            groupBox5 = new GroupBox();
            comboBox2 = new ComboBox();
            trackBar1 = new TrackBar();
            checkBox2 = new CheckBox();
            checkBox3 = new CheckBox();
            comboBox3 = new ComboBox();
            trackBar2 = new TrackBar();
            comboBox4 = new ComboBox();
            comboBox5 = new ComboBox();
            comboBox6 = new ComboBox();
            checkBox4 = new CheckBox();
            comboBox7 = new ComboBox();
            checkBox5 = new CheckBox();
            checkBox6 = new CheckBox();
            checkBox7 = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_InputFiles).BeginInit();
            tabPage1.SuspendLayout();
            groupBox_ssto.SuspendLayout();
            tabPage3.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar_preset).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_crf).BeginInit();
            tabPage4.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar_q_a).BeginInit();
            tabPage5.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar2).BeginInit();
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
            splitContainer1.Panel1.Controls.Add(button_OpenDownloadPath);
            splitContainer1.Panel1.Controls.Add(comboBox_Output_FileType);
            splitContainer1.Panel1.Controls.Add(comboBox_Output_FilePath);
            splitContainer1.Panel1.Controls.Add(comboBox_Output_FileName);
            splitContainer1.Panel1.Controls.Add(button_Save_File);
            splitContainer1.Panel1.Controls.Add(button_Open_File);
            splitContainer1.Panel1.Controls.Add(button1);
            splitContainer1.Panel1.Controls.Add(button_Main);
            splitContainer1.Panel1.Controls.Add(button_CopyCommand);
            splitContainer1.Panel1.Controls.Add(tabControl1);
            splitContainer1.Panel1.Controls.Add(comboBox_y_or_n_or_null);
            splitContainer1.Panel1.Controls.Add(button_Stop);
            splitContainer1.Panel1.Controls.Add(button_Text);
            splitContainer1.Panel1.Controls.Add(panel1);
            splitContainer1.Panel1.Controls.Add(button__DoCopyCommand);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(richTextBox);
            splitContainer1.Size = new Size(1553, 970);
            splitContainer1.SplitterDistance = 681;
            splitContainer1.TabIndex = 1;
            // 
            // button_OpenDownloadPath
            // 
            button_OpenDownloadPath.Font = new Font("微软雅黑", 16.2F);
            button_OpenDownloadPath.ImeMode = ImeMode.NoControl;
            button_OpenDownloadPath.Location = new Point(12, 682);
            button_OpenDownloadPath.Name = "button_OpenDownloadPath";
            button_OpenDownloadPath.Size = new Size(329, 55);
            button_OpenDownloadPath.TabIndex = 59;
            button_OpenDownloadPath.Text = "打开当前保存路径";
            button_OpenDownloadPath.UseVisualStyleBackColor = true;
            button_OpenDownloadPath.Click += button_OpenDownloadPath_Click;
            // 
            // comboBox_Output_FileType
            // 
            comboBox_Output_FileType.Font = new Font("微软雅黑", 16.2F);
            comboBox_Output_FileType.FormattingEnabled = true;
            comboBox_Output_FileType.Items.AddRange(new object[] { ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm", ".m4v", ".mpg", ".3gp", ".mp3", ".wav", ".flac", ".aac", ".ogg", ".m4a", ".wma", ".ape", ".jpg", ".png", ".gif", ".bmp", ".webp" });
            comboBox_Output_FileType.Location = new Point(528, 743);
            comboBox_Output_FileType.Name = "comboBox_Output_FileType";
            comboBox_Output_FileType.Size = new Size(150, 43);
            comboBox_Output_FileType.TabIndex = 58;
            comboBox_Output_FileType.Text = ".mp4";
            comboBox_Output_FileType.TextChanged += comboBox_Output_FileType_TextChanged;
            // 
            // comboBox_Output_FilePath
            // 
            comboBox_Output_FilePath.Font = new Font("微软雅黑", 16.2F);
            comboBox_Output_FilePath.FormattingEnabled = true;
            comboBox_Output_FilePath.Location = new Point(12, 796);
            comboBox_Output_FilePath.Name = "comboBox_Output_FilePath";
            comboBox_Output_FilePath.Size = new Size(666, 43);
            comboBox_Output_FilePath.TabIndex = 57;
            comboBox_Output_FilePath.TextChanged += comboBox_Output_FilePath_TextChanged;
            // 
            // comboBox_Output_FileName
            // 
            comboBox_Output_FileName.Font = new Font("微软雅黑", 16.2F);
            comboBox_Output_FileName.FormattingEnabled = true;
            comboBox_Output_FileName.Items.AddRange(new object[] { "output" });
            comboBox_Output_FileName.Location = new Point(12, 743);
            comboBox_Output_FileName.Name = "comboBox_Output_FileName";
            comboBox_Output_FileName.Size = new Size(510, 43);
            comboBox_Output_FileName.TabIndex = 56;
            comboBox_Output_FileName.Text = "output";
            comboBox_Output_FileName.TextChanged += comboBox_Output_FileName_TextChanged;
            // 
            // button_Save_File
            // 
            button_Save_File.Font = new Font("Microsoft YaHei UI", 12F);
            button_Save_File.Location = new Point(391, 852);
            button_Save_File.Name = "button_Save_File";
            button_Save_File.Size = new Size(131, 43);
            button_Save_File.TabIndex = 55;
            button_Save_File.Text = "导出";
            button_Save_File.UseVisualStyleBackColor = true;
            button_Save_File.Click += button_Save_File_Click;
            // 
            // button_Open_File
            // 
            button_Open_File.Font = new Font("微软雅黑", 16.2F);
            button_Open_File.Location = new Point(347, 682);
            button_Open_File.Name = "button_Open_File";
            button_Open_File.Size = new Size(321, 55);
            button_Open_File.TabIndex = 54;
            button_Open_File.Text = "添加文件";
            button_Open_File.UseVisualStyleBackColor = true;
            button_Open_File.Click += button_Open_File_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.Red;
            button1.Location = new Point(528, 852);
            button1.Name = "button1";
            button1.Size = new Size(150, 43);
            button1.TabIndex = 53;
            button1.Text = "停止";
            button1.UseVisualStyleBackColor = false;
            // 
            // button_Main
            // 
            button_Main.Font = new Font("微软雅黑", 16.2F);
            button_Main.Location = new Point(14, 901);
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
            button_CopyCommand.Location = new Point(528, 901);
            button_CopyCommand.Name = "button_CopyCommand";
            button_CopyCommand.Size = new Size(150, 55);
            button_CopyCommand.TabIndex = 50;
            button_CopyCommand.Text = "复制命令";
            button_CopyCommand.UseVisualStyleBackColor = true;
            button_CopyCommand.Click += button_CopyCommand_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Font = new Font("Microsoft YaHei UI", 12F);
            tabControl1.Location = new Point(8, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(670, 664);
            tabControl1.TabIndex = 49;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(dataGridView_InputFiles);
            tabPage2.Location = new Point(4, 36);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(662, 624);
            tabPage2.TabIndex = 6;
            tabPage2.Text = "输入流";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView_InputFiles
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Microsoft YaHei UI", 12F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView_InputFiles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView_InputFiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_InputFiles.Columns.AddRange(new DataGridViewColumn[] { ColumnFileName, ColumnFileType, ColumnDeleteFile, ColumnDirectory, ColumnFullPath, ColumnFileNameWithExt });
            dataGridView_InputFiles.Dock = DockStyle.Fill;
            dataGridView_InputFiles.Location = new Point(3, 3);
            dataGridView_InputFiles.Name = "dataGridView_InputFiles";
            dataGridView_InputFiles.RowHeadersWidth = 51;
            dataGridView_InputFiles.Size = new Size(656, 618);
            dataGridView_InputFiles.TabIndex = 0;
            dataGridView_InputFiles.CellContentClick += dataGridView_InputFiles_CellContentClick;
            // 
            // ColumnFileName
            // 
            ColumnFileName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ColumnFileName.DataPropertyName = "FileName";
            ColumnFileName.HeaderText = "文件名";
            ColumnFileName.MinimumWidth = 6;
            ColumnFileName.Name = "ColumnFileName";
            // 
            // ColumnFileType
            // 
            ColumnFileType.DataPropertyName = "FileType";
            ColumnFileType.HeaderText = "文件类型";
            ColumnFileType.MinimumWidth = 6;
            ColumnFileType.Name = "ColumnFileType";
            ColumnFileType.Resizable = DataGridViewTriState.True;
            ColumnFileType.Width = 125;
            // 
            // ColumnDeleteFile
            // 
            ColumnDeleteFile.HeaderText = "操作按钮";
            ColumnDeleteFile.MinimumWidth = 6;
            ColumnDeleteFile.Name = "ColumnDeleteFile";
            ColumnDeleteFile.Text = "删除";
            ColumnDeleteFile.UseColumnTextForButtonValue = true;
            ColumnDeleteFile.Width = 125;
            // 
            // ColumnDirectory
            // 
            ColumnDirectory.DataPropertyName = "Directory";
            ColumnDirectory.HeaderText = "所在目录";
            ColumnDirectory.MinimumWidth = 6;
            ColumnDirectory.Name = "ColumnDirectory";
            ColumnDirectory.Visible = false;
            ColumnDirectory.Width = 125;
            // 
            // ColumnFullPath
            // 
            ColumnFullPath.DataPropertyName = "FullPath";
            ColumnFullPath.HeaderText = "完整路径";
            ColumnFullPath.MinimumWidth = 6;
            ColumnFullPath.Name = "ColumnFullPath";
            ColumnFullPath.Visible = false;
            ColumnFullPath.Width = 125;
            // 
            // ColumnFileNameWithExt
            // 
            ColumnFileNameWithExt.DataPropertyName = "FileNameWithExt";
            ColumnFileNameWithExt.HeaderText = "带后缀文件名";
            ColumnFileNameWithExt.MinimumWidth = 6;
            ColumnFileNameWithExt.Name = "ColumnFileNameWithExt";
            ColumnFileNameWithExt.Visible = false;
            ColumnFileNameWithExt.Width = 125;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(groupBox_ssto);
            tabPage1.Controls.Add(comboBox1);
            tabPage1.Controls.Add(checkBox1);
            tabPage1.Location = new Point(4, 36);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(662, 624);
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
            groupBox_ssto.Size = new Size(476, 176);
            groupBox_ssto.TabIndex = 52;
            groupBox_ssto.TabStop = false;
            groupBox_ssto.Text = "截取片段";
            // 
            // radioButton_ssto_best
            // 
            radioButton_ssto_best.AutoSize = true;
            radioButton_ssto_best.Checked = true;
            radioButton_ssto_best.Font = new Font("Microsoft YaHei UI", 16.2F);
            radioButton_ssto_best.Location = new Point(312, 124);
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
            radioButton_ssto_precise.Location = new Point(160, 124);
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
            comboBox1.Size = new Size(274, 43);
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
            tabPage3.Controls.Add(groupBox5);
            tabPage3.Controls.Add(groupBox3);
            tabPage3.Location = new Point(4, 36);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(662, 624);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "视频选项";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(comboBox_preset);
            groupBox3.Controls.Add(trackBar_preset);
            groupBox3.Controls.Add(checkBox_preset);
            groupBox3.Controls.Add(checkBox_crf);
            groupBox3.Controls.Add(comboBox_r);
            groupBox3.Controls.Add(trackBar_crf);
            groupBox3.Controls.Add(comboBox_c_v);
            groupBox3.Controls.Add(comboBox_crf);
            groupBox3.Controls.Add(comboBox_b_v);
            groupBox3.Controls.Add(checkBox_c_v);
            groupBox3.Controls.Add(comboBox_s);
            groupBox3.Controls.Add(checkBox_s);
            groupBox3.Controls.Add(checkBox_r);
            groupBox3.Controls.Add(checkBox_b_v);
            groupBox3.Font = new Font("Microsoft YaHei UI", 12F);
            groupBox3.Location = new Point(6, 6);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(644, 287);
            groupBox3.TabIndex = 57;
            groupBox3.TabStop = false;
            groupBox3.Text = "视频重编码";
            // 
            // comboBox_preset
            // 
            comboBox_preset.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_preset.Enabled = false;
            comboBox_preset.Font = new Font("微软雅黑", 16.2F);
            comboBox_preset.FormattingEnabled = true;
            comboBox_preset.Items.AddRange(new object[] { "急速", "快速", "中速", "慢速", "极慢" });
            comboBox_preset.Location = new Point(529, 221);
            comboBox_preset.Name = "comboBox_preset";
            comboBox_preset.Size = new Size(105, 43);
            comboBox_preset.TabIndex = 40;
            comboBox_preset.SelectedIndexChanged += comboBox_preset_SelectedIndexChanged;
            // 
            // trackBar_preset
            // 
            trackBar_preset.Enabled = false;
            trackBar_preset.Location = new Point(216, 219);
            trackBar_preset.Maximum = 4;
            trackBar_preset.Name = "trackBar_preset";
            trackBar_preset.Size = new Size(307, 56);
            trackBar_preset.TabIndex = 39;
            trackBar_preset.Value = 1;
            trackBar_preset.Scroll += trackBar_preset_Scroll;
            // 
            // checkBox_preset
            // 
            checkBox_preset.AutoSize = true;
            checkBox_preset.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_preset.Location = new Point(16, 219);
            checkBox_preset.Name = "checkBox_preset";
            checkBox_preset.Size = new Size(205, 40);
            checkBox_preset.TabIndex = 38;
            checkBox_preset.Text = "编码速度预设";
            checkBox_preset.UseVisualStyleBackColor = true;
            checkBox_preset.CheckedChanged += checkBox_preset_CheckedChanged;
            // 
            // checkBox_crf
            // 
            checkBox_crf.AutoSize = true;
            checkBox_crf.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_crf.Location = new Point(16, 173);
            checkBox_crf.Name = "checkBox_crf";
            checkBox_crf.Size = new Size(205, 40);
            checkBox_crf.TabIndex = 27;
            checkBox_crf.Text = "恒定速率因子";
            checkBox_crf.UseVisualStyleBackColor = true;
            checkBox_crf.CheckedChanged += checkBox_crf_CheckedChanged;
            // 
            // comboBox_r
            // 
            comboBox_r.Enabled = false;
            comboBox_r.Font = new Font("微软雅黑", 16.2F);
            comboBox_r.FormattingEnabled = true;
            comboBox_r.Items.AddRange(new object[] { "180", "120", "60", "30", "15" });
            comboBox_r.Location = new Point(425, 29);
            comboBox_r.Name = "comboBox_r";
            comboBox_r.Size = new Size(209, 43);
            comboBox_r.TabIndex = 35;
            comboBox_r.Text = "60";
            // 
            // trackBar_crf
            // 
            trackBar_crf.Enabled = false;
            trackBar_crf.Location = new Point(216, 173);
            trackBar_crf.Maximum = 51;
            trackBar_crf.Name = "trackBar_crf";
            trackBar_crf.Size = new Size(307, 56);
            trackBar_crf.TabIndex = 29;
            trackBar_crf.Value = 23;
            trackBar_crf.Scroll += trackBar_crf_Scroll;
            // 
            // comboBox_c_v
            // 
            comboBox_c_v.Enabled = false;
            comboBox_c_v.Font = new Font("微软雅黑", 16.2F);
            comboBox_c_v.FormattingEnabled = true;
            comboBox_c_v.Items.AddRange(new object[] { "H.264", "H.265", "VP9", "AV1" });
            comboBox_c_v.Location = new Point(187, 31);
            comboBox_c_v.Name = "comboBox_c_v";
            comboBox_c_v.Size = new Size(143, 43);
            comboBox_c_v.TabIndex = 31;
            comboBox_c_v.Text = "H.265";
            // 
            // comboBox_crf
            // 
            comboBox_crf.Enabled = false;
            comboBox_crf.Font = new Font("微软雅黑", 16.2F);
            comboBox_crf.FormattingEnabled = true;
            comboBox_crf.Items.AddRange(new object[] { "极佳", "优秀", "良好", "一般", "较差" });
            comboBox_crf.Location = new Point(529, 172);
            comboBox_crf.Name = "comboBox_crf";
            comboBox_crf.Size = new Size(105, 43);
            comboBox_crf.TabIndex = 28;
            comboBox_crf.Text = "23";
            comboBox_crf.TextChanged += comboBox_crf_TextChanged;
            // 
            // comboBox_b_v
            // 
            comboBox_b_v.Enabled = false;
            comboBox_b_v.Font = new Font("微软雅黑", 16.2F);
            comboBox_b_v.FormattingEnabled = true;
            comboBox_b_v.Items.AddRange(new object[] { "1M", "500K" });
            comboBox_b_v.Location = new Point(187, 79);
            comboBox_b_v.Name = "comboBox_b_v";
            comboBox_b_v.Size = new Size(143, 43);
            comboBox_b_v.TabIndex = 33;
            comboBox_b_v.Text = "1M";
            // 
            // checkBox_c_v
            // 
            checkBox_c_v.AutoSize = true;
            checkBox_c_v.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_c_v.Location = new Point(15, 32);
            checkBox_c_v.Name = "checkBox_c_v";
            checkBox_c_v.Size = new Size(177, 40);
            checkBox_c_v.TabIndex = 30;
            checkBox_c_v.Text = "视频编码器";
            checkBox_c_v.UseVisualStyleBackColor = true;
            checkBox_c_v.CheckedChanged += checkBox_c_v_CheckedChanged;
            // 
            // comboBox_s
            // 
            comboBox_s.Enabled = false;
            comboBox_s.Font = new Font("微软雅黑", 16.2F);
            comboBox_s.FormattingEnabled = true;
            comboBox_s.Items.AddRange(new object[] { "1280x720" });
            comboBox_s.Location = new Point(131, 125);
            comboBox_s.Name = "comboBox_s";
            comboBox_s.Size = new Size(199, 43);
            comboBox_s.TabIndex = 37;
            comboBox_s.Text = "1280x720";
            // 
            // checkBox_s
            // 
            checkBox_s.AutoSize = true;
            checkBox_s.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_s.Location = new Point(15, 126);
            checkBox_s.Name = "checkBox_s";
            checkBox_s.Size = new Size(121, 40);
            checkBox_s.TabIndex = 36;
            checkBox_s.Text = "分辨率";
            checkBox_s.UseVisualStyleBackColor = true;
            checkBox_s.CheckedChanged += checkBox_s_CheckedChanged;
            // 
            // checkBox_r
            // 
            checkBox_r.AutoSize = true;
            checkBox_r.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_r.Location = new Point(336, 31);
            checkBox_r.Name = "checkBox_r";
            checkBox_r.Size = new Size(93, 40);
            checkBox_r.TabIndex = 34;
            checkBox_r.Text = "帧率";
            checkBox_r.UseVisualStyleBackColor = true;
            checkBox_r.CheckedChanged += checkBox_r_CheckedChanged;
            // 
            // checkBox_b_v
            // 
            checkBox_b_v.AutoSize = true;
            checkBox_b_v.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_b_v.Location = new Point(15, 80);
            checkBox_b_v.Name = "checkBox_b_v";
            checkBox_b_v.Size = new Size(177, 40);
            checkBox_b_v.TabIndex = 32;
            checkBox_b_v.Text = "视频比特率";
            checkBox_b_v.UseVisualStyleBackColor = true;
            checkBox_b_v.CheckedChanged += checkBox_b_v_CheckedChanged;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(groupBox4);
            tabPage4.Location = new Point(4, 36);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(662, 624);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "音频选项";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(comboBox_af_volume);
            groupBox4.Controls.Add(checkBox_af_volume);
            groupBox4.Controls.Add(comboBox_ar);
            groupBox4.Controls.Add(checkBox_ar);
            groupBox4.Controls.Add(comboBox_q_a);
            groupBox4.Controls.Add(checkBox_q_a);
            groupBox4.Controls.Add(trackBar_q_a);
            groupBox4.Controls.Add(comboBox_c_a);
            groupBox4.Controls.Add(comboBox_b_a);
            groupBox4.Controls.Add(comboBox_ac);
            groupBox4.Controls.Add(checkBox_c_a);
            groupBox4.Controls.Add(checkBox_ac);
            groupBox4.Controls.Add(checkBox_b_a);
            groupBox4.Font = new Font("Microsoft YaHei UI", 12F);
            groupBox4.Location = new Point(6, 6);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(644, 234);
            groupBox4.TabIndex = 58;
            groupBox4.TabStop = false;
            groupBox4.Text = "音频重编码";
            // 
            // comboBox_af_volume
            // 
            comboBox_af_volume.Enabled = false;
            comboBox_af_volume.Font = new Font("微软雅黑", 16.2F);
            comboBox_af_volume.FormattingEnabled = true;
            comboBox_af_volume.Items.AddRange(new object[] { "1", "2" });
            comboBox_af_volume.Location = new Point(490, 78);
            comboBox_af_volume.Name = "comboBox_af_volume";
            comboBox_af_volume.Size = new Size(148, 43);
            comboBox_af_volume.TabIndex = 51;
            comboBox_af_volume.Text = "2";
            // 
            // checkBox_af_volume
            // 
            checkBox_af_volume.AutoSize = true;
            checkBox_af_volume.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_af_volume.Location = new Point(369, 80);
            checkBox_af_volume.Name = "checkBox_af_volume";
            checkBox_af_volume.Size = new Size(121, 40);
            checkBox_af_volume.TabIndex = 50;
            checkBox_af_volume.Text = "增音量";
            checkBox_af_volume.UseVisualStyleBackColor = true;
            checkBox_af_volume.CheckedChanged += checkBox_af_volume_CheckedChanged;
            // 
            // comboBox_ar
            // 
            comboBox_ar.Enabled = false;
            comboBox_ar.Font = new Font("微软雅黑", 16.2F);
            comboBox_ar.FormattingEnabled = true;
            comboBox_ar.Items.AddRange(new object[] { "8000", "16000", "22050", "44100", "48000", "96000" });
            comboBox_ar.Location = new Point(178, 125);
            comboBox_ar.Name = "comboBox_ar";
            comboBox_ar.Size = new Size(185, 43);
            comboBox_ar.TabIndex = 49;
            comboBox_ar.Text = "44100";
            // 
            // checkBox_ar
            // 
            checkBox_ar.AutoSize = true;
            checkBox_ar.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_ar.Location = new Point(6, 126);
            checkBox_ar.Name = "checkBox_ar";
            checkBox_ar.Size = new Size(177, 40);
            checkBox_ar.TabIndex = 48;
            checkBox_ar.Text = "音频采样率";
            checkBox_ar.UseVisualStyleBackColor = true;
            checkBox_ar.CheckedChanged += checkBox_ar_CheckedChanged;
            // 
            // comboBox_q_a
            // 
            comboBox_q_a.Enabled = false;
            comboBox_q_a.Font = new Font("微软雅黑", 16.2F);
            comboBox_q_a.FormattingEnabled = true;
            comboBox_q_a.Items.AddRange(new object[] { "最佳", "极佳", "高等", "中等", "还行", "一般", "较差", "很差", "极差", "最差" });
            comboBox_q_a.Location = new Point(533, 173);
            comboBox_q_a.Name = "comboBox_q_a";
            comboBox_q_a.Size = new Size(105, 43);
            comboBox_q_a.TabIndex = 47;
            comboBox_q_a.Text = "3";
            comboBox_q_a.TextChanged += comboBox_q_a_TextChanged;
            // 
            // checkBox_q_a
            // 
            checkBox_q_a.AutoSize = true;
            checkBox_q_a.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_q_a.Location = new Point(6, 174);
            checkBox_q_a.Name = "checkBox_q_a";
            checkBox_q_a.Size = new Size(209, 40);
            checkBox_q_a.TabIndex = 44;
            checkBox_q_a.Text = "MP3音频质量";
            checkBox_q_a.UseVisualStyleBackColor = true;
            checkBox_q_a.CheckedChanged += checkBox_q_a_CheckedChanged;
            // 
            // trackBar_q_a
            // 
            trackBar_q_a.Enabled = false;
            trackBar_q_a.Location = new Point(206, 174);
            trackBar_q_a.Maximum = 8;
            trackBar_q_a.Name = "trackBar_q_a";
            trackBar_q_a.Size = new Size(321, 56);
            trackBar_q_a.TabIndex = 46;
            trackBar_q_a.Value = 2;
            trackBar_q_a.Scroll += trackBar_q_a_Scroll;
            // 
            // comboBox_c_a
            // 
            comboBox_c_a.Enabled = false;
            comboBox_c_a.Font = new Font("微软雅黑", 16.2F);
            comboBox_c_a.FormattingEnabled = true;
            comboBox_c_a.Items.AddRange(new object[] { "MP3编码器", "AAC编码器", "OGG编码器", "FLAC编码器", "OPUS编码器", "AC3编码器" });
            comboBox_c_a.Location = new Point(178, 31);
            comboBox_c_a.Name = "comboBox_c_a";
            comboBox_c_a.Size = new Size(185, 43);
            comboBox_c_a.TabIndex = 39;
            comboBox_c_a.Text = "MP3编码器";
            // 
            // comboBox_b_a
            // 
            comboBox_b_a.Enabled = false;
            comboBox_b_a.Font = new Font("微软雅黑", 16.2F);
            comboBox_b_a.FormattingEnabled = true;
            comboBox_b_a.Items.AddRange(new object[] { "64k", "96k", "128k", "198k", "256k", "320k" });
            comboBox_b_a.Location = new Point(178, 79);
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
            comboBox_ac.Items.AddRange(new object[] { "1", "2", "6" });
            comboBox_ac.Location = new Point(490, 31);
            comboBox_ac.Name = "comboBox_ac";
            comboBox_ac.Size = new Size(148, 43);
            comboBox_ac.TabIndex = 43;
            comboBox_ac.Text = "2";
            // 
            // checkBox_c_a
            // 
            checkBox_c_a.AutoSize = true;
            checkBox_c_a.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_c_a.Location = new Point(6, 32);
            checkBox_c_a.Name = "checkBox_c_a";
            checkBox_c_a.Size = new Size(177, 40);
            checkBox_c_a.TabIndex = 38;
            checkBox_c_a.Text = "音频编码器";
            checkBox_c_a.UseVisualStyleBackColor = true;
            checkBox_c_a.CheckedChanged += checkBox_c_a_CheckedChanged;
            // 
            // checkBox_ac
            // 
            checkBox_ac.AutoSize = true;
            checkBox_ac.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_ac.Location = new Point(369, 31);
            checkBox_ac.Name = "checkBox_ac";
            checkBox_ac.Size = new Size(121, 40);
            checkBox_ac.TabIndex = 42;
            checkBox_ac.Text = "声道数";
            toolTip.SetToolTip(checkBox_ac, "1：单声道\r\n2：立体声\r\n6：5.1环绕声");
            checkBox_ac.UseVisualStyleBackColor = true;
            checkBox_ac.CheckedChanged += checkBox_ac_CheckedChanged;
            // 
            // checkBox_b_a
            // 
            checkBox_b_a.AutoSize = true;
            checkBox_b_a.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_b_a.Location = new Point(6, 80);
            checkBox_b_a.Name = "checkBox_b_a";
            checkBox_b_a.Size = new Size(177, 40);
            checkBox_b_a.TabIndex = 40;
            checkBox_b_a.Text = "音频比特率";
            checkBox_b_a.UseVisualStyleBackColor = true;
            checkBox_b_a.CheckedChanged += checkBox_b_a_CheckedChanged;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(comboBox_disposition_additionalImages);
            tabPage5.Controls.Add(checkBox_disposition_additionalImages);
            tabPage5.Controls.Add(groupBox2);
            tabPage5.Controls.Add(groupBox1);
            tabPage5.Location = new Point(4, 36);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(662, 624);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "流映射项";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // comboBox_disposition_additionalImages
            // 
            comboBox_disposition_additionalImages.Enabled = false;
            comboBox_disposition_additionalImages.Font = new Font("微软雅黑", 16.2F);
            comboBox_disposition_additionalImages.FormattingEnabled = true;
            comboBox_disposition_additionalImages.Items.AddRange(new object[] { "(无可用文件)" });
            comboBox_disposition_additionalImages.Location = new Point(185, 194);
            comboBox_disposition_additionalImages.Name = "comboBox_disposition_additionalImages";
            comboBox_disposition_additionalImages.Size = new Size(465, 43);
            comboBox_disposition_additionalImages.TabIndex = 57;
            // 
            // checkBox_disposition_additionalImages
            // 
            checkBox_disposition_additionalImages.AutoSize = true;
            checkBox_disposition_additionalImages.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_disposition_additionalImages.Location = new Point(6, 195);
            checkBox_disposition_additionalImages.Name = "checkBox_disposition_additionalImages";
            checkBox_disposition_additionalImages.Size = new Size(205, 40);
            checkBox_disposition_additionalImages.TabIndex = 56;
            checkBox_disposition_additionalImages.Text = "嵌入艺术图：";
            checkBox_disposition_additionalImages.UseVisualStyleBackColor = true;
            checkBox_disposition_additionalImages.CheckedChanged += checkBox_disposition_additionalImages_CheckedChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(checkBox_cd_copy);
            groupBox2.Controls.Add(checkBox_ca_copy);
            groupBox2.Controls.Add(checkBox_cs_copy);
            groupBox2.Controls.Add(checkBox_cv_copy);
            groupBox2.Controls.Add(checkBox_c_copy);
            groupBox2.Font = new Font("Microsoft YaHei UI", 12F);
            groupBox2.Location = new Point(201, 6);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(449, 182);
            groupBox2.TabIndex = 55;
            groupBox2.TabStop = false;
            groupBox2.Text = "流复制模式";
            // 
            // checkBox_cd_copy
            // 
            checkBox_cd_copy.AutoSize = true;
            checkBox_cd_copy.Enabled = false;
            checkBox_cd_copy.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_cd_copy.Location = new Point(189, 124);
            checkBox_cd_copy.Name = "checkBox_cd_copy";
            checkBox_cd_copy.Size = new Size(177, 40);
            checkBox_cd_copy.TabIndex = 58;
            checkBox_cd_copy.Text = "复制数据流";
            checkBox_cd_copy.UseVisualStyleBackColor = true;
            // 
            // checkBox_ca_copy
            // 
            checkBox_ca_copy.AutoSize = true;
            checkBox_ca_copy.Enabled = false;
            checkBox_ca_copy.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_ca_copy.Location = new Point(6, 124);
            checkBox_ca_copy.Name = "checkBox_ca_copy";
            checkBox_ca_copy.Size = new Size(177, 40);
            checkBox_ca_copy.TabIndex = 56;
            checkBox_ca_copy.Text = "复制音频流";
            checkBox_ca_copy.UseVisualStyleBackColor = true;
            // 
            // checkBox_cs_copy
            // 
            checkBox_cs_copy.AutoSize = true;
            checkBox_cs_copy.Enabled = false;
            checkBox_cs_copy.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_cs_copy.Location = new Point(189, 78);
            checkBox_cs_copy.Name = "checkBox_cs_copy";
            checkBox_cs_copy.Size = new Size(177, 40);
            checkBox_cs_copy.TabIndex = 57;
            checkBox_cs_copy.Text = "复制字幕流";
            checkBox_cs_copy.UseVisualStyleBackColor = true;
            // 
            // checkBox_cv_copy
            // 
            checkBox_cv_copy.AutoSize = true;
            checkBox_cv_copy.Enabled = false;
            checkBox_cv_copy.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_cv_copy.Location = new Point(6, 78);
            checkBox_cv_copy.Name = "checkBox_cv_copy";
            checkBox_cv_copy.Size = new Size(177, 40);
            checkBox_cv_copy.TabIndex = 55;
            checkBox_cv_copy.Text = "复制视频流";
            checkBox_cv_copy.UseVisualStyleBackColor = true;
            // 
            // checkBox_c_copy
            // 
            checkBox_c_copy.AutoSize = true;
            checkBox_c_copy.Checked = true;
            checkBox_c_copy.CheckState = CheckState.Checked;
            checkBox_c_copy.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox_c_copy.Location = new Point(6, 32);
            checkBox_c_copy.Name = "checkBox_c_copy";
            checkBox_c_copy.Size = new Size(233, 40);
            checkBox_c_copy.TabIndex = 54;
            checkBox_c_copy.Text = "完整复制所有流";
            checkBox_c_copy.UseVisualStyleBackColor = true;
            checkBox_c_copy.CheckedChanged += checkBox_c_copy_CheckedChanged;
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
            // comboBox_y_or_n_or_null
            // 
            comboBox_y_or_n_or_null.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_y_or_n_or_null.Font = new Font("微软雅黑", 16.2F);
            comboBox_y_or_n_or_null.FormattingEnabled = true;
            comboBox_y_or_n_or_null.Items.AddRange(new object[] { "自动覆盖已存在文件", "不覆盖已存在文件", "默认" });
            comboBox_y_or_n_or_null.Location = new Point(14, 852);
            comboBox_y_or_n_or_null.Name = "comboBox_y_or_n_or_null";
            comboBox_y_or_n_or_null.Size = new Size(371, 43);
            comboBox_y_or_n_or_null.TabIndex = 45;
            // 
            // button_Stop
            // 
            button_Stop.Anchor = AnchorStyles.Bottom;
            button_Stop.BackColor = Color.Red;
            button_Stop.Location = new Point(1077, 1539);
            button_Stop.Name = "button_Stop";
            button_Stop.Size = new Size(94, 29);
            button_Stop.TabIndex = 15;
            button_Stop.Text = "停止";
            button_Stop.UseVisualStyleBackColor = false;
            // 
            // button_Text
            // 
            button_Text.Anchor = AnchorStyles.Bottom;
            button_Text.Location = new Point(1177, 1539);
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
            button__DoCopyCommand.Location = new Point(1020, 1574);
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
            richTextBox.Size = new Size(868, 970);
            richTextBox.TabIndex = 0;
            richTextBox.Text = "";
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = "所有文件 (*.*)|*.*";
            openFileDialog.Multiselect = true;
            // 
            // saveFileDialog
            // 
            saveFileDialog.FileName = "Output.mp3";
            saveFileDialog.Filter = resources.GetString("saveFileDialog.Filter");
            // 
            // toolTip
            // 
            toolTip.AutomaticDelay = 200;
            toolTip.AutoPopDelay = 200000;
            toolTip.InitialDelay = 200;
            toolTip.IsBalloon = true;
            toolTip.ReshowDelay = 40;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(comboBox2);
            groupBox5.Controls.Add(trackBar1);
            groupBox5.Controls.Add(checkBox2);
            groupBox5.Controls.Add(checkBox3);
            groupBox5.Controls.Add(comboBox3);
            groupBox5.Controls.Add(trackBar2);
            groupBox5.Controls.Add(comboBox4);
            groupBox5.Controls.Add(comboBox5);
            groupBox5.Controls.Add(comboBox6);
            groupBox5.Controls.Add(checkBox4);
            groupBox5.Controls.Add(comboBox7);
            groupBox5.Controls.Add(checkBox5);
            groupBox5.Controls.Add(checkBox6);
            groupBox5.Controls.Add(checkBox7);
            groupBox5.Font = new Font("Microsoft YaHei UI", 12F);
            groupBox5.Location = new Point(6, 299);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(644, 287);
            groupBox5.TabIndex = 58;
            groupBox5.TabStop = false;
            groupBox5.Text = "比特流过滤器";
            // 
            // comboBox2
            // 
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.Enabled = false;
            comboBox2.Font = new Font("微软雅黑", 16.2F);
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "急速", "快速", "中速", "慢速", "极慢" });
            comboBox2.Location = new Point(529, 221);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(105, 43);
            comboBox2.TabIndex = 40;
            // 
            // trackBar1
            // 
            trackBar1.Enabled = false;
            trackBar1.Location = new Point(216, 219);
            trackBar1.Maximum = 4;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(307, 56);
            trackBar1.TabIndex = 39;
            trackBar1.Value = 1;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox2.Location = new Point(16, 219);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(205, 40);
            checkBox2.TabIndex = 38;
            checkBox2.Text = "编码速度预设";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox3.Location = new Point(16, 173);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(205, 40);
            checkBox3.TabIndex = 27;
            checkBox3.Text = "恒定速率因子";
            checkBox3.UseVisualStyleBackColor = true;
            // 
            // comboBox3
            // 
            comboBox3.Enabled = false;
            comboBox3.Font = new Font("微软雅黑", 16.2F);
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "180", "120", "60", "30", "15" });
            comboBox3.Location = new Point(425, 29);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(209, 43);
            comboBox3.TabIndex = 35;
            comboBox3.Text = "60";
            // 
            // trackBar2
            // 
            trackBar2.Enabled = false;
            trackBar2.Location = new Point(216, 173);
            trackBar2.Maximum = 51;
            trackBar2.Name = "trackBar2";
            trackBar2.Size = new Size(307, 56);
            trackBar2.TabIndex = 29;
            trackBar2.Value = 23;
            // 
            // comboBox4
            // 
            comboBox4.Enabled = false;
            comboBox4.Font = new Font("微软雅黑", 16.2F);
            comboBox4.FormattingEnabled = true;
            comboBox4.Items.AddRange(new object[] { "H.264", "H.265", "VP9", "AV1" });
            comboBox4.Location = new Point(187, 31);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(143, 43);
            comboBox4.TabIndex = 31;
            comboBox4.Text = "H.265";
            // 
            // comboBox5
            // 
            comboBox5.Enabled = false;
            comboBox5.Font = new Font("微软雅黑", 16.2F);
            comboBox5.FormattingEnabled = true;
            comboBox5.Items.AddRange(new object[] { "极佳", "优秀", "良好", "一般", "较差" });
            comboBox5.Location = new Point(529, 172);
            comboBox5.Name = "comboBox5";
            comboBox5.Size = new Size(105, 43);
            comboBox5.TabIndex = 28;
            comboBox5.Text = "23";
            // 
            // comboBox6
            // 
            comboBox6.Enabled = false;
            comboBox6.Font = new Font("微软雅黑", 16.2F);
            comboBox6.FormattingEnabled = true;
            comboBox6.Items.AddRange(new object[] { "1M", "500K" });
            comboBox6.Location = new Point(187, 79);
            comboBox6.Name = "comboBox6";
            comboBox6.Size = new Size(143, 43);
            comboBox6.TabIndex = 33;
            comboBox6.Text = "1M";
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox4.Location = new Point(15, 32);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(177, 40);
            checkBox4.TabIndex = 30;
            checkBox4.Text = "视频编码器";
            checkBox4.UseVisualStyleBackColor = true;
            // 
            // comboBox7
            // 
            comboBox7.Enabled = false;
            comboBox7.Font = new Font("微软雅黑", 16.2F);
            comboBox7.FormattingEnabled = true;
            comboBox7.Items.AddRange(new object[] { "1280x720" });
            comboBox7.Location = new Point(131, 125);
            comboBox7.Name = "comboBox7";
            comboBox7.Size = new Size(199, 43);
            comboBox7.TabIndex = 37;
            comboBox7.Text = "1280x720";
            // 
            // checkBox5
            // 
            checkBox5.AutoSize = true;
            checkBox5.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox5.Location = new Point(15, 126);
            checkBox5.Name = "checkBox5";
            checkBox5.Size = new Size(121, 40);
            checkBox5.TabIndex = 36;
            checkBox5.Text = "分辨率";
            checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            checkBox6.AutoSize = true;
            checkBox6.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox6.Location = new Point(336, 31);
            checkBox6.Name = "checkBox6";
            checkBox6.Size = new Size(93, 40);
            checkBox6.TabIndex = 34;
            checkBox6.Text = "帧率";
            checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            checkBox7.AutoSize = true;
            checkBox7.Font = new Font("Microsoft YaHei UI", 16.2F);
            checkBox7.Location = new Point(15, 80);
            checkBox7.Name = "checkBox7";
            checkBox7.Size = new Size(177, 40);
            checkBox7.TabIndex = 32;
            checkBox7.Text = "视频比特率";
            checkBox7.UseVisualStyleBackColor = true;
            // 
            // FFmpegTool
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1553, 970);
            Controls.Add(splitContainer1);
            Name = "FFmpegTool";
            Text = "FFmpeg 工具";
            Load += FFmpegTool_Load;
            DragDrop += FFmpegTool_DragDrop;
            DragEnter += FFmpegTool_DragEnter;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_InputFiles).EndInit();
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            groupBox_ssto.ResumeLayout(false);
            groupBox_ssto.PerformLayout();
            tabPage3.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar_preset).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_crf).EndInit();
            tabPage4.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar_q_a).EndInit();
            tabPage5.ResumeLayout(false);
            tabPage5.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Button button_Stop;
        private Button button_Text;
        private Panel panel1;
        private Button button__DoCopyCommand;
        private RichTextBox richTextBox;
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
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
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
        private GroupBox groupBox2;
        private CheckBox checkBox_cd_copy;
        private CheckBox checkBox_ca_copy;
        private CheckBox checkBox_cs_copy;
        private CheckBox checkBox_cv_copy;
        private GroupBox groupBox3;
        private CheckBox checkBox_preset;
        private ComboBox comboBox_preset;
        private TrackBar trackBar_preset;
        private GroupBox groupBox4;
        private CheckBox checkBox_q_a;
        private TrackBar trackBar_q_a;
        private ComboBox comboBox_q_a;
        private ComboBox comboBox_af_volume;
        private CheckBox checkBox_af_volume;
        private ComboBox comboBox_ar;
        private CheckBox checkBox_ar;
        private ComboBox comboBox_disposition_additionalImages;
        private CheckBox checkBox_disposition_additionalImages;
        private TabPage tabPage2;
        private Button button_Save_File;
        private Button button_Open_File;
        private ComboBox comboBox_Output_FileType;
        private ComboBox comboBox_Output_FilePath;
        private ComboBox comboBox_Output_FileName;
        private DataGridView dataGridView_InputFiles;
        private DataGridViewTextBoxColumn ColumnFileName;
        private DataGridViewTextBoxColumn ColumnFileType;
        private DataGridViewButtonColumn ColumnDeleteFile;
        private DataGridViewTextBoxColumn ColumnDirectory;
        private DataGridViewTextBoxColumn ColumnFullPath;
        private DataGridViewTextBoxColumn ColumnFileNameWithExt;
        private Button button_OpenDownloadPath;
        private GroupBox groupBox5;
        private ComboBox comboBox2;
        private TrackBar trackBar1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private ComboBox comboBox3;
        private TrackBar trackBar2;
        private ComboBox comboBox4;
        private ComboBox comboBox5;
        private ComboBox comboBox6;
        private CheckBox checkBox4;
        private ComboBox comboBox7;
        private CheckBox checkBox5;
        private CheckBox checkBox6;
        private CheckBox checkBox7;
    }
}