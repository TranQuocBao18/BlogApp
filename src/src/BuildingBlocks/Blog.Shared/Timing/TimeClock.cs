using System;

namespace Blog.Shared.Timing;

public static class TimeClock
{
    public static LocalTimeProvider LocalProvider { get; } = new LocalTimeProvider();
    public static UtcTimeProvider UtcProvider { get; } = new UtcTimeProvider();

    public static ITimeProvider _timeProvider;

    static TimeClock()
    {
        Provider = LocalProvider;
    }

    public static ITimeProvider Provider
    {
        get { return _timeProvider; }
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "Can't assign null to TimeClock.Provider");
            }
            _timeProvider = value;
        }
    }

    public static DateTime Now => Provider.Now;
    public static DateTimeKind Kind => Provider.Kind;
}
