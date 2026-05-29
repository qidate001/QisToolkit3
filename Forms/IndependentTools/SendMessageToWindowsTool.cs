using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace QisToolkit3.Forms
{
    public partial class SendMessageToWindowsTool : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string? lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public SendMessageToWindowsTool()
        {
            InitializeComponent();

            // 初始化
            Qi.FormInitDo(this.Text);
        }

        private void SendMessageToWindowsTool_Load(object sender, EventArgs e)
        {
            // Application.AddMessageFilter(new DragDropMessageFilter(label3));
        }

        private void button_SendMessage_Click(object sender, EventArgs e)
        {
            if (comboBox_Msg.Text.Length >= 6 && comboBox_Msg.Text != string.Empty)
            {
                if (
                    uint.TryParse(Qi.NoST(Qi.NoRN(comboBox_Msg.Text[..6].Substring(2))),
                    NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture,
                    out uint number
                    ))
                {
                    string[] hWnds = Regex.Split(richTextBox_hWnd.Text, "\r\n|\n|\r");
                    for (int i = 0; i < hWnds.Length; ++i)
                    {
                        // String 转 IntPtr
                        if (IntPtr.TryParse(hWnds[i], out IntPtr _hWnd))
                        {
                            IntPtr wp = StrConvertToIntPtr(richTextBox_wParam.Text); // 动态转换 wParam
                            IntPtr lp = StrConvertToIntPtr(richTextBox_lParam.Text); // 动态转换 lParam
                            IntPtr result = IntPtr.Zero;
                            result = SendMessage(_hWnd, number, wp, lp);

                            //if (result == IntPtr.Zero)
                            //    MessageBox.Show($"已向窗口 {_hWnd.ToString()}，\n发送消息类型为：{Qi.NoST(Qi.NoRN(comboBox_Msg.Text[..6]))}\n附加参数1指针为：{wp}\n附加参数2指针为：{lp}\n\n目标未返回任何消息。", "发送消息");
                            //else
                            //    MessageBox.Show($"已向窗口 {_hWnd.ToString()}，\n发送消息类型为：{Qi.NoST(Qi.NoRN(comboBox_Msg.Text[..6]))}\n附加参数1指针为：{wp}\n附加参数2指针为：{lp}\n\n返回数据：\n{result.ToString()}", "发送消息");
                        }
                        //else
                        //    MessageBox.Show("句柄转换失败！", "错误");
                        //else
                        //    MessageBox.Show("未找到目标程序窗口！", "错误");
                    }
                    /*
                    IntPtr notepadHwnd = FindWindow(comboBox_hWnd.Text, null);
                    if (notepadHwnd != IntPtr.Zero)
                    {
                        IntPtr result = IntPtr.Zero;
                        if (richTextBox_wParam.Text == "IntPtr.Zero" && richTextBox_lParam.Text == "IntPtr.Zero")
                            result = SendMessage(notepadHwnd, number, IntPtr.Zero, IntPtr.Zero);
                        else if (richTextBox_wParam.Text != "IntPtr.Zero" && richTextBox_lParam.Text == "IntPtr.Zero")
                            result = SendMessage(notepadHwnd, number, richTextBox_lParam.Text, IntPtr.Zero);
                        else if (richTextBox_wParam.Text == "IntPtr.Zero" && richTextBox_lParam.Text != "IntPtr.Zero")
                            result = SendMessage(notepadHwnd, number, IntPtr.Zero, richTextBox_lParam.Text);
                        else if (richTextBox_wParam.Text != "IntPtr.Zero" && richTextBox_lParam.Text != "IntPtr.Zero")
                            result = SendMessage(notepadHwnd, number, richTextBox_wParam.Text, richTextBox_lParam.Text);

                        if (result == IntPtr.Zero)
                            MessageBox.Show($"已向窗口 {notepadHwnd.ToString()}，\n发送消息类型为：{Qi.NoST(Qi.NoRN(comboBox_Msg.Text[..6]))}\n附加参数1为：{richTextBox_wParam.Text}，\n附加参数2为：{richTextBox_lParam.Text}\n\n目标未返回任何消息。", "发送消息");
                        else
                            MessageBox.Show($"已向窗口 {notepadHwnd.ToString()}，\n发送消息类型为：{Qi.NoST(Qi.NoRN(comboBox_Msg.Text[..6]))}\n附加参数1为：{richTextBox_wParam.Text}，\n附加参数2为：{richTextBox_lParam.Text}\n\n返回数据：\n{result.ToString()}", "发送消息");
                    }*/

                }
                else
                    MessageBox.Show("消息类型（Msg）错误！\n\n消息类型值必须是uint\n十六进制值必须在最前面\n例如：0x0010（WM_CLOSE）", "错误");
            }
            else
                MessageBox.Show("消息类型（Msg）错误！\n\nuint值至少有六个字节\n例如：0x0010（WM_CLOSE）", "错误");
        }

        /*
        private IntPtr ConvertToIntPtr(dynamic value)
        {
            if (value == null) return IntPtr.Zero;
            if (value is int i) return (IntPtr)i;
            if (value is string s) return Marshal.StringToHGlobalAuto(s);
            if (value is byte[] bytes)
            {
                IntPtr ptr = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, ptr, bytes.Length);
                return ptr;
            }
            throw new ArgumentException("不支持的数据类型！");
        }
        */

        private IntPtr StrConvertToIntPtr(string value)
        {
            // null
            if (string.IsNullOrEmpty(value))
                return IntPtr.Zero;

            // int
            if (value.Length >= 4)
                if (value[..4] == "int.")
                    if (int.TryParse(value[4..], out int num))
                        if (num is int i)
                            return (IntPtr)i;

            // string
            if (value.Length >= 7)
                if (value[..7] == "string.")
                    if (value[7..] is string str)
                        return Marshal.StringToHGlobalAuto(str);

            // bytes
            if (value.Length >= 6)
                if (value[..6] == "bytes.")
                {
                    byte[] bytes = value[6..].Split(' ').Select(s => Convert.ToByte(s, 16)).ToArray();

                    if (bytes is byte[] _bytes)
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(_bytes.Length);
                        Marshal.Copy(_bytes, 0, ptr, _bytes.Length);
                        return ptr;
                    }
                }

            //throw new ArgumentException("不支持的数据类型！");
            MessageBox.Show("不支持的数据类型或你就没写数据类型！\n正确数据格式：[数据类型].[数据内容]\n例如：string.Hello World!", "错误");
            return IntPtr.Zero;
        }

        private void button_FindWindow_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox_hWnd.Text))
                richTextBox_hWnd.Text += "\r\n";
            richTextBox_hWnd.Text += FindWindow(comboBox_FindWindowhWnd.Text, null).ToString();
        }

        private void button_FindWindowOnText_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox_hWnd.Text))
                richTextBox_hWnd.Text += "\r\n";
            richTextBox_hWnd.Text += FindWindow(null, comboBox_FindWindowText.Text).ToString();
        }

        private void button_GetOnEnumWindowsAPI_Click(object sender, EventArgs e)
        {
            richTextBox_hWnd.Clear(); bool AddEnter = false;
            foreach (var handle in GetAllWindows())
            {
                if (AddEnter)
                    richTextBox_hWnd.Text += ($"\r\n{handle.Handle.ToString()}");
                else
                {
                    richTextBox_hWnd.Text += handle.Handle.ToString();
                    AddEnter = true;
                }
            }
        }

        private void button_GetNoVisibleWindows_Click(object sender, EventArgs e)
        {
            richTextBox_hWnd.Clear(); bool AddEnter = false;
            foreach (var handle in GetVisibleWindows())
            {
                if (AddEnter)
                    richTextBox_hWnd.Text += ($"\r\n{handle.ToString()}");
                else
                {
                    richTextBox_hWnd.Text += handle.ToString();
                    AddEnter = true;
                }
            }
        }

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        public static List<WindowInfo> GetAllWindows()
        {
            List<WindowInfo> windows = new List<WindowInfo>();

            EnumWindows((hWnd, lParam) =>
            {
                // 获取窗口标题
                StringBuilder title = new StringBuilder(256);
                GetWindowText(hWnd, title, title.Capacity);

                // 获取进程ID
                GetWindowThreadProcessId(hWnd, out uint processId);

                // 添加到列表
                windows.Add(new WindowInfo
                {
                    Handle = hWnd,
                    Title = title.ToString(),
                    ProcessId = processId
                });

                return true; // 继续枚举
            }, IntPtr.Zero);

            return windows;
        }

        public class WindowInfo
        {
            public IntPtr Handle { get; set; }
            public string Title { get; set; }
            public uint ProcessId { get; set; }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        public static List<IntPtr> GetVisibleWindows()
        {
            List<IntPtr> visibleWindows = new List<IntPtr>();

            EnumWindows((hWnd, lParam) =>
            {
                if (IsWindowVisible(hWnd))
                {
                    visibleWindows.Add(hWnd);
                }
                return true;
            }, IntPtr.Zero);

            return visibleWindows;
        }
    }


}
