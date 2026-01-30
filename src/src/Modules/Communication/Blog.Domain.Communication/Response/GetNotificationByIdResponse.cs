using System;

namespace Blog.Domain.Communication.Response;

public class GetNotificationByIdResponse : NotificationMessageResponse
{
    public IList<NotificationUserResponse> Users { get; set; } = [];
}
