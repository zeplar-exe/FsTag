using System.Runtime.InteropServices.ComTypes;

using CommandDotNet;

namespace FsTag;

public partial class Program
{
    [Command("clean")]
    [Subcommand]
    public class CleanCommand
    {
        [DefaultCommand]
        public int Execute(string a = "")
        {
            var removed = AppData.EnumerateIndex().Where(tag => !File.Exists(tag));

            AppData.RemoveFromIndex(removed);

            return 0;
        }
    }
}