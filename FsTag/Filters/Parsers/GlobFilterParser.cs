
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

        var filesOperation = App.FileSystem.EnumerateFiles(CurrentDirectory);

        if (filesOperation.Success)
        {
            foreach (var file in filesOperation.Result)
            {
                if (IsMatch(file))
                    yield return file;
            }
        }

        var dirsOperation = App.FileSystem.EnumerateDirectories(CurrentDirectory);

        if (dirsOperation.Success)
        {
            foreach (var dir in dirsOperation.Result)
            {
                if (IsMatch(dir))
                    yield return dir;
            }
        }
    }
}