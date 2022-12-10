using FsTag.Filters.Parsers;
using FsTag.Helpers;

namespace FsTag.Filters;

public class PathFilter
{
    private FilterParserContainer ParserContainer { get; }
    
    private string Filter { get; }
    
    public PathFilter(string filter)
    {
        Filter = filter;
        
        ParserContainer = new FilterParserContainer();
        ParserContainer.Add<AbsoluteFilterParser>();
        ParserContainer.Add<RelativeFilterParser>();
        ParserContainer.Add<FormattedFilterParser>();
    }

    public IEnumerable<string> EnumerateFiles()
    {
        foreach (var parser in ParserContainer.EnumerateParsers())
        {
            if (parser.CanHandle(Filter))
                return parser.EnumerateFiles(Filter);
        }

        return new[] { PathHelper.GetAbsolute(Filter) };
    }
}