using FsTag.Data;
using FsTag.Data.Interfaces;

using Newtonsoft.Json.Linq;

namespace FsTag.Tests.Unit.Mocks;

public class MockConfig : IConfigData
{
    private Configuration Configuration { get; set; }

    public MockConfig()
    {
        Configuration = new Configuration();
    }
    
    public Configuration Read()
    {
        return Configuration;
    }

    public void SetProperty(string key, JToken? value)
    {
        Configuration.Set(key, value);
    }

    public bool RemoveProperty(string key)
    {
        return false;
    }

    public void Clear()
    {
        Configuration = new Configuration();
    }
}