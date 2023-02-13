using CommandDotNet;
using CommandDotNet.Help;

using FsTag.Filters;
using FsTag.Helpers;

namespace FsTag;

[Command]
public partial class Program
{
    public static int Main(string[] args)
    {
        return MainMethod(args);
    }

    // This method exists purely for unit testing purposes
    public static int MainMethod(string[] args)
    {
        var runner = new AppRunner<Program>();

        return runner.Run(args);
    }
    
    [DefaultCommand]
    public int Execute(PathFilter filter, 
        [Option('r', "recursive")] bool isRecursive, 
        [Option("recurseDepth")] uint? recurseDepth = null)
    {
        return new TagCommand().Execute(filter, isRecursive, recurseDepth);
    }
}