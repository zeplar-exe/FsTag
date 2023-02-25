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
            var removed = AppData.FileIndex.EnumerateItems().Where(tag => !File.Exists(tag));
            AppData.FileIndex.Remove(removed);

            return 0;
        }
    }
}