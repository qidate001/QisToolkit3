using static QisToolkit3.Forms.SCG.GameState;

namespace QisToolkit3.Forms.SCG.Scenes
{
    public class LTEOTC_OuterCityScene : SceneBase
    {
        public LTEOTC_OuterCityScene(SurvivalChallengeGame form) : base(form) { }

        public override SceneId Id => SceneId.LTEOTC_OuterCityMain;

        public override void Enter() => Render();

        public override void HandleOption(int index)
        {
            switch (index)
            {
                case 0: DoRecyclingYard(); break;
                case 1: break;             // 地下诊所：未实现
                case 2: DoGarbageMountain(); break;
                case 3: break;             // 黑市：未实现
                case 4: DoHome(); break;
            }
            Render();
        }

        private void Render()
        {
            Form.SetButtonTexts(
                "前往回收站",
                "前往地下诊所",
                "前往垃圾山",
                "前往黑市",
                "返回住的地方");
            Form.SetButtonOptionFont();
            Form.SetButtonEnableds(
                LTEOTC_KnowOuterCity_RecyclingYard,
                LTEOTC_KnowOuterCity_UndergroundClinic,
                LTEOTC_KnowOuterCity_GarbageMountain,
                LTEOTC_KnowOuterCity_BlackMarket,
                LTEOTC_KnowOuterCity_Home);
            Form.NextBackEnable(false);
        }

        // ===== 回收站（首次进入触发垃圾山剧情，其余走废料兑换） =====
        private void DoRecyclingYard()
        {
            if (LTEOTC_UnlockTheGarbageMountainPlot)
            {
                SetMessageTextTemporarilyStored =
                    "你刚到回收站门口，就听见身后有人喊你。\n" +
                    $"{YanName}：“喂，等会儿！”\n" +
                    $"你回头，是{YanName}。他手里攥着一条破麻袋，小跑过来。\n" +
                    $"{YanName}：“今天垃圾山那边没人管，一起去？”\n" +
                    $"{YanName}：“前两天我在那边翻到一块完整的理式板，老炉说能换三天的口粮。”\n" +
                    $"{YanName}晃了晃手里的麻袋。\n" +
                    $"{YanName}：“走不走？”\n" +
                    "你看了他一眼，点了点头。\n" +
                    $"{YanName}：“走！天黑前回来。”\n";

                DoGarbageMountain();
                LTEOTC_KnowOuterCity_GarbageMountain = true;
                return;
            }

            // ---- 废料兑换（原 PL_LTEOTC_RecyclingYard 整段搬运，逐字不变）----
            if (Item_Scrap > 0 || Item_QualityScrap > 0)
            {
                if (Item_Scrap > 0 && Item_QualityScrap <= 0)
                {
                    if (Item_Scrap < 50)
                    {
                        Form.SetMessage(
                            $"你走进回收站，把一袋子废料倒在老炉面前。\n" +
                            $"老炉低头看了一眼，用脚踢了踢。\n" +
                            $"老炉：“就这点？才{Item_Scrap}个？也太少了吧！”\n" +
                            $"老炉：“这连塞牙缝都不够的。你是不是又偷懒了？”\n" +
                            $"{YanName}在旁边插嘴：“今天垃圾山那边条子多，不好翻。”\n" +
                            $"老炉哼了一声，从口袋里摸出几个硬币丢在桌上。\n" +
                            $"老炉：“算了算了，给你折算。下次多搞点再来，别浪费我时间。”\n" +
                            $"你收起钱，老炉已经转身去摆弄他那堆废铁了。",
                            0, 0, -2, -5, Item_Scrap / 10);
                    }
                    else if (Item_Scrap <= 200)
                    {
                        Form.SetMessage(
                            $"你走进回收站，把一袋子废料倒在老炉面前。\n" +
                            $"老炉蹲下来翻了翻。\n" +
                            $"老炉：“哟，今天收获不错嘛，有{Item_Scrap}个呐！”\n" +
                            $"老炉：“虽然都是些破烂货，但量还行。”\n" +
                            $"他站起来，拍了拍手上的灰，从抽屉里数出几张票子递给你。\n" +
                            $"老炉：“拿去吧。省着点花。”\n" +
                            $"{YanName}笑嘻嘻地凑过来：“老炉，能不能多给点？我们下顿饭还没着落。”\n" +
                            $"老炉瞪了他一眼：“滚蛋。”",
                            0, 0, -2, -5, Item_Scrap / 10);
                    }
                    else
                    {
                        Form.SetMessage(
                            $"你走进回收站，把两大袋子废料倒在老炉面前。\n" +
                            $"老炉惊了一下，随即蹲下来仔细翻看。\n" +
                            $"老炉：“哟，这么多？攒了几天呐？”\n" +
                            $"老炉：“不过……你咋一个优质的都没有阿？”\n" +
                            $"他抬头看了你一眼，眼神有点复杂。\n" +
                            $"老炉：“光靠量可不行。下次眼睛放亮点，好的和坏的混在一起，那才亏。”\n" +
                            $"他掏出钱，数了数，又加了两张。\n" +
                            $"老炉：“给你多算点吧。看来能吃点好点的了这两天……”\n" +
                            $"{YanName}在你旁边低声说：“老炉今天心情不错啊，平时他可不这样。”",
                            0, 0, -2, -5, Item_Scrap / 9);
                    }
                }
                else if (Item_QualityScrap > 0 && Item_Scrap <= 0)
                {
                    if (Item_QualityScrap < 5)
                    {
                        Form.SetMessage(
                            $"你走进回收站，小心翼翼地从怀里掏出几块优质废料放在桌上。\n" +
                            $"老炉眼睛一亮，拿起一块对着光看了看。\n" +
                            $"老炉：“嗯……品相不错。就是太少了。”\n" +
                            $"老炉：“{Item_QualityScrap}块，也就够你换两顿饱饭。”\n" +
                            $"他数了钱递给你。\n" +
                            $"老炉：“下次多找点这种的，我给你高价。”",
                            0, 0, -2, -5, Item_QualityScrap * 15);
                    }
                    else if (Item_QualityScrap <= 20)
                    {
                        Form.SetMessage(
                            $"你走进回收站，把一袋优质废料倒在老炉桌上。\n" +
                            $"老炉拿起一块，用手指敲了敲，又放到鼻子前闻了一下。\n" +
                            $"老炉：“好东西。这是从哪翻的？”\n" +
                            $"你没说话。老炉也没追问。\n" +
                            $"老炉：“行，我不问。你有你的路。”\n" +
                            $"他数钱的时候比平时慢，一张一张地数。\n" +
                            $"老炉：“{Item_QualityScrap}块……够你吃小半个月了。”\n" +
                            $"他把钱推到你面前。\n" +
                            $"老炉：“下次再有，优先拿来给我。别让篾片那小子截了。”",
                            0, 0, -2, -5, Item_QualityScrap * 18);
                    }
                    else
                    {
                        if (LTEOTC_KnowOuterCity_RecyclingYard_BackdoorTrade)
                        {
                            Form.SetMessage(
                                $"你从后门走进回收站，把满满一袋优质废料放在老炉面前。\n" +
                                $"老炉愣了一下，然后一块一块地翻看。\n" +
                                $"他看了很久，沉默了一会儿。\n" +
                                $"他把钱全部拿出来，又加了一些。\n" +
                                $"老炉：“拿着。这趟活我不问。”\n" +
                                $"你拿起钱的时候，老炉低声说了一句：“走的后门，你应该知道的吧。有些东西……翻出来是福，也是祸。”",
                                0, 0, -2, -5, Item_QualityScrap * 22, 0, 0, 3);
                        }
                        else
                        {
                            Form.SetMessage(
                                $"你走进回收站，把满满一袋优质废料放在老炉面前。\n" +
                                $"老炉愣了一下，然后一块一块地翻看。\n" +
                                $"他看了很久。\n" +
                                $"老炉：“……你这是在塔底下翻的？”\n" +
                                $"你摇头。老炉盯着你，不相信。\n" +
                                $"老炉：“这些不是普通废料。这是塔里流出来的那种。封塔之前的。”\n" +
                                $"他沉默了一会儿。\n" +
                                $"老炉：“我不管你在哪翻的。但你听好了——别让条子知道你有这东西。”\n" +
                                $"他把钱全部拿出来，又加了一些。\n" +
                                $"老炉：“拿着。这趟活我不问。”\n" +
                                $"你拿起钱的时候，老炉低声说了一句：“有些东西……翻出来是福，也是祸。”",
                                0, 0, -2, -5, Item_QualityScrap * 22, 0, 0, 3);
                        }
                    }
                }
                else
                {
                    if (Item_Scrap < 50 && Item_QualityScrap < 5)
                    {
                        Form.SetMessage(
                            $"你走进回收站，把两袋子废料倒在老炉面前。\n" +
                            $"一袋是普通废料，另一袋是挑出来的优质品。\n" +
                            $"老炉先翻了翻那袋普通货：“嗯，量不多。”\n" +
                            $"又看了看那几块优质废料：“品相还行，就是太少了。”\n" +
                            $"老炉：“行吧，凑一起给你算。”\n" +
                            $"他算了算，把钱递给你。\n" +
                            $"老炉：“下次多攒点再过来，分开跑不嫌累啊？”",
                            0, 0, -2, -5, Item_Scrap / 10 + Item_QualityScrap * 15);
                    }
                    else if (Item_Scrap <= 200 && Item_QualityScrap <= 20)
                    {
                        Form.SetMessage(
                            $"你走进回收站，把两袋子废料倒在老炉面前。\n" +
                            $"老炉先翻了翻普通货：“嗯，量还行。”\n" +
                            $"又翻了翻优质货：“啧，这个不错。”\n" +
                            $"他抬头看了你一眼。\n" +
                            $"老炉：“今天没少下功夫啊。”\n" +
                            $"他数钱的时候难得露出了一点笑意。\n" +
                            $"老炉：“这袋子优质货成色很好，以后多找这种的。”\n" +
                            $"他把钱递给你，又额外扔了一块干饼过来。\n" +
                            $"老炉：“搭你的。”\n" +
                            $"{YanName}一把抢过去咬了一口：“老炉你偏心！”",
                            5, 0, -2, -5, Item_Scrap / 10 + Item_QualityScrap * 18);
                        Item_CompressedBiscuit += 1;
                    }
                    else if (Item_Scrap > 200 && Item_QualityScrap <= 20)
                    {
                        if (LTEOTC_KnowOuterCity_RecyclingYard_BackdoorTrade)
                        {
                            Form.SetMessage(
                                $"你走进回收站，把两大袋普通废料和一袋优质废料倒在地。\n" +
                                $"老炉看着那堆东西，吹了声口哨。\n" +
                                $"老炉：“你这是把垃圾山搬空了吧？”\n" +
                                $"他蹲下来仔细翻看。\n" +
                                $"老炉：“普通货量大，优质货成色也好。”\n" +
                                $"老炉：“不过……”他拿起一块优质废料看了看，“这个纹路不太对。”\n" +
                                $"他看了你一眼，没有继续说下去。\n" +
                                $"老炉：“行，算你聪明，没明面拿来…… 算了，收。”\n" +
                                $"他给了你一大笔钱。\n" +
                                $"{YanName}在旁边看得眼睛都直了。\n",
                                0, 0, -2, -5, Item_Scrap / 9 + Item_QualityScrap * 20);
                        }
                        else
                        {
                            Form.SetMessage(
                                $"你走进回收站，把两大袋普通废料和一袋优质废料倒在地。\n" +
                                $"老炉看着那堆东西，吹了声口哨。\n" +
                                $"老炉：“你这是把垃圾山搬空了吧？”\n" +
                                $"他蹲下来仔细翻看。\n" +
                                $"老炉：“普通货量大，优质货成色也好。”\n" +
                                $"老炉：“不过……”他拿起一块优质废料看了看，“这个纹路不太对。”\n" +
                                $"他看了你一眼，没有继续说下去。\n" +
                                $"老炉：“算了，收。”\n" +
                                $"他给了你一大笔钱。\n" +
                                $"{YanName}在旁边看得眼睛都直了。\n" +
                                $"老炉：“下次再拿这种东西来，别这么大张旗鼓。走后面。”\n" +
                                $"他朝棚子后面努了努嘴。",
                                0, 0, -2, -5, Item_Scrap / 9 + Item_QualityScrap * 20);
                            LTEOTC_KnowOuterCity_RecyclingYard_BackdoorTrade = true;
                        }
                    }
                    else if (Item_Scrap <= 200 && Item_QualityScrap > 20)
                    {
                        if (LTEOTC_KnowOuterCity_RecyclingYard_BackdoorTrade)
                        {
                            Form.SetMessage(
                                $"你从后门走进回收站，把一袋普通废料和一袋优质废料放在老炉面前。\n" +
                                $"老炉：“……这么多？”\n" +
                                $"他拿起一块，又拿起一块，越看眉头皱得越紧。\n" +
                                $"老炉：“这成色不对。这不是垃圾山能翻出来的东西。”\n" +
                                $"他盯着你：“你下塔了？”\n" +
                                $"你摇头。\n" +
                                $"老炉：“你最好没有。”\n" +
                                $"他压低声音：“这些东西上面有编号。是塔里的。”\n" +
                                $"老炉：“收。但以后走后面。别让任何人看见。”\n" +
                                $"他快速数了钱塞到你手里。\n" +
                                $"他只是看着那袋子废料，不知道在想什么。",
                                0, 0, 2, 5, Item_Scrap / 10 + Item_QualityScrap * 23, 0, 0, 3);
                        }
                        else
                        {
                            Form.SetMessage(
                                $"你走进回收站，把一袋普通废料和一袋优质废料放在老炉面前。\n" +
                                $"老炉先看了普通货，点了点头。\n" +
                                $"然后他打开那袋优质废料。\n" +
                                $"他的动作停住了。\n" +
                                $"老炉：“……这么多？”\n" +
                                $"他拿起一块，又拿起一块，越看眉头皱得越紧。\n" +
                                $"老炉：“这成色不对。这不是垃圾山能翻出来的东西。”\n" +
                                $"他盯着你：“你下塔了？”\n" +
                                $"你摇头。\n" +
                                $"老炉：“你最好没有。”\n" +
                                $"他压低声音：“这些东西上面有编号。是塔里的。”\n" +
                                $"老炉：“收。但以后走后面。别让任何人看见。”\n" +
                                $"他快速数了钱塞到你手里。\n" +
                                $"你走的时候，老炉没有像往常一样说“下次再来”。\n" +
                                $"他只是看着那袋子废料，不知道在想什么。",
                                0, 0, 0, 0, Item_Scrap / 10 + Item_QualityScrap * 23, 0, 0, 3);
                            LTEOTC_KnowOuterCity_BlackMarket = true;
                        }
                    }
                    else
                    {
                        if (LTEOTC_KnowOuterCity_RecyclingYard_BackdoorTrade)
                        {
                            Form.SetMessage(
                                $"你从后门走进回收站，把三袋子废料倒在地上。\n" +
                                $"老炉站起来，走到棚子门口往外看了一眼，然后把帘子拉了下来。\n" +
                                $"老炉看着那堆东西，沉默了。\n" +
                                $"他蹲下去，一块一块地翻。\n" +
                                $"翻到后面，他的动作越来越慢。\n" +
                                $"老炉：“这些……”\n" +
                                $"他拿起一块优质废料，翻过来看了看背面。\n" +
                                $"老炉：“封塔之后被埋掉的东西是吗？”\n" +
                                $"老炉：“按理说，这种东西不该再出现在地面上。”\n" +
                                $"他看了你很久。\n" +
                                $"老炉：“行，批货我收了。”\n" +
                                $"他给了你比平时多一倍的价钱。\n",
                                0, 0, 3, 7, Item_Scrap / 9 + Item_QualityScrap * 26, 0, 0, 5);
                        }
                        else
                        {
                            Form.SetMessage(
                                $"你走进回收站，把三袋子废料倒在地上。\n" +
                                $"老炉看着那堆东西，沉默了。\n" +
                                $"他蹲下去，一块一块地翻。\n" +
                                $"翻到后面，他的动作越来越慢。\n" +
                                $"老炉：“这些……”\n" +
                                $"他拿起一块优质废料，翻过来看了看背面。\n" +
                                $"老炉的手停住了。\n" +
                                $"老炉：“这东西你从哪翻的？”\n" +
                                $"你没说话。\n" +
                                $"老炉站起来，走到棚子门口往外看了一眼，然后把帘子拉了下来。\n" +
                                $"老炉：“这块上面有塔的研究编号。这是封塔之后被埋掉的东西。”\n" +
                                $"老炉：“按理说，这种东西不该再出现在地面上。”\n" +
                                $"他看了你很久。\n" +
                                $"老炉：“这批货我收了。但你听好了——”\n" +
                                $"老炉：“以后翻到带纹路、带编号的，不要拿到明面上来。”\n" +
                                $"老炉：“直接走后面找我。”\n" +
                                $"他给了你比平时多一倍的价钱。\n" +
                                $"你走出回收站的时候，{YanName}拉了拉你的袖子。\n" +
                                $"{YanName}：“老炉今天……不对劲。”",
                                0, 0, 2, 5, Item_Scrap / 9 + Item_QualityScrap * 25, 0, 0, 5);
                            LTEOTC_KnowOuterCity_BlackMarket = true;
                        }
                    }
                }

                Item_Scrap = 0;
                Item_QualityScrap = 0;
            }
            else
            {
                int talkRoll = random.Next(1, 5);
                string talkMsg = "";

                switch (talkRoll)
                {
                    case 1:
                        talkMsg =
                            "你走进回收站，老炉正在给一个铁架子焊腿。\n" +
                            "他头也没抬：“空手来的？”\n" +
                            "你嗯了一声。\n" +
                            "老炉：“那就别站这碍事。去垃圾山翻点东西再回来。”\n" +
                            "他继续焊他的铁架子。火花溅了一地。\n" +
                            "岩在旁边找了个角落蹲着，捡了根铁丝在土里划拉。\n" +
                            $"{YanName}：“老炉，你这有没喝的？”\n" +
                            "老炉：“滚。”\n";
                        break;
                    case 2:
                        talkMsg =
                            "你走进回收站，老炉正在门口坐着晒太阳。\n" +
                            "他看了你一眼。\n" +
                            "老炉：“今天垃圾山那边条子少，不去翻两把？”\n" +
                            "老炉：“别跟我说你吃饱了。看你那脸色，三天没吃饭的样子。”\n" +
                            "你摸了摸自己的脸。\n" +
                            "老炉叹了口气：“年轻的时候不好好攒，老了就像我这样。”\n" +
                            "他拍了拍自己的瘸腿。\n";
                        break;
                    case 3:
                        talkMsg =
                            "你走进回收站，闻到一股糊味。\n" +
                            "老炉正对着一口小锅发愁。锅里的东西黑成一团。\n" +
                            "老炉：“……妈的。”\n" +
                            "岩凑过去看了一眼：“老炉你这是炖啥呢？”\n" +
                            "老炉：“本来想炖点汤。”\n" +
                            "他看了一眼锅里的黑炭，放弃了。\n" +
                            "老炉：“算了。等你有废料了再过来，我请你吃顿正经的。”\n" +
                            "你点了点头。老炉摆了摆手。\n";
                        break;
                    case 4:
                        talkMsg =
                            "你走进回收站，老炉正在跟一个人说话。\n" +
                            "那人背对着你，穿一件灰色斗篷。\n" +
                            "你进来的一瞬间，那个人的话停住了。\n" +
                            "他侧过头看了你一眼。\n" +
                            "你只看见他下巴上有一道很长的疤。\n" +
                            "然后他转身走了，经过你身边的时候带起一阵风，里面混着药味和铁锈味。\n" +
                            "老炉看着你，脸色不太好看。\n" +
                            "老炉：“……你下次来之前能不能先叫一声？”\n" +
                            "你没理他，看着那个人消失的方向。\n" +
                            "老炉：“那只是个送货的。”\n" +
                            "老炉：“……你信就信，不信拉倒。”\n";
                        break;
                }
                Form.SetMessage(talkMsg);
            }
        }

        // ===== 垃圾山 =====
        private void DoGarbageMountain()
        {
            int scrap = random.Next(30 + Luck * 2, 100 + Luck * 3);
            int qualityScrap = random.Next(0, 10 + Luck / 10);
            int eventRoll = random.Next(1, 101);

            string eventText = "";
            int healthCost = 0;
            int lostCost = 0;
            int extraBottledWater = 0;
            int extraScrap = 0;

            if (eventRoll <= 20)
            {
                int subEvent = random.Next(1, 5);
                switch (subEvent)
                {
                    case 1:
                        healthCost = random.Next(5, 15);
                        eventText =
                            "你正低头翻找，突然被人从后面推了一把。\n" +
                            "三个外环区的拾荒者站在你面前，手里拿着铁管。\n" +
                            "领头的那个吐了口唾沫：“这片是老子的。”\n" +
                            "你杀了对方。\n";
                        break;
                    case 2:
                        extraScrap = random.Next(30, 80);
                        extraBottledWater = random.Next(1, 3);
                        eventText =
                            "你发现了个半埋的箱子。\n" +
                            "撬开来看，里面有两瓶没开封的水，还有一捆废料。\n" +
                            "你把水塞进怀里。\n" +
                            $"【额外获得 废料 +{extraScrap}，矿泉水 ×{extraBottledWater}】\n";
                        break;
                    case 3:
                        healthCost = random.Next(15, 35);
                        eventText =
                            "你听见头顶传来一阵细碎的响声。\n" +
                            "垃圾山顶部正在往下滑。\n" +
                            "你被废铁和碎石埋了半截身子，差点死了。\n";
                        break;
                    case 4:
                        eventText =
                            "你听到一堆废铁后面传来微弱的呻吟声。\n" +
                            "绕过去一看，一个人蜷缩在那里。\n" +
                            "你看到他了之后，他死了。\n" +
                            (Lost < 50 ? "你站了很久。\n" : "你冷漠的走了。") +
                            (Lost < 50 ? "【你忘不掉他眼睛里的东西】\n" : "");
                        lostCost = Lost < 50 ? 5 : 9;
                        break;
                }
            }

            Item_Scrap += scrap + extraScrap;
            Item_QualityScrap += qualityScrap;
            Item_BottledWater += extraBottledWater;

            string scrapMsg = qualityScrap > 0
                ? $"你翻找了一番，找到了 {scrap} 份普通废料和 {qualityScrap} 份优质废料。"
                : $"你翻找了一番，找到了 {scrap} 份普通废料。";

            Form.SetMessage(
                "你前往了垃圾山，在上面寻找一些值钱的东西......\n" +
                (eventRoll <= 20 ? "\n" + eventText + "\n" : "") +
                scrapMsg,
                0, 0, 0, 0, 0, 0, -healthCost, 0, lostCost);

            LTEOTC_UnlockTheGarbageMountainPlot = false;
        }

        // ===== 回家 =====
        private void DoHome()
        {
            int lostReduce = random.Next(1, 6);
            Lost -= lostReduce;
            Lost = System.Math.Clamp(Lost, 0, 100);

            string lostMessage = lostReduce == 1 ? "你感到浮躁不安。" :
                                 lostReduce <= 3 ? "你迷迷糊糊睡了一夜，醒来时脑子还算清醒。" :
                                                   "你休息了一晚，精神不错。";

            Form.SetMessage(
                "你推开住处的门。\n" +
                "你开始了冥想。\n" +
                lostMessage + "\n" +
                $"【迷失值 -{lostReduce}】\n");
        }
    }
}