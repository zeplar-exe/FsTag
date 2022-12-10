namespace FsTag;

public static class WriteFormatter
{
    public static void Info(string text)
    {
        var originalForeground = Console.ForegroundColor;
        
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Info: {text}");
        Console.ForegroundColor = originalForeground;
    }
    
    public static void Warning(string text)
    {
        var originalForeground = Console.ForegroundColor;
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Warning: {text}");
        Console.ForegroundColor = originalForeground;
    }
    
    public static void Error(string text)
    {
        var originalForeground = Console.ForegroundColor;
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {text}");
        Console.ForegroundColor = originalForeground;
    }
}