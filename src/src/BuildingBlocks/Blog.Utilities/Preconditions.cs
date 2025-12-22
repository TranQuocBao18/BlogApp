using System;
using System.Globalization;

namespace Blog.Utilities;

public static class Preconditions
{
    private const string NullErrorMessage = "must not be NULL.";
    private const string BlankErrorMessage = "must not be BLANK.";
    private const string ValueParamName = "value";
    private const string NameParamName = "name";
    private const string MessageParamName = "message";

    public static void CheckNotNull<T>(T value, string name)
    {
        CheckNotNull(value, name, NullErrorMessage);
    }

    public static void CheckNotNull<T>(T value)
    {
        CheckNotNull(value, nameof(value), NullErrorMessage);
    }

    public static void CheckNotNull<T>(T value, string name, string message)
    {
        if (value != null)
        {
            return;
        }

        CheckNotBlank(name, NameParamName, BlankErrorMessage);
        CheckNotBlank(message, MessageParamName, BlankErrorMessage);

        var errorMessage = string.Format(CultureInfo.CurrentCulture, "{0} {1}", name, message);
        throw new Exception(errorMessage);
    }

    public static void CheckNotBlank(string value, string name)
    {
        CheckNotBlank(value, name, BlankErrorMessage);
    }

    public static void CheckNotBlank(string value, string name, string message)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            var errorMessage = string.Format(CultureInfo.CurrentCulture, "{0} {1}", NameParamName, BlankErrorMessage);
            throw new Exception(errorMessage);
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            var errorMessage = string.Format(CultureInfo.CurrentCulture, "{0} {1}", MessageParamName, BlankErrorMessage);
            throw new Exception(errorMessage);
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            var errorMessage = string.Format(CultureInfo.CurrentCulture, "{0} {1}", name, message);
            throw new Exception(errorMessage);
        }
    }

    public static void CheckFileExist(string value, string name)
    {
        CheckFileExist(value, name, "does not exist.");
    }

    public static void CheckFileExist(string value, string name, string message)
    {
        CheckNotNull(value, name);
        CheckNotBlank(name, NameParamName);
        CheckNotBlank(message, MessageParamName);
        CheckNotBlank(value, name);
        if (!File.Exists(value))
        {
            throw new Exception($"{value} does not exist");
        }
    }

    public static void CheckFilePathCorrect(string value, string name)
    {
        CheckFilePathCorrect(value, name, "file path not correct");
    }

    public static void CheckFilePathCorrect(string value, string name, string message)
    {
        CheckNotNull(value, name);
        CheckNotBlank(name, NameParamName);
        CheckNotBlank(message, MessageParamName);
        CheckNotBlank(value, name);
        if (!Directory.Exists(value))
        {
            throw new Exception($"{name} does not exist");
        }
    }

    public static void CheckNotNullOrEmpty(string value)
    {
        var paramName = nameof(value);
        CheckNotNull(value, paramName);
        CheckNotBlank(value, paramName);
    }

    public static void CheckEnumCorrect<T>(T value)
    {
        var exists = Enum.IsDefined(typeof(T), value);
        if (!exists)
        {
            throw new Exception($"{value} does not exist");
        }
    }
}
