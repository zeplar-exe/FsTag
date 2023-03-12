using System.IO.Abstractions.TestingHelpers;

using CommandDotNet.TestTools;

using FsTag.Data;
using FsTag.Tests.Unit.Mocks;

namespace FsTag.Tests.Unit;

public class UnitTestBase
{
    public static TestConsole TestConsole { get; set; }
    public static MockFileSystem MockFileSystem { get; set; }

    [SetUp]
    public void SetUp()
    {
        TestConsole = new TestConsole();
        Program.IConsole = TestConsole;
        
        App.FileIndex = new MockIndex();

        MockFileSystem = new MockFileSystem();
        App.FileSystem = MockFileSystem;
        
        MockFileSystem.AddEmptyFile("C:/test1.txt");
        MockFileSystem.AddEmptyFile("C:/test2.txt");
        MockFileSystem.AddEmptyFile("C:/test3.txt");
        MockFileSystem.AddEmptyFile("C:/dir/re1.txt");
        MockFileSystem.AddEmptyFile("C:/dir/re2.txt");
        MockFileSystem.AddEmptyFile("C:/dir/dir2/re11.txt");
        MockFileSystem.AddEmptyFile("C:/dir/dir2/re12.txt");
    }
}