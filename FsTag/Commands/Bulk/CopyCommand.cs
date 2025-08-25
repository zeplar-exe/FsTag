using CommandDotNet;

using FsTag.Attributes;
using FsTag.Data;
using FsTag.Helpers;
using FsTag.Resources;

using Microsoft.VisualBasic.FileIO;

using FileSystem = Microsoft.VisualBasic.FileIO.FileSystem;

namespace FsTag;

public partial class Program
{
    public partial class BulkCommand
    {
        [Command("copy", Description = nameof(Descriptions.BulkCopyCommand))]
        [Subcommand]
        public class CopyCommand
        {
            [DefaultCommand]
            public int Execute(
                [Operand("targetDir", Description = nameof(Descriptions.CopyTargetDir))] string targetDir,
                [Option('r', "retain", Description = nameof(Descriptions.CopyRetain))] bool retainStructure)
            {
                var files = App.FileIndex.EnumerateItems().ToArray();
                var sessionName = App.SessionData.CurrentSessionName;

                if (sessionName == null)
                    return 1;

                if (!Confirmation.Prompt(string.Format(ConfirmationText.BulkCopy, files.Length, sessionName)))
                    return 1;

                var copied = new List<string>();

                if (retainStructure)
                {
                    var dirRoot = files.Length > 0 
                        ? Path.GetDirectoryName(files[0]) ?? string.Empty 
                        : string.Empty;

                    foreach (var file in files.Skip(1))
                    {
                        var currentDir = Path.GetDirectoryName(file) ?? string.Empty;
                        
                        while (!currentDir.StartsWith(dirRoot, StringComparison.OrdinalIgnoreCase))
                        {
                            dirRoot = Path.GetDirectoryName(dirRoot) ?? string.Empty;
                            
                            if (string.IsNullOrEmpty(dirRoot)) 
                                break;
                        }
                    }

                    var depth = dirRoot.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Length;

                    if (!Confirmation.Prompt(string.Format(ConfirmationText.RetainStructureDepth, depth)))
                        return 1;

                    foreach (var path in files)
                    {
                        var remnant = Path.GetRelativePath(dirRoot, path);

                        try
                        {
                            Directory.CreateDirectory(Path.Join(targetDir, Path.GetDirectoryName(remnant)));

                            FileSystem.CopyFile(path, Path.Join(targetDir, remnant));

                            copied.Add(path);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            WriteFormatter.Warning(string.Format(CommandOutput.BulkCopyUnauthorized, path));
                        }
                    }
                }
                else
                {
                    foreach (var path in files)
                    {
                        var file = Path.GetFileName(path);

                        try
                        {
                            FileSystem.CopyFile(path, Path.Join(targetDir, file));

                            copied.Add(path);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            WriteFormatter.Warning(string.Format(CommandOutput.BulkCopyUnauthorized, path));
                        }
                    }
                }

                WriteFormatter.Info(string.Format(CommandOutput.BulkCopyCompleted, copied.Count, files.Length));

                return 0;
            }
        }
    }
}