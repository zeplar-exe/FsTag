using FsTag.Data.Interfaces;
using FsTag.Helpers;
using FsTag.Resources;

namespace FsTag.Data.Builtin;

public class SessionData : ISessionData
{
    public string? CurrentSessionName
    {
        get
        {
            var name = App.ConfigData.Read()?.SessionName;
            
            if (name != null)
                App.SessionData.EnsureSession(name);

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
            WriteFormatter.Error(string.Format(CommandOutput.SessionCreateInvalidName, string.Join(" ", invalidIntersect)));

            return false;
        }
        
        var fullPath = Path.Join(BuiltinPaths.SessionDirectoryPath, name);
        
        Directory.CreateDirectory(fullPath);

        return true;
    }

    public bool RemoveSession(string name)
    {
        var directory = Path.Join(BuiltinPaths.SessionDirectoryPath, name);

        if (!Directory.Exists(directory))
        {
            WriteFormatter.Error(string.Format(CommandOutput.SessionMissing, name));

            return false;
        }
            
        Directory.Delete(directory);

        return true;
    }

    public IEnumerable<string> GetExistingSessions()
    {
        return Directory.EnumerateDirectories(BuiltinPaths.SessionDirectoryPath);
    }
}