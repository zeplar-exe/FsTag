using CommandDotNet;

using FsTag.Data.Interfaces;

namespace FsTag.Tests.Unit.Mocks;

public class MockIndex : IFileIndex
{
    private List<string> Items { get; }

    public MockIndex()
    {
        Items = new List<string>();
    }

    public IEnumerable<string> EnumerateItems()
    {
        return Items.AsReadOnly();
    }

    public void Add(IEnumerable<string> items)
    {
        foreach (var item in items)
        {
            Items.Add(item);
        }
    }

    public void Remove(IEnumerable<string> items, int verbosity)
    {
        foreach (var item in items)
        {
            Items.Remove(item);
        }
    }

    public void Clean(int verbosity)
    {
        Program.IConsole.WriteLine("Cleaned index.");
    }

    public void Clear()
    {
        Items.Clear();
    }
}