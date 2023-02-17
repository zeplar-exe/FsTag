namespace FsTag.Filters.Parsers;

public class RelativeFilterParser : PrefixBasedPathFilterParser
{
    public RelativeFilterParser() : base("r:")
    {
        
    }
    
    public override IEnumerable<string> EnumerateFilesByActualFilter(string actualFilter)
    {
        yield return Path.Join(CurrentDirectory, actualFilter);
    }
}