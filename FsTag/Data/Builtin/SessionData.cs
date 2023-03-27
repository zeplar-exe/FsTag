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
}