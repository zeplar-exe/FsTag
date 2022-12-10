using System.Text.RegularExpressions;

using CommandDotNet;

using FsTag.Common;

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
                        WriteFormatter.Plain(delimiter);
                    
                    WriteFormatter.Plain(item);

                    first = false;
                }
                
                WriteFormatter.NewLine();

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
    }
}