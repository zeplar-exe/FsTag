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