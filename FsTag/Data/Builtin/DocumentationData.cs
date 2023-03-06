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
        var yamlDeserializer = new DeserializerBuilder().Build();

        foreach (var file in Directory.EnumerateFiles(DirectoryPath, "*.md"))
        {
            var textOperation = AppData.FileSystem.ReadText(file);
            
            if (!textOperation.Success)
                continue;
            
            var md = Markdown.Parse(textOperation.Result, markdownPipeline);

            var names = new List<string>
            {
                Path.GetFileNameWithoutExtension(file),
                Path.GetFileName(file)
            };

            var moduleContent = textOperation.Result;
            
            if (md.First() is YamlFrontMatterBlock yamlMetadata)
            {
                var yamlText = yamlMetadata.Lines.ToString();
                var metadata = yamlDeserializer.Deserialize<MarkdownMetadata>(yamlText);

                names.AddRange(metadata.Alias);
                
                moduleContent = moduleContent.Remove(0, yamlMetadata.Span.Length);
            }
                
            yield return new DocumentationModule(names.ToArray(), moduleContent);
        }
    }
    
    private class MarkdownMetadata
    {
        [YamlMember(Alias = "alias")] public string[] Alias { get; set; } = Array.Empty<string>();
    }
}