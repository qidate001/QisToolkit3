namespace QisToolkit3
{
    partial class FilesOperation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilesOperation));
            saveFileDialog = new SaveFileDialog();
            labelFileName = new Label();
            openFileDialog = new OpenFileDialog();
            buttonDeleteFile = new Button();
            buttonClearFile = new Button();
            buttonReadFile = new Button();
            labelCreationTime = new Label();
            textBoxCreationTime = new TextBox();
            textBoxLastWriteTime = new TextBox();
            labelLastWriteTime = new Label();
            textBoxLastAccessTime = new TextBox();
            labelLastAccessTime = new Label();
            buttonCreationTimeSetNow = new Button();
            buttonLastWriteTimeSetNow = new Button();
            buttonLastAccessTimeSetNow = new Button();
            buttonLastAccessTimeSet = new Button();
            buttonLastWriteTimeSet = new Button();
            buttonCreationTimeSet = new Button();
            buttonStartFile = new Button();
            contextMenuStrip = new ContextMenuStrip(components);
            toolStripMenuItem_Automatic = new ToolStripMenuItem();
            ToolStripMenuItem_Manual = new ToolStripMenuItem();
            ToolStripMenuItem_ReSet = new ToolStripMenuItem();
            textBox_ManualPath = new TextBox();
            button_ManualPath = new Button();
            checkBoxHidden = new CheckBox();
            checkBoxArchive = new CheckBox();
            checkBoxReadOnly = new CheckBox();
            checkBoxSystem = new CheckBox();
            checkBoxOffline = new CheckBox();
            checkBoxNotContentIndexed = new CheckBox();
            toolTip = new ToolTip(components);
            buttonCover = new Button();
            buttonAppend = new Button();
            buttonRead = new Button();
            checkBoxBinaryFile = new CheckBox();
            richTextBox = new RichTextBox();
            button_FilesOperationPlus = new Button();
            contextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // saveFileDialog
            // 
            saveFileDialog.FileName = "1.txt";
            resources.ApplyResources(saveFileDialog, "saveFileDialog");
            // 
            // labelFileName
            // 
            resources.ApplyResources(labelFileName, "labelFileName");
            labelFileName.ForeColor = Color.Black;
            labelFileName.Name = "labelFileName";
            toolTip.SetToolTip(labelFileName, resources.GetString("labelFileName.ToolTip"));
            labelFileName.Click += labelFileName_Click;
            labelFileName.MouseClick += labelFileName_MouseClick;
            // 
            // openFileDialog
            // 
            resources.ApplyResources(openFileDialog, "openFileDialog");
            // 
            // buttonDeleteFile
            // 
            resources.ApplyResources(buttonDeleteFile, "buttonDeleteFile");
            buttonDeleteFile.ForeColor = Color.Black;
            buttonDeleteFile.Name = "buttonDeleteFile";
            toolTip.SetToolTip(buttonDeleteFile, resources.GetString("buttonDeleteFile.ToolTip"));
            buttonDeleteFile.UseVisualStyleBackColor = true;
            buttonDeleteFile.Click += buttonDeleteFile_Click;
            // 
            // buttonClearFile
            // 
            resources.ApplyResources(buttonClearFile, "buttonClearFile");
            buttonClearFile.ForeColor = Color.Black;
            buttonClearFile.Name = "buttonClearFile";
            toolTip.SetToolTip(buttonClearFile, resources.GetString("buttonClearFile.ToolTip"));
            buttonClearFile.UseVisualStyleBackColor = true;
            buttonClearFile.Click += buttonClearFile_Click;
            // 
            // buttonReadFile
            // 
            resources.ApplyResources(buttonReadFile, "buttonReadFile");
            buttonReadFile.ForeColor = Color.Black;
            buttonReadFile.Name = "buttonReadFile";
            toolTip.SetToolTip(buttonReadFile, resources.GetString("buttonReadFile.ToolTip"));
            buttonReadFile.UseVisualStyleBackColor = true;
            buttonReadFile.Click += buttonReadFile_Click;
            // 
            // labelCreationTime
            // 
            resources.ApplyResources(labelCreationTime, "labelCreationTime");
            labelCreationTime.ForeColor = Color.Black;
            labelCreationTime.Name = "labelCreationTime";
            toolTip.SetToolTip(labelCreationTime, resources.GetString("labelCreationTime.ToolTip"));
            // 
            // textBoxCreationTime
            // 
            resources.ApplyResources(textBoxCreationTime, "textBoxCreationTime");
            textBoxCreationTime.ForeColor = Color.Black;
            textBoxCreationTime.Name = "textBoxCreationTime";
            toolTip.SetToolTip(textBoxCreationTime, resources.GetString("textBoxCreationTime.ToolTip"));
            // 
            // textBoxLastWriteTime
            // 
            resources.ApplyResources(textBoxLastWriteTime, "textBoxLastWriteTime");
            textBoxLastWriteTime.ForeColor = Color.Black;
            textBoxLastWriteTime.Name = "textBoxLastWriteTime";
            toolTip.SetToolTip(textBoxLastWriteTime, resources.GetString("textBoxLastWriteTime.ToolTip"));
            // 
            // labelLastWriteTime
            // 
            resources.ApplyResources(labelLastWriteTime, "labelLastWriteTime");
            labelLastWriteTime.ForeColor = Color.Black;
            labelLastWriteTime.Name = "labelLastWriteTime";
            toolTip.SetToolTip(labelLastWriteTime, resources.GetString("labelLastWriteTime.ToolTip"));
            // 
            // textBoxLastAccessTime
            // 
            resources.ApplyResources(textBoxLastAccessTime, "textBoxLastAccessTime");
            textBoxLastAccessTime.ForeColor = Color.Black;
            textBoxLastAccessTime.Name = "textBoxLastAccessTime";
            toolTip.SetToolTip(textBoxLastAccessTime, resources.GetString("textBoxLastAccessTime.ToolTip"));
            // 
            // labelLastAccessTime
            // 
            resources.ApplyResources(labelLastAccessTime, "labelLastAccessTime");
            labelLastAccessTime.ForeColor = Color.Black;
            labelLastAccessTime.Name = "labelLastAccessTime";
            toolTip.SetToolTip(labelLastAccessTime, resources.GetString("labelLastAccessTime.ToolTip"));
            // 
            // buttonCreationTimeSetNow
            // 
            resources.ApplyResources(buttonCreationTimeSetNow, "buttonCreationTimeSetNow");
            buttonCreationTimeSetNow.ForeColor = Color.Black;
            buttonCreationTimeSetNow.Name = "buttonCreationTimeSetNow";
            toolTip.SetToolTip(buttonCreationTimeSetNow, resources.GetString("buttonCreationTimeSetNow.ToolTip"));
            buttonCreationTimeSetNow.UseVisualStyleBackColor = true;
            buttonCreationTimeSetNow.Click += buttonCreationTimeSetNow_Click;
            // 
            // buttonLastWriteTimeSetNow
            // 
            resources.ApplyResources(buttonLastWriteTimeSetNow, "buttonLastWriteTimeSetNow");
            buttonLastWriteTimeSetNow.ForeColor = Color.Black;
            buttonLastWriteTimeSetNow.Name = "buttonLastWriteTimeSetNow";
            toolTip.SetToolTip(buttonLastWriteTimeSetNow, resources.GetString("buttonLastWriteTimeSetNow.ToolTip"));
            buttonLastWriteTimeSetNow.UseVisualStyleBackColor = true;
            buttonLastWriteTimeSetNow.Click += buttonLastWriteTimeSetNow_Click;
            // 
            // buttonLastAccessTimeSetNow
            // 
            resources.ApplyResources(buttonLastAccessTimeSetNow, "buttonLastAccessTimeSetNow");
            buttonLastAccessTimeSetNow.ForeColor = Color.Black;
            buttonLastAccessTimeSetNow.Name = "buttonLastAccessTimeSetNow";
            toolTip.SetToolTip(buttonLastAccessTimeSetNow, resources.GetString("buttonLastAccessTimeSetNow.ToolTip"));
            buttonLastAccessTimeSetNow.UseVisualStyleBackColor = true;
            buttonLastAccessTimeSetNow.Click += buttonLastAccessTimeSetNow_Click;
            // 
            // buttonLastAccessTimeSet
            // 
            resources.ApplyResources(buttonLastAccessTimeSet, "buttonLastAccessTimeSet");
            buttonLastAccessTimeSet.ForeColor = Color.Black;
            buttonLastAccessTimeSet.Name = "buttonLastAccessTimeSet";
            toolTip.SetToolTip(buttonLastAccessTimeSet, resources.GetString("buttonLastAccessTimeSet.ToolTip"));
            buttonLastAccessTimeSet.UseVisualStyleBackColor = true;
            buttonLastAccessTimeSet.Click += buttonLastAccessTimeSet_Click;
            // 
            // buttonLastWriteTimeSet
            // 
            resources.ApplyResources(buttonLastWriteTimeSet, "buttonLastWriteTimeSet");
            buttonLastWriteTimeSet.ForeColor = Color.Black;
            buttonLastWriteTimeSet.Name = "buttonLastWriteTimeSet";
            toolTip.SetToolTip(buttonLastWriteTimeSet, resources.GetString("buttonLastWriteTimeSet.ToolTip"));
            buttonLastWriteTimeSet.UseVisualStyleBackColor = true;
            buttonLastWriteTimeSet.Click += buttonLastWriteTimeSet_Click;
            // 
            // buttonCreationTimeSet
            // 
            resources.ApplyResources(buttonCreationTimeSet, "buttonCreationTimeSet");
            buttonCreationTimeSet.ForeColor = Color.Black;
            buttonCreationTimeSet.Name = "buttonCreationTimeSet";
            toolTip.SetToolTip(buttonCreationTimeSet, resources.GetString("buttonCreationTimeSet.ToolTip"));
            buttonCreationTimeSet.UseVisualStyleBackColor = true;
            buttonCreationTimeSet.Click += buttonCreationTimeSet_Click;
            // 
            // buttonStartFile
            // 
            resources.ApplyResources(buttonStartFile, "buttonStartFile");
            buttonStartFile.ForeColor = Color.Black;
            buttonStartFile.Name = "buttonStartFile";
            toolTip.SetToolTip(buttonStartFile, resources.GetString("buttonStartFile.ToolTip"));
            buttonStartFile.UseVisualStyleBackColor = true;
            buttonStartFile.Click += buttonStartFile_Click;
            // 
            // contextMenuStrip
            // 
            resources.ApplyResources(contextMenuStrip, "contextMenuStrip");
            contextMenuStrip.ImageScalingSize = new Size(24, 24);
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { toolStripMenuItem_Automatic, ToolStripMenuItem_Manual, ToolStripMenuItem_ReSet });
            contextMenuStrip.Name = "contextMenuStrip";
            toolTip.SetToolTip(contextMenuStrip, resources.GetString("contextMenuStrip.ToolTip"));
            // 
            // toolStripMenuItem_Automatic
            // 
            resources.ApplyResources(toolStripMenuItem_Automatic, "toolStripMenuItem_Automatic");
            toolStripMenuItem_Automatic.Name = "toolStripMenuItem_Automatic";
            toolStripMenuItem_Automatic.Click += toolStripMenuItem_Automatic_Click;
            // 
            // ToolStripMenuItem_Manual
            // 
            resources.ApplyResources(ToolStripMenuItem_Manual, "ToolStripMenuItem_Manual");
            ToolStripMenuItem_Manual.Name = "ToolStripMenuItem_Manual";
            ToolStripMenuItem_Manual.Click += ToolStripMenuItem_Manual_Click;
            // 
            // ToolStripMenuItem_ReSet
            // 
            resources.ApplyResources(ToolStripMenuItem_ReSet, "ToolStripMenuItem_ReSet");
            ToolStripMenuItem_ReSet.Name = "ToolStripMenuItem_ReSet";
            ToolStripMenuItem_ReSet.Click += ToolStripMenuItem_ReSet_Click;
            // 
            // textBox_ManualPath
            // 
            resources.ApplyResources(textBox_ManualPath, "textBox_ManualPath");
            textBox_ManualPath.ForeColor = Color.Black;
            textBox_ManualPath.Name = "textBox_ManualPath";
            toolTip.SetToolTip(textBox_ManualPath, resources.GetString("textBox_ManualPath.ToolTip"));
            // 
            // button_ManualPath
            // 
            resources.ApplyResources(button_ManualPath, "button_ManualPath");
            button_ManualPath.ForeColor = Color.Black;
            button_ManualPath.Name = "button_ManualPath";
            toolTip.SetToolTip(button_ManualPath, resources.GetString("button_ManualPath.ToolTip"));
            button_ManualPath.UseVisualStyleBackColor = true;
            button_ManualPath.Click += button_ManualPath_Click;
            // 
            // checkBoxHidden
            // 
            resources.ApplyResources(checkBoxHidden, "checkBoxHidden");
            checkBoxHidden.ForeColor = Color.Black;
            checkBoxHidden.Name = "checkBoxHidden";
            toolTip.SetToolTip(checkBoxHidden, resources.GetString("checkBoxHidden.ToolTip"));
            checkBoxHidden.UseVisualStyleBackColor = true;
            checkBoxHidden.CheckedChanged += checkBoxHidden_CheckedChanged;
            // 
            // checkBoxArchive
            // 
            resources.ApplyResources(checkBoxArchive, "checkBoxArchive");
            checkBoxArchive.ForeColor = Color.Black;
            checkBoxArchive.Name = "checkBoxArchive";
            toolTip.SetToolTip(checkBoxArchive, resources.GetString("checkBoxArchive.ToolTip"));
            checkBoxArchive.UseVisualStyleBackColor = true;
            checkBoxArchive.CheckedChanged += checkBoxArchive_CheckedChanged;
            // 
            // checkBoxReadOnly
            // 
            resources.ApplyResources(checkBoxReadOnly, "checkBoxReadOnly");
            checkBoxReadOnly.ForeColor = Color.Black;
            checkBoxReadOnly.Name = "checkBoxReadOnly";
            toolTip.SetToolTip(checkBoxReadOnly, resources.GetString("checkBoxReadOnly.ToolTip"));
            checkBoxReadOnly.UseVisualStyleBackColor = true;
            checkBoxReadOnly.CheckedChanged += checkBoxReadOnly_CheckedChanged;
            // 
            // checkBoxSystem
            // 
            resources.ApplyResources(checkBoxSystem, "checkBoxSystem");
            checkBoxSystem.ForeColor = Color.Black;
            checkBoxSystem.Name = "checkBoxSystem";
            toolTip.SetToolTip(checkBoxSystem, resources.GetString("checkBoxSystem.ToolTip"));
            checkBoxSystem.UseVisualStyleBackColor = true;
            checkBoxSystem.CheckedChanged += checkBoxSystem_CheckedChanged;
            // 
            // checkBoxOffline
            // 
            resources.ApplyResources(checkBoxOffline, "checkBoxOffline");
            checkBoxOffline.ForeColor = Color.Black;
            checkBoxOffline.Name = "checkBoxOffline";
            toolTip.SetToolTip(checkBoxOffline, resources.GetString("checkBoxOffline.ToolTip"));
            checkBoxOffline.UseVisualStyleBackColor = true;
            checkBoxOffline.CheckedChanged += checkBoxOffline_CheckedChanged;
            // 
            // checkBoxNotContentIndexed
            // 
            resources.ApplyResources(checkBoxNotContentIndexed, "checkBoxNotContentIndexed");
            checkBoxNotContentIndexed.ForeColor = Color.Black;
            checkBoxNotContentIndexed.Name = "checkBoxNotContentIndexed";
            toolTip.SetToolTip(checkBoxNotContentIndexed, resources.GetString("checkBoxNotContentIndexed.ToolTip"));
            checkBoxNotContentIndexed.UseVisualStyleBackColor = true;
            checkBoxNotContentIndexed.CheckedChanged += checkBoxNotContentIndexed_CheckedChanged;
            // 
            // buttonCover
            // 
            resources.ApplyResources(buttonCover, "buttonCover");
            buttonCover.ForeColor = Color.Black;
            buttonCover.Name = "buttonCover";
            toolTip.SetToolTip(buttonCover, resources.GetString("buttonCover.ToolTip"));
            buttonCover.UseVisualStyleBackColor = true;
            buttonCover.Click += buttonCover_Click;
            // 
            // buttonAppend
            // 
            resources.ApplyResources(buttonAppend, "buttonAppend");
            buttonAppend.ForeColor = Color.Black;
            buttonAppend.Name = "buttonAppend";
            toolTip.SetToolTip(buttonAppend, resources.GetString("buttonAppend.ToolTip"));
            buttonAppend.UseVisualStyleBackColor = true;
            // 
            // buttonRead
            // 
            resources.ApplyResources(buttonRead, "buttonRead");
            buttonRead.ForeColor = Color.Black;
            buttonRead.Name = "buttonRead";
            toolTip.SetToolTip(buttonRead, resources.GetString("buttonRead.ToolTip"));
            buttonRead.UseVisualStyleBackColor = true;
            buttonRead.Click += buttonRead_Click;
            // 
            // checkBoxBinaryFile
            // 
            resources.ApplyResources(checkBoxBinaryFile, "checkBoxBinaryFile");
            checkBoxBinaryFile.ForeColor = Color.Black;
            checkBoxBinaryFile.Name = "checkBoxBinaryFile";
            toolTip.SetToolTip(checkBoxBinaryFile, resources.GetString("checkBoxBinaryFile.ToolTip"));
            checkBoxBinaryFile.UseVisualStyleBackColor = true;
            checkBoxBinaryFile.CheckedChanged += checkBoxBinaryFile_CheckedChanged;
            // 
            // richTextBox
            // 
            resources.ApplyResources(richTextBox, "richTextBox");
            richTextBox.Name = "richTextBox";
            toolTip.SetToolTip(richTextBox, resources.GetString("richTextBox.ToolTip"));
            // 
            // button_FilesOperationPlus
            // 
            resources.ApplyResources(button_FilesOperationPlus, "button_FilesOperationPlus");
            button_FilesOperationPlus.Name = "button_FilesOperationPlus";
            toolTip.SetToolTip(button_FilesOperationPlus, resources.GetString("button_FilesOperationPlus.ToolTip"));
            button_FilesOperationPlus.UseVisualStyleBackColor = true;
            button_FilesOperationPlus.Click += button_FilesOperationPlus_Click;
            // 
            // FilesOperation
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(button_FilesOperationPlus);
            Controls.Add(checkBoxNotContentIndexed);
            Controls.Add(checkBoxBinaryFile);
            Controls.Add(buttonRead);
            Controls.Add(buttonAppend);
            Controls.Add(buttonCover);
            Controls.Add(richTextBox);
            Controls.Add(checkBoxOffline);
            Controls.Add(checkBoxSystem);
            Controls.Add(checkBoxReadOnly);
            Controls.Add(checkBoxArchive);
            Controls.Add(checkBoxHidden);
            Controls.Add(button_ManualPath);
            Controls.Add(textBox_ManualPath);
            Controls.Add(buttonStartFile);
            Controls.Add(buttonLastAccessTimeSet);
            Controls.Add(buttonLastWriteTimeSet);
            Controls.Add(buttonCreationTimeSet);
            Controls.Add(buttonLastAccessTimeSetNow);
            Controls.Add(buttonLastWriteTimeSetNow);
            Controls.Add(buttonCreationTimeSetNow);
            Controls.Add(textBoxLastAccessTime);
            Controls.Add(labelLastAccessTime);
            Controls.Add(textBoxLastWriteTime);
            Controls.Add(labelLastWriteTime);
            Controls.Add(textBoxCreationTime);
            Controls.Add(labelCreationTime);
            Controls.Add(buttonReadFile);
            Controls.Add(buttonClearFile);
            Controls.Add(buttonDeleteFile);
            Controls.Add(labelFileName);
            Name = "FilesOperation";
            toolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
            Load += FilesOperation_Load;
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SaveFileDialog saveFileDialog;
        private Label labelFileName;
        private OpenFileDialog openFileDialog;
        private Button buttonDeleteFile;
        private Button buttonClearFile;
        private Button buttonReadFile;
        private Label labelCreationTime;
        private TextBox textBoxCreationTime;
        private TextBox textBoxLastWriteTime;
        private Label labelLastWriteTime;
        private TextBox textBoxLastAccessTime;
        private Label labelLastAccessTime;
        private Button buttonCreationTimeSetNow;
        private Button buttonLastWriteTimeSetNow;
        private Button buttonLastAccessTimeSetNow;
        private Button buttonLastAccessTimeSet;
        private Button buttonLastWriteTimeSet;
        private Button buttonCreationTimeSet;
        private Button buttonStartFile;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem toolStripMenuItem_Automatic;
        private ToolStripMenuItem ToolStripMenuItem_Manual;
        private TextBox textBox_ManualPath;
        private Button button_ManualPath;
        private CheckBox checkBoxHidden;
        private ToolStripMenuItem ToolStripMenuItem_ReSet;
        private CheckBox checkBoxArchive;
        private CheckBox checkBoxReadOnly;
        private CheckBox checkBoxSystem;
        private CheckBox checkBoxOffline;
        private CheckBox checkBoxNotContentIndexed;
        private ToolTip toolTip;
        private RichTextBox richTextBox;
        private Button buttonCover;
        private Button buttonAppend;
        private Button buttonRead;
        private CheckBox checkBoxBinaryFile;
        private Button button_FilesOperationPlus;
    }
}