using FsTag.Data.Interfaces;
using FsTag.Data.Models;

using Markdig;
using Markdig.Extensions.Yaml;

using YamlDotNet.Serialization;

namespace FsTag.Data.Builtin;

public class DocumentationData : IDocumentationData
{
    private string DirectoryPath => Path.Join(Directory.GetCurrentDirectory(), "docs");

    public IEnumerable<DocumentationModule> GetModules()
    {
        var markdownPipeline = new MarkdownPipelineBuilder()
            .UseYamlFrontMatter()
            .EnableTrackTrivia()
            .Build();
        var yamlDeserializer = new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .Build();

        foreach (var file in Directory.EnumerateFiles(DirectoryPath, "*.md"))
        {
            var content = App.FileSystem.File.ReadAllText(file);
            var md = Markdown.Parse(content, markdownPipeline);

            var aliasNames = new List<string>
            {
                Path.GetFileNameWithoutExtension(file),
                Path.GetFileName(file)
            };

            var moduleContent = content;
            
            // Add any frontmatter aliases to our list, as well as trim it from the display text
            if (md.First() is YamlFrontMatterBlock yamlMetadata)
            {
                var yamlText = yamlMetadata.Lines.ToString();
                var metadata = yamlDeserializer.Deserialize<MarkdownMetadata>(yamlText);

                aliasNames.AddRange(metadata.Alias);
                
                moduleContent = moduleContent.Remove(yamlMetadata.Span.Start, yamlMetadata.Span.Length);
            }
                
            yield return new DocumentationModule(aliasNames.ToArray(), moduleContent);
        }
    }
    
    private class MarkdownMetadata
    {
        [YamlMember(Alias = "alias")] public string[] Alias { get; set; } = Array.Empty<string>();
    }
}