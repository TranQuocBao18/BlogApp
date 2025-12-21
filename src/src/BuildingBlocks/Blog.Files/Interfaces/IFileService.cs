using System;

namespace Blog.Files.Interfaces;

public interface IFileService
{
    bool TryOpen(string fileName, byte[] bytes);
}
