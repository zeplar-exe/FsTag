using FsTag.Common;

namespace FsTag.Filters.Parsers;

public class FormattedFilterParser : PrefixBasedPathFilterParser
{
    public FormattedFilterParser() : base("f:")
    {
        
    }
    
    public override IEnumerable<string> EnumerateFilesByTrimmed(string trimmedFilter)
    {
        foreach (var file in Directory.EnumerateFileSystemEntries(
                     CurrentDirectory, "*", SearchOption.AllDirectories))
        {
            var relative = Path.GetRelativePath(CurrentDirectory, file);

            if (Glob.IsMatch(relative, trimmedFilter))
                yield return file;
        }
    }
}