using CommandDotNet;

using FsTag.Resources;

namespace FsTag.Attributes;

public class LocalizedOptionAttribute : OptionAttribute
{
    public LocalizedOptionAttribute(char @short, string descriptionResourceKey) : base(@short)
    {
        Description = Descriptions.ResourceManager.GetString(descriptionResourceKey);
    }
    
    public LocalizedOptionAttribute(string @long, string descriptionResourceKey) : base(@long)
    {
        Description = Descriptions.ResourceManager.GetString(descriptionResourceKey);
    }
    
    public LocalizedOptionAttribute(char @short, string @long, string descriptionResourceKey) : base(@short, @long)
    {
        Description = Descriptions.ResourceManager.GetString(descriptionResourceKey);
    }
}