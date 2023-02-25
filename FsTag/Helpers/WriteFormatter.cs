using CommandDotNet;

namespace FsTag.Helpers;

public static class WriteFormatter
{
    public static void Plain(string text)
    {
        if (!Program.Quiet)
            Program.IConsole.WriteLine(text);
    }
    
    public static void PlainNoLine(string text)
    {
        if (!Program.Quiet)
            Program.IConsole.Write(text);
    }
    
    public static void Info(string text)
    {
        WriteWithColor($"Info: {text}", ConsoleColor.White);
    }
    
    public static void Warning(string text)
    {
        WriteWithColor($"Warning: {text}", ConsoleColor.Yellow);
    }
    
    public static void Error(string text)
    {
        WriteWithColor($"Error: {text}", ConsoleColor.Red);
    }

    private static void WriteWithColor(string text, ConsoleColor color)
    {
        if (Program.Quiet)
            return;
        
        var originalForeground = Console.ForegroundColor;
        
        Program.IConsole.ForegroundColor = color;
        Program.IConsole.WriteLine(text);
        Program.IConsole.ForegroundColor = originalForeground;
    }
    
    /// <summary>
    /// Writes `count` empty lines to the console.
    /// </summary>
    public static void NewLine(uint count = 1)
    {
        if (Program.Quiet)
            return;

        for (var i = 0; i < count; i++)
            Program.IConsole.WriteLine();
    }
}