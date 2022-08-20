using System.Text.RegularExpressions;

namespace FsTag.Common;

public class FormatPairBuilder
{
    private List<RegexFormatPair> Pairs { get; }

    public FormatPairBuilder()
    {
        Pairs = new List<RegexFormatPair>();
    }

    public SinglePair HandleFormat(Regex regex)
    {
        return new SinglePair(regex, this);
    }

    public List<RegexFormatPair> Build()
    {
        return Pairs;
    }

    public class SinglePair
    {
        private Regex Regex { get; }
        private FormatPairBuilder Builder { get; }

        public SinglePair(Regex regex, FormatPairBuilder builder)
        {
            Regex = regex;
            Builder = builder;
        }

        public FormatPairBuilder With(Func<Match, int> handler)
        {
            var pair = new RegexFormatPair(Regex, handler);
                    
            Builder.Pairs.Add(pair);

            return Builder;
        }
    }
}