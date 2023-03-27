using CommandDotNet;

using FsTag.Filters.Parsers;
using FsTag.Helpers;

namespace FsTag.Filters;

// https://commanddotnet.bilal-fazlani.com/arguments/argument-models/
public class PathFilter : IArgumentModel
{
    private FilterParserContainer ParserContainer { get; }
    
    [Operand("Filter/Identifier")]
    public string Identifier { get; set; }
    
    /// <remarks>When this is null, it is to be handled as if the identifier is a relative or absolute path.</remarks>
    [Operand("Filter")]
    public string? Filter { get; set; }

    public PathFilter()
    {
        ParserContainer = new FilterParserContainer();
        ParserContainer.Add<AbsoluteFilterParser>();
        ParserContainer.Add<RelativeFilterParser>();
        ParserContainer.Add<GlobFilterParser>();
        ParserContainer.Add<RegexFilterParser>();
    }

    public IEnumerable<string> EnumerateFiles(bool includeDirectories)
    {
        if (Filter == null)
        {
            // See doc comment on Filter
            return new[] { PathHelper.GetAbsolute(Identifier) };
        }
        
        foreach (var parser in ParserContainer.EnumerateParsers())
        {
            if (parser.Identifiers.Contains(Identifier))
                return parser.EnumerateFiles(Filter, includeDirectories);
        }

        return Array.Empty<string>();
    }
}