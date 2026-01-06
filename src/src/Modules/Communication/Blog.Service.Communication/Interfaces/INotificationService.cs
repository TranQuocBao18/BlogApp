using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Communication.Requests;
using Blog.Model.Dto.Communication.Response;

namespace Blog.Service.Communication.Interfaces;

public interface INotificationService
{
    Task<PagedResponse<IReadOnlyList<GetListNotificationResponse>>> GetListNotification(int pageNumber, int pageSize, string searchName, CancellationToken cancellationToken);
    Task<PagedResponse<IReadOnlyList<GetListNotificationByUserResponse>>> GetListNotificationByUserId(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Response<GetNotificationByIdResponse>> GetNotificationById(Guid notificationId, CancellationToken cancellationToken);
    Task<Response<MarkNotificationAsReadResponse>> MarkNotificationAsRead(MarkNotificationAsReadRequest request, CancellationToken cancellationToken);
    Task<Response<GetTopNotificationUnreadResponse>> GetTopNotificationUnreadByUser(CancellationToken cancellationToken);
}
