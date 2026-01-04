using System;

namespace Blog.Model.Dto.Communication.Requests;

public class CreateGroupNotificationRequest : NotificationRequest
{
    public Guid GroupId { get; set; } = default!;
}
