using CommandDotNet;

using FsTag.Data;
using FsTag.Data.Builtin;
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

                foreach (var helpModule in GetDocumentationModules())
                {
                    WriteFormatter.Plain(string.Join(" | ", helpModule.Names));
                }
                
                WriteFormatter.NewLine();

                return 0;
            }

            foreach (var module in modules)
            {
                foreach (var helpModule in GetDocumentationModules())
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

        private IEnumerable<DocumentationModule> GetDocumentationModules()
        {
            var directory = AppData.FilePaths.DocsDirectory;
            
            var markdownPipeline = new MarkdownPipelineBuilder()
                .UseYamlFrontMatter()
                .EnableTrackTrivia()
                .Build();
            var yamlDeserializer = new DeserializerBuilder().Build();

            foreach (var file in Directory.EnumerateFiles(directory, "*.md"))
            {
                var text = File.ReadAllText(file);
                var md = Markdown.Parse(text, markdownPipeline);

                var names = new List<string>
                {
                    Path.GetFileNameWithoutExtension(file),
                    Path.GetFileName(file)
                };

                if (md.First() is YamlFrontMatterBlock yamlMetadata)
                {
                    var yamlText = yamlMetadata.Lines.ToString();
                    var metadata = yamlDeserializer.Deserialize<MarkdownMetadata>(yamlText);

                    names.AddRange(metadata.Alias);
                }
                
                yield return new DocumentationModule(names.ToArray(), file);
            }
        }

        private record DocumentationModule(string[] Names, string FilePath)
        {
            public string Content => File.ReadAllText(FilePath);
            
            public bool IsMatch(string name)
            {
                return Names.Contains(name);
            }
        }

        private class MarkdownMetadata
        {
            [YamlMember(Alias = "alias")] public string[] Alias { get; set; } = Array.Empty<string>();
        }
    }
}