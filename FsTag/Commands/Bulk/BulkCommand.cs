using CommandDotNet;

namespace FsTag;

public partial class Program
{
    [Command("bulk")]
    [Subcommand]
    public partial class BulkCommand
    {

    }
}