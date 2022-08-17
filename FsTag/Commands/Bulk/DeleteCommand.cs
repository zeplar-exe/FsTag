using CommandDotNet;

namespace FsTag;

public partial class Program
{
    public partial class BulkCommand
    {
        [Command("delete")]
        [Subcommand]
        public class DeleteCommand
        {
            [DefaultCommand]
            public int Execute([Option('a', "all")] bool all)
            {
                if (all)
                {
                    foreach (var file in AppData.EnumerateIndex())
                    {
                        if (File.Exists(file))
                        {
                            File.Delete(file);
                            
                            WriteFormatter.Info($"Deleted '{file}'.");
                        }
                        else
                        {
                            WriteFormatter.Warning($"The file '{file}' does not exist.");
                        }
                    }
                    
                    AppData.ClearIndex();
                }
                
                return 0;
            }
        }
    }
}