namespace FsTag.Tests.CommandTests;

[TestFixture]
public class Tag : TestBase
{
    const string TestFileName = "test/test1.txt";
    
    [Test]
    public void TestTag()
    {
        Executable.DisplayCommandOutput = true;
        
        Executable.AssertExitCode(0, "tag", TestFileName);
        var output = Executable.AssertExitCode(0, "print");
        
        Assert.That(output.Output, Contains.Substring(TestFileName));
    }
    
    [Test]
    public void TestTagDefaultCommand()
    {
        Executable.DisplayCommandOutput = true;
        
        Executable.AssertExitCode(0, TestFileName);
        var output = Executable.AssertExitCode(0, "print");
        
        Assert.That(output.Output, Contains.Substring(TestFileName));
    }
}