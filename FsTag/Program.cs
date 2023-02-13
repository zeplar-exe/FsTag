using CommandDotNet;
using CommandDotNet.Help;

using FsTag.Filters;
using FsTag.Helpers;

namespace FsTag;

[Command]
public partial class Program
{
    public static bool Verbose { get; set; }
    public static bool Quiet { get; set; }
    
    public static int Main(string[] args)
    {
        return MainMethod(args);
    }

    // This method exists purely for unit testing purposes
    public static int MainMethod(string[] args)
    {
        var settings = new AppSettings
        {
            Localize = key => key
        };
        
        var runner = new AppRunner<Program>(settings);

        return runner.Run(args);
    }
    
    // https://commanddotnet.bilal-fazlani.com/extensibility/interceptors/
    public Task<int> Interceptor(InterceptorExecutionDelegate next, 
        [Option('q', "quiet", AssignToExecutableSubcommands = true)] bool quiet,
        [Option('v', "verbose", AssignToExecutableSubcommands = true)] bool verbose)
    {
        Quiet = quiet;
        Verbose = verbose;
        
        try
        {
            return next.Invoke();
        }
        catch (Exception e)
        {
            e = e.InnerException ?? e;
            // Anonymous methods like above usually don't return the exception directly
            
            WriteFormatter.Error(e.Message);

            return Task.FromResult(1);
        }
    }
    
    [DefaultCommand]
    public int Execute(PathFilter filter, 
        [Option('r', "recursive")] bool isRecursive, 
        [Option("recurseDepth")] uint? recurseDepth = null)
    {
        return new TagCommand().Execute(filter, isRecursive, recurseDepth);
    }
}