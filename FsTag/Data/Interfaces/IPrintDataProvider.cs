using FsTag.Data.Models;

namespace FsTag.Data.Interfaces;

public interface IPrintDataProvider
{
    public PrintData? Get(string key);
    public IEnumerable<PrintData> EnumerateData();
}