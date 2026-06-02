namespace QisToolkit3.Forms.SoftwareFunctionForms
{
    partial class PCLFunction
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
            labelIdentify = new Label();
            buttonRead = new Button();
            buttonSave = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            comboBoxCacheMsV2Migrated = new ComboBox();
            textBoxIdentify = new TextBox();
            textBoxSystemLastVersionReg = new TextBox();
            textBoxSystemHighestBetaVersionReg = new TextBox();
            textBoxUiLauncherThemeHide2 = new TextBox();
            textBoxCacheMsV2OAuthRefresh = new TextBox();
            textBoxCacheMsV2Access = new TextBox();
            label6 = new Label();
            textBoxLaunchFolders = new TextBox();
            label7 = new Label();
            textBoxCacheMsV2Name = new TextBox();
            labelCacheMsV2Name = new Label();
            textBoxCacheMsV2Uuid = new TextBox();
            label9 = new Label();
            textBoxCacheMsV2ProfileJson = new TextBox();
            label10 = new Label();
            comboBoxLoginType = new ComboBox();
            label8 = new Label();
            comboBoxSystemEula = new ComboBox();
            label11 = new Label();
            label12 = new Label();
            textBoxCacheJavaListVersion = new TextBox();
            textBoxSystemCount = new TextBox();
            label13 = new Label();
            comboBoxHintDownload = new ComboBox();
            label14 = new Label();
            label15 = new Label();
            comboBoxHintNotice = new ComboBox();
            textBoxLaunchArgumentJavaAll = new TextBox();
            label16 = new Label();
            textBoxToolUpdateSnapshotLast = new TextBox();
            label17 = new Label();
            textBoxToolUpdateReleaseLast = new TextBox();
            label18 = new Label();
            textBoxLoginMsJson = new TextBox();
            label19 = new Label();
            comboBoxHintBuy = new ComboBox();
            label20 = new Label();
            textBoxSystemLaunchCount = new TextBox();
            label21 = new Label();
            textBoxSystemHighestSavedBetaVersionReg = new TextBox();
            label22 = new Label();
            comboBoxHintUpdateMod = new ComboBox();
            label23 = new Label();
            textBoxSystemHelpVersion = new TextBox();
            label24 = new Label();
            comboBoxHintInstallBack = new ComboBox();
            label25 = new Label();
            textBoxLoginLegacyName = new TextBox();
            label26 = new Label();
            textBoxCacheDownloadFolder = new TextBox();
            label27 = new Label();
            buttonDelete = new Button();
            SuspendLayout();
            // 
            // labelIdentify
            // 
            labelIdentify.BackColor = Color.Gainsboro;
            labelIdentify.Location = new Point(7, 9);
            labelIdentify.Margin = new Padding(4, 0, 4, 0);
            labelIdentify.Name = "labelIdentify";
            labelIdentify.Size = new Size(368, 27);
            labelIdentify.TabIndex = 0;
            labelIdentify.Text = "用户或设备唯一标识符";
            // 
            // buttonRead
            // 
            buttonRead.Location = new Point(7, 567);
            buttonRead.Name = "buttonRead";
            buttonRead.Size = new Size(582, 48);
            buttonRead.TabIndex = 1;
            buttonRead.Text = "读取";
            buttonRead.UseVisualStyleBackColor = true;
            buttonRead.Click += buttonLoad_Click;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(595, 567);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(566, 48);
            buttonSave.TabIndex = 2;
            buttonSave.Text = "保存";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // label1
            // 
            label1.BackColor = Color.Gainsboro;
            label1.Location = new Point(7, 46);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(368, 27);
            label1.TabIndex = 4;
            label1.Text = "系统上次使用的版本";
            // 
            // label2
            // 
            label2.BackColor = Color.Gainsboro;
            label2.Location = new Point(7, 83);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(368, 27);
            label2.TabIndex = 6;
            label2.Text = "系统最高测试版版本号";
            // 
            // label3
            // 
            label3.BackColor = Color.Gainsboro;
            label3.Location = new Point(7, 193);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(368, 27);
            label3.TabIndex = 12;
            label3.Text = "Microsoft OAuth 刷新令牌";
            // 
            // label4
            // 
            label4.BackColor = Color.Gainsboro;
            label4.Location = new Point(7, 156);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(368, 27);
            label4.TabIndex = 10;
            label4.Text = "是否已完成 V2 缓存迁移";
            // 
            // label5
            // 
            label5.BackColor = Color.Gainsboro;
            label5.Location = new Point(7, 119);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(368, 27);
            label5.TabIndex = 8;
            label5.Text = "启动器主题隐藏设置";
            // 
            // comboBoxCacheMsV2Migrated
            // 
            comboBoxCacheMsV2Migrated.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCacheMsV2Migrated.FormattingEnabled = true;
            comboBoxCacheMsV2Migrated.Items.AddRange(new object[] { "True", "False" });
            comboBoxCacheMsV2Migrated.Location = new Point(382, 152);
            comboBoxCacheMsV2Migrated.Name = "comboBoxCacheMsV2Migrated";
            comboBoxCacheMsV2Migrated.Size = new Size(277, 35);
            comboBoxCacheMsV2Migrated.TabIndex = 14;
            // 
            // textBoxIdentify
            // 
            textBoxIdentify.Location = new Point(382, 6);
            textBoxIdentify.Name = "textBoxIdentify";
            textBoxIdentify.Size = new Size(277, 33);
            textBoxIdentify.TabIndex = 3;
            // 
            // textBoxSystemLastVersionReg
            // 
            textBoxSystemLastVersionReg.Location = new Point(382, 43);
            textBoxSystemLastVersionReg.Name = "textBoxSystemLastVersionReg";
            textBoxSystemLastVersionReg.Size = new Size(277, 33);
            textBoxSystemLastVersionReg.TabIndex = 5;
            // 
            // textBoxSystemHighestBetaVersionReg
            // 
            textBoxSystemHighestBetaVersionReg.Location = new Point(382, 80);
            textBoxSystemHighestBetaVersionReg.Name = "textBoxSystemHighestBetaVersionReg";
            textBoxSystemHighestBetaVersionReg.Size = new Size(277, 33);
            textBoxSystemHighestBetaVersionReg.TabIndex = 7;
            // 
            // textBoxUiLauncherThemeHide2
            // 
            textBoxUiLauncherThemeHide2.Location = new Point(382, 116);
            textBoxUiLauncherThemeHide2.Name = "textBoxUiLauncherThemeHide2";
            textBoxUiLauncherThemeHide2.Size = new Size(277, 33);
            textBoxUiLauncherThemeHide2.TabIndex = 9;
            // 
            // textBoxCacheMsV2OAuthRefresh
            // 
            textBoxCacheMsV2OAuthRefresh.Location = new Point(382, 190);
            textBoxCacheMsV2OAuthRefresh.Name = "textBoxCacheMsV2OAuthRefresh";
            textBoxCacheMsV2OAuthRefresh.Size = new Size(277, 33);
            textBoxCacheMsV2OAuthRefresh.TabIndex = 13;
            // 
            // textBoxCacheMsV2Access
            // 
            textBoxCacheMsV2Access.Location = new Point(382, 227);
            textBoxCacheMsV2Access.Name = "textBoxCacheMsV2Access";
            textBoxCacheMsV2Access.Size = new Size(277, 33);
            textBoxCacheMsV2Access.TabIndex = 16;
            // 
            // label6
            // 
            label6.BackColor = Color.Gainsboro;
            label6.Location = new Point(7, 230);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(368, 27);
            label6.TabIndex = 15;
            label6.Text = "Microsoft 访问令牌";
            // 
            // textBoxLaunchFolders
            // 
            textBoxLaunchFolders.Location = new Point(382, 373);
            textBoxLaunchFolders.Name = "textBoxLaunchFolders";
            textBoxLaunchFolders.Size = new Size(277, 33);
            textBoxLaunchFolders.TabIndex = 24;
            // 
            // label7
            // 
            label7.BackColor = Color.Gainsboro;
            label7.Location = new Point(7, 376);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(368, 27);
            label7.TabIndex = 23;
            label7.Text = "启动文件夹路径";
            // 
            // textBoxCacheMsV2Name
            // 
            textBoxCacheMsV2Name.Location = new Point(382, 337);
            textBoxCacheMsV2Name.Name = "textBoxCacheMsV2Name";
            textBoxCacheMsV2Name.Size = new Size(277, 33);
            textBoxCacheMsV2Name.TabIndex = 22;
            // 
            // labelCacheMsV2Name
            // 
            labelCacheMsV2Name.BackColor = Color.Gainsboro;
            labelCacheMsV2Name.Location = new Point(7, 340);
            labelCacheMsV2Name.Margin = new Padding(4, 0, 4, 0);
            labelCacheMsV2Name.Name = "labelCacheMsV2Name";
            labelCacheMsV2Name.Size = new Size(368, 27);
            labelCacheMsV2Name.TabIndex = 21;
            labelCacheMsV2Name.Text = "缓存的用户名称";
            // 
            // textBoxCacheMsV2Uuid
            // 
            textBoxCacheMsV2Uuid.Location = new Point(382, 300);
            textBoxCacheMsV2Uuid.Name = "textBoxCacheMsV2Uuid";
            textBoxCacheMsV2Uuid.Size = new Size(277, 33);
            textBoxCacheMsV2Uuid.TabIndex = 20;
            // 
            // label9
            // 
            label9.BackColor = Color.Gainsboro;
            label9.Location = new Point(7, 303);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(368, 27);
            label9.TabIndex = 19;
            label9.Text = "缓存的用户 UUID";
            // 
            // textBoxCacheMsV2ProfileJson
            // 
            textBoxCacheMsV2ProfileJson.Location = new Point(382, 263);
            textBoxCacheMsV2ProfileJson.Name = "textBoxCacheMsV2ProfileJson";
            textBoxCacheMsV2ProfileJson.Size = new Size(277, 33);
            textBoxCacheMsV2ProfileJson.TabIndex = 18;
            // 
            // label10
            // 
            label10.BackColor = Color.Gainsboro;
            label10.Location = new Point(7, 266);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(368, 27);
            label10.TabIndex = 17;
            label10.Text = "Microsoft 账户配置文件信息";
            // 
            // comboBoxLoginType
            // 
            comboBoxLoginType.FormattingEnabled = true;
            comboBoxLoginType.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5" });
            comboBoxLoginType.Location = new Point(382, 409);
            comboBoxLoginType.Name = "comboBoxLoginType";
            comboBoxLoginType.Size = new Size(277, 35);
            comboBoxLoginType.TabIndex = 26;
            // 
            // label8
            // 
            label8.BackColor = Color.Gainsboro;
            label8.Location = new Point(7, 413);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(368, 27);
            label8.TabIndex = 25;
            label8.Text = "登录类型";
            // 
            // comboBoxSystemEula
            // 
            comboBoxSystemEula.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSystemEula.FormattingEnabled = true;
            comboBoxSystemEula.Items.AddRange(new object[] { "True", "False" });
            comboBoxSystemEula.Location = new Point(382, 447);
            comboBoxSystemEula.Name = "comboBoxSystemEula";
            comboBoxSystemEula.Size = new Size(277, 35);
            comboBoxSystemEula.TabIndex = 28;
            // 
            // label11
            // 
            label11.BackColor = Color.Gainsboro;
            label11.Location = new Point(7, 451);
            label11.Margin = new Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new Size(368, 27);
            label11.TabIndex = 27;
            label11.Text = "是否同意 Minecraft EULA 协议";
            // 
            // label12
            // 
            label12.BackColor = Color.Gainsboro;
            label12.Location = new Point(7, 490);
            label12.Margin = new Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new Size(368, 27);
            label12.TabIndex = 29;
            label12.Text = "缓存的 Java 版本列表版本号";
            // 
            // textBoxCacheJavaListVersion
            // 
            textBoxCacheJavaListVersion.Location = new Point(382, 487);
            textBoxCacheJavaListVersion.Name = "textBoxCacheJavaListVersion";
            textBoxCacheJavaListVersion.Size = new Size(277, 33);
            textBoxCacheJavaListVersion.TabIndex = 30;
            // 
            // textBoxSystemCount
            // 
            textBoxSystemCount.Location = new Point(382, 525);
            textBoxSystemCount.Name = "textBoxSystemCount";
            textBoxSystemCount.Size = new Size(277, 33);
            textBoxSystemCount.TabIndex = 32;
            // 
            // label13
            // 
            label13.BackColor = Color.Gainsboro;
            label13.Location = new Point(7, 528);
            label13.Margin = new Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new Size(368, 27);
            label13.TabIndex = 31;
            label13.Text = "系统计数（启动次数）";
            // 
            // comboBoxHintDownload
            // 
            comboBoxHintDownload.FormattingEnabled = true;
            comboBoxHintDownload.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5" });
            comboBoxHintDownload.Location = new Point(1048, 6);
            comboBoxHintDownload.Name = "comboBoxHintDownload";
            comboBoxHintDownload.Size = new Size(277, 35);
            comboBoxHintDownload.TabIndex = 34;
            // 
            // label14
            // 
            label14.BackColor = Color.Gainsboro;
            label14.Location = new Point(673, 9);
            label14.Margin = new Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new Size(368, 27);
            label14.TabIndex = 33;
            label14.Text = "下载状态";
            // 
            // label15
            // 
            label15.BackColor = Color.Gainsboro;
            label15.Location = new Point(673, 46);
            label15.Margin = new Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new Size(368, 27);
            label15.TabIndex = 35;
            label15.Text = "通知提示状态";
            // 
            // comboBoxHintNotice
            // 
            comboBoxHintNotice.FormattingEnabled = true;
            comboBoxHintNotice.Items.AddRange(new object[] { "0", "180" });
            comboBoxHintNotice.Location = new Point(1048, 44);
            comboBoxHintNotice.Name = "comboBoxHintNotice";
            comboBoxHintNotice.Size = new Size(277, 35);
            comboBoxHintNotice.TabIndex = 36;
            // 
            // textBoxLaunchArgumentJavaAll
            // 
            textBoxLaunchArgumentJavaAll.Location = new Point(1048, 81);
            textBoxLaunchArgumentJavaAll.Name = "textBoxLaunchArgumentJavaAll";
            textBoxLaunchArgumentJavaAll.Size = new Size(277, 33);
            textBoxLaunchArgumentJavaAll.TabIndex = 38;
            // 
            // label16
            // 
            label16.BackColor = Color.Gainsboro;
            label16.Location = new Point(673, 83);
            label16.Margin = new Padding(4, 0, 4, 0);
            label16.Name = "label16";
            label16.Size = new Size(368, 27);
            label16.TabIndex = 37;
            label16.Text = "所有已配置的 Java 路径和版本信息";
            // 
            // textBoxToolUpdateSnapshotLast
            // 
            textBoxToolUpdateSnapshotLast.Location = new Point(1048, 153);
            textBoxToolUpdateSnapshotLast.Name = "textBoxToolUpdateSnapshotLast";
            textBoxToolUpdateSnapshotLast.Size = new Size(277, 33);
            textBoxToolUpdateSnapshotLast.TabIndex = 42;
            // 
            // label17
            // 
            label17.BackColor = Color.Gainsboro;
            label17.Location = new Point(673, 156);
            label17.Margin = new Padding(4, 0, 4, 0);
            label17.Name = "label17";
            label17.Size = new Size(368, 27);
            label17.TabIndex = 41;
            label17.Text = "上次更新的快照版版本号";
            // 
            // textBoxToolUpdateReleaseLast
            // 
            textBoxToolUpdateReleaseLast.Location = new Point(1048, 116);
            textBoxToolUpdateReleaseLast.Name = "textBoxToolUpdateReleaseLast";
            textBoxToolUpdateReleaseLast.Size = new Size(277, 33);
            textBoxToolUpdateReleaseLast.TabIndex = 40;
            // 
            // label18
            // 
            label18.BackColor = Color.Gainsboro;
            label18.Location = new Point(673, 119);
            label18.Margin = new Padding(4, 0, 4, 0);
            label18.Name = "label18";
            label18.Size = new Size(368, 27);
            label18.TabIndex = 39;
            label18.Text = "上次更新的正式版版本号";
            // 
            // textBoxLoginMsJson
            // 
            textBoxLoginMsJson.Location = new Point(1048, 190);
            textBoxLoginMsJson.Name = "textBoxLoginMsJson";
            textBoxLoginMsJson.Size = new Size(277, 33);
            textBoxLoginMsJson.TabIndex = 44;
            // 
            // label19
            // 
            label19.BackColor = Color.Gainsboro;
            label19.Location = new Point(673, 193);
            label19.Margin = new Padding(4, 0, 4, 0);
            label19.Name = "label19";
            label19.Size = new Size(368, 27);
            label19.TabIndex = 43;
            label19.Text = "Microsoft 登录信息";
            // 
            // comboBoxHintBuy
            // 
            comboBoxHintBuy.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxHintBuy.FormattingEnabled = true;
            comboBoxHintBuy.Items.AddRange(new object[] { "True", "False" });
            comboBoxHintBuy.Location = new Point(1048, 226);
            comboBoxHintBuy.Name = "comboBoxHintBuy";
            comboBoxHintBuy.Size = new Size(277, 35);
            comboBoxHintBuy.TabIndex = 46;
            // 
            // label20
            // 
            label20.BackColor = Color.Gainsboro;
            label20.Location = new Point(673, 230);
            label20.Margin = new Padding(4, 0, 4, 0);
            label20.Name = "label20";
            label20.Size = new Size(368, 27);
            label20.TabIndex = 45;
            label20.Text = "是否显示购买提示";
            // 
            // textBoxSystemLaunchCount
            // 
            textBoxSystemLaunchCount.Location = new Point(1048, 263);
            textBoxSystemLaunchCount.Name = "textBoxSystemLaunchCount";
            textBoxSystemLaunchCount.Size = new Size(277, 33);
            textBoxSystemLaunchCount.TabIndex = 48;
            // 
            // label21
            // 
            label21.BackColor = Color.Gainsboro;
            label21.Location = new Point(673, 266);
            label21.Margin = new Padding(4, 0, 4, 0);
            label21.Name = "label21";
            label21.Size = new Size(368, 27);
            label21.TabIndex = 47;
            label21.Text = "系统启动次数";
            // 
            // textBoxSystemHighestSavedBetaVersionReg
            // 
            textBoxSystemHighestSavedBetaVersionReg.Location = new Point(1048, 336);
            textBoxSystemHighestSavedBetaVersionReg.Name = "textBoxSystemHighestSavedBetaVersionReg";
            textBoxSystemHighestSavedBetaVersionReg.Size = new Size(277, 33);
            textBoxSystemHighestSavedBetaVersionReg.TabIndex = 52;
            // 
            // label22
            // 
            label22.BackColor = Color.Gainsboro;
            label22.Location = new Point(673, 339);
            label22.Margin = new Padding(4, 0, 4, 0);
            label22.Name = "label22";
            label22.Size = new Size(368, 27);
            label22.TabIndex = 51;
            label22.Text = "系统保存的最高测试版版本号";
            // 
            // comboBoxHintUpdateMod
            // 
            comboBoxHintUpdateMod.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxHintUpdateMod.FormattingEnabled = true;
            comboBoxHintUpdateMod.Items.AddRange(new object[] { "True", "False" });
            comboBoxHintUpdateMod.Location = new Point(1048, 299);
            comboBoxHintUpdateMod.Name = "comboBoxHintUpdateMod";
            comboBoxHintUpdateMod.Size = new Size(277, 35);
            comboBoxHintUpdateMod.TabIndex = 50;
            // 
            // label23
            // 
            label23.BackColor = Color.Gainsboro;
            label23.Location = new Point(673, 303);
            label23.Margin = new Padding(4, 0, 4, 0);
            label23.Name = "label23";
            label23.Size = new Size(368, 27);
            label23.TabIndex = 49;
            label23.Text = "是否提示模组更新";
            // 
            // textBoxSystemHelpVersion
            // 
            textBoxSystemHelpVersion.Location = new Point(1048, 409);
            textBoxSystemHelpVersion.Name = "textBoxSystemHelpVersion";
            textBoxSystemHelpVersion.Size = new Size(277, 33);
            textBoxSystemHelpVersion.TabIndex = 56;
            // 
            // label24
            // 
            label24.BackColor = Color.Gainsboro;
            label24.Location = new Point(673, 412);
            label24.Margin = new Padding(4, 0, 4, 0);
            label24.Name = "label24";
            label24.Size = new Size(368, 27);
            label24.TabIndex = 55;
            label24.Text = "系统帮助文档版本";
            // 
            // comboBoxHintInstallBack
            // 
            comboBoxHintInstallBack.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxHintInstallBack.FormattingEnabled = true;
            comboBoxHintInstallBack.Items.AddRange(new object[] { "True", "False" });
            comboBoxHintInstallBack.Location = new Point(1048, 372);
            comboBoxHintInstallBack.Name = "comboBoxHintInstallBack";
            comboBoxHintInstallBack.Size = new Size(277, 35);
            comboBoxHintInstallBack.TabIndex = 54;
            // 
            // label25
            // 
            label25.BackColor = Color.Gainsboro;
            label25.Location = new Point(673, 376);
            label25.Margin = new Padding(4, 0, 4, 0);
            label25.Name = "label25";
            label25.Size = new Size(368, 27);
            label25.TabIndex = 53;
            label25.Text = "是否提示安装备份";
            // 
            // textBoxLoginLegacyName
            // 
            textBoxLoginLegacyName.Location = new Point(1048, 484);
            textBoxLoginLegacyName.Name = "textBoxLoginLegacyName";
            textBoxLoginLegacyName.Size = new Size(277, 33);
            textBoxLoginLegacyName.TabIndex = 60;
            // 
            // label26
            // 
            label26.BackColor = Color.Gainsboro;
            label26.Location = new Point(673, 487);
            label26.Margin = new Padding(4, 0, 4, 0);
            label26.Name = "label26";
            label26.Size = new Size(368, 27);
            label26.TabIndex = 59;
            label26.Text = "旧版登录用户名";
            // 
            // textBoxCacheDownloadFolder
            // 
            textBoxCacheDownloadFolder.Location = new Point(1048, 447);
            textBoxCacheDownloadFolder.Name = "textBoxCacheDownloadFolder";
            textBoxCacheDownloadFolder.Size = new Size(277, 33);
            textBoxCacheDownloadFolder.TabIndex = 58;
            // 
            // label27
            // 
            label27.BackColor = Color.Gainsboro;
            label27.Location = new Point(673, 450);
            label27.Margin = new Padding(4, 0, 4, 0);
            label27.Name = "label27";
            label27.Size = new Size(368, 27);
            label27.TabIndex = 57;
            label27.Text = "下载缓存文件夹路径";
            // 
            // buttonDelete
            // 
            buttonDelete.ForeColor = Color.Red;
            buttonDelete.Location = new Point(1167, 567);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(158, 48);
            buttonDelete.TabIndex = 61;
            buttonDelete.Text = "清除";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // PCLFunction
            // 
            AutoScaleDimensions = new SizeF(12F, 27F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1331, 627);
            Controls.Add(buttonDelete);
            Controls.Add(textBoxLoginLegacyName);
            Controls.Add(label26);
            Controls.Add(textBoxCacheDownloadFolder);
            Controls.Add(label27);
            Controls.Add(textBoxSystemHelpVersion);
            Controls.Add(label24);
            Controls.Add(comboBoxHintInstallBack);
            Controls.Add(label25);
            Controls.Add(textBoxSystemHighestSavedBetaVersionReg);
            Controls.Add(label22);
            Controls.Add(comboBoxHintUpdateMod);
            Controls.Add(label23);
            Controls.Add(textBoxSystemLaunchCount);
            Controls.Add(label21);
            Controls.Add(comboBoxHintBuy);
            Controls.Add(label20);
            Controls.Add(textBoxLoginMsJson);
            Controls.Add(label19);
            Controls.Add(textBoxToolUpdateSnapshotLast);
            Controls.Add(label17);
            Controls.Add(textBoxToolUpdateReleaseLast);
            Controls.Add(label18);
            Controls.Add(textBoxLaunchArgumentJavaAll);
            Controls.Add(label16);
            Controls.Add(comboBoxHintNotice);
            Controls.Add(label15);
            Controls.Add(comboBoxHintDownload);
            Controls.Add(label14);
            Controls.Add(textBoxSystemCount);
            Controls.Add(label13);
            Controls.Add(textBoxCacheJavaListVersion);
            Controls.Add(label12);
            Controls.Add(comboBoxSystemEula);
            Controls.Add(label11);
            Controls.Add(comboBoxLoginType);
            Controls.Add(label8);
            Controls.Add(textBoxLaunchFolders);
            Controls.Add(label7);
            Controls.Add(textBoxCacheMsV2Name);
            Controls.Add(labelCacheMsV2Name);
            Controls.Add(textBoxCacheMsV2Uuid);
            Controls.Add(label9);
            Controls.Add(textBoxCacheMsV2ProfileJson);
            Controls.Add(label10);
            Controls.Add(textBoxCacheMsV2Access);
            Controls.Add(label6);
            Controls.Add(comboBoxCacheMsV2Migrated);
            Controls.Add(textBoxCacheMsV2OAuthRefresh);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(textBoxUiLauncherThemeHide2);
            Controls.Add(label5);
            Controls.Add(textBoxSystemHighestBetaVersionReg);
            Controls.Add(label2);
            Controls.Add(textBoxSystemLastVersionReg);
            Controls.Add(label1);
            Controls.Add(textBoxIdentify);
            Controls.Add(buttonSave);
            Controls.Add(buttonRead);
            Controls.Add(labelIdentify);
            Font = new Font("Microsoft YaHei UI", 12F);
            Margin = new Padding(4);
            Name = "PCLFunction";
            Text = "PCLFunction";
            Load += PCLFunction_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelIdentify;
        private Button buttonRead;
        private Button buttonSave;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private ComboBox comboBoxCacheMsV2Migrated;
        private TextBox textBoxIdentify;
        private TextBox textBoxSystemLastVersionReg;
        private TextBox textBoxSystemHighestBetaVersionReg;
        private TextBox textBoxUiLauncherThemeHide2;
        private TextBox textBoxCacheMsV2OAuthRefresh;
        private TextBox textBoxCacheMsV2Access;
        private Label label6;
        private TextBox textBoxLaunchFolders;
        private Label label7;
        private TextBox textBoxCacheMsV2Name;
        private Label labelCacheMsV2Name;
        private TextBox textBoxCacheMsV2Uuid;
        private Label label9;
        private TextBox textBoxCacheMsV2ProfileJson;
        private Label label10;
        private ComboBox comboBoxLoginType;
        private Label label8;
        private ComboBox comboBoxSystemEula;
        private Label label11;
        private Label label12;
        private TextBox textBoxCacheJavaListVersion;
        private TextBox textBoxSystemCount;
        private Label label13;
        private ComboBox comboBoxHintDownload;
        private Label label14;
        private Label label15;
        private ComboBox comboBoxHintNotice;
        private TextBox textBoxLaunchArgumentJavaAll;
        private Label label16;
        private TextBox textBoxToolUpdateSnapshotLast;
        private Label label17;
        private TextBox textBoxToolUpdateReleaseLast;
        private Label label18;
        private TextBox textBoxLoginMsJson;
        private Label label19;
        private ComboBox comboBoxHintBuy;
        private Label label20;
        private TextBox textBoxSystemLaunchCount;
        private Label label21;
        private TextBox textBoxSystemHighestSavedBetaVersionReg;
        private Label label22;
        private ComboBox comboBoxHintUpdateMod;
        private Label label23;
        private TextBox textBoxSystemHelpVersion;
        private Label label24;
        private ComboBox comboBoxHintInstallBack;
        private Label label25;
        private TextBox textBoxLoginLegacyName;
        private Label label26;
        private TextBox textBoxCacheDownloadFolder;
        private Label label27;
        private Button buttonDelete;
    }
}