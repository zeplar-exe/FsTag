using System.IO.Abstractions.TestingHelpers;

using FsTag.Data;
using FsTag.Tests.Extensions;
using FsTag.Tests.Unit.Mocks;

namespace FsTag.Tests.Unit;

[TestFixture]
public class Tag : UnitTestBase
{
    [Test]
    public void TestTag()
    {
        AssertIndexCountAfterRun(1, "tag", "C:/test1.txt");
    }
    
    [Test]
    public void TestTagAsDefaultCommand()
    {
        AssertIndexCountAfterRun(1, "C:/test1.txt");
    }

    private void AssertIndexCountAfterRun(int count, params string[] args)
    {
        Program.Runner.RunAndAssertExitCode(0, args);
        
        Assert.That(App.FileIndex.EnumerateItems().ToArray(), Has.Length.EqualTo(count));
    }
}