using System;
using Blog.Domain.Communication.Entities;
using Blog.Domain.Communication.Enums;
using Blog.Domain.Shared.Contracts;
using Blog.Infrastructure.Communication.Interfaces;
using Blog.Shared.Notification;
using Blog.SignalR.Notifications;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Blog.Service.Communication.Services.Consumer;

public class LikeCreatedConsumer : IConsumer<LikeCreatedIntegrationEvent>
{
    private readonly ICommunicationUnitOfWork _communicationUnitOfWork;
    private readonly SignalRRealTimeNotifier _signalRRealTimeNotifier;
    private readonly ILogger<LikeCreatedConsumer> _logger;

    public LikeCreatedConsumer(
        ICommunicationUnitOfWork communicationUnitOfWork,
        SignalRRealTimeNotifier signalRRealTimeNotifier,
        ILogger<LikeCreatedConsumer> logger
    )
    {
        _communicationUnitOfWork = communicationUnitOfWork;
        _signalRRealTimeNotifier = signalRRealTimeNotifier;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<LikeCreatedIntegrationEvent> context)
    {
        var cancellationToken = context.CancellationToken;
        var evenData = context.Message;

        var notification = new NotificationMessage
        {
            Title = "Someone has liked your blog!",
            NotificationType = NotificationType.User,
            ContentNotify = $"{evenData.AuthorName} has just liked your blog",
            ReferenceData = evenData.BlogId.ToString()
        };

        await _communicationUnitOfWork.NotificationMessageRepository.AddUsersNotificationAsync(
                userIds: new[] { evenData.BlogAuthorId!.Value },
                notification,
                cancellationToken
            );
        _logger.LogInformation("Notification saved to db successfully");

        var userNotification = new UserNotification<NotificationMessage>(evenData.BlogAuthorId.Value.ToString())
        {
            Type = "LikeCreated",
            Data = notification,
            CreationTime = DateTime.UtcNow
        };

        await _signalRRealTimeNotifier.SendNotification(new[] { userNotification });
        _logger.LogInformation($"Notification sent to user: {evenData.BlogAuthorId}");
    }
}
