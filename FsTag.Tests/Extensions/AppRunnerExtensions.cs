using CommandDotNet;
using CommandDotNet.TestTools.Scenarios;

namespace FsTag.Tests.Extensions;

public static class AppRunnerExtensions
{
    public static void VerifyExitCode(this AppRunner runner, int code, params string[] args)
    {
        runner.Verify(new Scenario
        {
            When =
            {
                ArgsArray = args
            },
            Then =
            {
                ExitCode = code
            }
        });
    }
}