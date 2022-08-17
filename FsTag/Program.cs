using CommandDotNet;

namespace FsTag;

[Command]
public class Program
{
    public static int Main(string[] args)
    {
        return new AppRunner<Program>().Run(args);
    }

    [Command("tag")]
    public int Tag(string filePath, 
        [Option('r', "recursive")] bool isRecursive, 
        [Option("recurseDepth")] int? recurseDepth)
    {
        if (!isRecursive && recurseDepth != null)
        {
            WriteFormatter.Warning("recurseDepth is set, but recursion is not specified. Did you forget -r?");
        }

        string absolutePath;
        
        if (IsAbsolutePath(filePath))
        {
            absolutePath = filePath;
        }
        else
        {
            absolutePath = Path.Join(Directory.GetCurrentDirectory(), filePath);
        }
        
        if (isRecursive)
        {
            if (Directory.Exists(absolutePath))
            {
                if (recurseDepth != null)
                {
                    var enumerable = EnumerateFilesToDepth(absolutePath, recurseDepth.Value);

                    return TagFileEnumerable(enumerable);
                }
                else
                {
                    var enumerable = Directory.EnumerateFiles(absolutePath, "*", SearchOption.AllDirectories);

                    return TagFileEnumerable(enumerable);
                }
            }
            else
            {
                WriteFormatter.Error($"The directory '{filePath}' does not exist.");
            
                return 1;
            }
        }

        return TagFile(absolutePath);
    }

    private int TagFileEnumerable(IEnumerable<string> fileNames)
    {
        foreach (var file in fileNames)
        {
            var result = TagFile(file);

            if (result != 0)
                return result;
        }

        return 0;
    }

    private int TagFile(string absolutePath)
    {
        if (!File.Exists(absolutePath))
        {
            WriteFormatter.Error($"The file '{absolutePath}' does not exist.");
            
            return 1;
        }
        
        Console.WriteLine($"Added '{absolutePath}' to tag index.");

        return 0;
    }

    private bool IsAbsolutePath(string path)
    {
        return Path.IsPathRooted(path) || Path.IsPathFullyQualified(path);
    }

    private IEnumerable<string> EnumerateFilesToDepth(string directory, int targetDepth)
    {
        if (targetDepth == 0)
            yield break;

        foreach (var file in EnumerateFilesToDepth(directory, targetDepth, 0))
        {
            yield return file;
        }
    }
    
    private IEnumerable<string> EnumerateFilesToDepth(string directory, int maxDepth, int depth)
    {
        foreach (var file in Directory.EnumerateFiles(directory))
        {
            yield return file;
        }
        
        if (depth != maxDepth)
        {
            foreach (var dir in Directory.EnumerateDirectories(directory))
            {
                foreach (var file in EnumerateFilesToDepth(dir, maxDepth, depth + 1))
                {
                    yield return file;
                }
            }
        }
    }
}