namespace FsTag.Filters.Parsers;

public class FormattedFilterParser : PrefixBasedPathFilterParser
{
    public FormattedFilterParser() : base("f:")
    {
        
    }
    
    public override IEnumerable<string> EnumerateFilesByTrimmed(string trimmedFilter)
    {
        yield break;
    }
}