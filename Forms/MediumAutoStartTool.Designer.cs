namespace QisToolkit3.Forms
{
    partial class MediumAutoStartTool
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
            comboBoxType = new ComboBox();
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
            splitContainer1.Panel2.Controls.Add(comboBoxType);
            splitContainer1.Panel2.Controls.Add(button_ReLoad);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Panel2.Controls.Add(comboBoxData);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Panel2.Controls.Add(comboBoxName);
            splitContainer1.Panel2.Controls.Add(buttonAddItem);
            splitContainer1.Panel2.Controls.Add(buttonDeleteItem);
            splitContainer1.Size = new Size(1088, 808);
            splitContainer1.SplitterDistance = 502;
            splitContainer1.TabIndex = 2;
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
            listBox.Size = new Size(502, 808);
            listBox.TabIndex = 0;
            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
            // 
            // comboBoxType
            // 
            comboBoxType.Dock = DockStyle.Bottom;
            comboBoxType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxType.Font = new Font("Microsoft YaHei UI", 24F);
            comboBoxType.FormattingEnabled = true;
            comboBoxType.Items.AddRange(new object[] { "CurrentUser", "LocalMachine" });
            comboBoxType.Location = new Point(0, 701);
            comboBoxType.Name = "comboBoxType";
            comboBoxType.Size = new Size(582, 60);
            comboBoxType.TabIndex = 21;
            comboBoxType.SelectedIndexChanged += comboBoxType_SelectedIndexChanged;
            // 
            // button_ReLoad
            // 
            button_ReLoad.Dock = DockStyle.Bottom;
            button_ReLoad.Font = new Font("微软雅黑", 12F);
            button_ReLoad.Location = new Point(0, 761);
            button_ReLoad.Name = "button_ReLoad";
            button_ReLoad.Size = new Size(582, 47);
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
            label1.Size = new Size(317, 20);
            label1.TabIndex = 18;
            label1.Text = "在上方输入要自启动的程序↑↑↑  例：CMD.exe";
            // 
            // comboBoxData
            // 
            comboBoxData.Dock = DockStyle.Top;
            comboBoxData.Font = new Font("微软雅黑", 12F);
            comboBoxData.FormattingEnabled = true;
            comboBoxData.Location = new Point(0, 215);
            comboBoxData.Name = "comboBoxData";
            comboBoxData.Size = new Size(582, 35);
            comboBoxData.TabIndex = 17;
            comboBoxData.SelectedIndexChanged += comboBoxData_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label2.Location = new Point(0, 195);
            label2.Name = "label2";
            label2.Size = new Size(205, 20);
            label2.TabIndex = 16;
            label2.Text = "在上方输入注册名或GUID↑↑↑";
            // 
            // comboBoxName
            // 
            comboBoxName.Dock = DockStyle.Top;
            comboBoxName.Font = new Font("微软雅黑", 12F);
            comboBoxName.FormattingEnabled = true;
            comboBoxName.Items.AddRange(new object[] { "{088E3905-0323-4B02-9826-5D99428E115F}", "{0DB7E03F-FC29-4DC6-9020-FF41B59E513A}", "{24AD3AD4-A569-4530-98E1-AB02F9417AA8}", "{3DFDF296-DBEC-4FB4-81D1-6A3438BCF4DE}", "{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", "{D3162B92-9365-467A-956B-92703ACA08AF}", "{F86FA3AB-70D2-4FC7-9C99-FCBF05467F3A}" });
            comboBoxName.Location = new Point(0, 160);
            comboBoxName.Name = "comboBoxName";
            comboBoxName.Size = new Size(582, 35);
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
            buttonAddItem.Size = new Size(582, 80);
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
            buttonDeleteItem.Size = new Size(582, 80);
            buttonDeleteItem.TabIndex = 0;
            buttonDeleteItem.Text = "删除选择项";
            buttonDeleteItem.UseVisualStyleBackColor = true;
            buttonDeleteItem.Click += buttonDeleteItem_Click;
            // 
            // MediumAutoStartTool
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1088, 808);
            Controls.Add(splitContainer1);
            Name = "MediumAutoStartTool";
            Text = "MediumAutoStartTool";
            Load += MediumAutoStartTool_Load;
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
        private ComboBox comboBoxType;
        private Button button_ReLoad;
        private Label label1;
        private ComboBox comboBoxData;
        private Label label2;
        private ComboBox comboBoxName;
        private Button buttonAddItem;
        private Button buttonDeleteItem;
    }
}