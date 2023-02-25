using FsTag.Data.Builtin;

using Newtonsoft.Json.Linq;

namespace FsTag.Data;

public sealed class ConfigJsonWrapper : JObject
{
    public bool FormatJsonOutput => GetOrDefault<bool>("format_json_output", new JValue(false));
    public string? SessionName => GetOrDefault<string>("session_name", new JValue(Constants.DefaultSessionName));

    public ConfigJsonWrapper(JObject original)
    {
        foreach (var item in original)
        {
            this[item.Key] = item.Value;
        }
    }
    
    private T? GetOrDefault<T>(string key, JToken defaultValue)
    {
        if (TryGetValue(key, out var value))
            return value.Value<T>();
        
        AppData.ConfigData.SetProperty(key, defaultValue);

        return defaultValue.Value<T>();
    }
}