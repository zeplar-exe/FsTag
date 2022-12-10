namespace FsTag.Filters.Parsers;

public abstract class PathFilterParser
{
    public abstract bool CanHandle(string filter);
    public abstract IEnumerable<string> EnumerateFiles(string filter);
}