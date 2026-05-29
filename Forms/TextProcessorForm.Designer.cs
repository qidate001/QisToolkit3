namespace QisToolkit3.Forms
{
    partial class TextProcessorForm
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
            richTextBox = new RichTextBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            button1 = new Button();
            tabPage2 = new TabPage();
            button_Path_SetWindows = new Button();
            button_Path_SetSystemSystemDrive = new Button();
            button_Path_SetParentDirectory = new Button();
            button_Path_OpenDir = new Button();
            button_Path_OpenFile = new Button();
            textBox_Path_Directory = new TextBox();
            textBox_Path_FileName = new TextBox();
            textBox_Path_FullPath = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            folderBrowserDialog = new FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(4);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(richTextBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tabControl1);
            splitContainer1.Size = new Size(1067, 741);
            splitContainer1.SplitterDistance = 438;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 0;
            // 
            // richTextBox
            // 
            richTextBox.Dock = DockStyle.Fill;
            richTextBox.Location = new Point(0, 0);
            richTextBox.Margin = new Padding(4);
            richTextBox.Name = "richTextBox";
            richTextBox.Size = new Size(1067, 438);
            richTextBox.TabIndex = 0;
            richTextBox.Text = "";
            richTextBox.TextChanged += richTextBox_TextChanged;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1067, 298);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(button1);
            tabPage1.Location = new Point(4, 36);
            tabPage1.Margin = new Padding(4);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(4);
            tabPage1.Size = new Size(1059, 258);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "路径操作";
            // 
            // button1
            // 
            button1.Enabled = false;
            button1.Font = new Font("Microsoft YaHei UI", 24F);
            button1.Location = new Point(911, 159);
            button1.Name = "button1";
            button1.Size = new Size(140, 92);
            button1.TabIndex = 0;
            button1.Text = "返回";
            button1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(button_Path_SetWindows);
            tabPage2.Controls.Add(button_Path_SetSystemSystemDrive);
            tabPage2.Controls.Add(button_Path_SetParentDirectory);
            tabPage2.Controls.Add(button_Path_OpenDir);
            tabPage2.Controls.Add(button_Path_OpenFile);
            tabPage2.Controls.Add(textBox_Path_Directory);
            tabPage2.Controls.Add(textBox_Path_FileName);
            tabPage2.Controls.Add(textBox_Path_FullPath);
            tabPage2.Controls.Add(label3);
            tabPage2.Controls.Add(label2);
            tabPage2.Controls.Add(label1);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Margin = new Padding(4);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(4);
            tabPage2.Size = new Size(1059, 265);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "路径操作";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // button_Path_SetWindows
            // 
            button_Path_SetWindows.Location = new Point(524, 44);
            button_Path_SetWindows.Name = "button_Path_SetWindows";
            button_Path_SetWindows.Size = new Size(142, 34);
            button_Path_SetWindows.TabIndex = 51;
            button_Path_SetWindows.Text = "Windows";
            button_Path_SetWindows.UseVisualStyleBackColor = true;
            button_Path_SetWindows.Click += button_Path_SetWindows_Click;
            // 
            // button_Path_SetSystemSystemDrive
            // 
            button_Path_SetSystemSystemDrive.Location = new Point(524, 3);
            button_Path_SetSystemSystemDrive.Name = "button_Path_SetSystemSystemDrive";
            button_Path_SetSystemSystemDrive.Size = new Size(142, 34);
            button_Path_SetSystemSystemDrive.TabIndex = 50;
            button_Path_SetSystemSystemDrive.Text = "系统根目录";
            button_Path_SetSystemSystemDrive.UseVisualStyleBackColor = true;
            button_Path_SetSystemSystemDrive.Click += button_Path_SetSystemSystemDrive_Click;
            // 
            // button_Path_SetParentDirectory
            // 
            button_Path_SetParentDirectory.Location = new Point(441, 4);
            button_Path_SetParentDirectory.Name = "button_Path_SetParentDirectory";
            button_Path_SetParentDirectory.Size = new Size(77, 34);
            button_Path_SetParentDirectory.TabIndex = 49;
            button_Path_SetParentDirectory.Text = "←";
            button_Path_SetParentDirectory.UseVisualStyleBackColor = true;
            button_Path_SetParentDirectory.Click += button_Path_SetParentDirectory_Click;
            // 
            // button_Path_OpenDir
            // 
            button_Path_OpenDir.Location = new Point(441, 44);
            button_Path_OpenDir.Name = "button_Path_OpenDir";
            button_Path_OpenDir.Size = new Size(77, 34);
            button_Path_OpenDir.TabIndex = 48;
            button_Path_OpenDir.Text = "打开";
            button_Path_OpenDir.UseVisualStyleBackColor = true;
            button_Path_OpenDir.Click += button_Path_OpenDir_Click;
            // 
            // button_Path_OpenFile
            // 
            button_Path_OpenFile.Location = new Point(441, 83);
            button_Path_OpenFile.Name = "button_Path_OpenFile";
            button_Path_OpenFile.Size = new Size(77, 34);
            button_Path_OpenFile.TabIndex = 47;
            button_Path_OpenFile.Text = "打开";
            button_Path_OpenFile.UseVisualStyleBackColor = true;
            button_Path_OpenFile.Click += button_Path_Open_Click;
            // 
            // textBox_Path_Directory
            // 
            textBox_Path_Directory.Location = new Point(99, 44);
            textBox_Path_Directory.Name = "textBox_Path_Directory";
            textBox_Path_Directory.Size = new Size(336, 33);
            textBox_Path_Directory.TabIndex = 7;
            textBox_Path_Directory.TextChanged += textBox_Path_Directory_TextChanged;
            // 
            // textBox_Path_FileName
            // 
            textBox_Path_FileName.Location = new Point(99, 84);
            textBox_Path_FileName.Name = "textBox_Path_FileName";
            textBox_Path_FileName.Size = new Size(336, 33);
            textBox_Path_FileName.TabIndex = 6;
            textBox_Path_FileName.TextChanged += textBox_Path_FileName_TextChanged;
            // 
            // textBox_Path_FullPath
            // 
            textBox_Path_FullPath.Location = new Point(99, 4);
            textBox_Path_FullPath.Name = "textBox_Path_FullPath";
            textBox_Path_FullPath.Size = new Size(336, 33);
            textBox_Path_FullPath.TabIndex = 5;
            textBox_Path_FullPath.TextChanged += textBox_Path_FullPath_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(3, 87);
            label3.Name = "label3";
            label3.Size = new Size(112, 27);
            label3.TabIndex = 4;
            label3.Text = "文件名称：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 47);
            label2.Name = "label2";
            label2.Size = new Size(112, 27);
            label2.TabIndex = 2;
            label2.Text = "目录路径：";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 7);
            label1.Name = "label1";
            label1.Size = new Size(112, 27);
            label1.TabIndex = 0;
            label1.Text = "完整路径：";
            // 
            // openFileDialog
            // 
            openFileDialog.CheckFileExists = false;
            openFileDialog.Filter = "所有文件 (*.*)|*.*";
            // 
            // saveFileDialog
            // 
            saveFileDialog.FileName = "Output.mp3";
            saveFileDialog.Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
            // 
            // TextProcessorForm
            // 
            AutoScaleDimensions = new SizeF(12F, 27F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1067, 741);
            Controls.Add(splitContainer1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Margin = new Padding(4);
            Name = "TextProcessorForm";
            Text = "TextProcessor";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private RichTextBox richTextBox;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label label1;
        private Label label3;
        private Label label2;
        private TextBox textBox_Path_Directory;
        private TextBox textBox_Path_FileName;
        private TextBox textBox_Path_FullPath;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private Button button_Path_OpenFile;
        private Button button_Path_OpenDir;
        private FolderBrowserDialog folderBrowserDialog;
        private Button button_Path_SetParentDirectory;
        private Button button_Path_SetWindows;
        private Button button_Path_SetSystemSystemDrive;
        private Button button1;
    }
}