namespace QisToolkit3
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            panelMenu = new Panel();
            buttonOptions = new Button();
            buttonStrangeQuestionAndAnswer = new Button();
            buttonGameTools = new Button();
            buttonTools = new Button();
            buttonCleaningUpTrash = new Button();
            buttonTextGeneration = new Button();
            buttonDoFile = new Button();
            panelLogo = new Panel();
            label = new Label();
            panelTitleBar = new Panel();
            labelTitle = new Label();
            panelDesktopPane = new Panel();
            panelMenu.SuspendLayout();
            panelLogo.SuspendLayout();
            panelTitleBar.SuspendLayout();
            SuspendLayout();
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.FromArgb(51, 51, 76);
            panelMenu.Controls.Add(buttonOptions);
            panelMenu.Controls.Add(buttonStrangeQuestionAndAnswer);
            panelMenu.Controls.Add(buttonGameTools);
            panelMenu.Controls.Add(buttonTools);
            panelMenu.Controls.Add(buttonCleaningUpTrash);
            panelMenu.Controls.Add(buttonTextGeneration);
            panelMenu.Controls.Add(buttonDoFile);
            panelMenu.Controls.Add(panelLogo);
            resources.ApplyResources(panelMenu, "panelMenu");
            panelMenu.Name = "panelMenu";
            // 
            // buttonOptions
            // 
            resources.ApplyResources(buttonOptions, "buttonOptions");
            buttonOptions.FlatAppearance.BorderSize = 0;
            buttonOptions.Name = "buttonOptions";
            buttonOptions.UseVisualStyleBackColor = true;
            buttonOptions.Click += buttonOptions_Click;
            // 
            // buttonStrangeQuestionAndAnswer
            // 
            resources.ApplyResources(buttonStrangeQuestionAndAnswer, "buttonStrangeQuestionAndAnswer");
            buttonStrangeQuestionAndAnswer.FlatAppearance.BorderSize = 0;
            buttonStrangeQuestionAndAnswer.Name = "buttonStrangeQuestionAndAnswer";
            buttonStrangeQuestionAndAnswer.UseVisualStyleBackColor = true;
            buttonStrangeQuestionAndAnswer.Click += buttonStrangeQuestionAndAnswer_Click;
            // 
            // buttonGameTools
            // 
            resources.ApplyResources(buttonGameTools, "buttonGameTools");
            buttonGameTools.FlatAppearance.BorderSize = 0;
            buttonGameTools.Name = "buttonGameTools";
            buttonGameTools.UseVisualStyleBackColor = true;
            buttonGameTools.Click += buttonMinecraft_Click;
            // 
            // buttonTools
            // 
            resources.ApplyResources(buttonTools, "buttonTools");
            buttonTools.FlatAppearance.BorderSize = 0;
            buttonTools.Name = "buttonTools";
            buttonTools.UseVisualStyleBackColor = true;
            buttonTools.Click += buttonTools_Click;
            // 
            // buttonCleaningUpTrash
            // 
            resources.ApplyResources(buttonCleaningUpTrash, "buttonCleaningUpTrash");
            buttonCleaningUpTrash.FlatAppearance.BorderSize = 0;
            buttonCleaningUpTrash.Name = "buttonCleaningUpTrash";
            buttonCleaningUpTrash.UseVisualStyleBackColor = true;
            buttonCleaningUpTrash.Click += buttonCleaningUpTrash_Click;
            // 
            // buttonTextGeneration
            // 
            resources.ApplyResources(buttonTextGeneration, "buttonTextGeneration");
            buttonTextGeneration.FlatAppearance.BorderSize = 0;
            buttonTextGeneration.Name = "buttonTextGeneration";
            buttonTextGeneration.UseVisualStyleBackColor = true;
            buttonTextGeneration.Click += buttonTextGeneration_Click;
            // 
            // buttonDoFile
            // 
            resources.ApplyResources(buttonDoFile, "buttonDoFile");
            buttonDoFile.FlatAppearance.BorderSize = 0;
            buttonDoFile.Name = "buttonDoFile";
            buttonDoFile.UseVisualStyleBackColor = true;
            buttonDoFile.Click += buttonDoFiles_Click;
            // 
            // panelLogo
            // 
            panelLogo.BackColor = Color.FromArgb(39, 39, 58);
            panelLogo.Controls.Add(label);
            resources.ApplyResources(panelLogo, "panelLogo");
            panelLogo.Name = "panelLogo";
            // 
            // label
            // 
            resources.ApplyResources(label, "label");
            label.Name = "label";
            label.Click += label_Click;
            // 
            // panelTitleBar
            // 
            panelTitleBar.BackColor = Color.FromArgb(0, 150, 136);
            panelTitleBar.Controls.Add(labelTitle);
            resources.ApplyResources(panelTitleBar, "panelTitleBar");
            panelTitleBar.Name = "panelTitleBar";
            // 
            // labelTitle
            // 
            resources.ApplyResources(labelTitle, "labelTitle");
            labelTitle.Name = "labelTitle";
            // 
            // panelDesktopPane
            // 
            panelDesktopPane.BackColor = Color.White;
            resources.ApplyResources(panelDesktopPane, "panelDesktopPane");
            panelDesktopPane.ForeColor = Color.Black;
            panelDesktopPane.Name = "panelDesktopPane";
            // 
            // Main
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelDesktopPane);
            Controls.Add(panelTitleBar);
            Controls.Add(panelMenu);
            ForeColor = Color.Gainsboro;
            Name = "Main";
            FormClosing += Main_FormClosing;
            Load += Main_Load;
            panelMenu.ResumeLayout(false);
            panelLogo.ResumeLayout(false);
            panelLogo.PerformLayout();
            panelTitleBar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMenu;
        private Panel panelLogo;
        private Button buttonDoFile;
        private Button buttonGameTools;
        private Button buttonTools;
        private Button buttonCleaningUpTrash;
        private Button buttonTextGeneration;
        private Panel panelTitleBar;
        private Label labelTitle;
        private Label label;
        private Panel panelDesktopPane;
        private Button buttonOptions;
        private Button buttonStrangeQuestionAndAnswer;
    }
}
