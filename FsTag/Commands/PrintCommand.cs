using CommandDotNet;

using FsTag.Data;
using FsTag.Helpers;
using FsTag.Resources;

using Newtonsoft.Json;

namespace FsTag;

public partial class Program
{
    [LocalizedCommand("print", nameof(Descriptions.PrintCommand))]
    [Subcommand]
    public class PrintCommand
    {
        private static readonly PrintKey[] Keys =
        {
            new("print_keys", PrintKeyDescriptions.PrintKeys, () =>
            {
                foreach (var key in Keys)
                {
                    WriteFormatter.Plain($"{key.Key} - {key.Description}");
                }
            }),
            new("index", PrintKeyDescriptions.Index, () =>
            {
                foreach (var item in AppData.EnumerateIndex())
                {
                    WriteFormatter.PlainNoLine(item + ';');
                }

                WriteFormatter.NewLine();
            }),
            new("index_path", PrintKeyDescriptions.IndexPath, () =>
            {
                WriteFormatter.Plain(AppData.IndexFilePath);
            }),
            new("config_path", PrintKeyDescriptions.ConfigPath, () =>
            {
                WriteFormatter.Plain(AppData.ConfigFilePath);
            }),
            new("session_path", PrintKeyDescriptions.SessionPath, () =>
            {
                WriteFormatter.Plain(AppData.SessionDirectoryPath);
            }),
            new("label_index_path", PrintKeyDescriptions.LabelIndexPath, () =>
            {
                WriteFormatter.Plain(AppData.LabelIndexFilePath);
            }),
            new("raw_config", PrintKeyDescriptions.RawConfig, () =>
            {
                WriteFormatter.Plain(AppData.GetConfig()?.ToString(Formatting.Indented) ?? "null");
            }),
            new("raw_label_index", PrintKeyDescriptions.RawLabelIndex, () =>
            {
                WriteFormatter.Plain(AppData.GetLabels()?.ToString(Formatting.Indented) ?? "null");
            }),
        };
        
        [DefaultCommand]
        public int Execute(string[]? keys = null)
        {
            if (keys == null)
            {
                WriteFormatter.NewLine();
                
                WriteFormatter.Plain(CommonOutput.ValidArgumentList);
                
                WriteFormatter.NewLine();
                
                foreach (var key in Keys)
                {
                    WriteFormatter.Plain($"{key.Key} - {key.Description}");
                }
                
                WriteFormatter.NewLine();
                
                return 0;
            }
            
            foreach (var key in keys)
            {
                foreach (var registered in Keys)
                {
                    if (registered.Key == key)
                    {
                        registered.Action.Invoke();

                        return 0;
                    }
                }
                
                WriteFormatter.Error($"'{key}' is not a recognized print key.");

                return 1;
            } // fstag tag r/rel/relative a/b/c

            return 0;
        }
    }
    
    private record PrintKey(string Key, string Description, Action Action);
}