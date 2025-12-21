using System;

namespace Blog.Files.Interfaces;

public interface IFileScannerService
{
    Task<bool> ScanAsync(string fileName, byte[] bytes);
}
