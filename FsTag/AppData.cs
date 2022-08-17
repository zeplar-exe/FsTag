namespace FsTag;

public static class AppData
{
    private const string DirectoryName = "fstag";
    private static string RootDirectory => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static string DataDirectory => Path.Join(RootDirectory, DirectoryName);

    public static bool IndexFiles(IEnumerable<string> fileNames)
    {
        var set = fileNames.ToHashSet();
        var path = Path.Join(DataDirectory, "sessions/__default");
        var file = Path.Join(path, "index.nsv");

        EnsureDirectory(path);
        EnsureFile(file);

        using (var reader = new StreamReader(file))
        {
            while (reader.ReadLine() is {} line) // obscure as hell syntax bro, basically just while it isn't null
            {
                set.Remove(line);
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
            Console.WriteLine($"Added '{item}' to tag index.");
        }

        return true;
    }

    private static void EnsureDirectory(string directory)
    {
        Directory.CreateDirectory(directory);
    }

    private static void EnsureFile(string file)
    {
        File.Create(file).Dispose();
    }
}