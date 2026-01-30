using System;

namespace Blog.Domain.Communication.Requests;

public class CreateGroupNotificationRequest : NotificationRequest
{
    public Guid GroupId { get; set; } = default!;
}
