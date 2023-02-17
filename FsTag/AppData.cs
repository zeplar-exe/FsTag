using FsTag.Helpers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FsTag;

public static class AppData
{
    private const string DirectoryName = "fstag";
    private static string RootDirectory => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static string DataDirectory => Path.Join(RootDirectory, DirectoryName);
    private static string CurrentSession => GetConfig()?.SessionDirectory ?? DefaultSession;
    
    public const string DefaultSession = "__default";

    public static string SessionDirectoryPath = EnsureDirectory(Path.Join(DataDirectory, $"sessions/{CurrentSession}"));

    public static void IndexFiles(IEnumerable<string> fileNames)
    {
        var set = fileNames.ToHashSet();
        var file = GetIndexFilePath();

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

    public static void RemoveFromIndex(IEnumerable<string> fileNames)
    {
        var names = fileNames.ToHashSet();

        var index = GetIndexFilePath();
        var tempIndex = Path.GetTempFileName();
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
    
    public static IEnumerable<string> EnumerateIndex()
    {
        var file = GetIndexFilePath();

        using var reader = new StreamReader(file);

        while (reader.ReadLine() is {} line)
        {
            yield return line;
        }
    }
    
    public static void ClearIndex()
    {
        var file = GetIndexFilePath();
        
        File.WriteAllText(file, string.Empty);
        
        WriteFormatter.Info("Successfully cleared tag index.");
    }

    public static ConfigJsonWrapper? GetConfig()
    {
        var json = ParseJson(GetConfigFilePath());
        
        return json != null ? new ConfigJsonWrapper(json) : null;
    }

    public static void WriteConfig(JObject json)
    {
        WriteJson(GetConfigFilePath(), json);
    }

    public static JObject? GetLabels()
    {
        var json = ParseJson(GetLabelIndexFilePath());

        return json;
    }

    public static void WriteLabels(JObject json)
    {
        WriteJson(GetLabelIndexFilePath(), json);
    }

    private static JObject? ParseJson(string path)
    {
        JObject json;

        try
        {
            json = JObject.Parse(File.ReadAllText(path));
        }
        catch (JsonReaderException e)
        {
            WriteFormatter.Error($"A JsonReaderException occured while reading {path}: {e}");
            WriteFormatter.Plain("This usually means your config file is corrupted. Either attempt " +
                                 "to fix it, or use 'fstag config clear' to *clear* the config file.");

            return null;
        }
        
        return json;
    }

    private static void WriteJson(string path, JObject json)
    {
        using var writer = new JsonTextWriter(new StreamWriter(path));
        json.WriteTo(writer);
        
        writer.Flush();
    }
    
    private static string GetIndexFilePath()
    {
        var filePath = EnsureFile(Path.Join(SessionDirectoryPath, "index.nsv"));

        return filePath;
    }
    
    private static string GetConfigFilePath()
    {
        return EnsureJsonFile(Path.Join(DataDirectory, "config.json"));
    }

    private static string GetLabelIndexFilePath()
    {
        return EnsureJsonFile(Path.Join(DataDirectory, "label_index.json"));
    }

    private static string EnsureJsonFile(string path)
    {
        var info = new FileInfo(path);

        if (info.Length == 0)
        {
            File.WriteAllText(path, "{}");
        }

        return path;
    }

    private static string EnsureDirectory(string directory)
    {
        Directory.CreateDirectory(directory);

        return directory;
    }

    private static string EnsureFile(string file)
    {
        if (!File.Exists(file))
            File.Create(file).Dispose();

        return file;
    }
}