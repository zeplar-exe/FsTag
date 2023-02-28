using CommandDotNet;

using FsTag.Attributes;
using FsTag.Data;
using FsTag.Helpers;
using FsTag.Resources;

namespace FsTag;

public partial class Program
{
    [LocalizedCommand("docs", nameof(Descriptions.DocsCommand))]
    [Subcommand]
    public class DocsCommand
    {
        [DefaultCommand]
        public int Execute(string[]? modules = null)
        {
            if (modules == null)
            {
                WriteFormatter.NewLine();
                WriteFormatter.Plain(CommonOutput.ValidArgumentList);
                WriteFormatter.NewLine();

                foreach (var helpModule in AppData.DocumentationData.GetModules())
                {
                    WriteFormatter.Plain(string.Join(" | ", helpModule.Names));
                }
                
                WriteFormatter.NewLine();

                return 0;
            }

            foreach (var module in modules)
            {
                foreach (var helpModule in AppData.DocumentationData.GetModules())
                {
                    if (helpModule.IsMatch(module))
                    {
                        WriteFormatter.Plain(helpModule.Content);
                        WriteFormatter.NewLine();

                        return 0;
                    }
                }
            }

            return 1;
        }
    }
}