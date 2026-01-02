using System;

namespace Blog.Shared.Timing;

public class UtcTimeProvider : ITimeProvider
{
    public DateTime Now => DateTime.UtcNow;
    public DateTimeKind Kind => DateTimeKind.Utc;
}
