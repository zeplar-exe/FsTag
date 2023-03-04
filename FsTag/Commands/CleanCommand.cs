using CommandDotNet;

using FsTag.Attributes;
using FsTag.Data;
using FsTag.Resources;

namespace FsTag;

public partial class Program
{
    [LocalizedCommand("clean", nameof(Descriptions.CleanCommand))]
    [Subcommand]
    public class CleanCommand
    {
        [DefaultCommand]
        public int Execute()
        {
            AppData.FileIndex.Clean();

            return 0;
        }
    }
}