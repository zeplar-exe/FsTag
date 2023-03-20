using CommandDotNet.TestTools.Scenarios;

using FsTag.Data;
using FsTag.Tests.Extensions;
using FsTag.Tests.Unit.Mocks;

using Newtonsoft.Json.Linq;

namespace FsTag.Tests.Unit;

[TestFixture]
public class Config : UnitTestBase
{
    private MockConfig MockConfig { get; set; }

    [SetUp]
    public void ConfigSetUp()
    {
        MockConfig = new MockConfig();
        App.ConfigData = MockConfig;
        
        MockConfig.SetProperty("a", JToken.FromObject("b"));
        MockConfig.SetProperty("c", JToken.FromObject(1));
    }
    
    [Test]
    public void TestGetConfig()
    {
        Program.Runner.Verify(new Scenario
        {
            When =
            {
                ArgsArray = new[] { "config", "a" }
            },
            Then =
            {
                Output = "\"b\"",
                ExitCode = 0
            }
        });
    }
    
    [Test]
    public void TestSetConfig()
    {
        Program.Runner.Verify(new Scenario
        {
            When =
            {
                ArgsArray = new[] { "config", "set", "c", "5" }
            },
            Then =
            {
                Output = "c=5",
                ExitCode = 0
            }
        });

        var config = MockConfig.Read();
        
        Assert.That(config.OtherProperties, Contains.Key("c"));
        Assert.That(config.OtherProperties["c"]!.Value<int>(), Is.EqualTo(5));
    }

    [Test]
    public void TestSetBuiltin()
    {
        Program.Runner.VerifyExitCode(0, "config", "set", "session_name", "\"hello world\"");
    }

    [Test]
    public void TestSetBuiltinInvalid()
    {
        Program.Runner.VerifyExitCode(1, "config", "set", "format_json_output", "{}");
    }
}