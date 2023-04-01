namespace FsTag.Data.Builtin;

internal class BuiltinPaths
{
    public static string RootDataDirectory { get; private set; } = 
        Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Constants.DataDirectoryName);
    
    public static string IntegrationRoot { get; } = 
        Path.Join(Path.GetTempPath(), Path.GetRandomFileName() + "_integration_" + Constants.DataDirectoryName);
    
    public static string SessionDirectoryPath => 
        DataFileHelper.EnsureDirectory(Path.Join(RootDataDirectory, "sessions", App.SessionData.CurrentSessionName));
    
    public static string IndexFilePath => 
        DataFileHelper.EnsureFile(
            Path.Join(
                SessionDirectoryPath,
                "index.nsv"));
    
    public static string ConfigFilePath => 
        DataFileHelper.EnsureJsonFile(Path.Join(RootDataDirectory, "config.json"));

    public static void UseIntegrationRoot()
    {
        RootDataDirectory = IntegrationRoot;
        Directory.CreateDirectory(IntegrationRoot);
    }
}