using System.Diagnostics.CodeAnalysis;

using FsTag.Helpers;

using Newtonsoft.Json.Linq;

namespace FsTag.Data.Builtin;

public class ConfigData : IConfigData
{
    public bool TryRead([NotNullWhen(true)] out ConfigJsonWrapper? json)
    {
        json = null;
        
        var parsedJson = DataFileHelper.ParseJson(StaticPaths.ConfigFilePath);

        if (parsedJson == null)
            return false;
        
        json = new ConfigJsonWrapper(parsedJson);

        return true;
    }

    public void SetProperty(string key, JToken? value)
    {
        if (!TryRead(out var json))
            return;

        json.Add(key, value);
        
        DataFileHelper.WriteJson(StaticPaths.ConfigFilePath, json);
    }

    public bool RemoveProperty(string key)
    {
        if (!TryRead(out var json))
            return false;

        if (json.TryGetValue(key, out var token))
        {
            token.Remove();
        }
        else
        {
            WriteFormatter.Error($"The config property '{key}' does not exist.");

            return false;
        }
        
        WriteFormatter.Info($"Removed config property '{key}'.");
        DataFileHelper.WriteJson(StaticPaths.ConfigFilePath, json);

        return true;
    }

    public void Clear()
    {
        DataFileHelper.WriteJson(StaticPaths.ConfigFilePath, new JObject());
    }
}