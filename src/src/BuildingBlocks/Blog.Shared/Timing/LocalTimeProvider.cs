using System;

namespace Blog.Shared.Timing;

public class LocalTimeProvider : ITimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTimeKind Kind => DateTimeKind.Local;
}
