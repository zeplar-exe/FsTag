using System.IO.Abstractions.TestingHelpers;

using FsTag.Data;
using FsTag.Tests.Extensions;
using FsTag.Tests.Unit.Mocks;

namespace FsTag.Tests.Unit;

[TestFixture]
public class Remove : UnitTestBase
{
    private const string TestFileName = "C:/test/test1.txt";
    
    [SetUp]
    public void RemoveSetUp()
    {
        App.FileIndex = new MockIndex();

        var mockFileSystem = new MockFileSystem();
        App.FileSystem = mockFileSystem;
        
        mockFileSystem.AddFile(TestFileName, new MockFileData(""));
    }
    
    [Test]
    public void TestRemove()
    {
        App.FileIndex.Add(new[] { TestFileName, "test2", "test3" });
        
        Program.Runner.RunAndAssertExitCode(0, "rm", TestFileName);

        var items = App.FileIndex.EnumerateItems().ToArray();
        
        Assert.That(items, Has.Length.EqualTo(2));
        Assert.That(items, Does.Not.Contain(TestFileName));
        Assert.That(items, Does.Contain("test2"));
        Assert.That(items, Does.Contain("test3"));
    }

    [Test]
    public void TestRemoveAll()
    {
        App.FileIndex.Add(new[] { "test1", "test2", "test3" });
        
        Program.Runner.RunAndAssertExitCode(0, "rm", "all");

        var items = App.FileIndex.EnumerateItems().ToArray();
        
        Assert.That(items, Has.Length.EqualTo(0));
    }
}