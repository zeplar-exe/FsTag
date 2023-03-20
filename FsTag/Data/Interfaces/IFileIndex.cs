namespace FsTag.Data.Interfaces;

public interface IFileIndex
{
    public IEnumerable<string> EnumerateItems();

    public void Add(IEnumerable<string> items);
    public void Remove(IEnumerable<string> items, uint verbosity);
    public void Clean(uint verbosity);
    public void Clear();
}