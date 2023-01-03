using System.Text.RegularExpressions;

using CommandDotNet;

using FsTag.Common;
using FsTag.Helpers;

namespace FsTag;

public partial class Program
{
    [Command("print", Description = "Print files that are currently tagged (in the index).")]
    [Subcommand]
    public class PrintCommand
    {
        [DefaultCommand]
        public int Execute(
            [Option("delimiter", Description = "Delimiter between file paths.")] string delimiter = ";",
            [Option("glob", Description = "The glob format to filter files.")] string glob = "*")
        {
            foreach (var item in AppData.EnumerateIndex())
            {
                if (!Glob.IsMatch(glob, item))
                    continue;
                
                WriteFormatter.Plain(item + delimiter);
            }
                
            WriteFormatter.NewLine();

            return 0;
        }
    }
}