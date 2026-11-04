using static QisToolkit3.Forms.SCG.GameState;

namespace QisToolkit3.Forms.SCG.Scenes
{
    public class TheWorldScene : SceneBase
    {
        private int _page = 0;

        public TheWorldScene(SurvivalChallengeGame form) : base(form) { }

        public override SceneId Id => SceneId.TheWorld;

        public override void Enter()
        {
            _page = 0;
            Form.NextBackEnable(true);
            Render();
        }

        public override void OnNext()
        {
            _page = 1;
            Render();
        }

        public override void OnBack()
        {
            _page = 0;
            Render();
        }

        public override void HandleOption(int index)
        {
            // 第 2 页所有按钮都禁用，不会走到这里
            if (_page == 0) HandlePage0(index);
        }

        private void Render()
        {
            if (_page == 0)
            {
                Form.SetMessage("你走出小镇，走向了……");
                Form.SetButtonTexts("前往医院", "前往超市", "前往问心崖", "前往神秘商店",
                                    Item_DarkBlocker ? "前往神秘森林" : "前往████");

                Form.SetButtonEnableds(
                    Gold >= 100,
                    Gold >= 500,
                    true,
                    San >= 50,
                    San <= 50 && Days < 50 && Item_God == 0 || Item_God > 0);
            }
            else
            {
                Form.SetMessage("你走出小镇，走向了……");
                if (Item_DarkBlocker)
                    Form.SetButtonTexts("前往████", "前往████", "前往████", "前往████", "前往████");
                else
                    Form.SetButtonTexts("前往海滩", "待定", "待定", "待定", "待定");
                Form.SetButtonEnableds(false, false, false, false, false);
            }
        }

        private void HandlePage0(int index)
        {
            switch (index)
            {
                case 0:
                    DoHospital();
                    break;
                case 1:
                    Form.EnterScene(SceneId.Supermarket);
                    break;
                case 2:
                    DoHeartAskingCliff();
                    break;
                case 3:
                    Form.EnterScene(SceneId.MysteryShop);
                    break;
                case 4:
                    if (Item_God == 0) Form.EnterScene(SceneId.WorshipOfGods);
                    else Form.EnterScene(SceneId.MysteriousForestIntersection);
                    break;
            }
        }

        private void DoHospital()
        {
            int count = 0;
            while (Health < 100 && Gold > 10)
            {
                Health += 1;
                Gold -= 10;
                ++count;
            }
            Form.SetMessage("你走出小镇，去了医院治疗。", 0, 0, 0, 0, -(count * 10), 0, count);
            Form.EnterScene(SceneId.Main);
        }

        private void DoHeartAskingCliff()
        {
            if (!HAC_AskBody[0] || !HAC_AskBody[1] || !HAC_AskBody[2])
            {
                if (Health > 60)
                    Form.SetMessage("你走向了那问心崖，但你无论如何跑向那天边，你始终无法接近那崖边");
                else if (Health <= 60 && Health > 40)
                    Form.SetMessage("你走向了那问心崖，你的身体仿佛抵达了那崖边，但又无法触及那遥不可及的天边");
                else
                    Form.SetMessage("你走向了那问心崖，你拖着残破的身躯抵达了那崖边，你看到了那空虚的天空");

                HAC_AskBody[0] = Health > 60 || HAC_AskBody[0];
                HAC_AskBody[1] = (Health <= 60 && Health > 40) || HAC_AskBody[1];
                HAC_AskBody[2] = Health <= 40 || HAC_AskBody[2];

                if ((HAC_AskBody[0] && HAC_AskBody[1] && HAC_AskBody[2]) || DeBugMode)
                {
                    Form.AddMessage("在这一刻，你感觉整个人都通透了！仿佛瞬间明白了许多道理");
                    Form.AddMessage("你看了看自己的身体，在这一瞬间，感到身体是如此之陌生！");
                    Form.AddMessage("无数的记忆碎片让你的头非常痛，无数的记忆自行拼接起来。");
                    Form.AddMessage("你看向了你的过去，那是让你非常陌生的过去......");
                }
            }
            else if (!HAC_AskSelf[0] || !HAC_AskSelf[1] || !HAC_AskSelf[2])
            {
                if (Mood >= 100)
                    Form.SetMessage("你走向了那问心崖，那崖边站着个人，你无法看清那人的模样... 就如同自己");
                else if (Mood < 100 && Mood >= 50)
                    Form.SetMessage("你走向了那问心崖，他正在哭泣，你无论如何都无法安慰下他... 就如同自己");
                else
                    Form.SetMessage("你走向了那问心崖，他一跃而下，你无论如何都无法救下他... 就如同自己");

                HAC_AskSelf[0] = Mood >= 100 || HAC_AskSelf[0];
                HAC_AskSelf[1] = (Mood < 100 && Mood >= 50) || HAC_AskSelf[1];
                HAC_AskSelf[2] = Mood < 50 || HAC_AskSelf[2];

                if ((HAC_AskSelf[0] && HAC_AskSelf[1] && HAC_AskSelf[2]) || DeBugMode)
                {
                    Form.AddMessage("在这一刻，你感觉整个人都通透了！仿佛瞬间明白了许多道理");
                    Form.AddMessage("你感受了以下自己，在这一瞬间，感到自己是如此之陌生！");
                    Form.AddMessage("无数的记忆碎片让你的头非常痛，无数的记忆自行拼接起来。");
                    Form.AddMessage("你看向了你的未来，那是让你非常陌生的未来......");
                }
            }
            else if (!HAC_AskSoul[0] || !HAC_AskSoul[1] || !HAC_AskSoul[2])
            {
                if (San >= 100)
                    Form.SetMessage("你走向了那问心崖，但你无论如何跑向那天边，你始终无法接近那崖边");
                else if (San < 100 && San >= 50)
                    Form.SetMessage("你走向了那问心崖，你的身体仿佛抵达了那崖边，但又无法触及那遥不可及的天边");
                else
                    Form.SetMessage("你走向了那问心崖，你拖着残破的身躯抵达了那崖边，你看到了那空虚的天空");

                HAC_AskSoul[0] = San >= 100 || HAC_AskSoul[0];
                HAC_AskSoul[1] = (San < 100 && San >= 50) || HAC_AskSoul[1];
                HAC_AskSoul[2] = San < 50 || HAC_AskSoul[2];

                if ((HAC_AskSoul[0] && HAC_AskSoul[1] && HAC_AskSoul[2]) || DeBugMode)
                {
                    Form.AddMessage("在这一刻，你感觉整个人都通透了！仿佛瞬间明白了许多道理");
                    Form.AddMessage("你感受了以下自己，在这一瞬间，感到自己是如此之陌生！");
                    Form.AddMessage("无数的记忆碎片让你的头非常痛，无数的记忆自行拼接起来。");
                    Form.AddMessage("你看向了你的现在，那是让你非常陌生的现在......");

                    Form.AddMessage("您看到了真我！游戏结束.");
                    Form.AddMessage("获得塔罗牌：月亮");
                    TC[18] = true;

                    Form.GameOver();
                    Form.TC_Save();
                }
            }

            NowDoing = SceneId.Main;
            Form.SetButtonTexts();
        }
    }
}