using System.Runtime.CompilerServices;

using CommandDotNet;

using FsTag.Attributes;
using FsTag.Filters;
using FsTag.Helpers;
using FsTag.Resources;

[assembly: InternalsVisibleTo("FsTag.Tests")]
namespace FsTag;

[Command]
public partial class Program
{
    /// <summary>
    /// A CommandDotNet-injected IConsole, should be used over Console.WriteXXX.
    /// </summary>
    public static IConsole IConsole { get; set; }
    public static bool Quiet { get; set; }
    public static bool NoPrompt { get; set; }
    public static bool DryRun { get; set; }
    
    public static int Main(string[] args)
    {
        Runner.AppSettings.Localization.Localize = s => Descriptions.ResourceManager.GetString(s);
        
        return Runner.Run(args);
    }

    public static AppRunner<Program> Runner { get; } = new();

    // https://commanddotnet.bilal-fazlani.com/extensibility/interceptors/
    public Task<int> Interceptor(InterceptorExecutionDelegate next, 
        IConsole console,
        [Option('q', "quiet", Description = nameof(Descriptions.QuietOption),
            AssignToExecutableSubcommands = true)] bool quiet,
        [Option("no-prompt", Description = nameof(Descriptions.NoPromptOption),
            AssignToExecutableSubcommands = true)] bool noPrompt,
        [Option("dryrun", Description = nameof(Descriptions.DryrunOption), 
            AssignToExecutableSubcommands = true)] bool dryRun)
    {
        IConsole = console;
        Quiet = quiet;
        NoPrompt = noPrompt;
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
    public int Execute(
        [PathFilterOperand] PathFilter filter, 
        [RecurseOption] int recurseDepth = 0)
    {
        return new TagCommand().Execute(filter, recurseDepth);
    }
}