using System;

namespace Blog.Shared;

public static class Guard
{
    public static void NotNull<TReturn>(TReturn value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
    }

    public static void NotNullOrEmpty(string value)
    {
        NotNull(value);
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("String parameter cannot be null or empty and cannot contain only blanks.", nameof(value));
        }
    }

    public static void NotNullOrEmpty(Guid value)
    {
        NotNull(value);
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Guid parameter cannot be null or empty and cannot contain only blanks.", nameof(value));
        }
    }
}
