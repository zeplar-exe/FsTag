using CommandDotNet;

using FsTag.Resources;

namespace FsTag;

public partial class Program
{
    [Command("bulk", Description = nameof(Descriptions.BulkCommand))]
    [Subcommand]
    public partial class BulkCommand
    {

    }
}