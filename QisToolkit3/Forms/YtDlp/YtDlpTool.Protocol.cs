using QisToolkit3.Forms.YtDlp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Qi;

namespace QisToolkit3.Forms
{
    public partial class YtDlpTool
    {
        private readonly TaskCompletionSource<bool> _ready = new();
        private YtDlpInvocation _pendingInvocation;
        private bool _isHttpTriggered = false;

        /// <summary>
        /// 由 Program 在 Application.Run 之前调用，注入协议参数
        /// </summary>
        public void ApplyPendingInvocation(YtDlpInvocation invocation)
        {
            _pendingInvocation = invocation;
        }

        /// <summary>
        /// Shown 时如果存在待处理请求，自动跑一次下载
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (_pendingInvocation?.AutoDownload == true && !_isHttpTriggered)
            {
                _ = RunPendingDownloadAsync();
            }
        }

        private async Task RunPendingDownloadAsync()
        {
            try
            {
                await _ready.Task;
                await Task.Delay(300);

                Log.Info("[YtDlp工具] 协议触发：开始自动下载");

                button_DoDownload.Enabled = false;
                button_AutoDownload.Enabled = false;
                button_DoAnalysis.Enabled = false;

                await DoDownload();
            }

            catch (Exception ex)
            {
                Log.Err("[YtDlp工具] 协议触发下载失败: " + ex.Message);
                MessageBox.Show("自动下载失败：\n" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                button_DoDownload.Enabled = true;
                button_AutoDownload.Enabled = true;
                button_DoAnalysis.Enabled = true;

                // 下载完成后清理临时 Cookie 文件
                if (!string.IsNullOrEmpty(_pendingInvocation?.CookieFilePath) && File.Exists(_pendingInvocation.CookieFilePath))
                {
                    try { File.Delete(_pendingInvocation.CookieFilePath); } catch { }
                }
            }
        }

        public void TriggerHttpDownload(YtDlpInvocation invocation)
        {
            _pendingInvocation = invocation;
            _isHttpTriggered = true;

            // 如果 Load 已经完成（窗口已经完全就绪），直接触发
            if (_ready.Task.IsCompleted)
            {
                ApplyInvocationAndDownload();
            }
        }

        private async void ApplyInvocationAndDownload()
        {
            if (_pendingInvocation == null) return;

            comboBox_URL.Text = _pendingInvocation.Url;
            ApplyControlOverrides(_pendingInvocation.Overrides);

            // 防止重复点击下载
            if (button_DoDownload.Enabled)
            {
                await RunPendingDownloadAsync();
            }
        }

        /// <summary>
        /// 把协议里的控件覆盖应用到实际控件上
        /// </summary>
        private void ApplyControlOverrides(Dictionary<string, string> overrides)
        {
            if (overrides == null || overrides.Count == 0) return;

            foreach (var kv in overrides)
            {
                var ctrl = FindControlRecursive(this, kv.Key);
                if (ctrl == null)
                {
                    Log.Warn($"[YtDlp工具] 协议参数：未找到控件 {kv.Key}");
                    continue;
                }

                try
                {
                    SetControlValue(ctrl, kv.Value);
                    Log.Info($"[YtDlp工具] 协议参数：{kv.Key} = {kv.Value}");
                }
                catch (Exception ex)
                {
                    Log.Warn($"[YtDlp工具] 协议参数设置失败 {kv.Key}: {ex.Message}");
                }
            }
        }

        private static Control FindControlRecursive(Control parent, string name)
        {
            if (parent.Name == name) return parent;
            foreach (Control c in parent.Controls)
            {
                var r = FindControlRecursive(c, name);
                if (r != null) return r;
            }
            return null;
        }

        private static void SetControlValue(Control c, string value)
        {
            switch (c)
            {
                case CheckBox cb:
                    cb.Checked = ParseBool(value);
                    break;

                case RadioButton rb:
                    rb.Checked = ParseBool(value);
                    break;

                case TextBox tb:
                    tb.Text = value ?? string.Empty;
                    break;

                case ComboBox combo:
                    combo.Text = value ?? string.Empty;
                    if (combo.DropDownStyle == ComboBoxStyle.DropDownList)
                    {
                        int idx = combo.FindStringExact(combo.Text);
                        if (idx >= 0) combo.SelectedIndex = idx;
                        else Log.Warn($"[YtDlp工具] ComboBox {combo.Name} 找不到选项：{value}");
                    }
                    break;

                case NumericUpDown num:
                    if (decimal.TryParse(value, out var d)) num.Value = d;
                    break;

                default:
                    throw new NotSupportedException($"控件类型 {c.GetType().Name} 不支持协议设置");
            }
        }

        private static bool ParseBool(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            return s.Equals("true", StringComparison.OrdinalIgnoreCase)
                || s == "1"
                || s.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }
    }
}