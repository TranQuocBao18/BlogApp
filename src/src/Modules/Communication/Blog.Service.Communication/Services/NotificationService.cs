using System;
using Blog.Domain.Communication.Entities;
using Blog.Domain.Communication.Enums;
using Blog.Domain.Communication.Interfaces;
using Blog.Infrastructure.Shared.ErrorCodes;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Communication.Requests;
using Blog.Domain.Communication.Response;
using Blog.Service.Communication.Interfaces;
using Blog.Shared.Auth;
using Microsoft.Extensions.Logging;

namespace Blog.Service.Communication.Services;

public class NotificationService : INotificationService
{
    private readonly ICommunicationUnitOfWork _communicationUnitOfWork;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        ICommunicationUnitOfWork communicationUnitOfWork,
        ISecurityContextAccessor securityContextAccessor,
        ILogger<NotificationService> logger
    )
    {
        _communicationUnitOfWork = communicationUnitOfWork;
        _securityContextAccessor = securityContextAccessor;
        _logger = logger;
    }

    public async Task<PagedResponse<IReadOnlyList<GetListNotificationResponse>>> GetListNotification(int pageNumber, int pageSize, string searchName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchName))
        {
            var totalItems = await _communicationUnitOfWork.NotificationMessageRepository.CountAsync(x => !x.IsDeleted, cancellationToken);
            var notificationMessages = await _communicationUnitOfWork.NotificationMessageRepository.GetPagedReponseAsync(pageNumber, pageSize, cancellationToken);
            var response = ConvertToGetListNotificationResponse(notificationMessages);
            return new PagedResponse<IReadOnlyList<GetListNotificationResponse>>(response, pageNumber, pageSize, totalItems);
        }
        else
        {
            var totalItems = await _communicationUnitOfWork.NotificationMessageRepository.CountAsync(x => !x.IsDeleted && x.Title.Contains(searchName), cancellationToken);
            var notificationMessages = await _communicationUnitOfWork.NotificationMessageRepository.SearchAsync(x => !x.IsDeleted && x.Title.Contains(searchName), pageNumber, pageSize, cancellationToken);
            var response = ConvertToGetListNotificationResponse(notificationMessages);
            return new PagedResponse<IReadOnlyList<GetListNotificationResponse>>(response, pageNumber, pageSize, totalItems);
        }
    }

    public async Task<PagedResponse<IReadOnlyList<GetListNotificationByUserResponse>>> GetListNotificationByUserId(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var totalItems = await _communicationUnitOfWork.NotificationMessageRepository.CountNotificationByUserAsync(userId, cancellationToken);
        var userNotifications = await _communicationUnitOfWork.NotificationMessageRepository.GetUserNotificationPagedReponseAsync(userId, pageNumber, pageSize, cancellationToken);

        var response = ConvertToGetListNotificationByUserResponse(userNotifications);

        return new PagedResponse<IReadOnlyList<GetListNotificationByUserResponse>>(response, pageNumber, pageSize, totalItems);
    }

    public async Task<Response<GetNotificationByIdResponse>> GetNotificationById(Guid notificationId, CancellationToken cancellationToken)
    {
        var notificationMessage = await _communicationUnitOfWork.NotificationMessageRepository.GetByIdAsync(notificationId, cancellationToken);
        if (notificationMessage == null)
        {
            _logger.LogError($"Notification with id {notificationId} not found");
            return new Response<GetNotificationByIdResponse>(ErrorCodeEnum.NOTI_ERR_001);
        }
        var response = ConvertToGetNotificationByIdResponse(notificationMessage);

        return new Response<GetNotificationByIdResponse>(response);
    }

    public async Task<Response<GetTopNotificationUnreadResponse>> GetTopNotificationUnreadByUser(CancellationToken cancellationToken)
    {
        var userId = _securityContextAccessor.UserId;
        var totalUnread = await _communicationUnitOfWork.NotificationMessageRepository.CountNotificationUnreadByUser(userId, cancellationToken);
        var userNotifications = await _communicationUnitOfWork.NotificationMessageRepository.GetTopNotificationUnreadByUser(userId, cancellationToken);

        var response = ConvertToGetTopNotificationUnreadResponse(userNotifications);
        response.TotalUnread = totalUnread;

        return new Response<GetTopNotificationUnreadResponse>(response);
    }

    public async Task<Response<MarkNotificationAsReadResponse>> MarkNotificationAsRead(MarkNotificationAsReadRequest request, CancellationToken cancellationToken)
    {
        var userId = _securityContextAccessor.UserId;
        var notificationMessages = await _communicationUnitOfWork.NotificationMessageRepository.GetUserNotificationByIds(userId, request.NotificationIds, cancellationToken, asNoTracking: false);
        if (notificationMessages == null || notificationMessages.Count == 0 || notificationMessages.Count != request.NotificationIds.Count)
        {
            _logger.LogError($"Notification with ids {string.Join(",", request.NotificationIds)} not found.");
            return new Response<MarkNotificationAsReadResponse>(ErrorCodeEnum.NOTI_ERR_001);
        }

        foreach (var notificationMessage in notificationMessages)
        {
            var notificationUser = notificationMessage.NotificationUsers.FirstOrDefault(x => x.UserId == userId);
            if (notificationUser != null)
            {
                notificationUser.IsRead = true;
                await _communicationUnitOfWork.NotificationMessageRepository.UpdateAsync(notificationMessage, cancellationToken);
            }
        }

        return new Response<MarkNotificationAsReadResponse>(new MarkNotificationAsReadResponse
        {
            NotificationIds = notificationMessages.Select(x => x.Id).ToList()
        });
    }

    #region Private Methods

    private static GetTopNotificationUnreadResponse ConvertToGetTopNotificationUnreadResponse(IReadOnlyList<NotificationMessage> userNotifications)
        => new()
        {
            ListNotification = userNotifications.Select(x => new GetListNotificationByUserResponse
            {
                Id = x.Id,
                Title = x.Title,
                ContentNotify = x.ContentNotify,
                ReferenceData = x.ReferenceData,
                NotificationType = GetNotificationTypeFromEntity(x.NotificationType),
                IsRead = x.NotificationUsers.First().IsRead
            }).ToList()
        };

    private static NotificationTypeDto GetNotificationTypeFromEntity(NotificationType notificationType)
    {
        return notificationType switch
        {
            NotificationType.Group => NotificationTypeDto.Group,
            NotificationType.User => NotificationTypeDto.User,
            _ => throw new ArgumentOutOfRangeException(nameof(notificationType), notificationType, null)
        };
    }

    private static IReadOnlyList<GetListNotificationByUserResponse> ConvertToGetListNotificationByUserResponse(IReadOnlyList<NotificationMessage> userNotifications)
        => userNotifications.Select(x => new GetListNotificationByUserResponse
        {
            Id = x.Id,
            Title = x.Title,
            ContentNotify = x.ContentNotify,
            ReferenceData = x.ReferenceData,
            NotificationType = GetNotificationTypeFromEntity(x.NotificationType),
            IsRead = x.NotificationUsers.First().IsRead
        }).ToList().AsReadOnly();

    private static IReadOnlyList<GetListNotificationResponse> ConvertToGetListNotificationResponse(IReadOnlyList<NotificationMessage> userNotifications)
        => userNotifications.Select(x => new GetListNotificationResponse
        {
            Id = x.Id,
            Title = x.Title,
            ContentNotify = x.ContentNotify,
            ReferenceData = x.ReferenceData,
            NotificationType = GetNotificationTypeFromEntity(x.NotificationType),
            Users = x.NotificationUsers.Select(y => new NotificationUserResponse
            {
                Id = y.Id,
                NotificationId = x.Id,
                UserId = y.UserId,
                IsRead = y.IsRead,
                Created = y.Created
            }).ToList()
        }).ToList().AsReadOnly();

    private static GetNotificationByIdResponse ConvertToGetNotificationByIdResponse(NotificationMessage notificationMessage)
    {
        var response = new GetNotificationByIdResponse
        {
            Id = notificationMessage.Id,
            Title = notificationMessage.Title,
            ContentNotify = notificationMessage.ContentNotify,
            ReferenceData = notificationMessage.ReferenceData,
            NotificationType = GetNotificationTypeFromEntity(notificationMessage.NotificationType),
            Users = notificationMessage.NotificationUsers.Select(y => new NotificationUserResponse
            {
                Id = y.Id,
                NotificationId = notificationMessage.Id,
                UserId = y.UserId,
                IsRead = y.IsRead,
                Created = y.Created
            }).ToList()
        };
        return response;
    }

    #endregion
}
