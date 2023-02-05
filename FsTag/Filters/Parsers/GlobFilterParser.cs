using DotNet.Globbing;

namespace FsTag.Filters.Parsers;

public class GlobFilterParser : PrefixBasedPathFilterParser
{
    public GlobFilterParser() : base("f:")
    {
        
    }
    
    public override IEnumerable<string> EnumerateFilesByTrimmed(string trimmedFilter)
    {
        var glob = Glob.Parse(trimmedFilter);
        
        foreach (var file in Directory.EnumerateFileSystemEntries(
                     CurrentDirectory, "*", SearchOption.AllDirectories))
        {
            var relative = Path.GetRelativePath(CurrentDirectory, file);

            if (glob.IsMatch(relative))
                yield return file;
        }
    }
}