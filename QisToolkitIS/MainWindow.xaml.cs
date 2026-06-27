using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
//using Windows.UI.Text;
using Microsoft.UI.Text;
using System;
using System.Diagnostics;

namespace QisToolkitIS;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void CommandBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            ExecuteCommand();
        }
    }

    private void Execute_Click(object sender, RoutedEventArgs e)
    {
        ExecuteCommand();
    }

    private async void ExecuteCommand()
    {
        string cmd = CommandBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(cmd))
            return;

        AppendLine($"> {cmd}");

        Process process = new();

        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = "/C " + cmd;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = true;

        process.OutputDataReceived += Output;
        process.ErrorDataReceived += Output;

        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();

        AppendLine("");
    }

    private void Output(object sender, DataReceivedEventArgs e)
    {
        if (e.Data == null)
            return;

        AppendLine(e.Data);
    }

    private void AppendLine(string text)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            var doc = ConsoleBox.Document;

            doc.Selection.EndKey(TextRangeUnit.Story, false);
            doc.Selection.TypeText(text + Environment.NewLine);

            // 移动到文档末尾，使视图跟随
            doc.Selection.EndKey(TextRangeUnit.Story, false);
        });
    }
}