using CommandDotNet;

using FsTag.Attributes;
using FsTag.Data;
using FsTag.Filters;
using FsTag.Helpers;
using FsTag.Resources;

namespace FsTag;

public partial class Program
{
    [LocalizedCommand("rm", nameof(Descriptions.RemoveCommand))]
    [Subcommand]
    public class RemoveCommand
    {
        [DefaultCommand]
        public int Execute(
            PathFilter filter,
            [RecurseOption] uint recurseDepth = 0)
        {
            return FilterHelper.ExecuteOnFilterItems(filter, recurseDepth, AppData.FileIndex.Remove);
        }

        [LocalizedCommand("all", nameof(Descriptions.RemoveAllCommand))]
        public int All()
        {
            AppData.FileIndex.Clear();

            return 0;
        }
    }
}