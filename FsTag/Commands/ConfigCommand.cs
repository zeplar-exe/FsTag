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
        public int Execute([Option('d', "delimiter")] string delimiter = ";")
        {// move to print [config]
            var config = AppData.GetConfig();
            
            if (config == null)
                return 1;

            var enumerable = (IEnumerable<KeyValuePair<string, JToken?>>)config;
            
            foreach (var item in enumerable.ToArray())
            {
                var format = $"{item.Key}={item.Value?.ToString(GetJsonFormatting(config)) ?? "null"}";
                
                WriteFormatter.PlainNoLine(format + delimiter);
            }

            WriteFormatter.NewLine();

            return 0;
        }
        
        [Command("get")]
        public int Get(string key)
        {
            var json = AppData.GetConfig();
            
            if (json == null)
                return 1;
            
            var value = json[key]?.ToString(GetJsonFormatting(json)) ?? "null";

            WriteFormatter.Plain(value);

            return 0;
        }
        
        [Command("set")]
        public int Set(string key, string value)
        {
            var config = AppData.GetConfig();
            
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

            config[key] = valueToken;

            AppData.WriteConfig(config);
            
            WriteFormatter.Plain($"{key}={valueToken.ToString(GetJsonFormatting(config))}");

            return 0;
        }
        
        [Command("clear")]
        public int Clear()
        {
            AppData.WriteConfig(new JObject());

            return 0;
        }

        private static Formatting GetJsonFormatting(ConfigJsonWrapper wrapper)
        {
            return wrapper.FormatJsonOutput ? Formatting.Indented : Formatting.None;
        }
    }
}