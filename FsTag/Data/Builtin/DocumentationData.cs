using FsTag.Data.Interfaces;
using FsTag.Data.Models;

using Markdig;
using Markdig.Extensions.Yaml;

using YamlDotNet.Serialization;

namespace FsTag.Data.Builtin;

public class DocumentationData : IDocumentationData
{
    public string DirectoryPath => Path.Join(Directory.GetCurrentDirectory(), "docs");

    public IEnumerable<DocumentationModule> GetModules()
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
    
    private class MarkdownMetadata
    {
        [YamlMember(Alias = "alias")] public string[] Alias { get; set; } = Array.Empty<string>();
    }
}