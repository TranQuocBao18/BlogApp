using System;
using Blog.Domain.Communication.Entities;
using Blog.Domain.Communication.Enums;
using Blog.Infrastructure.Communication.Interfaces;
using Blog.Model.Dto.Communication.Dtos;
using Blog.Shared.Notification;
using Blog.SignalR.Hubs;
using Blog.SignalR.Notifications;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Blog.Service.Communication.Services.Consumer;

public class CommentCreatedConsumer : IConsumer<CommentCreatedIntegrationEvent>
{
    private readonly ICommunicationUnitOfWork _communicationUnitOfWork;
    private readonly SignalRRealTimeNotifier _signalRRealTimeNotifier;
    private readonly ILogger<CommentCreatedConsumer> _logger;

    public CommentCreatedConsumer(
        ICommunicationUnitOfWork communicationUnitOfWork,
        ILogger<CommentCreatedConsumer> logger,
        SignalRRealTimeNotifier signalRRealTimeNotifier
    )
    {
        _communicationUnitOfWork = communicationUnitOfWork;
        _logger = logger;
        _signalRRealTimeNotifier = signalRRealTimeNotifier;
    }

    public async Task Consume(ConsumeContext<CommentCreatedIntegrationEvent> context)
    {
        var cancellationToken = context.CancellationToken;
        var evenData = context.Message;

        if (evenData.ParentId == Guid.Empty && evenData.BlogAuthorId != Guid.Empty)
        {
            var notification = new NotificationMessage
            {
                Title = "New comment appeared!",
                NotificationType = NotificationType.User,
                ContentNotify = $"{evenData.AuthorName} has just commented in your blog",
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
                Type = "CommentCreated",
                Data = notification,
                CreationTime = DateTime.UtcNow
            };

            await _signalRRealTimeNotifier.SendNotification(new[] { userNotification });
            _logger.LogInformation($"Notification sent to user: {evenData.BlogAuthorId}");
        }
        else
        {
            var notification = new NotificationMessage
            {
                Title = "New comment appeared!",
                NotificationType = NotificationType.User,
                ContentNotify = $"{evenData.AuthorName} has just replied your comment",
                ReferenceData = evenData.BlogId.ToString()
            };

            await _communicationUnitOfWork.NotificationMessageRepository.AddUsersNotificationAsync(
                userIds: new[] { evenData.ParentId!.Value },
                notification,
                cancellationToken
            );
            _logger.LogInformation("Notification saved to db successfully");

            var userNotification = new UserNotification<NotificationMessage>(evenData.ParentId.Value.ToString())
            {
                Type = "CommentCreated",
                Data = notification,
                CreationTime = DateTime.UtcNow
            };

            await _signalRRealTimeNotifier.SendNotification(new[] { userNotification });
            _logger.LogInformation($"Notification sent to user: {evenData.ParentId}");
        }

    }
}
