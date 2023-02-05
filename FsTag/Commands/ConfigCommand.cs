using CommandDotNet;

using FsTag.Helpers;

namespace FsTag;

public partial class Program
{
    [Command("config")]
    [Subcommand]
    public class ConfigCommand
    {
        [DefaultCommand]
        public int Execute(string delimiter = ";")
        {
            return ExceptionWrapper.TryExecute(() =>
            {
                var config = AppData.GetConfigs();
                
                if (config == null)
                    return 1;
                
                foreach (var item in config)
                {
                    var format = $"{item.Key}=\"{item.Value}\"";
                    
                    WriteFormatter.Plain(format + delimiter);
                }

                WriteFormatter.NewLine();

                return 0;
            });
        }
        
        [Command("get")]
        public int Get(string key, [Option("ctx")] string context = ".")
        {
            return ExceptionWrapper.TryExecute(() =>
            {
                WriteFormatter.Plain(AppData.GetConfig(key) ?? "null");
            });
        }
        
        [Command("set")]
        public int Set(string key, string value, [Option("ctx")] string context = ".")
        {
            return ExceptionWrapper.TryExecute(() =>
            {
                AppData.UpdateConfig(key, value);
                
                WriteFormatter.Plain($"{key}=\"{value}\"");
            });
        }
        
        [Command("clear")]
        public int Clear([Option("ctx")] string context = ".")
        {
            return ExceptionWrapper.TryExecute(AppData.ClearConfig);
        }
    }
}