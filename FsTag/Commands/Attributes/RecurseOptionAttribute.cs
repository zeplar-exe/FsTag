using CommandDotNet;

using FsTag.Resources;

namespace FsTag.Attributes;

public class RecurseOptionAttribute : OptionAttribute
{
    public RecurseOptionAttribute() : base('r', "recursive")
    {
        Description = nameof(Descriptions.RecursiveOp);
    }
}