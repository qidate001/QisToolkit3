using System;
using static QisToolkit3.Forms.SCG.GameState;

namespace QisToolkit3.Forms.SCG.Scenes
{
    public class EarnMoneyScene : SceneBase
    {
        public EarnMoneyScene(SurvivalChallengeGame form) : base(form) { }

        public override SceneId Id => SceneId.EarnMoney;

        public override void Enter()
        {
            Form.XSetMessage("你想要去挣大钱，……");
            Form.SetButtonTexts("打零工", "跑外卖", "做代购", "去上班", "做开发");
            Form.SetButtonEnableds(
                true,
                WorkExperience >= 15,
                WorkExperience >= 40,
                WorkExperience >= 80,
                WorkExperience >= 120
            );
        }

        public override void HandleOption(int Id)
        {
            int workExperience = random.Next(0, AddWorkExperienceMax);
            if (WorkExperience + workExperience > 150) workExperience = 150 - WorkExperience;

            string time = Morning ? "上午" : "下午";

            switch (Id)
            {
                case 0:
                    {
                        int gold = random.Next(30, 70);
                        int mood = random.Next(2, 5);
                        Form.XSetMessage($"你去打了一{time}临工。", 0, 0, 0, -mood,
                                         gold + gold * WorkExperience / 100, workExperience);
                        break;
                    }
                case 1:
                    {
                        int gold = random.Next(40, 80);
                        int mood = random.Next(3, 6);
                        Form.XSetMessage($"你去送了一{time}外卖。", 0, 0, 0, -mood,
                                         gold + gold * WorkExperience / 100, workExperience);
                        break;
                    }
                case 2:
                    {
                        int gold = random.Next(50, 100);
                        int mood = random.Next(5, 10);
                        Form.XSetMessage($"你去做了一{time}代购。", 0, 0, 0, -mood,
                                         gold + gold * WorkExperience / 100, workExperience);
                        break;
                    }
                case 3:
                    {
                        Form.XSetMessage($"你去上班上了一{time}。", 0, 0, 0, -25,
                                         250 + 250 * WorkExperience / 100, workExperience);
                        break;
                    }
                case 4:
                    {
                        int gold = random.Next(-250, 1000);
                        int mood = random.Next(-25, 25);
                        Form.XSetMessage($"你去做了一{time}开发。", 0, 0, 0, -mood,
                                         gold + gold * WorkExperience / 100, workExperience);
                        break;
                    }
            }

            Form.EnterScene(SceneId.Main);   // 回主菜单
        }
    }
}