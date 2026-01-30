using System;

namespace Blog.Domain.Communication.Requests;

public class CreateUserNotificationRequest : NotificationRequest
{
    public IList<Guid> UserIds { get; set; } = default!;
}
