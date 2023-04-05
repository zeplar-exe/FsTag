namespace FsTag.Data.Interfaces;

public interface ISessionData
{
    public string? CurrentSessionName { get; set; }
    public bool EnsureSession(string name);
    public bool RemoveSession(string name);
    public IEnumerable<string> GetExistingSessions();
}