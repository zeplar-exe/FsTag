﻿namespace FsTag.Data.Interfaces;

public interface IFileIndex
{
    public IEnumerable<string> EnumerateItems();

    public void Add(IEnumerable<string> items);
    public void Remove(IEnumerable<string> items, int verbosity);
    public void Clean(int verbosity);
    public void Clear();
}