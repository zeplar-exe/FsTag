using FsTag.Data.Interfaces;
using FsTag.Data.Models;
using FsTag.Helpers;
using FsTag.Resources;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FsTag.Data.Builtin;

public class PrintKeyProvider : IPrintKeyProvider
{
    private static readonly PrintData[] PrintData =
   {
       new("print_keys", PrintKeyDescriptions.PrintKeys, () =>
       {
           foreach (var data in PrintData)
           {
               WriteFormatter.Plain($"{data.Key} - {data.Description}");
           }
       }),
       new("index", PrintKeyDescriptions.Index, () =>
       {
           foreach (var item in App.FileIndex.EnumerateItems())
           {
               WriteFormatter.Plain(item);
           }
       }),
       new("index_size", PrintKeyDescriptions.Index, () =>
       {
           WriteFormatter.Plain(App.FileIndex.EnumerateItems().ToArray().Length.ToString());
       }),
       new("raw_config", PrintKeyDescriptions.RawConfig, () =>
       {
           var config = App.ConfigData.Read();
       
           if (config == null)
               return;

           var format = config.FormatJsonOutput ? Formatting.Indented : Formatting.None;
           var json = JObject.FromObject(config);
           
           WriteFormatter.Plain(json.ToString(format));
       }),
       new("config_list", PrintKeyDescriptions.ConfigList, () =>
       {
           var config = App.ConfigData.Read();
       
           if (config == null)
               return;
           
           var json = JObject.FromObject(config);

           foreach (var item in json)
           {
               var formatting = JsonHelper.GetConfigJsonFormatting(config);
               var output = $"{item.Key}={item.Value?.ToString(formatting) ?? "null"}";
           
               WriteFormatter.Plain(output);
           }
       }),
       new("data_directory_path", PrintKeyDescriptions.DataDirectoryPath, () =>
       {
           WriteFormatter.Plain(BuiltinPaths.RootDataDirectory);
       }),
       new("config_file_path", PrintKeyDescriptions.ConfigFilePath, () =>
       {
           WriteFormatter.Plain(BuiltinPaths.ConfigFilePath);
       }),
       new("index_file_path", PrintKeyDescriptions.IndexFilePath, () =>
       {
           WriteFormatter.Plain(BuiltinPaths.IndexFilePath);
       }),
       new("session_directory_path", PrintKeyDescriptions.SessionDirectoryPath, () =>
       {
           WriteFormatter.Plain(BuiltinPaths.SessionDirectoryPath);
       })
   };

    public PrintData? Get(string key)
    {
        return PrintData.FirstOrDefault(p => p.Key == key);
    }

    public IEnumerable<PrintData> EnumerateData()
    {
        return PrintData;
    }
}