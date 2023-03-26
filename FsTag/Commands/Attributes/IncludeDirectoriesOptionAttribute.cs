using CommandDotNet;

using FsTag.Resources;

namespace FsTag.Attributes;

public class IncludeDirectoriesOptionAttribute : OptionAttribute
{
    public IncludeDirectoriesOptionAttribute() : base("includeDirs")
    {
        Description = nameof(Descriptions.IncludeDirsOp);
    }
}