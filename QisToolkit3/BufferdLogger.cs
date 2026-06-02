using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Qi;

public static class BufferedLogger
{
    // ========== 配置参数 ==========

    // 日志文件路径
    private static readonly string _logFilePath;

    // 缓冲区：存储待写入的日志
    private static readonly List<string> _logBuffer = new List<string>();

    // 缓冲区最大条数（达到此数量时立即刷新）
    private static readonly int _bufferSize = 200;

    // 定时刷新间隔（毫秒）
    private static readonly int _flushInterval = 5000;

    // 最小刷新间隔（防止频繁刷新）
    private static readonly int _minFlushInterval = 1000;

    // 同步锁，确保线程安全
    private static readonly object _lock = new object();

    // 定时器，用于定期刷新
    private static System.Threading.Timer _flushTimer;

    // 最后一次刷新时间
    private static DateTime _lastFlushTime = DateTime.MinValue;

    // 日志级别
    public enum LogLevel
    {
        DEBUG,
        INFO,
        WARNING,
        ERROR,
        FATAL
    }

    // 当前日志级别（只记录此级别及以上的日志）
    private static LogLevel _currentLevel = LogLevel.INFO;

    // ========== 静态构造函数 ==========
    static BufferedLogger()
    {
        try
        {
            // 设置日志文件路径
            string logsDirectory = Path.Combine(QisToolkit3_Datas.actualDirectory, "Logs");
            Directory.CreateDirectory(logsDirectory);

            // 按日期分割日志文件：Main_年月日_时分.log
            string dateStr = DateTime.Now.ToString("yyyyMMdd_HHmm");
            _logFilePath = Path.Combine(logsDirectory, $"Main_{dateStr}.log");

            // 文件存在就多换几行
            if (File.Exists(_logFilePath)) 
                File.AppendAllText(_logFilePath, $"\n\n===================={DateTime.Now.ToString("HHmmss")}====================\n\n\n");

            // 启动定时刷新器
            _flushTimer = new System.Threading.Timer(FlushBufferTimerCallback, null,
                _flushInterval,  // 首次执行延迟
                _flushInterval); // 执行间隔

            // 写入启动日志
            LogInternal(LogLevel.INFO, "=== 日志记录器初始化 ===");
            LogInternal(LogLevel.INFO, $"日志文件: {_logFilePath}");
            LogInternal(LogLevel.INFO, $"缓冲区大小: {_bufferSize}, 刷新间隔: {_flushInterval}ms");

            //Console.WriteLine($"已初始化日志记录器。日志文件: {_logFilePath}");
        }
        catch (Exception ex)
        {
            //Console.WriteLine($"无法初始化日志记录器: {ex.Message}");
            throw;
        }
    }

    // ========== 公共日志方法 ==========

    /// <summary>
    /// 调试日志（只在DEBUG模式记录）
    /// </summary>
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Debug(string message)
    {
        LogInternal(LogLevel.DEBUG, message);
    }

    /// <summary>
    /// 普通信息日志
    /// </summary>
    public static void Info(string message)
    {
        LogInternal(LogLevel.INFO, message);
    }

    /// <summary>
    /// 警告日志
    /// </summary>
    public static void Warning(string message)
    {
        LogInternal(LogLevel.WARNING, message);
    }

    /// <summary>
    /// 错误日志（带异常信息）
    /// </summary>
    public static void Error(string message, Exception ex = null)
    {
        string fullMessage = message;
        if (ex != null)
        {
            fullMessage += $"\n  异常: {ex.Message}\n  堆栈跟踪: {ex.StackTrace}";
        }
        LogInternal(LogLevel.ERROR, fullMessage);
    }

    /// <summary>
    /// 致命错误日志（立即刷新）
    /// </summary>
    public static void Fatal(string message, Exception ex = null)
    {
        string fullMessage = $"致命错误: {message}";
        if (ex != null)
        {
            fullMessage += $"\n  异常: {ex.Message}\n  堆栈跟踪: {ex.StackTrace}";
        }

        // 立即写入，不等待缓冲区
        WriteImmediate(LogLevel.FATAL, fullMessage);
    }

    // ========== 内部核心方法 ==========

    /// <summary>
    /// 内部日志记录方法
    /// </summary>
    private static void LogInternal(LogLevel level, string message)
    {
        // 检查日志级别
        if (level < _currentLevel) return;

        string logEntry = FormatLogEntry(level, message);

        lock (_lock)
        {
            // 添加到缓冲区
            _logBuffer.Add(logEntry);

            // 如果缓冲区满了，立即刷新
            if (_logBuffer.Count >= _bufferSize)
            {
                FlushBuffer();
            }
        }
    }

    /// <summary>
    /// 格式化日志条目
    /// </summary>
    private static string FormatLogEntry(LogLevel level, string message)
    {
        return $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level.ToString()}] {message}";
    }

    /// <summary>
    /// 定时器回调：定期刷新缓冲区
    /// </summary>
    private static void FlushBufferTimerCallback(object state)
    {
        // 防止频繁刷新（至少间隔1秒）
        if ((DateTime.Now - _lastFlushTime).TotalMilliseconds < _minFlushInterval)
            return;

        FlushBuffer();
    }

    /// <summary>
    /// 刷新缓冲区到文件
    /// </summary>
    public static void FlushBuffer()
    {
        List<string> bufferToWrite = null;

        // 1. 从缓冲区取出数据
        lock (_lock)
        {
            if (_logBuffer.Count == 0) return;

            bufferToWrite = new List<string>(_logBuffer);
            _logBuffer.Clear();
        }

        // 2. 异步写入文件（不阻塞调用线程）
        Task.Run(() => WriteToFile(bufferToWrite));

        _lastFlushTime = DateTime.Now;
    }

    /// <summary>
    /// 立即写入文件（不经过缓冲区）
    /// </summary>
    private static void WriteImmediate(LogLevel level, string message)
    {
        string logEntry = FormatLogEntry(level, message);

        Task.Run(() =>
        {
            try
            {
                using (var writer = new StreamWriter(_logFilePath, true, Encoding.UTF8, 4096))
                {
                    writer.WriteLine(logEntry);
                    writer.Flush(); // 确保立即写入磁盘
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"无法写入即时日志: {ex.Message}");
            }
        });
    }

    /// <summary>
    /// 批量写入文件
    /// </summary>
    private static void WriteToFile(List<string> logEntries)
    {
        if (logEntries == null || logEntries.Count == 0) return;

        try
        {
            // 使用StreamWriter的缓冲区，提高写入性能
            using (var writer = new StreamWriter(_logFilePath, true, Encoding.UTF8, 65536)) // 64KB缓冲区
            {
                foreach (var entry in logEntries)
                {
                    writer.WriteLine(entry);
                }

                // 重要：批量写入后刷新
                writer.Flush();
            }

            // 可选：记录本次写入统计
            //Console.WriteLine($"[日志记录器] 已将 {logEntries.Count} 条日志刷新到文件中");
        }
        catch (Exception ex)
        {
            // 写入失败，尝试将日志重新放回缓冲区
            //Console.WriteLine($"无法将日志写入文件: {ex.Message}");

            lock (_lock)
            {
                // 将失败的日志重新放回缓冲区（放在最前面）
                _logBuffer.InsertRange(0, logEntries);

                // 如果缓冲区过大，丢弃一些旧日志
                if (_logBuffer.Count > _bufferSize * 2)
                {
                    int toRemove = _logBuffer.Count - _bufferSize;
                    _logBuffer.RemoveRange(_bufferSize, toRemove);
                    //Console.WriteLine($"[日志记录器] 缓冲区溢出，已删除 {toRemove} 条旧日志");
                }
            }
        }
    }

    // ========== 管理方法 ==========

    /// <summary>
    /// 设置日志级别
    /// </summary>
    public static void SetLogLevel(LogLevel level)
    {
        _currentLevel = level;
        Info($"日志级别已更改为: {level}");
    }

    /// <summary>
    /// 获取当前缓冲区中的日志数量
    /// </summary>
    public static int GetBufferCount()
    {
        lock (_lock)
        {
            return _logBuffer.Count;
        }
    }

    /// <summary>
    /// 获取当前日志文件大小（字节）
    /// </summary>
    public static long GetLogFileSize()
    {
        try
        {
            if (File.Exists(_logFilePath))
            {
                return new FileInfo(_logFilePath).Length;
            }
            return 0;
        }
        catch
        {
            return -1;
        }
    }

    /// <summary>
    /// 程序退出时调用，确保所有日志都已写入
    /// </summary>
    public static void Shutdown()
    {
        try
        {
            // 1. 停止定时器
            _flushTimer?.Dispose();
            _flushTimer = null;

            // 2. 立即刷新所有剩余日志
            FlushBuffer();

            // 3. 等待所有写入完成（最多等3秒）
            Thread.Sleep(100);

            // 4. 写入关闭日志
            WriteImmediate(LogLevel.INFO, "=== 日志记录器关闭 ===");

            //Console.WriteLine("日志记录器已成功关闭");
        }
        catch (Exception ex)
        {
            //Console.WriteLine($"日志记录器关闭期间发生错误: {ex.Message}");
        }
    }
}

public static class Log
{
    public static void I(string message) => BufferedLogger.Info(message);
    public static void W(string message) => BufferedLogger.Warning(message);
    public static void E(string message, Exception ex = null) => BufferedLogger.Error(message, ex);
    public static void D(string message) => BufferedLogger.Debug(message);
    public static void F(string message, Exception ex = null) => BufferedLogger.Fatal(message, ex);

    // 完整单词
    public static void Info(string message) => BufferedLogger.Info(message);
    public static void Warn(string message) => BufferedLogger.Warning(message);
    public static void Err(string message, Exception ex = null) => BufferedLogger.Error(message, ex);
    public static void Debug(string message) => BufferedLogger.Debug(message);
    public static void Fatal(string message, Exception ex = null) => BufferedLogger.Fatal(message, ex);
}