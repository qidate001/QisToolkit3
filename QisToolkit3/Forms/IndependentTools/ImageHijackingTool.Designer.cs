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
            // 
            // listBox
            // 
            resources.ApplyResources(listBox, "listBox");
            listBox.FormattingEnabled = true;
            listBox.Name = "listBox";
            listBox.SelectionMode = SelectionMode.MultiExtended;
            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
            // 
            // checkBox_DisableUserModeCallbackFilter
            // 
            resources.ApplyResources(checkBox_DisableUserModeCallbackFilter, "checkBox_DisableUserModeCallbackFilter");
            checkBox_DisableUserModeCallbackFilter.Name = "checkBox_DisableUserModeCallbackFilter";
            checkBox_DisableUserModeCallbackFilter.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowAll
            // 
            resources.ApplyResources(checkBox_ShowAll, "checkBox_ShowAll");
            checkBox_ShowAll.Name = "checkBox_ShowAll";
            checkBox_ShowAll.UseVisualStyleBackColor = true;
            checkBox_ShowAll.CheckedChanged += checkBox_ShowAll_CheckedChanged;
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // comboBoxDataMO
            // 
            resources.ApplyResources(comboBoxDataMO, "comboBoxDataMO");
            comboBoxDataMO.FormattingEnabled = true;
            comboBoxDataMO.Items.AddRange(new object[] { resources.GetString("comboBoxDataMO.Items"), resources.GetString("comboBoxDataMO.Items1"), resources.GetString("comboBoxDataMO.Items2"), resources.GetString("comboBoxDataMO.Items3"), resources.GetString("comboBoxDataMO.Items4") });
            comboBoxDataMO.Name = "comboBoxDataMO";
            // 
            // button_ReLoad
            // 
            resources.ApplyResources(button_ReLoad, "button_ReLoad");
            button_ReLoad.Name = "button_ReLoad";
            button_ReLoad.UseVisualStyleBackColor = true;
            button_ReLoad.Click += button_ReLoad_Click;
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
            comboBoxData.Items.AddRange(new object[] { resources.GetString("comboBoxData.Items"), resources.GetString("comboBoxData.Items1"), resources.GetString("comboBoxData.Items2"), resources.GetString("comboBoxData.Items3"), resources.GetString("comboBoxData.Items4"), resources.GetString("comboBoxData.Items5"), resources.GetString("comboBoxData.Items6"), resources.GetString("comboBoxData.Items7") });
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
            comboBoxName.SelectedIndexChanged += comboBoxName_SelectedIndexChanged;
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
            // ImageHijackingTool
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Name = "ImageHijackingTool";
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