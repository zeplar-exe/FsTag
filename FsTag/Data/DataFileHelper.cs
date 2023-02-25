using FsTag.Helpers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FsTag.Data;

internal class DataFileHelper
{
    public static JObject? ParseJson(string path)
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

    public static void WriteJson(string path, JObject json)
    {
        if (Program.DryRun)
            return;
        
        using var writer = new JsonTextWriter(new StreamWriter(path));
        json.WriteTo(writer);
        
        writer.Flush();
    }

    public static string EnsureJsonFile(string path)
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

    public static string EnsureDirectory(string directory)
    {
        if (Program.DryRun)
            return directory;
        
        Directory.CreateDirectory(directory);

        return directory;
    }

    public static string EnsureFile(string file)
    {
        if (Program.DryRun)
            return file;
        
        if (!File.Exists(file))
            File.Create(file).Dispose();

        return file;
    }
}