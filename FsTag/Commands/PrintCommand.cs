using CommandDotNet;

using FsTag.Attributes;
using FsTag.Data;
using FsTag.Helpers;
using FsTag.Resources;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FsTag;

public partial class Program
{
    [Command("print", Description = nameof(Descriptions.PrintCommand))]
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
                    WriteFormatter.Plain(item);
                }
            }),
            new("raw_config", PrintKeyDescriptions.RawConfig, () =>
            {
                var config = AppData.ConfigData.Read();
            
                if (config == null)
                    return;

                var format = config.FormatJsonOutput ? Formatting.Indented : Formatting.None;
                var json = JObject.FromObject(config);
                
                WriteFormatter.Plain(json.ToString(format));
            }),
            new("config_list", PrintKeyDescriptions.ConfigList, () =>
            {
                var config = AppData.ConfigData.Read();
            
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
        };
        
        [DefaultCommand]
        public int Execute(
            [Operand("keys", Description = nameof(Descriptions.PrintKeysOperand))] 
            string[]? keys = null)
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
                
                WriteFormatter.Error(string.Format(CommandOutput.PrintKeyNotFound, key));

                return 1;
            }

            return 0;
        }
    }
    
    private record PrintKey(string Key, string Description, Action Action);
}