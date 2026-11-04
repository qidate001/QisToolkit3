using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QisToolkit3.Forms.SCG.Scenes
{
    public class MainMenuScene : SceneBase
    {
        public MainMenuScene(SurvivalChallengeGame form) : base(form) { }

        public override SceneId Id => SceneId.Main;

        public override void Enter()
        {
            Form.SetButtonTexts();
            Form.ReSetButtonEnableds();
            Form.NextBackEnable(false);
            Form.SetButtonOptionFont();
        }

        public override void HandleOption(int index)
        {
            switch (index)
            {
                case 0: Form.EnterScene(SceneId.EarnMoney); break;
                case 1: Form.EnterScene(SceneId.Foods_0); break;
                case 2: Form.To_KnowledgeArea(); break;   // 未迁移，直调旧方法
                case 3: Form.GF_GoOutAndPlay(); break;   // 未迁移
                case 4: Form.NextBackEnable(); Form.To_TheWorld(); break; // 未迁移
            }
        }
    }
}
