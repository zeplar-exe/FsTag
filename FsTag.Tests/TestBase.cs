namespace FsTag.Tests;

public abstract class TestBase
{
    [SetUp]
    public void SetUp()
    {
        Executable.Setup();
    }

    [TearDown]
    public void TearDown()
    {
        Executable.Teardown();
    }
}