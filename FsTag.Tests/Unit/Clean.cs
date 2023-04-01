using FsTag.Tests.Extensions;

using NUnit.Framework;

namespace FsTag.Tests.Unit;

[TestFixture]
public class Clean : UnitTestBase
{
    [Test]
    public void TestClean()
    {
        Program.Runner.VerifyExitCode(0, "clean");
        
        Assert.That(MockFileIndex.Cleaned, Is.True);
    }
}