using Newtonsoft.Json.Linq;

namespace FsTag.Data.Interfaces;

public interface IConfigData
{
    public Configuration? Read();
    public void SetProperty(string key, JToken? value);
    public bool RemoveProperty(string key);
    public void Clear();
}