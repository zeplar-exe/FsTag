namespace FsTag.Data;

public interface ISessionData
{
    public string? CurrentSessionName { get; }
    public IEnumerable<string> GetExistingSessions();
}