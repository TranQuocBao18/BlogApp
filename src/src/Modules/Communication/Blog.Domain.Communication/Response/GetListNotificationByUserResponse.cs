using System;

namespace Blog.Domain.Communication.Response;

public class GetListNotificationByUserResponse : NotificationMessageResponse
{
    public bool IsRead { get; set; }
}
