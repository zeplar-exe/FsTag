using CommandDotNet;

namespace FsTag;

[Command]
public partial class Program
{
    public static int Main(string[] args)
    {
        var runner = new AppRunner<Program>();

        return runner.Run(args);
    }
}