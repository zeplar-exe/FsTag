using System.Text.RegularExpressions;

using FsTag.Data;

namespace FsTag.Filters.Parsers;

public class RegexFilterParser : PathFilterParser
{
    public override string[] Identifiers => new[] { "re", "regex" };
    
    public override IEnumerable<string> EnumerateFiles(string filter)
    {
        var regex = new Regex(filter);

        bool IsMatch(string path)
        {
            var relative = Path.GetRelativePath(CurrentDirectory, path);

            return regex!.IsMatch(relative);
        }

        var files = App.FileSystem.Directory.EnumerateFiles(CurrentDirectory, "*");

        foreach (var file in files)
        {
            if (IsMatch(file))
                yield return file;
        }

        var dirs = App.FileSystem.Directory.EnumerateDirectories(CurrentDirectory);

        foreach (var dir in dirs)
        {
            if (IsMatch(dir))
                yield return dir;
        }
    }
}