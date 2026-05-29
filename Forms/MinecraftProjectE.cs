using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QisToolkit3.Forms
{
    public partial class MinecraftProjectE : Form
    {
        string FilePath = @"C:\Users\Administrator\AppData\Roaming\.minecraft\config\ProjectE\custom_emc.json";
        bool TheHead = true;
        int OutType = 0;


        public MinecraftProjectE()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);

            comboBox.SelectedIndex = 0;
        }

        private void MinecraftProjectE_Load(object sender, EventArgs e)
        {
            FileOpenShowMain(false);
            LoadSaveFile();
        }

        private void buttonOpenMinecraft_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                FilePath = openFileDialog.FileName;
            FileOpenShowMain();
        }

        private void buttonSet_Click(object sender, EventArgs e) => WriteEmcFileMain();

        private void buttonSave_Click(object sender, EventArgs e) => OutSaveFile();

        private void LoadSaveFile()
        {
            Directory.CreateDirectory(@"C:\QiAppDatas\Datas\QisToolkit3");
            if (File.Exists(@"C:\QiAppDatas\Datas\QisToolkit3\MinecraftProjectE.qidata"))
            {
                using (StreamReader reader = new StreamReader(@"C:\QiAppDatas\Datas\QisToolkit3\MinecraftProjectE.qidata"))
                {
                    FilePath = reader.ReadLine(); FileOpenShowMain(false);

                    #region 原版 & 常用功能

                    checkBox_raw_iron.Checked = Qi.StrToBool(reader.ReadLine());
                    checkBox_raw_copper.Checked = Qi.StrToBool(reader.ReadLine());
                    checkBox_raw_gold.Checked = Qi.StrToBool(reader.ReadLine());
                    checkBox_ancient_debris.Checked = Qi.StrToBool(reader.ReadLine());
                    checkBox_experience_bottle.Checked = Qi.StrToBool(reader.ReadLine());
                    checkBox_wither_skeleton_skull.Checked = Qi.StrToBool(reader.ReadLine());
                    checkBox_dragon_head.Checked = Qi.StrToBool(reader.ReadLine());
                    checkBox_bee_nest.Checked = Qi.StrToBool(reader.ReadLine());
                    checkBox_totem_of_undying.Checked = Qi.StrToBool(reader.ReadLine());

                    #endregion

                    checkBox_Cyclic.Checked = Qi.StrToBool(reader.ReadLine());
                    checkBox_FarmersDelight.Checked = Qi.StrToBool(reader.ReadLine());
                    checkBox_Create.Checked = Qi.StrToBool(reader.ReadLine());
                    checkBox_Artifacts.Checked = Qi.StrToBool(reader.ReadLine());
                }
            }
        }

        private void OutSaveFile()
        {
            Directory.CreateDirectory(@"C:\QiAppDatas\Datas\QisToolkit3");
            using (StreamWriter writer = new StreamWriter(@"C:\QiAppDatas\Datas\QisToolkit3\MinecraftProjectE.qidata"))
            {
                writer.WriteLine(FilePath);

                #region 原版 & 常用功能

                writer.WriteLine(checkBox_raw_iron.Checked);
                writer.WriteLine(checkBox_raw_copper.Checked);
                writer.WriteLine(checkBox_raw_gold.Checked);
                writer.WriteLine(checkBox_ancient_debris.Checked);
                writer.WriteLine(checkBox_experience_bottle.Checked);
                writer.WriteLine(checkBox_wither_skeleton_skull.Checked);
                writer.WriteLine(checkBox_dragon_head.Checked);
                writer.WriteLine(checkBox_bee_nest.Checked);
                writer.WriteLine(checkBox_totem_of_undying.Checked);

                #endregion

                writer.WriteLine(checkBox_Cyclic.Checked);
                writer.WriteLine(checkBox_FarmersDelight.Checked);
                writer.WriteLine(checkBox_Create.Checked);
                writer.WriteLine(checkBox_Artifacts.Checked);
            }
        }

        private void WriteEmcFileMain()
        {
            File.WriteAllText(FilePath, "{\r\n  \"entries\": [");
            TheHead = true;


            OutJson_Minecraft();                                        // 原版
            OutJson_Cyclic(checkBox_Cyclic.Checked);                    // 循环
            OutJson_FarmersDelight(checkBox_FarmersDelight.Checked);    // 农夫乐事
            OutJson_Create(checkBox_Create.Checked);                    // 机械动力
            OutJson_Artifacts(checkBox_Artifacts.Checked);              // 奇异饰品
            OutJson_LuckyBlock(checkBox_LuckyBlock.Checked);            // 幸运方块
            OutJson_Pixelmon(checkBox_Pixelmon.Checked);                // 像素宝可梦 重铸


            File.AppendAllText(FilePath, "\r\n  ]\r\n}");

            OutSaveFile();
        }





        #region 基础函数

        // 打开文件处理
        private void FileOpenShowMain(bool IsOutFileNotExists = true)
        {
            if (File.Exists(FilePath))
            {
                labelOpenFile.Text = "当前打开：" + FilePath;
                buttonSet.Enabled = true;
                buttonSave.Enabled = true;
                tabControl.Enabled = true;
            }
            else if (IsOutFileNotExists)
                MessageBox.Show("文件 " + FilePath + " 不存在！", "提示", MessageBoxButtons.OK);
        }

        private void OutJsonMain(string Name, string Emc)
        {
            string str = "\r\n    {\r\n      \"item\": \"" + Name + "\",\r\n      \"emc\": " + Emc + "\r\n    }";

            if (!TheHead)
            {
                str = "," + str;
            }

            TheHead = false;
            File.AppendAllText(FilePath, str);
        }

        private void NewOutJsonMain(string Name, string Emc, string Data = null)
        {
            string str = "\r\n    {\r\n      \"id\": \"" + Name + "\",";


            if (!TheHead)
            {
                str = "," + str;
            }

            TheHead = false;
            if (Data != null)
                str += "\r\n      \"emc\": " + Emc + "\r\n    }";

            else
                str += "\r\n      \"data\": {\r\n        " + Data + "\r\n      },\r\n      \"emc\": " + Emc + "\r\n    }";


            File.AppendAllText(FilePath, str);
        }

        private void TOutJsonMain(string Name, string Emc)
        {
            if (OutType == 0) OutJsonMain(Name, Emc);
            else NewOutJsonMain(Name, Emc);
        }

        #endregion

        #region 输出 JSON 打包

        private void OutJson_Minecraft()
        {
            if (checkBox_raw_iron.Checked) TOutJsonMain("minecraft:raw_iron", "256");
            if (checkBox_raw_copper.Checked) TOutJsonMain("minecraft:raw_copper", "128");
            if (checkBox_raw_gold.Checked) TOutJsonMain("minecraft:raw_gold", "2048");
            if (checkBox_ancient_debris.Checked) TOutJsonMain("minecraft:ancient_debris", "12288");
            if (checkBox_experience_bottle.Checked) TOutJsonMain("minecraft:experience_bottle", "5000");
            if (checkBox_wither_skeleton_skull.Checked) TOutJsonMain("minecraft:wither_skeleton_skull", "46356");
            if (checkBox_dragon_head.Checked) TOutJsonMain("minecraft:dragon_head", "2048");
            if (checkBox_bee_nest.Checked) TOutJsonMain("minecraft:bee_nest", "1684");
            if (checkBox_totem_of_undying.Checked) TOutJsonMain("minecraft:totem_of_undying", "524288");
            if (checkBox_QisHead.Checked) TOutJsonMain("minecraft:player_head{SkullOwner:{Id:[I;1140890653,740968604,-1248505192,922747850],Name:\\\"QiNB_666\\\",Properties:{textures:[{Value:\\\"ewogICJ0aW1lc3RhbXAiIDogMTc0ODg0OTYxNDAzMywKICAicHJvZmlsZUlkIiA6ICI0NDAwOWMxZDJjMmE0ODljYjU5NTUyOTgzNzAwMDNjYSIsCiAgInByb2ZpbGVOYW1lIiA6ICJRaU5CXzY2NiIsCiAgInRleHR1cmVzIiA6IHsKICAgICJTS0lOIiA6IHsKICAgICAgInVybCIgOiAiaHR0cDovL3RleHR1cmVzLm1pbmVjcmFmdC5uZXQvdGV4dHVyZS8yMmY4MDU1NmM4MjhkNjY3ZGVkNGZhODdhZmU0YmM0ZDNhOWI3NGYwYzY4MTU0ZDdjMzVlNjg3YWRkZDkwNzciLAogICAgICAibWV0YWRhdGEiIDogewogICAgICAgICJtb2RlbCIgOiAic2xpbSIKICAgICAgfQogICAgfQogIH0KfQ\\u003d\\u003d\\\"}]}}}", "1");
        }

        private void OutJson_Cyclic(bool IsRun)
        {
            if (!IsRun)
                return;

            TOutJsonMain("cyclic:apple_diamond", "124701");
            TOutJsonMain("cyclic:apple_emerald", "17850");
            TOutJsonMain("cyclic:gem_obsidian", "1557");
            TOutJsonMain("cyclic:gem_amber", "1477");
            TOutJsonMain("cyclic:experience_food", "45");
            TOutJsonMain("cyclic:peat_baked", "3");
            TOutJsonMain("cyclic:peat_fuel_enriched", "3");
            TOutJsonMain("cyclic:fireball_dark", "2586");
            TOutJsonMain("cyclic:xpjuice_bucket", "832");
            TOutJsonMain("cyclic:slime_bucket", "832");
            TOutJsonMain("cyclic:biomass_bucket", "832");
            TOutJsonMain("cyclic:honey_bucket", "832");
            TOutJsonMain("cyclic:magma_bucket", "832");
            TOutJsonMain("cyclic:wax_bucket", "832");
        }

        private void OutJson_FarmersDelight(bool IsRun)
        {
            if (!IsRun)
                return;

            TOutJsonMain("farmersdelight:tree_bark", "8");
            TOutJsonMain("farmersdelight:sandy_shrub", "1");
            TOutJsonMain("farmersdelight:rich_soil", "261");
            TOutJsonMain("farmersdelight:red_mushroom_colony", "160");
            TOutJsonMain("farmersdelight:brown_mushroom_colony", "160");
            TOutJsonMain("farmersdelight:cabbage", "64");
            TOutJsonMain("farmersdelight:tomato", "64");
            TOutJsonMain("farmersdelight:onion", "64");
            TOutJsonMain("farmersdelight:rotten_tomato", "64");
            TOutJsonMain("farmersdelight:rice_panicle", "64");
            TOutJsonMain("farmersdelight:wild_beetroots", "16");
            TOutJsonMain("farmersdelight:wild_rice", "64");
            TOutJsonMain("farmersdelight:wild_potatoes", "64");
            TOutJsonMain("farmersdelight:wild_cabbages", "64");
            TOutJsonMain("farmersdelight:wild_onions", "96");
            TOutJsonMain("farmersdelight:wild_tomatoes", "64");
            TOutJsonMain("farmersdelight:wild_carrots", "64");
            TOutJsonMain("farmersdelight:cabbage_seeds", "16");
            TOutJsonMain("farmersdelight:tomato_seeds", "16");
            TOutJsonMain("farmersdelight:straw", "2");
            TOutJsonMain("farmersdelight:hot_cocoa", "149");
            TOutJsonMain("farmersdelight:apple_cider", "272");
            TOutJsonMain("farmersdelight:tomato_sauce", "134");
            TOutJsonMain("farmersdelight:raw_pasta", "24");
            TOutJsonMain("farmersdelight:pumpkin_slice", "36");
            TOutJsonMain("farmersdelight:cabbage_leaf", "32");
            TOutJsonMain("farmersdelight:minced_beef", "32");
            TOutJsonMain("farmersdelight:chicken_cuts", "32");
            TOutJsonMain("farmersdelight:bacon", "32");
            TOutJsonMain("farmersdelight:cod_slice", "32");
            TOutJsonMain("farmersdelight:salmon_slice", "32");
            TOutJsonMain("farmersdelight:mutton_chops", "32");
            TOutJsonMain("farmersdelight:ham", "64");
            TOutJsonMain("farmersdelight:cake_slice", "25");
            TOutJsonMain("farmersdelight:apple_pie_slice", "141");
            TOutJsonMain("farmersdelight:sweet_berry_cheesecake_slice", "45");
            TOutJsonMain("farmersdelight:chocolate_pie_slice", "78");
            TOutJsonMain("farmersdelight:glow_berry_custard", "69");
            TOutJsonMain("farmersdelight:dumplings", "71");
            TOutJsonMain("farmersdelight:cabbage_rolls", "64");
            TOutJsonMain("farmersdelight:cooked_rice", "70");
            TOutJsonMain("farmersdelight:kelp_roll_slice", "65");
            TOutJsonMain("farmersdelight:bone_broth", "154");
            TOutJsonMain("farmersdelight:beef_stew", "166");
            TOutJsonMain("farmersdelight:chicken_soup", "198");
            TOutJsonMain("farmersdelight:vegetable_soup", "230");
            TOutJsonMain("farmersdelight:fried_rice", "230");
            TOutJsonMain("farmersdelight:fish_stew", "268");
            TOutJsonMain("farmersdelight:pumpkin_soup", "111");
            TOutJsonMain("farmersdelight:baked_cod_stew", "198");
            TOutJsonMain("farmersdelight:noodle_soup", "95");
            TOutJsonMain("farmersdelight:pasta_with_meatballs", "196");
            TOutJsonMain("farmersdelight:pasta_with_mutton_chop", "196");
            TOutJsonMain("farmersdelight:mushroom_rice", "198");
            TOutJsonMain("farmersdelight:vegetable_noodles", "222");
            TOutJsonMain("farmersdelight:ratatouille", "262");
            TOutJsonMain("farmersdelight:squid_ink_pasta", "174");
            TOutJsonMain("farmersdelight:stuffed_pumpkin_block", "448");
            TOutJsonMain("farmersdelight:shepherds_pie", "91");
            TOutJsonMain("farmersdelight:honey_glazed_ham", "77");
            TOutJsonMain("farmersdelight:roast_chicken", "112");
            TOutJsonMain("farmersdelight:stuffed_pumpkin", "112");
            TOutJsonMain("farmersdelight:dog_food", "172");
        }

        private void OutJson_Create(bool IsRun)
        {
            if (!IsRun)
                return;

            OutJsonMain("create:copper_sheet", "128");
            OutJsonMain("create:brass_sheet", "320");
            OutJsonMain("create:iron_sheet", "256");
            OutJsonMain("create:golden_sheet", "2048");
            OutJsonMain("create:brass_ingot", "320");
            OutJsonMain("create:zinc_ingot", "512");
            OutJsonMain("create:raw_zinc", "512");
            OutJsonMain("create:polished_rose_quartz", "805");
            OutJsonMain("create:crushed_raw_iron", "256");
            OutJsonMain("create:crushed_raw_gold", "2048");
            OutJsonMain("create:crushed_raw_copper", "128");
            OutJsonMain("create:crushed_raw_zinc", "512");
            OutJsonMain("create:experience_nugget", "30");
            OutJsonMain("create:bar_of_chocolate", "83");
            OutJsonMain("create:chocolate_bucket", "1100");
            OutJsonMain("create:honey_bucket", "956");
            OutJsonMain("create:sweet_roll", "76");
            OutJsonMain("create:chocolate_glazed_berries", "99");
            OutJsonMain("create:honeyed_apple", "175");
            OutJsonMain("create:builders_tea", "4");
            OutJsonMain("create:cinder_flour", "1");
            OutJsonMain("create:blaze_cake_base", "48");
            OutJsonMain("create:blaze_cake", "64");
            OutJsonMain("create:precision_mechanism", "10720");
            OutJsonMain("create:powdered_obsidian", "16");
            OutJsonMain("create:sturdy_sheet", "32");
            OutJsonMain("create:haunted_bell", "3200");
            OutJsonMain("create:andesite_casing", "120");
            OutJsonMain("create:copper_casing", "160");
            OutJsonMain("create:brass_casing", "352");
            OutJsonMain("create:railway_casing", "384");
            OutJsonMain("create:blaze_burner", "1525");
            OutJsonMain("create:track", "88");
            OutJsonMain("create:wheat_flour", "12");
            OutJsonMain("create:asurine", "32");
            OutJsonMain("create:crimsite", "32");
            OutJsonMain("create:ochrum", "32");
            OutJsonMain("create:limestone", "32");
            OutJsonMain("create:veridium", "32");
        }

        private void OutJson_Artifacts(bool isRun)
        {
            if (!isRun) return;

            var artifacts = new Dictionary<string, string>
            {
                ["artifacts:everlasting_beef"] = "209715",
                ["artifacts:umbrella"] = "5079",
                ["artifacts:plastic_drinking_hat"] = "30000",
                ["artifacts:novelty_drinking_hat"] = "40000",
                ["artifacts:night_vision_goggles"] = "40000",
                ["artifacts:superstitious_hat"] = "40000",
                ["artifacts:villager_hat"] = "40000",
                ["artifacts:cowboy_hat"] = "40000",
                ["artifacts:anglers_hat"] = "40000",
                ["artifacts:lucky_scarf"] = "40000",
                ["artifacts:snorkel"] = "30000",
                ["artifacts:cross_necklace"] = "40000",
                ["artifacts:panic_necklace"] = "40000",
                ["artifacts:shock_pendant"] = "40000",
                ["artifacts:flame_pendant"] = "40000",
                ["artifacts:thorn_pendant"] = "40000",
                ["artifacts:charm_of_sinking"] = "50000",
                ["artifacts:cloud_in_a_bottle"] = "240000",
                ["artifacts:scarf_of_invisibility"] = "40000",
                ["artifacts:obsidian_skull"] = "80000",
                ["artifacts:crystal_heart"] = "140000",
                ["artifacts:antidote_vessel"] = "70000",
                ["artifacts:universal_attractor"] = "30000",
                ["artifacts:helium_flamingo"] = "80000",
                ["artifacts:chorus_totem"] = "527592",
                ["artifacts:digging_claws"] = "20000",
                ["artifacts:feral_claws"] = "70000",
                ["artifacts:power_glove"] = "240000",
                ["artifacts:fire_gauntlet"] = "30000",
                ["artifacts:pocket_piston"] = "30000",
                ["artifacts:vampiric_glove"] = "70000",
                ["artifacts:golden_hook"] = "40000",
                ["artifacts:onion_ring"] = "30000",
                ["artifacts:aqua_dashers"] = "60000",
                ["artifacts:pickaxe_heater"] = "140000",
                ["artifacts:kitty_slippers"] = "100000",
                ["artifacts:bunny_hoppers"] = "400000",
                ["artifacts:rooted_boots"] = "12000",
                ["artifacts:flippers"] = "30000",
                ["artifacts:snowshoes"] = "60000",
                ["artifacts:running_shoes"] = "70000",
                ["artifacts:whoopee_cushion"] = "2333",
                ["artifacts:steadfast_spikes"] = "170000"
            };

            foreach (var artifact in artifacts)
            {
                TOutJsonMain(artifact.Key, artifact.Value);
            }
        }

        private void OutJson_LuckyBlock(bool IsRun)
        {
            if (!IsRun)
                return;

            switch (OutType)
            {
                case 0:
                    OutJsonMain("lucky:lucky_block{Luck:80,display:{Name:\\u0027{\\\"translate\\\":\\\"block.lucky.lucky_block.veryLucky\\\"}\\u0027}}", "221015");
                    OutJsonMain("lucky:lucky_block{Luck:-80,display:{Name:\\u0027{\\\"translate\\\":\\\"block.lucky.lucky_block.unlucky\\\"}\\u0027}}", "1371");
                    OutJsonMain("minecraft:diamond_shovel{Damage:0,Enchantments:[{id:\\\"minecraft:efficiency\\\",lvl:4s},{id:\\\"minecraft:silk_touch\\\",lvl:1s},{id:\\\"minecraft:fortune\\\",lvl:3s}],display:{Name:\\u0027{\\\"text\\\": \\\"Hero\\u0027s Shovel\\\",\\\"color\\\": \\\"blue\\\",\\\"bold\\\": true}\\u0027}}", "16410");
                    OutJsonMain("minecraft:diamond_pickaxe{Damage:0,Enchantments:[{id:\\\"minecraft:silk_touch\\\",lvl:1s},{id:\\\"minecraft:fortune\\\",lvl:1s}],display:{Name:\\u0027{\\\"text\\\": \\\"Hero\\u0027s Pickaxe\\\",\\\"color\\\": \\\"blue\\\",\\\"bold\\\": true}\\u0027}}", "49198");
                    OutJsonMain("minecraft:diamond_axe{Damage:0,Enchantments:[{id:\\\"minecraft:bane_of_arthropods\\\",lvl:2s},{id:\\\"minecraft:knockback\\\",lvl:1s},{id:\\\"minecraft:efficiency\\\",lvl:3s},{id:\\\"minecraft:silk_touch\\\",lvl:1s},{id:\\\"minecraft:fortune\\\",lvl:3s}],display:{Name:\\u0027{\\\"text\\\": \\\"Hero\\u0027s Axe\\\",\\\"color\\\": \\\"blue\\\",\\\"bold\\\": true}\\u0027}}", "49198");
                    OutJsonMain("minecraft:diamond_hoe{Damage:0,Enchantments:[{id:\\\"minecraft:silk_touch\\\",lvl:1s},{id:\\\"minecraft:fortune\\\",lvl:2s}],display:{Name:\\u0027{\\\"text\\\": \\\"Hero\\u0027s Hoe\\\",\\\"color\\\": \\\"blue\\\",\\\"bold\\\": true}\\u0027}}", "32804");
                    OutJsonMain("minecraft:bow{Damage:0,Enchantments:[{id:\\\"minecraft:unbreaking\\\",lvl:2s},{id:\\\"minecraft:power\\\",lvl:5s},{id:\\\"minecraft:punch\\\",lvl:2s},{id:\\\"minecraft:flame\\\",lvl:1s},{id:\\\"minecraft:infinity\\\",lvl:1s},{id:\\\"minecraft:mending\\\",lvl:1s}],display:{Name:\\u0027{\\\"text\\\": \\\"Hero\\u0027s Bow\\\",\\\"color\\\": \\\"blue\\\",\\\"bold\\\": true}\\u0027}}", "1371");
                    OutJsonMain("minecraft:crossbow{Damage:0,Enchantments:[{id:\\\"minecraft:multishot\\\",lvl:1s},{id:\\\"minecraft:quick_charge\\\",lvl:3s},{id:\\\"minecraft:piercing\\\",lvl:2s}],display:{Name:\\u0027{\\\"text\\\": \\\"Hero\\u0027s Crossbow\\\",\\\"color\\\": \\\"blue\\\",\\\"bold\\\": true}\\u0027}}", "1432");
                    break;

                case 1:
                    break;
            }
        }

        private void OutJson_Pixelmon(bool IsRun)
        {
            if (!IsRun)
                return;

            TOutJsonMain("pixelmon:red_apricorn", "64");
            TOutJsonMain("pixelmon:yellow_apricorn", "64");
            TOutJsonMain("pixelmon:blue_apricorn", "64");
            TOutJsonMain("pixelmon:green_apricorn", "64");
            TOutJsonMain("pixelmon:pink_apricorn", "64");
            TOutJsonMain("pixelmon:white_apricorn", "64");
            TOutJsonMain("pixelmon:black_apricorn", "64");

            TOutJsonMain("pixelmon:xs_exp_candy", "4096");
            TOutJsonMain("pixelmon:s_exp_candy", "8192");
            TOutJsonMain("pixelmon:m_exp_candy", "16384");
            TOutJsonMain("pixelmon:l_exp_candy", "32768");
            TOutJsonMain("pixelmon:xl_exp_candy", "65536");

            TOutJsonMain("pixelmon:raw_bauxite", "128");
            TOutJsonMain("pixelmon:raw_silicon", "1024");
            TOutJsonMain("pixelmon:raw_silver", "2048");
            TOutJsonMain("pixelmon:raw_platinum", "4096");
            TOutJsonMain("pixelmon:raw_tumblestone", "4096");
            TOutJsonMain("pixelmon:raw_sky_tumblestone", "4096");
            TOutJsonMain("pixelmon:raw_black_tumblestone", "4096");

            switch (OutType)
            {
                case 0:
                    break;

                case 1:
                    NewOutJsonMain("pixelmon:poke_ball", "32768", "\"pixelmon:poke_ball\": \"cherish_ball\"");
                    break;
            }
        }

        #endregion

        private void labelOpenFile_Click(object sender, EventArgs e) => StartFileMain();

        private void StartFileMain()
        {
            try
            {
                Qi.OpenFile(FilePath);
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("文件打开失败，原因：" + ex.Message, "错误", MessageBoxButtons.CancelTryContinue) == DialogResult.TryAgain)
                    StartFileMain();
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OutType = comboBox.SelectedIndex;
        }
    }
}
