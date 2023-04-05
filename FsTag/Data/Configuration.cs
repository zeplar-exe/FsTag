using System.Reflection;

using FsTag.Data.Builtin;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FsTag.Data;

public class Configuration
{
    private static Dictionary<string, PropertyInfo> JsonBindings { get; } = new(); 
    // For Set() and Get() on arbitrary property names

    // All exposed JSON properties here should have snake_case JSON names and UpperCamelCase identifier names
    
    [JsonProperty("format_json_output")] public bool FormatJsonOutput { get; set; }

    [JsonExtensionData] public JObject OtherProperties { get; set; } = new();

    static Configuration()
    {
        foreach (var property in typeof(Configuration).GetProperties())
        {
            var attribute = property.GetCustomAttribute<JsonPropertyAttribute>();
            
            if (attribute == null)
                continue;

            JsonBindings[attribute.PropertyName ?? property.Name] = property;
        }
    }
    
    public void Set(string name, JToken value)
    {
        if (JsonBindings.TryGetValue(name, out var property))
            property.SetValue(this, value.ToObject(property.PropertyType));
        else
            OtherProperties[name] = value;
    }

    /// <summary>
    /// Attempt to get the specified JSON property of any type.
    /// </summary>
    /// <param name="name">The name to get.</param>
    /// <param name="value">The stored value.</param>
    /// <returns>Whether the JSON property exists.</returns>
    public bool TryGet(string name, out object? value) => TryGet<object>(name, out value);

    /// <summary>
    /// Attempt to get the specified JSON property in this configuration object.
    /// </summary>
    /// <param name="name">The name to get.</param>
    /// <param name="value">The stored value.</param>
    /// <typeparam name="T">The target type to get. This is passed to <see cref="JToken.Value{T}"/>,
    /// thus, types JToken will get the token itself, and other types will cause an attempted
    /// deserialization.</typeparam>
    /// <returns>Whether the JSON property exists.</returns>
    public bool TryGet<T>(string name, out T? value)
    {
        value = default;
        
        if (JsonBindings.TryGetValue(name, out var property))
        {
            value = (T)property.GetValue(this)!;

            return true;
        }

        if (OtherProperties.TryGetValue(name, out var token))
        {
            value = token.Value<T>();

            return true;
        }

        return false;
    }
}