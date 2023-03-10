using FsTag.Helpers;
using FsTag.Resources;

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
            var textOperation = App.FileSystem.ReadText(path);

            return textOperation.Success ? JObject.Parse(textOperation.Result) : null;
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

        var writerOperation = App.FileSystem.OpenStreamWriter(path);

        if (!writerOperation.Success)
            return;
        
        using var writer = new JsonTextWriter(writerOperation.Result);
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
            App.FileSystem.WriteText(path, "{}");
        }

        return path;
    }

    public static string EnsureDirectory(string directory)
    {
        if (Program.DryRun)
            return directory;

        App.FileSystem.CreateDirectory(directory);

        return directory;
    }

    public static string EnsureFile(string file)
    {
        if (Program.DryRun)
            return file;

        if (!App.FileSystem.FileExists(file))
        {
            App.FileSystem.CreateDirectory(Path.GetDirectoryName(file)!);
            App.FileSystem.WriteText(file, "");
        }

        return file;
    }
}