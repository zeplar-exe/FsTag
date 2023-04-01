using FsTag.Data.Models;

namespace FsTag.Data.Interfaces;

public interface IPrintKeyProvider
{
    public PrintData? Get(string key);
    public IEnumerable<PrintData> EnumerateData();
}