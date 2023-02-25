using CommandDotNet;

using FsTag.Data;
using FsTag.Helpers;
using FsTag.Resources;

using Newtonsoft.Json.Linq;

namespace FsTag;

public partial class Program
{
    public partial class BulkCommand
    {
        [LocalizedCommand("label", nameof(Descriptions.LabelCommand))]
        [Subcommand]
        public class LabelCommand
        {
            [DefaultCommand]
            public int Execute(
                string label,
                string value,
                [LocalizedOption('n', "new", nameof(Descriptions.LabelNewOp))]
                bool newOnly)
            {
                var labels = AppData.GetLabels();

                if (labels == null)
                    return 1;
                
                foreach (var file in AppData.EnumerateIndex())
                {
                    if (labels.TryGetValue(file, out var fileData))
                    {
                        if (fileData is not JObject fileDataObject)
                        {
                            WriteFormatter.Error($"Failed to update label '{label}' for {file} " +
                                                 $"because the existing data is a {fileData.GetType().Name}, " +
                                                 $"instead of a JSON object.");
                            
                            continue;
                        }
                        
                        if (fileDataObject.ContainsKey(label) && newOnly)
                            continue;

                        fileDataObject[label] = value;
                    }
                    else
                    {
                        labels[file] = new JObject
                        {
                            {label, value}
                        };
                    }
                }
                
                AppData.WriteLabels(labels);

                return 0;
            }
        }
    }
}