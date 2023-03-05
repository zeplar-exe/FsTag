using CommandDotNet;

using FsTag.Resources;

namespace FsTag.Attributes;

public class LocalizedOperandAttribute : OperandAttribute
{
    public LocalizedOperandAttribute(string name, string descriptionResourceKey) : base(name)
    {
        Description = Descriptions.ResourceManager.GetString(descriptionResourceKey) ?? "null";
    }
}