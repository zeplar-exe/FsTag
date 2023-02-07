using FsTag.Helpers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FsTag;

public static class AppData
{
    private const string DefaultSession = "__default";
    private const string DirectoryName = "fstag";
    private static string RootDirectory => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static string DataDirectory => Path.Join(RootDirectory, DirectoryName);

    public static void IndexFiles(IEnumerable<string> fileNames)
    {
        var set = fileNames.ToHashSet();
        var file = GetIndexFilePath(DefaultSession);

        foreach (var item in EnumerateIndex())
        {
            if (set.Remove(item))
            {
                WriteFormatter.Info($"'{item}' already exists in the tag index.");
            }
        }

        using var writer = new StreamWriter(file, append: true);

        foreach (var item in set)
        {
            if (!File.Exists(item))
            {
                WriteFormatter.Warning($"The file '{item}' does not exist.");
            
                continue;
            }
            
            writer.WriteLine(item);
            WriteFormatter.Info($"Added '{item}' to tag index.");
        }
    }

    public static IEnumerable<string> EnumerateIndex()
    {
        var file = GetIndexFilePath(DefaultSession);

        using var reader = new StreamReader(file);

        while (reader.ReadLine() is {} line)
        {
            yield return line;
        }
    }

    public static void ClearIndex()
    {
        var file = GetIndexFilePath(DefaultSession);
        
        File.WriteAllText(file, string.Empty);
        
        WriteFormatter.Info("Successfully cleared tag index.");
    }

    public static void RemoveFromIndex(IEnumerable<string> fileNames)
    {
        var names = fileNames.ToHashSet();

        var index = GetIndexFilePath(DefaultSession);
        var tempIndex = index + ".tmp";
        var removedAny = false;

        using (var reader = new StreamReader(index))
        {
            using var tempStream = File.OpenWrite(tempIndex);
            using var writer = new StreamWriter(tempStream);

            while (reader.ReadLine() is {} line)
            {
                if (names.Contains(line))
                {
                    WriteFormatter.Info($"Removing '{line}' from the index.");
                    removedAny = true;
                }
                else
                {
                    writer.WriteLine(line);
                }
            }
        }

        if (!removedAny)
        {
            WriteFormatter.Info("Nothing to remove. Terminating early.");
            
            return;
        }

        File.Delete(index);
        File.Move(tempIndex, index);
    }

    public static ConfigJsonWrapper? GetConfig()
    {
        var config = GetConfigFilePath();
        JObject json;

        try
        {
            json = JObject.Parse(File.ReadAllText(config));
        }
        catch (JsonReaderException e)
        {
            WriteFormatter.Error($"A JsonReaderException occured while reading {config}: {e}");
            WriteFormatter.Plain("This usually means your config file is corrupted. Either attempt " +
                                 "to fix it, or use 'fstag config clear' to *clear* the config file.");

            return null;
        }
        
        return new ConfigJsonWrapper(json);
    }

    public static void WriteConfig(JObject json)
    {
        var config = GetConfigFilePath();
        using var writer = new JsonTextWriter(new StreamWriter(config));
        json.WriteTo(writer);
        
        writer.Flush();
    }
    
    private static string GetIndexFilePath(string session)
    {
        var directoryPath = Path.Join(DataDirectory, $"sessions/{session}");
        var filePath = Path.Join(directoryPath, "index.nsv");

        EnsureDirectory(directoryPath);
        EnsureFile(filePath);

        return filePath;
    }
    
    private static string GetConfigFilePath()
    {
        var filePath = Path.Join(DataDirectory, "config.json");
        
        EnsureFile(filePath);

        var info = new FileInfo(filePath);

        if (info.Length == 0)
        {
            File.WriteAllText(filePath, "{}");
        }

        return filePath;
    }

    private static void EnsureDirectory(string directory)
    {
        Directory.CreateDirectory(directory);
    }

    private static void EnsureFile(string file)
    {
        if (!File.Exists(file))
            File.Create(file).Dispose();
    }
}