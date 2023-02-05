namespace FsTag.Tests;

[TestFixture]
public class TagTests : TestBase
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
    public void TestTagWithoutVerb()
    {
        Executable.DisplayCommandOutput = true;
        
        Executable.AssertExitCode(0, TestFileName);
        var output = Executable.AssertExitCode(0, "print");
        
        Assert.That(output.Output, Contains.Substring(TestFileName));
    }
}