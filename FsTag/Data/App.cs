using FsTag.Data.Builtin;
using FsTag.Data.Interfaces;

namespace FsTag.Data;

public static class App
{
    public static ISessionData SessionData { get; set; } = new SessionData();
    public static IFileIndex FileIndex { get; set; } = new FileIndex();
    public static IConfigData ConfigData { get; set; } = new ConfigData();
    public static IDocumentationData DocumentationData { get; set; } = new DocumentationData();
    public static IFileSystem FileSystem { get; set; }
}