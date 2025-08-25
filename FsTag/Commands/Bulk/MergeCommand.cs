using CommandDotNet;

using FsTag.Attributes;
using FsTag.Data;
using FsTag.Helpers;
using FsTag.Resources;

using Microsoft.VisualBasic.FileIO;

using FileSystem = Microsoft.VisualBasic.FileIO.FileSystem;

namespace FsTag;

public partial class Program
{
    public partial class BulkCommand
    {
        [Command("merge", Description = nameof(Descriptions.BulkMergeCommand))]
        [Subcommand]
        public class MergeCommand
        {
            [DefaultCommand]
            public int Execute()
            {
                foreach (var file in App.FileIndex.EnumerateItems())
                {
                    WriteFormatter.Plain(File.ReadAllText(file));
                }

                return 0;
            }
        }
    }
}