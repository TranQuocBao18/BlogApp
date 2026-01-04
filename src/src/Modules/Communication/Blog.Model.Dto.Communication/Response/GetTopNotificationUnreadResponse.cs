using System;

namespace Blog.Model.Dto.Communication.Response;

public class GetTopNotificationUnreadResponse
{
    public IList<GetListNotificationByUserResponse> ListNotification { get; set; } = [];
    public int TotalUnread { get; set; } = 0;
}
