using FsTag.Helpers;
using FsTag.Resources;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FsTag.Data;

internal class DataFileHelper
{
    public static JObject? ParseJson(string path)
    {
        try
        {
            var text = App.FileSystem.File.ReadAllText(path);

            return JObject.Parse(text);
        }
        catch (JsonReaderException e)
        {
            WriteFormatter.Error(string.Format(CommandOutput.ParseJsonReaderException, path, e));

            return null;
        }
    }

    public static void WriteJson(string path, JObject json)
    {
        if (Program.DryRun)
            return;

        var stream = App.FileSystem.File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
        using var writer = new JsonTextWriter(new StreamWriter(stream));
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
            App.FileSystem.File.WriteAllText(path, "{}");
        }

        return path;
    }

    public static string EnsureDirectory(string directory)
    {
        if (Program.DryRun)
            return directory;

        App.FileSystem.Directory.CreateDirectory(directory);

        return directory;
    }

    public static string EnsureFile(string file)
    {
        if (Program.DryRun)
            return file;

        if (!App.FileSystem.File.Exists(file))
        {
            App.FileSystem.Directory.CreateDirectory(Path.GetDirectoryName(file)!);
            App.FileSystem.File.WriteAllText(file, "");
        }

        return file;
    }
}