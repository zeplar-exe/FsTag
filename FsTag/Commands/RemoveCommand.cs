using CommandDotNet;

using FsTag.Filters;
using FsTag.Helpers;

namespace FsTag;

public partial class Program
{
    [Command("rm", Description = "Removes files from the index based on the provided filter.")]
    [Subcommand]
    public class RemoveCommand
    {
        [DefaultCommand]
        public int Execute(
            PathFilter? filter,
            [LocalizedOption('r', "recursive", nameof(Descriptions.RecursiveOp))] uint recurseDepth = 0)
        {
            return FilterHelper.ExecuteOnFilterItems(filter, recurseDepth, AppData.RemoveFromIndex);
        }

        [LocalizedCommand("all", nameof(Descriptions.RemoveAllCommand))]
        public int All()
        {
            AppData.ClearIndex();

            return 0;
        }
    }
}