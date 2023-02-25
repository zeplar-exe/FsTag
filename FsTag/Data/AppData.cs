using FsTag.Helpers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FsTag.Data;

public static class AppData
{
    private const string DirectoryName = "fstag";
    private static string RootDirectory => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static string DataDirectory => Path.Join(RootDirectory, DirectoryName);
    private static string CurrentSession => GetConfig()?.SessionDirectory ?? DefaultSession;
    
    public const string DefaultSession = "__default";

    public static string SessionDirectoryPath => EnsureDirectory(Path.Join(DataDirectory, $"sessions/{CurrentSession}"));
    public static string IndexFilePath => EnsureFile(Path.Join(SessionDirectoryPath, "index.nsv"));
    public static string ConfigFilePath => EnsureJsonFile(Path.Join(DataDirectory, "config.json"));
    public static string LabelIndexFilePath => EnsureJsonFile(Path.Join(DataDirectory, "label_index.json"));

    public static void IndexFiles(IEnumerable<string> fileNames)
    {
        var set = fileNames.ToHashSet();

        // Ensure there's no duplicates
        foreach (var item in EnumerateIndex())
        {
            if (set.Remove(item))
            {
                WriteFormatter.Info($"'{item}' already exists in the tag index.");
            }
        }

        using var writer = new StreamWriter(IndexFilePath, append: true);
        
        // By this point, all items in `set` will be unique
        foreach (var item in set)
        {
            if (!File.Exists(item))
            {
                WriteFormatter.Warning($"The file '{item}' does not exist.");
            
                continue;
            }
            
            if (!Program.DryRun)
                writer.WriteLine(item);
            
            WriteFormatter.Info($"Added '{item}' to tag index.");
        }
        
        if (!Program.DryRun)
            writer.Flush();
    }

    public static void RemoveFromIndex(IEnumerable<string> fileNames)
    {
        var names = fileNames.ToHashSet();
        
        var tempIndex = Path.GetTempFileName();
        var removedAny = false;

        using (var reader = new StreamReader(IndexFilePath))
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
                    if (!Program.DryRun)
                        writer.WriteLine(line);
                }
            }
            
            if (!Program.DryRun)
                writer.Flush();
        }

        if (!removedAny)
        {
            WriteFormatter.Info("Nothing to remove. Terminating early.");
            
            return;
        }

        if (!Program.DryRun)
        {
            File.Delete(IndexFilePath);
            File.Move(tempIndex, IndexFilePath);
        }
    }
    
    public static IEnumerable<string> EnumerateIndex()
    {
        using var reader = new StreamReader(IndexFilePath);

        while (reader.ReadLine() is {} line)
        {
            yield return line;
        }
    }
    
    public static void ClearIndex()
    {
        if (!Program.DryRun)
            File.WriteAllText(IndexFilePath, string.Empty);
        
        WriteFormatter.Info("Successfully cleared tag index.");
    }

    public static ConfigJsonWrapper? GetConfig()
    {
        var json = ParseJson(ConfigFilePath);
        
        return json != null ? new ConfigJsonWrapper(json) : null;
    }

    public static void WriteConfig(JObject json)
    {
        WriteJson(ConfigFilePath, json);
    }

    public static JObject? GetLabels()
    {
        var json = ParseJson(LabelIndexFilePath);

        return json;
    }

    public static void WriteLabels(JObject json)
    {
        WriteJson(LabelIndexFilePath, json);
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
        if (Program.DryRun)
            return;
        
        using var writer = new JsonTextWriter(new StreamWriter(path));
        json.WriteTo(writer);
        
        writer.Flush();
    }

    private static string EnsureJsonFile(string path)
    {
        if (Program.DryRun)
            return path;
        
        var info = new FileInfo(path);

        if (info.Length == 0)
        {
            File.WriteAllText(path, "{}");
        }

        return path;
    }

    private static string EnsureDirectory(string directory)
    {
        if (Program.DryRun)
            return directory;
        
        Directory.CreateDirectory(directory);

        return directory;
    }

    private static string EnsureFile(string file)
    {
        if (Program.DryRun)
            return file;
        
        if (!File.Exists(file))
            File.Create(file).Dispose();

        return file;
    }
}