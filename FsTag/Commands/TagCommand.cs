using CommandDotNet;

using FsTag.Attributes;
using FsTag.Data;
using FsTag.Filters;
using FsTag.Helpers;
using FsTag.Resources;

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
            [RecurseOption] uint recurseDepth = 0)
        {
            return FilterHelper.ExecuteOnFilterItems(filter, recurseDepth, AppData.FileIndex.Add);
        }
    }
}