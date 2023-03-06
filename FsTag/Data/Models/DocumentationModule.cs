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
        return CleanSteps.Aggregate(content, (current, step) => step.Invoke(current));
    }

    private static readonly Func<string, string>[] CleanSteps =
    {
        t => Regex.Replace(t, @"(?<!^)(?<!\\)\\(?!\\)", ""),
        // https://regex101.com/r/kHRWac/1 - Remove lone escape characters
    };
}