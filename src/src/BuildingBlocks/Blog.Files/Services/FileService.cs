using System;
using Blog.Files.Interfaces;

namespace Blog.Files.Services;

public class FileService : IFileService
{
    protected readonly IFileService _next;

    public FileService(IFileService next)
    {
        _next = next;
    }

    public bool TryOpen(string fileName, byte[] bytes)
    {
        if (CanOpen(fileName))
        {
            return Open(fileName, bytes);
        }
        if (_next != null)
        {
            return _next.TryOpen(fileName, bytes);
        }
        return false;
    }

    protected virtual bool CanOpen(string fileName)
    {
        return false;
    }
    protected virtual bool Open(string fileName, byte[] bytes)
    {
        return false;
    }
}
