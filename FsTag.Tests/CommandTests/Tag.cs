using CommandDotNet;
using CommandDotNet.TestTools.Scenarios;

using FsTag.Data;
using FsTag.Data.Interfaces;

namespace FsTag.Tests.CommandTests;

[TestFixture]
public class Tag
{
    const string TestFileName = "test/test1.txt";

    [SetUp]
    public void TagSetup()
    {
        AppData.FileIndex = new MockIndex();
        AppData.FileSystem = new MockFileSystem();
    }
    
    [Test]
    public void TestTag()
    {
        Program.Runner.Verify(
            new Scenario
            {
                When = { ArgsArray = new[] { "tag", TestFileName } },
                Then =
                {
                    ExitCode = 0,
                    OutputContainsTexts = new List<string> { TestFileName }
                }
            });

        Program.Runner.Verify(
            new Scenario
            {
                When = { ArgsArray = new[] { "print", "index" } },
                Then =
                {
                    ExitCode = 0,
                    OutputContainsTexts = new List<string> { TestFileName }
                }
            });
    }
    
    [Test]
    public void TestTagAsDefaultCommand()
    {
        Program.Runner.Verify(
            new Scenario
            {
                When = { ArgsArray = new[] { TestFileName } },
                Then =
                {
                    ExitCode = 0,
                    OutputContainsTexts = new List<string> { TestFileName }
                }
            });
    }
    
    private class MockIndex : IFileIndex
    {
        private List<string> Items { get; }

        public MockIndex()
        {
            Items = new List<string>();
        }

        public IEnumerable<string> EnumerateItems()
        {
            return Items;
        }

        public void Add(IEnumerable<string> items)
        {
            foreach (var item in items)
            {
                Items.Add(item);
                
                Program.IConsole.WriteLine($"{item}");
            }
        }

        public void Remove(IEnumerable<string> items)
        {
            foreach (var item in items)
            {
                Items.Remove(item);
                
                Program.IConsole.WriteLine($"{item}");
            }
        }

        public void Clean()
        {
            Program.IConsole.WriteLine("Cleaned index.");
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
    
    public class MockFileSystem : IFileSystem
    {
        public FileSystemOperation<string> ReadText(string path)
        {
            throw new NotImplementedException();
        }
    
        public FileSystemOperation WriteText(string path, string text)
        {
            throw new NotImplementedException();
        }
    
        public FileSystemOperation<StreamWriter> OpenStreamWriter(string path)
        {
            throw new NotImplementedException();
        }
    
        public FileSystemOperation<StreamReader> OpenStreamReader(string path)
        {
            throw new NotImplementedException();
        }
    
        public FileSystemOperation CreateDirectory(string directory)
        {
            throw new NotImplementedException();
        }
    
        public FileSystemOperation<IEnumerable<string>> EnumerateFiles(string directory)
        {
            throw new NotImplementedException();
        }
    
        public FileSystemOperation<IEnumerable<string>> EnumerateDirectories(string directory)
        {
            throw new NotImplementedException();
        }
    
        public FileSystemOperation RecycleFile(string file)
        {
            throw new NotImplementedException();
        }
    
        public FileSystemOperation DeleteFile(string file)
        {
            throw new NotImplementedException();
        }
    
        public FileSystemOperation FileExists(string file)
        {
            throw new NotImplementedException();
        }
    
        public FileSystemOperation DirectoryExists(string file)
        {
            throw new NotImplementedException();
        }
    
        public FileSystemOperation MoveFile(string source, string destination)
        {
            throw new NotImplementedException();
        }
    }
}