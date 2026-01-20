using System;
using Blog.Domain.Communication.Entities;
using Blog.Domain.Communication.Enums;
using Blog.Domain.Shared.Contracts;
using Blog.Infrastructure.Communication.Interfaces;
using Blog.Model.Dto.Communication.Dtos;
using Blog.Shared.Notification;
using Blog.SignalR.Notifications;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Blog.Presentation.Communication.Consumer;

public class CommentLikeCreatedConsumer : IConsumer<CommentLikeCreatedIntegrationEvent>
{
    private readonly ICommunicationUnitOfWork _communicationUnitOfWork;
    private readonly IRealTimeNotifier _realTimeNotifier;
    private readonly ILogger<CommentLikeCreatedConsumer> _logger;

    public CommentLikeCreatedConsumer(
        ICommunicationUnitOfWork communicationUnitOfWork,
        IRealTimeNotifier realTimeNotifier,
        ILogger<CommentLikeCreatedConsumer> logger
    )
    {
        _communicationUnitOfWork = communicationUnitOfWork;
        _realTimeNotifier = realTimeNotifier;
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

        var notificationDto = new NotificationMessageDto
        {
            Title = notification.Title,
            NotificationType = notification.NotificationType,
            ContentNotify = notification.ContentNotify,
            ReferenceData = notification.ReferenceData
        };

        var userNotification = new UserNotification<NotificationMessageDto>(evenData.CommentAuthorId.Value.ToString())
        {
            Type = "LikeCreated",
            Data = notificationDto,
            CreationTime = DateTime.UtcNow
        };

        await _realTimeNotifier.SendNotification(new[] { userNotification });
        _logger.LogInformation($"Notification sent to user: {evenData.CommentAuthorId}");
    }
}
