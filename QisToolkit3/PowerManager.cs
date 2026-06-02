using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;
using System.Xml.Linq;

public static class PowerSchemes
{
    // 系统内置的四个主要电源方案 GUID
    public static readonly Guid UltimatePerformance2 = new Guid("aef5e5c2-7f47-40f4-abfb-807175dc2564");  // 卓越性能
    public static readonly Guid UltimatePerformance = new Guid("e9a42b02-d5df-448d-aa00-03f14749eb61");  // 卓越性能
    public static readonly Guid HighPerformance = new Guid("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c");     // 高性能
    public static readonly Guid Balanced = new Guid("381b4222-f694-41f0-9685-ff5bb260df2e");            // 平衡
    public static readonly Guid PowerSaver = new Guid("a1841308-3541-4fab-bc81-f71556f20b4a");          // 节能

    // 获取方案的显示名称
    public static string GetSchemeName(Guid schemeGuid)
    {
        if (schemeGuid == UltimatePerformance || schemeGuid == UltimatePerformance2) return "卓越性能";
        if (schemeGuid == HighPerformance) return "高性能";
        if (schemeGuid == Balanced) return "平衡";
        if (schemeGuid == PowerSaver) return "节能";
        return "未知方案";
    }

    // 获取所有方案的列表
    public static List<(Guid Guid, string Name)> GetAllSchemes()
    {
        return new List<(Guid, string)>
        {
            (UltimatePerformance, "卓越性能"),
            (HighPerformance, "高性能"),
            (Balanced, "平衡"),
            (PowerSaver, "节能")
        };
    }
}

public static class PowerManager
{
    // 错误代码常量
    private const uint ERROR_SUCCESS = 0;
    private const uint ERROR_NOT_FOUND = 1168;
    private const uint ERROR_ALREADY_EXISTS = 183;
    private const uint ERROR_MORE_DATA = 234;
    private const uint ACCESS_SCHEME = 0;

    // GUID 常量
    private const string ULTIMATE_PERFORMANCE_GUID = "e9a42b02-d5df-448d-aa00-03f14749eb61";
    private const string HIGH_PERFORMANCE_GUID = "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c";
    private const string BALANCED_GUID = "381b4222-f694-41f0-9685-ff5bb260df2e";
    private const string POWER_SAVER_GUID = "a1841308-3541-4fab-bc81-f71556f20b4a";

    // API 导入
    [DllImport("powrprof.dll", SetLastError = true)]
    private static extern uint PowerGetActiveScheme(
        IntPtr RootPowerKey,
        out IntPtr ActivePolicyGuid);

    [DllImport("powrprof.dll", SetLastError = true)]
    private static extern uint PowerSetActiveScheme(
        IntPtr RootPowerKey,
        ref Guid SchemeGuid);

    [DllImport("powrprof.dll", SetLastError = true)]
    private static extern uint PowerDuplicateScheme(
        IntPtr RootPowerKey,
        ref Guid SourceGuid,
        out IntPtr DestinationGuid);

    [DllImport("powrprof.dll", SetLastError = true)]
    private static extern uint PowerDeleteScheme(
        IntPtr RootPowerKey,
        ref Guid SchemeGuid);

    [DllImport("powrprof.dll", SetLastError = true)]
    private static extern uint PowerEnumerate(
        IntPtr RootPowerKey,
        IntPtr SchemeGuid,
        IntPtr SubGroupOfPowerSettingsGuid,
        uint AccessFlags,
        uint Index,
        byte[] Buffer,
        ref uint BufferSize);

    [DllImport("powrprof.dll", SetLastError = true)]
    private static extern uint PowerReadFriendlyName(
        IntPtr RootPowerKey,
        ref Guid SchemeGuid,
        IntPtr SubGroupOfPowerSettingsGuid,
        IntPtr PowerSettingGuid,
        byte[] Buffer,
        ref uint BufferSize);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr LocalFree(IntPtr hMem);

    /// <summary>
    /// 获取当前活动的电源方案
    /// </summary>
    public static Guid? GetActiveScheme()
    {
        IntPtr activeGuidPtr = IntPtr.Zero;

        try
        {
            uint result = PowerGetActiveScheme(IntPtr.Zero, out activeGuidPtr);

            if (result == ERROR_SUCCESS && activeGuidPtr != IntPtr.Zero)
            {
                return (Guid)Marshal.PtrToStructure(activeGuidPtr, typeof(Guid));
            }

            return null;
        }
        finally
        {
            if (activeGuidPtr != IntPtr.Zero)
            {
                LocalFree(activeGuidPtr);
            }
        }
    }

    /// <summary>
    /// 获取所有电源方案的 GUID 和名称
    /// </summary>
    public static Dictionary<Guid, string> GetAllPowerSchemes()
    {
        var schemes = new Dictionary<Guid, string>();

        try
        {
            uint bufferSize = 0;

            // 第一次调用获取需要的缓冲区大小
            uint result = PowerEnumerate(
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                ACCESS_SCHEME,
                0,
                null,
                ref bufferSize);

            if (result != ERROR_SUCCESS && result != ERROR_MORE_DATA)
                return schemes;

            // 分配缓冲区并获取所有方案 GUID
            byte[] buffer = new byte[bufferSize];
            result = PowerEnumerate(
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                ACCESS_SCHEME,
                0,
                buffer,
                ref bufferSize);

            if (result != ERROR_SUCCESS)
                return schemes;

            // 解析缓冲区中的 GUID
            int guidCount = (int)(bufferSize / 16); // GUID 是 16 字节
            for (int i = 0; i < guidCount; i++)
            {
                byte[] guidBytes = new byte[16];
                Array.Copy(buffer, i * 16, guidBytes, 0, 16);
                Guid guid = new Guid(guidBytes);

                // 获取方案的友好名称
                string name = GetSchemeFriendlyName(guid);
                schemes[guid] = name;
            }
        }
        catch (Exception ex)
        {
            Log.Err($"枚举电源方案失败: {ex.Message}");
        }

        return schemes;
    }

    /// <summary>
    /// 获取电源方案的友好名称
    /// </summary>
    private static string GetSchemeFriendlyName(Guid schemeGuid)
    {
        try
        {
            uint bufferSize = 0;

            // 第一次调用获取缓冲区大小
            uint result = PowerReadFriendlyName(
                IntPtr.Zero,
                ref schemeGuid,
                IntPtr.Zero,
                IntPtr.Zero,
                null,
                ref bufferSize);

            if (result != ERROR_SUCCESS && bufferSize == 0)
                return "未知方案";

            // 分配缓冲区并获取名称
            byte[] buffer = new byte[bufferSize];
            result = PowerReadFriendlyName(
                IntPtr.Zero,
                ref schemeGuid,
                IntPtr.Zero,
                IntPtr.Zero,
                buffer,
                ref bufferSize);

            if (result == ERROR_SUCCESS)
            {
                string name = System.Text.Encoding.Unicode.GetString(buffer).TrimEnd('\0');
                return name;
            }
        }
        catch { }

        return "未知方案";
    }

    /// <summary>
    /// 检查卓越性能方案是否已存在（通过名称和 GUID 双重验证）
    /// </summary>
    public static bool IsUltimatePerformanceExists()
    {
        var schemes = GetAllPowerSchemes();

        foreach (var scheme in schemes)
        {
            // 检查名称是否为"卓越性能"
            if (scheme.Value.Contains("卓越性能") || scheme.Value.Contains("Ultimate Performance"))
            {
                return true;
            }

            // 检查 GUID 是否为原始的卓越性能 GUID
            if (scheme.Key.ToString().ToUpper() == PowerSchemes.UltimatePerformance.ToString().ToUpper())
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 获取所有卓越性能方案的 GUID（可能有多个副本）
    /// </summary>
    public static List<Guid> GetAllUltimatePerformanceSchemes()
    {
        var schemes = GetAllPowerSchemes();
        var ultimateSchemes = new List<Guid>();

        foreach (var scheme in schemes)
        {
            // 检查名称是否为"卓越性能"
            if (scheme.Value.Contains("卓越性能") || scheme.Value.Contains("Ultimate Performance"))
            {
                ultimateSchemes.Add(scheme.Key);
            }
            //MessageBox.Show(scheme.Value);
        }

        return ultimateSchemes;
    }

    /// <summary>
    /// 清理重复的卓越性能方案（只保留一个）
    /// </summary>
    public static bool CleanupDuplicateUltimatePerformance()
    {
        var ultimateSchemes = GetAllUltimatePerformanceSchemes();

        if (ultimateSchemes.Count <= 1)
            return true; // 没有重复或只有一个

        // 获取当前活动方案
        var activeScheme = GetActiveScheme();

        // 保留一个方案（如果当前活动的是卓越性能，则保留它）
        Guid keepGuid;
        if (activeScheme.HasValue && ultimateSchemes.Contains(activeScheme.Value))
        {
            keepGuid = activeScheme.Value;
            ultimateSchemes.Remove(keepGuid);
        }
        else
        {
            keepGuid = ultimateSchemes[0];
            ultimateSchemes.RemoveAt(0);
        }

        // 删除多余的方案
        bool allDeleted = true;
        foreach (var schemeGuid in ultimateSchemes)
        {
            if (!DeleteScheme(schemeGuid))
            {
                allDeleted = false;
            }
        }

        return allDeleted;
    }

    /// <summary>
    /// 删除指定的电源方案
    /// </summary>
    public static bool DeleteScheme(Guid schemeGuid)
    {
        try
        {
            // 不能删除系统内置的原始 GUID
            if (schemeGuid.ToString().ToUpper() == PowerSchemes.UltimatePerformance.ToString().ToUpper())
            {
                return false; // 保留原始方案
            }

            uint result = PowerDeleteScheme(IntPtr.Zero, ref schemeGuid);
            return result == ERROR_SUCCESS;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 设置活动的电源方案
    /// </summary>
    public static bool SetActiveScheme(Guid schemeGuid)
    {
        try
        {
            // 如果是卓越性能
            //if (schemeGuid == PowerSchemes.UltimatePerformance)
            //    EnableUltimatePerformanceDirect();

            uint result = PowerSetActiveScheme(IntPtr.Zero, ref schemeGuid);
            return result == ERROR_SUCCESS;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"设置电源方案失败: {ex.Message}");
            return false;
        }
    }

    public static bool EnableUltimatePerformanceDirect()
    {
        var UltimatePerformance = PowerSchemes.UltimatePerformance;

        try
        {
            // 直接使用原始 GUID 设置活动方案
            // 即使没有显示，系统也会应用此方案
            uint result = PowerSetActiveScheme(
                IntPtr.Zero,
                ref UltimatePerformance);

            return result == ERROR_SUCCESS;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 确保卓越性能方案已显示
    /// </summary>
    public static bool ShowUltimatePerformanceScheme()
    {
        var _UltimatePerformance = PowerSchemes.UltimatePerformance;

        try
        {
            // 先检查是否已经存在
            //if (IsUltimatePerformanceExists())
            //{
            //    Debug.WriteLine("卓越性能方案已存在，无需重复添加");
            //    return true;
            //}

            // 不存在才添加
            IntPtr destinationPtr;
            uint result = PowerDuplicateScheme(
                IntPtr.Zero,
                ref _UltimatePerformance,
                out destinationPtr);

            if (result == ERROR_SUCCESS)
            {
                Marshal.FreeHGlobal(destinationPtr);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Log.Err($"显示卓越性能方案失败: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 检查是否以管理员身份运行
    /// </summary>
    public static bool IsAdministrator()
    {
        using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
        {
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }

    /// <summary>
    /// 以管理员身份重新启动程序
    /// </summary>
    public static void RestartAsAdmin()
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = Application.ExecutablePath,
            UseShellExecute = true,
            Verb = "runas"
        };

        try
        {
            Process.Start(processInfo);
            Application.Exit();
        }
        catch
        {
            MessageBox.Show("无法以管理员身份启动程序", "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// 获取方案的友好名称（带描述）
    /// </summary>
    public static string GetSchemeDescription(Guid schemeGuid)
    {
        if (schemeGuid == PowerSchemes.UltimatePerformance)
            return "卓越性能 - 提供最高性能，适合游戏、渲染等场景（功耗较高）";
        if (schemeGuid == PowerSchemes.HighPerformance)
            return "高性能 - 优先性能，适合日常办公、开发等场景";
        if (schemeGuid == PowerSchemes.Balanced)
            return "平衡 - 性能和功耗平衡，适合大多数场景（系统推荐）";
        if (schemeGuid == PowerSchemes.PowerSaver)
            return "节能 - 优先省电，适合移动办公、续航优先场景";

        return "未知方案";
    }
}