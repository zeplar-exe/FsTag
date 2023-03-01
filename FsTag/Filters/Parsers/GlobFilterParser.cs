
namespace FsTag.Filters.Parsers;

public class GlobFilterParser : PathFilterParser
{
    public override string[] Identifiers => new[] { "f", "formatted", "g", "glob" };
    
    public override IEnumerable<string> EnumerateFiles(string filter)
    {
        var glob = DotNet.Globbing.Glob.Parse(filter);

        foreach (var file in Directory.EnumerateFileSystemEntries(CurrentDirectory))
        {
            var relative = Path.GetRelativePath(CurrentDirectory, file);
            
            if (glob.IsMatch(relative))
                yield return file;
        }
    }
}