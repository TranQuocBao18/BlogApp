using System;
using Blog.Infrastructure.Shared.Interfaces;

namespace Blog.Infrastructure.Shared.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime NowUtc => DateTime.UtcNow;
}
