using FsTag.Data.Interfaces;

namespace FsTag.Tests.Unit.Mocks;

public class MockSessionData : ISessionData
{
    private HashSet<string> Sessions { get; }

    public string? CurrentSessionName { get; set; } = "default_mock";

    public MockSessionData()
    {
        Sessions = new HashSet<string>();
    }

    public bool EnsureSession(string name)
    {
        Sessions.Add(name);

        return true;
    }

    public bool RemoveSession(string name)
    {
        return CurrentSessionName != name && Sessions.Remove(name);
    }

    public IEnumerable<string> GetExistingSessions()
    {
        return Sessions;
    }
}