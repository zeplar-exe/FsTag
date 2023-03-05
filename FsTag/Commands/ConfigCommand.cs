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
    [LocalizedCommand("config", nameof(Descriptions.ConfigCommand))]
    [Subcommand]
    public class ConfigCommand
    {
        [DefaultCommand]
        public int Execute(string name)
        {
            var config = AppData.ConfigData.Read();
            
            if (config == null)
                return 1;

            var formatting = JsonHelper.GetConfigJsonFormatting(config);

            if (!config.TryGet<JToken>(name, out var value))
            {
                WriteFormatter.Info($"The configuration '{name}' does not exist.");

                return 1;
            }

            WriteFormatter.Plain(value?.ToString(formatting) ?? "null");

            return 0;
        }
        
        [LocalizedCommand("set", nameof(Descriptions.ConfigSetCommand))]
        public int Set(string key, string value)
        {
            var config = AppData.ConfigData.Read();
            
            if (config == null)
                return 1;

            JToken valueToken;

            try
            {
                valueToken = JToken.Parse(value);
            }
            catch (JsonReaderException e)
            {
                WriteFormatter.Error($"'{value}' is not valid JSON: {e.Message}");
                
                return 1;
            }
            
            AppData.ConfigData.SetProperty(key, valueToken);

            var formatting = JsonHelper.GetConfigJsonFormatting(config);
            WriteFormatter.Plain($"{key}={valueToken.ToString(formatting)}");

            return 0;
        }
    }
}