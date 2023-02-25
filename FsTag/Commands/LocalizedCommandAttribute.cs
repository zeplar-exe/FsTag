using CommandDotNet;

using FsTag.Resources;

namespace FsTag;

public class LocalizedCommandAttribute : CommandAttribute
{
    public LocalizedCommandAttribute(string name, string descriptionResourceKey) : base(name)
    {
        Description = Descriptions.ResourceManager.GetString(descriptionResourceKey) ?? "null";
    }
}