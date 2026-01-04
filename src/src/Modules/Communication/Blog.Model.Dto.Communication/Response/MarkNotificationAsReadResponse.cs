using System;

namespace Blog.Model.Dto.Communication.Response;

public class MarkNotificationAsReadResponse
{
    public IList<Guid> NotificationIds { get; set; } = [];
}
