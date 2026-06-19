using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System;
using System.Text;
using System.Security.Principal;
using System.Management;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Text.RegularExpressions;
using System.Reflection;

public class Qi
{
    public class QisToolkit3_Datas
    {
        public static bool DarkMode = false;
        public static bool ComputerNoviceMode = false;

        public static bool FilesOperation_AutomaticallyPopUpTheOpenFileWindow = false;
        public static bool ToolsOperation_UseChineseName = false;
        public static bool ToolsProcessingTools_TopMost = true;


        public static string exePath = Assembly.GetEntryAssembly().Location;
        public static string actualDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        public static int processId = Process.GetCurrentProcess().Id;
        public static string owner = GetProcessOwner(processId);
        public static bool isSystem = owner.Equals(@"SYSTEM\NT AUTHORITY", StringComparison.OrdinalIgnoreCase);

        public static void SaveDatas()
        {
            Directory.CreateDirectory(@"C:\QiAppDatas\Datas\QisToolkit3\");
            StreamWriter writer = new StreamWriter(@"C:\QiAppDatas\Datas\QisToolkit3\Options.qidata");

            writer.WriteLine(DarkMode);
            writer.WriteLine(ComputerNoviceMode);
            writer.WriteLine(FilesOperation_AutomaticallyPopUpTheOpenFileWindow);
            writer.WriteLine(ToolsOperation_UseChineseName);
            writer.WriteLine(ToolsProcessingTools_TopMost); 
        }

        public static void LoadDatas()
        {
            MainLog($"加载数据中（执行方法 LoadDatas()）");
            try
            {
                if (System.IO.File.Exists(@"C:\QiAppDatas\Datas\QisToolkit3\Options.qidata"))
                {
                    using StreamReader reader = new StreamReader(@"C:\QiAppDatas\Datas\QisToolkit3\Options.qidata");
                    string line;

                    if ((line = reader.ReadLine()) != null) DarkMode = StrToBool(line);
                    if ((line = reader.ReadLine()) != null) ComputerNoviceMode = StrToBool(line);
                    if ((line = reader.ReadLine()) != null) FilesOperation_AutomaticallyPopUpTheOpenFileWindow = StrToBool(line);
                    if ((line = reader.ReadLine()) != null) ToolsOperation_UseChineseName = StrToBool(line);
                    if ((line = reader.ReadLine()) != null) ToolsProcessingTools_TopMost = StrToBool(line);
                }
            }
            catch (Exception ex)
            {
                MainLog($"执行函数 LoadDatas() 时发送错误，错误信息：{ex.Message}\n完整错误：\n{ex.ToString()}");
            }
        }
    }

    public class Web
    {
        public const string baidu = "https://www.baidu.com";
        public const string sogou = "https://www.sogou.com";
        public const string google = "https://www.google.com";
        public const string bing = "https://cn.bing.com";


        public const string AirSpace = "https://www.baidu.com";
        public class SE
        {
            private const string baidu_wd = "s?wd=";
            private const string sogou_wd = "web?query=";
            private const string google_wd = "search?q=";
            private const string bing_wd = "search?q=";

            public static void DoBaiDu(string _wd = "")
            {
                Process.Start(baidu + "/" + baidu_wd + _wd);
            }
            public static void DoSoGou(string _wd = "")
            {
                Process.Start(sogou + "/" + sogou_wd + _wd);
            }
            public static void DoGoogle(string _wd = "")
            {
                Process.Start(google + "/" + google_wd + _wd);
            }
            public static void DoBing(string _wd = "", bool ensearch = false)
            {
                if (ensearch)
                    Process.Start(bing + "/" + bing_wd + _wd + "&FORM=qi&" + "ensearch=1");
                else
                    Process.Start(bing + "/" + bing_wd + _wd + "&FORM=qi&" + "ensearch=0");
            }
        }
    }

    public class CleanUpTrash
    {
        // 清理所有
        public static void RunAll(string OutLogFile = @"C:\1.log")
        {
            StreamWriter LogFile = new StreamWriter(OutLogFile);
            LogFile.WriteLine(LogTextTime() + "开始清理...\n");

            LogFile.WriteLine(LogTextTime() + "清理系统文件夹（C:\\Windows\\Temp）：\n" + CleanUpSystemTemp() + LogTextTime() + "项目已完成.\n");
            LogFile.WriteLine(LogTextTime() + "清理内存转换文件（C:\\Windows\\MEMORY.DMP）：\n" + CleanUpMEMORY() + LogTextTime() + "项目已完成.\n");

            LogFile.Close();
        }

        #region 单独清理项目

        // 清理系统文件夹（C:\Windows\Temp）
        public static string CleanUpSystemTemp()
        {
            return TryDeleteDirectoryNd(@"C:\Windows\Temp");
        }

        // 清理内存转换文件（C:\Windows\MEMORY.DMP）
        public static string CleanUpMEMORY()
        {
            return TryDeleteFile(@"C:\Windows\MEMORY.DMP");
        }

        #endregion
    }

    // 运行 Cmd 命令
    public static string ExecuteInCmd(string cmdline)
    {
        using (var process = new Process())
        {
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.StandardInput.AutoFlush = true;
            process.StandardInput.WriteLine(cmdline + "&exit");

            string output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();
            process.Close();
            return output;
        }
    }

    // 运行 NSudo
    public static void RunNSudo(string exefile, string arguments = "-U:T -P:E -M:S -Priority:RealTime", bool CMDReLoad = false)
    {
        string nsudoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NSudoL.exe");
        
        if (CMDReLoad)
            ExecuteInCmd($"{nsudoPath} {arguments} {exefile}");

        else
        {
            string log = string.Empty;

            try
            {
                // 构建完整命令参数
                string nsudoArgs = $"-U:T -P:E -M:S -Priority:RealTime \"{exefile}\"";

                if (!string.IsNullOrEmpty(arguments))
                {
                    nsudoArgs += $" {arguments}";
                }

                // 配置进程启动信息
                var startInfo = new ProcessStartInfo
                {
                    FileName = nsudoPath,
                    Arguments = nsudoArgs,
                    UseShellExecute = false, // 重定向输出需要设为 false
                    CreateNoWindow = true,   // 不创建新窗口
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                // 启动进程
                using (var process = new Process { StartInfo = startInfo })
                {
                    // 捕获输出事件
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                            log += $"[NSudo] {e.Data}";
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                            log += $"[NSudo Error] {e.Data}";
                    };

                    process.Start();

                    // 开始异步读取输出
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    // 等待进程退出（可选）
                    // process.WaitForExit();

                    // 获取退出代码（如果需要）
                    // int exitCode = process.ExitCode;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行 NSudo 时出错: {ex.Message}", "错误",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
            

        //if (log != string.Empty)
        //    MessageBox.Show(log);
        // 
    }

    // 尝试删除文件
    public static string TryDeleteFile(string path)
    {
        try
        {
            long FileLengthTmp = new FileInfo(path).Length;
            System.IO.File.Delete(path);
            return MakeLogText_1(path, FileLengthTmp);
        }
        catch (Exception ex)
        {
            return MakeLogText_0(path, ex);
        }
    }

    // 尝试删除文件夹下的所有子文件与子目录或目录树
    public static string TryDeleteDirectoryNd(string targetDir, bool delSelf = false)
    {
        string CleanUpLog = "";

        if (Directory.Exists(targetDir))
        {
            long FileLengthTmp;
            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                try
                {
                    System.IO.File.SetAttributes(file, FileAttributes.Normal);
                    try
                    {
                        FileLengthTmp = new FileInfo(file).Length;
                        System.IO.File.Delete(file);
                        CleanUpLog += MakeLogText_1(file, FileLengthTmp);
                    }
                    catch (Exception ex)
                    {
                        CleanUpLog += MakeLogText_0(file, ex);
                    }
                }
                catch (Exception ex)
                {
                    CleanUpLog += MakeLogText__0(file, ex);
                }
            }

            foreach (string dir in dirs)
            {
                if (dir != targetDir)
                {
                    TryDeleteDirectoryNd(dir);

                    try
                    {
                        Directory.Delete(dir);
                    }
                    catch (Exception ex)
                    {
                        CleanUpLog += MakeLogText__0(dir, ex);
                    }
                }
                
            }
        }

        try
        {
            if (delSelf) Directory.Delete(targetDir);
        }
        catch
        {

        }

        return CleanUpLog;
    }

    public static void RestartExplorer()
    {
        Process process = new Process();
        process.StartInfo.FileName = "explorer.exe";

        foreach (var proc in Process.GetProcessesByName("explorer"))
            proc.Kill();
    }

    // 文件夹是否为空
    public static bool IsDirectoryEmpty(string path)
    {
        return !Directory.EnumerateFileSystemEntries(path).Any();
    }

    // 删除复制文件
    public static string TryFileCopy(string path1, string path2)
    {
        try
        {
            System.IO.File.Copy(path1, path2, true);
            return "文件复制成功。";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    // 尝试读取文件内所有内容
    public static string TryReadAllText(string path)
    {
        try
        {
            if (System.IO.File.Exists(path))
            {
                return System.IO.File.ReadAllText(path).ToString();
            }
            return path + " 文件不存在";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    // 制作日志格式：目录获取失败
    public static string MakeLogText__0(string path, Exception Ex)
    {
        return "[" + DateTime.Now + "] 目录权柄获取失败，原因：" + Ex.Message + " （" + path + "）\n";
    }

    // 制作日志格式：文件删除失败
    public static string MakeLogText_0(string path, Exception Ex)
    {
        return "[" + DateTime.Now + "] 删除文件失败，原因：" + Ex.Message + " （" + path + "）\n";
    }

    // 制作日志格式：文件删除成功
    public static string MakeLogText_1(string path, long FileLength)
    {
        return "[" + DateTime.Now + "] 成功删除文件，大小：" + FileLength + " （" + path + "）\n";
    }

    // 制作日志格式：结束进程失败
    public static string MakeLogText_PK0(int pid, Exception Ex)
    {
        return "[" + DateTime.Now + "] 强制结束进程失败，原因：" + Ex.Message + " （" + pid + "）\n";
    }

    // 制作日志格式：结束进程成功
    public static string MakeLogText_PK1(int pid)
    {
        return "[" + DateTime.Now + "] 已强制结束进程，PID：" + pid + '\n';
    }

    // 获取日志格式式时间
    public static string LogTextTime()
    {
        return "[" + DateTime.Now + "] ";
    }

    // 根据进程名获取PID
    public static int GetPidByProcessName(string processName)
    {
        Process[] arrayProcess = Process.GetProcessesByName(processName);

        foreach (Process p in arrayProcess)
            return p.Id;
        return 0;
    }

    // 尝试关闭进程
    public static string TryProcKill(string path)
    {
        int pid = -1;
        try
        {
            foreach (var proc in Process.GetProcessesByName(path))
            {
                pid = proc.Id;
                proc.Kill();
                proc.WaitForExit();
            }
            return MakeLogText_PK1(pid);
        }
        catch (Exception ex)
        {
            return MakeLogText_PK0(pid, ex);
        }
    }

    // 判断注册表项是否存在
    public static bool HkImExists(RegistryKey hkIm, string ExistsHkTm)
    {
        try
        {
            hkIm.OpenSubKey(ExistsHkTm);
            return true;
        }
        catch { return false; }
    }

    // 尝试删除删除注册表项树
    public static string HkImTryDeleteTree(RegistryKey hkIm, string DeleteHkTm)
    {
        try
        {
            hkIm.DeleteSubKeyTree(DeleteHkTm);
            return "删除成功";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    // 删除删除文件夹及其所有子项
    public static void DeleteDirectory(string targetDir)
    {
        if (Directory.Exists(targetDir))
        {
            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                System.IO.File.SetAttributes(file, FileAttributes.Normal);
                System.IO.File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(targetDir, true);
        }
    }

    // 计算算术前置 - 字符串获取数字
    public static int arithmetic_GetNum(string ArithmeticStr, int StartNum)
    {
        int number = 0, i = StartNum;
        bool HeadNoGet = true;
        string ArithmeticStrTmp = "";
        while
        (
            ArithmeticStr[i] == '-' && HeadNoGet || ArithmeticStr[i] == '+' && HeadNoGet ||
            ArithmeticStr[i] == '0' || ArithmeticStr[i] == '1' ||
            ArithmeticStr[i] == '2' || ArithmeticStr[i] == '3' ||
            ArithmeticStr[i] == '4' || ArithmeticStr[i] == '5' ||
            ArithmeticStr[i] == '6' || ArithmeticStr[i] == '7' ||
            ArithmeticStr[i] == '8' || ArithmeticStr[i] == '9'
        )
        {
            if (ArithmeticStr[i] == '-' || ArithmeticStr[i] == '+')
                HeadNoGet = false;
            ArithmeticStrTmp += ArithmeticStr[i];
        }

        if (int.TryParse(ArithmeticStrTmp, out number))
            return number;
        return -1;
    }

    // 计算算术前置 - 获取字符属于哪类
    public static int arithmetic_GetCharType(char ArCh)
    {
        if (ArCh == '-' || ArCh == '+' || ArCh == '*' || ArCh == '/')
            return 1;
        else if (
            ArCh == '0' || ArCh == '1' || ArCh == '2' ||
            ArCh == '3' || ArCh == '4' || ArCh == '5' ||
            ArCh == '6' || ArCh == '7' || ArCh == '8' || ArCh == '9'
        )
            return 0;
        else
            return -1;
    }

    // 计算算术
    public static int arithmetic(string ArithmeticStr)
    {
        int number = 0, numberTmp = 0, i = 0;
        string ArithmeticStrTmp = ArithmeticStr;
        while (ArithmeticStr.Length > i)
        {
            if (arithmetic_GetCharType(ArithmeticStr[i]) == 0)
            {
                numberTmp = arithmetic_GetNum(ArithmeticStr, i);
                i += numberTmp.ToString().Length;
            }

            i++;
        }


        return number;
    }

    // String 转 Int
    public static int StrToInt(string StrNumber)
    {
        if (Int32.TryParse(StrNumber, out int number))
        {
            return number;
        }
        else
        {
            return -1;
        }
    }

    // Bool 转 Int
    public static int BoolToInt(bool BoolData)
    {
        if (BoolData)
            return 1;
        else
            return 0;
    }

    // String 转 Float
    public static float StrToFloat(string StrNumber)
    {
        float result;
        if (float.TryParse(StrNumber, out result))
        {
            return result;
        }
        return -1f;
    }

    // 尝试 String 转 Bool
    public static bool TryStrToBool(string StrBool)
    {
        if (StrBool.ToLower() == "true" || StrBool.ToLower() == "false")
            return true;
        return false;
    }

    // 执行 String 转 Bool
    public static bool StrToBool(string StrBool)
    {
        if (StrBool != null)
            if (StrBool.ToLower() == "true")
                return true;
        return false;
    }

    // 执行 String 转 DateTime
    public static DateTime StrToDateTime(string StrDateTime)
    {
        DateTime time;
        if (DateTime.TryParse(StrDateTime, out time))
            return time;
        return DateTime.MinValue;
    }

    // 执行 String 转 Encoding
    public static Encoding StrToEncoding(string StrEncoding)
    {
        if (StrEncoding == "UTF8" || StrEncoding == "UTF-8")
            return Encoding.UTF8;
        else if (StrEncoding == "UTF7" || StrEncoding == "UTF-7")
            return Encoding.UTF7;
        else if (StrEncoding == "UTF16 LE" || StrEncoding == "UTF-16 LE" || StrEncoding == "Unicode")
            return Encoding.Unicode;
        else if (StrEncoding == "UTF16 BE" || StrEncoding == "UTF-16 BE" || StrEncoding == "BigEndianUnicode" || StrEncoding == "Big Endian Unicode")
            return Encoding.BigEndianUnicode;
        else if (StrEncoding == "UTF32" || StrEncoding == "UTF-32")
            return Encoding.UTF32;
        else if (StrEncoding == "Latin1")
            return Encoding.Latin1;
        else if (StrEncoding == "ASCII")
            return Encoding.ASCII;
        else if (StrEncoding == "ANSI")
            return Encoding.GetEncoding("GB2312");
        else if (StrEncoding == "Default" || StrEncoding == "系统默认" || StrEncoding == "默认")
            return Encoding.Default;
        return null;
    }

    // 智能转换
    public static string BytesIntelligentConversion(long bytes)
    {
        if (bytes >= 1024 && bytes < 1048576) return BytesToKilobytes(bytes).ToString("0.00") + "KB";
        else if (bytes >= 1048576 && bytes < 1073741824) return BytesToMegabytes(bytes).ToString("0.00") + "MB";
        else if (bytes >= 1073741824) return BytesToGigabytes(bytes).ToString("0.00") + "GB";
        return bytes.ToString("0.00") + "B";
    }

    // 字节转换为KB、MB、GB
    // KB
    public static float BytesToKilobytes(long bytes)
    {
        return bytes / 1024;
    }

    // MB
    public static float BytesToMegabytes(long bytes)
    {
        return BytesToKilobytes(bytes) / 1024;
    }

    // GB
    public static float BytesToGigabytes(long bytes)
    {
        return BytesToMegabytes(bytes) / 1024;
    }

    // 其他单位转换为字节
    public static long KilobytesToBytes(double kilobytes)
    {
        return Convert.ToInt64(kilobytes * 1024);
    }

    public static long MegabytesToBytes(double megabytes)
    {
        return KilobytesToBytes(megabytes) * 1024;
    }

    public static long GigabytesToBytes(double gigabytes)
    {
        return MegabytesToBytes(gigabytes) * 1024;
    }

    // 设置 Windows 用户密码
    public static bool SetWindowsUserPassword(string password, string UserName = "__Auto__")
    {
        if (UserName == "__Auto__") UserName = Environment.UserName;

        string _Path = "WinNT://" + Environment.MachineName;
        System.DirectoryServices.DirectoryEntry machine = new System.DirectoryServices.DirectoryEntry(_Path);
        //获得计算机实例
        System.DirectoryServices.DirectoryEntry user = machine.Children.Find(UserName, "User");
        //找得用户
        if (user != null)
        {
            user.Invoke("SetPassword", password); //用户密码
            user.CommitChanges();
        }
        // 返回值
        return user != null;
    }


    public static void MessageBoxReadFile(string path) =>
        MessageBox.Show(System.IO.File.ReadAllText(path), "文件阅读");

    public static bool IsAdministrator()
    {
        WindowsIdentity current = WindowsIdentity.GetCurrent();
        WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
        //WindowsBuiltInRole可以枚举出很多权限，例如系统用户、User、Guest等等
        return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    // 打开文件
    public static void OpenFile(string filePath)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = filePath,
            UseShellExecute = true // 这会使用默认的程序打开文件
        };

        Process.Start(startInfo);
    }

    // 输出二进制文件
    public static void ExportBinaryFile(string filePath, byte[] data)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fileStream))
                writer.Write(data);
    }

    // 将字符串转为二进制字符串
    public static string StringToByte(string data)
    {
        byte[] byteData = Encoding.Unicode.GetBytes(data);
        StringBuilder stringBuilder = new StringBuilder(byteData.Length * 8);
        foreach (byte b in byteData)
            stringBuilder.Append(Convert.ToString(b, 2).PadLeft(8, '0'));

        return stringBuilder.ToString();
    }

    // 将二进制字符串转为标准字符串
    public static string BytetoString(string str)
    {
        try
        {
            CaptureCollection cs = Regex.Match(str, @"([01]{8})+").Groups[1].Captures;
            byte[] bytes = new byte[cs.Count];
            for (int i = 0; i < cs.Count; ++i)
                bytes[i] = Convert.ToByte(cs[i].Value, 2);
            return Encoding.Unicode.GetString(bytes, 0, bytes.Length);
        }
        catch
        {
            return string.Empty;
        }
    }

    // Int 转 罗马数字
    public static string IntToRoman(int value)
    {
        int[] nums = new int[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
        string[] romans = new string[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
        string result = "";
        int start = 0;
        while (value > 0)
        {
            for (int i = start, l = nums.Length; i < l; ++i)
            {
                if (value >= nums[i])
                {
                    value -= nums[i];
                    result += romans[i];
                    start = i;
                    break;
                }
            }
        }

        return result;
    }

    // 去除 \r \n 字符
    public static string NoRN(string _str)
    {
        string str = "";

        foreach (char s in _str)
            if (s != '\r' && s != '\n')
                str += s;

        return str;
    }

    // 去除 空格 TAB 字符
    public static string NoST(string _str)
    {
        string str = "";

        foreach (char s in _str)
            if (s != ' ' && s != '\t')
                str += s;

        return str;
    }

    // 实化 \r \n 字符
    public static string TextRN(string _str)
    {
        string str = "";

        foreach (char s in _str)
        {
            if (s == '\n')
                str += "\\n";
            else if (s == '\r')
                str += "\\r";
            else
                str += s;
        }

        return str;
    }




    public static void SetHiddenFileMain(string path, bool data)
    {
        try
        {
            if (data)
                System.IO.File.SetAttributes(path, System.IO.File.GetAttributes(path) | FileAttributes.Hidden);
            else
                System.IO.File.SetAttributes(path, System.IO.File.GetAttributes(path) & ~FileAttributes.Hidden);
        }
        catch (Exception ex)
        {
            if (MessageBox.Show("文件修改属性失败，原因：" + ex.Message, "错误", MessageBoxButtons.CancelTryContinue) == DialogResult.TryAgain)
                SetHiddenFileMain(path, data);
        }
    }

    public static void SetArchiveFileMain(string path, bool data)
    {
        try
        {
            if (data)
                System.IO.File.SetAttributes(path, System.IO.File.GetAttributes(path) | FileAttributes.Archive);
            else
                System.IO.File.SetAttributes(path, System.IO.File.GetAttributes(path) & ~FileAttributes.Archive);
        }
        catch (Exception ex)
        {
            if (MessageBox.Show("文件修改属性失败，原因：" + ex.Message, "错误", MessageBoxButtons.CancelTryContinue) == DialogResult.TryAgain)
                SetArchiveFileMain(path, data);
        }
    }

    public static void SetReadOnlyFileMain(string path, bool data)
    {
        try
        {
            if (data)
                System.IO.File.SetAttributes(path, System.IO.File.GetAttributes(path) | FileAttributes.ReadOnly);
            else
                System.IO.File.SetAttributes(path, System.IO.File.GetAttributes(path) & ~FileAttributes.ReadOnly);
        }
        catch (Exception ex)
        {
            if (MessageBox.Show("文件修改属性失败，原因：" + ex.Message, "错误", MessageBoxButtons.CancelTryContinue) == DialogResult.TryAgain)
                SetReadOnlyFileMain(path, data);
        }
    }

    public static void SetSystemFileMain(string path, bool data)
    {
        try
        {
            if (data)
                System.IO.File.SetAttributes(path, System.IO.File.GetAttributes(path) | FileAttributes.System);
            else
                System.IO.File.SetAttributes(path, System.IO.File.GetAttributes(path) & ~FileAttributes.System);
        }
        catch (Exception ex)
        {
            if (MessageBox.Show("文件修改属性失败，原因：" + ex.Message, "错误", MessageBoxButtons.CancelTryContinue) == DialogResult.TryAgain)
                SetSystemFileMain(path, data);
        }
    }

    public static void SetNotContentIndexedFileMain(string path, bool data)
    {
        try
        {
            if (data)
                System.IO.File.SetAttributes(path, System.IO.File.GetAttributes(path) | FileAttributes.NotContentIndexed);
            else
                System.IO.File.SetAttributes(path, System.IO.File.GetAttributes(path) & ~FileAttributes.NotContentIndexed);
        }
        catch (Exception ex)
        {
            if (MessageBox.Show("文件修改属性失败，原因：" + ex.Message, "错误", MessageBoxButtons.CancelTryContinue) == DialogResult.TryAgain)
                SetNotContentIndexedFileMain(path, data);
        }
    }

    public static void SetOfflineFileMain(string path, bool data)
    {
        try
        {
            if (data)
                System.IO.File.SetAttributes(path, System.IO.File.GetAttributes(path) | FileAttributes.Offline);
            else
                System.IO.File.SetAttributes(path, System.IO.File.GetAttributes(path) & ~FileAttributes.Offline);
        }
        catch (Exception ex)
        {
            if (MessageBox.Show("文件修改属性失败，原因：" + ex.Message, "错误", MessageBoxButtons.CancelTryContinue) == DialogResult.TryAgain)
                SetOfflineFileMain(path, data);
        }
    }

    public static bool IsTextFile(string filePath)
    {
        try
        {
            const int bufferSize = 1024; // 读取文件的前 1024 字节
            byte[] buffer = new byte[bufferSize];

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int bytesRead = fs.Read(buffer, 0, bufferSize);
                for (int i = 0; i < bytesRead; i++)
                {
                    byte b = buffer[i];
                    if (b < 32 && b != 9 && b != 10 && b != 13) // 排除制表符、换行符和回车符
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }






    public static Encoding DetectFileEncoding(string filePath)
    {
        // 首先检查 BOM
        byte[] bom = new byte[4];
        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            fs.Read(bom, 0, 4);
        }

        if (bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF)
        {
            return Encoding.UTF8;
        }
        else if (bom[0] == 0xFF && bom[1] == 0xFE)
        {
            return Encoding.Unicode;
        }
        else if (bom[0] == 0xFE && bom[1] == 0xFF)
        {
            return Encoding.BigEndianUnicode;
        }
        else if (bom[0] == 0x00 && bom[1] == 0x00 && bom[2] == 0xFE && bom[3] == 0xFF)
        {
            return Encoding.UTF32;
        }

        // 如果没有 BOM，尝试根据内容特征推测
        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (BinaryReader br = new BinaryReader(fs))
        {
            int asciiCount = 0;
            int nonAsciiCount = 0;
            long totalBytesRead = 0;
            const int sampleSize = 1024;

            while (fs.Position < fs.Length && totalBytesRead < sampleSize)
            {
                byte b = br.ReadByte();
                totalBytesRead++;
                if (b < 128)
                {
                    asciiCount++;
                }
                else
                {
                    nonAsciiCount++;
                }
            }

            // 如果大部分是 ASCII 字符，推测为 ASCII 编码
            if ((double)asciiCount / (asciiCount + nonAsciiCount) > 0.9)
            {
                return Encoding.ASCII;
            }

            // 这里可以添加更多复杂的逻辑来进一步推测其他编码
            // 例如，如果文件中有中文字符，可能是 GB2312、GBK 等
            // 但这种判断会更复杂，且不一定准确

            return null;
        }
    }

    public static bool IsRuning(string exeName)
    {
        return Process.GetProcessesByName(exeName).ToList().Count > 0;
    }



    // 需要导入的Win32 API
    private const int HWND_BROADCAST = 0xffff;
    private const int WM_SETTINGCHANGE = 0x001A;
    private const int SMTO_ABORTIFHUNG = 0x0002;

    [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    private static extern IntPtr SendMessageTimeout(
        IntPtr hWnd, uint Msg, IntPtr wParam, string lParam,
        uint fuFlags, uint uTimeout, out IntPtr lpdwResult);
    public static void SetSystemEnvironmentVariable(string name, string value)
    {
        // 打开系统环境变量的注册表键
        using (RegistryKey envKey = Registry.LocalMachine.OpenSubKey(
            @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
            true))
        {
            if (envKey != null)
            {
                // 设置环境变量
                envKey.SetValue(name, value, RegistryValueKind.ExpandString);

                // 通知系统环境变量已更改
                Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Machine);

                // 发送广播消息通知所有窗口环境变量已更改
                SendMessageTimeout(
                    new IntPtr(HWND_BROADCAST), WM_SETTINGCHANGE,
                    IntPtr.Zero, "Environment", SMTO_ABORTIFHUNG, 5000, out _);
            }
        }
    }


    // 添加系统环境变量
    public static void AddToSystemPath(string newPath)
    {
        using (RegistryKey envKey = Registry.LocalMachine.OpenSubKey(
            @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment", true))
        {
            if (envKey != null)
            {
                string currentPath = envKey.GetValue("Path", "", RegistryValueOptions.DoNotExpandEnvironmentNames) as string;

                if (!currentPath.EndsWith(";"))
                {
                    currentPath += ";";
                }

                if (!currentPath.Contains(newPath))
                {
                    envKey.SetValue("Path", currentPath + newPath, RegistryValueKind.ExpandString);

                    // 通知系统
                    Environment.SetEnvironmentVariable("Path", currentPath + newPath, EnvironmentVariableTarget.Machine);
                    SendMessageTimeout(
                        new IntPtr(HWND_BROADCAST), WM_SETTINGCHANGE,
                        IntPtr.Zero, "Environment", SMTO_ABORTIFHUNG, 5000, out _);
                }
            }
        }
    }


    // 判断指定系统环境变量
    public static bool IsPathInSystemEnvironment(string pathToCheck, bool expandEnvironmentVariables = false)
    {
        string systemPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine) ?? "";

        if (expandEnvironmentVariables)
        {
            systemPath = Environment.ExpandEnvironmentVariables(systemPath);
            pathToCheck = Environment.ExpandEnvironmentVariables(pathToCheck);
        }

        pathToCheck = NormalizePath(pathToCheck);

        string[] paths = systemPath.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string path in paths)
        {
            string normalizedPath = expandEnvironmentVariables
                ? NormalizePath(path)
                : NormalizePath(Environment.ExpandEnvironmentVariables(path));

            if (normalizedPath.Equals(pathToCheck, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }


    // 移除指定系统环境变量
    public static void RemovePathFromSystemEnvironment(string path)
    {
        using (RegistryKey envKey = Registry.LocalMachine.OpenSubKey(
            @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
            true))
        {
            if (envKey != null)
            {
                string pathValue = envKey.GetValue("PATH", "", RegistryValueOptions.DoNotExpandEnvironmentNames) as string;

                if (!string.IsNullOrEmpty(pathValue))
                {
                    // 标准化要移除的路径
                    path = NormalizePath(path);

                    // 分割PATH并过滤掉要移除的路径
                    var paths = pathValue.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(p => !NormalizePath(p).Equals(path, StringComparison.OrdinalIgnoreCase));

                    // 重新组合PATH
                    string newPathValue = string.Join(";", paths);

                    // 更新注册表
                    envKey.SetValue("PATH", newPathValue, RegistryValueKind.ExpandString);

                    // 通知系统
                    Environment.SetEnvironmentVariable("PATH", newPathValue, EnvironmentVariableTarget.Machine);
                    SendMessageTimeout(
                        new IntPtr(HWND_BROADCAST), WM_SETTINGCHANGE,
                        IntPtr.Zero, "Environment", SMTO_ABORTIFHUNG, 5000, out _);
                }
            }
        }
    }


    // 标准化路径格式（去除尾部斜杠，统一大小写等）
    private static string NormalizePath(string path)
    {
        return path.Trim()
                   .TrimEnd('\\')
                   .TrimEnd('/')
                   .ToLower(); // 使用OrdinalIgnoreCase比较时可省略
    }


    // 路径文件前面加字符串
    public static string AddPrefixToFileName(string filePath, string prefix)
    {
        // 获取目录路径（C:\a\b\c）
        string directory = Path.GetDirectoryName(filePath);

        // 获取文件名（1.txt）
        string fileName = Path.GetFileName(filePath);

        // 在文件名前加前缀（New1.txt）
        string newFileName = prefix + fileName;

        // 组合成新路径（C:\a\b\c\New1.txt）
        return Path.Combine(directory, newFileName);
    }


    // 读取文件信息前 maxChars 个
    public static string ReadFirstChars(string filePath, int maxChars)
    {
        using (var reader = new StreamReader(filePath))
        {
            char[] buffer = new char[maxChars];
            int charsRead = reader.Read(buffer, 0, maxChars);
            return new string(buffer, 0, charsRead);
        }
    }


    // 主日志
    public static void MainLog(string message)
    {
        System.IO.File.AppendAllText(QisToolkit3_Datas.actualDirectory + @"\Logs\Main.log", $"{LogTextTime()} {message}\n");
    }


    public static bool IsRootDirectory(string path)
    {
        // 规范化路径（移除末尾的 \）
        path = Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar);

        // 获取路径的根目录部分（如 "C:\"）
        string root = Path.GetPathRoot(path);

        // 如果路径和根目录相同，则是磁盘根目录
        return path.Equals(root, StringComparison.OrdinalIgnoreCase);
    }


    // 获取 Unicode 字符
    public static string GetUnicodeCharacter(string input)
    {
        try
        {
            // 1. 清理输入字符串
            string hexString = input.Trim().ToUpper();

            // 2. 移除可能的"U+"前缀
            if (hexString.StartsWith("U+"))
                hexString = hexString.Substring(2);

            // 3. 解析十六进制数值
            int codePoint = Convert.ToInt32(hexString, 16);

            // 4. 验证Unicode范围
            if (codePoint < 0 || codePoint > 0x10FFFF)
                throw new ArgumentException("Unicode值超出有效范围 (0x0 - 0x10FFFF)");

            return char.ConvertFromUtf32(codePoint);

            //// 5. 转换代码点为字符/字符串
            //string unicodeChar = char.ConvertFromUtf32(codePoint);

            //// 6. 复制到剪贴板
            //Clipboard.SetText(unicodeChar);

            //Console.WriteLine($"成功复制: U+{codePoint:X4} -> '{unicodeChar}'");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"错误: {ex.Message}");
            return null;
        }
    }

    /*private static int Main()
    {
        // 获取当前程序所在目录（相当于 PowerShell 的 %~dp0）
        string currentDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\";

        try
        {
            // 连接到 WMI 命名空间
            ManagementScope scope = new ManagementScope(@"root\Microsoft\Windows\Defender");
            scope.Connect();

            // 准备参数对象
            ManagementClass mpPreference = new ManagementClass(scope, new ManagementPath("MSFT_MpPreference"), null);
            ManagementBaseObject inParams = mpPreference.GetMethodParameters("Add");

            // 设置参数
            inParams["ExclusionPath"] = new string[] { currentDir };
            inParams["Force"] = true;

            // 调用方法
            ManagementBaseObject outParams = mpPreference.InvokeMethod("Add", inParams, null);

            // 检查返回状态
            uint returnValue = (uint)outParams["ReturnValue"];
            return returnValue == 0 ? 0 : 1; // 0 表示成功
        }
        catch (ManagementException ex)
        {
            Console.Error.WriteLine($"WMI Error: {ex.Message}");
            return (int)ex.ErrorCode;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return ex.HResult;
        }
    }
    */

    public static string GetProcessOwner(int processId)
    {
        string query = $"SELECT * FROM Win32_Process WHERE ProcessId = {processId}";
        using (var searcher = new ManagementObjectSearcher(query))
        {
            foreach (ManagementObject obj in searcher.Get()) 
            {
                object[] args = { null, null };
                object result = obj.InvokeMethod("GetOwner", args);  
                if (result != null && (uint)result == 0)  
                {
                    string domain = args[0]?.ToString();
                    string user = args[1]?.ToString();
                    return @$"{domain}\{user}";
                }
            }
        }
        return "未知";
    }
}