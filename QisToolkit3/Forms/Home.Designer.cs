namespace QisToolkit3.Forms
{
    partial class Home
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            labelIntroduce1 = new Label();
            labelTitle = new Label();
            labelIntroduce2 = new Label();
            SuspendLayout();
            // 
            // labelIntroduce1
            // 
            resources.ApplyResources(labelIntroduce1, "labelIntroduce1");
            labelIntroduce1.BackColor = Color.FromArgb(233, 233, 233);
            labelIntroduce1.ForeColor = Color.Black;
            labelIntroduce1.Name = "labelIntroduce1";
            // 
            // labelTitle
            // 
            resources.ApplyResources(labelTitle, "labelTitle");
            labelTitle.ForeColor = Color.Black;
            labelTitle.Name = "labelTitle";
            // 
            // labelIntroduce2
            // 
            resources.ApplyResources(labelIntroduce2, "labelIntroduce2");
            labelIntroduce2.BackColor = Color.FromArgb(233, 233, 233);
            labelIntroduce2.ForeColor = Color.Black;
            labelIntroduce2.Name = "labelIntroduce2";
            // 
            // Home
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(labelIntroduce2);
            Controls.Add(labelIntroduce1);
            Controls.Add(labelTitle);
            Name = "Home";
            FormClosing += Home_FormClosing;
            Load += Home_Load;
            ResumeLayout(false);
        }

        #endregion

        private Label labelIntroduce1;
        private Label labelTitle;
        private Label labelIntroduce2;
    }
}