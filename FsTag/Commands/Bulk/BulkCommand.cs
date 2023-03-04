using CommandDotNet;

using FsTag.Attributes;
using FsTag.Resources;

namespace FsTag;

public partial class Program
{
    [LocalizedCommand("bulk", nameof(Descriptions.BulkCommand))]
    [Subcommand]
    public partial class BulkCommand
    {

    }
}