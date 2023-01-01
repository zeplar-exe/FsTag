using System.Diagnostics;
using System.Text;

namespace FsTag.Tests;

public static class Executable
{
    private static Process CurrentProcess => Process.GetCurrentProcess();
    private static StringBuilder Output { get; set; } = new();

    public static void Setup()
    {
        CurrentProcess.OutputDataReceived += OnOutput;
        CurrentProcess.ErrorDataReceived += OnError;
    }

    public static void Teardown()
    {
        CurrentProcess.OutputDataReceived -= OnOutput;
        CurrentProcess.ErrorDataReceived -= OnError;
    }
    
    public static void AssertExitCode(int expected, params string[] args)
    {
        var result = RunWithArgs(args);
        
        Assert.That(result.ExitCode, Is.EqualTo(expected));
    }
    
    public static CommandResult RunWithArgs(params string[] args)
    {
        var code = Program.Main(args);

        Output.Clear();
        
        return new CommandResult(Output.ToString(), code);
    }
    
    private static void OnOutput(object sender, DataReceivedEventArgs data)
    {
        Output.AppendLine(data.Data);
    }

    private static void OnError(object sender, DataReceivedEventArgs data)
    { 
        Output.AppendLine(data.Data);
    }
}