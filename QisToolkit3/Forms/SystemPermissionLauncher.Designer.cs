namespace QisToolkit3.Forms
{
    partial class SystemPermissionLauncher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemPermissionLauncher));
            button_Taskmgr = new Button();
            button_Cmd = new Button();
            button_PowerShell = new Button();
            button_Regedit = new Button();
            button_SystemInfo32 = new Button();
            button_mmc = new Button();
            button_mmc_compmgmt = new Button();
            button_mmc_services = new Button();
            button_mmc_comexp = new Button();
            button_mmc_diskmgmt = new Button();
            button_mmc_lusrmgr = new Button();
            button_mmc_devmgmt = new Button();
            button_mmc_taskschd = new Button();
            button_mmc_certlm = new Button();
            button_mmc_fsmgmt = new Button();
            button_mmc_perfmon = new Button();
            button_mmc_eventvwr = new Button();
            button_WMIC = new Button();
            button_Py = new Button();
            comboBox_RunMode = new ComboBox();
            SuspendLayout();
            // 
            // button_Taskmgr
            // 
            button_Taskmgr.Image = Properties.Resources.Taskmgr64;
            button_Taskmgr.ImageAlign = ContentAlignment.TopCenter;
            button_Taskmgr.Location = new Point(317, 4);
            button_Taskmgr.Name = "button_Taskmgr";
            button_Taskmgr.Size = new Size(100, 100);
            button_Taskmgr.TabIndex = 0;
            button_Taskmgr.Text = "任务管理器";
            button_Taskmgr.TextAlign = ContentAlignment.BottomCenter;
            button_Taskmgr.UseVisualStyleBackColor = true;
            button_Taskmgr.Click += button_Taskmgr_Click;
            // 
            // button_Cmd
            // 
            button_Cmd.Image = (Image)resources.GetObject("button_Cmd.Image");
            button_Cmd.ImageAlign = ContentAlignment.TopCenter;
            button_Cmd.Location = new Point(420, 4);
            button_Cmd.Name = "button_Cmd";
            button_Cmd.Size = new Size(100, 100);
            button_Cmd.TabIndex = 1;
            button_Cmd.Text = "命令提示符";
            button_Cmd.TextAlign = ContentAlignment.BottomCenter;
            button_Cmd.UseVisualStyleBackColor = true;
            button_Cmd.Click += button_Cmd_Click;
            // 
            // button_PowerShell
            // 
            button_PowerShell.Image = (Image)resources.GetObject("button_PowerShell.Image");
            button_PowerShell.ImageAlign = ContentAlignment.TopCenter;
            button_PowerShell.Location = new Point(420, 107);
            button_PowerShell.Name = "button_PowerShell";
            button_PowerShell.Size = new Size(100, 100);
            button_PowerShell.TabIndex = 2;
            button_PowerShell.Text = "PowerShell";
            button_PowerShell.TextAlign = ContentAlignment.BottomCenter;
            button_PowerShell.UseVisualStyleBackColor = true;
            button_PowerShell.Click += button_PowerShell_Click;
            // 
            // button_Regedit
            // 
            button_Regedit.Image = (Image)resources.GetObject("button_Regedit.Image");
            button_Regedit.ImageAlign = ContentAlignment.TopCenter;
            button_Regedit.Location = new Point(317, 107);
            button_Regedit.Name = "button_Regedit";
            button_Regedit.Size = new Size(100, 100);
            button_Regedit.TabIndex = 3;
            button_Regedit.Text = "注册表编辑";
            button_Regedit.TextAlign = ContentAlignment.BottomCenter;
            button_Regedit.UseVisualStyleBackColor = true;
            button_Regedit.Click += button_Regedit_Click;
            // 
            // button_SystemInfo32
            // 
            button_SystemInfo32.Image = (Image)resources.GetObject("button_SystemInfo32.Image");
            button_SystemInfo32.ImageAlign = ContentAlignment.TopCenter;
            button_SystemInfo32.Location = new Point(317, 210);
            button_SystemInfo32.Name = "button_SystemInfo32";
            button_SystemInfo32.Size = new Size(100, 100);
            button_SystemInfo32.TabIndex = 4;
            button_SystemInfo32.Text = "系统信息";
            button_SystemInfo32.TextAlign = ContentAlignment.BottomCenter;
            button_SystemInfo32.UseVisualStyleBackColor = true;
            button_SystemInfo32.Click += button_SystemInfo32_Click;
            // 
            // button_mmc
            // 
            button_mmc.Image = (Image)resources.GetObject("button_mmc.Image");
            button_mmc.ImageAlign = ContentAlignment.TopCenter;
            button_mmc.Location = new Point(213, 4);
            button_mmc.Name = "button_mmc";
            button_mmc.Size = new Size(100, 100);
            button_mmc.TabIndex = 5;
            button_mmc.Text = "MMC 本体";
            button_mmc.TextAlign = ContentAlignment.BottomCenter;
            button_mmc.UseVisualStyleBackColor = true;
            button_mmc.Click += button_mmc_Click;
            // 
            // button_mmc_compmgmt
            // 
            button_mmc_compmgmt.Image = (Image)resources.GetObject("button_mmc_compmgmt.Image");
            button_mmc_compmgmt.ImageAlign = ContentAlignment.TopCenter;
            button_mmc_compmgmt.Location = new Point(213, 107);
            button_mmc_compmgmt.Name = "button_mmc_compmgmt";
            button_mmc_compmgmt.Size = new Size(100, 100);
            button_mmc_compmgmt.TabIndex = 6;
            button_mmc_compmgmt.Text = "计算机管理";
            button_mmc_compmgmt.TextAlign = ContentAlignment.BottomCenter;
            button_mmc_compmgmt.UseVisualStyleBackColor = true;
            button_mmc_compmgmt.Click += button_mmc_compmgmt_Click;
            // 
            // button_mmc_services
            // 
            button_mmc_services.Image = (Image)resources.GetObject("button_mmc_services.Image");
            button_mmc_services.ImageAlign = ContentAlignment.TopCenter;
            button_mmc_services.Location = new Point(213, 210);
            button_mmc_services.Name = "button_mmc_services";
            button_mmc_services.Size = new Size(100, 100);
            button_mmc_services.TabIndex = 7;
            button_mmc_services.Text = "服务";
            button_mmc_services.TextAlign = ContentAlignment.BottomCenter;
            button_mmc_services.UseVisualStyleBackColor = true;
            button_mmc_services.Click += button_mmc_services_Click;
            // 
            // button_mmc_comexp
            // 
            button_mmc_comexp.Image = (Image)resources.GetObject("button_mmc_comexp.Image");
            button_mmc_comexp.ImageAlign = ContentAlignment.TopCenter;
            button_mmc_comexp.Location = new Point(213, 313);
            button_mmc_comexp.Name = "button_mmc_comexp";
            button_mmc_comexp.Size = new Size(100, 100);
            button_mmc_comexp.TabIndex = 8;
            button_mmc_comexp.Text = "组件服务";
            button_mmc_comexp.TextAlign = ContentAlignment.BottomCenter;
            button_mmc_comexp.UseVisualStyleBackColor = true;
            button_mmc_comexp.Click += button_mmc_comexp_Click;
            // 
            // button_mmc_diskmgmt
            // 
            button_mmc_diskmgmt.Image = (Image)resources.GetObject("button_mmc_diskmgmt.Image");
            button_mmc_diskmgmt.ImageAlign = ContentAlignment.TopCenter;
            button_mmc_diskmgmt.Location = new Point(109, 313);
            button_mmc_diskmgmt.Name = "button_mmc_diskmgmt";
            button_mmc_diskmgmt.Size = new Size(100, 100);
            button_mmc_diskmgmt.TabIndex = 12;
            button_mmc_diskmgmt.Text = "磁盘管理";
            button_mmc_diskmgmt.TextAlign = ContentAlignment.BottomCenter;
            button_mmc_diskmgmt.UseVisualStyleBackColor = true;
            button_mmc_diskmgmt.Click += button_mmc_diskmgmt_Click;
            // 
            // button_mmc_lusrmgr
            // 
            button_mmc_lusrmgr.Image = (Image)resources.GetObject("button_mmc_lusrmgr.Image");
            button_mmc_lusrmgr.ImageAlign = ContentAlignment.TopCenter;
            button_mmc_lusrmgr.Location = new Point(109, 210);
            button_mmc_lusrmgr.Name = "button_mmc_lusrmgr";
            button_mmc_lusrmgr.Size = new Size(100, 100);
            button_mmc_lusrmgr.TabIndex = 11;
            button_mmc_lusrmgr.Text = "用户和组";
            button_mmc_lusrmgr.TextAlign = ContentAlignment.BottomCenter;
            button_mmc_lusrmgr.UseVisualStyleBackColor = true;
            button_mmc_lusrmgr.Click += button_mmc_lusrmgr_Click;
            // 
            // button_mmc_devmgmt
            // 
            button_mmc_devmgmt.Image = (Image)resources.GetObject("button_mmc_devmgmt.Image");
            button_mmc_devmgmt.ImageAlign = ContentAlignment.TopCenter;
            button_mmc_devmgmt.Location = new Point(109, 107);
            button_mmc_devmgmt.Name = "button_mmc_devmgmt";
            button_mmc_devmgmt.Size = new Size(100, 100);
            button_mmc_devmgmt.TabIndex = 10;
            button_mmc_devmgmt.Text = "设备管理器";
            button_mmc_devmgmt.TextAlign = ContentAlignment.BottomCenter;
            button_mmc_devmgmt.UseVisualStyleBackColor = true;
            button_mmc_devmgmt.Click += button_mmc_devmgmt_Click;
            // 
            // button_mmc_taskschd
            // 
            button_mmc_taskschd.Image = (Image)resources.GetObject("button_mmc_taskschd.Image");
            button_mmc_taskschd.ImageAlign = ContentAlignment.TopCenter;
            button_mmc_taskschd.Location = new Point(109, 4);
            button_mmc_taskschd.Name = "button_mmc_taskschd";
            button_mmc_taskschd.Size = new Size(100, 100);
            button_mmc_taskschd.TabIndex = 9;
            button_mmc_taskschd.Text = "任务计划";
            button_mmc_taskschd.TextAlign = ContentAlignment.BottomCenter;
            button_mmc_taskschd.UseVisualStyleBackColor = true;
            button_mmc_taskschd.Click += button_mmc_taskschd_Click;
            // 
            // button_mmc_certlm
            // 
            button_mmc_certlm.Image = (Image)resources.GetObject("button_mmc_certlm.Image");
            button_mmc_certlm.ImageAlign = ContentAlignment.TopCenter;
            button_mmc_certlm.Location = new Point(5, 313);
            button_mmc_certlm.Name = "button_mmc_certlm";
            button_mmc_certlm.Size = new Size(100, 100);
            button_mmc_certlm.TabIndex = 16;
            button_mmc_certlm.Text = "证书管理器";
            button_mmc_certlm.TextAlign = ContentAlignment.BottomCenter;
            button_mmc_certlm.UseVisualStyleBackColor = true;
            button_mmc_certlm.Click += button_mmc_certlm_Click;
            // 
            // button_mmc_fsmgmt
            // 
            button_mmc_fsmgmt.Image = (Image)resources.GetObject("button_mmc_fsmgmt.Image");
            button_mmc_fsmgmt.ImageAlign = ContentAlignment.TopCenter;
            button_mmc_fsmgmt.Location = new Point(5, 210);
            button_mmc_fsmgmt.Name = "button_mmc_fsmgmt";
            button_mmc_fsmgmt.Size = new Size(100, 100);
            button_mmc_fsmgmt.TabIndex = 15;
            button_mmc_fsmgmt.Text = "共享文件夹";
            button_mmc_fsmgmt.TextAlign = ContentAlignment.BottomCenter;
            button_mmc_fsmgmt.UseVisualStyleBackColor = true;
            button_mmc_fsmgmt.Click += button_mmc_fsmgmt_Click;
            // 
            // button_mmc_perfmon
            // 
            button_mmc_perfmon.Image = (Image)resources.GetObject("button_mmc_perfmon.Image");
            button_mmc_perfmon.ImageAlign = ContentAlignment.TopCenter;
            button_mmc_perfmon.Location = new Point(5, 107);
            button_mmc_perfmon.Name = "button_mmc_perfmon";
            button_mmc_perfmon.Size = new Size(100, 100);
            button_mmc_perfmon.TabIndex = 14;
            button_mmc_perfmon.Text = "性能监视器";
            button_mmc_perfmon.TextAlign = ContentAlignment.BottomCenter;
            button_mmc_perfmon.UseVisualStyleBackColor = true;
            button_mmc_perfmon.Click += button_mmc_perfmon_Click;
            // 
            // button_mmc_eventvwr
            // 
            button_mmc_eventvwr.Image = (Image)resources.GetObject("button_mmc_eventvwr.Image");
            button_mmc_eventvwr.ImageAlign = ContentAlignment.TopCenter;
            button_mmc_eventvwr.Location = new Point(5, 4);
            button_mmc_eventvwr.Name = "button_mmc_eventvwr";
            button_mmc_eventvwr.Size = new Size(100, 100);
            button_mmc_eventvwr.TabIndex = 13;
            button_mmc_eventvwr.Text = "事件查看器";
            button_mmc_eventvwr.TextAlign = ContentAlignment.BottomCenter;
            button_mmc_eventvwr.UseVisualStyleBackColor = true;
            button_mmc_eventvwr.Click += button_mmc_eventvwr_Click;
            // 
            // button_WMIC
            // 
            button_WMIC.Image = (Image)resources.GetObject("button_WMIC.Image");
            button_WMIC.ImageAlign = ContentAlignment.TopCenter;
            button_WMIC.Location = new Point(420, 210);
            button_WMIC.Name = "button_WMIC";
            button_WMIC.Size = new Size(100, 100);
            button_WMIC.TabIndex = 17;
            button_WMIC.Text = "WMIC";
            button_WMIC.TextAlign = ContentAlignment.BottomCenter;
            button_WMIC.UseVisualStyleBackColor = true;
            button_WMIC.Click += button_WMIC_Click;
            // 
            // button_Py
            // 
            button_Py.Image = (Image)resources.GetObject("button_Py.Image");
            button_Py.ImageAlign = ContentAlignment.TopCenter;
            button_Py.Location = new Point(317, 313);
            button_Py.Name = "button_Py";
            button_Py.Size = new Size(100, 100);
            button_Py.TabIndex = 18;
            button_Py.Text = "Python";
            button_Py.TextAlign = ContentAlignment.BottomCenter;
            button_Py.UseVisualStyleBackColor = true;
            button_Py.Click += button_Py_Click;
            // 
            // comboBox_RunMode
            // 
            comboBox_RunMode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_RunMode.Font = new Font("Microsoft YaHei UI", 16F);
            comboBox_RunMode.FormattingEnabled = true;
            comboBox_RunMode.Items.AddRange(new object[] { "最高权限启动（System权限 启用全特权）", "当前权限启动（继承本程序的权限与特权）", "最低权限启动（当前用户权限启动 禁用全特权）" });
            comboBox_RunMode.Location = new Point(5, 419);
            comboBox_RunMode.Name = "comboBox_RunMode";
            comboBox_RunMode.Size = new Size(610, 43);
            comboBox_RunMode.TabIndex = 19;
            comboBox_RunMode.SelectedIndexChanged += comboBox_RunMode_SelectedIndexChanged;
            // 
            // SystemPermissionLauncher
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(627, 468);
            Controls.Add(comboBox_RunMode);
            Controls.Add(button_Py);
            Controls.Add(button_WMIC);
            Controls.Add(button_mmc_certlm);
            Controls.Add(button_mmc_fsmgmt);
            Controls.Add(button_mmc_perfmon);
            Controls.Add(button_mmc_eventvwr);
            Controls.Add(button_mmc_diskmgmt);
            Controls.Add(button_mmc_lusrmgr);
            Controls.Add(button_mmc_devmgmt);
            Controls.Add(button_mmc_taskschd);
            Controls.Add(button_mmc_comexp);
            Controls.Add(button_mmc_services);
            Controls.Add(button_mmc_compmgmt);
            Controls.Add(button_mmc);
            Controls.Add(button_SystemInfo32);
            Controls.Add(button_Regedit);
            Controls.Add(button_PowerShell);
            Controls.Add(button_Cmd);
            Controls.Add(button_Taskmgr);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SystemPermissionLauncher";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "齐的工具包3：系统权限启动器";
            TopMost = true;
            Load += SystemPermissionLauncher_Load;
            DragDrop += SystemPermissionLauncher_DragDrop;
            DragEnter += SystemPermissionLauncher_DragEnter;
            ResumeLayout(false);
        }

        #endregion

        private Button button_Taskmgr;
        private Button button_Cmd;
        private Button button_PowerShell;
        private Button button_Regedit;
        private Button button_SystemInfo32;
        private Button button_mmc;
        private Button button_mmc_compmgmt;
        private Button button_mmc_services;
        private Button button_mmc_comexp;
        private Button button_mmc_diskmgmt;
        private Button button_mmc_lusrmgr;
        private Button button_mmc_devmgmt;
        private Button button_mmc_taskschd;
        private Button button_mmc_certlm;
        private Button button_mmc_fsmgmt;
        private Button button_mmc_perfmon;
        private Button button_mmc_eventvwr;
        private Button button_WMIC;
        private Button button_Py;
        private ComboBox comboBox_RunMode;
    }
}