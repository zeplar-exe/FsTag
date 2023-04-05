using System.IO.Abstractions;

using CommandDotNet.TestTools;

using FsTag.Data;
using FsTag.Data.Builtin;

namespace FsTag.Tests.Integration;

public class IntegrationTestBase
{
    public static TestConsole TestConsole { get; set; }
    
    [SetUp]
    public void SetUp()
    {
        BuiltinPaths.UseIntegrationRoot();

        TestConsole = new TestConsole();
        Program.IConsole = TestConsole;
        
        App.FileIndex = new FileIndex();
        App.ConfigData = new ConfigData();
        App.FileSystem = new FileSystem();
        App.SessionData = new SessionData();
    }

    [TearDown]
    public void TearDown()
    {
        Directory.Delete(BuiltinPaths.IntegrationDirectoryRoot);
    }
}