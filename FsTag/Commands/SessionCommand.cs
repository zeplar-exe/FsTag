using CommandDotNet;

using FsTag.Attributes;
using FsTag.Data;
using FsTag.Helpers;
using FsTag.Resources;

namespace FsTag;

public partial class Program
{
    [LocalizedCommand("session", nameof(Descriptions.SessionCommand))]
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
        
        [LocalizedCommand("switch", nameof(Descriptions.SessionSwitchCommand))]
        public int Switch(string name)
        {
            if (!AppData.SessionData.EnsureSession(name))
                return 1;
            
            AppData.ConfigData.SetProperty("session_name", name);

            return 0;
        }

        [LocalizedCommand("rm", nameof(Descriptions.SessionRemoveCommand))]
        public int Remove(string name)
        {
            return AppData.SessionData.RemoveSession(name) ? 0 : 1;
        }
    }
}