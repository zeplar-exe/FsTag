namespace FsTag.Data.Builtin;

public class FilePaths : IFilePaths
{
    private const string DirectoryName = "fstag";
    
    public string GetRootDirectory()
    {
        return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DirectoryName);
    }
}