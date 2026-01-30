using System;
using Blog.Domain.Communication.Enums;

namespace Blog.Domain.Communication.Requests;

public class NotificationRequest
{
    public string Title { get; set; } = default!;
    public string ContentNotify { get; set; } = default!;
    public string ReferenceData { get; set; } = default!;
    public NotificationTypeDto NotificationType { get; set; } = default!;
}
