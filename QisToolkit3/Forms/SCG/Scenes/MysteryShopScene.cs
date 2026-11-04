using static QisToolkit3.Forms.SCG.GameState;

namespace QisToolkit3.Forms.SCG.Scenes
{
    public class MysteryShopScene : SceneBase
    {
        private int _page = 0;

        public MysteryShopScene(SurvivalChallengeGame form) : base(form) { }

        public override SceneId Id => SceneId.MysteryShop;

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
            // 原 To_MysteryShop 开头的那句 XSetMessage，两页共享
            Form.XSetMessage("你走出小镇，走向了那片屹立在迷雾之中的神秘商店，你在迷雾之中穿行\n不知道过去了多久，你抵达了那神秘商店");

            if (_page == 0)
            {
                Form.SetButtonTexts("购买黑暗屏蔽器", "购买饕餮药水", "购买癫狂药水", "购买太阳水滴", "购买月亮水滴");
                Form.SetButtonEnableds(
                    !Item_DarkBlocker && San >= 100,
                    San >= 200,
                    San >= 200,
                    San >= 50 && Gold >= 1000 && !Item_SunWaterDrop,
                    San >= 50 && Gold >= 1000 && !Item_MoonWaterDrop);
            }
            else
            {
                Form.SetButtonTexts("情绪项目", "呆滞项目", "强健项目", "诡异项目", "提纯项目");
                Form.SetButtonEnableds(
                    San >= 100,
                    Mood >= 100,
                    San >= 100,
                    Health >= 50,
                    San >= 100 & Item_God > 0);
            }
        }

        private void HandlePage0(int index)
        {
            switch (index)
            {
                case 0:
                    Item_DarkBlocker = true;
                    Form.XSetMessage("你买下了一个黑暗屏蔽器，你感到一阵头晕目眩", 0, 0, -100);
                    break;
                case 1:
                    Form.XSetMessage("你买下了一瓶饕餮药水，你感到一阵头晕目眩", 0, 0, -200);
                    MaxHunger = 9999; MaxThirsty = 9999; TaotieBuff = 10;
                    break;
                case 2:
                    Form.XSetMessage("你买下了一瓶癫狂药水，你感到一阵头晕目眩", 0, 0, -200);
                    MadBuff = 10;
                    break;
                case 3:
                    Form.XSetMessage("你买下了一滴太阳水滴！", 0, 0, -50, 0, -1000);
                    Item_SunWaterDrop = true;
                    break;
                case 4:
                    Form.XSetMessage("你买下了一滴太阳水滴！", 0, 0, -50, 0, -1000);
                    Item_MoonWaterDrop = true;
                    break;
            }
        }

        private void HandlePage1(int index)
        {
            switch (index)
            {
                case 0:
                    Form.XSetMessage("你选择了情绪项目，你仿佛感受到无数情绪被灌进了脑海", 0, 0, -100, 80);
                    break;
                case 1:
                    Form.XSetMessage("你选择了呆滞项目，你仿佛感受到无数情绪被从脑海抽走", 0, 0, 80, -100);
                    break;
                case 2:
                    Form.XSetMessage("你选择了强健项目，你仿佛感受到你的身体的每个细胞都在嚎叫", 0, 0, -100, 0, 0, 0, 100 - Health);
                    break;
                case 3:
                    Form.XSetMessage("你选择了情绪项目，你仿佛感受到你的身体正在疯狂的崩溃！", 0, 0, 100, 0, 0, 0, -80);
                    break;
                case 4:
                    Form.XSetMessage("你选择了提纯项目，你感受到你的理智在被疯狂抽出！\n你得到了——理智精华！", 0, 0, -100);
                    ++Item_EssenceReason;
                    break;
            }
        }
    }
}