using CommandDotNet;

using FsTag.Resources;

namespace FsTag.Attributes;

public class PathFilterOperand : OperandAttribute
{
    public PathFilterOperand() : base("filter")
    {
        Description = nameof(Descriptions.FilterOperand);
    }
}