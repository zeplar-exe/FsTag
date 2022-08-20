using System.Text.RegularExpressions;

namespace FsTag.Common;

public record RegexFormatPair(Regex Regex, Func<Match, int> Handle);