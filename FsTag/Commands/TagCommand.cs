using CommandDotNet;

using FsTag.Common;
using FsTag.Filters;
using FsTag.Helpers;

namespace FsTag;

public partial class Program
{
    [Command("tag", Description = "Adds files to the index based on the provided filter.")]
    [Subcommand]
    public class TagCommand
    {
        [DefaultCommand]
        public int Execute(
            PathFilter filter, 
            [Option('r', "recursive")] bool isRecursive, 
            [Option("recurseDepth")] int recurseDepth = -1)
        {
            CommonOutput.WarnIfRecurseDepthWithoutRecursion(isRecursive, recurseDepth);
            
            return FilterHelper.ExecuteOnFilterItems(filter, isRecursive, recurseDepth, AppData.IndexFiles);
        }
    }
}