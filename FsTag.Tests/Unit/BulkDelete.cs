using FsTag.Tests.Extensions;

using NUnit.Framework;

namespace FsTag.Tests.Unit;

[TestFixture]
public class BulkDelete : UnitTestBase
{
    [Test]
    public void TestBulkDelete()
    {
        MockFileIndex.Add(new[] { "C:/test1.txt" });
        
        Program.Runner.VerifyExitCode(0, "--no-prompt", "bulk", "delete");
        
        Assert.That(MockFileIndex.EnumerateItems().ToArray(), Has.Length.EqualTo(0));
    }
}