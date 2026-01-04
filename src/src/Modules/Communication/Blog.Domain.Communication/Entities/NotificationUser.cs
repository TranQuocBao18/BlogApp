using System;

namespace Blog.Domain.Communication.Entities;

public class NotificationUser
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid NotificationId { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime Created { get; set; }

    public NotificationMessage PushMultiNotificationMessage { get; set; } = default!;
}
