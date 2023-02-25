namespace FsTag.Data.Models;

public record DocumentationModule(string[] Names, string FilePath)
{
    public string Content => File.ReadAllText(FilePath);
            
    public bool IsMatch(string name)
    {
        return Names.Contains(name);
    }
}