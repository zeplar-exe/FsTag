using CommandDotNet;

using FsTag.Data.Interfaces;

namespace FsTag.Tests.Unit.Mocks;

public class MockFileIndex : IFileIndex
{
    private List<string> Items { get; }
    
    public bool Cleaned { get; private set; }

    public MockFileIndex()
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
        Cleaned = true;
    }

    public void Clear()
    {
        Items.Clear();
    }
}