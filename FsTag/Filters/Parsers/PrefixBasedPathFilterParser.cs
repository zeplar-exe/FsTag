namespace FsTag.Filters.Parsers;

public abstract class PrefixBasedPathFilterParser : PathFilterParser
{
    private string Prefix { get; }

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
        return EnumerateFilesByTrimmed(filter.Substring(Prefix.Length));
    }

    public abstract IEnumerable<string> EnumerateFilesByTrimmed(string trimmedFilter);
}