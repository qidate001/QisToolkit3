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
            buttonMinecraftProjectE = new Button();
            buttonSurvivalChallengeGame = new Button();
            labelSurvivalChallengeGame = new Label();
            label1 = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // buttonMinecraftProjectE
            // 
            buttonMinecraftProjectE.Image = Properties.Resources.MinecraftProjectE;
            buttonMinecraftProjectE.Location = new Point(16, 16);
            buttonMinecraftProjectE.Margin = new Padding(2);
            buttonMinecraftProjectE.Name = "buttonMinecraftProjectE";
            buttonMinecraftProjectE.Size = new Size(282, 78);
            buttonMinecraftProjectE.TabIndex = 0;
            buttonMinecraftProjectE.UseVisualStyleBackColor = true;
            buttonMinecraftProjectE.Click += buttonMinecraftProjectE_Click;
            // 
            // buttonSurvivalChallengeGame
            // 
            buttonSurvivalChallengeGame.Font = new Font("Microsoft Sans Serif", 20F);
            buttonSurvivalChallengeGame.Location = new Point(302, 16);
            buttonSurvivalChallengeGame.Margin = new Padding(2);
            buttonSurvivalChallengeGame.Name = "buttonSurvivalChallengeGame";
            buttonSurvivalChallengeGame.Size = new Size(319, 107);
            buttonSurvivalChallengeGame.TabIndex = 1;
            buttonSurvivalChallengeGame.Text = "挑战：生存小游戏\r\n《问身 问己 问心》";
            buttonSurvivalChallengeGame.UseVisualStyleBackColor = true;
            buttonSurvivalChallengeGame.Click += button1_Click;
            // 
            // labelSurvivalChallengeGame
            // 
            labelSurvivalChallengeGame.AutoSize = true;
            labelSurvivalChallengeGame.Font = new Font("微软雅黑", 12F);
            labelSurvivalChallengeGame.ForeColor = Color.Red;
            labelSurvivalChallengeGame.Location = new Point(333, 125);
            labelSurvivalChallengeGame.Name = "labelSurvivalChallengeGame";
            labelSurvivalChallengeGame.Size = new Size(264, 81);
            labelSurvivalChallengeGame.TabIndex = 2;
            labelSurvivalChallengeGame.Text = "↑↑↑ 该游戏还未开发完毕 ↑↑↑\r\n有BUG或意见请联系开发者\r\n3563532971@qq.com";
            labelSurvivalChallengeGame.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("微软雅黑", 12F);
            label1.ForeColor = Color.Red;
            label1.Location = new Point(333, 340);
            label1.Name = "label1";
            label1.Size = new Size(264, 81);
            label1.TabIndex = 4;
            label1.Text = "↑↑↑ 该游戏还未开发完毕 ↑↑↑\r\n有BUG或意见请联系开发者\r\n3563532971@qq.com";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            button1.Font = new Font("Microsoft Sans Serif", 33F);
            button1.Location = new Point(302, 231);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(319, 107);
            button1.TabIndex = 3;
            button1.Text = "奇怪的食物";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // GameTools
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(766, 497);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(labelSurvivalChallengeGame);
            Controls.Add(buttonSurvivalChallengeGame);
            Controls.Add(buttonMinecraftProjectE);
            Margin = new Padding(2);
            Name = "GameTools";
            Text = "游戏工具";
            Load += GameTools_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonMinecraftProjectE;
        private Button buttonSurvivalChallengeGame;
        private Label labelSurvivalChallengeGame;
        private Label label1;
        private Button button1;
    }
}