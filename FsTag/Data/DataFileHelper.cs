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
            var text = AppData.FileSystem.ReadText(path);

            return text != null ? JObject.Parse(text) : null;
        }
        catch (JsonReaderException e)
        {
            WriteFormatter.Error($"A JsonReaderException occured while reading {path}: {e}");
            WriteFormatter.Plain("This usually means your config file is corrupted. Either attempt " +
                                 "to fix it, or use 'fstag config clear' to *clear* the config file.");

            return null;
        }
    }

    public static void WriteJson(string path, JObject json)
    {
        if (Program.DryRun)
            return;

        var textWriter = AppData.FileSystem.OpenTextWriter(path);

        if (textWriter == null)
            return;
        
        using var writer = new JsonTextWriter(textWriter);
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
            AppData.FileSystem.WriteText(path, "{}");
        }

        return path;
    }

    public static string EnsureDirectory(string directory)
    {
        if (Program.DryRun)
            return directory;

        AppData.FileSystem.CreateDirectory(directory);

        return directory;
    }

    public static string EnsureFile(string file)
    {
        if (Program.DryRun)
            return file;

        if (!AppData.FileSystem.FileExists(file))
        {
            AppData.FileSystem.CreateDirectory(Path.GetDirectoryName(file)!);
            AppData.FileSystem.WriteText(file, "");
        }

        return file;
    }
}