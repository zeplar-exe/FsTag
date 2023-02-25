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

    public void EnsureSession(string name)
    {
        var fullPath = Path.Join(StaticPaths.SessionDirectoryPath, name);
        
        Directory.CreateDirectory(fullPath);
    }

    public IEnumerable<string> GetExistingSessions()
    {
        return Directory.EnumerateDirectories(StaticPaths.SessionDirectoryPath);
    }
}