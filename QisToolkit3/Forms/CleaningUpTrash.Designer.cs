namespace QisToolkit3.Forms
{
    partial class CleaningUpTrash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CleaningUpTrash));
            splitContainer1 = new SplitContainer();
            checkedListBox = new CheckedListBox();
            splitContainer2 = new SplitContainer();
            label_AllDeleteFileSize = new Label();
            label_AllDeleteFileCount = new Label();
            checkBoxKillProcess = new CheckBox();
            checkBoxUserCompatibilityMode = new CheckBox();
            label_AllFileSize = new Label();
            label_AllFileCount = new Label();
            buttonCleaningUp = new Button();
            buttonScanGarbage = new Button();
            richTextBox = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(splitContainer1, "splitContainer1");
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(checkedListBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            // 
            // checkedListBox
            // 
            resources.ApplyResources(checkedListBox, "checkedListBox");
            checkedListBox.FormattingEnabled = true;
            checkedListBox.Name = "checkedListBox";
            checkedListBox.SelectedIndexChanged += checkedListBox_SelectedIndexChanged;
            // 
            // splitContainer2
            // 
            resources.ApplyResources(splitContainer2, "splitContainer2");
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(label_AllDeleteFileSize);
            splitContainer2.Panel1.Controls.Add(label_AllDeleteFileCount);
            splitContainer2.Panel1.Controls.Add(checkBoxKillProcess);
            splitContainer2.Panel1.Controls.Add(checkBoxUserCompatibilityMode);
            splitContainer2.Panel1.Controls.Add(label_AllFileSize);
            splitContainer2.Panel1.Controls.Add(label_AllFileCount);
            splitContainer2.Panel1.Controls.Add(buttonCleaningUp);
            splitContainer2.Panel1.Controls.Add(buttonScanGarbage);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(richTextBox);
            // 
            // label_AllDeleteFileSize
            // 
            resources.ApplyResources(label_AllDeleteFileSize, "label_AllDeleteFileSize");
            label_AllDeleteFileSize.Name = "label_AllDeleteFileSize";
            label_AllDeleteFileSize.Click += label_AllDeleteFileSize_Click;
            // 
            // label_AllDeleteFileCount
            // 
            resources.ApplyResources(label_AllDeleteFileCount, "label_AllDeleteFileCount");
            label_AllDeleteFileCount.Name = "label_AllDeleteFileCount";
            // 
            // checkBoxKillProcess
            // 
            resources.ApplyResources(checkBoxKillProcess, "checkBoxKillProcess");
            checkBoxKillProcess.Name = "checkBoxKillProcess";
            checkBoxKillProcess.UseVisualStyleBackColor = true;
            checkBoxKillProcess.CheckedChanged += checkBoxKillAllUserProcess_CheckedChanged;
            // 
            // checkBoxUserCompatibilityMode
            // 
            resources.ApplyResources(checkBoxUserCompatibilityMode, "checkBoxUserCompatibilityMode");
            checkBoxUserCompatibilityMode.Name = "checkBoxUserCompatibilityMode";
            checkBoxUserCompatibilityMode.UseVisualStyleBackColor = true;
            checkBoxUserCompatibilityMode.CheckedChanged += checkBoxUserCompatibilityMode_CheckedChanged;
            // 
            // label_AllFileSize
            // 
            resources.ApplyResources(label_AllFileSize, "label_AllFileSize");
            label_AllFileSize.Name = "label_AllFileSize";
            label_AllFileSize.Click += label_AllFileSize_Click;
            // 
            // label_AllFileCount
            // 
            resources.ApplyResources(label_AllFileCount, "label_AllFileCount");
            label_AllFileCount.Name = "label_AllFileCount";
            // 
            // buttonCleaningUp
            // 
            resources.ApplyResources(buttonCleaningUp, "buttonCleaningUp");
            buttonCleaningUp.Name = "buttonCleaningUp";
            buttonCleaningUp.UseVisualStyleBackColor = true;
            buttonCleaningUp.Click += buttonCleaningUp_Click;
            // 
            // buttonScanGarbage
            // 
            resources.ApplyResources(buttonScanGarbage, "buttonScanGarbage");
            buttonScanGarbage.Name = "buttonScanGarbage";
            buttonScanGarbage.UseVisualStyleBackColor = true;
            buttonScanGarbage.Click += buttonScanGarbage_Click;
            // 
            // richTextBox
            // 
            resources.ApplyResources(richTextBox, "richTextBox");
            richTextBox.Name = "richTextBox";
            richTextBox.ReadOnly = true;
            // 
            // CleaningUpTrash
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            HelpButton = true;
            Name = "CleaningUpTrash";
            HelpButtonClicked += CleaningUpTrash_HelpButtonClicked;
            Load += CleaningUpTrash_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel1.PerformLayout();
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private CheckedListBox checkedListBox;
        private SplitContainer splitContainer2;
        private RichTextBox richTextBox;
        private Button button2;
        private Button buttonScanGarbage;
        private Button buttonCleaningUp;
        private Label label_AllFileCount;
        private Label label_AllFileSize;
        private CheckBox checkBoxUserCompatibilityMode;
        private CheckBox checkBoxKillProcess;
        private Label label_AllDeleteFileSize;
        private Label label_AllDeleteFileCount;
    }
}