namespace FsTag.Tests;

public class BasicTests : TestBase
{
    [Test]
    public void TestPrintExit()
    {
        Executable.AssertExitCode(0, "print");
    }
}