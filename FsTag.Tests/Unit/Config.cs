using CommandDotNet.TestTools.Scenarios;

using FsTag.Data;
using FsTag.Tests.Extensions;
using FsTag.Tests.Unit.Mocks;

using Newtonsoft.Json.Linq;

namespace FsTag.Tests.Unit;

[TestFixture]
public class Config : UnitTestBase
{
    private Configuration Configuration => App.ConfigData.Read()!;

    [SetUp]
    public void ConfigSetUp()
    {
        MockConfig.SetProperty("a", JToken.FromObject("b"));
        MockConfig.SetProperty("c", JToken.FromObject(1));
    }
    
    [Test]
    public void TestGetCustomConfig()
    {
        Program.Runner.VerifyExitCode(0, "config", "a");
    }
    
    [Test]
    public void TestSetCustomConfig()
    {
        Program.Runner.VerifyExitCode(0, "config", "set", "c", "5");

        var config = MockConfig.Read();
        
        Assert.That(config.OtherProperties, Contains.Key("c"));
        Assert.That(config.OtherProperties["c"]!.Value<int>(), Is.EqualTo(5));
    }

    [Test]
    public void TestSetBuiltin()
    {
        Program.Runner.VerifyExitCode(0, "config", "set", "session_name", "\"hello world\"");
        Assert.That(Configuration.SessionName, Is.EqualTo("hello world"));
    }

    [Test]
    public void TestSetBuiltinInvalid()
    {
        var initialValue = Configuration.FormatJsonOutput;
        
        Program.Runner.VerifyExitCode(1, "config", "set", "format_json_output", "{}");
        Assert.That(Configuration.FormatJsonOutput, Is.EqualTo(initialValue));
    }
}