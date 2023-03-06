namespace FsTag.Data.Interfaces;

public record FileSystemOperation<T>(bool Success, T Result);

public record FileSystemOperation(bool Success)
{
    public static implicit operator bool(FileSystemOperation operation) => operation.Success;
}