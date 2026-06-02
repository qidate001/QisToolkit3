namespace QisToolkit3.Forms
{
    partial class ImageHijackingTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageHijackingTool));
            splitContainer1 = new SplitContainer();
            listBox = new ListBox();
            checkBox_DisableUserModeCallbackFilter = new CheckBox();
            checkBox_ShowAll = new CheckBox();
            label3 = new Label();
            comboBoxDataMO = new ComboBox();
            button_ReLoad = new Button();
            label1 = new Label();
            comboBoxData = new ComboBox();
            label2 = new Label();
            comboBoxName = new ComboBox();
            buttonAddItem = new Button();
            buttonDeleteItem = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
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
            splitContainer1.Panel1.Controls.Add(listBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(checkBox_DisableUserModeCallbackFilter);
            splitContainer1.Panel2.Controls.Add(checkBox_ShowAll);
            splitContainer1.Panel2.Controls.Add(label3);
            splitContainer1.Panel2.Controls.Add(comboBoxDataMO);
            splitContainer1.Panel2.Controls.Add(button_ReLoad);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Panel2.Controls.Add(comboBoxData);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Panel2.Controls.Add(comboBoxName);
            splitContainer1.Panel2.Controls.Add(buttonAddItem);
            splitContainer1.Panel2.Controls.Add(buttonDeleteItem);
            splitContainer1.Size = new Size(800, 761);
            splitContainer1.SplitterDistance = 266;
            splitContainer1.TabIndex = 0;
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
            listBox.Size = new Size(266, 761);
            listBox.TabIndex = 0;
            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
            // 
            // checkBox_DisableUserModeCallbackFilter
            // 
            checkBox_DisableUserModeCallbackFilter.AutoSize = true;
            checkBox_DisableUserModeCallbackFilter.Font = new Font("Microsoft YaHei UI", 12F);
            checkBox_DisableUserModeCallbackFilter.Location = new Point(3, 468);
            checkBox_DisableUserModeCallbackFilter.Name = "checkBox_DisableUserModeCallbackFilter";
            checkBox_DisableUserModeCallbackFilter.Size = new Size(194, 31);
            checkBox_DisableUserModeCallbackFilter.TabIndex = 24;
            checkBox_DisableUserModeCallbackFilter.Text = "用户模式回调过滤";
            checkBox_DisableUserModeCallbackFilter.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowAll
            // 
            checkBox_ShowAll.AutoSize = true;
            checkBox_ShowAll.Dock = DockStyle.Bottom;
            checkBox_ShowAll.Location = new Point(0, 690);
            checkBox_ShowAll.Name = "checkBox_ShowAll";
            checkBox_ShowAll.Size = new Size(530, 24);
            checkBox_ShowAll.TabIndex = 23;
            checkBox_ShowAll.Text = "显示所有项";
            checkBox_ShowAll.UseVisualStyleBackColor = true;
            checkBox_ShowAll.CheckedChanged += checkBox_ShowAll_CheckedChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Top;
            label3.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label3.Location = new Point(0, 405);
            label3.Name = "label3";
            label3.Size = new Size(496, 60);
            label3.TabIndex = 22;
            label3.Text = "在上方输入执行的缓解选项的掩码↑↑↑   例：0x100\r\n如果您想要同时开启策略：0x100（限制子进程策略） 0x2（强制ASLR）\r\n那么您可以这么写：0x100 | 0x2  程序将自动计算成 0x00000102";
            // 
            // comboBoxDataMO
            // 
            comboBoxDataMO.Dock = DockStyle.Top;
            comboBoxDataMO.Font = new Font("微软雅黑", 12F);
            comboBoxDataMO.FormattingEnabled = true;
            comboBoxDataMO.Items.AddRange(new object[] { "0x00000000", "0x00000002", "0x00000100", "0x00200000", "0x00111111" });
            comboBoxDataMO.Location = new Point(0, 370);
            comboBoxDataMO.Name = "comboBoxDataMO";
            comboBoxDataMO.Size = new Size(530, 35);
            comboBoxDataMO.TabIndex = 21;
            // 
            // button_ReLoad
            // 
            button_ReLoad.Dock = DockStyle.Bottom;
            button_ReLoad.Font = new Font("微软雅黑", 12F);
            button_ReLoad.Location = new Point(0, 714);
            button_ReLoad.Name = "button_ReLoad";
            button_ReLoad.Size = new Size(530, 47);
            button_ReLoad.TabIndex = 20;
            button_ReLoad.Text = "刷新";
            button_ReLoad.UseVisualStyleBackColor = true;
            button_ReLoad.Click += button_ReLoad_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label1.Location = new Point(0, 250);
            label1.Name = "label1";
            label1.Size = new Size(494, 120);
            label1.TabIndex = 18;
            label1.Text = resources.GetString("label1.Text");
            // 
            // comboBoxData
            // 
            comboBoxData.Dock = DockStyle.Top;
            comboBoxData.Font = new Font("微软雅黑", 12F);
            comboBoxData.FormattingEnabled = true;
            comboBoxData.Items.AddRange(new object[] { "*", "ctfmon.exe", "cmd.exe", "calc.exe", "notepad.exe", "QisToolkit3.exe", "%windir%\\System32\\taskkill.exe", "%windir%\\System32\\cmd.exe" });
            comboBoxData.Location = new Point(0, 215);
            comboBoxData.Name = "comboBoxData";
            comboBoxData.Size = new Size(530, 35);
            comboBoxData.TabIndex = 17;
            comboBoxData.Text = "*";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label2.Location = new Point(0, 195);
            label2.Name = "label2";
            label2.Size = new Size(347, 20);
            label2.TabIndex = 16;
            label2.Text = "在上方输入要劫持的程序文件名↑↑↑   例：cmd.exe";
            // 
            // comboBoxName
            // 
            comboBoxName.Dock = DockStyle.Top;
            comboBoxName.Font = new Font("微软雅黑", 12F);
            comboBoxName.FormattingEnabled = true;
            comboBoxName.Items.AddRange(new object[] { "cmd.exe", "calc.exe", "sethc.exe", "utilman.exe", "magnify.exe", "notepad.exe", "Taskmgr.exe" });
            comboBoxName.Location = new Point(0, 160);
            comboBoxName.Name = "comboBoxName";
            comboBoxName.Size = new Size(530, 35);
            comboBoxName.TabIndex = 15;
            comboBoxName.SelectedIndexChanged += comboBoxName_SelectedIndexChanged;
            comboBoxName.TextChanged += comboBoxName_SelectedIndexChanged;
            // 
            // buttonAddItem
            // 
            buttonAddItem.Dock = DockStyle.Top;
            buttonAddItem.Enabled = false;
            buttonAddItem.Font = new Font("微软雅黑", 24F);
            buttonAddItem.Location = new Point(0, 80);
            buttonAddItem.Name = "buttonAddItem";
            buttonAddItem.Size = new Size(530, 80);
            buttonAddItem.TabIndex = 1;
            buttonAddItem.Text = "添加至列表";
            buttonAddItem.UseVisualStyleBackColor = true;
            buttonAddItem.Click += buttonAddItem_Click;
            // 
            // buttonDeleteItem
            // 
            buttonDeleteItem.Dock = DockStyle.Top;
            buttonDeleteItem.Enabled = false;
            buttonDeleteItem.Font = new Font("微软雅黑", 24F);
            buttonDeleteItem.Location = new Point(0, 0);
            buttonDeleteItem.Name = "buttonDeleteItem";
            buttonDeleteItem.Size = new Size(530, 80);
            buttonDeleteItem.TabIndex = 0;
            buttonDeleteItem.Text = "删除选择项";
            buttonDeleteItem.UseVisualStyleBackColor = true;
            buttonDeleteItem.Click += buttonDeleteItem_Click;
            // 
            // ImageHijackingTool
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 761);
            Controls.Add(splitContainer1);
            Name = "ImageHijackingTool";
            Text = "IFEO映像劫持工具";
            Load += ImageHijackingTool_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private ListBox listBox;
        private Button buttonDeleteItem;
        private TextBox textBoxName;
        private Button buttonAddItem;
        private Label label1;
        private ComboBox comboBoxData;
        private Label label2;
        private ComboBox comboBoxName;
        private Button button_ReLoad;
        private Label label3;
        private ComboBox comboBoxDataMO;
        private CheckBox checkBox_DisableUserModeCallbackFilter;
        private CheckBox checkBox_ShowAll;
    }
}