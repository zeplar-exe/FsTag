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
                var files = App.FileIndex.EnumerateItems().ToArray();
                var sessionName = App.SessionData.CurrentSessionName;

                if (sessionName == null)
                    return 1;

                if (!Confirmation.Prompt(string.Format(ConfirmationText.BulkDelete, files.Length, sessionName)))
                    return 1;

                // Here to prevent removal of files which couldn't be deleted
                var removed = new List<string>();
                
                foreach (var file in files)
                {
                    if (App.FileSystem.FileExists(file))
                    {
                        try
                        {
                            if (recycle)
                            {
                                if (!DryRun)
                                    App.FileSystem.RecycleFile(file);
                            }
                            else
                            {
                                if (!DryRun)
                                    App.FileSystem.DeleteFile(file);

                                WriteFormatter.Info(string.Format(CommandOutput.BulkDeletedFile, file));
                            }
                            
                            removed.Add(file);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            WriteFormatter.Warning(string.Format(CommandOutput.BulkDeleteUnauthorized, file));
                        }
                    }
                    else
                    {
                        WriteFormatter.Warning(string.Format(CommandOutput.BulkDeleteFileMissing, file));
                    }
                }

                App.FileIndex.Remove(removed);

                return 0;
            }
        }
    }
}