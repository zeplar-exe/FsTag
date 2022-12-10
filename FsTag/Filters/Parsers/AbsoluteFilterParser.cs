namespace FsTag.Filters.Parsers;

public class AbsoluteFilterParser : PrefixBasedPathFilterParser
{
    public AbsoluteFilterParser() : base("a:")
    {
        
    }

    public override IEnumerable<string> EnumerateFilesByTrimmed(string trimmedFilter)
    {
        yield return trimmedFilter;
    }
}