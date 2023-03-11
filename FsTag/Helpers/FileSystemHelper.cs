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
        
        var files = App.FileSystem.Directory.EnumerateFiles(directory);

        foreach (var file in files)
        {
            yield return file;
        }

        var dirs = App.FileSystem.Directory.EnumerateDirectories(directory);

        foreach (var dir in dirs)
        {
            foreach (var file in EnumerateFilesToDepth(dir, maxDepth, depth + 1))
            {
                yield return file;
            }
        }
    }
}