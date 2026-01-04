using System;

namespace Blog.Model.Dto.Communication.Response;

public class CreateGroupNotificationResponse
{
    public Guid NotificationId { get; set; } = default!;
    public IList<Guid> NotificationUserIds { get; set; } = [];
}
