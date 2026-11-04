using static QisToolkit3.Forms.SCG.GameState;

namespace QisToolkit3.Forms.SCG.Scenes
{
    public class WorkRoomScene : SceneBase
    {
        public WorkRoomScene(SurvivalChallengeGame form) : base(form) { }

        public override SceneId Id => SceneId.WorkRoom;

        public override void Enter()
        {
            if (Item_DarkBlocker)
            {
                Form.SetMessage("你走进了工作房室，入眼就是无数研究工作台，与无数的祭坛、尸体等……\n你打算制作/研究……");
                Form.SetButtonTexts("太阳·御寒服", "月亮·御焰服", "污染干扰器", "暂无", "暂无");
            }
            else
            {
                Form.SetMessage("你走进了████，入眼就是无数█████，与无数的██、██等……\n你打算██/██……");
                Form.SetButtonTexts("██████", "██████", "█████", "暂无", "暂无");
            }
            Form.SetButtonEnableds(
                Item_SunWaterDrop && Item_WinterClothing == 1,
                Item_MoonWaterDrop && Item_SummerClothing == 1,
                Item_EssenceReason >= 2 && Gold >= 2000 && !Item_PollutionJammer,
                false,
                false);
        }

        public override void HandleOption(int index)
        {
            switch (index)
            {
                case 0:
                    Form.XSetMessage("你通过使用生命能量将 太阳水滴 融入了 御寒服！你得到了 太阳·御寒服！", 0, 0, -5, 5, 0, 0, -15, 50);
                    Item_WinterClothing = 2; Item_SunWaterDrop = false;
                    break;

                case 1:
                    Form.XSetMessage("你通过使用生命能量将 月亮水滴 融入了 御焰服！你得到了 月亮·御焰服！", 0, 0, -5, 5, 0, 0, -15, 50);
                    Item_SummerClothing = 2; Item_MoonWaterDrop = false;
                    break;

                case 2:
                    Form.XSetMessage("你通过使用理智精华进行疯狂的实验！你得到了 污染干扰器！", -3, -3, -25, 25, 2000, 0, -15, 30);
                    Item_EssenceReason -= 2; Item_PollutionJammer = true;
                    break;
            }
            Form.EnterScene(SceneId.Main);
        }
    }
}