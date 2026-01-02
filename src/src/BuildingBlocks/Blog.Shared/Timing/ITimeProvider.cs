using System;

namespace Blog.Shared.Timing;

public interface ITimeProvider
{
    DateTime Now { get; }
    DateTimeKind Kind { get; }
}
