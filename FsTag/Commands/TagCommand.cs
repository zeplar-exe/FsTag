using CommandDotNet;

using FsTag.Common;

namespace FsTag;

public partial class Program
{
    [Command("tag")]
    [Subcommand]
    public class TagCommand
    {
        [DefaultCommand]
        public int Execute(
            PathFilter filter, 
            [Option('r', "recursive")] bool isRecursive, 
            [Option("recurseDepth")] int recurseDepth = -1)
        {
            CommonOutput.WarnIfRecurseDepthWithoutRecursion(isRecursive, recurseDepth);

            IEnumerable<string>? fullInput = null;

            return FilterHelper.ExecuteOnFilterItems(filter, isRecursive, recurseDepth, AppData.IndexFiles);
            
            if (isRecursive)
            {
                foreach (var file in filter.EnumerateFiles())
                {
                    if (Directory.Exists(file))
                    {
                        var enumerable = FileSystemHelper.EnumerateFilesToDepth(file, recurseDepth);
                        
                        if (fullInput == null)
                        {
                            fullInput = enumerable;
                        }
                        else
                        {
                            fullInput = fullInput.Concat(enumerable);
                        }
                    }
                    else
                    {
                        WriteFormatter.Warning($"The directory '{file}' does not exist.");
                    }
                }
            }

            return TagFileEnumerable(fullInput ?? filter.EnumerateFiles());
        }

        private int TagFileEnumerable(IEnumerable<string> fileNames)
        {
            return ExceptionWrapper.TryExecute(() => AppData.IndexFiles(fileNames));
        }
    }
}