
using FsTag.Data;

namespace FsTag.Filters.Parsers;

public class GlobFilterParser : PathFilterParser
{
    public override string[] Identifiers => new[] { "f", "formatted", "g", "glob" };
    
    public override IEnumerable<string> EnumerateFiles(string filter, bool includeDirectories)
    {
        var glob = DotNet.Globbing.Glob.Parse(filter);
        
        bool IsMatch(string path)
        {
            var relative = Path.GetRelativePath(CurrentDirectory, path);

            return glob.IsMatch(relative);
        }

        var files = App.FileSystem.Directory.EnumerateFiles(CurrentDirectory);

        foreach (var file in files)
        {
            if (IsMatch(file))
                yield return file;
        }

        if (includeDirectories)
        {
            var dirs = App.FileSystem.Directory.EnumerateDirectories(CurrentDirectory);

            foreach (var dir in dirs)
            {
                if (IsMatch(dir))
                    yield return dir;
            }
        }
    }
}