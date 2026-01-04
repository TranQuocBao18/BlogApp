using System;
using Blog.Domain.Communication.Enums;
using Blog.Domain.Shared.Common;

namespace Blog.Domain.Communication.Entities;

public class NotificationMessage : BaseEntityWithAudit
{
    public string Title { get; set; } = default!;
    public NotificationType NotificationType { get; set; }
    public string? ContentNotify { get; set; }
    public string? ReferenceData { get; set; }
    public ICollection<NotificationUser> NotificationUsers { get; set; } = [];
}
