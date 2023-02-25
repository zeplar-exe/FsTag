using CommandDotNet.TestTools.Scenarios;

namespace FsTag.Tests.CommandTests;

[TestFixture]
public class Tag : TestBase
{
    const string TestFileName = "test/test1.txt";
    
    [Test]
    public void TestTag()
    {
        Program.Runner.Verify(
            new Scenario
            {
                When = { ArgsArray = new[] { "tag", TestFileName } },
                Then =
                {
                    ExitCode = 0,
                    OutputContainsTexts = new List<string> { TestFileName }
                }
            });

        Program.Runner.Verify(
            new Scenario
            {
                When = { ArgsArray = new[] { "print", "index" } },
                Then =
                {
                    ExitCode = 0,
                    OutputContainsTexts = new List<string> { TestFileName }
                }
            });
    }
    
    [Test]
    public void TestTagAsDefaultCommand()
    {
        Program.Runner.Verify(
            new Scenario
            {
                When = { ArgsArray = new[] { TestFileName } },
                Then =
                {
                    ExitCode = 0,
                    OutputContainsTexts = new List<string> { TestFileName }
                }
            });
    }
}