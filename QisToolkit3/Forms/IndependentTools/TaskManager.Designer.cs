namespace QisToolkit3.Forms
{
    partial class TaskManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskManager));
            splitContainer1 = new SplitContainer();
            listBox = new ListBox();
            button7 = new Button();
            button_CTRL_SHUTDOWN_EVENT = new Button();
            button_WM_QUERYENDSESSION = new Button();
            button_JobKill = new Button();
            button_WM_ENDSESSION = new Button();
            button_Kill = new Button();
            label1 = new Label();
            button_RemoveCriticalProcess = new Button();
            button_ReLoad = new Button();
            button_AddCriticalProcess = new Button();
            button_WM_CLOSE = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(splitContainer1, "splitContainer1");
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(listBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(button7);
            splitContainer1.Panel2.Controls.Add(button_CTRL_SHUTDOWN_EVENT);
            splitContainer1.Panel2.Controls.Add(button_WM_QUERYENDSESSION);
            splitContainer1.Panel2.Controls.Add(button_JobKill);
            splitContainer1.Panel2.Controls.Add(button_WM_ENDSESSION);
            splitContainer1.Panel2.Controls.Add(button_Kill);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Panel2.Controls.Add(button_RemoveCriticalProcess);
            splitContainer1.Panel2.Controls.Add(button_ReLoad);
            splitContainer1.Panel2.Controls.Add(button_AddCriticalProcess);
            splitContainer1.Panel2.Controls.Add(button_WM_CLOSE);
            splitContainer1.Panel2.Paint += splitContainer1_Panel2_Paint;
            // 
            // listBox
            // 
            resources.ApplyResources(listBox, "listBox");
            listBox.FormattingEnabled = true;
            listBox.Name = "listBox";
            listBox.SelectionMode = SelectionMode.MultiExtended;
            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
            // 
            // button7
            // 
            resources.ApplyResources(button7, "button7");
            button7.ForeColor = SystemColors.ControlText;
            button7.Name = "button7";
            button7.UseVisualStyleBackColor = true;
            // 
            // button_CTRL_SHUTDOWN_EVENT
            // 
            resources.ApplyResources(button_CTRL_SHUTDOWN_EVENT, "button_CTRL_SHUTDOWN_EVENT");
            button_CTRL_SHUTDOWN_EVENT.ForeColor = SystemColors.ControlText;
            button_CTRL_SHUTDOWN_EVENT.Name = "button_CTRL_SHUTDOWN_EVENT";
            button_CTRL_SHUTDOWN_EVENT.UseVisualStyleBackColor = true;
            button_CTRL_SHUTDOWN_EVENT.Click += button_CTRL_SHUTDOWN_EVENT_Click;
            // 
            // button_WM_QUERYENDSESSION
            // 
            resources.ApplyResources(button_WM_QUERYENDSESSION, "button_WM_QUERYENDSESSION");
            button_WM_QUERYENDSESSION.ForeColor = SystemColors.ControlText;
            button_WM_QUERYENDSESSION.Name = "button_WM_QUERYENDSESSION";
            button_WM_QUERYENDSESSION.UseVisualStyleBackColor = true;
            button_WM_QUERYENDSESSION.Click += button_WM_QUERYENDSESSION_Click;
            // 
            // button_JobKill
            // 
            resources.ApplyResources(button_JobKill, "button_JobKill");
            button_JobKill.ForeColor = SystemColors.ControlText;
            button_JobKill.Name = "button_JobKill";
            button_JobKill.UseVisualStyleBackColor = true;
            button_JobKill.Click += button_JobKill_Click;
            // 
            // button_WM_ENDSESSION
            // 
            resources.ApplyResources(button_WM_ENDSESSION, "button_WM_ENDSESSION");
            button_WM_ENDSESSION.ForeColor = SystemColors.ControlText;
            button_WM_ENDSESSION.Name = "button_WM_ENDSESSION";
            button_WM_ENDSESSION.UseVisualStyleBackColor = true;
            button_WM_ENDSESSION.Click += button_WM_ENDSESSION_Click;
            // 
            // button_Kill
            // 
            resources.ApplyResources(button_Kill, "button_Kill");
            button_Kill.ForeColor = SystemColors.ControlText;
            button_Kill.Name = "button_Kill";
            button_Kill.UseVisualStyleBackColor = true;
            button_Kill.Click += button_Kill_Click;
            // 
            // label1
            // 
            label1.BackColor = Color.Gainsboro;
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // button_RemoveCriticalProcess
            // 
            resources.ApplyResources(button_RemoveCriticalProcess, "button_RemoveCriticalProcess");
            button_RemoveCriticalProcess.Name = "button_RemoveCriticalProcess";
            button_RemoveCriticalProcess.UseVisualStyleBackColor = true;
            button_RemoveCriticalProcess.Click += button_RemoveCriticalProcess_Click;
            // 
            // button_ReLoad
            // 
            resources.ApplyResources(button_ReLoad, "button_ReLoad");
            button_ReLoad.Name = "button_ReLoad";
            button_ReLoad.UseVisualStyleBackColor = true;
            button_ReLoad.Click += TaskManager_Load;
            // 
            // button_AddCriticalProcess
            // 
            resources.ApplyResources(button_AddCriticalProcess, "button_AddCriticalProcess");
            button_AddCriticalProcess.Name = "button_AddCriticalProcess";
            button_AddCriticalProcess.UseVisualStyleBackColor = true;
            button_AddCriticalProcess.Click += button_AddCriticalProcess_Click;
            // 
            // button_WM_CLOSE
            // 
            resources.ApplyResources(button_WM_CLOSE, "button_WM_CLOSE");
            button_WM_CLOSE.ForeColor = SystemColors.ControlText;
            button_WM_CLOSE.Name = "button_WM_CLOSE";
            button_WM_CLOSE.UseVisualStyleBackColor = true;
            button_WM_CLOSE.Click += button_WM_CLOSE_Click;
            // 
            // TaskManager
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Name = "TaskManager";
            Load += TaskManager_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private ListBox listBox;
        private Button button_ReLoad;
        private Button button_AddCriticalProcess;
        private Button button_WM_CLOSE;
        private Button button_RemoveCriticalProcess;
        private Button button_Kill;
        private Label label1;
        private Button button_JobKill;
        private Button button_WM_ENDSESSION;
        private Button button_WM_QUERYENDSESSION;
        private Button button_CTRL_SHUTDOWN_EVENT;
        private Button button7;
    }
}