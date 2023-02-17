using System.Text.RegularExpressions;

using CommandDotNet;

using FsTag.Common;
using FsTag.Filters;
using FsTag.Helpers;

namespace FsTag;

public partial class Program
{
    [LocalizedCommand("print", nameof(Descriptions.PrintCommand))]
    [Subcommand]
    public class PrintCommand
    {
        [DefaultCommand]
        public int Execute(
            [LocalizedOption('d', "delimiter", nameof(Descriptions.PrintDelimiterOp))]
            string delimiter = ";")
        {
            foreach (var item in AppData.EnumerateIndex())
            {
                WriteFormatter.PlainNoLine(item + delimiter);
            }

            WriteFormatter.NewLine();

            return 0;
        }
    }
}