using FsTag.Data.Interfaces;
using FsTag.Helpers;
using FsTag.Resources;

using Newtonsoft.Json.Linq;

namespace FsTag.Data.Builtin;

public class ConfigData : IConfigData
{
    public Configuration? Read()
    {
        var parsedJson = DataFileHelper.ParseJson(BuiltinPaths.ConfigFilePath);

        return parsedJson?.ToObject<Configuration>();
    }

    public void SetProperty(string key, JToken? value)
    {
        var config = App.ConfigData.Read();
            
        if (config == null)
            return;

        config.Set(key, value);
        
        DataFileHelper.WriteJson(BuiltinPaths.ConfigFilePath, JObject.FromObject(config));
    }

    public bool RemoveProperty(string key)
    {
        var config = App.ConfigData.Read();
            
        if (config == null)
            return false;

        if (config.OtherProperties.TryGetValue(key, out var token))
        {
            token.Remove();
        }
        else
        {
            WriteFormatter.Error(string.Format(CommandOutput.ConfigExtraPropertyMissing, key));

            return false;
        }
        
        WriteFormatter.Info(string.Format(CommandOutput.ConfigRemove, key));
        DataFileHelper.WriteJson(BuiltinPaths.ConfigFilePath, JObject.FromObject(config));

        return true;
    }

    public void Clear()
    {
        DataFileHelper.WriteJson(BuiltinPaths.ConfigFilePath, new JObject());
    }
}