namespace QisToolkit3.Forms
{
    partial class UninstallRegistryKeysTool
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
            buttonMakeCode = new Button();
            labelHelpLink = new Label();
            comboBoxHelpLink = new ComboBox();
            labelPublisher = new Label();
            comboBoxPublisher = new ComboBox();
            label8 = new Label();
            comboBoxDisplayVersion = new ComboBox();
            label6 = new Label();
            comboBoxUninstallString = new ComboBox();
            label4 = new Label();
            comboBoxInstallLocation = new ComboBox();
            label3 = new Label();
            comboBoxDisplayIcon = new ComboBox();
            comboBoxType = new ComboBox();
            button_ReLoad = new Button();
            label1 = new Label();
            comboBoxDisplayName = new ComboBox();
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
            splitContainer1.Panel2.Controls.Add(buttonMakeCode);
            splitContainer1.Panel2.Controls.Add(labelHelpLink);
            splitContainer1.Panel2.Controls.Add(comboBoxHelpLink);
            splitContainer1.Panel2.Controls.Add(labelPublisher);
            splitContainer1.Panel2.Controls.Add(comboBoxPublisher);
            splitContainer1.Panel2.Controls.Add(label8);
            splitContainer1.Panel2.Controls.Add(comboBoxDisplayVersion);
            splitContainer1.Panel2.Controls.Add(label6);
            splitContainer1.Panel2.Controls.Add(comboBoxUninstallString);
            splitContainer1.Panel2.Controls.Add(label4);
            splitContainer1.Panel2.Controls.Add(comboBoxInstallLocation);
            splitContainer1.Panel2.Controls.Add(label3);
            splitContainer1.Panel2.Controls.Add(comboBoxDisplayIcon);
            splitContainer1.Panel2.Controls.Add(comboBoxType);
            splitContainer1.Panel2.Controls.Add(button_ReLoad);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Panel2.Controls.Add(comboBoxDisplayName);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Panel2.Controls.Add(comboBoxName);
            splitContainer1.Panel2.Controls.Add(buttonAddItem);
            splitContainer1.Panel2.Controls.Add(buttonDeleteItem);
            splitContainer1.Size = new Size(1369, 814);
            splitContainer1.SplitterDistance = 778;
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
            listBox.Size = new Size(778, 814);
            listBox.TabIndex = 0;
            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
            // 
            // buttonMakeCode
            // 
            buttonMakeCode.Dock = DockStyle.Bottom;
            buttonMakeCode.Font = new Font("微软雅黑", 12F);
            buttonMakeCode.Location = new Point(0, 660);
            buttonMakeCode.Name = "buttonMakeCode";
            buttonMakeCode.Size = new Size(587, 47);
            buttonMakeCode.TabIndex = 36;
            buttonMakeCode.Text = "生成代码（C#）";
            buttonMakeCode.UseVisualStyleBackColor = true;
            buttonMakeCode.Click += buttonMakeCode_Click;
            // 
            // labelHelpLink
            // 
            labelHelpLink.AutoSize = true;
            labelHelpLink.Dock = DockStyle.Top;
            labelHelpLink.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            labelHelpLink.Location = new Point(0, 580);
            labelHelpLink.Name = "labelHelpLink";
            labelHelpLink.Size = new Size(187, 20);
            labelHelpLink.TabIndex = 35;
            labelHelpLink.Text = "帮助链接（HelpLink）↑↑↑";
            // 
            // comboBoxHelpLink
            // 
            comboBoxHelpLink.Dock = DockStyle.Top;
            comboBoxHelpLink.Font = new Font("微软雅黑", 12F);
            comboBoxHelpLink.FormattingEnabled = true;
            comboBoxHelpLink.Location = new Point(0, 545);
            comboBoxHelpLink.Name = "comboBoxHelpLink";
            comboBoxHelpLink.Size = new Size(587, 35);
            comboBoxHelpLink.TabIndex = 34;
            // 
            // labelPublisher
            // 
            labelPublisher.AutoSize = true;
            labelPublisher.Dock = DockStyle.Top;
            labelPublisher.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            labelPublisher.Location = new Point(0, 525);
            labelPublisher.Name = "labelPublisher";
            labelPublisher.Size = new Size(175, 20);
            labelPublisher.TabIndex = 33;
            labelPublisher.Text = "发布者（Publisher）↑↑↑";
            // 
            // comboBoxPublisher
            // 
            comboBoxPublisher.Dock = DockStyle.Top;
            comboBoxPublisher.Font = new Font("微软雅黑", 12F);
            comboBoxPublisher.FormattingEnabled = true;
            comboBoxPublisher.Location = new Point(0, 490);
            comboBoxPublisher.Name = "comboBoxPublisher";
            comboBoxPublisher.Size = new Size(587, 35);
            comboBoxPublisher.TabIndex = 32;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Dock = DockStyle.Top;
            label8.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label8.Location = new Point(0, 470);
            label8.Name = "label8";
            label8.Size = new Size(230, 20);
            label8.TabIndex = 31;
            label8.Text = "版本信息（DisplayVersion）↑↑↑";
            // 
            // comboBoxDisplayVersion
            // 
            comboBoxDisplayVersion.Dock = DockStyle.Top;
            comboBoxDisplayVersion.Font = new Font("微软雅黑", 12F);
            comboBoxDisplayVersion.FormattingEnabled = true;
            comboBoxDisplayVersion.Location = new Point(0, 435);
            comboBoxDisplayVersion.Name = "comboBoxDisplayVersion";
            comboBoxDisplayVersion.Size = new Size(587, 35);
            comboBoxDisplayVersion.TabIndex = 30;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Top;
            label6.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label6.Location = new Point(0, 415);
            label6.Name = "label6";
            label6.Size = new Size(229, 20);
            label6.TabIndex = 27;
            label6.Text = "卸载路径（UninstallString）↑↑↑";
            // 
            // comboBoxUninstallString
            // 
            comboBoxUninstallString.Dock = DockStyle.Top;
            comboBoxUninstallString.Font = new Font("微软雅黑", 12F);
            comboBoxUninstallString.FormattingEnabled = true;
            comboBoxUninstallString.Location = new Point(0, 380);
            comboBoxUninstallString.Name = "comboBoxUninstallString";
            comboBoxUninstallString.Size = new Size(587, 35);
            comboBoxUninstallString.TabIndex = 26;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Top;
            label4.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label4.Location = new Point(0, 360);
            label4.Name = "label4";
            label4.Size = new Size(228, 20);
            label4.TabIndex = 25;
            label4.Text = "安装路径（InstallLocation）↑↑↑";
            // 
            // comboBoxInstallLocation
            // 
            comboBoxInstallLocation.Dock = DockStyle.Top;
            comboBoxInstallLocation.Font = new Font("微软雅黑", 12F);
            comboBoxInstallLocation.FormattingEnabled = true;
            comboBoxInstallLocation.Location = new Point(0, 325);
            comboBoxInstallLocation.Name = "comboBoxInstallLocation";
            comboBoxInstallLocation.Size = new Size(587, 35);
            comboBoxInstallLocation.TabIndex = 24;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Top;
            label3.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label3.Location = new Point(0, 305);
            label3.Name = "label3";
            label3.Size = new Size(206, 20);
            label3.TabIndex = 23;
            label3.Text = "软件图标（DisplayIcon）↑↑↑";
            // 
            // comboBoxDisplayIcon
            // 
            comboBoxDisplayIcon.Dock = DockStyle.Top;
            comboBoxDisplayIcon.Font = new Font("微软雅黑", 12F);
            comboBoxDisplayIcon.FormattingEnabled = true;
            comboBoxDisplayIcon.Location = new Point(0, 270);
            comboBoxDisplayIcon.Name = "comboBoxDisplayIcon";
            comboBoxDisplayIcon.Size = new Size(587, 35);
            comboBoxDisplayIcon.TabIndex = 22;
            // 
            // comboBoxType
            // 
            comboBoxType.Dock = DockStyle.Bottom;
            comboBoxType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxType.Font = new Font("Microsoft YaHei UI", 24F);
            comboBoxType.FormattingEnabled = true;
            comboBoxType.Items.AddRange(new object[] { "CurrentUser", "LocalMachine" });
            comboBoxType.Location = new Point(0, 707);
            comboBoxType.Name = "comboBoxType";
            comboBoxType.Size = new Size(587, 60);
            comboBoxType.TabIndex = 21;
            comboBoxType.SelectedIndexChanged += comboBoxType_SelectedIndexChanged;
            // 
            // button_ReLoad
            // 
            button_ReLoad.Dock = DockStyle.Bottom;
            button_ReLoad.Font = new Font("微软雅黑", 12F);
            button_ReLoad.Location = new Point(0, 767);
            button_ReLoad.Name = "button_ReLoad";
            button_ReLoad.Size = new Size(587, 47);
            button_ReLoad.TabIndex = 20;
            button_ReLoad.Text = "刷新";
            button_ReLoad.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label1.Location = new Point(0, 250);
            label1.Name = "label1";
            label1.Size = new Size(218, 20);
            label1.TabIndex = 18;
            label1.Text = "软件名称（DisplayName）↑↑↑";
            // 
            // comboBoxDisplayName
            // 
            comboBoxDisplayName.Dock = DockStyle.Top;
            comboBoxDisplayName.Font = new Font("微软雅黑", 12F);
            comboBoxDisplayName.FormattingEnabled = true;
            comboBoxDisplayName.Location = new Point(0, 215);
            comboBoxDisplayName.Name = "comboBoxDisplayName";
            comboBoxDisplayName.Size = new Size(587, 35);
            comboBoxDisplayName.TabIndex = 17;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label2.Location = new Point(0, 195);
            label2.Name = "label2";
            label2.Size = new Size(175, 20);
            label2.TabIndex = 16;
            label2.Text = "软件的注册名或GUID↑↑↑";
            // 
            // comboBoxName
            // 
            comboBoxName.Dock = DockStyle.Top;
            comboBoxName.Font = new Font("微软雅黑", 12F);
            comboBoxName.FormattingEnabled = true;
            comboBoxName.Items.AddRange(new object[] { "{088E3905-0323-4B02-9826-5D99428E115F}", "{0DB7E03F-FC29-4DC6-9020-FF41B59E513A}", "{24AD3AD4-A569-4530-98E1-AB02F9417AA8}", "{3DFDF296-DBEC-4FB4-81D1-6A3438BCF4DE}", "{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", "{D3162B92-9365-467A-956B-92703ACA08AF}", "{F86FA3AB-70D2-4FC7-9C99-FCBF05467F3A}" });
            comboBoxName.Location = new Point(0, 160);
            comboBoxName.Name = "comboBoxName";
            comboBoxName.Size = new Size(587, 35);
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
            buttonAddItem.Size = new Size(587, 80);
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
            buttonDeleteItem.Size = new Size(587, 80);
            buttonDeleteItem.TabIndex = 0;
            buttonDeleteItem.Text = "删除选择项";
            buttonDeleteItem.UseVisualStyleBackColor = true;
            buttonDeleteItem.Click += buttonDeleteItem_Click;
            // 
            // UninstallRegistryKeysTool
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1369, 814);
            Controls.Add(splitContainer1);
            Name = "UninstallRegistryKeysTool";
            Text = "UninstallRegistryKeysTool";
            Load += UninstallRegistryKeysTool_Load;
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
        private ComboBox comboBoxDisplayName;
        private Label label2;
        private ComboBox comboBoxName;
        private Button buttonAddItem;
        private Button buttonDeleteItem;
        private Label label4;
        private ComboBox comboBoxInstallLocation;
        private Label label3;
        private ComboBox comboBoxDisplayIcon;
        private Label labelPublisher;
        private ComboBox comboBoxPublisher;
        private Label label8;
        private ComboBox comboBoxDisplayVersion;
        private Label label6;
        private ComboBox comboBoxUninstallString;
        private Label labelHelpLink;
        private ComboBox comboBoxHelpLink;
        private Button buttonMakeCode;
    }
}