using System;
using Blog.Domain.Communication.Enums;

namespace Blog.Domain.Communication.Response;

public class NotificationMessageResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public NotificationTypeDto NotificationType { get; set; } = default!;
    public string? ContentNotify { get; set; }
    public string? ReferenceData { get; set; }
}
