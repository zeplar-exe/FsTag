using System.Text.RegularExpressions;

namespace FsTag;

public class PathFilter
{
    private static Regex AbsoluteRegex = new("a:(.*)");
    private static Regex RelativeRegex = new("r:(.*)");
    private static Regex FormattedRegex = new("f:(.*)");
    
    private string Filter { get; }
    
    public PathFilter(string filter)
    {
        Filter = filter;
    }

    public IEnumerable<string> EnumerateFiles()
    {
        var formatted = FormattedRegex.Match(Filter);

        if (formatted.Success)
        {
            var formattedFilter = formatted.Groups[1].Value;
            
            foreach (var file in HandleFormatted(formattedFilter))
            {
                yield return file;
            }
        }
        
        var absolute = AbsoluteRegex.Match(Filter);
        var relative = RelativeRegex.Match(Filter);

        if (absolute.Success)
        {
            yield return PathHelper.GetAbsolute(absolute.Groups[1].Value);
        }
        else if (relative.Success)
        {
            yield return PathHelper.GetAbsolute(relative.Groups[1].Value);
        }
        else
        {
            yield return PathHelper.GetAbsolute(Filter);   
        }
    }
    
    private IEnumerable<string> HandleFormatted(string formatted)
    {
        yield break;
    }
}