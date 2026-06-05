namespace QisToolkit3.Forms
{
    partial class ExtendedFeatures
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtendedFeatures));
            button_qicmd_download = new Button();
            label1 = new Label();
            button_qicmd_delete = new Button();
            comboBox_QiCmd = new ComboBox();
            label2 = new Label();
            comboBox1 = new ComboBox();
            button_mas_delete = new Button();
            button_mas_download = new Button();
            SuspendLayout();
            // 
            // button_qicmd_download
            // 
            resources.ApplyResources(button_qicmd_download, "button_qicmd_download");
            button_qicmd_download.Name = "button_qicmd_download";
            button_qicmd_download.UseVisualStyleBackColor = true;
            button_qicmd_download.Click += button_qicmd_download_Click;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // button_qicmd_delete
            // 
            resources.ApplyResources(button_qicmd_delete, "button_qicmd_delete");
            button_qicmd_delete.Name = "button_qicmd_delete";
            button_qicmd_delete.UseVisualStyleBackColor = true;
            button_qicmd_delete.Click += button_qicmd_delete_Click;
            // 
            // comboBox_QiCmd
            // 
            resources.ApplyResources(comboBox_QiCmd, "comboBox_QiCmd");
            comboBox_QiCmd.FormattingEnabled = true;
            comboBox_QiCmd.Items.AddRange(new object[] { resources.GetString("comboBox_QiCmd.Items"), resources.GetString("comboBox_QiCmd.Items1"), resources.GetString("comboBox_QiCmd.Items2"), resources.GetString("comboBox_QiCmd.Items3") });
            comboBox_QiCmd.Name = "comboBox_QiCmd";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // comboBox1
            // 
            resources.ApplyResources(comboBox1, "comboBox1");
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { resources.GetString("comboBox1.Items"), resources.GetString("comboBox1.Items1"), resources.GetString("comboBox1.Items2"), resources.GetString("comboBox1.Items3") });
            comboBox1.Name = "comboBox1";
            // 
            // button_mas_delete
            // 
            resources.ApplyResources(button_mas_delete, "button_mas_delete");
            button_mas_delete.Name = "button_mas_delete";
            button_mas_delete.UseVisualStyleBackColor = true;
            button_mas_delete.Click += button_mas_delete_Click;
            // 
            // button_mas_download
            // 
            resources.ApplyResources(button_mas_download, "button_mas_download");
            button_mas_download.Name = "button_mas_download";
            button_mas_download.UseVisualStyleBackColor = true;
            button_mas_download.Click += button_mas_download_Click;
            // 
            // ExtendedFeatures
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(comboBox1);
            Controls.Add(button_mas_delete);
            Controls.Add(button_mas_download);
            Controls.Add(label2);
            Controls.Add(comboBox_QiCmd);
            Controls.Add(button_qicmd_delete);
            Controls.Add(label1);
            Controls.Add(button_qicmd_download);
            Name = "ExtendedFeatures";
            Load += ExtendedFeatures_Load;
            ResumeLayout(false);
        }

        #endregion
        private Label label1;
        private Button button_qicmd_download;
        private Button button_qicmd_delete;
        private ComboBox comboBox_QiCmd;
        private Label label2;
        private ComboBox comboBox1;
        private Button button_mas_delete;
        private Button button_mas_download;
    }
}