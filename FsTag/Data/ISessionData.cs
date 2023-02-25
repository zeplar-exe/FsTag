namespace FsTag.Data;

public interface ISessionData
{
    public string? CurrentSessionName { get; }
    public void EnsureSession(string name);
    public IEnumerable<string> GetExistingSessions();
}