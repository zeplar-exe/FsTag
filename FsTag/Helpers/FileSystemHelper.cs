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
        foreach (var file in Directory.EnumerateFiles(directory))
        {
            yield return file;
        }
            
        if (depth != maxDepth)
        {
            foreach (var dir in Directory.EnumerateDirectories(directory))
            {
                foreach (var file in EnumerateFilesToDepth(dir, maxDepth, depth + 1))
                {
                    yield return file;
                }
            }
        }
    }
}