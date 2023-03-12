using System.IO.Abstractions.TestingHelpers;

using FsTag.Data;
using FsTag.Tests.Extensions;
using FsTag.Tests.Unit.Mocks;

namespace FsTag.Tests.Unit;

[TestFixture]
public class Remove : UnitTestBase
{
    [Test]
    public void TestRemove()
    {
        App.FileIndex.Add(new[] { "C:/test1.txt", "C:/test2.txt", "C:/test3.txt" });
        
        Program.Runner.RunAndAssertExitCode(0, "rm", "C:/test1.txt");

        var items = App.FileIndex.EnumerateItems().ToArray();
        
        Assert.That(items, Has.Length.EqualTo(2));
        Assert.That(items, Does.Not.Contain("C:/test1.txt"));
        Assert.That(items, Does.Contain("C:/test2.txt"));
        Assert.That(items, Does.Contain("C:/test3.txt"));
    }

    [Test]
    public void TestRemoveAll()
    {
        App.FileIndex.Add(new[] { "C:/test1.txt", "C:/test2.txt", "C:/test3.txt" });
        
        Program.Runner.RunAndAssertExitCode(0, "rm", "all");

        var items = App.FileIndex.EnumerateItems().ToArray();
        
        Assert.That(items, Has.Length.EqualTo(0));
    }
}