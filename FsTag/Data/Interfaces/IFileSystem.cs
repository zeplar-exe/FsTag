namespace FsTag.Data.Interfaces;

public interface IFileSystem
{
    public FileSystemOperation<string> ReadText(string path);
    public FileSystemOperation WriteText(string path, string text);
    public FileSystemOperation<StreamWriter> OpenStreamWriter(string path);
    public FileSystemOperation<StreamReader> OpenStreamReader(string path);
    public FileSystemOperation CreateDirectory(string directory);
    public FileSystemOperation<IEnumerable<string>> EnumerateFiles(string directory);
    public FileSystemOperation<IEnumerable<string>> EnumerateDirectories(string directory);
    public FileSystemOperation RecycleFile(string file);
    public FileSystemOperation DeleteFile(string file);
    public FileSystemOperation FileExists(string file);
    public FileSystemOperation DirectoryExists(string file);
    public FileSystemOperation MoveFile(string source, string destination);
}