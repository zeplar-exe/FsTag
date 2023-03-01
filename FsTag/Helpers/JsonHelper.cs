using FsTag.Data;

using Newtonsoft.Json;

namespace FsTag.Helpers;

public class JsonHelper
{
    public static Formatting GetConfigJsonFormatting(Configuration wrapper)
    {
        return wrapper.FormatJsonOutput ? Formatting.Indented : Formatting.None;
    }
}