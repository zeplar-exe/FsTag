namespace FsTag.Data.Builtin;

internal class BuiltinPaths
{
    public static string Root { get; private set; } = 
        Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Constants.DataDirectoryName);
    
    public static string IntegrationRoot => Path.Join(Path.GetTempPath(), "integration_" + Constants.DataDirectoryName);
    
    public static string SessionDirectoryPath => 
        DataFileHelper.EnsureDirectory(Path.Join(Root, $"sessions"));
    
    public static string IndexFilePath => 
        DataFileHelper.EnsureFile(
            Path.Join(
                SessionDirectoryPath,
                "index.nsv"));
    
    public static string ConfigFilePath => 
        DataFileHelper.EnsureJsonFile(Path.Join(Root, "config.json"));

    public static void UseIntegrationRoot()
    {
        Root = IntegrationRoot;
    }
}