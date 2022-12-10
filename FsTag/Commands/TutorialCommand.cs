using CommandDotNet;

using FsTag.Extensions;

namespace FsTag;

public partial class Program
{
    private const string TestDirectoryName = "fstag_test";
    
    [Command("tutorial")]
    [Subcommand]
    public class TutorialCommand
    {
        [DefaultCommand]
        public int Interactive()
        {
            WriteFormatter.Plain("Nothing yet.");
            
            return 0;
        }

        [Command("init")]
        public int Init([Option('f', "force")] bool force = false)
        {
            var root = Path.Join(Directory.GetCurrentDirectory(), TestDirectoryName);

            if (Directory.Exists(root))
            {
                if (!force)
                {
                    WriteFormatter.Error($"The {TestDirectoryName} directory already exists in the current directory. " +
                                         "Did you mean to use -f?");
                    
                    return 1;
                }
                
                Directory.Delete(root, true);
            }

            var directory = Directory.CreateDirectory(root);

            directory.CreateFile("test.txt");
            directory.CreateFile("test1.txt");
            directory.CreateFile("test2.txt");
            directory.CreateFile("test3.txt");

            var aDir = directory.CreateSubdirectory("A");

            aDir.CreateFile("A_file_of_the_will");
            aDir.CreateFile("A_pizza");
            aDir.CreateFile("A.pdf");
            aDir.CreateFile("Apothecary.txt");

            var underDir = directory.CreateSubdirectory("_under");

            underDir.CreateFile("_abc_");
            underDir.CreateFile("_abc_123_.txt");
            underDir.CreateFile("abc_123.jpg");
            
            var realUnderDir = directory.CreateSubdirectory("realunder");
            
            realUnderDir.CreateFile("_abc_");
            realUnderDir.CreateFile("_abc_123_.txt");
            realUnderDir.CreateFile("abc_123.jpg");
            
            WriteFormatter.Info($"Created {TestDirectoryName} in the current directory.");

            return 0;
        }

        [Command("deinit")]
        public int Deinit()
        {
            var path = Path.Join(Directory.GetCurrentDirectory(), TestDirectoryName);

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                
                WriteFormatter.Info($"Deleted {TestDirectoryName} in the current directory.");
            }
            else
            {
                WriteFormatter.Info($"The {TestDirectoryName} directory does not exist.");
            }

            return 0;
        }
    }
}