using FsTag.Data.Interfaces;

namespace FsTag.Data.Builtin;

public class FilePaths : IFilePaths
{
    private const string DirectoryName = "fstag";
    
    public string RootDataDirectory => 
        Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DirectoryName);

    public string DocsDirectory => Path.Join(Directory.GetCurrentDirectory(), "docs");
}