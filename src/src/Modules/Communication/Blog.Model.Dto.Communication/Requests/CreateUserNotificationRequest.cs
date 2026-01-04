using System;

namespace Blog.Model.Dto.Communication.Requests;

public class CreateUserNotificationRequest : NotificationRequest
{
    public IList<Guid> UserIds { get; set; } = default!;
}
