using System;

namespace Blog.Infrastructure.Shared.Interfaces;

public interface IDateTimeService
{
    DateTime NowUtc { get; }
}
