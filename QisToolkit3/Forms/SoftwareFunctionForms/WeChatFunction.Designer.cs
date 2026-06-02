namespace QisToolkit3.Forms.SoftwareFunctionForms
{
    partial class WeChatFunction
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
            components = new System.ComponentModel.Container();
            checkBox_NoWeChatAppEx = new CheckBox();
            toolTip = new ToolTip(components);
            checkBox_WeixinUpdate = new CheckBox();
            checkBox_NoWetypeInstaller = new CheckBox();
            checkBox_NoWeChat = new CheckBox();
            SuspendLayout();
            // 
            // checkBox_NoWeChatAppEx
            // 
            checkBox_NoWeChatAppEx.AutoSize = true;
            checkBox_NoWeChatAppEx.Location = new Point(15, 83);
            checkBox_NoWeChatAppEx.Margin = new Padding(6);
            checkBox_NoWeChatAppEx.Name = "checkBox_NoWeChatAppEx";
            checkBox_NoWeChatAppEx.Size = new Size(364, 56);
            checkBox_NoWeChatAppEx.TabIndex = 0;
            checkBox_NoWeChatAppEx.Text = "禁用微信附加模块";
            toolTip.SetToolTip(checkBox_NoWeChatAppEx, "强行禁止 WeChatAppEx.exe 的运行\r\n这会让微信部分功能如视频号、浏览器等功能不可用\r\n提供给那些不想要这些恼人的不必要的功能的用户\r\n此功能开启后可以降低25%左右微信占用的资源\r\n此功能开启后会在一定程度上影响启动速度\r\n启动速度±10%左右，这个因设备不同而异\r\n但此功能可能会导致微信不稳定");
            checkBox_NoWeChatAppEx.UseVisualStyleBackColor = true;
            checkBox_NoWeChatAppEx.CheckedChanged += checkBox_NoWeChatAppEx_CheckedChanged;
            // 
            // toolTip
            // 
            toolTip.AutoPopDelay = 50000000;
            toolTip.InitialDelay = 50;
            toolTip.IsBalloon = true;
            toolTip.ReshowDelay = 100;
            // 
            // checkBox_WeixinUpdate
            // 
            checkBox_WeixinUpdate.AutoSize = true;
            checkBox_WeixinUpdate.Location = new Point(15, 151);
            checkBox_WeixinUpdate.Margin = new Padding(6);
            checkBox_WeixinUpdate.Name = "checkBox_WeixinUpdate";
            checkBox_WeixinUpdate.Size = new Size(284, 56);
            checkBox_WeixinUpdate.TabIndex = 1;
            checkBox_WeixinUpdate.Text = "禁用微信更新";
            toolTip.SetToolTip(checkBox_WeixinUpdate, "暴力禁止微信更新\r\n将会阻止 WeixinUpdate.exe 的正常运行\r\n提供给那些绝对不想要让微信更新的用户");
            checkBox_WeixinUpdate.UseVisualStyleBackColor = true;
            checkBox_WeixinUpdate.CheckedChanged += checkBox_WeixinUpdate_CheckedChanged;
            // 
            // checkBox_NoWetypeInstaller
            // 
            checkBox_NoWetypeInstaller.AutoSize = true;
            checkBox_NoWetypeInstaller.Location = new Point(15, 219);
            checkBox_NoWetypeInstaller.Margin = new Padding(6);
            checkBox_NoWetypeInstaller.Name = "checkBox_NoWetypeInstaller";
            checkBox_NoWetypeInstaller.Size = new Size(404, 56);
            checkBox_NoWetypeInstaller.TabIndex = 2;
            checkBox_NoWetypeInstaller.Text = "禁用微信输入法安装";
            toolTip.SetToolTip(checkBox_NoWetypeInstaller, "暴力禁止微信输入法安装程序\r\n其实微信即时没用点击下载输入法按钮\r\n但其实大多数微信都自带了微信输入法安装程序\r\n位于 .\\Tencent\\Weixin\\*.*.*.*\\WetypeInstaller.exe\r\n将会阻止 WetypeInstaller.exe 的正常运行\r\n提供给那些绝对不想要微信输入法的用户");
            checkBox_NoWetypeInstaller.UseVisualStyleBackColor = true;
            checkBox_NoWetypeInstaller.CheckedChanged += checkBox_NoWetypeInstaller_CheckedChanged;
            // 
            // checkBox_NoWeChat
            // 
            checkBox_NoWeChat.AutoSize = true;
            checkBox_NoWeChat.ForeColor = Color.Red;
            checkBox_NoWeChat.Location = new Point(15, 15);
            checkBox_NoWeChat.Margin = new Padding(6);
            checkBox_NoWeChat.Name = "checkBox_NoWeChat";
            checkBox_NoWeChat.Size = new Size(204, 56);
            checkBox_NoWeChat.TabIndex = 3;
            checkBox_NoWeChat.Text = "禁用微信";
            toolTip.SetToolTip(checkBox_NoWeChat, "强行禁止 Weixin.exe 的运行\r\n这会让微信完全无法运行\r\n尤其是当您的微信有些重要内容，\r\n但无法保证使用此电脑的用户是否可信时");
            checkBox_NoWeChat.UseVisualStyleBackColor = true;
            checkBox_NoWeChat.CheckedChanged += checkBox_NoWeChat_CheckedChanged;
            // 
            // WeChatFunction
            // 
            AutoScaleDimensions = new SizeF(24F, 52F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(429, 294);
            Controls.Add(checkBox_NoWeChat);
            Controls.Add(checkBox_NoWetypeInstaller);
            Controls.Add(checkBox_WeixinUpdate);
            Controls.Add(checkBox_NoWeChatAppEx);
            Font = new Font("Microsoft YaHei UI", 24F);
            Margin = new Padding(8);
            Name = "WeChatFunction";
            Text = "微信功能";
            Load += WeChatFunction_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox checkBox_NoWeChatAppEx;
        private ToolTip toolTip;
        private CheckBox checkBox_WeixinUpdate;
        private CheckBox checkBox_NoWetypeInstaller;
        private CheckBox checkBox_NoWeChat;
    }
}