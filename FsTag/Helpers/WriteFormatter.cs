namespace FsTag.Helpers;

public static class WriteFormatter
{
    public static void Plain(string text)
    {
        Console.WriteLine(text);
    }
    
    public static void PlainNoLine(string text)
    {
        Console.Write(text);
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
        var originalForeground = Console.ForegroundColor;
        
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = originalForeground;
    }
    
    public static void NewLine()
    {
        Console.WriteLine();
    }
}