using CommandDotNet.TestTools.Scenarios;

using FsTag.Data;
using FsTag.Data.Interfaces;
using FsTag.Tests.Extensions;
using FsTag.Tests.Integration;
using FsTag.Tests.Unit.Mocks;

namespace FsTag.Tests.Unit;

[TestFixture]
public class Tag : UnitTestBase
{
    private const string TestFileName = "test/test1.txt";

    [SetUp]
    public void TagSetup()
    {
        App.FileIndex = new MockIndex();
    }
    
    [Test]
    public void TestTag()
    {
        AssertIndexCountAfterRun(1, "tag", TestFileName);
    }
    
    [Test]
    public void TestTagAsDefaultCommand()
    {
        AssertIndexCountAfterRun(1, TestFileName);
    }

    private void AssertIndexCountAfterRun(int count, params string[] args)
    {
        Program.Runner.RunAndAssertExitCode(0, args);
        
        Assert.That(App.FileIndex.EnumerateItems().ToArray(), Has.Length.EqualTo(count));
    }
}