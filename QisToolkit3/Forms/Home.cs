using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace QisToolkit3.Forms
{
    public partial class Home : Form
    {
        private bool hotKeysRegistered = false;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const uint MOD_ALT = 0x0001;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint MOD_WIN = 0x0008;
        private const int HOTKEY_ID_0 = 100;  // Ctrl+1
        private const int HOTKEY_ID_1 = 101;  // Ctrl+2
        private const int HOTKEY_ID_2 = 102;  // Ctrl+3
        private const int HOTKEY_ID_3 = 103;  // Ctrl+4
        private const int HOTKEY_ID_10 = 200;  // Alt+1
        private const int HOTKEY_ID_11 = 201;  // Alt+2
        private const int HOTKEY_ID_12 = 202;  // Alt+3
        private const int HOTKEY_ID_13 = 203;  // Alt+4
        private const int HOTKEY_ID_21 = 110;  // Ctrl+S
        private const int HOTKEY_ID_ShowLab = 1000;  // Ctrl+空格

        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            RegisterAllHotKeys();
        }

        private void Run(int RunMode = 0, string name = "cmd", string arguments = "")
        {
            if (RunMode == 0)
                Qi.RunNSudo($"{name} {arguments}");
            else if (RunMode == 1)
            {
                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = name,
                        Arguments = arguments,
                    };

                    process.Start();
                }
            }
            else
            {
                Qi.UnRunNSudo($"{name} {arguments}");
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            if (m.Msg == WM_HOTKEY)
            {
                switch (m.WParam.ToInt32())
                {
                    case HOTKEY_ID_0:
                        Run(1, "cmd.exe");
                        break;

                    case HOTKEY_ID_1:
                        Run(1, "powershell.exe");
                        break;

                    case HOTKEY_ID_2:
                        Run(1, "Taskmgr.exe");
                        break;

                    case HOTKEY_ID_3:
                        Run(1, "powershell.exe");
                        break;

                    case HOTKEY_ID_10:
                        Run(0, "cmd.exe");
                        break;

                    case HOTKEY_ID_11:
                        Run(0, "powershell.exe");
                        break;

                    case HOTKEY_ID_12:
                        Run(0, "Taskmgr.exe");
                        break;

                    case HOTKEY_ID_13:
                        Run(0, "powershell");
                        break;

                    case HOTKEY_ID_21:
                        new SystemPermissionLauncher().Show();
                        break;

                    case HOTKEY_ID_ShowLab:
                        labelTitle.Visible = !labelTitle.Visible;
                        labelIntroduce1.Visible = !labelIntroduce1.Visible;
                        labelIntroduce2.Visible = !labelIntroduce2.Visible;
                        break;

                    default:
                        break;
                }
            }
            base.WndProc(ref m);
        }

        private void RegisterAllHotKeys()
        {
            if (!hotKeysRegistered && IsHandleCreated)
            {
                bool result = true;
                result = RegisterHotKey(Handle, HOTKEY_ID_0, MOD_CONTROL, (uint)Keys.D1) && result;
                result = RegisterHotKey(Handle, HOTKEY_ID_1, MOD_CONTROL, (uint)Keys.D2) && result;
                result = RegisterHotKey(Handle, HOTKEY_ID_2, MOD_CONTROL, (uint)Keys.D3) && result;
                result = RegisterHotKey(Handle, HOTKEY_ID_3, MOD_CONTROL, (uint)Keys.D4) && result;

                result = RegisterHotKey(Handle, HOTKEY_ID_10, MOD_ALT, (uint)Keys.D1) && result;
                result = RegisterHotKey(Handle, HOTKEY_ID_11, MOD_ALT, (uint)Keys.D2) && result;
                result = RegisterHotKey(Handle, HOTKEY_ID_10, MOD_ALT, (uint)Keys.D3) && result;
                result = RegisterHotKey(Handle, HOTKEY_ID_11, MOD_ALT, (uint)Keys.D4) && result;

                result = RegisterHotKey(Handle, HOTKEY_ID_21, MOD_CONTROL, (uint)Keys.S) && result;
                result = RegisterHotKey(Handle, HOTKEY_ID_ShowLab, MOD_CONTROL, (uint)Keys.Space) && result;
                if (result)
                {
                    hotKeysRegistered = true;
                    Log.Info("[Home] 全局热键注册成功");
                }
                else
                    Log.Err("[Home] 全局热键注册失败");
            }
        }

        private void UnregisterAllHotKeys()
        {
            if (hotKeysRegistered)
            {
                UnregisterHotKey(Handle, HOTKEY_ID_0);
                UnregisterHotKey(Handle, HOTKEY_ID_1);
                UnregisterHotKey(Handle, HOTKEY_ID_2);
                UnregisterHotKey(Handle, HOTKEY_ID_3);
                UnregisterHotKey(Handle, HOTKEY_ID_10);
                UnregisterHotKey(Handle, HOTKEY_ID_11);
                UnregisterHotKey(Handle, HOTKEY_ID_12);
                UnregisterHotKey(Handle, HOTKEY_ID_13);
                UnregisterHotKey(Handle, HOTKEY_ID_21);
                UnregisterHotKey(Handle, HOTKEY_ID_ShowLab);
                hotKeysRegistered = false;
                Log.Info("[Home] 全局热键已注销");
            }
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterAllHotKeys();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            UnregisterAllHotKeys();
            base.OnHandleDestroyed(e);
        }
    }
}