using System;
using Blog.Shared.Notification;
using Blog.SignalR.Core;
using Blog.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Blog.SignalR.Notifications;

public class SignalRRealTimeNotifier : IRealTimeNotifier
{
    private const string REALTIME_NOTIFICATION_KEY = "NOTIFICATION";
    private readonly IOnlineClientManager _onlineClientManager;
    private readonly IHubContext<CommonHub> _hubContext;
    private readonly ILogger<SignalRRealTimeNotifier> _logger;

    public SignalRRealTimeNotifier(
        IOnlineClientManager onlineClientManager,
        IHubContext<CommonHub> hubContext,
        ILogger<SignalRRealTimeNotifier> logger
    )
    {
        _onlineClientManager = onlineClientManager;
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task SendNotification(IList<IUserNotification> notifications)
    {
        _logger.LogWarning("Start send noti..............");
        foreach (var notification in notifications)
        {
            try
            {
                var clients = GetOnlineClientsByNotification(notification);

                _logger.LogInformation($"Have {clients.Count} clients of user {notification.UserId}");

                if (clients != null && clients.Any())
                {
                    foreach (var client in clients)
                    {
                        await SendNotification2Client(notification, client);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Can't send notification to user {notification.UserId}. {ex.Message}", ex);
            }
        }
    }

    public async Task SendNotificationToAllClients(IUserNotification notification)
    {
        try
        {
            var clients = GetAllOnlineClients();
            _logger.LogInformation($"Have {clients.Count} clients of user {notification.UserId}");
            foreach (var client in clients)
            {
                await SendNotification2Client(notification, client);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Can't send nofication to user {notification.UserId}. {ex.Message}", ex);
        }
    }

    private async Task SendNotification2Client(IUserNotification notification, IOnlineClient client)
    {
        var signalRClient = _hubContext.Clients.Client(client.ConnectionId);
        if (signalRClient == null)
        {
            _logger.LogDebug($"Can't get Connection {client.ConnectionId} of user {notification.UserId}");
            return;
        }
        await signalRClient.SendAsync(REALTIME_NOTIFICATION_KEY, notification);
    }

    private IReadOnlyList<IOnlineClient> GetOnlineClientsByNotification(IUserNotification notification)
    {
        return _onlineClientManager.GetByUser(notification);
    }

    private IReadOnlyList<IOnlineClient> GetAllOnlineClients()
    {
        return _onlineClientManager.GetAll();
    }

    public async Task SendNotificationToClient(IUserNotification notification)
    {
        await _hubContext.Clients.Group(notification.UserId).SendAsync("ReceiveMessage", notification);
    }

    public async Task SendNotificationBellToClient(IList<IUserNotification> notifications)
    {
        try
        {
            _logger.LogInformation($"{nameof(SendNotificationBellToClient)} - Begin...");
            var clients = GetAllOnlineClients();

            var pushNoticeBells = notifications.Where(n => clients.Any(c => c.UserId == n.UserId));

            if (pushNoticeBells.Any())
            {
                foreach (var item in pushNoticeBells)
                {
                    _logger.LogInformation($"{nameof(SendNotificationBellToClient)} - push bell to userid = {item.UserId}");
                    await _hubContext.Clients.Group(item.UserId).SendAsync("ReceiveMessage", item);
                }
            }
            _logger.LogInformation($"{nameof(SendNotificationBellToClient)} - End");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"{nameof(SendNotificationBellToClient)} - End: {ex.Message}");
        }
    }
}
