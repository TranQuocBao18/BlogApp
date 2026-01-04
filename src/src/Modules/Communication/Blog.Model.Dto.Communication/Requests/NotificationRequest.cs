using System;
using Blog.Model.Dto.Communication.Enums;

namespace Blog.Model.Dto.Communication.Requests;

public class NotificationRequest
{
    public string Title { get; set; } = default!;
    public string ContentNotify { get; set; } = default!;
    public string ReferenceData { get; set; } = default!;
    public NotificationTypeDto NotificationType { get; set; } = default!;
}
