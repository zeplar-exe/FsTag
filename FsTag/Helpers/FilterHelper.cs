﻿using FsTag.Data;
using FsTag.Filters;
using FsTag.Resources;

namespace FsTag.Helpers;

public static class FilterHelper
{
    public static int ExecuteOnFilterItems(PathFilter filter, int recurseDepth, bool includeDirectories, 
        Action<IEnumerable<string>> action)
    {
        var files = GetFilesRecursive(filter, recurseDepth, includeDirectories);

        action.Invoke(files);

        return 0;
    }
    
    public static IEnumerable<string> GetFilesRecursive(PathFilter filter, int recurseDepth, bool includeDirectories)
    {
        foreach (var file in filter.EnumerateFiles(includeDirectories))
        {
            if (App.FileSystem.Directory.Exists(file))
            {
                var enumerable = FileSystemHelper.EnumerateFilesToDepth(file, recurseDepth);

                foreach (var item in enumerable)
                {
                    yield return item;
                }
            }
            else if (App.FileSystem.File.Exists(file))
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