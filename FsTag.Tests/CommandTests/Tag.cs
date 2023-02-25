namespace FsTag.Tests.CommandTests;

[TestFixture]
public class Tag : TestBase
{
    const string TestFileName = "test/test1.txt";
    
    [Test]
    public void TestTag()
    {
        Executable.AssertExitCode(0, "tag", TestFileName);
        var output = Executable.AssertExitCode(0, "print", "index");
        
        Assert.That(output.Output, Contains.Substring(TestFileName));
    }
    
    [Test]
    public void TestTagDefaultCommand()
    {
        Executable.AssertExitCode(0, TestFileName);
        var output = Executable.AssertExitCode(0, "print");
        
        Assert.That(output.Output, Contains.Substring(TestFileName));
    }
}