namespace FsTag.Filters.Parsers;

public abstract class PathFilterParser
{
    protected string CurrentDirectory => Directory.GetCurrentDirectory();
    
    public abstract string[] Identifiers { get; }
    public abstract IEnumerable<string> EnumerateFiles(string filter);
}