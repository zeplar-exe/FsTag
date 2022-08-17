namespace FsTag.Extensions;

internal static class DirectoryInfoExtensions
{
    public static FileInfo CreateFile(this DirectoryInfo directory, string relativePath)
    {
        var info = new FileInfo(Path.Join(directory.FullName, relativePath));

        info.Create().Dispose();
        
        return info;
    }
}