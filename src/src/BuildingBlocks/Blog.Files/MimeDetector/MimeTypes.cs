using System;
using System.Linq;

namespace Blog.Files.MimeDetector;

public static class MimeTypes
{
    private static List<FileType> _fileTypes;

    static MimeTypes()
    {
        _fileTypes = new List<FileType> { JPEG, PNG, GIF, ICO, BMP };
    }

    #region 

    public readonly static FileType JPEG = new FileType(new byte?[] { 0xFF, 0xD8, 0xFF }, "jpg", "image/jpeg");
    public readonly static FileType PNG = new FileType(new byte?[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, "png", "image/png");
    public readonly static FileType GIF = new FileType(new byte?[] { 0x47, 0x49, 0x46, 0x38, null, 0x61 }, "gif", "image/gif");
    public readonly static FileType BMP = new FileType(new byte?[] { 66, 77 }, "bmp", "image/gif");
    public readonly static FileType ICO = new FileType(new byte?[] { 0, 0, 1, 0 }, "ico", "image/x-icon");

    private readonly static List<string> JPGHEX = new List<string> { "FF", "D8" };
    private readonly static List<string> BMPHEX = new List<string> { "42", "4D" };
    private readonly static List<string> GIFHEX = new List<string> { "47", "49", "46" };
    private readonly static List<string> PNGHEX = new List<string> { "89", "50", "4E", "47", "0D", "0A", "1A", "0A" };

    private const string JPGBIT = "FF";
    private const string BMPBIT = "42";
    private const string GIFBIT = "47";
    private const string PNGBIT = "89";

    public const int MaxHeaderSize = 560;

    #endregion

    public static bool IsAllowedExtension(string extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
            return false;

        var ext = extension.TrimStart('.');
        return _fileTypes.Any(ft => string.Equals(ft.Extension, ext, StringComparison.OrdinalIgnoreCase));
    }

    #region 

    private static int GetFileMatchingCount(byte[] fileHeader, FileType fileType)
    {
        int matchingCount = 0;
        for (int i = 0; i < fileType.Header.Length; i++)
        {

            if (fileType.Header[i] != null && fileType.Header[i] != fileHeader[i + fileType.HeaderOffset])
            {

                matchingCount = 0;
                break;
            }

            matchingCount++;
        }

        return matchingCount;
    }
    public static FileType GetFileType(Func<byte[]> fileHeaderReadFunc, string fileFullName = "")
    {
        FileType fileType = null;

        byte[] fileHeader = fileHeaderReadFunc();

        foreach (FileType type in _fileTypes)
        {
            int matchingCount = GetFileMatchingCount(fileHeader, type);
            if (matchingCount == type.Header.Length)
            {
                fileType = type;

            }
        }
        return fileType;
    }

    private static byte[] ReadFileHeader(FileInfo fileInfo, int maxHeaderSize)
    {
        byte[] header = new byte[maxHeaderSize];
        try
        {
            using (FileStream fsSource = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
            {

                fsSource.Read(header, 0, maxHeaderSize);
            }

        }
        catch (Exception e)
        {
            throw new ArgumentException($"Could not read file : {e.Message}");
        }

        return header;
    }

    public static FileType GetFileType(this FileInfo fileInfo)
    {
        return GetFileType(() => ReadFileHeader(fileInfo, MaxHeaderSize), fileInfo.FullName);
    }
    public static bool IsType(this FileInfo fileInfo, FileType fileType)
    {
        FileType actualType = GetFileType(fileInfo);

        if (null == actualType)
            return false;

        return (actualType.Equals(fileType));
    }

    #endregion

}
