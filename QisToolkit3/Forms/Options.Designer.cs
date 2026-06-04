namespace QisToolkit3.Forms
{
    partial class Options
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            tabControlOptions = new TabControl();
            tabPageHead = new TabPage();
            checkBox_MinSudo = new CheckBox();
            button_CommandMode = new Button();
            button_SystemCommandMode = new Button();
            comboBoxLanguage = new ComboBox();
            label4 = new Label();
            button_ExtendedFeatures = new Button();
            checkBox_DarkMode = new CheckBox();
            checkBox_ComputerNoviceMode = new CheckBox();
            buttonOut = new Button();
            buttonIn = new Button();
            button_System = new Button();
            tabPageFilesOperation = new TabPage();
            checkBoxFilesOperation_AutomaticallyPopUpTheOpenFileWindow = new CheckBox();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            tabPageToolsOperation = new TabPage();
            checkBoxToolsProcessingTools_TopMost = new CheckBox();
            checkBoxToolsOperation_UseChineseName = new CheckBox();
            tabPage4 = new TabPage();
            tabPage5 = new TabPage();
            tabPage3 = new TabPage();
            label7 = new Label();
            label6 = new Label();
            button_Gmail = new Button();
            button_QQEmail = new Button();
            label5 = new Label();
            button_DownloadForGitHub = new Button();
            label3 = new Label();
            button_GitHub = new Button();
            label2 = new Label();
            label1 = new Label();
            button_DownloadForQQ = new Button();
            button_DownloadForLan = new Button();
            button_DownloadForBaidu = new Button();
            tabControlOptions.SuspendLayout();
            tabPageHead.SuspendLayout();
            tabPageFilesOperation.SuspendLayout();
            tabPageToolsOperation.SuspendLayout();
            tabPage3.SuspendLayout();
            SuspendLayout();
            // 
            // tabControlOptions
            // 
            resources.ApplyResources(tabControlOptions, "tabControlOptions");
            tabControlOptions.Controls.Add(tabPageHead);
            tabControlOptions.Controls.Add(tabPageFilesOperation);
            tabControlOptions.Controls.Add(tabPage1);
            tabControlOptions.Controls.Add(tabPage2);
            tabControlOptions.Controls.Add(tabPageToolsOperation);
            tabControlOptions.Controls.Add(tabPage4);
            tabControlOptions.Controls.Add(tabPage5);
            tabControlOptions.Controls.Add(tabPage3);
            tabControlOptions.Name = "tabControlOptions";
            tabControlOptions.SelectedIndex = 0;
            // 
            // tabPageHead
            // 
            resources.ApplyResources(tabPageHead, "tabPageHead");
            tabPageHead.Controls.Add(checkBox_MinSudo);
            tabPageHead.Controls.Add(button_CommandMode);
            tabPageHead.Controls.Add(button_SystemCommandMode);
            tabPageHead.Controls.Add(comboBoxLanguage);
            tabPageHead.Controls.Add(label4);
            tabPageHead.Controls.Add(button_ExtendedFeatures);
            tabPageHead.Controls.Add(checkBox_DarkMode);
            tabPageHead.Controls.Add(checkBox_ComputerNoviceMode);
            tabPageHead.Controls.Add(buttonOut);
            tabPageHead.Controls.Add(buttonIn);
            tabPageHead.Controls.Add(button_System);
            tabPageHead.Name = "tabPageHead";
            tabPageHead.UseVisualStyleBackColor = true;
            // 
            // checkBox_MinSudo
            // 
            resources.ApplyResources(checkBox_MinSudo, "checkBox_MinSudo");
            checkBox_MinSudo.Name = "checkBox_MinSudo";
            checkBox_MinSudo.UseVisualStyleBackColor = true;
            checkBox_MinSudo.CheckedChanged += checkBox_MinSudo_CheckedChanged;
            // 
            // button_CommandMode
            // 
            resources.ApplyResources(button_CommandMode, "button_CommandMode");
            button_CommandMode.FlatAppearance.BorderSize = 0;
            button_CommandMode.Name = "button_CommandMode";
            button_CommandMode.UseVisualStyleBackColor = true;
            button_CommandMode.Click += button_CommandMode_Click;
            // 
            // button_SystemCommandMode
            // 
            resources.ApplyResources(button_SystemCommandMode, "button_SystemCommandMode");
            button_SystemCommandMode.FlatAppearance.BorderSize = 0;
            button_SystemCommandMode.Name = "button_SystemCommandMode";
            button_SystemCommandMode.UseVisualStyleBackColor = true;
            button_SystemCommandMode.Click += button_SystemCommandMode_Click;
            // 
            // comboBoxLanguage
            // 
            resources.ApplyResources(comboBoxLanguage, "comboBoxLanguage");
            comboBoxLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxLanguage.FormattingEnabled = true;
            comboBoxLanguage.Items.AddRange(new object[] { resources.GetString("comboBoxLanguage.Items"), resources.GetString("comboBoxLanguage.Items1") });
            comboBoxLanguage.Name = "comboBoxLanguage";
            comboBoxLanguage.SelectedIndexChanged += comboBoxLanguage_SelectedIndexChanged;
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // button_ExtendedFeatures
            // 
            resources.ApplyResources(button_ExtendedFeatures, "button_ExtendedFeatures");
            button_ExtendedFeatures.FlatAppearance.BorderSize = 0;
            button_ExtendedFeatures.Name = "button_ExtendedFeatures";
            button_ExtendedFeatures.UseVisualStyleBackColor = true;
            button_ExtendedFeatures.Click += button_ExtendedFeatures_Click;
            // 
            // checkBox_DarkMode
            // 
            resources.ApplyResources(checkBox_DarkMode, "checkBox_DarkMode");
            checkBox_DarkMode.Name = "checkBox_DarkMode";
            checkBox_DarkMode.UseVisualStyleBackColor = true;
            checkBox_DarkMode.CheckedChanged += checkBox_DarkMode_CheckedChanged;
            // 
            // checkBox_ComputerNoviceMode
            // 
            resources.ApplyResources(checkBox_ComputerNoviceMode, "checkBox_ComputerNoviceMode");
            checkBox_ComputerNoviceMode.Name = "checkBox_ComputerNoviceMode";
            checkBox_ComputerNoviceMode.UseVisualStyleBackColor = true;
            checkBox_ComputerNoviceMode.CheckedChanged += checkBox_ComputerNoviceMode_CheckedChanged;
            // 
            // buttonOut
            // 
            resources.ApplyResources(buttonOut, "buttonOut");
            buttonOut.FlatAppearance.BorderColor = Color.Silver;
            buttonOut.Name = "buttonOut";
            buttonOut.UseVisualStyleBackColor = true;
            buttonOut.Click += buttonOff_Click;
            // 
            // buttonIn
            // 
            resources.ApplyResources(buttonIn, "buttonIn");
            buttonIn.FlatAppearance.BorderColor = Color.Silver;
            buttonIn.Name = "buttonIn";
            buttonIn.UseVisualStyleBackColor = true;
            buttonIn.Click += buttonOn_Click;
            // 
            // button_System
            // 
            resources.ApplyResources(button_System, "button_System");
            button_System.FlatAppearance.BorderSize = 0;
            button_System.Name = "button_System";
            button_System.UseVisualStyleBackColor = true;
            button_System.Click += button_System_Click;
            // 
            // tabPageFilesOperation
            // 
            resources.ApplyResources(tabPageFilesOperation, "tabPageFilesOperation");
            tabPageFilesOperation.Controls.Add(checkBoxFilesOperation_AutomaticallyPopUpTheOpenFileWindow);
            tabPageFilesOperation.Name = "tabPageFilesOperation";
            tabPageFilesOperation.UseVisualStyleBackColor = true;
            // 
            // checkBoxFilesOperation_AutomaticallyPopUpTheOpenFileWindow
            // 
            resources.ApplyResources(checkBoxFilesOperation_AutomaticallyPopUpTheOpenFileWindow, "checkBoxFilesOperation_AutomaticallyPopUpTheOpenFileWindow");
            checkBoxFilesOperation_AutomaticallyPopUpTheOpenFileWindow.ForeColor = Color.Black;
            checkBoxFilesOperation_AutomaticallyPopUpTheOpenFileWindow.Name = "checkBoxFilesOperation_AutomaticallyPopUpTheOpenFileWindow";
            checkBoxFilesOperation_AutomaticallyPopUpTheOpenFileWindow.UseVisualStyleBackColor = true;
            checkBoxFilesOperation_AutomaticallyPopUpTheOpenFileWindow.CheckedChanged += checkBoxFilesOperation_AutomaticallyPopUpTheOpenFileWindow_CheckedChanged;
            // 
            // tabPage1
            // 
            resources.ApplyResources(tabPage1, "tabPage1");
            tabPage1.Name = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            resources.ApplyResources(tabPage2, "tabPage2");
            tabPage2.Name = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPageToolsOperation
            // 
            resources.ApplyResources(tabPageToolsOperation, "tabPageToolsOperation");
            tabPageToolsOperation.Controls.Add(checkBoxToolsProcessingTools_TopMost);
            tabPageToolsOperation.Controls.Add(checkBoxToolsOperation_UseChineseName);
            tabPageToolsOperation.Name = "tabPageToolsOperation";
            tabPageToolsOperation.UseVisualStyleBackColor = true;
            // 
            // checkBoxToolsProcessingTools_TopMost
            // 
            resources.ApplyResources(checkBoxToolsProcessingTools_TopMost, "checkBoxToolsProcessingTools_TopMost");
            checkBoxToolsProcessingTools_TopMost.Checked = true;
            checkBoxToolsProcessingTools_TopMost.CheckState = CheckState.Checked;
            checkBoxToolsProcessingTools_TopMost.ForeColor = Color.Black;
            checkBoxToolsProcessingTools_TopMost.Name = "checkBoxToolsProcessingTools_TopMost";
            checkBoxToolsProcessingTools_TopMost.UseVisualStyleBackColor = true;
            checkBoxToolsProcessingTools_TopMost.CheckedChanged += checkBoxToolsProcessingTools_TopMost_CheckedChanged;
            // 
            // checkBoxToolsOperation_UseChineseName
            // 
            resources.ApplyResources(checkBoxToolsOperation_UseChineseName, "checkBoxToolsOperation_UseChineseName");
            checkBoxToolsOperation_UseChineseName.ForeColor = Color.Black;
            checkBoxToolsOperation_UseChineseName.Name = "checkBoxToolsOperation_UseChineseName";
            checkBoxToolsOperation_UseChineseName.UseVisualStyleBackColor = true;
            checkBoxToolsOperation_UseChineseName.CheckedChanged += checkBoxToolsOperation_UseChineseName_CheckedChanged;
            // 
            // tabPage4
            // 
            resources.ApplyResources(tabPage4, "tabPage4");
            tabPage4.Name = "tabPage4";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            resources.ApplyResources(tabPage5, "tabPage5");
            tabPage5.Name = "tabPage5";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            resources.ApplyResources(tabPage3, "tabPage3");
            tabPage3.Controls.Add(label7);
            tabPage3.Controls.Add(label6);
            tabPage3.Controls.Add(button_Gmail);
            tabPage3.Controls.Add(button_QQEmail);
            tabPage3.Controls.Add(label5);
            tabPage3.Controls.Add(button_DownloadForGitHub);
            tabPage3.Controls.Add(label3);
            tabPage3.Controls.Add(button_GitHub);
            tabPage3.Controls.Add(label2);
            tabPage3.Controls.Add(label1);
            tabPage3.Controls.Add(button_DownloadForQQ);
            tabPage3.Controls.Add(button_DownloadForLan);
            tabPage3.Controls.Add(button_DownloadForBaidu);
            tabPage3.Name = "tabPage3";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            resources.ApplyResources(label7, "label7");
            label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // button_Gmail
            // 
            resources.ApplyResources(button_Gmail, "button_Gmail");
            button_Gmail.FlatAppearance.BorderSize = 0;
            button_Gmail.Name = "button_Gmail";
            button_Gmail.UseVisualStyleBackColor = true;
            button_Gmail.Click += button_Gmail_Click;
            // 
            // button_QQEmail
            // 
            resources.ApplyResources(button_QQEmail, "button_QQEmail");
            button_QQEmail.FlatAppearance.BorderSize = 0;
            button_QQEmail.Name = "button_QQEmail";
            button_QQEmail.UseVisualStyleBackColor = true;
            button_QQEmail.Click += button_QQEmail_Click;
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.BackColor = Color.Gainsboro;
            label5.Name = "label5";
            // 
            // button_DownloadForGitHub
            // 
            resources.ApplyResources(button_DownloadForGitHub, "button_DownloadForGitHub");
            button_DownloadForGitHub.FlatAppearance.BorderSize = 0;
            button_DownloadForGitHub.Name = "button_DownloadForGitHub";
            button_DownloadForGitHub.UseVisualStyleBackColor = true;
            button_DownloadForGitHub.Click += button_DownloadForGitHub_Click;
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.BackColor = Color.Gainsboro;
            label3.Name = "label3";
            // 
            // button_GitHub
            // 
            resources.ApplyResources(button_GitHub, "button_GitHub");
            button_GitHub.FlatAppearance.BorderSize = 0;
            button_GitHub.Name = "button_GitHub";
            button_GitHub.UseVisualStyleBackColor = true;
            button_GitHub.Click += button_GitHub_Click;
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // button_DownloadForQQ
            // 
            resources.ApplyResources(button_DownloadForQQ, "button_DownloadForQQ");
            button_DownloadForQQ.FlatAppearance.BorderSize = 0;
            button_DownloadForQQ.Name = "button_DownloadForQQ";
            button_DownloadForQQ.UseVisualStyleBackColor = true;
            button_DownloadForQQ.Click += button_DownloadForQQ_Click;
            // 
            // button_DownloadForLan
            // 
            resources.ApplyResources(button_DownloadForLan, "button_DownloadForLan");
            button_DownloadForLan.FlatAppearance.BorderSize = 0;
            button_DownloadForLan.Name = "button_DownloadForLan";
            button_DownloadForLan.UseVisualStyleBackColor = true;
            button_DownloadForLan.Click += button_DownloadForLan_Click;
            // 
            // button_DownloadForBaidu
            // 
            resources.ApplyResources(button_DownloadForBaidu, "button_DownloadForBaidu");
            button_DownloadForBaidu.FlatAppearance.BorderSize = 0;
            button_DownloadForBaidu.Name = "button_DownloadForBaidu";
            button_DownloadForBaidu.UseVisualStyleBackColor = true;
            button_DownloadForBaidu.Click += button_DownloadForBaidu_Click;
            // 
            // Options
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabControlOptions);
            Name = "Options";
            Load += Options_Load;
            tabControlOptions.ResumeLayout(false);
            tabPageHead.ResumeLayout(false);
            tabPageHead.PerformLayout();
            tabPageFilesOperation.ResumeLayout(false);
            tabPageFilesOperation.PerformLayout();
            tabPageToolsOperation.ResumeLayout(false);
            tabPageToolsOperation.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControlOptions;
        private TabPage tabPageFilesOperation;
        private CheckBox checkBoxFilesOperation_AutomaticallyPopUpTheOpenFileWindow;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPageToolsOperation;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private CheckBox checkBoxToolsOperation_UseChineseName;
        private CheckBox checkBoxToolsProcessingTools_TopMost;
        private TabPage tabPageHead;
        private Button button_System;
        private Button buttonIn;
        private Button buttonOut;
        private CheckBox checkBox_ComputerNoviceMode;
        private TabPage tabPage3;
        private Button button_ExtendedFeatures;
        private ComboBox comboBoxLanguage;
        private Label label4;
        private Button button_CommandMode;
        private Button button_SystemCommandMode;
        private CheckBox checkBox_MinSudo;
        private CheckBox checkBox_DarkMode;
        private Button button_GitHub;
        private Label label2;
        private Label label1;
        private Button button_DownloadForQQ;
        private Button button_DownloadForLan;
        private Button button_DownloadForBaidu;
        private Button button_DownloadForGitHub;
        private Label label3;
        private Button button_QQEmail;
        private Label label5;
        private Button button_Gmail;
        private Label label7;
        private Label label6;
    }
}