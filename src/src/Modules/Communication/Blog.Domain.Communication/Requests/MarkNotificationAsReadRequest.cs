using System;

namespace Blog.Domain.Communication.Requests;

public class MarkNotificationAsReadRequest
{
    public IList<Guid> NotificationIds { get; set; } = [];
}
