using FsTag.Filters;

namespace FsTag.Helpers;

public static class FilterHelper
{
    public static int ExecuteOnFilterItems(PathFilter filter, bool isRecursive, uint recurseDepth, Action<IEnumerable<string>> action)
    {
        if (isRecursive)
        {
            var files = GetFilesRecursive(filter, recurseDepth);

            action.Invoke(files);

            return 0;
        }

        action.Invoke(filter.EnumerateFiles());

        return 0;
    }
    
    public static IEnumerable<string> GetFilesRecursive(PathFilter filter, uint recurseDepth)
    {
        foreach (var file in filter.EnumerateFiles())
        {
            if (Directory.Exists(file))
            {
                var enumerable = FileSystemHelper.EnumerateFilesToDepth(file, recurseDepth);

                foreach (var item in enumerable)
                {
                    yield return item;
                }
            }
            else
            {
                WriteFormatter.Warning($"The directory '{file}' does not exist.");
            }
        }
    }
}