﻿using FsTag.Data.Interfaces;
using FsTag.Helpers;
using FsTag.Resources;

namespace FsTag.Data.Builtin;

public class FileIndex : IFileIndex
{
    public IEnumerable<string> EnumerateItems()
    {
        using var reader = new StreamReader(BuiltinPaths.IndexFilePath);

        while (reader.ReadLine() is {} line)
        {
            yield return line;
        }
    }

    public void Add(IEnumerable<string> items)
    {
        var itemsSet = items.ToHashSet();

        // Ensure there's no duplicates
        foreach (var item in EnumerateItems())
        {
            if (itemsSet.Remove(item))
            {
                WriteFormatter.Info(string.Format(CommandOutput.FileIndexAddDuplicate, item));
            }
        }

        using var writer = new StreamWriter(BuiltinPaths.IndexFilePath, append: true);
        
        // By this point, all items in `set` will be unique
        foreach (var item in itemsSet)
        {
            if (!App.FileSystem.FileExists(item))
            {
                WriteFormatter.Warning(string.Format(CommandOutput.FileIndexAddMissing, item));
            
                continue;
            }
            
            if (!Program.DryRun)
                writer.WriteLine(item);
            
            WriteFormatter.Info(string.Format(CommandOutput.FileIndexAddItem, item));
        }
        
        if (!Program.DryRun)
            writer.Flush();
    }

    public void Remove(IEnumerable<string> items)
    {
        var itemsSet = items.ToHashSet();
        
        var tempIndex = Path.GetTempFileName();
        var removedAny = false;

        using (var reader = new StreamReader(BuiltinPaths.IndexFilePath))
        {
            var writerOperation = App.FileSystem.OpenStreamWriter(tempIndex);
            
            if (!writerOperation.Success)
                return;
            
            using var writer = writerOperation.Result;

            while (reader.ReadLine() is {} line)
            {
                if (itemsSet.Contains(line))
                {
                    WriteFormatter.Info(string.Format(CommandOutput.FileIndexRemoveItem, line));
                    removedAny = true;
                }
                else
                {
                    if (!Program.DryRun)
                        writer.WriteLine(line);
                }
            }
            
            if (!Program.DryRun)
                writer.Flush();
        }

        if (!removedAny)
        {
            WriteFormatter.Info(CommandOutput.FileIndexRemoveNone);
            
            return;
        }

        if (!Program.DryRun)
        {
            App.FileSystem.MoveFile(tempIndex, BuiltinPaths.IndexFilePath);
        }
    }

    public void Clean()
    {
        var removed = EnumerateItems().Where(tag => !App.FileSystem.FileExists(tag));
        
        Remove(removed);
    }

    public void Clear()
    {
        if (!Program.DryRun)
            App.FileSystem.WriteText(BuiltinPaths.IndexFilePath, "");
        
        WriteFormatter.Info(CommandOutput.FileIndexClear);
    }
}