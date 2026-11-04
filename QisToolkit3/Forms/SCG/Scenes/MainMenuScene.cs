using System;
using static QisToolkit3.Forms.SCG.GameState;

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
                case 1: Form.EnterScene(SceneId.Foods); break;
                case 2: Form.EnterScene(SceneId.KnowledgeArea); break;
                case 3: DoGoOut(); break;
                case 4: Form.EnterScene(SceneId.TheWorld); break;
            }
        }

        // 从 Form.GF_GoOutAndPlay 完整迁移
        private void DoGoOut()
        {
            Random r = new Random();
            int TmpType = r.Next(0, 10);
            int TmpGold = r.Next(25, 75);
            int TmpGetMood = r.Next(15, 35);

            bool hasFriend = Friend > 0 && (Gold >= 75 || Form.DifficultyValue == 0);
            bool tarotBoost = Form.CurrentTarot == "皇后" || Form.CurrentTarot == "战车";

            if (hasFriend)
            {
                if (TmpType <= 7 || tarotBoost)
                {
                    if (Form.DifficultyValue != 0)
                        Form.XSetMessage("你与朋友一起出去玩了！", 0, 0, -1, TmpGetMood, -TmpGold);
                    else
                        Form.XSetMessage("你与朋友一起出去玩了！", 0, 0, -1, TmpGetMood);
                }
                else if (TmpType == 8)
                {
                    if (Form.DifficultyValue != 0)
                        Form.XSetMessage("你与朋友一起出去玩了！", 0, 0, -1, TmpGetMood, -TmpGold);
                    else
                        Form.XSetMessage("你与朋友一起出去玩了！中途结交到一个新朋友", 0, 0, -1, TmpGetMood + 20);
                    ++Friend;
                }
                else if (TmpType == 9)
                {
                    if (Form.DifficultyValue != 0)
                        Form.XSetMessage("你与朋友一起出去玩了！结果与朋友闹矛盾绝交了！", 0, 0, -5, -30, -TmpGold, -5);
                    else
                        Form.XSetMessage("你与朋友一起出去玩了！结果与朋友闹矛盾绝交了！", 0, 0, -5, -30, -15, -5);
                    --Friend;
                }
            }
            else
            {
                if (TmpType <= 7 || tarotBoost)
                {
                    if (Form.DifficultyValue != 0)
                        Form.XSetMessage("你自己出去玩了！", 0, 0, 0, TmpGetMood - 5, -TmpGold);
                    else
                        Form.XSetMessage("你自己出去玩了！", 0, 0, 0, TmpGetMood - 5);
                }
                else if (TmpType >= 8 && TmpType <= 9)
                {
                    if (Form.DifficultyValue != 0)
                        Form.XSetMessage("你与朋友一起出去玩了！中途结交到一个新朋友", 0, 0, 0, TmpGetMood + 25, -TmpGold);
                    else
                        Form.XSetMessage("你与朋友一起出去玩了！中途结交到一个新朋友", 0, 0, 0, TmpGetMood + 25);
                    ++Friend;
                }
            }
        }
    }
}