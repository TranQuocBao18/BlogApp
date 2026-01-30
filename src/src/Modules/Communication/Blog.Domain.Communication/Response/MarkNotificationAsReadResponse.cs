using System;

namespace Blog.Domain.Communication.Response;

public class MarkNotificationAsReadResponse
{
    public IList<Guid> NotificationIds { get; set; } = [];
}
