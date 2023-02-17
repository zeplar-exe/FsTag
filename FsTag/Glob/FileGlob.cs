namespace FsTag.Glob;

public class FileGlob
{
    public static FileGlob Parse(string glob)
    {
        return new FileGlob();
    }
    
    public bool IsMatchFrom(string directory)
    {
        return false;
    }
    
    public bool IsMatch()
    {
        return false;
    }

    public IEnumerable<string> GetMatchesFrom(string directory)
    {
        yield break;
    }
}