﻿using FsTag.Data;
using FsTag.Filters;
using FsTag.Resources;

namespace FsTag.Helpers;

public static class FilterHelper
{
    public static int ExecuteOnFilterItems(PathFilter filter, uint recurseDepth, Action<IEnumerable<string>> action)
    {
        var files = GetFilesRecursive(filter, recurseDepth);

        action.Invoke(files);

        return 0;
    }
    
    public static IEnumerable<string> GetFilesRecursive(PathFilter filter, uint recurseDepth)
    {
        foreach (var file in filter.EnumerateFiles())
        {
            if (App.FileSystem.DirectoryExists(file))
            {
                var enumerable = FileSystemHelper.EnumerateFilesToDepth(file, recurseDepth);

                foreach (var item in enumerable)
                {
                    yield return item;
                }
            }
            else if (App.FileSystem.FileExists(file))
            {
                yield return file;
            }
            else
            {
                WriteFormatter.Warning(string.Format(CommandOutput.DirectoryMissing, file));
            }
        }
    }
}