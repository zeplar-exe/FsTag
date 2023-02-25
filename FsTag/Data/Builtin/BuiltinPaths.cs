namespace FsTag.Data.Builtin;

internal class BuiltinPaths
{
    public static string Root => 
        Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Constants.DataDirectoryName);
    
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