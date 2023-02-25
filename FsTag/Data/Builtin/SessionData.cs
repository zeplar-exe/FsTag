using FsTag.Data.Interfaces;
using FsTag.Helpers;

namespace FsTag.Data.Builtin;

public class SessionData : ISessionData
{
    public string? CurrentSessionName
    {
        get
        {
            var name = AppData.ConfigData.TryRead(out var json) ? json.SessionName : null;
            
            if (name != null)
                AppData.SessionData.EnsureSession(name);

            return name;
        }
    }

    public bool EnsureSession(string name)
    {
        var invalidIntersect = Path.GetInvalidPathChars()
            .Intersect(name)
            .ToArray();
            
        if (invalidIntersect.Any())
        {
            WriteFormatter.Error($"Invalid session name, contains {string.Join(" ", invalidIntersect)}");

            return false;
        }
        
        var fullPath = Path.Join(StaticPaths.SessionDirectoryPath, name);
        
        Directory.CreateDirectory(fullPath);

        return true;
    }

    public bool RemoveSession(string name)
    {
        var directory = Path.Join(StaticPaths.SessionDirectoryPath, name);

        if (!Directory.Exists(directory))
        {
            WriteFormatter.Error($"The session '{name}' does not exist.");

            return false;
        }
            
        Directory.Delete(directory);

        return true;
    }

    public IEnumerable<string> GetExistingSessions()
    {
        return Directory.EnumerateDirectories(StaticPaths.SessionDirectoryPath);
    }
}