using CommandDotNet;

using FsTag.Attributes;
using FsTag.Filters;
using FsTag.Helpers;

namespace FsTag;

[Command]
public partial class Program
{
    /// <summary>
    /// A CommandDotNet-injected IConsole, should be used over Console.WriteXXX.
    /// </summary>
    public static IConsole IConsole { get; set; }
    public static bool Verbose { get; set; }
    public static bool Quiet { get; set; }
    public static bool DryRun { get; set; }
    
    public static int Main(string[] args)
    {
        return Runner.Run(args);
    }

    public static AppRunner<Program> Runner { get; } = new();

    // https://commanddotnet.bilal-fazlani.com/extensibility/interceptors/
    public Task<int> Interceptor(InterceptorExecutionDelegate next, 
        IConsole console,
        [Option('q', "quiet", AssignToExecutableSubcommands = true)] bool quiet,
        [Option('v', "verbose", AssignToExecutableSubcommands = true)] bool verbose,
        [Option("dryrun", AssignToExecutableSubcommands = true)] bool dryRun)
    {
        IConsole = console;
        Quiet = quiet;
        Verbose = verbose;
        DryRun = dryRun;
        
        try
        {
            return next.Invoke();
        }
        catch (Exception e)
        {
            // Anonymous methods like above usually don't return the exception directly
            e = e.InnerException ?? e;

            WriteFormatter.Error(e.Message);

            return Task.FromResult(1);
        }
    }
    
    [DefaultCommand]
    public int Execute(PathFilter filter, 
        [RecurseOption] uint recurseDepth = 0)
    {
        return new TagCommand().Execute(filter, recurseDepth);
    }
}