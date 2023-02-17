namespace FsTag.Filters.Parsers;

public class AbsoluteFilterParser : PrefixBasedPathFilterParser
{
    public AbsoluteFilterParser() : base("a:")
    {
        
    }

    public override IEnumerable<string> EnumerateFilesByActualFilter(string actualFilter)
    {
        yield return actualFilter;
    }
}