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
        
        var filesOperation = AppData.FileSystem.EnumerateFiles(directory);

        if (filesOperation.Success)
        {
            foreach (var file in filesOperation.Result)
            {
                yield return file;
            }
        }

        var dirsOperation = AppData.FileSystem.EnumerateDirectories(directory);

        if (dirsOperation.Success)
        {
            foreach (var dir in dirsOperation.Result)
            {
                foreach (var file in EnumerateFilesToDepth(dir, maxDepth, depth + 1))
                {
                    yield return file;
                }
            }
        }
    }
}