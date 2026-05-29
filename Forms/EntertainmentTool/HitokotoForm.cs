using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QisToolkit3.Forms
{
    public partial class HitokotoForm : Form
    {
        public HitokotoForm()
        {
            InitializeComponent();
        }

        private void HitokotoForm_Load(object sender, EventArgs e)
        {
            
        }

        private void button_GetText_Click(object sender, EventArgs e)
        {
            Log.Info($"[Hitokoto] 开始请求随机句子...");
            WebClient client = new();

            try
            {
                string json = client.DownloadString("https://v1.hitokoto.cn/?encode=json");
                HitokotoModel res = JsonConvert.DeserializeObject<HitokotoModel>(json);

                Log.Info($"[Hitokoto] 句子：{res.hitokoto}");
                Log.Info($"[Hitokoto] 来自：{res.from}");

                label.Text = $"{res.hitokoto}\n——{res.from}";

                Clipboard.SetText($"{res.hitokoto}\n——{res.from}");
            }
            catch (Exception ex)
            {
                Log.Err($"[Hitokoto] 获取失败：{ex.Message}");
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}

public class HitokotoModel
{
    public string hitokoto { get; set; }   // 句子主体
    public string from { get; set; }        // 出处
}