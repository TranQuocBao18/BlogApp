using System;
using Blog.Domain.Communication.Entities;
using Blog.Domain.Communication.Enums;
using Blog.Domain.Shared.Contracts;
using Blog.Infrastructure.Communication.Interfaces;
using Blog.Shared.Notification;
using Blog.SignalR.Core;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Blog.Infrastructure.Communication.Consumer;

public class LikeCreatedConsumer : IConsumer<LikeCreatedIntegrationEvent>
{
    private readonly ICommunicationUnitOfWork _communicationUnitOfWork;
    private readonly IRealTimeNotifier _realTimeNotifier;
    private readonly ILogger<LikeCreatedConsumer> _logger;

    public LikeCreatedConsumer(
        ICommunicationUnitOfWork communicationUnitOfWork,
        IRealTimeNotifier realTimeNotifier,
        ILogger<LikeCreatedConsumer> logger
    )
    {
        _communicationUnitOfWork = communicationUnitOfWork;
        _realTimeNotifier = realTimeNotifier;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<LikeCreatedIntegrationEvent> context)
    {
        var cancellationToken = context.CancellationToken;
        var evenData = context.Message;

        _logger.LogInformation("=== LikeCreatedConsumer.Consume START ===");
        _logger.LogInformation($"Event received: BlogId={evenData.BlogId}, AuthorId={evenData.AuthorId}, BlogAuthorId={evenData.BlogAuthorId}");

        try
        {
            var notification = new NotificationMessage
            {
                Title = "Someone has liked your blog!",
                NotificationType = NotificationType.User,
                ContentNotify = $"{evenData.AuthorName} has just liked your blog",
                ReferenceData = evenData.BlogId.ToString()
            };

            _logger.LogInformation($"Notification message created: {notification.Title}");

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

            _logger.LogInformation($"Sending SignalR notification to user: {evenData.BlogAuthorId}");
            await _realTimeNotifier.SendNotification(new[] { userNotification });
            _logger.LogInformation($"Notification sent to user: {evenData.BlogAuthorId}");
            _logger.LogInformation("=== LikeCreatedConsumer.Consume COMPLETED ===");
        }
        catch (Exception ex)
        {
            _logger.LogError($"=== LikeCreatedConsumer.Consume ERROR ===");
            _logger.LogError($"Exception: {ex.Message}");
            _logger.LogError($"StackTrace: {ex.StackTrace}");
            throw;
        }
    }
}
