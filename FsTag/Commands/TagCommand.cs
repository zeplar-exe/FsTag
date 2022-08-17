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
            [Option("recurseDepth")] int? recurseDepth)
        {
            if (!isRecursive && recurseDepth != null)
            {
                WriteFormatter.Warning("recurseDepth is set, but recursion is not specified. Did you forget -r?");
            }



            var absolutePath = PathHelper.GetAbsolute(filePath);

            if (isRecursive)
            {
                if (Directory.Exists(absolutePath))
                {
                    if (recurseDepth != null)
                    {
                        var enumerable = EnumerateFilesToDepth(absolutePath, recurseDepth.Value);

                        return TagFileEnumerable(enumerable);
                    }
                    else
                    {
                        var enumerable = Directory.EnumerateFiles(absolutePath, "*", SearchOption.AllDirectories);

                        return TagFileEnumerable(enumerable);
                    }
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

        private IEnumerable<string> EnumerateFilesToDepth(string directory, int targetDepth)
        {
            if (targetDepth == 0)
                yield break;

            foreach (var file in EnumerateFilesToDepth(directory, targetDepth, 0))
            {
                yield return file;
            }
        }
        
        private IEnumerable<string> EnumerateFilesToDepth(string directory, int maxDepth, int depth)
        {
            foreach (var file in Directory.EnumerateFiles(directory))
            {
                yield return file;
            }
            
            if (depth != maxDepth)
            {
                foreach (var dir in Directory.EnumerateDirectories(directory))
                {
                    foreach (var file in EnumerateFilesToDepth(dir, maxDepth, depth + 1))
                    {
                        yield return file;
                    }
                }
            }
        }
    }
}