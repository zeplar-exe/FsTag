using System.Text.RegularExpressions;

namespace FsTag.Data.Models;

public class DocumentationModule
{
    public string[] Names { get; }
    public string Content { get; }
    
    public DocumentationModule(string[] names, string content)
    {
        Names = names;
        Content = CleanContent(content); 
    }

    public bool IsMatch(string name)
    {
        return Names.Contains(name);
    }

    private static string CleanContent(string content)
    {
        // https://regex101.com/r/kHRWac/1
        return Regex.Replace(content, @"(?<!^)(?<!\\)\\(?!\\)", "");
    }

    public void Deconstruct(out string[] Names, out string Content)
    {
        Names = this.Names;
        Content = this.Content;
    }
}