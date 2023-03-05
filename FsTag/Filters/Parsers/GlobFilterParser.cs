
using FsTag.Data;

namespace FsTag.Filters.Parsers;

public class GlobFilterParser : PathFilterParser
{
    public override string[] Identifiers => new[] { "f", "formatted", "g", "glob" };
    
    public override IEnumerable<string> EnumerateFiles(string filter)
    {
        var glob = DotNet.Globbing.Glob.Parse(filter);
        
        bool IsMatch(string path)
        {
            var relative = Path.GetRelativePath(CurrentDirectory, path);

            return glob.IsMatch(relative);
        }

        foreach (var file in AppData.FileSystem.EnumerateFiles(CurrentDirectory))
        {
            if (IsMatch(file))
                yield return file;
        }
        
        foreach (var dir in AppData.FileSystem.EnumerateDirectories(CurrentDirectory))
        {
            if (IsMatch(dir))
                yield return dir;
        }
    }
}