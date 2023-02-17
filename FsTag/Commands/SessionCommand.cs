using CommandDotNet;

using FsTag.Helpers;

namespace FsTag;

public partial class Program
{
    [Command("session")]
    [Subcommand]
    public class SessionCommand
    {
        [DefaultCommand]
        public int Execute()
        {
            var directory = AppData.SessionDirectoryPath;
            
            if (Verbose)
            {
                WriteFormatter.Plain(directory);
            }
            else
            {
                WriteFormatter.Plain(Path.GetFileName(directory));
            }

            return 0;
        }
        
        [Command("switch")]
        public int Switch(string name)
        {
            var config = AppData.GetConfig();

            if (config == null)
                return 1;

            config["session_name"] = name;
            
            AppData.WriteConfig(config);
            
            return 0;
        }

        [Command("rm")]
        public int Remove(string name)
        {
            var config = AppData.GetConfig();

            if (config == null)
                return 1;

            var directory = config.SessionDirectory;
            
            Directory.Delete(directory);

            return 0;
        }
    }
}