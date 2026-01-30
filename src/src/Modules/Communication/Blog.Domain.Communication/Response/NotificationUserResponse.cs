using System;

namespace Blog.Domain.Communication.Response;

public class NotificationUserResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid NotificationId { get; set; }
    public bool IsRead { get; set; }
    public DateTime Created { get; set; }
}
