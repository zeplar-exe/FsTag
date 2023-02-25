using CommandDotNet;
using CommandDotNet.TestTools.Scenarios;

using FsTag.Data;

namespace FsTag.Tests.CommandTests;

[TestFixture]
public class Tag : TestBase
{
    const string TestFileName = "test/test1.txt";

    [SetUp]
    public void TagSetup()
    {
        AppData.FileIndex = new MockIndex();
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

        public void Clear()
        {
            Items.Clear();
        }
    }
}