namespace QisToolkit3.Forms
{
    partial class YtDlpToolMatchFilters
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            listBox = new ListBox();
            label2 = new Label();
            label1 = new Label();
            button_CommandAdd = new Button();
            comboBox_CommandText = new ComboBox();
            comboBoxValue = new ComboBox();
            checkBoxSafeOperator = new CheckBox();
            comboBoxOperator = new ComboBox();
            comboBoxMetadataFields = new ComboBox();
            button_Add = new Button();
            buttonDeleteItem = new Button();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(listBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(label3);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Panel2.Controls.Add(button_CommandAdd);
            splitContainer1.Panel2.Controls.Add(comboBox_CommandText);
            splitContainer1.Panel2.Controls.Add(comboBoxValue);
            splitContainer1.Panel2.Controls.Add(checkBoxSafeOperator);
            splitContainer1.Panel2.Controls.Add(comboBoxOperator);
            splitContainer1.Panel2.Controls.Add(comboBoxMetadataFields);
            splitContainer1.Panel2.Controls.Add(button_Add);
            splitContainer1.Panel2.Controls.Add(buttonDeleteItem);
            splitContainer1.Panel2.Paint += splitContainer1_Panel2_Paint;
            splitContainer1.Size = new Size(1242, 710);
            splitContainer1.SplitterDistance = 364;
            splitContainer1.TabIndex = 1;
            splitContainer1.SplitterMoved += splitContainer1_SplitterMoved;
            // 
            // listBox
            // 
            listBox.Dock = DockStyle.Fill;
            listBox.Font = new Font("微软雅黑", 12F);
            listBox.FormattingEnabled = true;
            listBox.ItemHeight = 27;
            listBox.Location = new Point(0, 0);
            listBox.Name = "listBox";
            listBox.SelectionMode = SelectionMode.MultiExtended;
            listBox.Size = new Size(364, 710);
            listBox.TabIndex = 0;
            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.BackColor = Color.Gainsboro;
            label2.Font = new Font("Microsoft YaHei UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 134);
            label2.Location = new Point(9, 215);
            label2.Name = "label2";
            label2.Size = new Size(853, 43);
            label2.TabIndex = 9;
            label2.Text = "手动添加";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.BackColor = Color.Gainsboro;
            label1.Font = new Font("Microsoft YaHei UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 134);
            label1.Location = new Point(9, 86);
            label1.Name = "label1";
            label1.Size = new Size(853, 43);
            label1.TabIndex = 8;
            label1.Text = "预设添加";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button_CommandAdd
            // 
            button_CommandAdd.Font = new Font("Microsoft YaHei UI", 16F);
            button_CommandAdd.Location = new Point(768, 261);
            button_CommandAdd.Name = "button_CommandAdd";
            button_CommandAdd.Size = new Size(94, 46);
            button_CommandAdd.TabIndex = 7;
            button_CommandAdd.Text = "添加";
            button_CommandAdd.UseVisualStyleBackColor = true;
            button_CommandAdd.Click += button_CommandAdd_Click;
            // 
            // comboBox_CommandText
            // 
            comboBox_CommandText.Font = new Font("Microsoft YaHei UI", 16F);
            comboBox_CommandText.FormattingEnabled = true;
            comboBox_CommandText.Items.AddRange(new object[] { "Tile*=第一集", "Tile*=?第一集", "Tile*=第1集", "Tile*=?第1集" });
            comboBox_CommandText.Location = new Point(9, 263);
            comboBox_CommandText.Name = "comboBox_CommandText";
            comboBox_CommandText.Size = new Size(753, 43);
            comboBox_CommandText.TabIndex = 6;
            comboBox_CommandText.Text = "Tile*=第一集";
            // 
            // comboBoxValue
            // 
            comboBoxValue.Font = new Font("Microsoft YaHei UI", 16F);
            comboBoxValue.FormattingEnabled = true;
            comboBoxValue.Items.AddRange(new object[] { "教程", "'教程(?!.*广告)'" });
            comboBoxValue.Location = new Point(377, 133);
            comboBoxValue.Name = "comboBoxValue";
            comboBoxValue.Size = new Size(385, 43);
            comboBoxValue.TabIndex = 5;
            comboBoxValue.Text = "'教程(?!.*广告)'";
            // 
            // checkBoxSafeOperator
            // 
            checkBoxSafeOperator.AutoSize = true;
            checkBoxSafeOperator.Font = new Font("Microsoft YaHei UI", 12F);
            checkBoxSafeOperator.Location = new Point(202, 182);
            checkBoxSafeOperator.Name = "checkBoxSafeOperator";
            checkBoxSafeOperator.Size = new Size(134, 31);
            checkBoxSafeOperator.TabIndex = 4;
            checkBoxSafeOperator.Text = "安全比较符";
            checkBoxSafeOperator.UseVisualStyleBackColor = true;
            // 
            // comboBoxOperator
            // 
            comboBoxOperator.Font = new Font("Microsoft YaHei UI", 16F);
            comboBoxOperator.FormattingEnabled = true;
            comboBoxOperator.Items.AddRange(new object[] { "等于", "不等于", "大于", "小于", "大于等于", "小于等于", "正则匹配", "包含子串", "以...开头", "以...结尾" });
            comboBoxOperator.Location = new Point(197, 133);
            comboBoxOperator.Name = "comboBoxOperator";
            comboBoxOperator.Size = new Size(174, 43);
            comboBoxOperator.TabIndex = 3;
            comboBoxOperator.Text = "正则匹配";
            // 
            // comboBoxMetadataFields
            // 
            comboBoxMetadataFields.Font = new Font("Microsoft YaHei UI", 16F);
            comboBoxMetadataFields.FormattingEnabled = true;
            comboBoxMetadataFields.Items.AddRange(new object[] { "视频标题", "视频描述", "上传者名称", "上传者ID", "频道名称", "频道ID", "视频时长（秒）", "播放次数", "点赞数", "评论数", "年龄限制", "上传日期", "Unix时间戳", "是否直播", "直播状态", "可用性", "文件扩展名", "格式名称", "格式ID", "视频宽度", "视频高度", "帧率", "视频编码", "音频编码", "文件大小（字节）", "估计文件大小", "下载协议", "语言", "系列名称", "季数", "集数", "音轨名称", "艺术家", "专辑名称" });
            comboBoxMetadataFields.Location = new Point(9, 133);
            comboBoxMetadataFields.Name = "comboBoxMetadataFields";
            comboBoxMetadataFields.Size = new Size(182, 43);
            comboBoxMetadataFields.TabIndex = 2;
            comboBoxMetadataFields.Text = "视频标题";
            // 
            // button_Add
            // 
            button_Add.Font = new Font("Microsoft YaHei UI", 16F);
            button_Add.Location = new Point(768, 131);
            button_Add.Name = "button_Add";
            button_Add.Size = new Size(94, 46);
            button_Add.TabIndex = 1;
            button_Add.Text = "添加";
            button_Add.UseVisualStyleBackColor = true;
            button_Add.Click += button_Add_Click;
            // 
            // buttonDeleteItem
            // 
            buttonDeleteItem.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            buttonDeleteItem.Enabled = false;
            buttonDeleteItem.Font = new Font("微软雅黑", 24F);
            buttonDeleteItem.Location = new Point(2, 3);
            buttonDeleteItem.Name = "buttonDeleteItem";
            buttonDeleteItem.Size = new Size(869, 80);
            buttonDeleteItem.TabIndex = 0;
            buttonDeleteItem.Text = "删除选择项";
            buttonDeleteItem.UseVisualStyleBackColor = true;
            buttonDeleteItem.Click += buttonDeleteItem_Click;
            // 
            // label3
            // 
            label3.Font = new Font("Microsoft YaHei UI", 16F);
            label3.Location = new Point(9, 310);
            label3.Name = "label3";
            label3.Size = new Size(853, 57);
            label3.TabIndex = 46;
            label3.Text = "提示：关闭此窗口自动保存";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // YtDlpToolMatchFilters
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1242, 710);
            Controls.Add(splitContainer1);
            Name = "YtDlpToolMatchFilters";
            Text = "Yt-Dlp 工具：高级下载过滤编辑器";
            FormClosed += YtDlpToolMatchFilters_FormClosed;
            Load += YtDlpToolMatchFilters_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private ListBox listBox;
        private Button buttonDeleteItem;
        private Button button_Add;
        private ComboBox comboBoxOperator;
        private ComboBox comboBoxMetadataFields;
        private ComboBox comboBoxValue;
        private CheckBox checkBoxSafeOperator;
        private Label label2;
        private Label label1;
        private Button button_CommandAdd;
        private ComboBox comboBox_CommandText;
        private Label label3;
    }
}