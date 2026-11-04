using static QisToolkit3.Forms.SCG.GameState;

namespace QisToolkit3.Forms.SCG.Scenes
{
    public class MysteriousForestIntersectionScene : SceneBase
    {
        public MysteriousForestIntersectionScene(SurvivalChallengeGame form) : base(form) { }

        public override SceneId Id => SceneId.MysteriousForestIntersection;

        public override void Enter()
        {
            if (Item_DarkBlocker)
            {
                Form.SetMessage("你走出小镇，走向了神秘森林，刚入踏入神秘森林，你就看到了五条路。\n正北，西北，东北，正西，正东。每一条路上都有写有字。");
                if (Item_God == 0) Form.SetButtonTexts("正北神像祭坛", "西北时光之地", "东北森林深处", "正西工作房室", "正东塔罗牌墙");
                else if (Item_God == 1) Form.SetButtonTexts("正北神像祭坛", "西北嬉笑之潮", "东北森林深处", "正西工作房室", "正东塔罗牌墙");
                else if (Item_God == 2) Form.SetButtonTexts("正北神像祭坛", "西北命运长廊", "东北森林深处", "正西工作房室", "正东塔罗牌墙");
                else if (Item_God == 3) Form.SetButtonTexts("正北神像祭坛", "西北死亡尸海", "东北森林深处", "正西工作房室", "正东塔罗牌墙");
                else if (Item_God == 4) Form.SetButtonTexts("正北神像祭坛", "西北天锁地牢", "东北森林深处", "正西工作房室", "正东塔罗牌墙");
                else if (Item_God == 5) Form.SetButtonTexts("正北神像祭坛", "西北历史神殿", "东北森林深处", "正西工作房室", "正东塔罗牌墙");
            }
            else
            {
                Form.SetMessage("你走出小镇，走向了████，刚入踏入████，你就看到了███。\n正北，西北，东北，正西，正东。████上都有███。");
                Form.SetButtonTexts("正北████", "西北████", "东北████", "正西████", "正东████");
            }
            Form.SetButtonEnableds(
                true,
                false,
                Item_WinterClothing >= 2 && Item_SummerClothing >= 2,
                San > 10,
                Form.CurrentTarot != string.Empty && TC_Wall);
        }

        public override void HandleOption(int index)
        {
            switch (index)
            {
                case 0:
                    Form.EnterScene(SceneId.WorshipOfGods);
                    break;

                case 1:
                    break;

                case 2:
                    if (Item_WinterClothing >= 2 && Item_SummerClothing >= 2)
                    {
                        Form.EnterScene(SceneId.DeepForest);
                    }
                    else
                    {
                        if (Item_DarkBlocker)
                            Form.SetMessage("你看向了森林深处，里面似乎冰火两重天，你不敢进入……");
                        else
                            Form.SetMessage("你看向了████，里面似乎冰火两重天，你不敢进入……");
                        Form.EnterScene(SceneId.Main);
                    }
                    break;

                case 3:
                    Form.EnterScene(SceneId.WorkRoom);
                    break;

                case 4:
                    int temp = 0;
                    foreach (bool item in TC)
                        if (item == true) ++temp;
                    if (temp == 22) Form.SetMessage("你走向了正东方的路，看着那一面贴满塔罗牌的墙，\n突然！整个墙瞬间亮起！所有的牌上飞出一道光钻进了你的身体！\n你现在可以中途改变一次你携带的塔罗牌！仅现在！");
                    else Form.SetMessage("你走向了正东方的路，看到了一面墙，上面一共有着 " + temp + " 张塔罗牌。");
                    Form.TarotCardEnabled(temp == 22);
                    TC_Set = temp == 22;
                    TC_Wall = !(temp == 22);
                    Form.EnterScene(SceneId.Main);
                    break;
            }
        }
    }
}