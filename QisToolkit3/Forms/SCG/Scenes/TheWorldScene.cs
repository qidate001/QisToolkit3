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
                    Form.PL_HeartAskingCliff();
                    break;
                case 3:
                    Form.EnterScene(SceneId.MysteryShop);
                    break;
                case 4:
                    if (Item_God == 0) Form.To_WorshipOfGods();
                    else Form.To_MysteriousForestIntersection();
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
    }
}