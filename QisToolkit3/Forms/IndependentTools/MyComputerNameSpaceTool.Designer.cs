namespace QisToolkit3.Forms
{
    partial class MyComputerNameSpaceTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyComputerNameSpaceTool));
            splitContainer1 = new SplitContainer();
            listBox = new ListBox();
            button_TryRepairFolder = new Button();
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
            resources.ApplyResources(splitContainer1, "splitContainer1");
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(splitContainer1.Panel1, "splitContainer1.Panel1");
            splitContainer1.Panel1.Controls.Add(listBox);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(splitContainer1.Panel2, "splitContainer1.Panel2");
            splitContainer1.Panel2.Controls.Add(button_TryRepairFolder);
            splitContainer1.Panel2.Controls.Add(comboBoxType);
            splitContainer1.Panel2.Controls.Add(button_ReLoad);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Panel2.Controls.Add(comboBoxData);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Panel2.Controls.Add(comboBoxName);
            splitContainer1.Panel2.Controls.Add(buttonAddItem);
            splitContainer1.Panel2.Controls.Add(buttonDeleteItem);
            // 
            // listBox
            // 
            resources.ApplyResources(listBox, "listBox");
            listBox.FormattingEnabled = true;
            listBox.Name = "listBox";
            listBox.SelectionMode = SelectionMode.MultiExtended;
            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
            // 
            // button_TryRepairFolder
            // 
            resources.ApplyResources(button_TryRepairFolder, "button_TryRepairFolder");
            button_TryRepairFolder.Name = "button_TryRepairFolder";
            button_TryRepairFolder.UseVisualStyleBackColor = true;
            button_TryRepairFolder.Click += button_TryRepairFolder_Click;
            // 
            // comboBoxType
            // 
            resources.ApplyResources(comboBoxType, "comboBoxType");
            comboBoxType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxType.FormattingEnabled = true;
            comboBoxType.Items.AddRange(new object[] { resources.GetString("comboBoxType.Items"), resources.GetString("comboBoxType.Items1") });
            comboBoxType.Name = "comboBoxType";
            comboBoxType.SelectedIndexChanged += comboBoxType_SelectedIndexChanged;
            // 
            // button_ReLoad
            // 
            resources.ApplyResources(button_ReLoad, "button_ReLoad");
            button_ReLoad.Name = "button_ReLoad";
            button_ReLoad.UseVisualStyleBackColor = true;
            button_ReLoad.Click += MyComputerNameSpaceTool_Load;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // comboBoxData
            // 
            resources.ApplyResources(comboBoxData, "comboBoxData");
            comboBoxData.FormattingEnabled = true;
            comboBoxData.Name = "comboBoxData";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // comboBoxName
            // 
            resources.ApplyResources(comboBoxName, "comboBoxName");
            comboBoxName.FormattingEnabled = true;
            comboBoxName.Items.AddRange(new object[] { resources.GetString("comboBoxName.Items"), resources.GetString("comboBoxName.Items1"), resources.GetString("comboBoxName.Items2"), resources.GetString("comboBoxName.Items3"), resources.GetString("comboBoxName.Items4"), resources.GetString("comboBoxName.Items5"), resources.GetString("comboBoxName.Items6") });
            comboBoxName.Name = "comboBoxName";
            comboBoxName.SelectedIndexChanged += comboBoxType_SelectedIndexChanged;
            comboBoxName.TextChanged += comboBoxName_SelectedIndexChanged;
            // 
            // buttonAddItem
            // 
            resources.ApplyResources(buttonAddItem, "buttonAddItem");
            buttonAddItem.Name = "buttonAddItem";
            buttonAddItem.UseVisualStyleBackColor = true;
            buttonAddItem.Click += buttonAddItem_Click;
            // 
            // buttonDeleteItem
            // 
            resources.ApplyResources(buttonDeleteItem, "buttonDeleteItem");
            buttonDeleteItem.Name = "buttonDeleteItem";
            buttonDeleteItem.UseVisualStyleBackColor = true;
            buttonDeleteItem.Click += buttonDeleteItem_Click;
            // 
            // MyComputerNameSpaceTool
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Name = "MyComputerNameSpaceTool";
            Load += MyComputerNameSpaceTool_Load;
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
        private ComboBox comboBoxData;
        private Label label2;
        private ComboBox comboBoxName;
        private Button buttonAddItem;
        private Button buttonDeleteItem;
        private ComboBox comboBoxType;
        private Button button_TryRepairFolder;
    }
}