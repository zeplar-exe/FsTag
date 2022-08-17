using CommandDotNet;

namespace FsTag;

public partial class Program
{
    [Command("rm")]
    [Subcommand]
    public class RemoveCommand
    {
        [DefaultCommand]
        public int Execute(
            string? filePath,
            [Option('a', "all")] bool all,
            [Option('r', "recursive")] bool isRecursive,
            [Option("recurseDepth")] int recurseDepth = -1)
        {
            if (all)
            {
                if (filePath != null)
                {
                    WriteFormatter.Warning("filePath will be ignored because -a is specified.");
                }
                
                if (isRecursive)
                {
                    WriteFormatter.Warning("-r will be ignored because -a is specified.");
                }
                
                if (recurseDepth != -1)
                {
                    WriteFormatter.Warning("recurseDepth will be ignored because -a is specified.");
                }
                
                return ExceptionWrapper.TryExecute(AppData.ClearIndex);
            }
            
            if (!isRecursive && recurseDepth != -1)
            {
                WriteFormatter.Warning("recurseDepth is set, but recursion is not specified. Did you forget -r?");
            }
            
            if (filePath == null)
            {
                WriteFormatter.Error("Expected a filePath. Maybe you forgot -a?");
                
                return 1;
            }

            var absolutePath = PathHelper.GetAbsolute(filePath);

            IEnumerable<string> removeEnumerable = new[] { absolutePath };

            if (isRecursive)
            {
                if (!Directory.Exists(absolutePath))
                {
                    WriteFormatter.Error($"The directory '{absolutePath}' does not exist.");

                    return 1;
                }
                
                removeEnumerable = FileSystemHelper.EnumerateFilesToDepth(absolutePath, recurseDepth);
            }

            return ExceptionWrapper.TryExecute(() => AppData.RemoveFromIndex(removeEnumerable));
        }
    }
}