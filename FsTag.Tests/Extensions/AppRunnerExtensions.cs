using CommandDotNet;

using FsTag.Data;

namespace FsTag.Tests.Extensions;

public static class AppRunnerExtensions
{
    public static void RunAndAssertExitCode(this AppRunner runner, int code, params string[] args)
    {
        var result = runner.Run(args);
        
        Assert.That(result, Is.EqualTo(code), $"Expected exit code of {code}, got {result}.");
    }
}