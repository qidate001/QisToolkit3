namespace QisToolkit3.Forms
{
    partial class SystemServiceTools
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
            listBox = new ListBox();
            checkBox_ShowAll = new CheckBox();
            button_ReLoad = new Button();
            label1 = new Label();
            comboBoxStart = new ComboBox();
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
            splitContainer1.Panel2.Controls.Add(checkBox_ShowAll);
            splitContainer1.Panel2.Controls.Add(button_ReLoad);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Panel2.Controls.Add(comboBoxStart);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Panel2.Controls.Add(comboBoxName);
            splitContainer1.Panel2.Controls.Add(buttonAddItem);
            splitContainer1.Panel2.Controls.Add(buttonDeleteItem);
            splitContainer1.Size = new Size(874, 796);
            splitContainer1.SplitterDistance = 291;
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
            listBox.Size = new Size(291, 796);
            listBox.TabIndex = 0;
            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
            // 
            // checkBox_ShowAll
            // 
            checkBox_ShowAll.AutoSize = true;
            checkBox_ShowAll.Dock = DockStyle.Bottom;
            checkBox_ShowAll.Location = new Point(0, 725);
            checkBox_ShowAll.Name = "checkBox_ShowAll";
            checkBox_ShowAll.Size = new Size(579, 24);
            checkBox_ShowAll.TabIndex = 24;
            checkBox_ShowAll.Text = "显示所有项";
            checkBox_ShowAll.UseVisualStyleBackColor = true;
            checkBox_ShowAll.CheckedChanged += checkBox_ShowAll_CheckedChanged;
            // 
            // button_ReLoad
            // 
            button_ReLoad.Dock = DockStyle.Bottom;
            button_ReLoad.Font = new Font("微软雅黑", 12F);
            button_ReLoad.Location = new Point(0, 749);
            button_ReLoad.Name = "button_ReLoad";
            button_ReLoad.Size = new Size(579, 47);
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
            label1.Size = new Size(172, 20);
            label1.TabIndex = 18;
            label1.Text = "在上方输入启动模式 ↑↑↑";
            // 
            // comboBoxStart
            // 
            comboBoxStart.Dock = DockStyle.Top;
            comboBoxStart.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxStart.Font = new Font("微软雅黑", 12F);
            comboBoxStart.FormattingEnabled = true;
            comboBoxStart.Items.AddRange(new object[] { "引导启动  (0x0)", "系统启动  (0x1)", "自动启动  (0x2)", "手动启动  (0x3)", "彻底禁用  (0x4)" });
            comboBoxStart.Location = new Point(0, 215);
            comboBoxStart.Name = "comboBoxStart";
            comboBoxStart.Size = new Size(579, 35);
            comboBoxStart.TabIndex = 17;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label2.Location = new Point(0, 195);
            label2.Name = "label2";
            label2.Size = new Size(368, 20);
            label2.TabIndex = 16;
            label2.Text = "在上方输入要操作的服务的内部名↑↑↑   例：wuauserv";
            // 
            // comboBoxName
            // 
            comboBoxName.Dock = DockStyle.Top;
            comboBoxName.Font = new Font("微软雅黑", 12F);
            comboBoxName.FormattingEnabled = true;
            comboBoxName.Items.AddRange(new object[] { "cmd.exe", "calc.exe", "sethc.exe", "utilman.exe", "magnify.exe", "notepad.exe", "Taskmgr.exe" });
            comboBoxName.Location = new Point(0, 160);
            comboBoxName.Name = "comboBoxName";
            comboBoxName.Size = new Size(579, 35);
            comboBoxName.TabIndex = 15;
            comboBoxName.TextChanged += comboBoxName_TextChanged;
            // 
            // buttonAddItem
            // 
            buttonAddItem.Dock = DockStyle.Top;
            buttonAddItem.Enabled = false;
            buttonAddItem.Font = new Font("微软雅黑", 24F);
            buttonAddItem.Location = new Point(0, 80);
            buttonAddItem.Name = "buttonAddItem";
            buttonAddItem.Size = new Size(579, 80);
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
            buttonDeleteItem.Size = new Size(579, 80);
            buttonDeleteItem.TabIndex = 0;
            buttonDeleteItem.Text = "删除选择项";
            buttonDeleteItem.UseVisualStyleBackColor = true;
            buttonDeleteItem.Click += buttonDeleteItem_Click;
            // 
            // SystemServiceTools
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(874, 796);
            Controls.Add(splitContainer1);
            Name = "SystemServiceTools";
            Text = "系统服务工具";
            Load += SystemServiceTools_Load;
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
        private Button button_ReLoad;
        private Label label1;
        private ComboBox comboBoxStart;
        private Label label2;
        private ComboBox comboBoxName;
        private Button buttonAddItem;
        private Button buttonDeleteItem;
        private CheckBox checkBox_ShowAll;
    }
}