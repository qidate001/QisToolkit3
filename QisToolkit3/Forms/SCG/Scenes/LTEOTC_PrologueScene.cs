using static QisToolkit3.Forms.SCG.GameState;

namespace QisToolkit3.Forms.SCG.Scenes
{
    public class LTEOTC_PrologueScene : SceneBase
    {
        private int _page = 0;

        public LTEOTC_PrologueScene(SurvivalChallengeGame form) : base(form) { }

        public override SceneId Id => SceneId.LTEOTC_Prologue_0;

        public override void Enter()
        {
            _page = 0;
            Render();
        }

        public override void OnNext()
        {
            if (_page == 0)
            {
                _page = 1;
                Render();
            }
        }

        public override void OnBack()
        {
            if (_page == 1)
            {
                _page = 0;
                Render();
            }
        }

        public override void HandleOption(int index)
        {
            if (_page == 0) HandlePage0(index);
            else HandlePage1(index);
        }

        private void HandlePage0(int index)
        {
            switch (index)
            {
                case 0:
                    Form.SetMessage(
                        $"{YanName}：“你失忆了？”\n" +
                        $"岩：“我是岩啊，你不记得我了吗？”\n" +
                        $"岩：“算了算了……我们先离开这再说。”");
                    LTEOTC_KnowYanName = true;
                    YanName = LTEOTC_KnowYanName ? "岩" : "未知";
                    Render();
                    break;

                case 1:
                    Form.SetMessage(
                        $"{YanName}：一个山洞里。\n" +
                        $"{YanName}：我们和内城的条子打了起来，你被射伤了，你昏死过去了！\n" +
                        $"{YanName}：不过放心，这里特别隐蔽，条子绝对不可能找不到这里的。");
                    LTEOTC_KnowPrologue_0_1 = true;
                    Render();
                    break;

                case 2:
                    Form.SetMessage(
                        $"{YanName}：我们和内城的条子打了起来，你被射伤了，你昏死过去了！\n" +
                        $"{YanName}：我把你带到了这个山洞里，放心，这里特别隐蔽，条子绝对不可能找不到这里的。");
                    LTEOTC_KnowPrologue_0_1 = true;
                    Render();
                    break;

                case 3:
                    Form.SetMessage(
                        $"{YanName}：“还能为什么？我们是无籍者啊。”\n" +
                        $"{YanName}：“内城那帮人觉得无籍者都是污染携带者，见一个抓一个。”\n" +
                        $"{YanName}：“妈的，我们连外环区都不让进，住山洞里还要被追。”");
                    Render();
                    break;

                case 4:
                    Form.SetMessage(
                        $"{YanName}：“被条子的理式棍敲了一下，骨头应该没断。”\n" +
                        $"{YanName}：“不过你左手一直在抽……”\n" +
                        $"{YanName}：“算了，回去找疤姨看看吧。”");
                    Render();
                    break;
            }
        }

        private void HandlePage1(int index)
        {
            switch (index)
            {
                case 0:
                    Form.SetMessage(
                        $"{YanName}：“我没事，皮糙肉厚的。”\n" +
                        $"{YanName}：“倒是你……伤口旁边有点发黑了，别是污染感染。”\n" +
                        $"{YanName}：“回去得让疤姨仔细看看。”");
                    YanSuspicion -= 3;
                    Render();
                    break;

                case 1:
                    Form.SetMessage(
                        $"{YanName}：“先回外环区，疤姨那边能躲。”\n" +
                        $"{YanName}：“外面条子还在搜，等天黑了再走。”");
                    YanSuspicion += 2;
                    LTEOTC_KnowPrologue_KnowHowToLeave = true;
                    Render();
                    break;

                case 2:
                    Form.SetMessage(
                        $"{YanName}：“行了，天快黑了，我们出去吧。”\n" +
                        $"你和{YanName}从山洞里钻出来，外环区的破败街道在暮色中像一道未愈合的伤口。\n" +
                        $"{YanName}拍了拍你的肩膀，朝老炉的废铁回收站走去。\n" +
                        "那里是你们的落脚点，也是你们这个月换口粮的地方。\n\n" +
                        "【序章结束】\n" +
                        "你是一个无籍者，没有户籍，没有家。\n" +
                        (LTEOTC_KnowYanName ? "你有一个兄弟叫岩。\n" : "你有一个不知名的兄弟\n") +
                        "你住在老炉的回收站附近。\n" +
                        "你不知道自己为什么想进入那个隧道里，也不知道那面镜子是什么。\n" +
                        "但你的左手隐隐作痛，像是在提醒你——" +
                        (LTEOTC_FirstLeave ? "你肯定发生了什么。" : "你又回来了。"));
                    Lost += LTEOTC_FirstLeave ? 5 : 1;
                    Form.EnterScene(SceneId.LTEOTC_OuterCityMain);
                    break;
            }
        }

        private void Render()
        {
            if (_page == 0)
            {
                Form.SetButtonTexts(
                    "你是谁？",
                    "这是在哪？",
                    "什么情况现在？",
                    LTEOTC_KnowPrologue_0_1 ? "条子为什么抓我们？" : "",
                    LTEOTC_KnowPrologue_0_1 ? "我的手怎么了？" : "");
                Form.SetButtonOptionFont(33F, 33F, 33F, 28F);
                Form.SetButtonEnableds(
                    !LTEOTC_KnowYanName, true, true,
                    LTEOTC_KnowPrologue_0_1, LTEOTC_KnowPrologue_0_1);
                Form.NextBackEnable(LTEOTC_KnowPrologue_0_1);
            }
            else
            {
                Form.SetButtonTexts(
                    LTEOTC_KnowPrologue_0_1 ? "你受伤了吗？" : "",
                    LTEOTC_KnowPrologue_0_1 ? "接下来怎么办？" : "",
                    "【离开山洞】",
                    "",
                    "");
                Form.SetButtonEnableds(
                    LTEOTC_KnowPrologue_0_1,
                    LTEOTC_KnowPrologue_0_1,
                    LTEOTC_KnowPrologue_KnowHowToLeave,
                    false, false);
                // 原代码 0-1 页不重设字体，保持 0-0 的字体，这里原样保留
            }
        }
    }
}