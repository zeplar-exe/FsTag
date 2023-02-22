using CommandDotNet;

using FsTag.Helpers;

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
                
                WriteFormatter.Plain("Append one or more of the following as the arguments to this " +
                                     "command in order to see their contents:");
                
                WriteFormatter.NewLine();

                foreach (var helpModule in GetDocumentationModules())
                {
                    WriteFormatter.PlainNoLine(helpModule.Name);
                    WriteFormatter.PlainNoLine($" [ {helpModule.FileName} ]");
                
                    WriteFormatter.NewLine();
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