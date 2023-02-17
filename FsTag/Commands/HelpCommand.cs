using CommandDotNet;

using FsTag.Helpers;

namespace FsTag;

public partial class Program
{
    [LocalizedCommand("docs", nameof(Descriptions.HelpCommand))]
    [Subcommand]
    public class DocsCommand
    {
        [DefaultCommand]
        public int Execute(string? module = null)
        {
            var modules = GetDocumentationModules().ToArray();

            if (module != null)
            {
                foreach (var helpModule in modules)
                {
                    if (helpModule.IsMatch(module))
                    {
                        WriteFormatter.Plain(helpModule.Content);

                        return 0;
                    }
                }

                return 1;
            }
            
            WriteFormatter.Plain("Loaded Documentation Modules:");

            foreach (var helpModule in modules)
            {
                WriteFormatter.PlainNoLine(helpModule.Name);
                WriteFormatter.PlainNoLine($" [ {helpModule.FileName} ]");
                
                WriteFormatter.NewLine();
            }

            return 0;
        }

        private IEnumerable<DocumentationModule> GetDocumentationModules()
        {
            var directory = Path.Join(Directory.GetCurrentDirectory(), "docs");

            foreach (var file in Directory.EnumerateFiles(directory, "*.md"))
            {
                yield return new DocumentationModule(
                    Path.GetFileNameWithoutExtension(file),  
                    Path.GetFileName(file), 
                    file);
            }
        }

        private record DocumentationModule(string Name, string FileName, string FilePath)
        {
            public string Content => File.ReadAllText(FilePath);
            
            public bool IsMatch(string name)
            {
                return name == Name || name == FileName;
            }
        }
    }
}