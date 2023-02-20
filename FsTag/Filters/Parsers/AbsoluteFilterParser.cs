namespace FsTag.Filters.Parsers;

public class AbsoluteFilterParser : PathFilterParser
{
    public override string[] Identifiers => new[] { "a", "abs", "absolute" };
    
    public override IEnumerable<string> EnumerateFiles(string filter)
    {
        yield return filter;
    }
}