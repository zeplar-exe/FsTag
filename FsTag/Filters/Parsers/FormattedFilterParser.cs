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

            if (Glob(relative, trimmedFilter))
                yield return file;
        }
    }
    
    private static bool Glob(string text, string pattern)
    {
        var pos = 0;

        while (pattern.Length != pos)
        {
            switch (pattern[pos])
            {
                case '?':
                {
                    break;
                }
                case '*':
                {
                    for (var i = text.Length; i >= pos; i--)
                    {
                        if (Glob(text.Substring(i), pattern.Substring(pos + 1)))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                default:
                {
                    if (text.Length == pos || char.ToUpper(pattern[pos]) != char.ToUpper(text[pos]))
                    {
                        return false;
                    }
                    
                    break;
                }
            }

            pos++;
        }

        return text.Length == pos;
    } // https://stackoverflow.com/a/8094334/16324801
}