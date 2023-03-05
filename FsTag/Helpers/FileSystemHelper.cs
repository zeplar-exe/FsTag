using FsTag.Data;

namespace FsTag.Helpers;

public static class FileSystemHelper
{
    public static IEnumerable<string> EnumerateFilesToDepth(string directory, uint targetDepth)
    {
        foreach (var file in EnumerateFilesToDepth(directory, targetDepth, 0))
        {
            yield return file;
        }
    }
        
    private static IEnumerable<string> EnumerateFilesToDepth(string directory, uint maxDepth, uint depth)
    {
        if (depth > maxDepth)
            yield break;
        
        foreach (var file in AppData.FileSystem.EnumerateFiles(directory))
        {
            yield return file;
        }
            
        foreach (var dir in AppData.FileSystem.EnumerateDirectories(directory))
        {
            foreach (var file in EnumerateFilesToDepth(dir, maxDepth, depth + 1))
            {
                yield return file;
            }
        }
    }
}