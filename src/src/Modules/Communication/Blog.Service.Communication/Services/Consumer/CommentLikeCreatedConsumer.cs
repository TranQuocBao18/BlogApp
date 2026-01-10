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

public class CommentLikeCreatedConsumer : IConsumer<CommentLikeCreatedIntegrationEvent>
{
    private readonly ICommunicationUnitOfWork _communicationUnitOfWork;
    private readonly SignalRRealTimeNotifier _signalRRealTimeNotifier;
    private readonly ILogger<CommentLikeCreatedConsumer> _logger;

    public CommentLikeCreatedConsumer(
        ICommunicationUnitOfWork communicationUnitOfWork,
        SignalRRealTimeNotifier signalRRealTimeNotifier,
        ILogger<CommentLikeCreatedConsumer> logger
    )
    {
        _communicationUnitOfWork = communicationUnitOfWork;
        _signalRRealTimeNotifier = signalRRealTimeNotifier;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CommentLikeCreatedIntegrationEvent> context)
    {
        var cancellationToken = context.CancellationToken;
        var evenData = context.Message;

        var notification = new NotificationMessage
        {
            Title = "Someone has liked your comment!",
            NotificationType = NotificationType.User,
            ContentNotify = $"{evenData.AuthorName} has just liked your comment",
            ReferenceData = evenData.BlogId.ToString()
        };

        await _communicationUnitOfWork.NotificationMessageRepository.AddUsersNotificationAsync(
                userIds: new[] { evenData.CommentAuthorId!.Value },
                notification,
                cancellationToken
            );
        _logger.LogInformation("Notification saved to db successfully");

        var userNotification = new UserNotification<NotificationMessage>(evenData.CommentAuthorId.Value.ToString())
        {
            Type = "LikeCreated",
            Data = notification,
            CreationTime = DateTime.UtcNow
        };

        await _signalRRealTimeNotifier.SendNotification(new[] { userNotification });
        _logger.LogInformation($"Notification sent to user: {evenData.CommentAuthorId}");
    }
}
