using System.Text.RegularExpressions;

namespace FsTag.Filters.Parsers;

public class RegexFilterParser : PathFilterParser
{
    public override string[] Identifiers => new[] { "re", "regex" };
    
    public override IEnumerable<string> EnumerateFiles(string filter)
    {
        var regex = new Regex(filter);

        foreach (var file in Directory.EnumerateFileSystemEntries(
                     CurrentDirectory, "*", SearchOption.AllDirectories))
        {
            var relative = Path.GetRelativePath(CurrentDirectory, file);

            if (regex.IsMatch(relative))
                yield return file;
        }
    }
}