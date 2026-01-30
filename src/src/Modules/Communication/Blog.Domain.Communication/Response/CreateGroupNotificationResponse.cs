using System;

namespace Blog.Domain.Communication.Response;

public class CreateGroupNotificationResponse
{
    public Guid NotificationId { get; set; } = default!;
    public IList<Guid> NotificationUserIds { get; set; } = [];
}
