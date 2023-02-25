namespace FsTag.Data.Builtin;

internal class StaticPaths
{
    private static string Root => AppData.FilePaths.RootDataDirectory;
    
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