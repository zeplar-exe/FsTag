using System.Diagnostics.CodeAnalysis;

using FsTag.Data.Interfaces;
using FsTag.Helpers;

using Newtonsoft.Json.Linq;

namespace FsTag.Data.Builtin;

public class ConfigData : IConfigData
{
    public bool TryRead([NotNullWhen(true)] out Configuration? json)
    {
        json = null;
        
        var parsedJson = DataFileHelper.ParseJson(BuiltinPaths.ConfigFilePath);

        if (parsedJson == null)
            return false;

        json = parsedJson.ToObject<Configuration>() ?? new Configuration();
        // If null for whatever reason, return the default

        return true;
    }

    public void SetProperty(string key, JToken? value)
    {
        if (!TryRead(out var config))
            return;

        config.Set(key, value);
        
        DataFileHelper.WriteJson(BuiltinPaths.ConfigFilePath, JObject.FromObject(config));
    }

    public bool RemoveProperty(string key)
    {
        if (!TryRead(out var config))
            return false;

        if (config.TryGet<object>(key, out var token))
        {
            config.OtherProperties.Remove();
        }
        else
        {
            WriteFormatter.Error($"The extra config property '{key}' does not exist.");

            return false;
        }
        
        WriteFormatter.Info($"Removed config property '{key}'.");
        DataFileHelper.WriteJson(BuiltinPaths.ConfigFilePath, JObject.FromObject(config));

        return true;
    }

    public void Clear()
    {
        DataFileHelper.WriteJson(BuiltinPaths.ConfigFilePath, new JObject());
    }
}