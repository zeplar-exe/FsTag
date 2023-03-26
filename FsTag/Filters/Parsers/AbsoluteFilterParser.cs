using FsTag.Data;

namespace FsTag.Filters.Parsers;

public class AbsoluteFilterParser : PathFilterParser
{
    public override string[] Identifiers => new[] { "a", "abs", "absolute" };
    
    public override IEnumerable<string> EnumerateFiles(string filter, bool includeDirectories)
    {
        if (App.FileSystem.Directory.Exists(filter) && includeDirectories)
            yield break;
        
        yield return filter;
    }
}