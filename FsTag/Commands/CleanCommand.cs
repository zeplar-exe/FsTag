using CommandDotNet;

using FsTag.Data;

namespace FsTag;

public partial class Program
{
    [Command("clean", Description = "Cleans the index of any files that do not exist.")]
    [Subcommand]
    public class CleanCommand
    {
        [DefaultCommand]
        public int Execute()
        {
            var removed = AppData.EnumerateIndex().Where(tag => !File.Exists(tag));
            AppData.RemoveFromIndex(removed);

            var labels = AppData.GetLabels();

            if (labels != null)
            {
                foreach (var item in labels)
                {
                    if (!File.Exists(item.Key))
                        labels.Remove(item.Key);
                }
                
                AppData.WriteLabels(labels);
            }

            return 0;
        }
    }
}