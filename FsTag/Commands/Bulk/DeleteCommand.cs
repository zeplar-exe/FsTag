using CommandDotNet;

using FsTag.Data;
using FsTag.Helpers;
using FsTag.Resources;

using Microsoft.VisualBasic.FileIO;

namespace FsTag;

public partial class BulkCommand
{
    [Command("delete")]
    [Subcommand]
    public class DeleteCommand
    {
        [DefaultCommand]
        public int Execute(
            [LocalizedOption('r', "recycle", nameof(Descriptions.DeleteRecycle))] 
            bool recycle)
        {
            foreach (var file in AppData.FileIndex.EnumerateItems())
            {
                if (File.Exists(file))
                {
                    try
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
                    catch (UnauthorizedAccessException)
                    {
                        WriteFormatter.Warning($"Not authorized to delete '{file}', skipping.");
                    }
                }
                else
                {
                    WriteFormatter.Warning($"The file '{file}' does not exist.");
                }
            }
        
            AppData.FileIndex.Clear();
            
            return 0;
        }
    }
}