using CommandDotNet;

namespace FsTag;

[Command]
public partial class Program
{
    public static int Main(string[] args)
    {
        return new AppRunner<Program>().Run(args);
    }
}