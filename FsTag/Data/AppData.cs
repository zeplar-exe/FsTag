using FsTag.Data.Builtin;
using FsTag.Data.Interfaces;

namespace FsTag.Data;

public static class AppData
{
    public static ISessionData SessionData { get; set; } = new SessionData();
    public static IFileIndex FileIndex { get; set; } = new FileIndex();
    public static IConfigData ConfigData { get; set; } = new ConfigData();
    public static IFilePaths FilePaths { get; set; } = new FilePaths();
    public static IDocumentationData DocumentationData { get; set; } = new DocumentationData();
}