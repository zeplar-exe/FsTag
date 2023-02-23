using FsTag.Glob;

namespace FsTag.Filters.Parsers;

public class GlobFilterParser : PathFilterParser
{
    public override string[] Identifiers => new[] { "f", "formatted", "g", "glob" };
    
    public override IEnumerable<string> EnumerateFiles(string filter)
    {
        var glob = FileGlob.Parse(filter);

        return glob.GetMatchesFrom(CurrentDirectory);
    }
}