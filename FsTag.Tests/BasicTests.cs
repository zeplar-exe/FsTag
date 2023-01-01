namespace FsTag.Tests;

public class BasicTests : TestBase
{
    [Test]
    public void TestBase()
    {
        Executable.AssertExitCode(1); // No params is an "error": Code 1
    }
    
    [Test]
    public void TestPrintExit()
    {
        Executable.AssertExitCode(0, "print");
    }
}