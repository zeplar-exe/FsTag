using CommandDotNet;

using FsTag.Data;
using FsTag.Data.Builtin;
using FsTag.Data.Models;
using FsTag.Helpers;
using FsTag.Resources;

using Markdig;
using Markdig.Extensions.Yaml;

using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

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

                        return 0;
                    }
                }
            }

            return 1;
        }
    }
}