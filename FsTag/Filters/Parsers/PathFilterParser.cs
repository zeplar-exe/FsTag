using FsTag.Data;

namespace FsTag.Filters.Parsers;

public abstract class PathFilterParser
{
    protected string CurrentDirectory => App.FileSystem.Directory.GetCurrentDirectory();
    
    public abstract string[] Identifiers { get; }
    
    /// <summary>
    /// In derived classes, this method should be implemented to yield matching
    /// file paths relative downward from the working directory.
    /// </summary>
    /// <param name="filter">The raw filter string from the user.</param>
    /// <param name="includeDirectories">If specified, this method should also
    /// match directories if possible.</param>
    /// <returns></returns>
    public abstract IEnumerable<string> EnumerateFiles(string filter, bool includeDirectories);
}