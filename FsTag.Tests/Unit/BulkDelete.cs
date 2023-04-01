using FsTag.Tests.Extensions;

using NUnit.Framework;

namespace FsTag.Tests.Unit;

[TestFixture]
public class BulkDelete : UnitTestBase
{
    [Test]
    public void TestBulkDelete()
    {
        Program.Runner.VerifyExitCode(0, "tag", "--no-prompt", "-r", "-1", "glob", "*");
        Program.Runner.VerifyExitCode(0, "bulk", "delete");
        
        Assert.That(MockFileIndex.EnumerateItems().ToArray(), Has.Length.EqualTo(0));
    }
}