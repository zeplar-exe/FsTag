using FsTag.Data.Interfaces;
using FsTag.Helpers;
using FsTag.Resources;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FsTag.Data.Builtin;

public class SessionData : ISessionData
{
    private static string SessionInfoFilePath => Path.Join(BuiltinPaths.SessionDirectoryPath, "info.json");
    private const string DefaultSessionName = "__default";

    public string? CurrentSessionName
    {
        get
        {
            var info = GetSessionInfo();
            
            if (info.CurrentSession != null)
            {
                EnsureSession(info.CurrentSession);
            }
            else
            {
                info.CurrentSession = DefaultSessionName;
                
                DataFileHelper.WriteJson(SessionInfoFilePath, JObject.FromObject(info));
            }

            return info.CurrentSession;
        }
        set
        {
            var info = GetSessionInfo();

            info.CurrentSession = value;
            
            DataFileHelper.WriteJson(SessionInfoFilePath, JObject.FromObject(info));
        }
    }

    private SessionInfo GetSessionInfo()
    {
        DataFileHelper.EnsureFile(SessionInfoFilePath);

        var json = DataFileHelper.ParseJson(SessionInfoFilePath);
        var info = json?.ToObject<SessionInfo>();

        if (json == null || info == null)
        {
            info = new SessionInfo
            {
                CurrentSession = DefaultSessionName
            };
        }

        return info;
    }

    public bool EnsureSession(string name)
    {
        var invalidIntersect = Path.GetInvalidPathChars()
            .Intersect(name)
            .ToArray();
            
        if (invalidIntersect.Any())
        {
            WriteFormatter.Error(string.Format(CommandOutput.SessionCreateInvalidName, string.Join(" ", invalidIntersect)));

            return false;
        }
        
        var fullPath = Path.Join(BuiltinPaths.SessionDirectoryPath, name);
        
        App.FileSystem.Directory.CreateDirectory(fullPath);

        return true;
    }

    public bool RemoveSession(string name)
    {
        if (CurrentSessionName == name)
        {
            WriteFormatter.Error("Cannot remove the current session. Switch to a different one.");

            return false;
        }
        
        var directory = Path.Join(BuiltinPaths.SessionDirectoryPath, name);

        if (!App.FileSystem.Directory.Exists(directory))
        {
            WriteFormatter.Error(string.Format(CommandOutput.SessionMissing, name));

            return false;
        }
            
        App.FileSystem.Directory.Delete(directory);

        return true;
    }

    public IEnumerable<string> GetExistingSessions()
    {
        return App.FileSystem.Directory.EnumerateDirectories(BuiltinPaths.SessionDirectoryPath);
    }

    private class SessionInfo
    {
        [JsonProperty("current_session")] public string? CurrentSession { get; set; }
    }
}