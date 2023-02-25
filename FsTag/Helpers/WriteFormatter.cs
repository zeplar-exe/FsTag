using CommandDotNet;

namespace FsTag.Helpers;

public static class WriteFormatter
{
    public static void Plain(string text)
    {
        if (!Program.Quiet)
            Program.Console.WriteLine(text);
    }
    
    public static void PlainNoLine(string text)
    {
        if (!Program.Quiet)
            Program.Console.Write(text);
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
        
        Program.Console.ForegroundColor = color;
        Program.Console.WriteLine(text);
        Program.Console.ForegroundColor = originalForeground;
    }
    
    public static void NewLine()
    {
        if (Program.Quiet)
            return;
        
        Program.Console.WriteLine();
    }
}