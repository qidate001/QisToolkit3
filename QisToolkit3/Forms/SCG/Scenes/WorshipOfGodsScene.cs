using static QisToolkit3.Forms.SCG.GameState;

namespace QisToolkit3.Forms.SCG.Scenes
{
    public class WorshipOfGodsScene : SceneBase
    {
        public WorshipOfGodsScene(SurvivalChallengeGame form) : base(form) { }

        public override SceneId Id => SceneId.WorshipOfGods;

        public override void Enter()
        {
            if (Item_DarkBlocker)
            {
                Form.XSetMessage("你走出小镇，走向了神秘森林，刚入神秘森林浅处，你就看到了五尊神像。\n在五尊神像地板上写着：“供奉你最诚挚的信仰致辞，换取神明的兴趣。”\n通过再三思考，你决定供奉的神像是……");
                Form.SetButtonTexts("欺诈之神", "命运之神", "死亡之神", "秩序之神", "记忆之神");
            }
            else
            {
                Form.XSetMessage("你走出小镇，走向了████，刚入██████，你就看到了████。\n在██████上写着：“████████████████████”\n通过再三思考，你决定██的██是……");
                Form.SetButtonTexts("████", "████", "████", "████", "████");
            }
            Form.SetButtonEnableds(
                Item_God == 0 || Item_God == 1,
                Item_God == 0 || Item_God == 2,
                Item_God == 0 || Item_God == 3,
                Item_God == 0 || Item_God == 4,
                Item_God == 0 || Item_God == 5);
        }

        public override void HandleOption(int index)
        {
            switch (index)
            {
                case 0:
                    if (Item_DarkBlocker)
                    {
                        Form.XSetMessage("你对着代表着欺诈的神像拜了一拜说道：\n“赞美我伟大的欺诈之神，行伟大的欺诈之举”");
                        Form.XAddMessage("欺诈似乎回应了你，一个只有半截的面具飞了过来！\n获得唯一道具：欺诈之面");
                    }
                    else
                    {
                        Form.XSetMessage("你对着█████的██████说道：\n“███████████████████”");
                        Form.XAddMessage("██似乎██了你，██████的███了过来！\n获得唯一道具：████！！");
                    }
                    break;

                case 1:
                    if (Item_DarkBlocker)
                    {
                        Form.XSetMessage("你对着代表着命运的神像拜了一拜说道：\n“赞美我伟大的命运之神，我将重配命运之体”");
                        Form.XAddMessage("命运似乎回应了你，一个永远壹点朝上的骰子飞了过来！\n获得唯一道具：命运之骰");
                    }
                    else
                    {
                        Form.XSetMessage("你对着█████的██████说道：\n“███████████████████”");
                        Form.XAddMessage("██似乎██了你，████████的███了过来！\n获得唯一道具：████！！");
                    }
                    break;

                case 2:
                    if (Item_DarkBlocker)
                    {
                        Form.XSetMessage("你对着代表着死亡的神像拜了一拜说道：\n“赞美我伟大的死亡之神，万物实向死而生也”");
                        Form.XAddMessage("死亡似乎回应了你，一个散发着浓郁死亡之气的镰刀飞了过来！\n获得唯一道具：死亡之镰");
                    }
                    else
                    {
                        Form.XSetMessage("你对着█████的██████说道：\n“███████████████████”");
                        Form.XAddMessage("██似乎██了你，███████████████的███了过来！\n获得唯一道具：████！！");
                    }
                    break;

                case 3:
                    if (Item_DarkBlocker)
                    {
                        Form.XSetMessage("你对着代表着秩序的神像拜了一拜说道：\n“赞美我伟大的秩序之神，我等必敬畏秩序也”");
                        Form.XAddMessage("秩序似乎回应了你，一个散发规则的权杖飞了过来！\n获得唯一道具：秩序之杖");
                    }
                    else
                    {
                        Form.XSetMessage("你对着█████的██████说道：\n“███████████████████”");
                        Form.XAddMessage("██似乎██了你，██████的███了过来！\n获得唯一道具：████！！");
                    }
                    break;

                case 4:
                    if (Item_DarkBlocker)
                    {
                        Form.XSetMessage("你对着代表着时间的神像拜了一拜说道：\n“赞美我伟大的记忆之神，历史与万界为永恒”");
                        Form.XAddMessage("记忆乎回应了你，一个包含了无数知识的书本飞了过来！\n获得唯一道具：记忆之书");
                    }
                    else
                    {
                        Form.XSetMessage("你对着█████的██████说道：\n“███████████████████”");
                        Form.XAddMessage("██似乎██了你，█████████的███了过来！\n获得唯一道具：████！！");
                    }
                    break;
            }

            Item_God = index + 1;
            Form.EnterScene(SceneId.Main);
        }
    }
}