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
        [Command("move", Description = nameof(Descriptions.BulkMoveCommand))]
        [Subcommand]
        public class MoveCommand
        {
            [DefaultCommand]
            public int Execute(
                [Operand("targetDir", Description = nameof(Descriptions.MoveTargetDir))] string targetDir,
                [Option('r', "retain", Description = nameof(Descriptions.MoveRetain))] bool retainStructure)
            {
                var files = App.FileIndex.EnumerateItems().ToArray();
                var sessionName = App.SessionData.CurrentSessionName;

                if (sessionName == null)
                    return 1;

                if (!Confirmation.Prompt(string.Format(ConfirmationText.BulkMove, files.Length, sessionName)))
                    return 1;

                var moved = new List<string>();

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
                        
                            FileSystem.MoveFile(path, Path.Join(targetDir, remnant));
                            
                            moved.Add(path);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            WriteFormatter.Warning(string.Format(CommandOutput.BulkMoveUnauthorized, path));
                            
                            moved.Add(path);
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
                            FileSystem.MoveFile(path, Path.Join(targetDir, file));
                        }
                        catch (UnauthorizedAccessException)
                        {
                            WriteFormatter.Warning(string.Format(CommandOutput.BulkMoveUnauthorized, path));
                        }
                    }
                }
                
                WriteFormatter.Info(string.Format(CommandOutput.BulkMoveCompleted, moved.Count, files.Length));

                return 0;
            }
        }
    }
}