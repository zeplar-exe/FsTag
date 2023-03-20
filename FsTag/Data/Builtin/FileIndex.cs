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

    public void Remove(IEnumerable<string> items, uint verbosity)
    {
        var itemsSet = items.ToHashSet();
        var removedAny = false;

        using var indexWriter = new StreamWriter(new MemoryStream());

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
                    indexWriter.WriteLine(line);
                }
            }
            
            indexWriter.Flush();
        }
        
        if (!removedAny)
        {
            WriteFormatter.Info(CommandOutput.FileIndexRemoveNone);
        }

        using var indexStream = App.FileSystem.File.OpenWrite(BuiltinPaths.IndexFilePath);
        indexWriter.BaseStream.CopyTo(indexStream);

        indexStream.Flush();
    }

    public void Clean(uint verbosity)
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