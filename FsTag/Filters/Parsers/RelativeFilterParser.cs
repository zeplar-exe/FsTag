using FsTag.Data;

namespace FsTag.Filters.Parsers;

public class RelativeFilterParser : PathFilterParser
{
    public override string[] Identifiers => new[] { "r", "rel", "relative" };
    
    public override IEnumerable<string> EnumerateFiles(string filter, bool includeDirectories)
    {
        if (App.FileSystem.Directory.Exists(filter) && includeDirectories)
            yield break;
        
        yield return Path.Join(CurrentDirectory, filter);
    }
}