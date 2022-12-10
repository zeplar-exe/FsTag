namespace FsTag.Helpers;

public static class PathHelper
{
    public static string GetAbsolute(string path)
    {
        if (IsAbsolutePath(path))
        {
            return path;
        }
        else
        {
            return Path.Join(Directory.GetCurrentDirectory(), path);
        }
    }
    
    private static bool IsAbsolutePath(string path)
    {
        return Path.IsPathRooted(path) || Path.IsPathFullyQualified(path);
    }
}