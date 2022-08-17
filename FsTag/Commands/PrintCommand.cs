using System.Text.RegularExpressions;

using CommandDotNet;

namespace FsTag;

public partial class Program
{
    [Command("print")]
    [Subcommand]
    public class PrintCommand
    {
        private static readonly Regex DelimiterRegex = new("delimiter:(.*)");
        
        [DefaultCommand]
        public int Execute(string format = "delimiter:;")
        {
            var builder = new FormatPairBuilder();

            builder.HandleFormat(DelimiterRegex).With(match =>
            {
                var delimiter = match.Groups[1].Value;
                var first = true;

                foreach (var item in AppData.EnumerateIndex())
                {
                    if (!first)
                        Console.Write(delimiter);
                    
                    Console.Write(item);

                    first = false;
                }
                
                Console.WriteLine();

                return 0;
            });

            var pairs = builder.Build();

            foreach (var formatPair in pairs)
            {
                var match = formatPair.Regex.Match(format);

                if (match.Success)
                    return formatPair.Handle(match);
            }
            
            WriteFormatter.Error($"'{format}' is unrecognized as a print format.");

            return 0;
        }

        private class FormatPairBuilder
        {
            private List<FormatPair> Pairs { get; }

            public FormatPairBuilder()
            {
                Pairs = new List<FormatPair>();
            }

            public SinglePair HandleFormat(Regex regex)
            {
                return new SinglePair(regex, this);
            }

            public List<FormatPair> Build()
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
                    var pair = new FormatPair(Regex, handler);
                    
                    Builder.Pairs.Add(pair);

                    return Builder;
                }
            }
        }

        private record FormatPair(Regex Regex, Func<Match, int> Handle);
    }
}