namespace QisToolkit3.Forms
{
    partial class UnicodeTool
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
            tabControl = new TabControl();
            tabPage_Main = new TabPage();
            richTextBoxClipboardUnicode = new RichTextBox();
            buttonGetUnicodeData = new Button();
            buttonGetClipboardData = new Button();
            labelClipboardUnicode = new Label();
            tabPage2 = new TabPage();
            buttonDC4 = new Button();
            buttonDC3 = new Button();
            buttonDC2 = new Button();
            buttonDC1 = new Button();
            buttonDLE = new Button();
            buttonSI = new Button();
            buttonSO = new Button();
            buttonCR = new Button();
            buttonFF = new Button();
            buttonVT = new Button();
            buttonLF = new Button();
            buttonHT = new Button();
            buttonBS = new Button();
            buttonBEL = new Button();
            buttonACK = new Button();
            buttonENQ = new Button();
            buttonEOT = new Button();
            buttonETX = new Button();
            buttonSTX = new Button();
            buttonSOH = new Button();
            buttonNUL = new Button();
            tabControl.SuspendLayout();
            tabPage_Main.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabPage_Main);
            tabControl.Controls.Add(tabPage2);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Margin = new Padding(4);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(954, 796);
            tabControl.TabIndex = 0;
            // 
            // tabPage_Main
            // 
            tabPage_Main.Controls.Add(richTextBoxClipboardUnicode);
            tabPage_Main.Controls.Add(buttonGetUnicodeData);
            tabPage_Main.Controls.Add(buttonGetClipboardData);
            tabPage_Main.Controls.Add(labelClipboardUnicode);
            tabPage_Main.Location = new Point(4, 36);
            tabPage_Main.Margin = new Padding(4);
            tabPage_Main.Name = "tabPage_Main";
            tabPage_Main.Padding = new Padding(4);
            tabPage_Main.Size = new Size(946, 756);
            tabPage_Main.TabIndex = 0;
            tabPage_Main.Text = "控制器";
            tabPage_Main.UseVisualStyleBackColor = true;
            // 
            // richTextBoxClipboardUnicode
            // 
            richTextBoxClipboardUnicode.Font = new Font("Microsoft YaHei UI", 40F, FontStyle.Regular, GraphicsUnit.Point, 134);
            richTextBoxClipboardUnicode.Location = new Point(328, 133);
            richTextBoxClipboardUnicode.Name = "richTextBoxClipboardUnicode";
            richTextBoxClipboardUnicode.Size = new Size(610, 120);
            richTextBoxClipboardUnicode.TabIndex = 3;
            richTextBoxClipboardUnicode.Text = "U+0000";
            richTextBoxClipboardUnicode.TextChanged += richTextBoxClipboardUnicode_TextChanged;
            // 
            // buttonGetUnicodeData
            // 
            buttonGetUnicodeData.Font = new Font("Microsoft YaHei UI", 16F);
            buttonGetUnicodeData.Location = new Point(8, 133);
            buttonGetUnicodeData.Name = "buttonGetUnicodeData";
            buttonGetUnicodeData.Size = new Size(314, 120);
            buttonGetUnicodeData.TabIndex = 2;
            buttonGetUnicodeData.Text = "获取 Unicode 文本\r\n至剪切板";
            buttonGetUnicodeData.UseVisualStyleBackColor = true;
            buttonGetUnicodeData.Click += buttonGetUnicodeData_Click;
            // 
            // buttonGetClipboardData
            // 
            buttonGetClipboardData.Font = new Font("Microsoft YaHei UI", 16F);
            buttonGetClipboardData.Location = new Point(8, 7);
            buttonGetClipboardData.Name = "buttonGetClipboardData";
            buttonGetClipboardData.Size = new Size(314, 120);
            buttonGetClipboardData.TabIndex = 1;
            buttonGetClipboardData.Text = "获取剪切板文本 Unicode";
            buttonGetClipboardData.UseVisualStyleBackColor = true;
            buttonGetClipboardData.Click += buttonGetClipboardData_Click;
            // 
            // labelClipboardUnicode
            // 
            labelClipboardUnicode.BackColor = Color.Gainsboro;
            labelClipboardUnicode.Font = new Font("Microsoft YaHei UI", 16F);
            labelClipboardUnicode.Location = new Point(328, 7);
            labelClipboardUnicode.Name = "labelClipboardUnicode";
            labelClipboardUnicode.Size = new Size(610, 120);
            labelClipboardUnicode.TabIndex = 0;
            labelClipboardUnicode.Text = "无内容";
            labelClipboardUnicode.Click += labelClipboardUnicode_Click;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(buttonDC4);
            tabPage2.Controls.Add(buttonDC3);
            tabPage2.Controls.Add(buttonDC2);
            tabPage2.Controls.Add(buttonDC1);
            tabPage2.Controls.Add(buttonDLE);
            tabPage2.Controls.Add(buttonSI);
            tabPage2.Controls.Add(buttonSO);
            tabPage2.Controls.Add(buttonCR);
            tabPage2.Controls.Add(buttonFF);
            tabPage2.Controls.Add(buttonVT);
            tabPage2.Controls.Add(buttonLF);
            tabPage2.Controls.Add(buttonHT);
            tabPage2.Controls.Add(buttonBS);
            tabPage2.Controls.Add(buttonBEL);
            tabPage2.Controls.Add(buttonACK);
            tabPage2.Controls.Add(buttonENQ);
            tabPage2.Controls.Add(buttonEOT);
            tabPage2.Controls.Add(buttonETX);
            tabPage2.Controls.Add(buttonSTX);
            tabPage2.Controls.Add(buttonSOH);
            tabPage2.Controls.Add(buttonNUL);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Margin = new Padding(4);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(4);
            tabPage2.Size = new Size(946, 763);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonDC4
            // 
            buttonDC4.Font = new Font("Microsoft YaHei UI", 24F);
            buttonDC4.Location = new Point(812, 235);
            buttonDC4.Name = "buttonDC4";
            buttonDC4.Size = new Size(128, 108);
            buttonDC4.TabIndex = 20;
            buttonDC4.Text = "DC4";
            buttonDC4.UseVisualStyleBackColor = true;
            // 
            // buttonDC3
            // 
            buttonDC3.Font = new Font("Microsoft YaHei UI", 24F);
            buttonDC3.Location = new Point(678, 235);
            buttonDC3.Name = "buttonDC3";
            buttonDC3.Size = new Size(128, 108);
            buttonDC3.TabIndex = 19;
            buttonDC3.Text = "DC3";
            buttonDC3.UseVisualStyleBackColor = true;
            // 
            // buttonDC2
            // 
            buttonDC2.Font = new Font("Microsoft YaHei UI", 24F);
            buttonDC2.Location = new Point(544, 235);
            buttonDC2.Name = "buttonDC2";
            buttonDC2.Size = new Size(128, 108);
            buttonDC2.TabIndex = 18;
            buttonDC2.Text = "DC2";
            buttonDC2.UseVisualStyleBackColor = true;
            // 
            // buttonDC1
            // 
            buttonDC1.Font = new Font("Microsoft YaHei UI", 24F);
            buttonDC1.Location = new Point(410, 235);
            buttonDC1.Name = "buttonDC1";
            buttonDC1.Size = new Size(128, 108);
            buttonDC1.TabIndex = 17;
            buttonDC1.Text = "DC1";
            buttonDC1.UseVisualStyleBackColor = true;
            // 
            // buttonDLE
            // 
            buttonDLE.Font = new Font("Microsoft YaHei UI", 24F);
            buttonDLE.Location = new Point(276, 235);
            buttonDLE.Name = "buttonDLE";
            buttonDLE.Size = new Size(128, 108);
            buttonDLE.TabIndex = 16;
            buttonDLE.Text = "DLE";
            buttonDLE.UseVisualStyleBackColor = true;
            // 
            // buttonSI
            // 
            buttonSI.Font = new Font("Microsoft YaHei UI", 24F);
            buttonSI.Location = new Point(142, 235);
            buttonSI.Name = "buttonSI";
            buttonSI.Size = new Size(128, 108);
            buttonSI.TabIndex = 15;
            buttonSI.Text = "SI";
            buttonSI.UseVisualStyleBackColor = true;
            // 
            // buttonSO
            // 
            buttonSO.Font = new Font("Microsoft YaHei UI", 24F);
            buttonSO.Location = new Point(8, 235);
            buttonSO.Name = "buttonSO";
            buttonSO.Size = new Size(128, 108);
            buttonSO.TabIndex = 14;
            buttonSO.Text = "SO";
            buttonSO.UseVisualStyleBackColor = true;
            // 
            // buttonCR
            // 
            buttonCR.Font = new Font("Microsoft YaHei UI", 24F);
            buttonCR.Location = new Point(812, 121);
            buttonCR.Name = "buttonCR";
            buttonCR.Size = new Size(128, 108);
            buttonCR.TabIndex = 13;
            buttonCR.Text = "CR";
            buttonCR.UseVisualStyleBackColor = true;
            // 
            // buttonFF
            // 
            buttonFF.Font = new Font("Microsoft YaHei UI", 24F);
            buttonFF.Location = new Point(678, 121);
            buttonFF.Name = "buttonFF";
            buttonFF.Size = new Size(128, 108);
            buttonFF.TabIndex = 12;
            buttonFF.Text = "FF";
            buttonFF.UseVisualStyleBackColor = true;
            // 
            // buttonVT
            // 
            buttonVT.Font = new Font("Microsoft YaHei UI", 24F);
            buttonVT.Location = new Point(544, 121);
            buttonVT.Name = "buttonVT";
            buttonVT.Size = new Size(128, 108);
            buttonVT.TabIndex = 11;
            buttonVT.Text = "VT";
            buttonVT.UseVisualStyleBackColor = true;
            // 
            // buttonLF
            // 
            buttonLF.Font = new Font("Microsoft YaHei UI", 24F);
            buttonLF.Location = new Point(410, 121);
            buttonLF.Name = "buttonLF";
            buttonLF.Size = new Size(128, 108);
            buttonLF.TabIndex = 10;
            buttonLF.Text = "LF";
            buttonLF.UseVisualStyleBackColor = true;
            // 
            // buttonHT
            // 
            buttonHT.Font = new Font("Microsoft YaHei UI", 24F);
            buttonHT.Location = new Point(276, 121);
            buttonHT.Name = "buttonHT";
            buttonHT.Size = new Size(128, 108);
            buttonHT.TabIndex = 9;
            buttonHT.Text = "HT";
            buttonHT.UseVisualStyleBackColor = true;
            // 
            // buttonBS
            // 
            buttonBS.Font = new Font("Microsoft YaHei UI", 24F);
            buttonBS.Location = new Point(142, 121);
            buttonBS.Name = "buttonBS";
            buttonBS.Size = new Size(128, 108);
            buttonBS.TabIndex = 8;
            buttonBS.Text = "BS";
            buttonBS.UseVisualStyleBackColor = true;
            // 
            // buttonBEL
            // 
            buttonBEL.Font = new Font("Microsoft YaHei UI", 24F);
            buttonBEL.Location = new Point(8, 121);
            buttonBEL.Name = "buttonBEL";
            buttonBEL.Size = new Size(128, 108);
            buttonBEL.TabIndex = 7;
            buttonBEL.Text = "BEL";
            buttonBEL.UseVisualStyleBackColor = true;
            // 
            // buttonACK
            // 
            buttonACK.Font = new Font("Microsoft YaHei UI", 24F);
            buttonACK.Location = new Point(812, 7);
            buttonACK.Name = "buttonACK";
            buttonACK.Size = new Size(128, 108);
            buttonACK.TabIndex = 6;
            buttonACK.Text = "ACK";
            buttonACK.UseVisualStyleBackColor = true;
            // 
            // buttonENQ
            // 
            buttonENQ.Font = new Font("Microsoft YaHei UI", 24F);
            buttonENQ.Location = new Point(678, 7);
            buttonENQ.Name = "buttonENQ";
            buttonENQ.Size = new Size(128, 108);
            buttonENQ.TabIndex = 5;
            buttonENQ.Text = "ENQ";
            buttonENQ.UseVisualStyleBackColor = true;
            // 
            // buttonEOT
            // 
            buttonEOT.Font = new Font("Microsoft YaHei UI", 24F);
            buttonEOT.Location = new Point(544, 7);
            buttonEOT.Name = "buttonEOT";
            buttonEOT.Size = new Size(128, 108);
            buttonEOT.TabIndex = 4;
            buttonEOT.Text = "EOT";
            buttonEOT.UseVisualStyleBackColor = true;
            // 
            // buttonETX
            // 
            buttonETX.Font = new Font("Microsoft YaHei UI", 24F);
            buttonETX.Location = new Point(410, 7);
            buttonETX.Name = "buttonETX";
            buttonETX.Size = new Size(128, 108);
            buttonETX.TabIndex = 3;
            buttonETX.Text = "ETX";
            buttonETX.UseVisualStyleBackColor = true;
            // 
            // buttonSTX
            // 
            buttonSTX.Font = new Font("Microsoft YaHei UI", 24F);
            buttonSTX.Location = new Point(276, 7);
            buttonSTX.Name = "buttonSTX";
            buttonSTX.Size = new Size(128, 108);
            buttonSTX.TabIndex = 2;
            buttonSTX.Text = "STX";
            buttonSTX.UseVisualStyleBackColor = true;
            // 
            // buttonSOH
            // 
            buttonSOH.Font = new Font("Microsoft YaHei UI", 24F);
            buttonSOH.Location = new Point(142, 7);
            buttonSOH.Name = "buttonSOH";
            buttonSOH.Size = new Size(128, 108);
            buttonSOH.TabIndex = 1;
            buttonSOH.Text = "SOH";
            buttonSOH.UseVisualStyleBackColor = true;
            // 
            // buttonNUL
            // 
            buttonNUL.Font = new Font("Microsoft YaHei UI", 24F);
            buttonNUL.Location = new Point(8, 7);
            buttonNUL.Name = "buttonNUL";
            buttonNUL.Size = new Size(128, 108);
            buttonNUL.TabIndex = 0;
            buttonNUL.Text = "NUL";
            buttonNUL.UseVisualStyleBackColor = true;
            // 
            // UnicodeTool
            // 
            AutoScaleDimensions = new SizeF(12F, 27F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(954, 796);
            Controls.Add(tabControl);
            Font = new Font("Microsoft YaHei UI", 12F);
            Margin = new Padding(4);
            Name = "UnicodeTool";
            Text = "UnicodeTool";
            Load += UnicodeTool_Load;
            tabControl.ResumeLayout(false);
            tabPage_Main.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage_Main;
        private TabPage tabPage2;
        private TabControl tabControl;
        private Button buttonGetClipboardData;
        private Label labelClipboardUnicode;
        private Button buttonNUL;
        private Button buttonSOH;
        private Button buttonSTX;
        private Button buttonEOT;
        private Button buttonETX;
        private Button buttonCR;
        private Button buttonFF;
        private Button buttonVT;
        private Button buttonLF;
        private Button buttonHT;
        private Button buttonBS;
        private Button buttonBEL;
        private Button buttonACK;
        private Button buttonENQ;
        private Button buttonDC4;
        private Button buttonDC3;
        private Button buttonDC2;
        private Button buttonDC1;
        private Button buttonDLE;
        private Button buttonSI;
        private Button buttonSO;
        private RichTextBox richTextBoxClipboardUnicode;
        private Button buttonGetUnicodeData;
    }
}