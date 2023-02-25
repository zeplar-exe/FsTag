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
            var value = config[name]?.ToString(formatting) ?? "null";

            WriteFormatter.Plain(value);

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