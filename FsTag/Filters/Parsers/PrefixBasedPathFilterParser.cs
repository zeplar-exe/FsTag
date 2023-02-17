namespace FsTag.Filters.Parsers;

public abstract class PrefixBasedPathFilterParser : PathFilterParser
{
    private string Prefix { get; }

    protected string CurrentDirectory => Directory.GetCurrentDirectory();

    protected PrefixBasedPathFilterParser(string prefix)
    {
        Prefix = prefix;
    }

    public sealed override bool CanHandle(string filter)
    {
        return filter.StartsWith(Prefix);
    }

    public sealed override IEnumerable<string> EnumerateFiles(string filter)
    {
        return EnumerateFilesByActualFilter(filter.Substring(Prefix.Length));
    }

    public abstract IEnumerable<string> EnumerateFilesByActualFilter(string actualFilter);
}