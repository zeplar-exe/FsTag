using System.IO.Abstractions.TestingHelpers;

using FsTag.Data;
using FsTag.Tests.Extensions;
using FsTag.Tests.Unit.Mocks;

namespace FsTag.Tests.Unit;

[TestFixture]
public class Tag : UnitTestBase
{
    private const string TestFileName = "C:/test/test1.txt";

    [SetUp]
    public void TagSetup()
    {
        App.FileIndex = new MockIndex();

        var mockFileSystem = new MockFileSystem();
        App.FileSystem = mockFileSystem;
        
        mockFileSystem.AddFile(TestFileName, new MockFileData(""));
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