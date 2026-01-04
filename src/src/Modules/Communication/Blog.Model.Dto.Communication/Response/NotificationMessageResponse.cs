using System;
using Blog.Model.Dto.Communication.Enums;

namespace Blog.Model.Dto.Communication.Response;

public class NotificationMessageResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public NotificationTypeDto NotificationType { get; set; } = default!;
    public string? ContentNotify { get; set; }
    public string? ReferenceData { get; set; }
}
