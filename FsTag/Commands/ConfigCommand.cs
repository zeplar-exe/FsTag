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
    [Command("config", Description = nameof(Descriptions.ConfigCommand))]
    [Subcommand]
    public class ConfigCommand
    {
        [DefaultCommand]
        public int Execute([Operand("key", Description = nameof(Descriptions.GetConfigKey))] string name)
        {
            var config = AppData.ConfigData.Read();
            
            if (config == null)
                return 1;

            var formatting = JsonHelper.GetConfigJsonFormatting(config);

            if (!config.TryGet<JToken>(name, out var value))
            {
                WriteFormatter.Info(string.Format(CommandOutput.ConfigMissing, name));

                return 1;
            }

            WriteFormatter.Plain(value?.ToString(formatting) ?? "null");

            return 0;
        }
        
        [Command("set", Description = nameof(Descriptions.ConfigSetCommand))]
        public int Set(
            [Operand("key", Description = nameof(Descriptions.SetConfigKey))] string key,
            [Operand("value", Description = nameof(Descriptions.SetConfigValue))] string value)
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
                WriteFormatter.Error(string.Format(CommandOutput.ConfigSetInvalidJson, value, e.Message));
                
                return 1;
            }
            
            AppData.ConfigData.SetProperty(key, valueToken);

            var formatting = JsonHelper.GetConfigJsonFormatting(config);
            WriteFormatter.Plain($"{key}={valueToken.ToString(formatting)}");

            return 0;
        }
    }
}