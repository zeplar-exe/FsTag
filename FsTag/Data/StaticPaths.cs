namespace FsTag.Data;

internal class StaticPaths
{
    private static string Root => AppData.FilePaths.GetRootDirectory();
    
    public static string SessionDirectoryPath => 
        DataFileHelper.EnsureDirectory(Path.Join(Root, $"sessions"));
    
    public static string IndexFilePath => 
        DataFileHelper.EnsureFile(
            Path.Join(
                SessionDirectoryPath,
                "index.nsv"));
    
    public static string ConfigFilePath => 
        DataFileHelper.EnsureJsonFile(Path.Join(Root, "config.json"));
}