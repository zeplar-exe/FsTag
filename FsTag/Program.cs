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
        var runner = new AppRunner<Program>();
        
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
        [LocalizedOption('r', "recursive", nameof(Descriptions.RecursiveOp))] 
        uint recurseDepth = 0)
    {
        return new TagCommand().Execute(filter, recurseDepth);
    }
}