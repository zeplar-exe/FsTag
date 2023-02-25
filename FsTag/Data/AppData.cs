using FsTag.Data.Builtin;

namespace FsTag.Data;

public static class AppData
{
    public static ISessionData SessionData { get; set; } = new SessionData();
    public static IFileIndex FileIndex { get; set; } = new FileIndex();
    public static IConfigData ConfigData { get; set; } = new ConfigData();
    public static IFilePaths FilePaths { get; set; } = new FilePaths();
}