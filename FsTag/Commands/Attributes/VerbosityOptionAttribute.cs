using CommandDotNet;

using FsTag.Resources;

namespace FsTag.Attributes;

public class VerbosityOptionAttribute : OptionAttribute
{
    public VerbosityOptionAttribute(string descriptionKey) : base('v', "verbosity")
    {
        Description = descriptionKey;
    }
    
    public VerbosityOptionAttribute() : base('v', "verbosity")
    {
        Description = nameof(Descriptions.VerbosityOperand);
    }
}