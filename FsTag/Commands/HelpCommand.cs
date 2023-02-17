using CommandDotNet;

using FsTag.Helpers;

namespace FsTag;

public partial class Program
{
    [LocalizedCommand("help", nameof(Descriptions.HelpCommand))]
    [Subcommand]
    public class HelpCommand
    {
        [DefaultCommand]
        public int Execute()
        {
            WriteFormatter.Plain("Loaded Help Modules:");

            foreach (var module in GetHelpModules())
            {
                WriteFormatter.PlainNoLine(module.Name);
                WriteFormatter.PlainNoLine($"[ {module.FileName} ]");
                
                WriteFormatter.NewLine();
            }

            return 0;
        }
        
        [Command("filters")]
        public int Filters()
        {


            return 0;
        }

        private IEnumerable<HelpModule> GetHelpModules()
        {
            var directory = Path.Join(Directory.GetCurrentDirectory(), "docs");

            foreach (var file in Directory.EnumerateFiles(directory, "*.md"))
            {
                yield return new HelpModule(Path.GetFileNameWithoutExtension(file),  Path.GetFileName(file));
            }
        }

        private record HelpModule(string Name, string FileName);
    }
}