using System;

namespace Blog.Utilities.Extensions;

public static class GuardExtension
{
    public static TReturn NotNull<TReturn>(this TReturn value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return value;
    }

    public static string NotNullOrEmpty(this string value)
    {
        value.NotNull();
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("String parameter cannot be null or empty and cannot contain only blanks.",
                nameof(value));
        }

        return value;
    }

    public static Guid NotNullOrEmpty(this Guid value)
    {
        value.NotNull();
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Guid parameter cannot be null or empty and cannot contain only blanks.",
                nameof(value));
        }

        return value;
    }

    public static Guid NotNullOrEmpty(this Guid? value)
    {
        if (!value.HasValue)
        {
            throw new ArgumentException("Guid parameter cannot be null or empty and cannot contain only blanks.",
                nameof(value));
        }

        if (value == Guid.Empty)
        {
            throw new ArgumentException("Guid parameter cannot be null or empty and cannot contain only blanks.",
                nameof(value));
        }

        return value.Value;
    }

    public static void NotNullOrEmpty<T>(this IEnumerable<T> items)
    {
        items.NotNull();
        if (!items.Any())
        {
            throw new ArgumentException("Items cannot be null or empty", nameof(items));
        }
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
    {
        return items == null || !items.Any();
    }

    public static async Task WaitUntilReadyAsync(int timeoutInSeconds, Func<Task<bool>> func)
    {
        if (timeoutInSeconds <= 0)
        {
            throw new ArgumentException("timeoutInSeconds must be larger than zero");
        }

        var timeout = timeoutInSeconds;
        var waitInSeconds = 5;

        while (true)
        {
            var isTrue = false;
            try
            {
                isTrue = await func();
            }
            catch
            {
                isTrue = false;
            }

            if (isTrue || timeout <= 0)
            {
                break;
            }

            Thread.Sleep(waitInSeconds * 1000);
            timeout -= waitInSeconds;
        }
    }
}
