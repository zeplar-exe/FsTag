namespace FsTag.Data;

public interface IFileIndex
{
    public IEnumerable<string> EnumerateItems();

    public void Add(IEnumerable<string> items);
    public void Remove(IEnumerable<string> items);
    public void Clear();
}