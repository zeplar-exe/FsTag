using FsTag.Filters.Parsers;

namespace FsTag.Filters;

public class FilterParserContainer
{
    private List<PathFilterParser> Parsers { get; }

    public FilterParserContainer()
    {
        Parsers = new List<PathFilterParser>();
    }

    public void Add<T>() where T : PathFilterParser, new()
    {
        Parsers.Add(new T());
    }

    public IEnumerable<PathFilterParser> EnumerateParsers()
    {
        return Parsers;
    }
}