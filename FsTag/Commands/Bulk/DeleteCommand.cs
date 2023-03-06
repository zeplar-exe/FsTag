using CommandDotNet;

using FsTag.Attributes;
using FsTag.Data;
using FsTag.Helpers;
using FsTag.Resources;

namespace FsTag;

public partial class Program
{
    public partial class BulkCommand
    {
        [Command("delete", Description = nameof(Descriptions.BulkDeleteCommand))]
        [Subcommand]
        public class DeleteCommand
        {
            [DefaultCommand]
            public int Execute(
                [Option('r', "recycle", Description = nameof(Descriptions.DeleteRecycle))] bool recycle)
            {
                var files = AppData.FileIndex.EnumerateItems().ToArray();
                var sessionName = AppData.SessionData.CurrentSessionName;

                if (sessionName == null)
                    return 1;

                if (!Confirmation.Prompt(string.Format(ConfirmationText.BulkDelete, files.Length, sessionName)))
                    return 1;

                // Here to prevent removal of files which couldn't be deleted
                var removed = new List<string>();
                
                foreach (var file in files)
                {
                    if (AppData.FileSystem.FileExists(file))
                    {
                        try
                        {
                            if (recycle)
                            {
                                if (!DryRun)
                                    AppData.FileSystem.RecycleFile(file);
                            }
                            else
                            {
                                if (!DryRun)
                                    AppData.FileSystem.DeleteFile(file);

                                WriteFormatter.Info($"Deleted '{file}'.");
                            }
                            
                            removed.Add(file);
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

                AppData.FileIndex.Remove(removed);

                return 0;
            }
        }
    }
}