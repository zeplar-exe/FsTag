using CommandDotNet;

using Microsoft.VisualBasic.FileIO;

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
            public int Execute(
                [Option('r', "recycle")] bool recycle)
            {
                foreach (var file in AppData.EnumerateIndex())
                {
                    if (File.Exists(file))
                    {
                        if (recycle)
                        {
                            FileSystem.DeleteFile(file, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                        }
                        else
                        {
                            File.Delete(file);
                        
                            WriteFormatter.Info($"Deleted '{file}'.");
                        }
                    }
                    else
                    {
                        WriteFormatter.Warning($"The file '{file}' does not exist.");
                    }
                }
                
                AppData.ClearIndex();
                
                return 0;
            }
        }
    }
}