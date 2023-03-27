using CommandDotNet;
using CommandDotNet.TestTools;
using CommandDotNet.TestTools.Scenarios;

namespace FsTag.Tests.Extensions;

public static class AppRunnerExtensions
{
    public static void VerifyExitCode(this AppRunner runner, int code, params string[] args)
    {
        var result = runner.RunInMem(args);

        Assert.That(result.ExitCode, Is.EqualTo(code));
    }
}