using System;

namespace Blog.Model.Dto.Communication.Response;

public class GetListNotificationResponse : NotificationMessageResponse
{
    public List<NotificationUserResponse> Users { get; set; }
}
