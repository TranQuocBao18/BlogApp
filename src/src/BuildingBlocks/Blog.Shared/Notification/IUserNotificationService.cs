using System;

namespace Blog.Shared.Notification;

public interface IUserNotificationService
{
    Task SendNotificationAsync(Guid userId, string contentNotify, string title, string referenceData, CancellationToken cancellationToken);
}
