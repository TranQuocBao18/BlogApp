using System;

namespace Blog.Shared.Notification;

public interface IRealTimeNotifier
{
    Task SendNotification(IList<IUserNotification> notifications);
    Task SendNotificationToAllClients(IUserNotification notification);
    Task SendNotificationToClient(IUserNotification notification);
    Task SendNotificationBellToClient(IList<IUserNotification> notifications);
}
