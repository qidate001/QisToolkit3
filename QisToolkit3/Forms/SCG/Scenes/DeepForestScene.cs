using static QisToolkit3.Forms.SCG.GameState;

namespace QisToolkit3.Forms.SCG.Scenes
{
    public class DeepForestScene : SceneBase
    {
        public DeepForestScene(SurvivalChallengeGame form) : base(form) { }

        public override SceneId Id => SceneId.DeepForest;

        public override void Enter() => Render();

        private void Render()
        {
            if (Item_DarkBlocker)
            {
                Form.SetMessage("你走进了丛林深处，左右两边仿佛仿佛一边是山火，一边是冰川，\n但你凭借着 太阳·御寒服 与 月亮·御焰服 安然走过。\n" +
                                "深处的尽头是一块石板与两个大理石柱子，石板上面刻着两个字——「高塔」");
                Form.SetButtonTexts(
                    L_ForestGrass ? "左通道" : "左草丛",
                    "左大理石柱子", "石板", "右大理石柱子",
                    R_ForestGrass ? "右通道" : "右草丛");
                Form.SetButtonEnableds();
            }
            else
            {
                Form.SetMessage("你走进了████，████仿佛仿佛██是██，██是██，\n但你凭借着 太阳·御寒服 与 月亮·御焰服 安然走过。\n" +
                                "██的██是一块██与两个█████，██上面█████——「██」");
                Form.SetButtonTexts("███", "██████", "██", "██████", "███");
            }
        }

        public override void HandleOption(int index)
        {
            switch (index)
            {
                // 左草丛
                case 0:
                    if (L_ForestGrass)
                    {
                        Form.SetWorld(WorldId.L_TheEndOfTheContinent);
                        YanName = LTEOTC_KnowYanName ? "岩" : "未知";
                        Form.SetMessage(
                            "你走向了那漆黑且望不尽头的隧道，勇敢的走了进去！\n" +
                            "你每走出一步，你的脚步就变重一分……\n" +
                            "不知道走了多久，你的眼前出现了一面镜子？\n" +
                            "你看向了那面镜子，但那镜子中显现出了另一个面孔。\n" +
                            "对方一副现代科学家打扮，下一刻，那面镜子无限扭曲延展！\n" +
                            "瞬时间，囊括了你的身体！你只感觉你的灵魂坠入冰窟……\n" +
                            "虚无之中，你仿佛听到了一句来自灵魂的声音\n\n" +
                            "当你凝视深渊时，深渊也在凝视着你。Kido……\n\n" +
                            "【你已进入时空碎块：大陆终焉】\n" +
                            "你从一个山洞中醒来，左手异常疼痛，眼睛难以睁开。\n" +
                            $"{YanName}：“你醒了？”\n{YanName}：“伤好点了没？”\n" +
                            $"{YanName}：“那群外城的条子真的逼烦，让我们进外城能咋的？”\n" +
                            "");
                        Form.EnterScene(SceneId.LTEOTC_Prologue_0);
                    }
                    else
                    {
                        Form.XSetMessage("你触摸了一下左草丛，你感觉自己身体的能量在被快速抽走……", -500);
                        if (Hunger > 0)
                        {
                            Form.AddMessage("当你回过神之时，草丛早已消失。");
                            L_ForestGrass = true;
                        }
                        else Form.DeathJudgment();
                        Render();
                    }
                    break;

                case 1:
                    Form.SetMessage("你触摸了一下左大理石柱子，什么事都没发生……");
                    break;

                case 2:
                    Form.SetMessage("你触摸了一下石板，什么事都没发生……");
                    break;

                case 3:
                    Form.SetMessage("你触摸了一下右大理石柱子，什么事都没发生……");
                    break;

                // 右草丛
                case 4:
                    if (R_ForestGrass)
                    {
                        // 注意：原代码此分支只显示消息，不切换世界（未完成的 bug），保持原样
                        Form.SetMessage(
                            "你走向了那漆黑且望不尽头的隧道，勇敢的走了进去！\n" +
                            "你每走出一步，你的脚步就变重一分……\n" +
                            "不知道走了多久，你的眼前出现了一面镜子？\n" +
                            "你看向了那面镜子，但那镜子中显现出了另一个面孔。\n" +
                            "对方一副现代科学家打扮，下一刻，那面镜子无限扭曲延展！\n" +
                            "瞬时间，囊括了你的身体！你只感觉你的灵魂坠入冰窟……\n" +
                            "虚无之中，你仿佛听到了一句来自灵魂的声音\n\n" +
                            "当你凝视深渊时，深渊也在凝视着你。Kido……\n\n" +
                            "【你已进入时空碎块：大陆终焉】\n" +
                            "你从一个山洞中醒来，左手异常疼痛，眼睛难以睁开。\n" +
                            $"{YanName}：“你醒了？”\n{YanName}：“伤好点了没？”\n" +
                            $"{YanName}：“那群外城的条子真的逼烦，让我们进外城能咋的？”\n"
                        );
                    }
                    else
                    {
                        Form.XSetMessage("你触摸了一下右草丛，你感觉自己身体的水分在被快速抽走……", 0, -500);
                        if (Thirsty > 0)
                        {
                            Form.AddMessage("当你回过神之时，草丛早已消失。");
                            R_ForestGrass = true;
                        }
                        else Form.DeathJudgment();
                        Render();
                    }
                    break;
            }
        }
    }
}