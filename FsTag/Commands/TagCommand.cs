﻿using CommandDotNet;

using FsTag.Attributes;
using FsTag.Data;
using FsTag.Filters;
using FsTag.Helpers;
using FsTag.Resources;

namespace FsTag;

public partial class Program
{
    [Command("tag", Description = nameof(Descriptions.TagCommand))]
    [Subcommand]
    public class TagCommand
    {
        [DefaultCommand]
        public int Execute(
            [PathFilterOperand] PathFilter filter, 
            [RecurseOption] int recurseDepth = 0,
            [IncludeDirectoriesOption] bool includeDirectories = false)
        {
            return FilterHelper.ExecuteOnFilterItems(filter, recurseDepth, includeDirectories, App.FileIndex.Add);
        }
    }
}