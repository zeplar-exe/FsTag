using CommandDotNet;
using CommandDotNet.TypeDescriptors;

using FsTag.Filters.Parsers;
using FsTag.Helpers;

namespace FsTag.Filters;

// https://commanddotnet.bilal-fazlani.com/arguments/argument-models/
public class PathFilter : IArgumentModel
{
    private FilterParserContainer ParserContainer { get; }
    
    [Operand("Filter Identifier")]
    public string Identifier { get; set; }
    [Operand("Filter")]
    public string Filter { get; set; }

    public PathFilter()
    {
        ParserContainer = new FilterParserContainer();
        ParserContainer.Add<AbsoluteFilterParser>();
        ParserContainer.Add<RelativeFilterParser>();
        ParserContainer.Add<GlobFilterParser>();
        ParserContainer.Add<RegexFilterParser>();
    }

    public IEnumerable<string> EnumerateFiles()
    {
        foreach (var parser in ParserContainer.EnumerateParsers())
        {
            if (parser.Identifiers.Contains(Identifier))
                return parser.EnumerateFiles(Filter);
        }

        return new[] { PathHelper.GetAbsolute(Filter) };
    }
}