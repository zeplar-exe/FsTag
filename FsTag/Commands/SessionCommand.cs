using CommandDotNet;

using FsTag.Data;
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
            var currentSession = AppData.SessionData.CurrentSessionName;

            foreach (var session in AppData.SessionData.GetExistingSessions())
            {
                if (session == currentSession)
                {
                    WriteFormatter.Plain("***" + currentSession);
                }
                else
                {
                    WriteFormatter.Plain(currentSession);
                }
            }

            return 0;
        }
        
        [Command("switch")]
        public int Switch(string name)
        {
            var invalidIntersect = Path.GetInvalidFileNameChars()
                .Intersect(name)
                .ToArray();
            
            if (invalidIntersect.Any())
            {
                WriteFormatter.Error($"Invalid session name, contains {string.Join(" ", invalidIntersect)}");

                return 1;
            }
            
            AppData.SessionData.EnsureSession(name);
            AppData.ConfigData.SetProperty("session_name", name);

            return 0;
        }

        [Command("rm")]
        public int Remove(string name)
        {
            var directory = Path.Join(StaticPaths.SessionDirectoryPath, name);

            if (!Directory.Exists(directory))
            {
                WriteFormatter.Error($"The session '{name}' does not exist.");

                return 1;
            }
            
            Directory.Delete(directory);

            return 0;
        }
    }
}