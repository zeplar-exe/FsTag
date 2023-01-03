using CommandDotNet;

using FsTag.Common;
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
            [Option('a', "all")] bool all,
            [Option('r', "recursive")] bool isRecursive,
            [Option("recurseDepth")] int recurseDepth = -1)
        {
            if (all)
            {
                CommonOutput.WarnWhenXIgnoredBecauseYIsSpecified(filter, all);
                CommonOutput.WarnWhenXIgnoredBecauseYIsSpecified(isRecursive, all);
                CommonOutput.WarnWhenXIgnoredBecauseYIsSpecified(recurseDepth, all);
                
                return ExceptionWrapper.TryExecute(AppData.ClearIndex);
            }
            
            CommonOutput.WarnIfRecurseDepthWithoutRecursion(isRecursive, recurseDepth);
            
            if (CommonOutput.ErrorIfFilterNull(filter))
            {
                return 1;
            }

            return FilterHelper.ExecuteOnFilterItems(filter, isRecursive, recurseDepth, AppData.RemoveFromIndex);
        }
    }
}