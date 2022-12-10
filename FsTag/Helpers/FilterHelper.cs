using FsTag.Filters;

namespace FsTag.Helpers;

public static class FilterHelper
{
    public static int ExecuteOnFilterItems(PathFilter filter, bool isRecursive, int recurseDepth, Action<IEnumerable<string>> action)
    {
        if (isRecursive)
        {
            var files = GetFilesRecursive(filter, recurseDepth);
                
            return ExceptionWrapper.TryExecute(() => action.Invoke(files));
        }

        return ExceptionWrapper.TryExecute(() => action.Invoke(filter.EnumerateFiles()));
    }
    
    public static IEnumerable<string> GetFilesRecursive(PathFilter filter, int recurseDepth)
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