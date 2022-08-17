using CommandDotNet;

namespace FsTag;

public partial class Program
{
    [Command("print")]
    [Subcommand]
    public class PrintCommand
    {
        [DefaultCommand]
        public int Execute([Option("delimiter")] string delimiter = ";")
        {
            foreach (var item in AppData.EnumerateIndex())
            {
                Console.Write(item);
                Console.Write(delimiter);
            }

            Console.WriteLine();

            return 0;
        }
    }
}