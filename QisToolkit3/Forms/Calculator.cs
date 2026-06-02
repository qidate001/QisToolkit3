using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QisToolkit3.Forms
{
    public partial class Calculator : Form
    {
        public Calculator()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void textBox_expression_TextChanged(object sender, EventArgs e)
        {
            // string expression = "3 + ∑(i=1,3,i*2) - 4 / 2";
            try
            {
                double result = EvaluateExpression(textBox_expression.Text);
                label.Text = $"算式 = {result}";
            }
            catch (Exception ex)
            {
                label.Text = $"算式 = ?\n\n算式报错: {ex.Message}";
            }
        }

        private void label_Click(object sender, EventArgs e)
        {

        }


        // 处理求和算式
        private static double EvaluateSummation(string summationExpr)
        {
            // 正则表达式匹配求和算式，格式为 ∑(i=a,b,f(i))
            Match match = Regex.Match(summationExpr, @"∑\(i=(\d+),(\d+),(.*)\)");
            if (match.Success)
            {
                int start = int.Parse(match.Groups[1].Value);
                int end = int.Parse(match.Groups[2].Value);
                string formula = match.Groups[3].Value;

                double sum = 0;
                for (int i = start; i <= end; i++)
                {
                    string currentExpr = formula.Replace("i", i.ToString());
                    sum += EvaluateSimpleExpression(currentExpr);
                }
                return sum;
            }
            throw new ArgumentException("无效的求和算式");
        }

        // 处理简单的加减乘除算式
        private static double EvaluateSimpleExpression(string expression)
        {
            DataTable dt = new DataTable();
            return Convert.ToDouble(dt.Compute(expression, ""));
        }

        // 主计算方法
        public static double EvaluateExpression(string expression)
        {
            // 检查是否包含求和算式
            if (expression.Contains("∑"))
            {
                // 正则表达式匹配所有求和算式
                MatchCollection matches = Regex.Matches(expression, @"∑\(i=\d+,\d+,.+?\)");
                foreach (Match match in matches)
                {
                    string summationExpr = match.Value;
                    double result = EvaluateSummation(summationExpr);
                    expression = expression.Replace(summationExpr, result.ToString());
                }
            }
            return EvaluateSimpleExpression(expression);
        }

        private void button_Add_Sigma_Click(object sender, EventArgs e)
        {
            if (textBox_expression.Text == string.Empty)
                textBox_expression.Text += "∑(i=1,10,i)";
            else
                textBox_expression.Text += "+∑(i=1,10,i)";
        }
    }


}
