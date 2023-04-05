using System.Diagnostics;

using CommandDotNet;

using FsTag.Data;
using FsTag.Helpers;
using FsTag.Resources;

namespace FsTag;

public partial class Program
{
    [Command("session", Description = nameof(Descriptions.SessionCommand))]
    [Subcommand]
    public class SessionCommand
    {
        [DefaultCommand]
        public int Execute()
        {
            var currentSession = App.SessionData.CurrentSessionName;

            foreach (var session in App.SessionData.GetExistingSessions())
            {
                if (session == currentSession)
                {
                    WriteFormatter.Plain("***" + currentSession);
                }
                else
                {
                    WriteFormatter.Plain(currentSession!);
                }
            }

            return 0;
        }
        
        [Command("switch", Description = nameof(Descriptions.SessionSwitchCommand))]
        public int Switch([Operand("name", Description = nameof(Descriptions.SessionSwitchName))] string name)
        {
            if (!App.SessionData.EnsureSession(name))
                return 1;

            App.SessionData.CurrentSessionName = name;

            return 0;
        }

        [Command("rm", Description = nameof(Descriptions.SessionRemoveCommand))]
        public int Remove([Operand("name", Description = nameof(Descriptions.SessionRemoveName))] string name)
        {
            return App.SessionData.RemoveSession(name) ? 0 : 1;
        }
    }
}