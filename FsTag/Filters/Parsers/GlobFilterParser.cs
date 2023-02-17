using DotNet.Globbing;

using FsTag.Glob;

namespace FsTag.Filters.Parsers;

public class GlobFilterParser : PrefixBasedPathFilterParser
{
    public GlobFilterParser() : base("f:")
    {
        
    }
    
    public override IEnumerable<string> EnumerateFilesByActualFilter(string actualFilter)
    {
        var glob = FileGlob.Parse(actualFilter);
        
        foreach (var file in Directory.EnumerateFileSystemEntries(
                     CurrentDirectory, "*", SearchOption.AllDirectories))
        {
            var relative = Path.GetRelativePath(CurrentDirectory, file);

            if (glob.IsMatchFrom(relative))
                yield return file; // custom glob
        }
    }
}