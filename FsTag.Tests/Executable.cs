namespace FsTag.Tests;

public static class Executable
{
    private static TextWriter? OriginalWriter { get; set; }
    private static MemoryStream Output { get; set; }
    private static StreamWriter? Writer { get; set; }
    
    public static bool DisplayCommandOutput { get; set; }

    public static void Setup()
    { 
        // No teardown cause all of these have to use the same console stream, causing disposal problems
        
        Output = new MemoryStream();
        OriginalWriter = Console.Out;
        
        Writer = new StreamWriter(Output)
        {
            AutoFlush = true
        };
        Console.SetOut(Writer);
        Console.SetError(Writer);
    }
    
    public static CommandResult AssertExitCode(int expected, params string[] args)
    {
        var result = RunWithArgs(args);
        
        Assert.That(result.ExitCode, Is.EqualTo(expected));

        return result;
    }
    
    public static CommandResult RunWithArgs(params string[] args)
    {
        var code = Program.MainMethod(args);
        var reader = new StreamReader(Output);

        Output.Seek(0, SeekOrigin.Begin);
        var outputText = reader.ReadToEnd();
        
        var result = new CommandResult(outputText, code);
        
        if (DisplayCommandOutput)
            OriginalWriter?.Write(outputText);
        
        Output.SetLength(0); // Clear as to isolate the next output

        return result;
    }
}