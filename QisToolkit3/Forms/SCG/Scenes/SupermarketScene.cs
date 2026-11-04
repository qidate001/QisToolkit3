using static QisToolkit3.Forms.SCG.GameState;

namespace QisToolkit3.Forms.SCG.Scenes
{
    public class SupermarketScene : SceneBase
    {
        private int _page = 0;

        public SupermarketScene(SurvivalChallengeGame form) : base(form) { }

        public override SceneId Id => SceneId.Supermarket;

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
            if (_page == 0) HandlePage0(index);
            else HandlePage1(index);
            Form.EnterScene(SceneId.Main);
        }

        private void Render()
        {
            if (_page == 0)
            {
                Form.SetMessage("你前往了超市，你准备买……");
                Form.SetButtonTexts("御寒服", "御焰服", "压缩饼干", "瓶装饮料", "矿泉水");
                Form.SetButtonEnableds(
                    Gold >= 500 && Item_WinterClothing == 0,
                    Gold >= 500 && Item_SummerClothing == 0,
                    Gold >= 500,
                    Gold >= 500,
                    Gold >= 500);
            }
            else
            {
                Form.SetMessage("你走出小镇，走向了……");
                Form.SetButtonTexts("急救包", "解毒剂", "幸运符", "舒缓药", "暂无");
                Form.SetButtonEnableds(
                    Gold >= 1000 && !Item_FirstAidKit,
                    Gold >= 1000 && !Item_Antidote,
                    Gold >= 1000 && !Item_LuckyCharm,
                    Gold >= 1000 && !Item_SoothingMedicine,
                    false);
            }
        }

        private void HandlePage0(int index)
        {
            switch (index)
            {
                case 0:
                    Item_WinterClothing = 1;
                    Form.XSetMessage("你买下了一个御寒服！你现在可以抵御基础低温了。", 0, 0, 0, 0, -500);
                    break;
                case 1:
                    Item_SummerClothing = 1;
                    Form.XSetMessage("你买下了一个御焰服！你现在可以抵御基础高温了。", 0, 0, 0, 0, -500);
                    break;
                case 2:
                    Item_CompressedBiscuit += 1;
                    Form.XSetMessage("你买下了一块压缩饼干。", 0, 0, 0, 0, -500);
                    break;
                case 3:
                    Item_BottledBeverage += 1;
                    Form.XSetMessage("你买下了一瓶瓶装饮料。", 0, 0, 0, 0, -500);
                    break;
                case 4:
                    Item_BottledWater += 1;
                    Form.XSetMessage("你买下了一瓶矿泉水。", 0, 0, 0, 0, -500);
                    break;
            }
        }

        private void HandlePage1(int index)
        {
            switch (index)
            {
                case 0:
                    Item_FirstAidKit = true;
                    Form.XSetMessage("你买下了一个急救包！它或许可以在关键时刻救你一命！", 0, 0, 0, 0, -1000);
                    break;
                case 1:
                    Item_Antidote = true;
                    Form.XSetMessage("你买下了一个解毒剂！它或许可以在关键时刻救你一命！", 0, 0, 0, 0, -1000);
                    break;
                case 2:
                    Item_LuckyCharm = true;
                    Form.XSetMessage("你买下了一个幸运符！它或许可以在关键时刻救你一命！", 0, 0, 0, 0, -1000);
                    break;
                case 3:
                    Item_SoothingMedicine = true;
                    Form.XSetMessage("你买下了一个舒缓药！它或许可以在关键时刻救你一命！", 0, 0, 0, 0, -1000);
                    break;
            }
        }
    }
}