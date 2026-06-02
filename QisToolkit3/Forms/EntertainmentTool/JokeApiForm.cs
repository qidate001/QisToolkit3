using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static JokeAPIModel;

namespace QisToolkit3.Forms.EntertainmentTool
{
    public partial class JokeApiForm : Form
    {
        private static readonly Random Random = new Random();

        private static readonly string[] Jokes = new string[]
        {
            "问：为什么程序员总是分不清万圣节和圣诞节？\n答：因为 Oct 31 = Dec 25",
            "程序员最讨厌康熙的哪个儿子？胤禩。（胤禩 = 硬是 && 他是八阿哥（bug））",
            "女朋友给程序员打电话：下班顺路买10个包子，如果看见卖西瓜的，买1个。\n" +
            "程序员晚上回来手捧1个包子，女朋友大怒：为什么只买1个？\n" +
            "程序员答：因为看见卖西瓜的了。",
            "一个SQL语句走进酒吧，看到两张桌子，问：我能JOIN你们吗？",
            "程序员：我吃了一个bug。朋友：然后呢？程序员：感觉饱（bug）了。"
        };

        public static string GetRandomJoke()
        {
            int index = Random.Next(Jokes.Length);
            return Jokes[index];
        }

        public JokeApiForm()
        {
            InitializeComponent();
            label.Text = GetRandomJoke();
            comboBox_Type.SelectedIndex = 1;
        }

        private async void button_GetText_Click(object sender, EventArgs e)
        {
            Log.Info($"[JokeAPI] 开始请求随机玩笑...");
            WebClient client = new();

            try
            {
                string url = "https://v2.jokeapi.dev/joke/";

                // 类型
                url += comboBox_Type.SelectedIndex switch
                {
                    0 => "Any",
                    1 => "Programming",
                    2 => "Miscellaneous",
                    3 => "Dark",
                    4 => "Pun",
                    5 => "Spooky",
                    6 => "Christmas",
                    _ => "Any"
                };

                // 参数
                if (checkBox_Language.Checked || checkBox_BlackListFlags.Checked)
                {
                    url += '?';

                    if (checkBox_Language.Checked)
                        url += $"lang={comboBox_Language.Text.Trim()}";

                    if (checkBox_BlackListFlags.Checked)
                        url += $"blacklistFlags={comboBox_BlackListFlags.Text.Trim()}";
                }

                var joke = await GetJokeAsync(url);

                if (joke != null && !joke.Error)
                {

                    Log.Info($"[JokeAPI] 玩笑：{joke.DisplayJoke}");
                    //Log.Info($"[JokeAPI] 来自：{joke.}");

                    label.Text = joke.DisplayJoke;

                    Clipboard.SetText(joke.DisplayJoke);
                }
                else
                {
                    Log.Err($"[JokeAPI] ERROR");
                }
            }
            catch (Exception ex)
            {
                Log.Err($"[JokeAPI] 获取失败：{ex.Message}");
            }
            finally
            {
                client.Dispose();
            }
        }

        public async Task<JokeAPIModel> GetJokeAsync(string url)
        {
            try
            {
                using HttpClient client = new HttpClient();
                string json = await client.GetStringAsync(url);
                var joke = JsonConvert.DeserializeObject<JokeAPIModel>(json);
                
                return joke;
            }
            catch (HttpRequestException ex)
            {
                Log.Err($"[JokeAPI] 请求失败: {ex.Message}");
                return null;
            }
        }
    }
}