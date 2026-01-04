using System;

namespace Blog.Model.Dto.Communication.Response;

public class GetListNotificationByUserResponse : NotificationMessageResponse
{
    public bool IsRead { get; set; }
}
