using Newtonsoft.Json.Linq;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace QisToolkit3.Forms
{
    public partial class SurvivalChallengeGame : Form
    {
        private static readonly Random random = new Random();

        // 主界
        private static int Days = 1, Gold = 50, San = 80, Mood = 55, Health = 100, Hunger = 60, Thirsty = 60, BaseLuck = 0, Luck = 0;
        private static int WorkExperience = 0, Temperature = 25, Friend = 1, Pollution = 0, Lost = 0;
        private static int Natural_San = 5, Natural_Mood = 10, Natural_Health = 1, Natural_Hunger = 10, Natural_Thirsty = 10;
        private static int MaxHunger = 100, MaxThirsty = 100, MaxHealth = 100;
        private static int BasicSalary = 50, AddWorkExperienceMax = 8, MadBuff = 0, TaotieBuff = 0;
        private static int ExtremeColdComes = random.Next(20, 30), EndOfExtremeCold = ExtremeColdComes + random.Next(10, 25);
        private static int ExtremeHotComes = EndOfExtremeCold + random.Next(10, 20), EndOfExtremeHot = ExtremeHotComes + random.Next(10, 25);
        private static int Food_Dumpling = 50, Food_RouJiaMo = 40, Food_RoastedCorn = 10, Food_Lemonade = 10, Food_Coffee = 30;
        private static int Food_Popsicle = 5, Food_PurificationPackage = 1000, Food_HungerJelly = 25, Food_ThirstJelly = 25, Food_GluttonousFeast = 1000;
        private static int PrincipalLine = 0, KnowledgeCd = 0; bool Morning = true, TC_Wall = true, TC_Set = false; // TC 是塔罗牌
        private static bool Item_DarkBlocker = false, Item_SunWaterDrop = false, Item_MoonWaterDrop = false, Item_PollutionJammer = false;
        private static bool Item_FirstAidKit = false, Item_Antidote = false, Item_LuckyCharm = false, Item_SoothingMedicine = false;
        private static int Item_God = 0, Item_WinterClothing = 0, Item_SummerClothing = 0;
        private static int Item_EssenceReason = 0, Item_CompressedBiscuit = 0, Item_BottledWater = 0, Item_BottledBeverage = 0;
        private static int G_AvoidingDeath = 0, G_MustGoodRandomEvent = 0, G_AbsoluteRationality = 0, G_DoubleTheValue = 0;
        private static bool L_ForestGrass = false, R_ForestGrass = false;
        private static bool[] HAC_AskBody = { false, false, false };
        private static bool[] HAC_AskSelf = { false, false, false };
        private static bool[] HAC_AskSoul = { false, false, false };
        private static string MessageLog = "你出生了......";
        private static string NowDoing = "Main", World = "Main";
        private static bool DeBugMode = false, DeBug_2Gold = false;
        private static bool[] TC = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };

        // 通用主线状态
        private static bool KnowVoid = false;

        // 时空碎块
        private static int ResetCount = 10, SecDays = 1, SecGold = 50, SecSan = 80, SecMood = 55, SecHealth = 100, SecHunger = 60, SecThirsty = 60;
        private static int SecPollution = 0, SecMaxHunger = 100, SecMaxThirsty = 100, SecMaxHealth = 100, SecTemperature = 25;
        private static bool SecMorning = true;

        // 时空碎块：大陆终焉
        private static bool LTEOTC_KnowYanName = false, LTEOTC_KnowPrologue_0_1 = false, LTEOTC_KnowPrologue_KnowHowToLeave = false;
        private static bool LTEOTC_FirstLeave = true;
        private static string YanName = "未知";
        private static int YanSuspicion = -60;

        public int DisplayLuckValue => LuckValue + 100; // 显示用的幸运值，范围0到200

        // 幸运值属性
        public int LuckValue
        {
            get => Luck;
            set => Luck = Math.Clamp(value, -100, 100);
        }



        public SurvivalChallengeGame()
        {
            InitializeComponent();
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

        private void ReSetButtonEnableds() =>
            SetButtonEnableds(true, Gold >= 5, KnowledgeCd <= 0, Gold >= 50, Days >= 10);

        private void ShowMessage()
        {
            labelSan.Text = $"理智：{San}%";
            labelMood.Text = $"心情：{Mood}%";
            labelLuck.Text = $"气运：{DisplayLuckValue}%";
            labelLost.Text = $"迷失：{Lost}%";
            labelPollution.Text = KnowVoid ? $"归虚值：{Pollution}%" : $"污染值：{Pollution}%";
            labelSan.ForeColor = San <= 30 ? Color.Red : Color.Black;
            labelMood.ForeColor = Mood <= 30 ? Color.Red : Color.Black;
            labelLost.ForeColor = Lost >= 60 ? Color.Red : Color.Black;
            labelPollution.ForeColor = Pollution >= 60 ? Color.Red : Color.Black;


            if (NowDoing == "Main")
            {
                labelDays.Text = $"今天是第 {Days} 天 {(Morning ? "上午" : "下午")}";
                labelGold.Text = $"金币：{Gold}G";
                labelHealth.Text = $"健康：{Health}%";
                labelHunger.Text = $"饥饿值：{Hunger} / {MaxHunger}";
                labelThirsty.Text = $"口渴值：{Thirsty} / {MaxThirsty}";
                labelWorkExperience.Text = $"工作经验：{WorkExperience}%";
                labelTemperature.Text = $"室外温度：{Temperature}℃";

                labelHunger.ForeColor = Hunger <= 30 ? Color.Red : Color.Black;
                labelHealth.ForeColor = Health <= 30 ? Color.Red : Color.Black;
                labelThirsty.ForeColor = Thirsty <= 30 ? Color.Red : Color.Black;


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

            else if (NowDoing == "L_TheEndOfTheContinent")
            {
                labelDays.Text = $"今天是第 {SecDays} 天 {(SecMorning ? "上午" : "下午")}";
                labelGold.Text = $"金币：{SecGold}G";
                labelHealth.Text = $"健康：{SecHealth}%";
                labelHunger.Text = $"饥饿值：{SecHunger} / {SecMaxHunger}";
                labelThirsty.Text = $"口渴值：{SecThirsty} / {SecMaxThirsty}";
                labelWorkExperience.Text = $"时空回溯：{ResetCount}次";
                labelTemperature.Text = $"室外温度：{Temperature}℃";

                labelHunger.ForeColor = SecHunger <= 30 ? Color.Red : Color.Black;
                labelHealth.ForeColor = SecHealth <= 30 ? Color.Red : Color.Black;
                labelThirsty.ForeColor = SecThirsty <= 30 ? Color.Red : Color.Black;
            }

            SetButtonEnabledsColor(buttonOption0.Enabled, buttonOption1.Enabled, buttonOption2.Enabled, buttonOption3.Enabled, buttonOption4.Enabled);
        }

        // 主函数
        private void Main()
        {
            if (NowDoing == "Main")
            {
                // 主界
                if (World == "Main")
                {
                    AddLog("执行了主函数");
                    Morning = !Morning; // 上下午转换

                    // 每天执行
                    if (Morning == true)
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



                    ReSetButtonEnableds();
                    NextBackEnable(false);
                    SetButtonOptionFont();
                }

                // 时空碎块：大陆终焉
                else if (World == "L_TheEndOfTheContinent")
                {

                }
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
            switch (World)
            {
                // 主界
                case "Main":
                    switch (NowDoing)
                    {
                        // 主页
                        case "Main":
                            switch (Id)
                            {
                                // 去挣钱
                                case 0:
                                    To_EarnMoney();
                                    break;

                                // 买吃食
                                case 1:
                                    NextBackEnable();
                                    To_Foods();
                                    break;

                                // 知识区
                                case 2:
                                    To_KnowledgeArea();
                                    break;

                                // 出去玩
                                case 3:
                                    GF_GoOutAndPlay();
                                    break;

                                // 向世界
                                case 4:
                                    NextBackEnable();
                                    To_TheWorld();
                                    break;
                            }
                            break;

                        // 买吃食
                        case "Foods_0":
                            GF_Foods_0(Id);
                            break;

                        // 买吃食 2
                        case "Foods_1":
                            GF_Foods_1(Id);
                            break;

                        // 去挣钱
                        case "EarnMoney":
                            GF_EarnMoney(Id);
                            break;

                        // 向世界 1（去冒险）
                        case "TheWorld_0":
                            switch (Id)
                            {
                                case 0:
                                    GF_Hospital();
                                    break;

                                case 1:
                                    NextBackEnable();
                                    To_Supermarket();
                                    break;

                                case 2:
                                    PL_HeartAskingCliff();
                                    break;

                                case 3:
                                    NextBackEnable();
                                    To_MysteryShop();
                                    break;

                                case 4:
                                    if (Item_God == 0) To_WorshipOfGods();
                                    else To_MysteriousForestIntersection();
                                    break;
                            }
                            break;

                        // 神秘商店 1
                        case "MysteryShop_0":
                            NextBackEnable();
                            GF_MysteryShop_0(Id);
                            break;

                        // 神秘商店 2
                        case "MysteryShop_1":
                            NextBackEnable();
                            GF_MysteryShop_1(Id);
                            break;

                        // 知识区
                        case "KnowledgeArea":
                            GF_KnowledgeArea(Id);
                            break;

                        // 超市
                        case "Supermarket_0":
                            GF_Supermarket_0(Id);
                            break;

                        // 超市
                        case "Supermarket_1":
                            GF_Supermarket_1(Id);
                            break;

                        // 参拜神像
                        case "WorshipOfGods":
                            PL_WorshipOfGods(Id);
                            break;

                        // 神秘森林岔路口
                        case "MysteriousForestIntersection":
                            PL_MysteriousForestIntersection(Id);
                            break;

                        // 工作房室
                        case "WorkRoom":
                            PL_WorkRoom(Id);
                            break;

                        // 森林深处
                        case "DeepForest":
                            PL_DeepForest(Id);
                            break;
                    }
                    break;

                // 时空碎块：大陆终焉
                case "L_TheEndOfTheContinent":

                    switch (NowDoing)
                    {
                        // 开篇对话 0
                        case "LTEOTC_Prologue_0_0":
                            PL_LTEOTC_Prologue_0(Id);
                            break;

                        // 开篇对话 0-1
                        case "LTEOTC_Prologue_0_1":
                            PL_LTEOTC_Prologue_0_1(Id);
                            break;
                    }
                    break;
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

        #region 通用功能前往

        // 前往美食街页
        private void To_Foods(int Mode = 0)
        {
            if (Mode == 0)
            {
                NowDoing = "Foods_0";
                XSetMessage("你前往了美食街，……");
                SetButtonTexts("购买 饺子", "购买 肉夹馍", "购买 烤玉米", "购买 柠檬水", "购买 咖啡");
                SetButtonEnableds(Gold >= Food_Dumpling, Gold >= Food_RouJiaMo, Gold >= Food_RoastedCorn, Gold >= Food_Lemonade, Gold >= Food_Coffee);
            }
            else if (Mode == 1)
            {
                NowDoing = "Foods_1";
                XSetMessage("你前往了美食街，……");
                SetButtonTexts("购买 冰淇淋", "购买 净化套餐", "购买 口渴果冻", "购买 饥饿果冻", "购买 饕餮盛宴");
                SetButtonEnableds(Gold >= Food_Popsicle, Gold >= Food_PurificationPackage, Gold >= Food_ThirstJelly, Gold >= Food_HungerJelly, Gold >= Food_GluttonousFeast);
            }
        }

        // 前往知识区页
        private void To_KnowledgeArea()
        {
            NowDoing = "KnowledgeArea";
            XSetMessage("你看向了“知识区”，你打算前往……");
            SetButtonTexts("权杖：图书馆", "面具：艺术馆", "书本：历史馆", "骰子：占卜馆", "镰刀：消逝馆");
            SetButtonEnableds(
                Item_God == 0 || Item_God == 4,
                Item_God == 0 || Item_God == 1 || Item_God == 2,
                Item_God == 0 || Item_God == 5,
                Item_God == 0 || Item_God == 2 || Item_God == 1,
                Item_God == 0 || Item_God == 3
            );
        }

        // 前往区冒险页（向世界页）
        private void To_TheWorld(int Mode = 0)
        {
            if (Mode == 0)
            {
                NowDoing = "TheWorld_0";
                SetMessage("你走出小镇，走向了……");

                SetButtonTexts("前往医院", "前往超市", "前往问心崖", "前往神秘商店", Item_DarkBlocker ? "前往神秘森林" : "前往████");
                SetButtonEnableds(Gold >= 100, Gold >= 500, true, San >= 50, San <= 50 && Days < 50 && Item_God == 0 || Item_God > 0);
                /*
                 * 无神赐物品神秘森林进入条件：
                 * 理智 <= 50
                 * 天数 < 50
                 * 无神赐道具
                 */
            }
            else if (Mode == 1)
            {
                NowDoing = "TheWorld_1";
                SetMessage("你走出小镇，走向了……");


                if (Item_DarkBlocker)
                    SetButtonTexts("前往████", "前往████", "前往████", "前往████", "前往████");
                else if (true)
                    SetButtonTexts("前往海滩", "待定", "待定", "待定", "待定");
                else
                    SetButtonTexts("前往倒悬之海", "待定", "待定", "待定", "待定");
                SetButtonEnableds(false, false, false, false, false);
            }
        }

        // 前往超市页
        private void To_Supermarket(int Mode = 0)
        {
            if (Mode == 0)
            {
                NowDoing = "Supermarket_0";
                SetMessage("你前往了超市，你准备买……");
                SetButtonTexts("御寒服", "御焰服", "压缩饼干", "瓶装饮料", "矿泉水");
                SetButtonEnableds(
                    Gold >= 500 && Item_WinterClothing == 0,
                    Gold >= 500 && Item_SummerClothing == 0,
                    Gold >= 500,
                    Gold >= 500,
                    Gold >= 500
                );
            }
            else if (Mode == 1)
            {
                NowDoing = "Supermarket_1";
                SetMessage("你走出小镇，走向了……");
                SetButtonTexts("急救包", "解毒剂", "幸运符", "舒缓药", "暂无");
                SetButtonEnableds(
                    Gold >= 1000 && !Item_FirstAidKit,
                    Gold >= 1000 && !Item_Antidote,
                    Gold >= 1000 && !Item_LuckyCharm,
                    Gold >= 1000 && !Item_SoothingMedicine,
                    false
                );
            }
        }

        // 前往挣钱页
        private void To_EarnMoney()
        {
            NowDoing = "EarnMoney";
            XSetMessage("你想要去挣大钱，……");
            SetButtonTexts("打零工", "跑外卖", "做代购", "去上班", "做开发");
            SetButtonEnableds(true, WorkExperience >= 15, WorkExperience >= 40, WorkExperience >= 80, WorkExperience >= 120);
        }

        // 前往神秘商店页
        private void To_MysteryShop(int Mode = 0)
        {
            XSetMessage("你走出小镇，走向了那片屹立在迷雾之中的神秘商店，你在迷雾之中穿行\n不知道过去了多久，你抵达了那神秘商店");
            if (Mode == 0)
            {
                NowDoing = "MysteryShop_0";
                if (Item_DarkBlocker)
                    SetButtonTexts("购买黑暗屏蔽器", "购买饕餮药水", "购买癫狂药水", "购买太阳水滴", "购买月亮水滴");
                else
                    SetButtonTexts("购买黑暗屏蔽器", "购买饕餮药水", "购买癫狂药水", "购买太阳水滴", "购买月亮水滴");
                SetButtonEnableds(!Item_DarkBlocker && San >= 100, San >= 200, San >= 200, San >= 50 && Gold >= 1000 && !Item_SunWaterDrop, San >= 50 && Gold >= 1000 && !Item_MoonWaterDrop);
            }
            else if (Mode == 1)
            {
                NowDoing = "MysteryShop_1";
                SetButtonTexts("情绪项目", "呆滞项目", "强健项目", "诡异项目", "提纯项目");
                SetButtonEnableds(San >= 100, Mood >= 100, San >= 100, Health >= 50, San >= 100 & Item_God > 0);
            }
        }

        #endregion


        #region 通用功能实现

        // 买吃食 1（ID 1）
        private void GF_Foods_0(int Id)
        {
            switch (Id)
            {

                // 购买 饺子
                case 0:
                    XSetMessage("你前往了美食街，购买了一盘饺子", 64, 10, 0, 5, -Food_Dumpling);
                    break;

                // 购买 肉夹馍
                case 1:
                    XSetMessage("你前往了美食街，购买了一个肉夹馍", 50, 0, 0, 5, -Food_Dumpling, 0, 7);
                    break;

                // 购买 烤玉米
                case 2:
                    XSetMessage("你前往了美食街，购买了一个烤玉米", 20, 3, 0, 5, -Food_RoastedCorn);
                    break;

                // 购买 柠檬水
                case 3:
                    XSetMessage("你前往了美食街，购买了一个柠檬水", 5, 45, 0, 5, -Food_Lemonade);
                    break;

                // 购买 咖啡
                case 4:
                    XSetMessage("你前往了美食街，购买了一个咖啡", 2, 30, 10, -2, -Food_Coffee);
                    break;
            }
            NowDoing = "Main";
            SetButtonTexts();
        }

        // 买吃食 2（ID 11）
        private void GF_Foods_1(int Id)
        {

            // 买吃食
            switch (Id)
            {
                // 购买 冰淇淋
                case 0:
                    if (Temperature > 30)
                        XSetMessage("你前往了美食街，购买了一根冰淇淋", 5, 15, 5, 5, -Food_Popsicle, 0, 15);
                    else if (Temperature < 0)
                        XSetMessage("你前往了美食街，购买了一根冰淇淋", 5, 15, 5, 5, -Food_Popsicle, 0, -15);
                    else
                        XSetMessage("你前往了美食街，购买了一根冰淇淋", 5, 15, 5, 5, -Food_Popsicle);
                    break;

                // 购买 净化套餐
                case 1:
                    XSetMessage("你前往了美食街，购买了净化套餐", 5, 5, 5, -25, -Food_PurificationPackage, -5, -25, -25);
                    break;

                // 购买 口渴果冻
                case 2:
                    XSetMessage("你前往了美食街，购买了一个口渴果冻", 40, -30, 0, 0, -Food_ThirstJelly);
                    break;

                // 购买 饥饿果冻
                case 3:
                    XSetMessage("你前往了美食街，购买了一个饥饿果冻", -30, 40, 0, 0, -Food_Lemonade);
                    break;

                // 购买 饕餮盛宴
                case 4:
                    XSetMessage("你前往了美食街，购买了一整套饕餮盛宴", 500, 500, -10, 20, -Food_GluttonousFeast);
                    break;
            }
            NowDoing = "Main";
            SetButtonTexts();
        }

        // 去挣钱（ID 2）
        private void GF_EarnMoney(int Id)
        {
            Random random = new Random();
            int Tmp = random.Next(0, AddWorkExperienceMax); if (WorkExperience + Tmp > 150) Tmp = 150 - WorkExperience;
            switch (Id)
            {
                case 0:
                    int Temp = random.Next(30, 70);
                    int MoodTmp = random.Next(2, 5);
                    if (Morning) XSetMessage("你去打了一上午临工。", 0, 0, 0, -MoodTmp, Temp + Temp * WorkExperience / 100, Tmp);
                    else XSetMessage("你去打了一下午临工。", 0, 0, 0, -MoodTmp, Temp + Temp * WorkExperience / 100, Tmp);
                    break;

                case 1:
                    Temp = random.Next(40, 80);
                    MoodTmp = random.Next(3, 6);
                    if (Morning) XSetMessage("你去送了一上午外卖。", 0, 0, 0, -MoodTmp, Temp + Temp * WorkExperience / 100, Tmp);
                    else XSetMessage("你去送了一下午外卖。", 0, 0, 0, -MoodTmp, Temp + Temp * WorkExperience / 100, Tmp);
                    break;

                case 2:
                    Temp = random.Next(50, 100);
                    MoodTmp = random.Next(5, 10);
                    if (Morning) XSetMessage("你去做了一上午代购。", 0, 0, 0, -MoodTmp, Temp + Temp * WorkExperience / 100, Tmp);
                    else XSetMessage("你去做了一下午代购。", 0, 0, 0, -MoodTmp, Temp + Temp * WorkExperience / 100, Tmp);
                    break;

                case 3:
                    if (Morning) XSetMessage("你去上班上了一上午。", 0, 0, 0, -25, 250 + 250 * WorkExperience / 100, Tmp);
                    else XSetMessage("你去上班上了一下午。", 0, 0, 0, -25, 250 + 250 * WorkExperience / 100, Tmp);
                    break;

                case 4:
                    Temp = random.Next(-250, 1000);
                    MoodTmp = random.Next(-25, 25);
                    if (Morning) XSetMessage("你去做了一上午开发。", 0, 0, 0, -MoodTmp, Temp + Temp * WorkExperience / 100, Tmp);
                    else XSetMessage("你去做了一下午开发。", 0, 0, 0, -MoodTmp, Temp + Temp * WorkExperience / 100, Tmp);
                    break;
            }

            NowDoing = "Main";
            SetButtonTexts();
        }

        // 神秘商店（ID 4）
        private void GF_MysteryShop_0(int Id)
        {
            switch (Id)
            {
                case 0:
                    Item_DarkBlocker = true;
                    XSetMessage("你买下了一个黑暗屏蔽器，你感到一阵头晕目眩", 0, 0, -100);
                    break;
                case 1:
                    XSetMessage("你买下了一瓶饕餮药水，你感到一阵头晕目眩", 0, 0, -200);
                    MaxHunger = 9999; MaxThirsty = 9999; TaotieBuff = 10;
                    break;
                case 2:
                    XSetMessage("你买下了一瓶癫狂药水，你感到一阵头晕目眩", 0, 0, -200);
                    MadBuff = 10;
                    break;
                case 3:
                    XSetMessage("你买下了一滴太阳水滴！", 0, 0, -50, 0, -1000);
                    Item_SunWaterDrop = true;
                    break;
                case 4:
                    XSetMessage("你买下了一滴太阳水滴！", 0, 0, -50, 0, -1000);
                    Item_MoonWaterDrop = true;
                    break;
            }
            NowDoing = "Main";
            SetButtonTexts();
        }

        // 神秘商店（ID 44）
        private void GF_MysteryShop_1(int Id)
        {
            switch (Id)
            {
                case 0:
                    XSetMessage("你选择了情绪项目，你仿佛感受到无数情绪被灌进了脑海", 0, 0, -100, 80);
                    break;
                case 1:
                    XSetMessage("你选择了呆滞项目，你仿佛感受到无数情绪被从脑海抽走", 0, 0, 80, -100);
                    break;
                case 2:
                    XSetMessage("你选择了强健项目，你仿佛感受到你的身体的每个细胞都在嚎叫", 0, 0, -100, 0, 0, 0, 100 - Health);
                    break;
                case 3:
                    XSetMessage("你选择了情绪项目，你仿佛感受到你的身体正在疯狂的崩溃！", 0, 0, 100, 0, 0, 0, -80);
                    break;
                case 4:
                    XSetMessage("你选择了提纯项目，你感受到你的理智在被疯狂抽出！", 0, 0, -100);
                    XSetMessage("你得到了——理智精华！");
                    ++Item_EssenceReason;
                    break;
            }
            NowDoing = "Main";
            SetButtonTexts();
        }

        // 知识区（ID 5）
        private void GF_KnowledgeArea(int Id)
        {
            int Tmp;
            switch (Id)
            {

                // 图书馆（秩序）
                case 0:
                    if (Item_God == 0)
                    {
                        Tmp = new Random().Next(15, 35);
                        if (Morning)
                            XSetMessage("你前往了图书馆，你在里面看了一上午，沉浸在知识的海洋", 0, 0, Tmp);
                        else
                            XSetMessage("你前往了图书馆，你在里面看了一下午，沉浸在知识的海洋", 0, 0, Tmp);
                    }
                    else if (Item_God == 4)
                    {
                        Tmp = new Random().Next(15, 35);
                        if (Item_DarkBlocker)
                        {
                            if (Morning)
                                XSetMessage("你前往了图书馆，你在里面看了一上午，享受着秩序的光辉", 0, 0, Tmp + 10);
                            else
                                XSetMessage("你前往了图书馆，你在里面看了一下午，享受着秩序的光辉", 0, 0, Tmp + 10);
                        }
                        else
                        {
                            if (Morning)
                                XSetMessage("你前往了图书馆，你在里面看了一上午，享受着██的光辉", 0, 0, Tmp + 10);
                            else
                                XSetMessage("你前往了图书馆，你在里面看了一下午，享受着██的光辉", 0, 0, Tmp + 10);
                        }
                    }
                    else
                    {
                        Tmp = new Random().Next(15, 35);
                        if (Item_DarkBlocker)
                        {
                            if (Morning)
                                XSetMessage("你前往了图书馆，你在里面看了一上午，但同样秩序的存在让你很不舒服", 0, 0, Tmp - 10);
                            else
                                XSetMessage("你前往了图书馆，你在里面看了一下午，但同样秩序的存在让你很不舒服", 0, 0, Tmp - 10);
                        }
                        else
                        {
                            if (Morning)
                                XSetMessage("你前往了图书馆，你在里面看了一上午，但同样██的存在让你很不舒服", 0, 0, Tmp - 10);
                            else
                                XSetMessage("你前往了图书馆，你在里面看了一下午，但同样██的存在让你很不舒服", 0, 0, Tmp - 10);
                        }
                    }
                    break;

                // 艺术馆（欺诈）
                case 1:
                    if (Item_God == 0)
                    {
                        Tmp = new Random().Next(15, 35);
                        if (Morning)
                            XSetMessage("你前往了艺术馆，你在里面看了一上午，沉浸在艺术之美中", 0, 0, Tmp);
                        else
                            XSetMessage("你前往了艺术馆，你在里面看了一下午，沉浸在艺术之美中", 0, 0, Tmp);
                    }
                    else if (Item_God == 1 || Item_God == 2)
                    {
                        Tmp = new Random().Next(1, 50);
                        if (Item_DarkBlocker)
                            XSetMessage("你前往了艺术馆，你仿佛进入了那传说中的嬉笑之潮。\n在那更深层的虚空之中，无数的“镜子”呈现出无尽的抽象", 0, 0, Tmp);
                        else
                            XSetMessage("你前往了艺术馆，你仿佛进入了那传说中的████。\n在那更深层的██之中，无数的“██”呈现出█████", 0, 0, Tmp);
                    }
                    else
                    {
                        Tmp = new Random().Next(-15, 15);
                        if (Item_DarkBlocker)
                        {
                            if (Morning)
                                XSetMessage("你前往了艺术馆，你在里面看了一上午，你仿佛感受到无数双嬉笑的面孔正盯着你", 0, 0, Tmp);
                            else
                                XSetMessage("你前往了艺术馆，你在里面看了一下午，你仿佛感受到无数双嬉笑的面孔正盯着你", 0, 0, Tmp);
                        }
                        else
                        {
                            if (Morning)
                                XSetMessage("你前往了艺术馆，你在里面看了一上午，你仿佛感受到████████正盯着你", 0, 0, Tmp);
                            else
                                XSetMessage("你前往了艺术馆，你在里面看了一下午，你仿佛感受到████████正盯着你", 0, 0, Tmp);
                        }
                    }
                    break;

                // 历史馆（记忆）
                case 2:
                    if (Item_God == 0)
                    {
                        Tmp = new Random().Next(15, 35);
                        if (Morning)
                            XSetMessage("你前往了历史馆，你在里面看了一上午，沉浸在对历史的惊叹之中", 0, 0, Tmp);
                        else
                            XSetMessage("你前往了历史馆，你在里面看了一下午，沉浸在对历史的惊叹之中", 0, 0, Tmp);
                    }
                    else if (Item_God == 5)
                    {
                        Tmp = new Random().Next(15, 35);
                        if (Item_DarkBlocker)
                            XSetMessage("你前往了历史馆，你仿佛进入了那传说中的记忆神殿。\n在那被时间长河与嬉笑之潮包围，充满无量知识的神圣大殿", 0, 0, Tmp + 10);
                        else
                            XSetMessage("你前往了历史馆，你仿佛进入了那传说中的████。\n在那被████与████包围，充满████的████", 0, 0, Tmp + 10);
                    }
                    else
                    {
                        Tmp = new Random().Next(15, 35);
                        if (Morning)
                            XSetMessage("你前往了历史馆，你在里面看了一上午，你仿佛感受到那些历史活了过来，正盯着你", 0, 0, Tmp - 10);
                        else
                            XSetMessage("你前往了历史馆，你在里面看了一下午，你仿佛感受到那些历史活了过来，正盯着你", 0, 0, Tmp - 10);
                    }
                    break;

                // 占卜馆（命运）
                case 3:
                    if (Item_God != 1 && Item_God != 2)
                    {
                        string StrTmp = "暂无预言";
                        if (Days < 10) StrTmp = (10 - Days) + " 天之后世界将大变。";
                        else if (Days >= 10 && Days < ExtremeColdComes) StrTmp = "天启之相，伴 " + (ExtremeColdComes - Days) + " 日后极寒降临。";
                        else if (Days >= EndOfExtremeCold && Days < ExtremeHotComes) StrTmp = "戮地之相，伴 " + (ExtremeHotComes - Days) + " 日后极热降临。";

                        if (Morning)
                            XSetMessage("你前往了占卜馆，上面写着：“" + StrTmp + "”\n当你回过神之时，上午已经过去了", 0, 0, 1);
                        else
                            XSetMessage("你前往了占卜馆，上面写着：“" + StrTmp + "”\n当你回过神之时，下午已经过去了", 0, 0, 1);
                    }
                    else if (Item_God == 1 || Item_God == 2)
                    {
                        Tmp = new Random().Next(15, 45);
                        if (Item_DarkBlocker)
                            XSetMessage("你前往了占卜馆，你仿佛看到了那传说中的命运神轮。\n祂坐在神轮前，执掌着万千生命的命与运", 0, 0, Tmp + 10);
                        else
                            XSetMessage("你前往了占卜馆，你仿佛看到了那传说中的████。\n█████前，██着████的█与█", 0, 0, Tmp + 10);
                    }
                    else
                    {
                        if (Item_DarkBlocker)
                            XSetMessage("你前往了占卜馆，但很明显祂不想见你，上来就给你了一个闭门羹", 0, 0, -1, -10);
                        else
                            XSetMessage("你前往了占卜馆，但很明显█不想见你，上来就给你了一个闭门羹", 0, 0, -1, -10);
                    }
                    break;

                // 消逝馆（死亡）
                case 4:
                    if (Item_God == 0)
                    {
                        if (Morning)
                            XSetMessage("你前往了消逝馆，你在里面看了一上午，懵了一上午", 0, 0, 5);
                        else
                            XSetMessage("你前往了消逝馆，你在里面看了一下午，懵了一下午", 0, 0, 5);
                    }
                    else if (Item_God == 3)
                    {
                        if (Item_DarkBlocker)
                            XSetMessage("你前往了消逝馆，你仿佛踏上了那传说中的死亡骨阶。\n在那不可承载一切生命，你仿佛看到自己就是一副骨架，踏上面见祂的骨阶", 0, 0, 15);
                        else
                            XSetMessage("你前往了消逝馆，你仿佛踏上了那传说中的████。\n在那不可██████，你仿佛看到自己就是████，踏上面见█的██", 0, 0, 15);
                    }
                    else
                    {
                        Tmp = new Random().Next(-10, 2);
                        if (Morning)
                            XSetMessage("你前往了历史馆，你在里面看了一上午，在那里充斥着浓郁的死亡气息，令你汗毛直竖", 0, 0, Tmp);
                        else
                            XSetMessage("你前往了历史馆，你在里面看了一下午，在那里充斥着浓郁的死亡气息，令你汗毛直竖", 0, 0, Tmp);
                    }
                    break;
            }

            if (Item_God == 0)
                KnowledgeCd = 1;
            else
                KnowledgeCd = 2;

            NowDoing = "Main";
            SetButtonTexts();
        }

        // 超市（ID 7）
        private void GF_Supermarket_0(int Id)
        {
            switch (Id)
            {
                case 0:
                    Item_WinterClothing = 1;
                    XSetMessage("你买下了一个御寒服！你现在可以抵御基础低温了。", 0, 0, 0, 0, -500);
                    break;
                case 1:
                    Item_SummerClothing = 1;
                    XSetMessage("你买下了一个御焰服！你现在可以抵御基础高温了。", 0, 0, 0, 0, -500);
                    break;
                case 2:
                    Item_CompressedBiscuit += 1;
                    XSetMessage("你买下了一块压缩饼干。", 0, 0, 0, 0, -500);
                    break;
                case 3:
                    Item_BottledBeverage += 1;
                    XSetMessage("你买下了一瓶瓶装饮料。", 0, 0, 0, 0, -500);
                    break;
                case 4:
                    Item_BottledWater += 1;
                    XSetMessage("你买下了一瓶矿泉水。", 0, 0, 0, 0, -500);
                    break;
            }
            NowDoing = "Main";
            SetButtonTexts();
        }

        // 超市（ID 77）
        private void GF_Supermarket_1(int Id)
        {
            switch (Id)
            {
                case 0:
                    Item_FirstAidKit = true;
                    XSetMessage("你买下了一个急救包！它或许可以在关键时刻救你一命！", 0, 0, 0, 0, -1000);
                    break;
                case 1:
                    Item_Antidote = true;
                    XSetMessage("你买下了一个解毒剂！它或许可以在关键时刻救你一命！", 0, 0, 0, 0, -1000);
                    break;
                case 2:
                    Item_LuckyCharm = true;
                    XSetMessage("你买下了一个幸运符！它或许可以在关键时刻救你一命！", 0, 0, 0, 0, -1000);
                    break;
                case 3:
                    Item_SoothingMedicine = true;
                    XSetMessage("你买下了一个舒缓药！它或许可以在关键时刻救你一命！", 0, 0, 0, 0, -1000);
                    break;
            }
            NowDoing = "Main";
            SetButtonTexts();
        }

        // 出去玩（ID 0-3）
        private void GF_GoOutAndPlay()
        {
            Random random_GOAP = new Random();
            int TmpType = random_GOAP.Next(0, 10); // 用于随机是出去玩类型
            int TmpGold = random_GOAP.Next(25, 75); // 非简单难度的花费
            int TmpGetMood = random_GOAP.Next(15, 35); // 加的心情
            if (Friend > 0 && Gold >= 75 || Friend > 0 && trackBarDifficulty.Value == 0)
            {
                if (TmpType <= 7 || comboBoxTarotCard.Text == "皇后" || comboBoxTarotCard.Text == "战车")
                {
                    if (trackBarDifficulty.Value != 0)
                        XSetMessage("你与朋友一起出去玩了！", 0, 0, -1, TmpGetMood, -TmpGold);
                    else
                        XSetMessage("你与朋友一起出去玩了！", 0, 0, -1, TmpGetMood);
                }
                else if (TmpType == 8)
                {
                    if (trackBarDifficulty.Value != 0)
                        XSetMessage("你与朋友一起出去玩了！", 0, 0, -1, TmpGetMood, -TmpGold);
                    else
                        XSetMessage("你与朋友一起出去玩了！中途结交到一个新朋友", 0, 0, -1, TmpGetMood + 20);
                    ++Friend;
                }
                else if (TmpType == 9)
                {
                    if (trackBarDifficulty.Value != 0)
                        XSetMessage("你与朋友一起出去玩了！结果与朋友闹矛盾绝交了！", 0, 0, -5, -30, -TmpGold, -5);
                    else
                        XSetMessage("你与朋友一起出去玩了！结果与朋友闹矛盾绝交了！", 0, 0, -5, -30, -15, -5);
                    --Friend;
                }
            }
            else
            {
                if (TmpType <= 7 || comboBoxTarotCard.Text == "皇后" || comboBoxTarotCard.Text == "战车")
                {
                    if (trackBarDifficulty.Value != 0)
                        XSetMessage("你自己出去玩了！", 0, 0, 0, TmpGetMood - 5, -TmpGold);
                    else
                        XSetMessage("你自己出去玩了！", 0, 0, 0, TmpGetMood - 5);
                }
                else if (TmpType >= 8 && TmpType <= 9)
                {
                    if (trackBarDifficulty.Value != 0)
                        XSetMessage("你与朋友一起出去玩了！中途结交到一个新朋友", 0, 0, 0, TmpGetMood + 25, -TmpGold);
                    else
                        XSetMessage("你与朋友一起出去玩了！中途结交到一个新朋友", 0, 0, 0, TmpGetMood + 25);
                    ++Friend;
                }
            }
        }

        // 医院（ID 3-1）
        private void GF_Hospital()
        {
            int count = 0;
            while (Health < 100 && Gold > 10)
            {
                Health += 1;
                Gold -= 10;

                ++count;
            }


            SetMessage("你走出小镇，去了医院治疗。", 0, 0, 0, 0, -(count * 10), 0, count);

            NowDoing = "Main";
            SetButtonTexts();
        }

        #endregion


        #region 主线前往

        // 首次进入神秘森林（参拜神像）
        private void To_WorshipOfGods()
        {
            NowDoing = "WorshipOfGods";
            if (Item_DarkBlocker)
            {
                XSetMessage("你走出小镇，走向了神秘森林，刚入神秘森林浅处，你就看到了五尊神像。\n在五尊神像地板上写着：“供奉你最诚挚的信仰致辞，换取神明的兴趣。”\n通过再三思考，你决定供奉的神像是……");
                SetButtonTexts("欺诈之神", "命运之神", "死亡之神", "秩序之神", "记忆之神");
            }
            else
            {
                XSetMessage("你走出小镇，走向了████，刚入██████，你就看到了████。\n在██████上写着：“████████████████████”\n通过再三思考，你决定██的██是……");
                SetButtonTexts("████", "████", "████", "████", "████");
            }
            SetButtonEnableds(Item_God == 0 || Item_God == 1, Item_God == 0 || Item_God == 2, Item_God == 0 || Item_God == 3, Item_God == 0 || Item_God == 4, Item_God == 0 || Item_God == 5);
        }

        // 神秘森林岔路口
        private void To_MysteriousForestIntersection()
        {
            NowDoing = "MysteriousForestIntersection";
            if (Item_DarkBlocker)
            {
                SetMessage("你走出小镇，走向了神秘森林，刚入踏入神秘森林，你就看到了五条路。\n正北，西北，东北，正西，正东。每一条路上都有写有字。");
                if (Item_God == 0) SetButtonTexts("正北神像祭坛", "西北时光之地", "东北森林深处", "正西工作房室", "正东塔罗牌墙");
                else if (Item_God == 1) SetButtonTexts("正北神像祭坛", "西北嬉笑之潮", "东北森林深处", "正西工作房室", "正东塔罗牌墙");
                else if (Item_God == 2) SetButtonTexts("正北神像祭坛", "西北命运长廊", "东北森林深处", "正西工作房室", "正东塔罗牌墙");
                else if (Item_God == 3) SetButtonTexts("正北神像祭坛", "西北死亡尸海", "东北森林深处", "正西工作房室", "正东塔罗牌墙");
                else if (Item_God == 4) SetButtonTexts("正北神像祭坛", "西北天锁地牢", "东北森林深处", "正西工作房室", "正东塔罗牌墙");
                else if (Item_God == 5) SetButtonTexts("正北神像祭坛", "西北历史神殿", "东北森林深处", "正西工作房室", "正东塔罗牌墙");
            }
            else
            {
                SetMessage("你走出小镇，走向了████，刚入踏入████，你就看到了███。\n正北，西北，东北，正西，正东。████上都有███。");
                SetButtonTexts("正北████", "西北████", "东北████", "正西████", "正东████");
            }
            SetButtonEnableds(true, false, Item_WinterClothing >= 2 && Item_SummerClothing >= 2, San > 10, comboBoxTarotCard.Text != string.Empty && TC_Wall);
        }

        // 工作房室
        private void To_WorkRoom()
        {
            NowDoing = "WorkRoom";
            if (Item_DarkBlocker)
            {
                SetMessage("你走进了工作房室，入眼就是无数研究工作台，与无数的祭坛、尸体等……\n你打算制作/研究……");
                SetButtonTexts("太阳·御寒服", "月亮·御焰服", "污染干扰器", "暂无", "暂无");
            }
            else
            {
                SetMessage("你走进了████，入眼就是无数█████，与无数的██、██等……\n你打算██/██……");
                SetButtonTexts("██████", "██████", "█████", "暂无", "暂无");
            }
            SetButtonEnableds(
                Item_SunWaterDrop && Item_WinterClothing == 1,
                Item_MoonWaterDrop && Item_SummerClothing == 1,
                Item_EssenceReason >= 2 && Gold >= 2000 && !Item_PollutionJammer,
                false,
                false
            );
        }

        // 森林深处
        private void To_DeepForest()
        {
            NowDoing = "DeepForest";


            if (Item_DarkBlocker)
            {
                SetMessage("你走进了丛林深处，左右两边仿佛仿佛一边是山火，一边是冰川，\n但你凭借着 太阳·御寒服 与 月亮·御焰服 安然走过。\n" +
                           "深处的尽头是一块石板与两个大理石柱子，石板上面刻着两个字——「高塔」");
                SetButtonTexts(L_ForestGrass ? "左通道" : "左草丛", "左大理石柱子", "石板", "右大理石柱子", R_ForestGrass ? "右通道" : "右草丛");
                SetButtonEnableds();
            }
            else
            {
                SetMessage("你走进了████，████仿佛仿佛██是██，██是██，\n但你凭借着 太阳·御寒服 与 月亮·御焰服 安然走过。\n" +
                           "██的██是一块██与两个█████，██上面█████——「██」");
                SetButtonTexts("███", "██████", "██", "██████", "███");
            }

        }

        #endregion


        #region 主线实现

        // 主线 参拜神像（ID 9）
        private void PL_WorshipOfGods(int Id)
        {
            switch (Id)
            {
                // 欺诈
                case 0:
                    if (Item_DarkBlocker)
                    {
                        XSetMessage("你对着代表着欺诈的神像拜了一拜说道：\n“赞美我伟大的欺诈之神，行伟大的欺诈之举”");
                        XAddMessage("欺诈似乎回应了你，一个只有半截的面具飞了过来！\n获得唯一道具：欺诈之面");
                    }
                    else
                    {
                        XSetMessage("你对着█████的██████说道：\n“███████████████████”");
                        XAddMessage("██似乎██了你，██████的███了过来！\n获得唯一道具：████！！");
                    }
                    break;

                // 命运
                case 1:
                    if (Item_DarkBlocker)
                    {
                        XSetMessage("你对着代表着命运的神像拜了一拜说道：\n“赞美我伟大的命运之神，我将重配命运之体”");
                        XAddMessage("命运似乎回应了你，一个永远壹点朝上的骰子飞了过来！\n获得唯一道具：命运之骰");
                    }
                    else
                    {
                        XSetMessage("你对着█████的██████说道：\n“███████████████████”");
                        XAddMessage("██似乎██了你，████████的███了过来！\n获得唯一道具：████！！");
                    }
                    break;

                // 死亡
                case 2:
                    if (Item_DarkBlocker)
                    {
                        XSetMessage("你对着代表着死亡的神像拜了一拜说道：\n“赞美我伟大的死亡之神，万物实向死而生也”");
                        XAddMessage("死亡似乎回应了你，一个散发着浓郁死亡之气的镰刀飞了过来！\n获得唯一道具：死亡之镰");
                    }
                    else
                    {
                        XSetMessage("你对着█████的██████说道：\n“███████████████████”");
                        XAddMessage("██似乎██了你，███████████████的███了过来！\n获得唯一道具：████！！");
                    }
                    break;

                // 秩序
                case 3:
                    if (Item_DarkBlocker)
                    {
                        XSetMessage("你对着代表着秩序的神像拜了一拜说道：\n“赞美我伟大的秩序之神，我等必敬畏秩序也”");
                        XAddMessage("秩序似乎回应了你，一个散发规则的权杖飞了过来！\n获得唯一道具：秩序之杖");
                    }
                    else
                    {
                        XSetMessage("你对着█████的██████说道：\n“███████████████████”");
                        XAddMessage("██似乎██了你，██████的███了过来！\n获得唯一道具：████！！");
                    }
                    break;

                // 记忆
                case 4:
                    if (Item_DarkBlocker)
                    {
                        XSetMessage("你对着代表着时间的神像拜了一拜说道：\n“赞美我伟大的记忆之神，历史与万界为永恒”");
                        XAddMessage("记忆乎回应了你，一个包含了无数知识的书本飞了过来！\n获得唯一道具：记忆之书");
                    }
                    else
                    {
                        XSetMessage("你对着█████的██████说道：\n“███████████████████”");
                        XAddMessage("██似乎██了你，█████████的███了过来！\n获得唯一道具：████！！");
                    }
                    break;
            }

            Item_God = Id + 1;
            NowDoing = "Main";
            SetButtonTexts();
        }

        // 主线 神秘森林岔路口（ID 10）
        private void PL_MysteriousForestIntersection(int Id)
        {
            switch (Id)
            {
                // 正北神像祭坛
                case 0:
                    To_WorshipOfGods();
                    break;

                // 西北各自神掌管的领域
                case 1:
                    break;

                // 东北森林深处
                case 2:
                    if (Item_WinterClothing >= 2 && Item_SummerClothing >= 2)
                    {
                        To_DeepForest();
                    }
                    else
                    {
                        if (Item_DarkBlocker)
                            SetMessage("你看向了森林深处，里面似乎冰火两重天，你不敢进入……");
                        else
                            SetMessage("你看向了████，里面似乎冰火两重天，你不敢进入……");

                        NowDoing = "Main";
                        SetButtonTexts();
                    }
                    break;

                // 正西工作房室
                case 3:
                    To_WorkRoom();
                    break;

                // 正东塔罗牌墙
                case 4:
                    int temp = 0;
                    foreach (bool item in TC)
                        if (item == true)
                            ++temp;
                    if (temp == 22) SetMessage("你走向了正东方的路，看着那一面贴满塔罗牌的墙，\n突然！整个墙瞬间亮起！所有的牌上飞出一道光钻进了你的身体！\n你现在可以中途改变一次你携带的塔罗牌！仅现在！");
                    else SetMessage("你走向了正东方的路，看到了一面墙，上面一共有着 " + temp + " 张塔罗牌。");
                    comboBoxTarotCard.Enabled = temp == 22;
                    TC_Set = temp == 22;
                    TC_Wall = !(temp == 22);
                    NowDoing = "Main";
                    SetButtonTexts();
                    break;
            }
        }

        // 主线 工作房室（ID 22）
        private void PL_WorkRoom(int Id)
        {
            switch (Id)
            {
                // 太阳·御寒服
                case 0:
                    XSetMessage("你通过使用生命能量将 太阳水滴 融入了 御寒服！你得到了 太阳·御寒服！", 0, 0, -5, 5, 0, 0, -15, 50);
                    Item_WinterClothing = 2; Item_SunWaterDrop = false;
                    break;

                // 月亮·御焰服
                case 1:
                    XSetMessage("你通过使用生命能量将 月亮水滴 融入了 御焰服！你得到了 月亮·御焰服！", 0, 0, -5, 5, 0, 0, -15, 50);
                    Item_SummerClothing = 2; Item_MoonWaterDrop = false;
                    break;

                // 污染干扰器
                case 2:
                    XSetMessage("你通过使用理智精华进行疯狂的实验！你得到了 污染干扰器！", -3, -3, -25, 25, 2000, 0, -15, 30);
                    Item_EssenceReason -= 2; Item_PollutionJammer = true;
                    break;
            }
            NowDoing = "Main";
            SetButtonTexts();
        }

        // 主线 问心崖
        private void PL_HeartAskingCliff()
        {
            if (!HAC_AskBody[0] || !HAC_AskBody[1] || !HAC_AskBody[2])
            {
                if (Health > 60)
                    SetMessage("你走向了那问心崖，但你无论如何跑向那天边，你始终无法接近那崖边");
                else if (Health <= 60 && Health > 40)
                    SetMessage("你走向了那问心崖，你的身体仿佛抵达了那崖边，但又无法触及那遥不可及的天边");
                else if (Health <= 40)
                    SetMessage("你走向了那问心崖，你拖着残破的身躯抵达了那崖边，你看到了那空虚的天空");

                HAC_AskBody[0] = Health > 60 || HAC_AskBody[0];
                HAC_AskBody[1] = Health <= 60 && Health > 40 || HAC_AskBody[1];
                HAC_AskBody[2] = Health <= 40 || HAC_AskBody[2];

                if (HAC_AskBody[0] && HAC_AskBody[1] && HAC_AskBody[2] || DeBugMode)
                {
                    AddMessage("在这一刻，你感觉整个人都通透了！仿佛瞬间明白了许多道理");
                    AddMessage("你看了看自己的身体，在这一瞬间，感到身体是如此之陌生！");
                    AddMessage("无数的记忆碎片让你的头非常痛，无数的记忆自行拼接起来。");
                    AddMessage("你看向了你的过去，那是让你非常陌生的过去......");
                }
            }

            else if (!HAC_AskSelf[0] || !HAC_AskSelf[1] || !HAC_AskSelf[2])
            {
                if (Mood >= 100)
                    SetMessage("你走向了那问心崖，那崖边站着个人，你无法看清那人的模样... 就如同自己");
                else if (Mood < 100 && Mood >= 50)
                    SetMessage("你走向了那问心崖，他正在哭泣，你无论如何都无法安慰下他... 就如同自己");
                else if (Mood < 50)
                    SetMessage("你走向了那问心崖，他一跃而下，你无论如何都无法救下他... 就如同自己");

                HAC_AskSelf[0] = Mood >= 100 || HAC_AskSelf[0];
                HAC_AskSelf[1] = Mood < 100 && Mood >= 50 || HAC_AskSelf[1];
                HAC_AskSelf[2] = Mood < 50 || HAC_AskSelf[2];

                if (HAC_AskSelf[0] && HAC_AskSelf[1] && HAC_AskSelf[2] || DeBugMode)
                {
                    AddMessage("在这一刻，你感觉整个人都通透了！仿佛瞬间明白了许多道理");
                    AddMessage("你感受了以下自己，在这一瞬间，感到自己是如此之陌生！");
                    AddMessage("无数的记忆碎片让你的头非常痛，无数的记忆自行拼接起来。");
                    AddMessage("你看向了你的未来，那是让你非常陌生的未来......");
                }
            }

            else if (!HAC_AskSoul[0] || !HAC_AskSoul[1] || !HAC_AskSoul[2])
            {
                if (San >= 100)
                    SetMessage("你走向了那问心崖，但你无论如何跑向那天边，你始终无法接近那崖边");
                else if (San < 100 && San >= 50)
                    SetMessage("你走向了那问心崖，你的身体仿佛抵达了那崖边，但又无法触及那遥不可及的天边");
                else if (San < 50)
                    SetMessage("你走向了那问心崖，你拖着残破的身躯抵达了那崖边，你看到了那空虚的天空");

                HAC_AskSoul[0] = San >= 100 || HAC_AskSoul[0];
                HAC_AskSoul[1] = San < 100 && San >= 50 || HAC_AskSoul[1];
                HAC_AskSoul[2] = San < 50 || HAC_AskSoul[2];

                if (HAC_AskSoul[0] && HAC_AskSoul[1] && HAC_AskSoul[2] || DeBugMode)
                {
                    AddMessage("在这一刻，你感觉整个人都通透了！仿佛瞬间明白了许多道理");
                    AddMessage("你感受了以下自己，在这一瞬间，感到自己是如此之陌生！");
                    AddMessage("无数的记忆碎片让你的头非常痛，无数的记忆自行拼接起来。");
                    AddMessage("你看向了你的现在，那是让你非常陌生的现在......");

                    AddMessage("您看到了真我！游戏结束.");
                    AddMessage("获得塔罗牌：月亮");
                    TC[18] = true;

                    GameOver();
                    TC_Save();
                }
            }

            NowDoing = "Main";
            SetButtonTexts();
        }

        // 森林深处
        private void PL_DeepForest(int Id)
        {
            switch (Id)
            {
                // 左草丛
                case 0:
                    // 通道
                    if (L_ForestGrass)
                    {
                        World = "L_TheEndOfTheContinent";

                        YanName = LTEOTC_KnowYanName ? "岩" : "未知";
                        SetMessage(
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
                            ""
                        );
                        SetWorld(World);
                        To_LTEOTC_Prologue_0();
                    }

                    // 去除草丛
                    else
                    {
                        XSetMessage("你触摸了一下左草丛，你感觉自己身体的能量在被快速抽走……", -500);
                        if (Hunger > 0)
                        {
                            AddMessage("当你回过神之时，草丛早已消失。");
                            L_ForestGrass = true;
                        }

                        // 死亡判断
                        else DeathJudgment();

                        To_DeepForest();
                    }
                    break;

                // 左大理石柱子
                case 1:
                    SetMessage("你触摸了一下左大理石柱子，什么事都没发生……");
                    break;

                // 石板
                case 2:
                    SetMessage("你触摸了一下石板，什么事都没发生……");
                    break;

                // 右大理石柱子
                case 3:
                    SetMessage("你触摸了一下右大理石柱子，什么事都没发生……");
                    break;

                // 右草丛
                case 4:
                    // 通道
                    if (R_ForestGrass)
                    {
                        NowDoing = "R_TheEndOfTheContinent";
                        SetMessage(
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
                            "未知：“你醒了？”\n未知：“伤好点了没？”\n" +
                            "未知：“那群外城的条子真的逼烦，让我们进外城能咋的？”\n" +
                            ""
                        );
                    }

                    // 去除草丛
                    else
                    {
                        XSetMessage("你触摸了一下右草丛，你感觉自己身体的水分在被快速抽走……", 0, -500);
                        if (Thirsty > 0)
                        {
                            AddMessage("当你回过神之时，草丛早已消失。");
                            R_ForestGrass = true;
                        }

                        // 死亡判断
                        else DeathJudgment();

                        To_DeepForest();
                    }
                    break;
            }
        }

        #endregion


        #region 时空碎块：大陆终焉

        // 开篇对话 0
        private void To_LTEOTC_Prologue_0(int Mode = 0)
        {
            if (Mode == 0)
            {
                NowDoing = "LTEOTC_Prologue_0_0";
                SetButtonTexts(
                    "你是谁？",
                    "这是在哪？",
                    "什么情况现在？",
                    LTEOTC_KnowPrologue_0_1 ? "条子为什么抓我们？" : "",
                    LTEOTC_KnowPrologue_0_1 ? "我的手怎么了？" : ""
                );
                SetButtonOptionFont(33F, 33F, 33F, 28F);
                SetButtonEnableds(!LTEOTC_KnowYanName, true, true, LTEOTC_KnowPrologue_0_1, LTEOTC_KnowPrologue_0_1);
                NextBackEnable(LTEOTC_KnowPrologue_0_1);
            }

            else if (Mode == 1)
            {
                NowDoing = "LTEOTC_Prologue_0_1";
                SetButtonTexts(
                    LTEOTC_KnowPrologue_0_1 ? "你受伤了吗？" : "",
                    LTEOTC_KnowPrologue_0_1 ? "接下来怎么办？" : "",
                    "【离开山洞】",
                    "",
                    ""
                );
                SetButtonEnableds(LTEOTC_KnowPrologue_0_1, LTEOTC_KnowPrologue_0_1, LTEOTC_KnowPrologue_KnowHowToLeave, false, false);
            }
        }

        // 开篇对话 0
        private void PL_LTEOTC_Prologue_0(int Id)
        {
            switch (Id)
            {
                case 0:
                    SetMessage(
                        $"{YanName}：“你失忆了？”\n" +
                        $"岩：“我是岩啊，你不记得我了吗？”\n" +
                        $"岩：“算了算了……我们先离开这再说。”"
                    );
                    LTEOTC_KnowYanName = true;
                    YanName = LTEOTC_KnowYanName ? "岩" : "未知";
                    break;

                case 1:
                    SetMessage(
                        $"{YanName}：一个山洞里。\n" +
                        $"{YanName}：我们和内城的条子打了起来，你被射伤了，你昏死过去了！\n" +
                        $"{YanName}：不过放心，这里特别隐蔽，条子绝对不可能找不到这里的。"
                    );
                    LTEOTC_KnowPrologue_0_1 = true;
                    break;

                case 2:
                    SetMessage(
                        $"{YanName}：我们和内城的条子打了起来，你被射伤了，你昏死过去了！\n" +
                        $"{YanName}：我把你带到了这个山洞里，放心，这里特别隐蔽，条子绝对不可能找不到这里的。"
                    );
                    LTEOTC_KnowPrologue_0_1 = true;
                    break;

                case 3:
                    SetMessage(
                        $"{YanName}：“还能为什么？我们是无籍者啊。”\n" +
                        $"{YanName}：“内城那帮人觉得无籍者都是污染携带者，见一个抓一个。”\n" +
                        $"{YanName}：“妈的，我们连外环区都不让进，住山洞里还要被追。”"
                    );
                    break;

                case 4:
                    SetMessage(
                        $"{YanName}：“被条子的理式棍敲了一下，骨头应该没断。”\n" +
                        $"{YanName}：“不过你左手一直在抽……”\n" +
                        $"{YanName}：“算了，回去找疤姨看看吧。”"
                    );
                    break;
            }

            To_LTEOTC_Prologue_0();  // 刷新
        }

        // 开篇对话 0-1
        private void PL_LTEOTC_Prologue_0_1(int Id)
        {
            switch (Id)
            {
                case 0:
                    SetMessage(
                        $"{YanName}：“我没事，皮糙肉厚的。”\n" +
                        $"{YanName}：“倒是你……伤口旁边有点发黑了，别是污染感染。”\n" +
                        $"{YanName}：“回去得让疤姨仔细看看。”"
                    );
                    YanSuspicion -= 3;
                    break;

                case 1:
                    SetMessage(
                        $"{YanName}：“先回外环区，疤姨那边能躲。”\n" +
                        $"{YanName}：“外面条子还在搜，等天黑了再走。”"
                    );
                    YanSuspicion += 2;
                    LTEOTC_KnowPrologue_KnowHowToLeave = true;
                    break;

                case 2:
                    SetMessage(
                        $"{YanName}：“行了，天快黑了，我们出去吧。”\n" +
                        "你和岩从山洞里钻出来，外环区的破败街道在暮色中像一道未愈合的伤口。\n" +
                        "岩拍了拍你的肩膀，朝老炉的废铁回收站走去。\n" +
                        "那里是你们的落脚点，也是你们这个月换口粮的地方。\n\n" +
                        "【序章结束】\n" +
                        "你是一个无籍者，没有户籍，没有家。\n" +
                        "你有一个兄弟叫岩。\n" +
                        "你住在老炉的回收站附近。\n" +
                        "你不知道自己为什么出现在那个隧道里，也不知道那面镜子是什么。\n" +
                        "但你的左手隐隐作痛，像是在提醒你——" + 
                        (LTEOTC_FirstLeave ? "你肯定发生了什么。" : "你又回来了。")
                    );
                    Lost += LTEOTC_FirstLeave ? 5 : 1;
                    break;
            }

            To_LTEOTC_Prologue_0(1);  // 刷新
        }

        #endregion





        private void SetWorld(string _World = "Main")
        {
            NowDoing = "Main";
            if (_World == "Main")
            {

            }

            else if (_World == "L_TheEndOfTheContinent")
            {
                // Label隐藏
                //labelWorkExperience.Visible = false;
                //labelTemperature.Visible = false;
                //labelHAC_AskBody.Visible = false;
                //labelHAC_AskSelf.Visible = false;
                //labelHAC_AskSoul.Visible = false;

                // 初始化（玩家）
                SecGold = 3;
                SecHealth = 10; SecMaxHealth = 50;
                SecHunger = 30; SecMaxHunger = 50;
                SecThirsty = 30; SecMaxThirsty = 50;
                Lost += 3;
                
                // 初始化（NPC）
                YanSuspicion = -60;
            }

            ShowMessage();
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
            {
                return GetGoodEvent(random);
            }
            else if (randValue < goodProb + badProb)
            {
                return GetBadEvent(random);
            }
            else
            {
                return GetNeutralEvent(random);
            }
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
                            //foundItems.Add($"{extraGold}块钱（特殊全满奖励）");
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
                                //foundItems.Add($"{extraGoldForClothes}块钱（衣物全满奖励）");
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
                                //foundItems.Add($"{extraGoldForClothes}块钱（衣物全满奖励）");
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
        private bool DeathJudgment()
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
            Days = 1; Gold = 50; San = 80; Mood = 55; Health = 100; Hunger = 60; MaxHealth = 100; Pollution = 0; BaseLuck = 0; Luck = 0;
            MaxHunger = 100; Thirsty = 60; MaxThirsty = 100; WorkExperience = 0; Temperature = 25; Friend = 1; Lost = 0;
            Natural_San = 5; Natural_Mood = 10; Natural_Health = 1; Natural_Hunger = 10; Natural_Thirsty = 10;
            BasicSalary = 50; AddWorkExperienceMax = 8; MadBuff = 0; TaotieBuff = 0;
            ExtremeColdComes = random.Next(15, 30); EndOfExtremeCold = ExtremeColdComes + random.Next(15, 30);
            ExtremeHotComes = EndOfExtremeCold + random.Next(20, 30); EndOfExtremeHot = ExtremeHotComes + random.Next(15, 30);
            Food_Dumpling = 50; Food_RouJiaMo = 40; Food_RoastedCorn = 10; Food_Lemonade = 10; Food_Coffee = 30;
            Food_Popsicle = 5; Food_PurificationPackage = 1000; Food_HungerJelly = 25; Food_ThirstJelly = 25; Food_GluttonousFeast = 1000;
            NowDoing = "Main"; World = "Main"; PrincipalLine = 0; KnowledgeCd = 0; Morning = true; TC_Wall = true; TC_Set = false;
            G_AvoidingDeath = 0; G_MustGoodRandomEvent = 0; G_AbsoluteRationality = 0; G_DoubleTheValue = 0;
            Item_WinterClothing = 0; Item_God = 0; Item_SummerClothing = 0; Item_EssenceReason = 0;
            Item_CompressedBiscuit = 0; Item_BottledWater = 0; Item_BottledBeverage = 0;
            Item_FirstAidKit = false; Item_Antidote = false; Item_LuckyCharm = false; Item_SoothingMedicine = false;
            Item_DarkBlocker = false; Item_SunWaterDrop = false; Item_MoonWaterDrop = false; Item_PollutionJammer = false;
            L_ForestGrass = false; R_ForestGrass = false;



            ResetCount = 10; SecDays = 1; SecGold = 50; SecSan = 80; SecMood = 55; SecHealth = 100; SecHunger = 60; SecThirsty = 60;
            SecPollution = 0; SecMaxHunger = 100; SecMaxThirsty = 100; SecMaxHealth = 100; SecTemperature = 25;
            SecMorning = true;

            Array.Fill(HAC_AskBody, false); Array.Fill(HAC_AskSelf, false); Array.Fill(HAC_AskSoul, false);
            buttonHome.Enabled = true; checkBoxMessageLog.Checked = false;
            comboBoxTarotCard.Enabled = true; trackBarDifficulty.Enabled = true;
            labelMessage.Text = "你重生了，重生在事故发生前……"; MessageLog = "你出生了......";

            // 难度数值更新
            trackBarDifficulty_Scroll(null, null);

            // 调试模式
            if (DeBugMode)
            {
                Gold = 999999999; //San = 999999999;
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
            switch (World)
            {
                case "Main":
                    switch (NowDoing)
                    {
                        case "Foods_0":
                            To_Foods(1);
                            break;
                        case "MysteryShop_0":
                            To_MysteryShop(1);
                            break;
                        case "Supermarket_0":
                            To_Supermarket(1);
                            break;
                        case "TheWorld_0":
                            To_TheWorld(1);
                            break;
                    }
                    break;

                case "L_TheEndOfTheContinent":
                    switch (NowDoing)
                    {
                        case "LTEOTC_Prologue_0_0":
                            To_LTEOTC_Prologue_0(1);
                            break;
                    }
                    break;
            }


        }

        // 上一页
        private void buttonBack_Click(object sender, EventArgs e)
        {
            switch (World)
            {
                case "Main":
                    switch (NowDoing)
                    {
                        case "Foods_1":
                            To_Foods(0);
                            break;
                        case "MysteryShop_1":
                            To_MysteryShop(0);
                            break;
                        case "Supermarket_1":
                            To_Supermarket(0);
                            break;
                        case "TheWorld_1":
                            To_TheWorld(0);
                            break;
                    }
                    break;

                case "L_TheEndOfTheContinent":
                    switch (NowDoing)
                    {
                        case "LTEOTC_Prologue_0_1":
                            To_LTEOTC_Prologue_0(0);
                            break;
                    }
                    break;
            }
        }



        #region 基础函数

        // 信息文本核心
        private string MessageMain(string message, int hunger = 0, int thirsty = 0, int san = 0, int mood = 0, int gold = 0, int workExperience = 0, int health = 0, int pollution = 0)
        {
            string Str = message;

            if (hunger != 0 || thirsty != 0 || san != 0 || mood != 0 || gold != 0 || workExperience != 0 || health != 0)
            {
                Str += '\n';

                if (hunger > 0)
                    Str += "饥饿值 + " + hunger + ' ';
                else if (hunger < 0)
                    Str += "饥饿值 - " + Math.Abs(hunger) + ' ';

                if (thirsty > 0)
                    Str += "口渴值 + " + thirsty + ' ';
                else if (thirsty < 0)
                    Str += "口渴值 - " + Math.Abs(thirsty) + ' ';

                if (san > 0)
                    Str += "理智 + " + san + "% ";
                else if (san < 0)
                    Str += "理智 - " + Math.Abs(san) + "% ";

                if (mood > 0)
                    Str += "心情 + " + mood + "% ";
                else if (mood < 0)
                    Str += "心情 - " + Math.Abs(mood) + "% ";

                if (health > 0)
                    Str += "健康 + " + health + "% ";
                else if (health < 0)
                    Str += "健康 - " + Math.Abs(health) + "% ";

                if (gold > 0)
                    Str += "金币 + " + gold + "G ";
                else if (gold < 0)
                    Str += "金币 - " + Math.Abs(gold) + "G ";

                if (workExperience > 0)
                    Str += "工作经验 + " + workExperience + "%";
                else if (workExperience < 0)
                    Str += "工作经验 - " + Math.Abs(workExperience) + "%";

                if (pollution > 0)
                    Str += "污染 + " + pollution + "%";
                else if (pollution < 0)
                    Str += "污染 - " + Math.Abs(pollution) + "%";
            }


            return Str;
        }

        // 添加信息
        private void AddMessage(string message, int hunger = 0, int thirsty = 0, int san = 0, int mood = 0, int gold = 0, int workExperience = 0, int health = 0, int pollution = 0)
        {
            string Str = MessageMain(message, hunger, thirsty, san, mood, gold, workExperience, health, pollution);

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
        private void SetMessage(string message, int hunger = 0, int thirsty = 0, int san = 0, int mood = 0, int gold = 0, int workExperience = 0, int health = 0, int pollution = 0)
        {
            string Str = MessageMain(message, hunger, thirsty, san, mood, gold, workExperience, health, pollution);

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
        private void XSetMessage(string message, int hunger = 0, int thirsty = 0, int san = 0, int mood = 0, int gold = 0, int workExperience = 0, int health = 0, int pollution = 0)
        {
            MessageMakeData(ref hunger, ref thirsty, ref san, ref mood, ref gold, ref workExperience, ref health, ref pollution);
            SetMessage(message, hunger, thirsty, san, mood, gold, workExperience, health, pollution);
            MessageSetData(hunger, thirsty, san, mood, gold, workExperience, health, pollution);
        }

        // 添加信息 & 属性加减生效
        private void XAddMessage(string message, int hunger = 0, int thirsty = 0, int san = 0, int mood = 0, int gold = 0, int workExperience = 0, int health = 0, int pollution = 0)
        {
            MessageMakeData(ref hunger, ref thirsty, ref san, ref mood, ref gold, ref workExperience, ref health, ref pollution);
            AddMessage(message, hunger, thirsty, san, mood, gold, workExperience, health, pollution);
            MessageSetData(hunger, thirsty, san, mood, gold, workExperience, health, pollution);
        }

        private void MessageSetData(int hunger, int thirsty, int san, int mood, int gold, int workExperience, int health, int pollution)
        {
            Hunger += hunger;
            Thirsty += thirsty;
            San += san;
            Mood += mood;
            Gold += gold;
            Pollution += pollution;
            WorkExperience += workExperience;

            // 工作经验上限
            if (WorkExperience > 150 && comboBoxTarotCard.Text != "月亮") WorkExperience = 150;
            else if (WorkExperience > 100 && comboBoxTarotCard.Text == "月亮") WorkExperience = 100;

            // 生命值
            Health += health; if (Health > MaxHealth) Health = MaxHealth;
        }

        // 数据加成
        private void MessageMakeData(ref int hunger, ref int thirsty, ref int san, ref int mood, ref int gold, ref int workExperience, ref int health, ref int pollution)
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
            }

            if (DeBug_2Gold)
                gold *= 2;

            MessageTarotCard(ref hunger, ref thirsty, ref san, ref mood, ref gold, ref workExperience, ref health, ref pollution);
        }

        // 判断事件类型的方法
        private bool IsGoodEvent(int eventType)
        {
            int[] goodEvents = { 3, 7, 9, 11, 15, 22, 25, 26, 28, 32 };
            return goodEvents.Contains(eventType);
        }

        private bool IsBadEvent(int eventType)
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
        private int GetGoodEvent(Random random)
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
        private void MessageTarotCard(ref int hunger, ref int thirsty, ref int san, ref int mood, ref int gold, ref int workExperience, ref int health, ref int pollution)
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
                if (OutMessages)
                    MessageLog += $"{message} ({Days}.{Morning})\n";
                else
                    MessageLog += message + '\n';
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
        private void NextBackEnable(bool enab = true)
        {
            buttonNext.Enabled = enab;
            buttonBack.Enabled = enab;
        }

        // 按钮字体大小编辑启用
        private void SetButtonOptionFont(Single FontSize0 = 33F, Single FontSize1 = 33F, Single FontSize2 = 33F, Single FontSize3 = 33F, Single FontSize4 = 33F)
        {
            buttonOption0.Font = new System.Drawing.Font("Microsoft Sans Serif", FontSize0, FontStyle.Bold);
            buttonOption1.Font = new System.Drawing.Font("Microsoft Sans Serif", FontSize1, FontStyle.Bold);
            buttonOption2.Font = new System.Drawing.Font("Microsoft Sans Serif", FontSize2, FontStyle.Bold);
            buttonOption3.Font = new System.Drawing.Font("Microsoft Sans Serif", FontSize3, FontStyle.Bold);
            buttonOption4.Font = new System.Drawing.Font("Microsoft Sans Serif", FontSize4, FontStyle.Bold);
        }

        // 设置按钮文本
        private void SetButtonTexts(string bt0 = "去挣钱", string bt1 = "美食街", string bt2 = "知识区", string bt3 = "出去玩", string bt4 = "去冒险")
        {
            buttonOption0.Text = bt0;
            buttonOption1.Text = bt1;
            buttonOption2.Text = bt2;
            buttonOption3.Text = bt3;
            buttonOption4.Text = bt4;
        }

        // 设置按钮启用
        private void SetButtonEnableds(bool bt0 = true, bool bt1 = true, bool bt2 = true, bool bt3 = true, bool bt4 = true)
        {
            buttonOption0.Enabled = bt0;
            buttonOption1.Enabled = bt1;
            buttonOption2.Enabled = bt2;
            buttonOption3.Enabled = bt3;
            buttonOption4.Enabled = bt4;
            SetButtonEnabledsColor(bt0, bt1, bt2, bt3, bt4);
        }

        // 设置按钮启用颜色
        private void SetButtonEnabledsColor(bool bt0, bool bt1, bool bt2, bool bt3, bool bt4)
        {
            if (bt0) buttonOption0.BackColor = Color.White; else buttonOption0.BackColor = Color.Gainsboro;
            if (bt1) buttonOption1.BackColor = Color.White; else buttonOption1.BackColor = Color.Gainsboro;
            if (bt2) buttonOption2.BackColor = Color.White; else buttonOption2.BackColor = Color.Gainsboro;
            if (bt3) buttonOption3.BackColor = Color.White; else buttonOption3.BackColor = Color.Gainsboro;
            if (bt4) buttonOption4.BackColor = Color.White; else buttonOption4.BackColor = Color.Gainsboro;
        }

        // 塔罗牌保存
        private void TC_Save()
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
        private void GameOver()
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
                Directory.CreateDirectory(@"C:\QiAppDatas\Logs\QisToolkit3\");
                File.WriteAllText(@"C:\QiAppDatas\Logs\QisToolkit3\问身 问己 问心.log", MessageLog);

                // 开发者模式自动打开日志文件
                if (DeBugMode)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = @"C:\QiAppDatas\Logs\QisToolkit3\问身 问己 问心.log",
                        UseShellExecute = true  // 显式启用 ShellExecute
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出日志出现错误，请联系开发人员。\n报错信息：{ex.Message}\n\n完整报错：\n{ex}", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (Item_CompressedBiscuit <= 0 || World != "Main")
                return;

            Item_CompressedBiscuit -= 1;
            XAddMessage("你吃了一块压缩饼干。", 60, -10, -1, -5, 0, 0, -3);
            ShowMessage(); DeathJudgment();
        }

        // 瓶装饮料
        private void labelBottledBeverage_Click(object sender, EventArgs e)
        {
            if (Item_BottledBeverage <= 0 || World != "Main")
                return;

            Item_BottledBeverage -= 1;
            XAddMessage("你喝一瓶饮料。", 10, 35, -5, 40, 0, 0, -10);
            ShowMessage(); DeathJudgment();
        }

        // 矿泉水
        private void labelBottledWater_Click(object sender, EventArgs e)
        {
            if (Item_BottledWater <= 0 || World != "Main")
                return;

            Item_BottledWater -= 1;
            XAddMessage("你喝一瓶饮料。", -5, 50, -1, 2, 0, 0, -1);
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
            NowDoing = "Main";
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
            //labelMessage.Text =
            //    "通关目标：坚持生活100天！\r\n游戏内容：你每天只能干两件事，知识区每天只能去一次。" +
            //    "\r\n\r\n注意事项：\r\n1. 饥饿值与口渴值大于100或小于等于 0 会死。\r\n\r\n" +
            //    "2. 理智低于0会死，心情低于10会掉理智，很危险。\r\n\r\n" +
            //    "3. 各项数值每天都会减少。\r\n\r\n" +
            //    "4. 饥饿、口渴可以通过去美食街回复，理智可以通过知识来回复，\r\n心情可以通过出门玩回复。健康通过医院回复，医院在冒险区。\r\n\r\n" +
            //    "5. 每天会有事件，造成各项数值波动。\r\n\r\n" +
            //    "6. 极端天气需要用到特殊道具特殊道具在冒险区，\r\n需要达到特定理智与金钱才能开启。\r\n\r\n" +
            //    "7. 游戏存在隐藏主线，找到主线才能探明真相。\r\n请自行探索哦~（主线参考了许多作品qwq）\r\n\r\n" +
            //    "祝您游戏愉快~";

            //SetMessage(
            //    "你走向了那漆黑且望不尽头的隧道，勇敢的走了进去！\n" +
            //    "你每走出一步，你的脚步就变重一分……\n" +
            //    "不知道走了多久，你的眼前出现了出口！那出口亮的你睁不开眼！\n" +
            //    "那是隧道仿佛是另一个世界！那是一座繁华的城市，灯光无数！\n" +
            //    "但奇怪的是，这个城市仿佛毫无生机！或者说，一切都事物都静止着！\n" +
            //    "当你看到这座城市时，你的脑海中出现了无数声音，在催促你进入城市！\n" +
            //    "你走向了这座城市，下一刻，你感觉到意识在不断远离自己的身体！\n" +
            //    "你的意识越来越模糊…… 直至彻底昏厥。\n" +
            //    "你再次醒来，你发现你身处一个山洞之中。\n" +
            //    ""
            //);

            //SetMessage(
            //    "test test test test test test test test test test test test test test test test test test test test " +
            //    "test test test test test test test test test test test test test test test test test test test test " +
            //    "test test test test test test test test test test test test test test test test test test test test " +
            //    "test test test test test test test test test test test test test test test test test test test test " +
            //    "test test test test test test test test test test test test test test test test test test test test ");
            if (DeBugMode)
            {
                L_ForestGrass = true;
                PL_DeepForest(0);
            }
        }

        private void button_OpenGameDoc_Click(object sender, EventArgs e)
        {
            string filePath = $@"{Qi.QisToolkit3_Datas.actualDirectory}\《问身 问己 问心》游戏介绍.docx";

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
            GameOver();
            //MessageBox.Show(MessageLog);
        }
    }
}
