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
            [Option('a', "all")] bool all)
        {
            if (all)
            {
                if (filePath != null)
                {
                    WriteFormatter.Warning("The filePath will be ignored because -a is specified.");
                }
                
                return ExceptionWrapper.TryExecute(AppData.ClearIndex);
            }

            if (filePath == null)
            {
                WriteFormatter.Error("Expected a filePath. Maybe you forgot -a?");
                
                return 1;
            }

            var absolutePath = PathHelper.GetAbsolute(filePath);

            return ExceptionWrapper.TryExecute(() => AppData.RemoveFromIndex(absolutePath));
        }
    }
}