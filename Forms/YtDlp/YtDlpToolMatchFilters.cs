using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QisToolkit3.Forms
{
    public partial class YtDlpToolMatchFilters : Form
    {
        public YtDlpToolMatchFilters()
        {
            InitializeComponent();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            string MatchFilters = MakeCommand(
                comboBoxMetadataFields.Text,
                comboBoxOperator.Text,
                checkBoxSafeOperator.Checked,
                comboBoxValue.Text
            );

            listBox.Items.Add(MatchFilters);
            //YtDlpToolMatchFilters_Save();
        }

        private void button_CommandAdd_Click(object sender, EventArgs e)
        {
            listBox.Items.Add(comboBox_CommandText.Text);
        }

        private string MakeCommand(string Fields, string Operator, bool SafeOperator, string Value) =>
            $"{GetField(Fields)}{GetOperator(Operator, SafeOperator)}{Value}";

        private string GetField(string FieldsText) => FieldsText switch
        {
            "视频标题" => "title",
            "视频描述" => "description",
            "上传者名称" => "uploader",
            "上传者ID" => "uploader_id",
            "频道名称" => "channel",
            "频道ID" => "channel_id",
            "视频时长（秒）" => "duration",
            "播放次数" => "view_count",
            "点赞数" => "like_count",
            "评论数" => "comment_count",
            "年龄限制" => "age_limit",
            "上传日期" => "upload_date",
            "Unix时间戳" => "timestamp",
            "是否直播" => "is_live",
            "直播状态" => "live_status",
            "可用性" => "availability",
            "文件扩展名" => "ext",
            "格式名称" => "format",
            "格式ID" => "format_id",
            "视频宽度" => "width",
            "视频高度" => "height",
            "帧率" => "fps",
            "视频编码" => "vcodec",
            "音频编码" => "acodec",
            "文件大小（字节）" => "filesize",
            "估计文件大小" => "filesize_approx",
            "下载协议" => "protocol",
            "语言" => "language",
            "系列名称" => "series",
            "季数" => "season_number",
            "集数" => "episode_number",
            "音轨名称" => "track",
            "艺术家" => "artist",
            "专辑名称" => "album",
            _ => FieldsText
        };

        private string GetOperator(string OperatorText, bool SafeOperator)
        {
            string Operatortr = OperatorText switch
            {
                "等于" => "==",
                "不等于" => "!=",
                "大于" => ">",
                "小于" => "<",
                "大于等于" => ">=",
                "小于等于" => "<=",
                "正则匹配" => "~=",
                "包含子串" => "*=",
                "以...开头" => "^=",
                "以...结尾" => "$=",
                _ => OperatorText
            };

            if (SafeOperator) Operatortr += '?';

            return Operatortr;
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedItems.Count > 0)
            {
                buttonDeleteItem.Enabled = true;
            }
        }

        private void YtDlpToolMatchFilters_Save()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(YtDlpTool.MatchFiltersPath))
                {
                    foreach (var item in listBox.Items)
                    {
                        sw.WriteLine(item.ToString());
                    }
                }
            }
            catch
            {

            }
        }

        private void YtDlpToolMatchFilters_Load(object sender, EventArgs e)
        {
            try
            {
                // 清空现有项
                listBox.Items.Clear();

                using (StreamReader sr = new StreamReader(YtDlpTool.MatchFiltersPath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        listBox.Items.Add(line);
                    }
                }
            }
            catch
            {

            }
        }

        private void buttonDeleteItem_Click(object sender, EventArgs e)
        {
            listBox.Items.Remove(listBox.SelectedItem);
            //YtDlpToolMatchFilters_Save();
        }

        private void YtDlpToolMatchFilters_FormClosed(object sender, FormClosedEventArgs e)
        {
            YtDlpToolMatchFilters_Save();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
