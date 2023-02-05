using CommandDotNet;

using FsTag.Helpers;

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
                return ExceptionWrapper.TryExecute(() =>
                {
                    foreach (var file in AppData.EnumerateIndex())
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
                
                    AppData.ClearIndex();
                });
            }
        }
    }
}