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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemServiceTools));
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
            resources.ApplyResources(splitContainer1, "splitContainer1");
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
            // 
            // listBox
            // 
            resources.ApplyResources(listBox, "listBox");
            listBox.FormattingEnabled = true;
            listBox.Name = "listBox";
            listBox.SelectionMode = SelectionMode.MultiExtended;
            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
            // 
            // checkBox_ShowAll
            // 
            resources.ApplyResources(checkBox_ShowAll, "checkBox_ShowAll");
            checkBox_ShowAll.Name = "checkBox_ShowAll";
            checkBox_ShowAll.UseVisualStyleBackColor = true;
            checkBox_ShowAll.CheckedChanged += checkBox_ShowAll_CheckedChanged;
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
            // comboBoxStart
            // 
            resources.ApplyResources(comboBoxStart, "comboBoxStart");
            comboBoxStart.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxStart.FormattingEnabled = true;
            comboBoxStart.Items.AddRange(new object[] { resources.GetString("comboBoxStart.Items"), resources.GetString("comboBoxStart.Items1"), resources.GetString("comboBoxStart.Items2"), resources.GetString("comboBoxStart.Items3"), resources.GetString("comboBoxStart.Items4") });
            comboBoxStart.Name = "comboBoxStart";
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
            comboBoxName.TextChanged += comboBoxName_TextChanged;
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
            // SystemServiceTools
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Name = "SystemServiceTools";
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