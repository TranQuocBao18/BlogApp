using System;
using Blog.Domain.Communication.Enums;

namespace Blog.Domain.Communication.Dtos;

public class NotificationMessageDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public NotificationType NotificationType { get; set; }
    public string? ContentNotify { get; set; }
    public string? ReferenceData { get; set; }
}
