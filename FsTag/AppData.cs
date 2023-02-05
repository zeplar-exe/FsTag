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
        var file = IndexFile(DefaultSession);

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
        var file = IndexFile(DefaultSession);

        using var reader = new StreamReader(file);

        while (reader.ReadLine() is {} line)
        {
            yield return line;
        }
    }

    public static void ClearIndex()
    {
        var file = IndexFile(DefaultSession);
        
        File.WriteAllText(file, string.Empty);
        
        WriteFormatter.Info("Successfully cleared tag index.");
    }

    public static void RemoveFromIndex(IEnumerable<string> fileNames)
    {
        var names = fileNames.ToHashSet();

        var index = IndexFile(DefaultSession);
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

    public static JObject? GetConfigs()
    {
        var config = ConfigFile(DefaultSession);
        JObject json;

        try
        {
            json = JObject.Parse(File.ReadAllText(config));
        }
        catch (JsonReaderException e)
        {
            WriteFormatter.Error($"A JsonReaderException occured while reading {config}: {e}");
            WriteFormatter.Plain("This usually means your config file is corrupted. Either attempt " +
                                 "to fix it, or use 'fstag config clear' to reset the config file.");

            return null;
        }

        return json;
    }

    public static string? GetConfig(string key)
    {
        return GetConfigs()?.TryGetValue(key, out var value) ?? false ? value.Value<string>() : null;
    }

    public static void UpdateConfig(string key, string value)
    {
        var json = GetConfigs();

        if (json == null)
        {
            WriteFormatter.Error("Could not update config due to an invalid config JSON.");
            
            return;
        }
        
        json[key] = value;
        
        var config = ConfigFile(DefaultSession);
        using var writer = new JsonTextWriter(new StreamWriter(config));
        json.WriteTo(writer);
        
        writer.Flush();
    }
    
    public static void ClearConfig()
    {
        var config = ConfigFile(DefaultSession);
        var json = new JObject();
        var writer = new JsonTextWriter(new StreamWriter(config));
        json.WriteTo(writer);
        
        WriteFormatter.Info("Successfully cleared configuration.");
    }
    
    private static string IndexFile(string session)
    {
        var path = Path.Join(DataDirectory, $"sessions/{session}");
        var file = Path.Join(path, "index.nsv");

        EnsureDirectory(path);
        EnsureFile(file);

        return file;
    }
    
    private static string ConfigFile(string session)
    {
        var path = Path.Join(DataDirectory, $"sessions/{session}");
        var file = Path.Join(path, "config.json");

        EnsureDirectory(path);
        EnsureFile(file);

        var info = new FileInfo(file);

        if (info.Length == 0)
        {
            File.WriteAllText(file, "{}");
        }

        return file;
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