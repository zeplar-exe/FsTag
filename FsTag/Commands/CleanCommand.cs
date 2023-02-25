using CommandDotNet;

using FsTag.Data;

namespace FsTag;

public partial class Program
{
    [Command("clean", Description = "Cleans the index of any files that do not exist.")]
    [Subcommand]
    public class CleanCommand
    {
        [DefaultCommand]
        public int Execute()
        {
            AppData.FileIndex.Clean();

            return 0;
        }
    }
}