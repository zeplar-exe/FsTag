using FsTag.Data;
using FsTag.Tests.Extensions;

using NUnit.Framework;

namespace FsTag.Tests.Unit;

[TestFixture]
public class Session : UnitTestBase
{
    [Test]
    public void TestSwitch()
    {
        Program.Runner.VerifyExitCode(0, "session", "switch", "abc");
        Assert.Multiple(() =>
        {
            Assert.That(App.SessionData.CurrentSessionName, Is.EqualTo("abc"));
            Assert.That(App.SessionData.GetExistingSessions(), Does.Contain("abc"));
        });
    }

    [Test]
    public void TestRemove()
    {
        Program.Runner.VerifyExitCode(0, "session", "switch", "abc");
        Program.Runner.VerifyExitCode(0, "session", "switch", "abc2");
        Program.Runner.VerifyExitCode(0, "session", "rm", "abc");
        Assert.Multiple(() =>
        {
            Assert.That(App.SessionData.CurrentSessionName, Is.EqualTo("abc2"));
            Assert.That(App.SessionData.GetExistingSessions(), Does.Contain("abc2"));
            Assert.That(App.SessionData.GetExistingSessions(), Does.Not.Contain("abc"));
        });
    }

    [Test]
    public void TestRemoveCurrentInvalid()
    {
        Program.Runner.VerifyExitCode(0, "session", "switch", "abc");
        Program.Runner.VerifyExitCode(1, "session", "rm", "abc");
        Assert.Multiple(() =>
        {
            Assert.That(App.SessionData.CurrentSessionName, Is.EqualTo("abc"));
            Assert.That(App.SessionData.GetExistingSessions(), Does.Contain("abc"));
        });
    }

    [Test]
    public void TestRemoveNonExistent()
    {
        Program.Runner.VerifyExitCode(1, "session", "rm", "DAJAJ");
    }
}