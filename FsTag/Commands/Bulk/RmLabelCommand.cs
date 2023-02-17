using CommandDotNet;

namespace FsTag;

public partial class Program
{
    public partial class BulkCommand
    {
        [LocalizedCommand("rmlabel", nameof(Descriptions.RmLabelCommand))]
        public class RmLabelCommand
        {
            [DefaultCommand]
            public int Execute(
                string label)
            {
                var labels = AppData.GetLabels();
                
                if (labels == null)
                    return 1;

                foreach (var file in AppData.EnumerateIndex())
                {
                    labels[file]?[label]?.Remove();
                }

                return 0;
            }

            [LocalizedCommand("all", nameof(Descriptions.RmLabelAllCommand))]
            public int All()
            {
                var labels = AppData.GetLabels();

                if (labels == null)
                    return 1;
                
                foreach (var file in AppData.EnumerateIndex())
                {
                    labels.Remove(file);
                }
                
                AppData.WriteLabels(labels);

                return 0;
            }
        }
    }
}