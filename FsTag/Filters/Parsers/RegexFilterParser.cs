using System.Text.RegularExpressions;

namespace FsTag.Filters.Parsers;

public class RegexFilterParser : PrefixBasedPathFilterParser
{
    public RegexFilterParser() : base("re:")
    {
        
    }
    

    public override IEnumerable<string> EnumerateFilesByTrimmed(string trimmedFilter)
    {
        var regex = new Regex(trimmedFilter);

        foreach (var file in Directory.EnumerateFileSystemEntries(
                     CurrentDirectory, "*", SearchOption.AllDirectories))
        {
            var relative = Path.GetRelativePath(CurrentDirectory, file);

            if (regex.IsMatch(relative))
                yield return file;
        }
    }
}