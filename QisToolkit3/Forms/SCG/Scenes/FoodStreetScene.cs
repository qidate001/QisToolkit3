using static QisToolkit3.Forms.SCG.GameState;

namespace QisToolkit3.Forms.SCG.Scenes
{
    public class FoodStreetScene : SceneBase
    {
        private int _page = 0;

        public FoodStreetScene(SurvivalChallengeGame form) : base(form) { }

        public override SceneId Id => _page == 0 ? SceneId.Foods_0 : SceneId.Foods_1;

        public override void Enter()
        {
            _page = 0;
            NowDoing = Id;
            Form.NextBackEnable(true);
            Render();
        }

        public override void OnNext()
        {
            _page = 1;
            NowDoing = Id;
            Render();
        }

        public override void OnBack()
        {
            _page = 0;
            NowDoing = Id;
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
                Form.XSetMessage("你前往了美食街，……");
                Form.SetButtonTexts("购买 饺子", "购买 肉夹馍", "购买 烤玉米", "购买 柠檬水", "购买 咖啡");
                Form.SetButtonEnableds(
                    Gold >= Food_Dumpling,
                    Gold >= Food_RouJiaMo,
                    Gold >= Food_RoastedCorn,
                    Gold >= Food_Lemonade,
                    Gold >= Food_Coffee);
            }
            else
            {
                Form.XSetMessage("你前往了美食街，……");
                Form.SetButtonTexts("购买 冰淇淋", "购买 净化套餐", "购买 口渴果冻", "购买 饥饿果冻", "购买 饕餮盛宴");
                Form.SetButtonEnableds(
                    Gold >= Food_Popsicle,
                    Gold >= Food_PurificationPackage,
                    Gold >= Food_ThirstJelly,
                    Gold >= Food_HungerJelly,
                    Gold >= Food_GluttonousFeast);
            }
        }

        private void HandlePage0(int index)
        {
            switch (index)
            {
                case 0:
                    Form.XSetMessage("你前往了美食街，购买了一盘饺子", 64, 10, 0, 5, -Food_Dumpling);
                    break;
                case 1:
                    Form.XSetMessage("你前往了美食街，购买了一个肉夹馍", 50, 0, 0, 5, -Food_Dumpling, 0, 7);
                    break;
                case 2:
                    Form.XSetMessage("你前往了美食街，购买了一个烤玉米", 20, 3, 0, 5, -Food_RoastedCorn);
                    break;
                case 3:
                    Form.XSetMessage("你前往了美食街，购买了一个柠檬水", 5, 45, 0, 5, -Food_Lemonade);
                    break;
                case 4:
                    Form.XSetMessage("你前往了美食街，购买了一个咖啡", 2, 30, 10, -2, -Food_Coffee);
                    break;
            }
        }

        private void HandlePage1(int index)
        {
            switch (index)
            {
                case 0:
                    if (Temperature > 30)
                        Form.XSetMessage("你前往了美食街，购买了一根冰淇淋", 5, 15, 5, 5, -Food_Popsicle, 0, 15);
                    else if (Temperature < 0)
                        Form.XSetMessage("你前往了美食街，购买了一根冰淇淋", 5, 15, 5, 5, -Food_Popsicle, 0, -15);
                    else
                        Form.XSetMessage("你前往了美食街，购买了一根冰淇淋", 5, 15, 5, 5, -Food_Popsicle);
                    break;
                case 1:
                    Form.XSetMessage("你前往了美食街，购买了净化套餐", 5, 5, 5, -25, -Food_PurificationPackage, -5, -25, -25);
                    break;
                case 2:
                    Form.XSetMessage("你前往了美食街，购买了一个口渴果冻", 40, -30, 0, 0, -Food_ThirstJelly);
                    break;
                case 3:
                    Form.XSetMessage("你前往了美食街，购买了一个饥饿果冻", -30, 40, 0, 0, -Food_HungerJelly);
                    break;
                case 4:
                    Form.XSetMessage("你前往了美食街，购买了一整套饕餮盛宴", 500, 500, -10, 20, -Food_GluttonousFeast);
                    break;
            }
        }
    }
}