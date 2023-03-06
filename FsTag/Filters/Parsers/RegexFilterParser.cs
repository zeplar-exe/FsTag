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

        var filesOperation = AppData.FileSystem.EnumerateFiles(CurrentDirectory);

        if (filesOperation.Success)
        {
            foreach (var file in filesOperation.Result)
            {
                if (IsMatch(file))
                    yield return file;
            }
        }

        var dirsOperation = AppData.FileSystem.EnumerateDirectories(CurrentDirectory);

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