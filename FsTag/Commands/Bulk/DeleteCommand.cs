using CommandDotNet;

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
        [Command("delete")]
        [Subcommand]
        public class DeleteCommand
        {
            [DefaultCommand]
            public int Execute(
                [LocalizedOption('r', "recycle", nameof(Descriptions.DeleteRecycle))]
                bool recycle)
            {
                if (DryRun)
                    goto SkipDeletion;

                var files = AppData.FileIndex.EnumerateItems();
                string currentSessionName = AppData.SessionData.CurrentSessionName ?? "";

                if (ConfirmDeletion(files.Count(), currentSessionName) == false)
                    goto SkipDeletion;

                foreach (var file in files)
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

            SkipDeletion:

                AppData.FileIndex.Clear();

                return 0;
            }
            private bool ConfirmDeletion(int filesCount, string sessionName)
            {
                WriteFormatter.Warning($"Are you sure you want to delete {filesCount} files in the {sessionName} session? (yes/no):");

                string input = Console.ReadLine()!;
                if (!string.IsNullOrEmpty(input))
                {
                    string lowerInput = input.ToLower();
                    if (lowerInput == "y" || lowerInput == "yes")
                        return true;
                }
                return false;
            }
        }
    }
}