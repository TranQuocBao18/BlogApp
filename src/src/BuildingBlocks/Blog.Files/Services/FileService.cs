using System;
using Blog.Files.Interfaces;
using Blog.Files.MimeDetector;

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
        return MimeTypes.IsAllowedExtension(Path.GetExtension(fileName));
    }
    protected virtual bool Open(string fileName, byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0)
        {
            return false;
        }

        var fileType = MimeTypes.GetFileType(() => bytes);

        if (fileType == null)
        {
            return false;
        }

        var ext = Path.GetExtension(fileName).TrimStart('.');

        if (!string.Equals(ext, fileType.Extension, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }
}
