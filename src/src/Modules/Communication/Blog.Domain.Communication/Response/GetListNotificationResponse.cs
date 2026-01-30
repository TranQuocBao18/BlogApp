using System;

namespace Blog.Domain.Communication.Response;

public class GetListNotificationResponse : NotificationMessageResponse
{
    public List<NotificationUserResponse> Users { get; set; }
}
