namespace FsTag.Data.Builtin;

public class SessionData : ISessionData
{
    public string? CurrentSessionName => AppData.ConfigData.TryRead(out var json) ? json.SessionName : null;

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