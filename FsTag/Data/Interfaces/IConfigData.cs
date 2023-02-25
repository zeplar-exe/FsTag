using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

namespace FsTag.Data.Interfaces;

public interface IConfigData
{
    public bool TryRead([NotNullWhen(true)] out ConfigJsonWrapper? json);
    public void SetProperty(string key, JToken? value);
    public bool RemoveProperty(string key);
    public void Clear();
}