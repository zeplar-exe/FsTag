using System.IO.Abstractions.TestingHelpers;

using CommandDotNet.TestTools;

using FsTag.Data;
using FsTag.Data.Builtin;
using FsTag.Tests.Unit.Mocks;

namespace FsTag.Tests.Unit;

public class UnitTestBase
{
    public static TestConsole TestConsole { get; set; }
    public static MockFileIndex MockFileIndex { get; set; }
    public static MockFileSystem MockFileSystem { get; set; }
    public static MockConfig MockConfig { get; set; }
    public static MockSessionData MockSessionData { get; set; }

    [SetUp]
    public void SetUp()
    {
        TestConsole = new TestConsole();
        Program.IConsole = TestConsole;

        Program.Runner.Configure(c => c.Console = TestConsole);
        
        App.FileIndex = MockFileIndex = new MockFileIndex();
        App.ConfigData = MockConfig = new MockConfig();
        App.FileSystem = MockFileSystem = new MockFileSystem();
        App.SessionData = MockSessionData = new MockSessionData();
        
        MockFileSystem.AddEmptyFile("C:/test1.txt");
        MockFileSystem.AddEmptyFile("C:/test2.txt");
        MockFileSystem.AddEmptyFile("C:/test3.txt");
        MockFileSystem.AddEmptyFile("C:/dir/re1.txt");
        MockFileSystem.AddEmptyFile("C:/dir/re2.txt");
        MockFileSystem.AddEmptyFile("C:/dir/dir2/re11.txt");
        MockFileSystem.AddEmptyFile("C:/dir/dir2/re12.txt");
    }
}