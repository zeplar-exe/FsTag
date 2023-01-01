namespace FsTag.Filters.Parsers;

public class RelativeFilterParser : PrefixBasedPathFilterParser
{
    public RelativeFilterParser() : base("r:")
    {
        
    }
    
    public override IEnumerable<string> EnumerateFilesByTrimmed(string trimmedFilter)
    {
        yield return Path.Join(CurrentDirectory, trimmedFilter);
    }
}