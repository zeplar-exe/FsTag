namespace FsTag.Data.Interfaces;

public interface IFileSystem
{
    public string? ReadText(string path);
    public bool WriteText(string path, string text);
    public TextWriter? OpenTextWriter(string path);
    public TextReader? OpenTextReader(string path);
    public bool CreateDirectory(string directory);
    public IEnumerable<string> EnumerateFiles(string directory);
    public IEnumerable<string> EnumerateDirectories(string directory);
    public bool RecycleFile(string file);
    public bool DeleteFile(string file);
    bool FileExists(string file);
    bool DirectoryExists(string file);
}