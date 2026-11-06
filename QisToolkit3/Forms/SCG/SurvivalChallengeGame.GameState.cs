using System;
using System.Collections.Generic;

namespace QisToolkit3.Forms.SCG
{
    // ============ 跨世界共享（玩家本体的属性） ============
    public class GlobalState
    {
        public int San = 80, Mood = 55, Pollution = 0, Lost = 0;
        public int BaseLuck = 0, Luck = 0, WorkExperience = 0;
        public int PrincipalLine = 0, KnowledgeCd = 0;
        public int MadBuff = 0, TaotieBuff = 0;
        public int ResetCount = 10;

        public int Item_God = 0, Item_EssenceReason = 0;
        public int Item_WinterClothing = 0, Item_SummerClothing = 0;
        public int Item_CompressedBiscuit = 0, Item_BottledWater = 0, Item_BottledBeverage = 0;
        public int Item_Scrap = 0, Item_QualityScrap = 0;
        public bool Item_DarkBlocker = false, Item_SunWaterDrop = false, Item_MoonWaterDrop = false;
        public bool Item_PollutionJammer = false, Item_FirstAidKit = false, Item_Antidote = false;
        public bool Item_LuckyCharm = false, Item_SoothingMedicine = false;

        public int G_AvoidingDeath = 0, G_MustGoodRandomEvent = 0,
                   G_AbsoluteRationality = 0, G_DoubleTheValue = 0;

        public bool KnowVoid = false;
        public string MessageLog = "你出生了......";
        public bool[] TC = new bool[22];
        public bool TC_Wall = true, TC_Set = false;
        public bool[] HAC_AskBody = new bool[3];
        public bool[] HAC_AskSelf = new bool[3];
        public bool[] HAC_AskSoul = new bool[3];
        public bool L_ForestGrass = false, R_ForestGrass = false;

        // 时空碎块：大陆终焉剧情标记
        public bool LTEOTC_KnowYanName = false;
        public bool LTEOTC_KnowPrologue_0_1 = false;
        public bool LTEOTC_KnowPrologue_KnowHowToLeave = false;
        public bool LTEOTC_KnowOuterCity_RecyclingYard = true;
        public bool LTEOTC_KnowOuterCity_RecyclingYard_BackdoorTrade = true;
        public bool LTEOTC_KnowOuterCity_UndergroundClinic = false;
        public bool LTEOTC_KnowOuterCity_GarbageMountain = false;
        public bool LTEOTC_KnowOuterCity_BlackMarket = false;
        public bool LTEOTC_KnowOuterCity_Home = true;
        public bool LTEOTC_FirstLeave = true;
        public bool LTEOTC_UnlockTheGarbageMountainPlot = true;
        public string YanName = "未知";
        public int YanSuspicion = -60;
    }

    // ============ 每个世界 / 副本私有 ============
    public class WorldState
    {
        public WorldId Id = WorldId.Main;
        public SceneId NowDoing = SceneId.Main;
        public int Days = 1, Gold = 50, Health = 100;
        public int Hunger = 60, Thirsty = 60;
        public int MaxHealth = 100, MaxHunger = 100, MaxThirsty = 100;
        public int Temperature = 25, Friend = 1;
        public bool Morning = true;
    }

    public static class GameState
    {
        static GameState()
        {
            Worlds.Push(new WorldState()); 
        }

        // ---- 基础工具 ----
        public static readonly Random random = new Random();
        public static string SetMessageTextTemporarilyStored = string.Empty;
        public static bool DeBugMode = false, DeBug_2Gold = false;

        // ---- 状态容器 ----
        public static GlobalState Global = new GlobalState();
        public static readonly Stack<WorldState> Worlds = new Stack<WorldState>();
        public static WorldState Current => Worlds.Peek();

        // ---- 配置 / 常数 ----
        public static int Natural_San = 5, Natural_Mood = 10, Natural_Health = 1,
                          Natural_Hunger = 10, Natural_Thirsty = 10;

        public static int BasicSalary = 50, AddWorkExperienceMax = 8;

        public static int ExtremeColdComes = random.Next(20, 30),
                          EndOfExtremeCold = ExtremeColdComes + random.Next(10, 25);

        public static int ExtremeHotComes = EndOfExtremeCold + random.Next(10, 20),
                          EndOfExtremeHot = ExtremeHotComes + random.Next(10, 25);

        public static int Food_Dumpling = 50, Food_RouJiaMo = 40, Food_RoastedCorn = 10,
                          Food_Lemonade = 10, Food_Coffee = 30;

        public static int Food_Popsicle = 5, Food_PurificationPackage = 1000,
                          Food_HungerJelly = 25, Food_ThirstJelly = 25, Food_GluttonousFeast = 1000;

        // ============ 属性包装器：世界私有（指向 Current） ============
        public static int Days { get => Current.Days; set => Current.Days = value; }
        public static int Gold { get => Current.Gold; set => Current.Gold = value; }
        public static int Health { get => Current.Health; set => Current.Health = value; }
        public static int Hunger { get => Current.Hunger; set => Current.Hunger = value; }
        public static int Thirsty { get => Current.Thirsty; set => Current.Thirsty = value; }
        public static int MaxHealth { get => Current.MaxHealth; set => Current.MaxHealth = value; }
        public static int MaxHunger { get => Current.MaxHunger; set => Current.MaxHunger = value; }
        public static int MaxThirsty { get => Current.MaxThirsty; set => Current.MaxThirsty = value; }
        public static int Temperature { get => Current.Temperature; set => Current.Temperature = value; }
        public static int Friend { get => Current.Friend; set => Current.Friend = value; }
        public static bool Morning { get => Current.Morning; set => Current.Morning = value; }
        public static WorldId World { get => Current.Id; set => Current.Id = value; }
        public static SceneId NowDoing { get => Current.NowDoing; set => Current.NowDoing = value; }

        // ============ 属性包装器：全局共享（指向 Global） ============
        public static int San { get => Global.San; set => Global.San = value; }
        public static int Mood { get => Global.Mood; set => Global.Mood = value; }
        public static int Pollution { get => Global.Pollution; set => Global.Pollution = value; }
        public static int Lost { get => Global.Lost; set => Global.Lost = value; }
        public static int BaseLuck { get => Global.BaseLuck; set => Global.BaseLuck = value; }
        public static int Luck { get => Global.Luck; set => Global.Luck = value; }
        public static int WorkExperience { get => Global.WorkExperience; set => Global.WorkExperience = value; }
        public static int PrincipalLine { get => Global.PrincipalLine; set => Global.PrincipalLine = value; }
        public static int KnowledgeCd { get => Global.KnowledgeCd; set => Global.KnowledgeCd = value; }
        public static int MadBuff { get => Global.MadBuff; set => Global.MadBuff = value; }
        public static int TaotieBuff { get => Global.TaotieBuff; set => Global.TaotieBuff = value; }
        public static int ResetCount { get => Global.ResetCount; set => Global.ResetCount = value; }
        public static int Item_God { get => Global.Item_God; set => Global.Item_God = value; }
        public static int Item_EssenceReason { get => Global.Item_EssenceReason; set => Global.Item_EssenceReason = value; }
        public static int Item_WinterClothing { get => Global.Item_WinterClothing; set => Global.Item_WinterClothing = value; }
        public static int Item_SummerClothing { get => Global.Item_SummerClothing; set => Global.Item_SummerClothing = value; }
        public static int Item_CompressedBiscuit { get => Global.Item_CompressedBiscuit; set => Global.Item_CompressedBiscuit = value; }
        public static int Item_BottledWater { get => Global.Item_BottledWater; set => Global.Item_BottledWater = value; }
        public static int Item_BottledBeverage { get => Global.Item_BottledBeverage; set => Global.Item_BottledBeverage = value; }
        public static int Item_Scrap { get => Global.Item_Scrap; set => Global.Item_Scrap = value; }
        public static int Item_QualityScrap { get => Global.Item_QualityScrap; set => Global.Item_QualityScrap = value; }
        public static bool Item_DarkBlocker { get => Global.Item_DarkBlocker; set => Global.Item_DarkBlocker = value; }
        public static bool Item_SunWaterDrop { get => Global.Item_SunWaterDrop; set => Global.Item_SunWaterDrop = value; }
        public static bool Item_MoonWaterDrop { get => Global.Item_MoonWaterDrop; set => Global.Item_MoonWaterDrop = value; }
        public static bool Item_PollutionJammer { get => Global.Item_PollutionJammer; set => Global.Item_PollutionJammer = value; }
        public static bool Item_FirstAidKit { get => Global.Item_FirstAidKit; set => Global.Item_FirstAidKit = value; }
        public static bool Item_Antidote { get => Global.Item_Antidote; set => Global.Item_Antidote = value; }
        public static bool Item_LuckyCharm { get => Global.Item_LuckyCharm; set => Global.Item_LuckyCharm = value; }
        public static bool Item_SoothingMedicine { get => Global.Item_SoothingMedicine; set => Global.Item_SoothingMedicine = value; }
        public static int G_AvoidingDeath { get => Global.G_AvoidingDeath; set => Global.G_AvoidingDeath = value; }
        public static int G_MustGoodRandomEvent { get => Global.G_MustGoodRandomEvent; set => Global.G_MustGoodRandomEvent = value; }
        public static int G_AbsoluteRationality { get => Global.G_AbsoluteRationality; set => Global.G_AbsoluteRationality = value; }
        public static int G_DoubleTheValue { get => Global.G_DoubleTheValue; set => Global.G_DoubleTheValue = value; }
        public static bool KnowVoid { get => Global.KnowVoid; set => Global.KnowVoid = value; }
        public static string MessageLog { get => Global.MessageLog; set => Global.MessageLog = value; }
        public static bool[] TC => Global.TC;
        public static bool TC_Wall { get => Global.TC_Wall; set => Global.TC_Wall = value; }
        public static bool TC_Set { get => Global.TC_Set; set => Global.TC_Set = value; }
        public static bool[] HAC_AskBody => Global.HAC_AskBody;
        public static bool[] HAC_AskSelf => Global.HAC_AskSelf;
        public static bool[] HAC_AskSoul => Global.HAC_AskSoul;
        public static bool L_ForestGrass { get => Global.L_ForestGrass; set => Global.L_ForestGrass = value; }
        public static bool R_ForestGrass { get => Global.R_ForestGrass; set => Global.R_ForestGrass = value; }
        public static bool LTEOTC_KnowYanName { get => Global.LTEOTC_KnowYanName; set => Global.LTEOTC_KnowYanName = value; }
        public static bool LTEOTC_KnowPrologue_0_1 { get => Global.LTEOTC_KnowPrologue_0_1; set => Global.LTEOTC_KnowPrologue_0_1 = value; }
        public static bool LTEOTC_KnowPrologue_KnowHowToLeave { get => Global.LTEOTC_KnowPrologue_KnowHowToLeave; set => Global.LTEOTC_KnowPrologue_KnowHowToLeave = value; }
        public static bool LTEOTC_KnowOuterCity_RecyclingYard { get => Global.LTEOTC_KnowOuterCity_RecyclingYard; set => Global.LTEOTC_KnowOuterCity_RecyclingYard = value; }
        public static bool LTEOTC_KnowOuterCity_RecyclingYard_BackdoorTrade { get => Global.LTEOTC_KnowOuterCity_RecyclingYard_BackdoorTrade; set => Global.LTEOTC_KnowOuterCity_RecyclingYard_BackdoorTrade = value; }
        public static bool LTEOTC_KnowOuterCity_UndergroundClinic { get => Global.LTEOTC_KnowOuterCity_UndergroundClinic; set => Global.LTEOTC_KnowOuterCity_UndergroundClinic = value; }
        public static bool LTEOTC_KnowOuterCity_GarbageMountain { get => Global.LTEOTC_KnowOuterCity_GarbageMountain; set => Global.LTEOTC_KnowOuterCity_GarbageMountain = value; }
        public static bool LTEOTC_KnowOuterCity_BlackMarket { get => Global.LTEOTC_KnowOuterCity_BlackMarket; set => Global.LTEOTC_KnowOuterCity_BlackMarket = value; }
        public static bool LTEOTC_KnowOuterCity_Home { get => Global.LTEOTC_KnowOuterCity_Home; set => Global.LTEOTC_KnowOuterCity_Home = value; }
        public static bool LTEOTC_FirstLeave { get => Global.LTEOTC_FirstLeave; set => Global.LTEOTC_FirstLeave = value; }
        public static bool LTEOTC_UnlockTheGarbageMountainPlot { get => Global.LTEOTC_UnlockTheGarbageMountainPlot; set => Global.LTEOTC_UnlockTheGarbageMountainPlot = value; }
        public static string YanName { get => Global.YanName; set => Global.YanName = value; }
        public static int YanSuspicion { get => Global.YanSuspicion; set => Global.YanSuspicion = value; }

        // ---- 幸运值 ----
        public static int DisplayLuckValue => LuckValue + 100;
        public static int LuckValue
        {
            get => Luck;
            set => Luck = Math.Clamp(value, -100, 100);
        }
    }

    public enum WorldId
    {
        Main,
        L_TheEndOfTheContinent,
    }

    public enum SceneId
    {
        Main,

        // 主世界页
        Foods,
        EarnMoney,
        TheWorld,
        MysteryShop,
        KnowledgeArea,
        Supermarket,
        WorshipOfGods,
        MysteriousForestIntersection,
        WorkRoom,
        DeepForest,

        // 时空碎块：大陆终焉
        LTEOTC_Prologue_0,
        LTEOTC_OuterCityMain,
    }
}