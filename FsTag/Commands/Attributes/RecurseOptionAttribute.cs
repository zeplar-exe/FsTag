using FsTag.Resources;

namespace FsTag.Attributes;

public class RecurseOptionAttribute : LocalizedOptionAttribute
{
    public RecurseOptionAttribute() : base('r', "recursive", nameof(Descriptions.RecursiveOp))
    {
        
    }
}