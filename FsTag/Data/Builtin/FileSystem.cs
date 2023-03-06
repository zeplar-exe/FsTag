using FsTag.Data.Interfaces;

using Microsoft.VisualBasic.FileIO;

namespace FsTag.Data.Builtin;

public class FileSystem : IFileSystem
{
    public FileSystemOperation<string> ReadText(string path)
    {
        if (!File.Exists(path))
            return new FileSystemOperation<string>(false, null);

        return new FileSystemOperation<string>(true, File.ReadAllText(path));
    }

    public FileSystemOperation WriteText(string path, string text)
    {
        if (!File.Exists(path))
            return new FileSystemOperation(false);

        File.WriteAllText(path, text);
        
        return new FileSystemOperation(true);
    }

    public FileSystemOperation<StreamWriter> OpenStreamWriter(string path)
    {
        if (!File.Exists(path))
            return new FileSystemOperation<StreamWriter>(false, null);

        var stream = File.OpenWrite(path);
        var writer = new StreamWriter(stream);

        return new FileSystemOperation<StreamWriter>(true, writer);
    }

    public FileSystemOperation<StreamReader> OpenStreamReader(string path)
    {
        if (!File.Exists(path))
            return new FileSystemOperation<StreamReader>(false, null);

        var stream = File.OpenWrite(path);
        var reader = new StreamReader(stream);

        return new FileSystemOperation<StreamReader>(true, reader);
    }

    public FileSystemOperation CreateDirectory(string directory)
    {
        Directory.CreateDirectory(directory);

        return new FileSystemOperation(true);
    }

    public FileSystemOperation<IEnumerable<string>> EnumerateFiles(string directory)
    {
        if (!Directory.Exists(directory))
            return new FileSystemOperation<IEnumerable<string>>(false, Enumerable.Empty<string>());
        
        var files = Directory.EnumerateFiles(directory);

        return new FileSystemOperation<IEnumerable<string>>(true, files);
    }

    public FileSystemOperation<IEnumerable<string>> EnumerateDirectories(string directory)
    {
        if (!Directory.Exists(directory))
            return new FileSystemOperation<IEnumerable<string>>(false, Enumerable.Empty<string>());
        
        var files = Directory.EnumerateDirectories(directory);

        return new FileSystemOperation<IEnumerable<string>>(true, files);
    }

    public FileSystemOperation RecycleFile(string file)
    {
        if (!File.Exists(file))
            return new FileSystemOperation(false);
        
        Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(file, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);

        return new FileSystemOperation(true);
    }

    public FileSystemOperation DeleteFile(string file)
    {
        if (!File.Exists(file))
            return new FileSystemOperation(false);
        
        File.Delete(file);
        
        return new FileSystemOperation(true);
    }

    public FileSystemOperation FileExists(string file)
    {
        return new FileSystemOperation(File.Exists(file));
    }

    public FileSystemOperation DirectoryExists(string file)
    {
        return new FileSystemOperation(Directory.Exists(file));
    }

    public FileSystemOperation MoveFile(string source, string destination)
    {
        File.Move(source, destination);

        return new FileSystemOperation(true);
    }
}