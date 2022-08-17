using CommandDotNet;

namespace FsTag;

public partial class Program
{
    [Command("tag")]
    [Subcommand]
    public class TagCommand
    {
        [DefaultCommand]
        public int Execute(
            string filePath, 
            [Option('r', "recursive")] bool isRecursive, 
            [Option("recurseDepth")] int recurseDepth = -1)
        {
            if (!isRecursive && recurseDepth != -1)
            {
                WriteFormatter.Warning("recurseDepth is set, but recursion is not specified. Did you forget -r?");
            }

            var absolutePath = PathHelper.GetAbsolute(filePath);

            if (isRecursive)
            {
                if (Directory.Exists(absolutePath))
                {
                    var enumerable = FileSystemHelper.EnumerateFilesToDepth(absolutePath, recurseDepth);

                    return TagFileEnumerable(enumerable);
                }
                else
                {
                    WriteFormatter.Error($"The directory '{filePath}' does not exist.");
                
                    return 1;
                }
            }

            return TagFile(absolutePath);
        }

        private int TagFileEnumerable(IEnumerable<string> fileNames)
        {
            return ExceptionWrapper.TryExecute(() => AppData.IndexFiles(fileNames));
        }

        private int TagFile(string absolutePath)
        {
            return ExceptionWrapper.TryExecute(() => AppData.IndexFiles(new[] { absolutePath }));
        }
    }
}