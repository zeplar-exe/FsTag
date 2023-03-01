using CommandDotNet;

using FsTag.Data;
using FsTag.Helpers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FsTag;

public partial class Program
{
    [Command("config")]
    [Subcommand]
    public class ConfigCommand
    {
        [DefaultCommand]
        public int Execute(string name)
        {
            if (!AppData.ConfigData.TryRead(out var config))
                return 1;

            var formatting = JsonHelper.GetConfigJsonFormatting(config);

            if (!config.TryGet<JToken>(name, out var value))
            {
                WriteFormatter.Info($"The configuration '{name}' does not exist.");

                return 1;
            }

            WriteFormatter.Plain(value.ToString(formatting));

            return 0;
        }
        
        [Command("set")]
        public int Set(string key, string value)
        {
            if (!AppData.ConfigData.TryRead(out var config))
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