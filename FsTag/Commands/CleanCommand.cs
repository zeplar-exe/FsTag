using CommandDotNet;

using FsTag.Attributes;
using FsTag.Data;
using FsTag.Resources;

namespace FsTag;

public partial class Program
{
    [Command("clean", Description = nameof(Descriptions.CleanCommand))]
    [Subcommand]
    public class CleanCommand
    {
        [DefaultCommand]
        public int Execute([VerbosityOption] int verbosity = 0)
        {
            App.FileIndex.Clean(verbosity);

            return 0;
        }
    }
}