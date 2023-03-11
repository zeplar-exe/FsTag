using CommandDotNet.TestTools;

namespace FsTag.Tests.Unit;

public class UnitTestBase
{
    public static TestConsole TestConsole { get; set; }

    [SetUp]
    public void SetUp()
    {
        TestConsole = new TestConsole();
        Program.IConsole = TestConsole;
    }
}