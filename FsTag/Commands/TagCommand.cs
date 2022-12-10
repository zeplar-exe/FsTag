using CommandDotNet;

using FsTag.Common;

namespace FsTag;

public partial class Program
{
    [Command("tag")]
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