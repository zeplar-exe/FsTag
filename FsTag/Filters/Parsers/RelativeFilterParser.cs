namespace FsTag.Filters.Parsers;

public class RelativeFilterParser : PathFilterParser
{
    public override string[] Identifiers => new[] { "r", "rel", "relative" };
    
    public override IEnumerable<string> EnumerateFiles(string filter)
    {
        yield return Path.Join(CurrentDirectory, filter);
    }
}