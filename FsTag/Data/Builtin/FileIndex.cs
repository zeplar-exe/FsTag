using System.Text;

using FsTag.Data.Interfaces;
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
        
        // By this point, all items in `set` will be unique to the index
        foreach (var item in itemsSet)
        {
            if (!App.FileSystem.File.Exists(item))
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

    public void Remove(IEnumerable<string> items, int verbosity)
    {
        var itemsSet = items.ToHashSet();
        var removedAny = false;

        var indexBuilder = new StringBuilder();

        using (var reader = new StreamReader(BuiltinPaths.IndexFilePath))
        {
            while (reader.ReadLine() is {} line)
            {
                if (itemsSet.Contains(line))
                {
                    if (verbosity >= 1)
                        WriteFormatter.Info(string.Format(CommandOutput.FileIndexRemoveItem, line));
                    
                    removedAny = true;
                }
                else
                {
                    indexBuilder.AppendLine(line);
                }
            }
        }
        
        if (!removedAny)
        {
            WriteFormatter.Info(CommandOutput.FileIndexRemoveNone);
        }

        if (!Program.DryRun)
        {
            App.FileSystem.File.WriteAllText(BuiltinPaths.IndexFilePath, indexBuilder.ToString());
        }
    }

    public void Clean(int verbosity)
    {
        var removed = EnumerateItems().Where(tag => !App.FileSystem.File.Exists(tag));
        
        Remove(removed, verbosity);
    }

    public void Clear()
    {
        if (!Program.DryRun)
            App.FileSystem.File.WriteAllText(BuiltinPaths.IndexFilePath, "");
        
        WriteFormatter.Info(CommandOutput.FileIndexClear);
    }
}