using System;

namespace Blog.Model.Dto.Communication.Requests;

public class MarkNotificationAsReadRequest
{
    public IList<Guid> NotificationIds { get; set; } = [];
}
