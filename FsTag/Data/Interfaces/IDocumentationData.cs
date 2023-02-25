using FsTag.Data.Models;

namespace FsTag.Data.Interfaces;

public interface IDocumentationData
{
    public IEnumerable<DocumentationModule> GetModules();
}