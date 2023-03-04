using CommandDotNet;
using CommandDotNet.Prompts;

using FsTag.Attributes;
using FsTag.Data;
using FsTag.Helpers;
using FsTag.Resources;

using Microsoft.VisualBasic.FileIO;

namespace FsTag;

public partial class Program
{
    public partial class BulkCommand
    {
        [LocalizedCommand("delete", nameof(Descriptions.BulkDeleteCommand))]
        [Subcommand]
        public class DeleteCommand
        {
            [DefaultCommand]
            public int Execute(
                [LocalizedOption('r', "recycle", nameof(Descriptions.DeleteRecycle))]
                bool recycle)
            {
                var files = AppData.FileIndex.EnumerateItems().ToArray();
                var sessionName = AppData.SessionData.CurrentSessionName;

                if (sessionName == null)
                    return 1;

                if (!Confirmation.Prompt(string.Format(ConfirmationText.BulkDelete, files.Length, sessionName)))
                    return 1;

                foreach (var file in files)
                {
                    if (File.Exists(file))
                    {
                        try
                        {
                            if (recycle)
                            {
                                if (!DryRun)
                                    FileSystem.DeleteFile(file, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            }
                            else
                            {
                                if (!DryRun)
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
}