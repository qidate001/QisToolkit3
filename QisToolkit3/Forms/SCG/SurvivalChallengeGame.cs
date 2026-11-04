using Microsoft.VisualBasic.Devices;
using Newtonsoft.Json.Linq;
using QisToolkit3.Forms.SCG.Scenes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QisToolkit3.Forms.SCG.GameState;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace QisToolkit3.Forms.SCG
{
    public partial class SurvivalChallengeGame : Form
    {
        public SurvivalChallengeGame()
        {
            InitializeComponent();

            // 注册场景
            SceneRegistry.Register(new MainMenuScene(this));
            SceneRegistry.Register(new EarnMoneyScene(this));
            SceneRegistry.Register(new KnowledgeAreaScene(this));
            SceneRegistry.Register(new FoodsScene(this));
            SceneRegistry.Register(new TheWorldScene(this));
            SceneRegistry.Register(new SupermarketScene(this));
            SceneRegistry.Register(new MysteryShopScene(this));
            SceneRegistry.Register(new WorshipOfGodsScene(this));
            SceneRegistry.Register(new WorkRoomScene(this));
            SceneRegistry.Register(new MysteriousForestIntersectionScene(this));
            SceneRegistry.Register(new DeepForestScene(this));
            SceneRegistry.Register(new LTEOTC_PrologueScene(this));
            SceneRegistry.Register(new LTEOTC_OuterCityScene(this));

            ShowMessage();
            TC_Read();
            TC_Show();

            // 初始化
            Qi.FormInitDo(this.Text);

            // if (DeBugMode) labelLog.Text = string.Empty;
            labelDeBug_Times.Visible = DeBugMode;
            // labelTitle.Visible = !DeBugMode;
            labelDeBug_Times.Text = $"{ExtremeColdComes} | {EndOfExtremeCold}   {ExtremeHotComes} | {EndOfExtremeHot}";
            checkBoxDeBug_NoDied.Visible = DeBugMode;
        }

        public void ReSetButtonEnableds() =>
            SetButtonEnableds(true, Gold >= 5, KnowledgeCd <= 0, Gold >= 50, Days >= 10);

        public void TarotCardEnabled(bool enabled) => comboBoxTarotCard.Enabled = enabled;

        public int DifficultyValue => trackBarDifficulty.Value;

        public string CurrentTarot => comboBoxTarotCard.Text;

        private void ShowMessage()
        {
            labelDays.Text = $"今天是第 {Days} 天 {(Morning ? "上午" : "下午")}";
            labelSan.Text = $"理智：{San}%";
            labelMood.Text = $"心情：{Mood}%";
            labelLuck.Text = $"气运：{DisplayLuckValue}%";
            labelLost.Text = $"迷失：{Lost}%";
            labelGold.Text = $"金币：{Gold}G";
            labelHealth.Text = $"健康：{Health}%";
            labelTemperature.Text = $"室外温度：{Temperature}℃";
            labelHunger.Text = $"饥饿值：{Hunger} / {MaxHunger}";
            labelThirsty.Text = $"口渴值：{Thirsty} / {MaxThirsty}";

            labelWorkExperience.Text = World == WorldId.Main
                ? $"工作经验：{WorkExperience}%"
                : $"时空回溯：{ResetCount}次";

            labelPollution.Text = KnowVoid 
                ? $"归虚值：{Pollution}%" 
                : $"污染值：{Pollution}%";

            labelSan.ForeColor = San <= 30 ? Color.Red : Color.Black;
            labelMood.ForeColor = Mood <= 30 ? Color.Red : Color.Black;
            labelLost.ForeColor = Lost >= 60 ? Color.Red : Color.Black;
            labelHunger.ForeColor = Hunger <= 30 ? Color.Red : Color.Black;
            labelHealth.ForeColor = Health <= 30 ? Color.Red : Color.Black;
            labelThirsty.ForeColor = Thirsty <= 30 ? Color.Red : Color.Black;
            labelPollution.ForeColor = Pollution >= 60 ? Color.Red : Color.Black;

            // 仅主世界部分
            if (World == WorldId.Main)
            {
                bool showAsk = HAC_AskBody.Contains(true) || HAC_AskSelf.Contains(true) || HAC_AskSoul.Contains(true);
                
                // 问心崖挑战进度显示
                if (showAsk)
                {
                    labelHAC_AskBody.Text = $"问身挑战：{string.Concat(HAC_AskBody.Select(hac => hac ? "■" : "□"))}";
                    labelHAC_AskSelf.Text = $"问己挑战：{string.Concat(HAC_AskSelf.Select(hac => hac ? "■" : "□"))}";
                    labelHAC_AskSoul.Text = $"问心挑战：{string.Concat(HAC_AskSoul.Select(hac => hac ? "■" : "□"))}";

                    labelHAC_AskBody.Visible = true;
                    labelHAC_AskSelf.Visible = true;
                    labelHAC_AskSoul.Visible = true;
                }
            }


            // 道具显示
            labelDarkBlocker.Visible = Item_DarkBlocker;
            labelSunWaterDrop.Visible = Item_SunWaterDrop;
            labelMoonWaterDrop.Visible = Item_MoonWaterDrop;
            labelPollutionJammer.Visible = Item_PollutionJammer;

            labelItem_Antidote.Visible = Item_Antidote;
            labelItem_LuckyCharm.Visible = Item_LuckyCharm;
            labelItem_FirstAidKit.Visible = Item_FirstAidKit;
            label_SoothingMedicine.Visible = Item_SoothingMedicine;

            labelWinterClothing.Visible = Item_WinterClothing != 0;
            labelWinterClothing.Text = Item_WinterClothing == 1
                ? "御寒服" : Item_WinterClothing == 2
                ? "太阳·御寒服" : labelWinterClothing.Text;

            labelSummerClothing.Visible = Item_SummerClothing != 0;
            labelSummerClothing.Text = Item_SummerClothing == 1
                ? "御焰服" : Item_SummerClothing == 2
                ? "月亮·御焰服" : labelSummerClothing.Text;

            labelGodItem.Visible = Item_God != 0;

            // 神赐道具
            switch (Item_God)
            {
                case 1:
                    labelGodItem.Text = "欺诈之面";
                    break;

                case 2:
                    labelGodItem.Text = "命运之骰";
                    break;

                case 3:
                    labelGodItem.Text = "死亡之镰";
                    break;

                case 4:
                    labelGodItem.Text = "秩序之杖";
                    break;

                case 5:
                    labelGodItem.Text = "记忆之书";
                    break;
            }
            labelEssenceReason.Visible = Item_EssenceReason > 0;
            labelEssenceReason.Text = $"理智精华 × {Item_EssenceReason}";

            labelCompressedBiscuit.Visible = Item_CompressedBiscuit > 0;
            labelCompressedBiscuit.Text = $"压缩饼干 × {Item_CompressedBiscuit}";

            labelBottledWater.Visible = Item_BottledWater > 0;
            labelBottledWater.Text = $"矿泉水 × {Item_BottledWater}";

            labelBottledBeverage.Visible = Item_BottledBeverage > 0;
            labelBottledBeverage.Text = $"瓶装饮料 × {Item_BottledBeverage}";

            labelScrap.Visible = Item_Scrap > 0;
            labelScrap.Text = $"废料 × {Item_Scrap}";

            labelQualityScrap.Visible = Item_QualityScrap > 0;
            labelQualityScrap.Text = $"优质废料 × {Item_QualityScrap}";

            SetButtonEnabledsColor(buttonOption0.Enabled, buttonOption1.Enabled, buttonOption2.Enabled, buttonOption3.Enabled, buttonOption4.Enabled);
        }

        // 主函数
        private void Main()
        {
            if (!(NowDoing == SceneId.Main || NowDoing == SceneId.LTEOTC_OuterCityMain))
                return;

            // 上下午转换
            Morning = !Morning;

            // 主界
            if (World == WorldId.Main)
            {
                // 每天执行
                if (Morning)
                {
                    DailyRandomEvents(); // 每日随机事件
                    Hunger -= Natural_Hunger;
                    Thirsty -= Natural_Thirsty;
                    --KnowledgeCd;
                    --MadBuff;
                    ++Days;



                    // 癫狂药水结束
                    if (MadBuff - 1 == 0)
                        San = 1;

                    // 饕餮药水结束
                    if (TaotieBuff - 1 == 0)
                    {
                        MaxHunger = 100; Hunger = MaxHunger;
                        MaxThirsty = 100; Thirsty = MaxThirsty;
                    }

                    // 绝对理智
                    if (G_AbsoluteRationality > 0)
                    {
                        //Mood = -99999;
                        San = 99999;
                    }

                    // 绝对理智结束
                    if (G_AbsoluteRationality - 1 == 0)
                    {
                        San = 30;
                        // Mood = 10;
                    }

                    // 绝对理智每日减少
                    if (G_AbsoluteRationality > 0)
                        --G_AbsoluteRationality;

                    // 饕餮BUFF每日减少
                    if (TaotieBuff > 0)
                        --TaotieBuff;

                    // 避死BUFF每日减少
                    if (G_AvoidingDeath > 0)
                        --G_AvoidingDeath;

                    // 随机事件必定好运BUFF每日减少
                    if (G_MustGoodRandomEvent > 0) --G_MustGoodRandomEvent;

                    // 数值翻倍BUFF每日减少
                    if (G_DoubleTheValue > 0) --G_DoubleTheValue;

                    // 塔罗牌：月亮 - 排解恐惧
                    if (comboBoxTarotCard.Text != "月亮") Mood -= Natural_Mood;

                    // 塔罗牌：愚者 - 自负顽固
                    if (comboBoxTarotCard.Text == "愚者") San -= Natural_San * 2;

                    // 塔罗牌：月亮 - 排解恐惧
                    else if (comboBoxTarotCard.Text != "月亮") San -= Natural_San;

                    // 新手期结束
                    if (Days >= 10)
                    {
                        ProcessPollution();  // 污染

                        Health -= Natural_Health + Pollution / 5;
                        Gold -= (Days - 10) * 2;

                        // 塔罗牌：愚者 - 自负顽固
                        if (comboBoxTarotCard.Text == "愚者") Gold -= (Days - 10) * 2;

                        // 高低温处理
                        ProcessLowTemperature();
                        ProcessHighTemperature();
                    }
                }


                // 命运之骰
                ProcessGodItem2();



                SetButtonTexts();
                ReSetButtonEnableds();
                NextBackEnable(false);
                SetButtonOptionFont();
            }

            // 时空碎块
            else
            {
                if (Morning) ++Days;   

            }



            // 死亡判断
            if (DeathJudgment())
            {
                // 心情过低降低理智
                if (Mood < 10)
                {
                    if (Item_SoothingMedicine)
                    {
                        Mood = 50;
                        Item_SoothingMedicine = false;
                        XAddMessage("由于心情过低，情急之下吃了舒缓药平复了一下情绪。");
                    }
                    else
                    {
                        XAddMessage("由于心情过低，理智 - " + (30 - Mood) + '%');
                        San -= 30 - Mood;
                    }
                }

                if (MadBuff > 0) San = -99999;

                if (Morning == true && Days >= 100)
                {
                    if (Item_God == 0)
                    {
                        SetMessage("您活过了 100 天！游戏结束.");
                        if (PrincipalLine == 0)
                            AddMessage("获得塔罗牌：愚者");
                        TC[0] = true;
                        GameOver();
                        TC_Save();
                    }
                    else
                    {
                        SetMessage("您活过了 100 天！游戏结束.");
                        if (PrincipalLine == 0)
                            AddMessage("获得塔罗牌：月亮");

                        TC[18] = true;
                        GameOver();
                        TC_Save();
                    }
                }

                if (Morning) MessageLog += "\n\n第 " + Days + " 天 上午：";

                else MessageLog += "\n\n第 " + Days + " 天 下午：";
            }



            // 塔罗牌变动
            if (TC_Set) TC_Set = false;
            else comboBoxTarotCard.Enabled = false;

            // 禁用难度拖动模块
            trackBarDifficulty.Enabled = false;



            // 计算幸运值
            CalculateLucky();

            // 更新/显示信息
            ShowMessage();
        }

        // 按钮中转站
        private void OperationAssignment(int Id)
        {
            AddLog($"选项操作跳转 ({World} | {NowDoing} | {Id})");

            if (SceneRegistry.TryGet(NowDoing, out var scene))
            {
                scene.HandleOption(Id);
                Main();   // 若场景切回 Main，Main() 会自动过半天 + 刷新按钮
                return;
            }

            // 主函数
            Main();
        }

        // 计算当前幸运值
        private void CalculateLucky()
        {
            Luck = BaseLuck +
                // 幸运符
                (Item_LuckyCharm ? 5 : 0);

            // 限制上下限
            Luck = Math.Clamp(Luck, -100, 100);
        }


        #region 主界数值变动处理

        // 处理每日污染值变动
        private void ProcessPollution()
        {
            // 污染每日上涨条件
            if (Days >= 50 && Item_God != 0)
            {
                Pollution += Days / 20;
            }

            // 污染干扰器处理
            if (Item_PollutionJammer)
            {
                if (Pollution >= 5)
                    Pollution -= 5;
                else
                    Pollution = 0;
            }

            // 限制污染值范围
            Pollution = Math.Clamp(Pollution, 0, 9999);
        }

        // 处理低温
        private void ProcessLowTemperature()
        {
            // 温度最低 -50℃
            if (Days >= ExtremeColdComes && Days <= EndOfExtremeCold)
                Temperature -= 3;
            else if (Days >= EndOfExtremeCold && Temperature < 25)
                Temperature += 3;

            // 低温伤害判定
            if (Temperature <= 10)
            {
                if (Temperature <= 10 && Temperature > 0 && Item_WinterClothing == 0)
                {
                    XAddMessage("天气过冷，缺少抗寒衣物！", 0, 0, 0, 0, 0, 0, -10);
                }

                else if (Temperature <= 0 && Temperature > -10 && Item_WinterClothing == 0)
                {
                    XAddMessage("天气过冷，缺少抗寒衣物！！", 0, 0, 0, 0, 0, 0, -50);
                }

                else if (Temperature <= -10 && Temperature > -20 && Item_WinterClothing == 0)
                {
                    XAddMessage("天气过冷，缺少抗寒衣物！！！", 0, 0, 0, 0, 0, 0, -100);
                }
                else if (Temperature <= -10 && Temperature > -20 && Item_WinterClothing == 1)
                {
                    XAddMessage("天气过冷，抗寒衣物级别不足！", 0, 0, 0, 0, 0, 0, -5);
                }

                else if (Temperature <= -20 && Temperature > -30 && Item_WinterClothing == 0)
                {
                    XAddMessage("天气过冷，缺少抗寒衣物！！！", 0, 0, 0, 0, 0, 0, -500);
                }
                else if (Temperature <= -20 && Temperature > -30 && Item_WinterClothing == 1)
                {
                    XAddMessage("天气过冷，抗寒衣物级别不足！！", 0, 0, 0, 0, 0, 0, -20);
                }

                else if (Temperature <= -30 && Temperature > -40 && Item_WinterClothing == 0)
                {
                    XAddMessage("天气过冷，缺少抗寒衣物！！！", 0, 0, 0, 0, 0, 0, -2000);
                }
                else if (Temperature <= -30 && Temperature > -40 && Item_WinterClothing == 1)
                {
                    XAddMessage("天气过冷，抗寒衣物级别不足！！！", 0, 0, 0, 0, 0, 0, -30);
                }

                else if (Temperature <= -50 && Item_WinterClothing < 2)
                {
                    XAddMessage("天气过冷！", 0, 0, 0, 0, 0, 0, -99999);
                }
            }
        }

        // 处理高温
        private void ProcessHighTemperature()
        {
            // 温度最高 +100℃
            if (Days >= ExtremeHotComes && Days <= EndOfExtremeHot)
                Temperature += 3;
            else if (Days >= EndOfExtremeHot && Temperature > 25)
                Temperature -= 3;

            // 高温伤害判定
            if (Temperature >= 40)
            {
                if (Temperature >= 40 && Temperature < 50 && Item_SummerClothing == 0)
                {
                    XAddMessage("天气过热，缺少抗热衣物！", 0, 0, 0, 0, 0, 0, -10);
                }

                else if (Temperature >= 50 && Temperature < 60 && Item_SummerClothing == 0)
                {
                    XAddMessage("天气过热，缺少抗热衣物！！", 0, 0, 0, 0, 0, 0, -50);
                }

                else if (Temperature >= 60 && Temperature < 70 && Item_SummerClothing == 0)
                {
                    XAddMessage("天气过热，缺少抗热衣物！！！", 0, 0, 0, 0, 0, 0, -100);
                }
                else if (Temperature >= 60 && Temperature < 70 && Item_SummerClothing == 1)
                {
                    XAddMessage("天气过热，抗热衣物级别不足！", 0, 0, 0, 0, 0, 0, -5);
                }

                else if (Temperature >= 70 && Temperature < 85 && Item_SummerClothing == 0)
                {
                    XAddMessage("天气过热，缺少抗热衣物！！！", 0, 0, 0, 0, 0, 0, -500);
                }
                else if (Temperature >= 70 && Temperature < 85 && Item_SummerClothing == 1)
                {
                    XAddMessage("天气过热，抗热衣物级别不足！！", 0, 0, 0, 0, 0, 0, -20);
                }

                else if (Temperature >= 85 && Temperature < 100 && Item_SummerClothing == 0)
                {
                    XAddMessage("天气过热，缺少抗热衣物！！！", 0, 0, 0, 0, 0, 0, -2000);
                }
                else if (Temperature >= 85 && Temperature < 100 && Item_SummerClothing == 1)
                {
                    XAddMessage("天气过热，抗热衣物级别不足！！！", 0, 0, 0, 0, 0, 0, -30);
                }

                else if (Temperature >= 100 && Item_SummerClothing < 2)
                {
                    XAddMessage("天气过热！", 0, 0, 0, 0, 0, 0, -99999);
                }
            }
        }

        // 处理命运之骰被动
        private void ProcessGodItem2()
        {
            if (Days % 5 == 0)
            {
                // 命运投掷
                if (Item_God == 2)
                {
                    int tmp = new Random().Next(0, 6);
                    if (tmp == 0)
                    {
                        Hunger = MaxHunger - MaxHunger / 5;
                        XAddMessage("命运投掷：饥饿值设置为上限的 80%");
                    }
                    else if (tmp == 1)
                    {
                        Thirsty = MaxThirsty - MaxThirsty / 5;
                        XAddMessage("命运投掷：口渴值设置为上限的 80%");
                    }
                    else if (tmp == 2)
                    {
                        Mood += 66;
                        XAddMessage("命运投掷：心情值加 66%");
                    }
                    else if (tmp == 3)
                    {
                        San += 66;
                        XAddMessage("命运投掷：理智值加 66%");
                    }
                    else if (tmp == 4)
                    {
                        Gold += 1000;
                        XAddMessage("命运投掷：金钱值加 1000G");
                    }
                    else if (tmp == 5)
                    {
                        WorkExperience += 5;
                        XAddMessage("命运投掷：工作经验加 5%");
                    }
                }
            }
        }

        #endregion





        public void SetWorld(WorldId _World = WorldId.Main)
        {
            if (_World == WorldId.Main)
            {
                // 回到主世界：把副本全部弹掉
                while (Worlds.Count > 1) Worlds.Pop();
            }
            else if (World != _World)
            {
                // 进入副本：主世界数据原封不动留在栈底
                Worlds.Push(new WorldState
                {
                    Id = _World,
                    NowDoing = SceneId.Main,
                    Days = 1,
                    Gold = 3,
                    Health = 10,
                    MaxHealth = 50,
                    Hunger = 30,
                    MaxHunger = 50,
                    Thirsty = 30,
                    MaxThirsty = 50,
                    Temperature = 25,
                    Morning = true,
                });
                Lost += 3;
                YanSuspicion = -60;
            }
            ShowMessage();
        }

        public void EnterScene(SceneId id)
        {
            NowDoing = id;
            if (SceneRegistry.TryGet(id, out var scene))
                scene.Enter();
        }


        // 每日随机事件
        private void DailyRandomEvents()
        {
            const string head = "每日事件：";

            // 特殊世界事件
            if (Days == 9)
            {
                XAddMessage("世界事件：在夜晚，一颗巨大陨石从天而降瞬间将结界粉碎！\n你将不再受到结界的保护，同样不再受到结界的限制！");
                return;
            }
            else if (Days == ExtremeColdComes - 1)
            {
                XAddMessage("世界事件：在夜晚，一颗永冻的陨石从天而降，散发出无穷无尽的寒冷！！");
                return;
            }
            else if (Days == ExtremeHotComes - 1)
            {
                XAddMessage("世界事件：在夜晚，一颗永燃的陨石从天而降，散发出无穷无尽的热量！！");
                return;
            }
            else if (Days == 49 && Item_God != 0)
            {
                XAddMessage("世界事件：整个小镇的空气似乎不再干净了");
                return;
            }

            // 使用幸运值系统决定事件类型
            Random random = new Random();
            int selectedEventType;

            // 强制好事件（如果有）
            if (G_MustGoodRandomEvent > 0)
            {
                selectedEventType = GetGoodEvent(random);
                G_MustGoodRandomEvent--;
            }
            else
            {
                // 根据幸运值计算概率
                selectedEventType = GetRandomEventByLuck(random);
            }

            // 执行事件
            ExecuteEvent(selectedEventType, head);
            //ExecuteEvent(34, head);
        }

        // 根据幸运值获取随机事件
        private int GetRandomEventByLuck(Random random)
        {
            // 基础概率：好事件30%，坏事件20%，平淡事件50%
            float baseGoodProb = 0.3f;
            float baseBadProb = 0.2f;

            // 幸运值影响（每点幸运值影响0.2%概率）
            float luckEffect = LuckValue * 0.002f;

            // 调整概率
            float goodProb = Math.Clamp(baseGoodProb + luckEffect, 0.1f, 0.6f);
            float badProb = Math.Clamp(baseBadProb - luckEffect, 0.1f, 0.6f);
            float neutralProb = 1.0f - goodProb - badProb;

            // 重新平衡概率
            float total = goodProb + badProb + neutralProb;
            goodProb /= total;
            badProb /= total;

            // 根据概率随机选择事件类型
            float randValue = (float)random.NextDouble();

            if (randValue < goodProb) 
                return GetGoodEvent(random);

            else if (randValue < goodProb + badProb) 
                return GetBadEvent(random);

            else return GetNeutralEvent(random);
        }

        // 执行事件
        private void ExecuteEvent(int eventType, string head)
        {
            switch (eventType)
            {
                case 0:
                    XAddMessage(head + "深夜一颗流星从天空划过，可惜你在睡觉没看见");
                    break;
                case 1:
                    XAddMessage(head + "夜晚有几只乌鸦在你门口拉屎，让你很无语", 0, 0, 0, -1);
                    break;
                case 2:
                    XAddMessage(head + "你梦到了你在吃饺子，醒来之后发现是梦", 0, 0, 0, -1);
                    break;
                case 3:
                    XAddMessage(head + "深夜一颗流星从天空划过，被你正巧看见", 0, 0, 2, 10);
                    break;
                case 4:
                    XAddMessage(head + "昨夜大半夜由数百人一起炸街", 0, 0, -2, -10);
                    break;
                case 5:
                    XAddMessage(head + "做梦梦到有人骂你", 0, 0, -2, -5);
                    break;
                case 6:
                    XAddMessage(head + "谎如昨日，嗤笑今朝");
                    break;
                case 7:
                    XAddMessage(head + "你在回家的路上捡到到了钱！", 0, 0, -1, 10, 50);
                    break;
                case 8:
                    XAddMessage(head + "你被诈骗电话诈骗了！", 0, 0, 1, -15, -100);
                    break;
                case 9:
                    XAddMessage(head + "去迷雾森林漫步，捡到了一大颗水晶！", 0, 0, 0, 10, 250);
                    break;
                case 10:
                    XAddMessage(head + "家里灯泡突然炸了！", 0, 0, 0, -5, -20);
                    break;
                case 11:
                    XAddMessage(head + "天气有点小冷，看见霜叶红于二月花，心情大好", 0, 0, 0, 10);
                    break;
                case 12:
                    XAddMessage(head + "路上被 ? 揍了一拳，心情巨差", 0, 0, 0, -20);
                    break;
                case 13:
                    XAddMessage(head + $"你睡觉时陷入了时间跳跃！！！\n跳过 {(Days < 100 ? 3 : Days - 3)} 天");
                    Days += Days < 100 ? 3 : Days - 3;
                    break;
                case 14:
                    XAddMessage(head + $"你睡觉时陷入了时间回溯！！！\n回退 {(Days > 3 ? 3 : 3 - Days)} 天");
                    Days -= Days > 3 ? 3 : 3 - Days;
                    break;
                case 15:
                    XAddMessage(head + "喜欢提携后辈的\"小镇做题家\"张九龄，给你写了一首诗！", 0, 0, 0, 20);
                    break;

                case 16: case 17: case 18: case 19: case 20: case 21:
                    XAddMessage(head + "平平淡淡的一晚。");
                    break;

                case 22:
                    XAddMessage(head + "你看了舞剧《李白》，还和主创合影，感动又开心。", 0, 0, 5, 15);
                    break;

                case 23:
                    XAddMessage(head + "你称霸武林，笑傲江湖，大笑中忽然被门槛绊倒……原来是一场梦", 0, 0, 0, -2);
                    break;

                case 24:
                    XAddMessage(head + "大街小巷响起了琼瑶的《一帘幽梦》主题曲，你听了十分伤怀。", 0, 0, 2, -2);
                    break;

                case 25:
                    XAddMessage(head + "你扮演鬼屋的NPC，把客人吓得半死。", 0, 0, 0, 5);
                    break;

                case 26:
                    XAddMessage(head + "你手持三尺青锋，随意挽了几个剑花，赢得路人喝彩。", 0, 0, 0, 5, 25);
                    break;
                case 27:
                    if (Days >= 10 && (trackBarDifficulty.Value == 1 || trackBarDifficulty.Value == 2) || Days >= 50)
                    {
                        if (Item_Antidote)
                        {
                            XAddMessage(head + "你误食红伞伞白杆杆，幸亏急忙打了一针解毒剂才安然无恙。", 0, 0, -10, -5, 0, 0, -3);
                            Item_Antidote = false;
                        }
                        else
                        {
                            XAddMessage(head + "你误食红伞伞白杆杆，险些命丧黄泉。", 0, 0, -25, -5, 0, 0, -50);
                        }
                    }
                    else
                    {
                        XAddMessage(head + "平平淡淡的一晚。");
                    }
                    break;
                case 28:
                    XAddMessage(head + "你有能力说\"我不知道\"的头脑，正处于可以发现\n任何事物的状态，恭喜你！", 0, 0, 50);
                    break;
                case 29:
                    XAddMessage(head + "隔壁邻居新养了一只猫，你撸猫撸得不亦乐乎", 0, 0, 1, 5, -10);
                    break;
                case 30:
                    XAddMessage(head + "你撸了别人家的猫，自家的猫心生嫉妒，给了你一爪子，\n冷艳高贵地走开了。", 0, 0, -1, -5);
                    break;
                case 31:
                    if (Days >= 10 && (trackBarDifficulty.Value == 1 || trackBarDifficulty.Value == 2) || Days >= 50)
                    {
                        if (Item_LuckyCharm)
                        {
                            XAddMessage(head + "你走在路上，差点被一辆疾驰而过的汽车撞到，手中的幸运符化为的飞灰。", 0, 0, -1, -1);
                            Item_LuckyCharm = false;
                        }
                        else
                        {
                            XAddMessage(head + "你走在路上，被一辆疾驰而过的汽车撞到，生死未卜。", 0, 0, 0, 0, 600, 0, -51);
                        }
                    }
                    else
                    {
                        XAddMessage(head + "平平淡淡的一晚。");
                    }
                    break;
                case 32:
                    XAddMessage(head + "有人说：生命的价值不在于有没有用，生命存在本身就是价值。\n你听了豁然开朗。", 0, 0, 20, 20);
                    break;
                case 33:
                    XAddMessage(head + "你看着左右摇摆的钟摆，陷入了沉思。");
                    break;
                case 34: case 35:
                    var foundItems = new List<string>();

                    // ========== 1. 基础消耗品（0~2） ==========
                    int water = random.Next(0, 3);
                    Item_BottledWater += water;
                    if (water > 0) foundItems.Add($"{water}瓶矿泉水");

                    int beverage = random.Next(0, 3);
                    Item_BottledBeverage += beverage;
                    if (beverage > 0) foundItems.Add($"{beverage}瓶瓶装饮料");

                    int biscuit = random.Next(0, 3);
                    Item_CompressedBiscuit += biscuit;
                    if (biscuit > 0) foundItems.Add($"{biscuit}包压缩饼干");

                    // ========== 2. 基础金钱（50% 概率，200~2000 + Luck 加成） ==========
                    int gold = 0;
                    if (random.Next(0, 2) == 1)
                    {
                        gold = random.Next(200 + Luck * 30, 3000 + Luck * 60);
                    }

                    // ========== 3. 特殊物品：四个布尔物品（20% 概率，必得一个未拥有的，否则给 500~1000 金钱） ==========
                    if (random.Next(0, 100) < 20)   // 20% 概率触发
                    {
                        var specialItems = new (bool flag, string name, Action setTrue)[]
                        {
                            (Item_FirstAidKit, "急救包", () => Item_FirstAidKit = true),
                            (Item_Antidote, "解毒剂", () => Item_Antidote = true),
                            (Item_LuckyCharm, "幸运符", () => Item_LuckyCharm = true),
                            (Item_SoothingMedicine, "舒缓药", () => Item_SoothingMedicine = true)
                        };

                        var missing = specialItems.Where(item => !item.flag).ToList();

                        if (missing.Any())
                        {
                            var chosen = missing[random.Next(0, missing.Count)];
                            chosen.setTrue();
                            foundItems.Add(chosen.name);
                        }
                        else
                        {
                            int extraGold = random.Next(500, 1001);
                            gold += extraGold;
                        }
                    }

                    // ========== 4. 御寒服 & 御焰服（20% 概率，按天数决定优先级） ==========
                    if (random.Next(0, 100) < 20)   // 20% 概率触发衣服事件
                    {
                        if (Days < EndOfExtremeCold)
                        {
                            // 优先给御寒服
                            if (Item_WinterClothing == 0)
                            {
                                Item_WinterClothing = 1;
                                foundItems.Add("御寒服");
                            }

                            else if (Item_SummerClothing == 0)
                            {
                                Item_SummerClothing = 1;
                                foundItems.Add("御焰服");
                            }

                            else
                            {
                                gold += random.Next(100, 301);
                            }
                        }

                        else
                        {
                            // 优先给御焰服
                            if (Item_SummerClothing == 0)
                            {
                                Item_SummerClothing = 1;
                                foundItems.Add("御焰服");
                            }

                            else if (Item_WinterClothing == 0)
                            {
                                Item_WinterClothing = 1;
                                foundItems.Add("御寒服");
                            }

                            else
                            {
                                gold += random.Next(100, 301);
                            }
                        }
                    }

                    // ========== 5. 将最终金币加入列表 ==========
                    if (gold > 0)
                    {
                        foundItems.Add($"{gold}块钱");
                    }

                    // ========== 6. 自然连接并输出消息 ==========
                    string itemList = foundItems.Count switch
                    {
                        0 => "……什么也没有！！",
                        1 => foundItems[0],
                        _ => string.Join("、", foundItems.Take(foundItems.Count - 1)) + "和" + foundItems.Last()
                    };

                    XAddMessage(head + "你在路边找到一个宝箱！你在里面找到了\n" + itemList + "！",
                        0, 0,
                        (water + beverage + biscuit) * 10 + gold / 150,
                        (water + beverage + biscuit) * 7 + gold / 300,
                        gold, 0, 0, 0
                    );
                    break;
            }
        }



        // 死亡判定
        public bool DeathJudgment()
        {
            // 调试模式禁用死亡判定
            if (checkBoxDeBug_NoDied.Checked)
                return true;

            // 避死BUFF禁用死亡判定
            else if (G_AvoidingDeath > 0)
                return true;

            string DeathText = string.Empty;
            string NotDeadType = string.Empty;
            bool NotDead = false;

            // 欺诈面具被动
            if (Item_God == 1 && !NotDead)
            {
                NotDead = random.Next(0, 5) == 0;
                if (NotDead) NotDeadType = "God_1";
            }




            // 撑死
            if (Hunger > MaxHunger)
            {
                DeathText = "您撑死了。";
                if (NotDead) Hunger = 50;
            }

            // 饿死
            else if (Hunger <= 0)
            {
                DeathText = "您饿死了。";
                if (NotDead) Hunger = 50;
            }

            // 胀死
            else if (Thirsty > MaxThirsty)
            {
                DeathText = "您胀死了。";
                if (NotDead) Thirsty = 50;
            }

            // 渴死
            else if (Thirsty < 0)
            {
                DeathText = "您渴死了。";
                if (NotDead) Thirsty = 50;
            }

            // 死亡
            else if (Health < 0)
            {
                DeathText = "您死亡了。";
                if (NotDead) Health = 50;
                else if (Item_FirstAidKit) 
                { 
                    Health = 50; 
                    Item_FirstAidKit = false; 
                    NotDeadType = "FirstAidKit"; 
                    NotDead = true;
                }
            }

            // 理智过低疯了而死
            else if (San <= 0)
            {
                DeathText = "您疯了，从十五楼一跃而下。。";
                if (NotDead) San = 50;
            }

            // 快乐死了
            else if (Mood > 200)
            {
                DeathText = "您乐疯了，一下子咽气了。";
                if (NotDead) Mood = 50;

                else if (Item_SoothingMedicine) 
                {
                    Mood = 50; 
                    Item_SoothingMedicine = false; 
                    NotDeadType = "SoothingMedicine"; 
                    NotDead = true; 
                }
            }

            // 污染过高死亡
            else if (Pollution >= 100)
            {
                DeathText = "您变异了，当成脑死亡。";
                if (NotDead) Pollution = 50;
            }

            // 皇后：快乐的消失
            else if (Mood <= 0 && comboBoxTarotCard.Text == "皇后")
            {
                DeathText = "您心情极差，一下子咽气了。";
                if (NotDead) Mood = 50;
            }






            // 死亡执行
            if (DeathText != string.Empty)
            {
                // 停止打字机效果，确保死亡信息能完整显示
                StopTypeWriterEffect();

                // 欺诈被动：欺骗死亡
                if (NotDead)
                {
                    switch (NotDeadType)
                    {
                        case "God_1":
                            AddMessage($"{DeathText}但你骗过了死亡！");
                            break;

                        case "FirstAidKit":
                            AddMessage($"你健康值过低，即时使用急救包救了你一命！");
                            break;

                        case "SoothingMedicine":
                            AddMessage($"你情绪异常，即时使用舒缓药救了你一命！");
                            break;
                    }
                }
                else
                {
                    AddMessage($"{DeathText}");

                    // 非主事件死亡触发回溯
                    if (World != WorldId.Main)
                    {
                        SetWorld(World);
                        return true;
                    }
                    
                    GameOver();
                    return false;
                }
            }


            return true;
        }

        // 重置
        private void ReSet()
        {
            buttonReSet.Visible = false;

            // 重建状态
            Worlds.Clear();
            Global = new GlobalState();
            Worlds.Push(new WorldState());   // 主世界默认值

            // 配置重新随机
            ExtremeColdComes = random.Next(15, 30); EndOfExtremeCold = ExtremeColdComes + random.Next(15, 30);
            ExtremeHotComes = EndOfExtremeCold + random.Next(20, 30); EndOfExtremeHot = ExtremeHotComes + random.Next(15, 30);

            buttonHome.Enabled = true; 
            checkBoxMessageLog.Checked = false;
            comboBoxTarotCard.Enabled = true; 
            trackBarDifficulty.Enabled = true;
            labelMessage.Text = "你重生了，重生在事故发生前……"; MessageLog = "你出生了......";

            // 难度数值更新
            trackBarDifficulty_Scroll(null, null);

            // 调试模式
            if (DeBugMode)
            {
                Gold = 999999999; // San = 999999999;
                Days = 11;
            }

            // 控件显示更新
            TC_Show();
            ShowMessage();
            SetButtonTexts();
            ReSetButtonEnableds();
        }

        // 难度
        private void trackBarDifficulty_Scroll(object sender, EventArgs e)
        {
            if (trackBarDifficulty.Value == 0)
            {
                BasicSalary = 80; BaseLuck = 20;
                AddWorkExperienceMax = 15; Friend = 2;
                Food_Dumpling = 40; Food_RouJiaMo = 35; Food_RoastedCorn = 5; Food_GluttonousFeast = 500;
                Food_Lemonade = 5; Food_Coffee = 15; Food_PurificationPackage = 500;
                Natural_San = 2; Natural_Mood = 5; Natural_Health = 0; Natural_Hunger = 5; Natural_Thirsty = 5;
            }
            else if (trackBarDifficulty.Value == 1)
            {
                BasicSalary = 90; BaseLuck = 0;
                AddWorkExperienceMax = 12; Friend = 1;
                Food_Dumpling = 50; Food_RouJiaMo = 40; Food_RoastedCorn = 10; Food_GluttonousFeast = 1000;
                Food_Lemonade = 10; Food_Coffee = 30; Food_PurificationPackage = 1000;
                Natural_San = 5; Natural_Mood = 7; Natural_Health = 1; Natural_Hunger = 10; Natural_Thirsty = 10;
            }
            else
            {
                BasicSalary = 100; BaseLuck = -20;
                AddWorkExperienceMax = 12; Friend = 0;
                Food_Dumpling = 65; Food_RouJiaMo = 55; Food_RoastedCorn = 15; Food_GluttonousFeast = 3000;
                Food_Lemonade = 20; Food_Coffee = 50; Food_PurificationPackage = 1500;
                Natural_San = 7; Natural_Mood = 10; Natural_Health = 2; Natural_Hunger = 15; Natural_Thirsty = 15;
            }
            CalculateLucky();
            ShowMessage();
        }



        // 下一页
        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (SceneRegistry.TryGet(NowDoing, out var scene))
                scene.OnNext();
        }

        // 上一页
        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (SceneRegistry.TryGet(NowDoing, out var scene))
                scene.OnBack();
        }



        #region 基础函数

        // 信息文本核心
        private string MessageMain(string message, int hunger = 0, int thirsty = 0, int san = 0, int mood = 0, int gold = 0, int workExperience = 0, int health = 0, int pollution = 0, int lost = 0)
        {
            var sb = new StringBuilder(SetMessageTextTemporarilyStored);
            sb.Append(message);

            // 存在数值改变
            bool hasChanges = hunger != 0 || thirsty != 0 || san != 0 || mood != 0 ||
                              gold != 0 || workExperience != 0 || health != 0 ||
                              pollution != 0 || lost != 0;

            if (hasChanges)
            {
                sb.Append('\n');

                AppendChange(sb, "饥饿值", hunger);
                AppendChange(sb, "口渴值", thirsty);
                AppendChange(sb, "理智", san, "%");
                AppendChange(sb, "心情", mood, "%");
                AppendChange(sb, "健康", health, "%");
                AppendChange(sb, "金币", gold, "G");
                AppendChange(sb, "工作经验", workExperience, "%");
                AppendChange(sb, "污染", pollution, "%");
                AppendChange(sb, "迷失", lost, "%");

                // 去掉最后一个多余空格
                if (sb.Length > 0 && sb[sb.Length - 1] == ' ')
                    sb.Length--;
            }

            SetMessageTextTemporarilyStored = string.Empty;
            return sb.ToString();
        }

        private static void AppendChange(StringBuilder sb, string label, int value, string suffix = "")
        {
            if (value == 0)
                return;

            sb.Append(label)
              .Append(value > 0 ? " + " : " - ")
              .Append(Math.Abs(value))
              .Append(suffix)
              .Append(' ');
        }

        // 添加信息
        public void AddMessage(string message, int hunger = 0, int thirsty = 0, int san = 0, int mood = 0, int gold = 0, int workExperience = 0, int health = 0, int pollution = 0, int lost = 0)
        {
            string Str = MessageMain(message, hunger, thirsty, san, mood, gold, workExperience, health, pollution, lost);

            // 停止之前的打字机效果
            StopTypeWriterEffect();

            if (!checkBoxMessageLog.Checked)
            {
                // 对于 AddMessage，我们追加到现有文本
                string newText = string.IsNullOrEmpty(labelMessage.Text) ? Str : '\n' + Str;
                _currentDisplayText = labelMessage.Text + newText;

                // 使用打字机效果显示文本
                TypeWriterEffect(labelMessage, newText, true);
            }
            else
            {
                labelMessage.Text += '\n' + Str;
                ScrollToBottom();
            }
            MessageLog += '\n' + Str;
        }

        // 设置信息
        public void SetMessage(string message, int hunger = 0, int thirsty = 0, int san = 0, int mood = 0, int gold = 0, int workExperience = 0, int health = 0, int pollution = 0, int lost = 0)
        {
            string Str = MessageMain(message, hunger, thirsty, san, mood, gold, workExperience, health, pollution, lost);

            // 停止之前的打字机效果
            StopTypeWriterEffect();

            if (!checkBoxMessageLog.Checked)
            {
                // 对于 SetMessage，替换整个文本内容
                _currentDisplayText = Str;
                labelMessage.Text = string.Empty;

                // 使用打字机效果显示文本
                TypeWriterEffect(labelMessage, Str);
            }
            else
            {
                labelMessage.Text = Str;
                ScrollToBottom();
            }
            MessageLog += '\n' + Str;
        }

        // 打字机效果实现
        private async void TypeWriterEffect(System.Windows.Forms.Label label, string text, bool isAppend = false, int delayMilliseconds = 20)
        {
            // 如果当前有正在进行的打字机效果，先停止它
            if (_typeWriterCts != null)
            {
                _typeWriterCts.Cancel();
            }

            _typeWriterCts = new CancellationTokenSource();
            var cancellationToken = _typeWriterCts.Token;

            string originalText = isAppend ? label.Text : string.Empty;

            try
            {
                // 逐个字符显示
                for (int i = 0; i <= text.Length; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    label.Text = originalText + text.Substring(0, i);

                    // 每次更新文本后滚动到底部
                    ScrollToBottom();

                    await Task.Delay(delayMilliseconds, cancellationToken);
                }

                // 更新当前显示文本
                _currentDisplayText = label.Text;

                // 后滚动到底部
                ScrollToBottom();
            }
            catch (TaskCanceledException)
            {
                // 任务被取消是正常情况，忽略这个异常
            }
        }

        // 添加一个取消令牌源用于控制打字机效果
        private CancellationTokenSource _typeWriterCts;

        // 添加字段来跟踪当前显示的文本
        private string _currentDisplayText = string.Empty;

        // 停止当前的打字机效果
        private void StopTypeWriterEffect()
        {
            _typeWriterCts?.Cancel();
            _typeWriterCts?.Dispose();
            _typeWriterCts = null;

            // 确保标签显示完整的当前文本
            if (!string.IsNullOrEmpty(_currentDisplayText))
            {
                labelMessage.Text = _currentDisplayText;
            }
        }

        // 设置信息 & 属性加减生效
        public void XSetMessage(string message, int hunger = 0, int thirsty = 0, int san = 0, int mood = 0, int gold = 0, int workExperience = 0, int health = 0, int pollution = 0, int lost = 0)
        {
            MessageMakeData(ref hunger, ref thirsty, ref san, ref mood, ref gold, ref workExperience, ref health, ref pollution, ref lost);
            SetMessage(message, hunger, thirsty, san, mood, gold, workExperience, health, pollution, lost);
            MessageSetData(hunger, thirsty, san, mood, gold, workExperience, health, pollution, lost);
        }

        // 添加信息 & 属性加减生效
        public void XAddMessage(string message, int hunger = 0, int thirsty = 0, int san = 0, int mood = 0, int gold = 0, int workExperience = 0, int health = 0, int pollution = 0, int lost = 0)
        {
            MessageMakeData(ref hunger, ref thirsty, ref san, ref mood, ref gold, ref workExperience, ref health, ref pollution, ref lost);
            AddMessage(message, hunger, thirsty, san, mood, gold, workExperience, health, pollution, lost);
            MessageSetData(hunger, thirsty, san, mood, gold, workExperience, health, pollution, lost);
        }

        public void MessageSetData(int hunger, int thirsty, int san, int mood, int gold, int workExperience, int health, int pollution, int lost = 0)
        {
            Hunger += hunger;
            Thirsty += thirsty;
            San += san;
            Mood += mood;
            Gold += gold;
            Pollution += pollution;
            WorkExperience += workExperience;
            Lost += lost;

            // 工作经验上限
            if (WorkExperience > 150 && comboBoxTarotCard.Text != "月亮") WorkExperience = 150;
            else if (WorkExperience > 100 && comboBoxTarotCard.Text == "月亮") WorkExperience = 100;

            // 生命值
            Health += health; if (Health > MaxHealth) Health = MaxHealth;
        }

        // 数据加成
        public void MessageMakeData(ref int hunger, ref int thirsty, ref int san, ref int mood, ref int gold, ref int workExperience, ref int health, ref int pollution, ref int lost)
        {
            // San 获取公式
            san *= (100 - pollution) / 100;

            // 秩序数值翻倍
            if (G_DoubleTheValue > 0)
            {
                hunger *= 2;
                thirsty *= 2;
                san *= 2;
                mood *= 2;
                gold *= 2;
                workExperience *= 2;
                health *= 2;
                pollution *= 2;
                lost *= 2;
            }

            if (DeBug_2Gold)
                gold *= 2;

            MessageTarotCard(ref hunger, ref thirsty, ref san, ref mood, ref gold, ref workExperience, ref health, ref pollution, ref lost);
        }

        // 判断事件类型的方法
        public bool IsGoodEvent(int eventType)
        {
            int[] goodEvents = { 3, 7, 9, 11, 15, 22, 25, 26, 28, 32 };
            return goodEvents.Contains(eventType);
        }

        public bool IsBadEvent(int eventType)
        {
            int[] badEvents = { 1, 2, 4, 5, 8, 10, 12, 27, 30, 31 };
            return badEvents.Contains(eventType);
        }

        // 调整幸运值
        public void AdjustLuck(int amount, string reason = "")
        {
            int oldLuck = LuckValue;
            LuckValue += amount;

            if (!string.IsNullOrEmpty(reason))
            {
                XAddMessage($"{reason} 幸运值: {oldLuck + 100} -> {DisplayLuckValue}");
            }
        }

        // 获取好事件
        public int GetGoodEvent(Random random)
        {
            int[] goodEvents = { 3, 7, 9, 11, 15, 22, 25, 26, 28, 32, 34, 35 };
            return goodEvents[random.Next(goodEvents.Length)];
        }

        // 获取坏事件
        private int GetBadEvent(Random random)
        {
            int[] badEvents = { 1, 2, 4, 5, 8, 10, 12, 27, 30, 31 };
            return badEvents[random.Next(badEvents.Length)];
        }

        // 获取平淡事件
        private int GetNeutralEvent(Random random)
        {
            int[] neutralEvents = { 0, 6, 13, 14, 16, 17, 18, 19, 20, 21, 23, 24, 29, 33 };
            return neutralEvents[random.Next(neutralEvents.Length)];
        }

        // 数据塔罗牌加成
        private void MessageTarotCard(ref int hunger, ref int thirsty, ref int san, ref int mood, ref int gold, ref int workExperience, ref int health, ref int pollution, ref int lost)
        {
            if (comboBoxTarotCard.Text != string.Empty)
            {
                // 愚者：勇往直前
                if (comboBoxTarotCard.Text == "愚者" && san > 0) san *= 2;

                // 魔术师
                else if (comboBoxTarotCard.Text == "魔术师")
                {
                    // 炉火纯青的技艺
                    if (workExperience > 0) workExperience *= 2;
                    // 意志浅薄
                    if (san < 0) san *= 2;
                }

                // 女祭司
                else if (comboBoxTarotCard.Text == "女祭司")
                {
                    // 知性理智
                    if (san > 0) san *= 2;

                    //无知贪婪
                    if (workExperience > 0) workExperience /= 2;
                }
            }
        }

        // 添加开发者日志
        private void AddLog(string message, bool OutMessages = false)
        {
            if (DeBugMode)
            {
                Log.Info($"[SurvivalChallengeGame] {message}");
            }
        }

        // 滚动到底部的方法
        private void ScrollToBottom()
        {
            // 确保在UI线程上执行
            if (splitContainer1.Panel2.InvokeRequired)
            {
                splitContainer1.Panel2.Invoke(new Action(ScrollToBottom));
                return;
            }

            splitContainer1.Panel2.VerticalScroll.Value = splitContainer1.Panel2.VerticalScroll.Maximum;
            splitContainer1.Panel2.PerformLayout();

            // 强制刷新
            splitContainer1.Panel2.Refresh();
        }

        // 上下页按钮启用
        public void NextBackEnable(bool enab = true)
        {
            buttonNext.Enabled = enab;
            buttonBack.Enabled = enab;
        }

        // 按钮字体大小编辑启用
        public void SetButtonOptionFont(Single FontSize0 = 33F, Single FontSize1 = 33F, Single FontSize2 = 33F, Single FontSize3 = 33F, Single FontSize4 = 33F)
        {
            buttonOption0.Font = new System.Drawing.Font("Microsoft Sans Serif", FontSize0, FontStyle.Bold);
            buttonOption1.Font = new System.Drawing.Font("Microsoft Sans Serif", FontSize1, FontStyle.Bold);
            buttonOption2.Font = new System.Drawing.Font("Microsoft Sans Serif", FontSize2, FontStyle.Bold);
            buttonOption3.Font = new System.Drawing.Font("Microsoft Sans Serif", FontSize3, FontStyle.Bold);
            buttonOption4.Font = new System.Drawing.Font("Microsoft Sans Serif", FontSize4, FontStyle.Bold);
        }

        // 设置按钮文本
        public void SetButtonTexts(string bt0 = "去挣钱", string bt1 = "美食街", string bt2 = "知识区", string bt3 = "出去玩", string bt4 = "去冒险")
        {
            buttonOption0.Text = bt0;
            buttonOption1.Text = bt1;
            buttonOption2.Text = bt2;
            buttonOption3.Text = bt3;
            buttonOption4.Text = bt4;
        }

        // 设置按钮启用
        public void SetButtonEnableds(bool bt0 = true, bool bt1 = true, bool bt2 = true, bool bt3 = true, bool bt4 = true)
        {
            buttonOption0.Enabled = bt0;
            buttonOption1.Enabled = bt1;
            buttonOption2.Enabled = bt2;
            buttonOption3.Enabled = bt3;
            buttonOption4.Enabled = bt4;

            SetButtonEnabledsColor(bt0, bt1, bt2, bt3, bt4);
        }

        // 设置按钮启用颜色
        public void SetButtonEnabledsColor(bool bt0, bool bt1, bool bt2, bool bt3, bool bt4)
        {
            buttonOption0.BackColor = bt0 ? Color.White : Color.Gainsboro;
            buttonOption1.BackColor = bt1 ? Color.White : Color.Gainsboro;
            buttonOption2.BackColor = bt2 ? Color.White : Color.Gainsboro;
            buttonOption3.BackColor = bt3 ? Color.White : Color.Gainsboro;
            buttonOption4.BackColor = bt4 ? Color.White : Color.Gainsboro;
        }

        // 塔罗牌保存
        public void TC_Save()
        {
            Directory.CreateDirectory(@"C:\QiAppDatas\Datas\QisToolkit3\");
            using StreamWriter writer = new StreamWriter(@"C:\QiAppDatas\Datas\QisToolkit3\SurvivalChallengeGame.qidata");
            for (int i = 0; i < 22; ++i)
                writer.WriteLine(TC[i].ToString());
        }

        // 塔罗牌读取
        private void TC_Read()
        {
            Directory.CreateDirectory(@"C:\QiAppDatas\Datas\QisToolkit3");
            if (File.Exists(@"C:\QiAppDatas\Datas\QisToolkit3\SurvivalChallengeGame.qidata"))
            {
                using StreamReader reader = new StreamReader(@"C:\QiAppDatas\Datas\QisToolkit3\SurvivalChallengeGame.qidata");
                string line;
                for (int i = 0; i < 22; ++i)
                    if ((line = reader.ReadLine()) != null)
                        TC[i] = Qi.StrToBool(line);
            }
        }

        // 塔罗牌展示
        private void TC_Show()
        {
            comboBoxTarotCard.Items.Clear();

            string[] tarotCards = {
                "愚者", "魔术师", "女祭司", "皇后", "皇帝", "教皇", "恋人", "战车", "力量", "隐士",
                "命运之轮", "正义", "倒吊人", "死神", "节制", "恶魔", "高塔", "星辰", "月亮", "太阳",
                "审判", "世界"
            };

            // 添加到 Items
            for (int i = 0; i < tarotCards.Length; i++)
                if (TC[i] || DeBugMode)
                    comboBoxTarotCard.Items.Add(tarotCards[i]);
        }

        // 游戏结束
        public void GameOver()
        {
            buttonOption0.Enabled = false;
            buttonOption1.Enabled = false;
            buttonOption2.Enabled = false;
            buttonOption3.Enabled = false;
            buttonOption4.Enabled = false;
            buttonBack.Enabled = false;
            buttonNext.Enabled = false;
            buttonHome.Enabled = false;
            buttonReSet.Visible = true;

            try
            {
                // 导出日志
                Directory.CreateDirectory(Path.Combine(Qi.QisToolkit3_Datas.actualDirectory, "Logs"));
                File.WriteAllText(Path.Combine(Qi.QisToolkit3_Datas.actualDirectory, "Logs", "问身问己问心.log"), MessageLog);

                // 开发者模式自动打开日志文件
                if (DeBugMode)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = Path.Combine(Qi.QisToolkit3_Datas.actualDirectory, "Logs", "问身问己问心.log"),
                        UseShellExecute = true  // 显式启用 ShellExecute
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出日志出现错误，请联系开发人员。\n报错信息：{ex.Message}\n\n完整报错：\n{ex}", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Err("[SurvivalChallengeGame] 导出日志出现错误！", ex);
            }
        }

        #endregion


        #region 物品功能

        // 黑暗屏蔽器
        private void labelDarkBlocker_Click(object sender, EventArgs e) =>
            SetMessage("“驱散黑暗，逐见深渊”\n此道具可以屏蔽大部分因为认知干扰而无法读的文字！");

        // 御寒服 & 太阳·御寒服
        private void labelWinterClothing_Click(object sender, EventArgs e)
        {
            if (Item_WinterClothing == 1)
                SetMessage("此道具可抵挡低温，但无法抵挡低于 -10℃ 的低温");
            else
                SetMessage("此道具可抵挡任意低温");
        }

        // 御焰服 & 月亮·御焰服
        private void labelSummerClothing_Click(object sender, EventArgs e)
        {
            if (Item_SummerClothing == 1)
                SetMessage("此道具可抵挡高温，但无法抵挡高于 60℃ 的高温");
            else
                SetMessage("此道具可抵挡任意高温");
        }

        // 神赐道具
        private void labelGodItem_Click(object sender, EventArgs e)
        {
            switch (Item_God)
            {
                // 欺诈
                case 1:
                    if (Item_EssenceReason >= 1)
                    {
                        SetMessage("你带上了那块面具，附着在上面的理智精华顿时沸腾起来！");
                        AddMessage("无数的无形的不可描述的能力钻入你的身体！");
                        AddMessage("下一刻你感觉自己的一切都变了！");
                        AddMessage("消耗了 理智精华 × 1，污染 + 25，欺骗了自己！身体属性重置。");
                        Gold = 50; San = 80; Mood = 55; Health = 100; Hunger = 60; MaxHunger = 100;
                        Thirsty = 60; MaxThirsty = 100; WorkExperience = 0; Temperature = 25; Friend = 1;
                        --Item_EssenceReason; Pollution += 25;
                    }
                    else
                    {
                        SetMessage("你带上了那块面具，什么事情都没发生……");
                        AddMessage("似乎缺失了某种激活它的东西……");
                    }
                    break;

                // 命运
                case 2:
                    if (Item_EssenceReason >= 1)
                    {
                        SetMessage("你抛起了那块骰子，附着在上面的理智精华顿时沸腾起来！");
                        AddMessage("无数的无形的不可描述的能力直冲天地！");
                        AddMessage("下一刻你感觉这世界一切都变了！");
                        AddMessage("消耗了 理智精华 × 1，污染 + 25，强行编写了命运！（三天随机事件必是好的）");
                        --Item_EssenceReason; G_MustGoodRandomEvent = 3; Pollution += 25;
                    }
                    else
                    {
                        SetMessage("你带上了那块面具，什么事情都没发生……");
                        AddMessage("似乎缺失了某种激活它的东西……");
                    }
                    break;

                // 死亡
                case 3:
                    if (Item_EssenceReason >= 1)
                    {
                        SetMessage("你举起了手中的镰刀，附着在上面的理智精华顿时沸腾起来！");
                        AddMessage("无数的无形的不可描述的能力钻入你的身体！");
                        AddMessage("下一刻仿佛无法感受到自己的生命力！");
                        AddMessage("消耗了 理智精华 × 1，污染 + 25，使你进入了避死状态持续三天！");
                        --Item_EssenceReason; G_AvoidingDeath = 3; Pollution += 25;
                    }
                    else
                    {
                        SetMessage("你举起了手中的镰刀，什么事情都没发生……");
                        AddMessage("似乎缺失了某种激活它的东西……");
                    }
                    break;

                // 秩序
                case 4:
                    if (Item_EssenceReason >= 1)
                    {
                        SetMessage("你手中的权杖重重的撑向地面，附着在上面的理智精华顿时沸腾起来！");
                        AddMessage("无数的无形的不可描述的能力扩散入附近的时空！");
                        AddMessage("整个时空仿佛变得不可思议了起来！");
                        AddMessage("消耗了 理智精华 × 1，污染 + 25，使你的一切数值获取翻倍！");
                        --Item_EssenceReason; G_DoubleTheValue = 3; Pollution += 25;
                    }
                    else
                    {
                        SetMessage("你手中的权杖重重的撑向地面，什么事情都没发生……");
                        AddMessage("似乎缺失了某种激活它的东西……");
                    }
                    break;

                // 记忆
                case 5:
                    if (Item_EssenceReason >= 1)
                    {
                        SetMessage("你翻开了手中的书本，附着在上面的理智精华顿时沸腾起来！");
                        AddMessage("无数的无形的不可描述的能力钻入你的身体！");
                        AddMessage("下一刻你赶紧似乎置身于历史之中，你感觉自己从来都没有这么清醒过！");
                        AddMessage("消耗了 理智精华 × 1，污染 + 25，使你进入了绝对理智状态持续三天！");
                        --Item_EssenceReason; G_AbsoluteRationality = 3; Pollution += 25; San = 99999;
                    }
                    else
                    {
                        SetMessage("你翻开了手中的书本，什么事情都没发生……");
                        AddMessage("似乎缺失了某种激活它的东西……");
                    }
                    break;
            }
            ShowMessage();
        }

        // 压缩饼干
        private void labelCompressedBiscuit_Click(object sender, EventArgs e)
        {
            if (Item_CompressedBiscuit <= 0)
                return;

            Item_CompressedBiscuit -= 1;
            AddMessage("你吃了一块压缩饼干。", 60, -10, -1, -5, 0, 0, -3);
            ShowMessage(); DeathJudgment();
        }

        // 瓶装饮料
        private void labelBottledBeverage_Click(object sender, EventArgs e)
        {
            if (Item_BottledBeverage <= 0)
                return;

            Item_BottledBeverage -= 1;
            AddMessage("你喝一瓶饮料。", 10, 35, -5, 40, 0, 0, -10);
            ShowMessage(); DeathJudgment();
        }

        // 矿泉水
        private void labelBottledWater_Click(object sender, EventArgs e)
        {
            if (Item_BottledWater <= 0)
                return;

            Item_BottledWater -= 1;
            AddMessage("你喝一瓶饮料。", -5, 50, -1, 2, 0, 0, -1);
            ShowMessage(); DeathJudgment();
        }

        #endregion


        #region 按钮方法

        private void buttonOption0_Click(object sender, EventArgs e) => OperationAssignment(0);

        private void buttonOption1_Click(object sender, EventArgs e) => OperationAssignment(1);

        private void buttonOption2_Click(object sender, EventArgs e) => OperationAssignment(2);

        private void buttonOption3_Click(object sender, EventArgs e) => OperationAssignment(3);

        private void buttonOption4_Click(object sender, EventArgs e) => OperationAssignment(4);

        private void buttonReSet_Click(object sender, EventArgs e) => ReSet();

        private void SurvivalChallengeGame_FormClosed(object sender, FormClosedEventArgs e) => TC_Save();

        private void checkBoxMessageLog_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMessageLog.Checked)
            {
                labelMessage.Text = MessageLog;
                ScrollToBottom();
            }
            else
            {
                labelMessage.Text = "暂无信息";
            }
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            NowDoing = SceneId.Main;
            XSetMessage("你回了家……");
            SetButtonTexts();
            ReSetButtonEnableds();
        }

        #endregion


        private void comboBoxTarotCard_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMessage(comboBoxTarotCard.Text switch
            {
                "愚者" => "塔罗牌 0号牌：愚者\n勇往直前：增加的理智翻倍\n自负顽固：每日减少的心情值加倍，减少的金钱值加倍\n此牌没有编号，亦是开始，亦是结束，代表无限的可能。",
                "魔术师" => "塔罗牌 1号牌：魔术师\n炉火纯青的技艺：获得工作经验翻倍\n意志浅薄：扣除的理智翻倍\n魔术师作为起源之牌，象征着与生俱来的创造力和潜能",
                "女祭司" => "塔罗牌 2号牌：女祭司\n知性理智：获得的理智翻倍\n无知贪婪：获得工作经验效率减半\n女祭司能够判明世间对立的两面，此牌象征着直觉和学识的不可质疑，\n以及内心深处的感性。",
                "皇后" => "塔罗牌 3号牌：皇后\n群众的欢乐：不会丢失朋友，也不会交到新朋友\n快乐的消失：心情过低扣除双倍理智，心情低于零点即死\n皇后象征着大地的母性，生命的诞生，物质的繁荣。\n此牌代表着幸福与爱情，安逸及富足的生活。",
                "战车" => "塔罗牌 7号牌：战车\n效果：不会丢失朋友，也不会交到新朋友",
                "月亮" => "塔罗牌 18号牌：月亮\n潜能受限：工作经验上限减少至 100%\n排解恐惧：每日不再减少心情与理智\n月有阴晴圆，象征变化与潜意识的活动，隐喻居安思危，拥有洞察\n不安定的敏锐性直觉。",
                "世界" => "塔罗牌 21号牌：世界",
                _ => $"未知塔罗牌 '{comboBoxTarotCard.Text}' 或还未实现该塔罗牌"
            });
            MessageLog = string.Empty;
        }

        private void SurvivalChallengeGame_Load(object sender, EventArgs e)
        {
            comboBoxTarotCard.Enabled = Days == 1 && Morning || DeBugMode;
        }

        private void button_HowToPlay_Click(object sender, EventArgs e)
        {
            labelMessage.Text =
                "主线任务：\n" +
                " · 方向1：存活一百天（基础 愚者结局）\n" +
                " · 方向2：完成问心崖挑战（进阶 女祭司结局）\n" +
                " · 方向3：通过六大时空碎块（终极 结局开发中）\n" +

                "\n" +
                "存活提示：\n" +
                " · 注意左侧数值，超出限制会死亡。\n" +
                " · 有三大天灾会降临（极寒、污染、极热），注意做好特别准备。\n" +
                " · 极致的天灾普通的防护手段不足以抵挡，或许你该去接触神明？\n" +
                " · 当字迹不清时，也许你需要黑暗屏蔽器。\n" +
                " · 神秘商店以理智为货币，不仅有特殊道具，还有特殊项目哦！\n" +
                " · 初入神秘森林，需要特殊的理智状态。\n" +
                " · 神秘森林的工作房室是个奇妙的地方。\n" +

                "\n" +
                "工作房室：\n" +
                " · 太阳·御寒服：抵挡极致低温的必备道具，由【御寒服】和【太阳水滴】合成。\n" +
                " · 月亮·御焰服：抵挡极致高温的必备道具，由【御焰服】和【月亮水滴】合成。\n" +
                " · 污染干扰器：抵挡污染入侵的必备道具，由【理智精华】合成。\n" +
                " · 理智精华由理智提纯而来。\n";

            if (DeBugMode)
            {
                L_ForestGrass = true; Item_DarkBlocker = true;
                EnterScene(SceneId.DeepForest);   // 进入深林，用户点左草丛即可走 LTEOTC
            }
        }

        private void button_OpenGameDoc_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Qi.QisToolkit3_Datas.actualDirectory, "《问身 问己 问心》游戏介绍.docx");

            if (File.Exists(filePath))
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true  // 显式启用 ShellExecute
                });
            else
                MessageBox.Show($"文件丢失：{filePath}\n请检查文件是否存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button_KillSelf_Click(object sender, EventArgs e)
        {
            XSetMessage("你自杀了......", 0, 0, 0, 0, 0, 0, -999999999);

        }

        private void buttonDebug_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void buttonCopyDatas_Click(object sender, EventArgs e)
        {
            string text =
                $"天数：{Days} | " + (Morning ? "上午\n" : "下午\n") +
                $"金钱：{Gold} | 理智：{San} | 心情：{Mood}\n" +
                $"健康：{Health} | 污染：{Pollution}%\n" +
                $"饥饿：{Hunger}/{MaxHunger} | 口渴：{Thirsty}/{MaxThirsty}\n" +
                $"工作经验：{WorkExperience} | 室外温度：{Temperature}\n\n" +
                $"选项：{buttonOption0.Text} | {buttonOption1.Text} | {buttonOption2.Text} | {buttonOption3.Text} | {buttonOption4.Text}\n\n" +
                $"信息：{labelMessage.Text}";

            Clipboard.SetText(text);
            //GameOver();
        }
    }
}
