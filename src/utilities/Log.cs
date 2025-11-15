using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

public static class Log
{
    private static void Construct(string level, string message, string? callingClass = null)
    {
        string timestamp = DateTime.Now.ToString("yyy-MM-dd HH:mm:ss.fff");

        if (callingClass == null)
        {
            var stackTrace = new StackTrace();
            var callingMethod = stackTrace.GetFrame(2)?.GetMethod();
            callingClass = callingMethod?.DeclaringType?.Name;
        }

        string entry = $"[{timestamp}] {level}: {callingClass}: {message}";
        Console.WriteLine(entry);
    }

    public static void Debug(string message)
    {
        Construct("DEBUG", message);
    }
    public static void Info(string message)
    {
        Construct("INFO", message);
    }

    public static WarningException Warning(string message)
    {
        Construct("WARN", message);
        return new WarningException(message);

    }

    public static Exception Error(string message)
    {
        Construct("ERROR", message);
        return new Exception(message);
    }

    public static Exception Critical(string message)
    {
        Construct("CRITICAL", message);
        return new Exception(message);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static unsafe void RaylibLogCallback(int logLevel, sbyte* text, sbyte* args)
    {
        string msg = Marshal.PtrToStringAnsi((IntPtr)text) ?? "";
        string more = Marshal.PtrToStringAnsi((IntPtr)args) ?? "";
        Construct(RaylibLogLevel(logLevel), msg, "Raylib");
    }

    static string RaylibLogLevel(int level) => level switch
    {
        1 => "TRACE",
        2 => "DEBUG",
        3 => "INFO",
        4 => "WARNING",
        5 => "ERROR",
        6 => "FATAL",
        _ => $"LEVEL({level})"
    };

}