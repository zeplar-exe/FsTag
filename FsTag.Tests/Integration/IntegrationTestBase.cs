using CommandDotNet.TestTools;

using FsTag.Data.Builtin;

namespace FsTag.Tests.Integration;

public class IntegrationTestBase
{
    public static TestConsole TestConsole { get; set; }
    
    [SetUp]
    public void SetUp()
    {
        BuiltinPaths.UseIntegrationRoot();

        TestConsole = new TestConsole();
        Program.IConsole = TestConsole;
    }

    [TearDown]
    public void TearDown()
    {
        Directory.Delete(BuiltinPaths.IntegrationRoot);
    }
}