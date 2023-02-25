using FsTag.Data.Models;

namespace FsTag.Data.Interfaces;

public interface IDocumentationData
{
    public string DirectoryPath { get; }
    public IEnumerable<DocumentationModule> GetModules();
}