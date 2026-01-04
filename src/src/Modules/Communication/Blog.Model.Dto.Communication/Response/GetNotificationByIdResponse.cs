using System;

namespace Blog.Model.Dto.Communication.Response;

public class GetNotificationByIdResponse : NotificationMessageResponse
{
    public IList<NotificationUserResponse> Users { get; set; } = [];
}
