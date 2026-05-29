using QisToolkit3.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QisToolkit3.Forms.StrangeFoods;

namespace QisToolkit3.Forms
{
    public partial class StrangeFoods : Form
    {
        //private bool[] DamagedDevelopItems = { false, false, true, true, true };
        private bool[] DamagedDevelopItems = { false, false, false, false, false };
        private Ingredient[] DevelopItems =
        {
            new Ingredient { Type = IngredientType.Null, Name = string.Empty },
            new Ingredient { Type = IngredientType.Null, Name = string.Empty },
            new Ingredient { Type = IngredientType.Null, Name = string.Empty },
            new Ingredient { Type = IngredientType.Null, Name = string.Empty },
            new Ingredient { Type = IngredientType.Null, Name = string.Empty }
        };

        // 配方
        private List<Recipe> recipes = new List<Recipe>
        {
            new Recipe
            {
                Name = "饭团",
                PerfectIngredients = new Dictionary<IngredientType, int>
                {
                    { IngredientType.Rice, 3 },
                    { IngredientType.Seaweed, 1 }
                },
                RequiredIngredients = new List<IngredientType> { IngredientType.Rice },
                MinIngredientCount = 2,
                BasePrice = 50,
                PerfectPrice = 200
            },
            new Recipe
            {
                Name = "鸡蛋拌饭",
                PerfectIngredients = new Dictionary<IngredientType, int>
                {
                    { IngredientType.Rice, 3 },
                    { IngredientType.Egg, 1 }
                },
                RequiredIngredients = new List<IngredientType> { IngredientType.Egg },
                MinIngredientCount = 2,
                BasePrice = 60,
                PerfectPrice = 250
            }
        };

        // 存储已研发菜品的最高美味度
        private Dictionary<string, RecipeDeliciousness> discoveredRecipes = new Dictionary<string, RecipeDeliciousness>();




        public StrangeFoods()
        {
            InitializeComponent();
            ReLoadDevelopText();

            // 初始化
            Qi.FormInitDo(this.Text);



            string ImgPath = Qi.QisToolkit3_Datas.actualDirectory + @"\Datas\StrangeFoods\";
            button_add_flour.BackgroundImage = Image.FromFile(@$"{ImgPath}\flour.png");
            button_add_rise.BackgroundImage = Image.FromFile(@$"{ImgPath}\rise.png");
        }

        private void ReLoadDevelopText()
        {
            label_MakeFoods_0.Text = DamagedDevelopItems[0] ? "已损坏" : DevelopItems[0].Name;
            label_MakeFoods_1.Text = DamagedDevelopItems[1] ? "已损坏" : DevelopItems[1].Name;
            label_MakeFoods_2.Text = DamagedDevelopItems[2] ? "已损坏" : DevelopItems[2].Name;
            label_MakeFoods_3.Text = DamagedDevelopItems[3] ? "已损坏" : DevelopItems[3].Name;
            label_MakeFoods_4.Text = DamagedDevelopItems[4] ? "已损坏" : DevelopItems[4].Name;
            button_MakeFood.Enabled = IsDevelopMMP();
        }

        private /*async*/ void MakeFoodsGame_Load(object sender, EventArgs e)
        {
            /*
            // 显示一个信息框告诉用户正在等待
            MessageBox.Show("正在等待10秒...");

            // 等待十秒
            await Task.Delay(10000); // 10000毫秒 = 10秒

            // 执行完等待后的操作
            MessageBox.Show("等待结束！");
            */
        }

        private bool IsDevelopMMP()
        {
            int count = 0;
            foreach (var item in DevelopItems)
                if (item.Type != IngredientType.Null)
                    ++count;

            return count >= 2;
        }

        private void AddDevelopItems(Ingredient item)
        {
            for (int i = 0; i < 5; ++i)
            {
                if (!DamagedDevelopItems[i] && DevelopItems[i].Type == IngredientType.Null)
                {
                    DevelopItems[i] = item;
                    ReLoadDevelopText();
                    return;
                }
            }
        }

        private void button_MakeFood_Click(object sender, EventArgs e)
        {
            // 获取当前研发栏的食材
            var ingredients = new List<Ingredient>();
            for (int i = 0; i < 5; i++)
            {
                if (!DamagedDevelopItems[i] && DevelopItems[i].Type != IngredientType.Null)
                {
                    ingredients.Add(DevelopItems[i]);
                }
            }

            // 使用 Tuple 版本计算匹配结果
            var result = CalculateRecipeMatchTuple(ingredients);

            if (result.score > 0)
            {
                Recipe recipe = GetRecipe(result.recipeName);
                int newDeliciousness = GetNewDeliciousnessPercent(result.score);

                // 获取当前美味度（如果已存在）
                int currentDeliciousness = 0;
                if (discoveredRecipes.ContainsKey(result.recipeName))
                {
                    currentDeliciousness = discoveredRecipes[result.recipeName].CurrentDeliciousness;
                }

                // 更新美味度
                int finalDeliciousness = UpdateDeliciousness(result.recipeName, newDeliciousness, currentDeliciousness);

                // 保存美味度记录
                discoveredRecipes[result.recipeName] = new RecipeDeliciousness
                {
                    RecipeName = result.recipeName,
                    CurrentDeliciousness = finalDeliciousness,
                    LastUpdated = DateTime.Now
                };

                // 计算价格
                int price = CalculatePrice(recipe, result.score, finalDeliciousness);

                // 显示结果
                MessageBox.Show($"研发成功！制作出了{result.quality}{result.recipeName}\n" +
                               $"美味度：{finalDeliciousness}%\n" +
                               $"售价：{price}G");

                // 清空研发栏
                ClearDevelopItems();
            }
            else
            {
                MessageBox.Show("研发失败，请尝试不同的食材组合");
            }
        }

        private string GetQualityLevel(double score)
        {
            if (score >= 0.9) return "完美的";
            if (score >= 0.7) return "优质的";
            if (score >= 0.5) return "普通的";
            return "劣质的";
        }

        private void ClearDevelopItems()
        {
            for (int i = 0; i < 5; i++)
            {
                DevelopItems[i] = new Ingredient { Type = IngredientType.Null, Name = string.Empty };
            }
            ReLoadDevelopText();
        }

        private (string recipeName, double score, string quality) CalculateRecipeMatchTuple(List<Ingredient> ingredients)
        {
            if (ingredients.Count < 2) return ("食材不足", 0, "");

            string bestRecipe = "研发失败";
            double bestScore = 0;
            string bestQuality = "";

            foreach (var recipe in recipes)
            {
                if (!CheckRequiredIngredients(ingredients, recipe))
                    continue;

                double score = CalculateMatchScore(ingredients, recipe);

                if (score > bestScore)
                {
                    bestRecipe = recipe.Name;
                    bestScore = score;
                    bestQuality = GetQualityLevel(score);
                }
            }

            return bestScore >= 0.3 ? (bestRecipe, bestScore, bestQuality) : ("研发失败", 0, "");
        }

        private bool CheckRequiredIngredients(List<Ingredient> ingredients, Recipe recipe)
        {
            foreach (var required in recipe.RequiredIngredients)
            {
                if (!ingredients.Any(i => i.Type == required))
                    return false;
            }
            return true;
        }

        private double CalculateMatchScore(List<Ingredient> ingredients, Recipe recipe)
        {
            // 统计食材数量
            var ingredientCounts = ingredients
                .GroupBy(i => i.Type)
                .ToDictionary(g => g.Key, g => g.Count());

            double totalScore = 0;
            int totalWeight = 0;

            // 计算每种食材的匹配度
            foreach (var perfectIngredient in recipe.PerfectIngredients)
            {
                var ingredientType = perfectIngredient.Key;
                var perfectCount = perfectIngredient.Value;

                ingredientCounts.TryGetValue(ingredientType, out int actualCount);

                // 单个食材匹配度 (0-1)
                double ingredientScore = CalculateIngredientScore(actualCount, perfectCount);

                totalScore += ingredientScore * perfectCount; // 加权
                totalWeight += perfectCount;
            }

            // 计算额外食材惩罚
            double extraPenalty = CalculateExtraPenalty(ingredientCounts, recipe);

            // 最终得分
            double finalScore = (totalScore / totalWeight) * (1 - extraPenalty);

            return Math.Max(0, Math.Min(1, finalScore));
        }

        private double CalculateIngredientScore(int actualCount, int perfectCount)
        {
            if (actualCount == perfectCount) return 1.0;
            if (actualCount == 0) return 0;

            // 数量接近度评分
            double ratio = (double)actualCount / perfectCount;

            if (ratio > 1) // 超出完美数量
                return 1.0 / ratio; // 递减奖励
            else
                return ratio; // 线性奖励
        }

        private double CalculateExtraPenalty(Dictionary<IngredientType, int> ingredientCounts, Recipe recipe)
        {
            int extraCount = 0;
            int totalCount = 0;

            foreach (var ingredient in ingredientCounts)
            {
                totalCount += ingredient.Value;
                recipe.PerfectIngredients.TryGetValue(ingredient.Key, out int perfectCount);

                if (ingredient.Value > perfectCount)
                    extraCount += ingredient.Value - perfectCount;
            }

            // 额外食材惩罚系数
            return Math.Min(0.5, extraCount * 0.1);
        }

        private int CalculatePrice(Recipe recipe, double matchScore, int deliciousnessPercent)
        {
            // 实际价格 = 完美配方价格 × 研制分数 + 美味度 × 最低价格
            double perfectPricePart = recipe.PerfectPrice * matchScore;
            double deliciousnessPart = recipe.BasePrice * (deliciousnessPercent / 100.0);

            return (int)Math.Round(perfectPricePart + deliciousnessPart);
        }

        private int UpdateDeliciousness(string recipeName, int newDeliciousnessPercent, int currentDeliciousnessPercent)
        {
            // 如果是第一次研发这个菜品
            if (currentDeliciousnessPercent == 0)
            {
                return newDeliciousnessPercent;
            }

            // 新配方美味度大于当前配方美味度时，直接覆写
            if (newDeliciousnessPercent > currentDeliciousnessPercent)
            {
                return Math.Min(newDeliciousnessPercent, GetRecipe(recipeName).MaxDeliciousness);
            }

            // 新配方美味度小于当前配方美味度时，计算调整值
            int adjustment = CalculateDeliciousnessAdjustment(newDeliciousnessPercent);
            int newValue = currentDeliciousnessPercent + adjustment;

            return Math.Min(newValue, GetRecipe(recipeName).MaxDeliciousness);
        }

        private int CalculateDeliciousnessAdjustment(int deliciousnessPercent)
        {
            if (deliciousnessPercent == 100) return 12;
            if (deliciousnessPercent == 200) return 24;
            if (deliciousnessPercent == 300) return 48;

            if (deliciousnessPercent > 100)
            {
                int lastTwoDigits = deliciousnessPercent % 100;
                if (lastTwoDigits > 30)
                {
                    return lastTwoDigits / 2;
                }
                return lastTwoDigits;
            }
            else
            {
                if (deliciousnessPercent > 30)
                {
                    return deliciousnessPercent / 2;
                }
                return deliciousnessPercent;
            }
        }


        private Recipe GetRecipe(string name)
        {
            return recipes.FirstOrDefault(r => r.Name == name);
        }

        private int GetNewDeliciousnessPercent(double matchScore)
        {
            // 根据匹配分数计算新美味度（0-300%）
            return (int)Math.Round(matchScore * 300);
        }


        #region

        #endregion



        private void button_flour_Click(object sender, EventArgs e)
        {
            AddDevelopItems(new Ingredient { Type = IngredientType.Flour, Name = "面粉" });
        }

        private void button_add_rise_Click(object sender, EventArgs e)
        {
            AddDevelopItems(new Ingredient { Type = IngredientType.Rice, Name = "米饭" });
        }

        private void button_add_egg_Click(object sender, EventArgs e)
        {
            AddDevelopItems(new Ingredient { Type = IngredientType.Egg, Name = "鸡蛋" });
        }
    }


    // 食材类型
    public enum IngredientType
    {
        Null, // 空
        Rice, // 米饭
        Flour, // 面粉
        Egg, // 鸡蛋
        Seaweed // 海带
    }


    // 单种食材类
    [Serializable]
    public class Ingredient
    {
        public IngredientType Type;
        public string Name;
    }


    // 食材堆叠类（玩家库存中的一组食材）
    [Serializable]
    public class IngredientStack
    {
        public Ingredient Data;
        public int Count;
    }


    public class Recipe
    {
        public string Name { get; set; }
        public Dictionary<IngredientType, int> PerfectIngredients { get; set; }
        public List<IngredientType> RequiredIngredients { get; set; }
        public int MinIngredientCount { get; set; }
        public double PerfectMatchThreshold { get; set; } = .7;

        // 新增属性
        public int BasePrice { get; set; } // 最低价格
        public int PerfectPrice { get; set; } // 完美配方价格
        public int MaxDeliciousness { get; set; } = 400; // 美味度上限（百分比）
    }

    // 菜品美味度记录类
    public class RecipeDeliciousness
    {
        public string RecipeName { get; set; }
        public int CurrentDeliciousness { get; set; } // 当前美味度（百分比，如100表示100%）
        public DateTime LastUpdated { get; set; }
    }
}


