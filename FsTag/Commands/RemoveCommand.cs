using CommandDotNet;

using FsTag.Attributes;
using FsTag.Data;
using FsTag.Filters;
using FsTag.Helpers;
using FsTag.Resources;

namespace FsTag;

public partial class Program
{
    [Command("rm", Description = nameof(Descriptions.RemoveCommand))]
    [Subcommand]
    public class RemoveCommand
    {
        [DefaultCommand]
        public int Execute(
            [PathFilterOperand] PathFilter filter,
            [RecurseOption] uint recurseDepth = 0)
        {
            return FilterHelper.ExecuteOnFilterItems(filter, recurseDepth, App.FileIndex.Remove);
        }

        [Command("all", Description = nameof(Descriptions.RemoveAllCommand))]
        public int All()
        {
            App.FileIndex.Clear();

            return 0;
        }
    }
}