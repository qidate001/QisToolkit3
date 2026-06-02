namespace QisToolkit3.Forms.IndependentTools
{
    partial class ShellFolderSetPathTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShellFolderSetPathTool));
            buttonAddItem = new Button();
            splitContainer1 = new SplitContainer();
            listBox = new ListBox();
            button_FixShellFolderNameAndIcon_lnk = new Button();
            label1 = new Label();
            comboBoxData = new ComboBox();
            label2 = new Label();
            comboBoxName = new ComboBox();
            buttonDeleteItem = new Button();
            button_ReStartExplorer = new Button();
            button_ReLoad = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // buttonAddItem
            // 
            buttonAddItem.Dock = DockStyle.Top;
            buttonAddItem.Enabled = false;
            buttonAddItem.Font = new Font("微软雅黑", 24F);
            buttonAddItem.ForeColor = Color.Black;
            buttonAddItem.Location = new Point(0, 80);
            buttonAddItem.Name = "buttonAddItem";
            buttonAddItem.Size = new Size(713, 80);
            buttonAddItem.TabIndex = 1;
            buttonAddItem.Text = "添加至列表";
            buttonAddItem.UseVisualStyleBackColor = true;
            buttonAddItem.Click += buttonAddItem_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(listBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(button_FixShellFolderNameAndIcon_lnk);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Panel2.Controls.Add(comboBoxData);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Panel2.Controls.Add(comboBoxName);
            splitContainer1.Panel2.Controls.Add(buttonAddItem);
            splitContainer1.Panel2.Controls.Add(buttonDeleteItem);
            splitContainer1.Panel2.Controls.Add(button_ReStartExplorer);
            splitContainer1.Panel2.Controls.Add(button_ReLoad);
            splitContainer1.Size = new Size(1074, 875);
            splitContainer1.SplitterDistance = 357;
            splitContainer1.TabIndex = 1;
            // 
            // listBox
            // 
            listBox.Dock = DockStyle.Fill;
            listBox.Font = new Font("微软雅黑", 12F);
            listBox.FormattingEnabled = true;
            listBox.ItemHeight = 27;
            listBox.Location = new Point(0, 0);
            listBox.Name = "listBox";
            listBox.SelectionMode = SelectionMode.MultiExtended;
            listBox.Size = new Size(357, 875);
            listBox.TabIndex = 0;
            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
            // 
            // button_FixShellFolderNameAndIcon_lnk
            // 
            button_FixShellFolderNameAndIcon_lnk.Dock = DockStyle.Bottom;
            button_FixShellFolderNameAndIcon_lnk.Font = new Font("微软雅黑", 12F);
            button_FixShellFolderNameAndIcon_lnk.ForeColor = Color.FromArgb(192, 64, 0);
            button_FixShellFolderNameAndIcon_lnk.Location = new Point(0, 734);
            button_FixShellFolderNameAndIcon_lnk.Name = "button_FixShellFolderNameAndIcon_lnk";
            button_FixShellFolderNameAndIcon_lnk.Size = new Size(713, 47);
            button_FixShellFolderNameAndIcon_lnk.TabIndex = 21;
            button_FixShellFolderNameAndIcon_lnk.Text = "快速修复图标与名称";
            button_FixShellFolderNameAndIcon_lnk.UseVisualStyleBackColor = true;
            button_FixShellFolderNameAndIcon_lnk.Click += button_FixShellFolderNameAndIcon_lnk_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label1.Location = new Point(0, 250);
            label1.Name = "label1";
            label1.Size = new Size(659, 380);
            label1.TabIndex = 18;
            label1.Text = resources.GetString("label1.Text");
            // 
            // comboBoxData
            // 
            comboBoxData.Dock = DockStyle.Top;
            comboBoxData.Font = new Font("微软雅黑", 12F);
            comboBoxData.FormattingEnabled = true;
            comboBoxData.Items.AddRange(new object[] { "%USERPROFILE%\\", "%USERPROFILE%\\Desktop", "D:\\UserFolder\\Desktop" });
            comboBoxData.Location = new Point(0, 215);
            comboBoxData.Name = "comboBoxData";
            comboBoxData.Size = new Size(713, 35);
            comboBoxData.TabIndex = 17;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label2.Location = new Point(0, 195);
            label2.Name = "label2";
            label2.Size = new Size(418, 20);
            label2.TabIndex = 16;
            label2.Text = "在上方输入要修改的Shell文件夹名或GUID↑↑↑   例：Desktop";
            // 
            // comboBoxName
            // 
            comboBoxName.Dock = DockStyle.Top;
            comboBoxName.Font = new Font("微软雅黑", 12F);
            comboBoxName.FormattingEnabled = true;
            comboBoxName.Items.AddRange(new object[] { "cmd.exe", "calc.exe", "sethc.exe", "utilman.exe", "magnify.exe", "notepad.exe", "Taskmgr.exe" });
            comboBoxName.Location = new Point(0, 160);
            comboBoxName.Name = "comboBoxName";
            comboBoxName.Size = new Size(713, 35);
            comboBoxName.TabIndex = 15;
            comboBoxName.TextChanged += comboBoxName_TextChanged;
            // 
            // buttonDeleteItem
            // 
            buttonDeleteItem.Dock = DockStyle.Top;
            buttonDeleteItem.Enabled = false;
            buttonDeleteItem.Font = new Font("微软雅黑", 24F);
            buttonDeleteItem.ForeColor = Color.Red;
            buttonDeleteItem.Location = new Point(0, 0);
            buttonDeleteItem.Name = "buttonDeleteItem";
            buttonDeleteItem.Size = new Size(713, 80);
            buttonDeleteItem.TabIndex = 0;
            buttonDeleteItem.Text = "删除选择项";
            buttonDeleteItem.UseVisualStyleBackColor = true;
            buttonDeleteItem.Click += buttonDeleteItem_Click;
            // 
            // button_ReStartExplorer
            // 
            button_ReStartExplorer.Dock = DockStyle.Bottom;
            button_ReStartExplorer.Font = new Font("微软雅黑", 12F);
            button_ReStartExplorer.Location = new Point(0, 781);
            button_ReStartExplorer.Name = "button_ReStartExplorer";
            button_ReStartExplorer.Size = new Size(713, 47);
            button_ReStartExplorer.TabIndex = 22;
            button_ReStartExplorer.Text = "立即应用（重启资源管理器）";
            button_ReStartExplorer.UseVisualStyleBackColor = true;
            button_ReStartExplorer.Click += button_ReStartExplorer_Click;
            // 
            // button_ReLoad
            // 
            button_ReLoad.Dock = DockStyle.Bottom;
            button_ReLoad.Font = new Font("微软雅黑", 12F);
            button_ReLoad.Location = new Point(0, 828);
            button_ReLoad.Name = "button_ReLoad";
            button_ReLoad.Size = new Size(713, 47);
            button_ReLoad.TabIndex = 20;
            button_ReLoad.Text = "刷新";
            button_ReLoad.UseVisualStyleBackColor = true;
            button_ReLoad.Click += button_ReLoad_Click;
            // 
            // ShellFolderSetPathTool
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1074, 875);
            Controls.Add(splitContainer1);
            Name = "ShellFolderSetPathTool";
            Text = "Shell 文件夹重定向工具";
            Load += ShellFolderSetPathTool_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button buttonAddItem;
        private SplitContainer splitContainer1;
        private ListBox listBox;
        private Button button_ReLoad;
        private Label label1;
        private ComboBox comboBoxData;
        private Label label2;
        private ComboBox comboBoxName;
        private Button buttonDeleteItem;
        private Button button_FixShellFolderNameAndIcon_lnk;
        private Button button_ReStartExplorer;
    }
}