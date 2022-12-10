using FsTag.Helpers;

namespace FsTag;

public static class AppData
{
    private const string DefaultSession = "__default";
    private const string DirectoryName = "fstag";
    private static string RootDirectory => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static string DataDirectory => Path.Join(RootDirectory, DirectoryName);

    public static void IndexFiles(IEnumerable<string> fileNames)
    {
        var set = fileNames.ToHashSet();
        var file = IndexOfSession(DefaultSession);

        foreach (var item in EnumerateIndex())
        {
            if (set.Remove(item))
            {
                WriteFormatter.Info($"'{item}' already exists in the tag index.");
            }
        }

        using var writer = new StreamWriter(file, append: true);

        foreach (var item in set)
        {
            if (!File.Exists(item))
            {
                WriteFormatter.Warning($"The file '{item}' does not exist.");
            
                continue;
            }
            
            writer.WriteLine(item);
            WriteFormatter.Info($"Added '{item}' to tag index.");
        }
    }

    public static IEnumerable<string> EnumerateIndex()
    {
        var file = IndexOfSession(DefaultSession);

        using var reader = new StreamReader(file);

        while (reader.ReadLine() is {} line) // obscure as hell syntax bro, basically just while it isn't null
        {
            yield return line;
        }
    }

    public static void ClearIndex()
    {
        var file = IndexOfSession(DefaultSession);
        
        File.WriteAllText(file, string.Empty);
        
        WriteFormatter.Info("Successfully cleared tag index.");
    }

    public static void RemoveFromIndex(IEnumerable<string> fileNames)
    {
        var names = fileNames.ToHashSet();

        var index = IndexOfSession(DefaultSession);
        var tempIndex = index + ".tmp";
        var removedAny = false;

        using (var reader = new StreamReader(index))
        {
            using var tempStream = File.OpenWrite(tempIndex);
            using var writer = new StreamWriter(tempStream);

            while (reader.ReadLine() is {} line)
            {
                if (names.Contains(line))
                {
                    WriteFormatter.Info($"Removing '{line}' from the index.");
                    removedAny = true;
                }
                else
                {
                    writer.WriteLine(line);
                }
            }
        }

        if (!removedAny)
        {
            WriteFormatter.Info("Nothing to remove. Terminating early.");
            
            return;
        }

        File.Delete(index);
        
        using (var destination = File.OpenWrite(index))
        {
            using var source = File.OpenRead(tempIndex);

            source.CopyTo(destination);
        }
        
        File.Delete(tempIndex);
    }

    private static void EnsureDirectory(string directory)
    {
        Directory.CreateDirectory(directory);
    }

    private static void EnsureFile(string file)
    {
        if (!File.Exists(file))
            File.Create(file).Dispose();
    }

    private static string IndexOfSession(string session)
    {
        var path = Path.Join(DataDirectory, $"sessions/{session}");
        var file = Path.Join(path, "index.nsv");

        EnsureDirectory(path);
        EnsureFile(file);

        return file;
    }
}