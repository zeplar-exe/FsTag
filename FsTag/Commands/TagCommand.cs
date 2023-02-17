using CommandDotNet;

using FsTag.Common;
using FsTag.Filters;
using FsTag.Helpers;

namespace FsTag;

public partial class Program
{
    [LocalizedCommand("tag", nameof(Descriptions.TagCommand))]
    [Subcommand]
    public class TagCommand
    {
        [DefaultCommand]
        public int Execute(
            PathFilter filter, 
            [LocalizedOption('r', "recursive", nameof(Descriptions.RecursiveOp))]
            uint recurseDepth = 0)
        {
            return FilterHelper.ExecuteOnFilterItems(filter, recurseDepth, AppData.IndexFiles);
        }
    }
}