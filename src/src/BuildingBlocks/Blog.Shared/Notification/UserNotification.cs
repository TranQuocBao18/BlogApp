using System;
using Blog.Shared.Timing;

namespace Blog.Shared.Notification;

public interface IUserIdentifier
{
    string UserId { get; }
}

public interface IUserNotification : IUserIdentifier
{
    DateTime CreationTime { get; set; }
    string Type { get; set; }
}

public class UserNotification<NotificationT> : IUserNotification
{
    public UserNotification(string userId)
    {
        CreationTime = TimeClock.Now;
        UserId = userId;
    }

    public NotificationT Data { get; set; }

    public string Type { get; set; }
    public DateTime CreationTime { get; set; }
    public string UserId { get; private set; }
}
