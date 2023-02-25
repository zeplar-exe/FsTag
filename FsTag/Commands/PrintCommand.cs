using CommandDotNet;

using FsTag.Attributes;
using FsTag.Data;
using FsTag.Data.Builtin;
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
                foreach (var item in AppData.FileIndex.EnumerateItems())
                {
                    WriteFormatter.PlainNoLine(item + ';');
                }

                WriteFormatter.NewLine();
            }),
            new("raw_config", PrintKeyDescriptions.RawConfig, () =>
            {
                if (!AppData.ConfigData.TryRead(out var config))
                    return;

                var format = config.FormatJsonOutput ? Formatting.Indented : Formatting.None;
                
                WriteFormatter.Plain(config.ToString(format));
            }),
            new("config_list", PrintKeyDescriptions.ConfigList, () =>
            {
                if (!AppData.ConfigData.TryRead(out var config))
                    return;

                foreach (var item in config)
                {
                    var formatting = JsonHelper.GetConfigJsonFormatting(config);
                    var output = $"{item.Key}={item.Value?.ToString(formatting) ?? "null"}";
                
                    WriteFormatter.Plain(output);
                }
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
            }

            return 0;
        }
    }
    
    private record PrintKey(string Key, string Description, Action Action);
}