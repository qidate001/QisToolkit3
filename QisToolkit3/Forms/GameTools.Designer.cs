namespace QisToolkit3.Forms
{
    partial class GameTools
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameTools));
            buttonMinecraftProjectE = new Button();
            buttonSurvivalChallengeGame = new Button();
            labelSurvivalChallengeGame = new Label();
            button1 = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // buttonMinecraftProjectE
            // 
            buttonMinecraftProjectE.Image = Properties.Resources.MinecraftProjectE;
            resources.ApplyResources(buttonMinecraftProjectE, "buttonMinecraftProjectE");
            buttonMinecraftProjectE.Name = "buttonMinecraftProjectE";
            buttonMinecraftProjectE.UseVisualStyleBackColor = true;
            buttonMinecraftProjectE.Click += buttonMinecraftProjectE_Click;
            // 
            // buttonSurvivalChallengeGame
            // 
            resources.ApplyResources(buttonSurvivalChallengeGame, "buttonSurvivalChallengeGame");
            buttonSurvivalChallengeGame.Name = "buttonSurvivalChallengeGame";
            buttonSurvivalChallengeGame.UseVisualStyleBackColor = true;
            buttonSurvivalChallengeGame.Click += button1_Click;
            // 
            // labelSurvivalChallengeGame
            // 
            resources.ApplyResources(labelSurvivalChallengeGame, "labelSurvivalChallengeGame");
            labelSurvivalChallengeGame.ForeColor = Color.Red;
            labelSurvivalChallengeGame.Name = "labelSurvivalChallengeGame";
            // 
            // button1
            // 
            resources.ApplyResources(button1, "button1");
            button1.Name = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.ForeColor = Color.Red;
            label1.Name = "label1";
            // 
            // GameTools
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(labelSurvivalChallengeGame);
            Controls.Add(buttonSurvivalChallengeGame);
            Controls.Add(buttonMinecraftProjectE);
            Name = "GameTools";
            Load += GameTools_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button buttonMinecraftProjectE;
        private Button buttonSurvivalChallengeGame;
        private Label labelSurvivalChallengeGame;
        private Button button1;
        private Label label1;
    }
}