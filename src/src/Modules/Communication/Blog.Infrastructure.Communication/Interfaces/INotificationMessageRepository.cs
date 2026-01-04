using System;
using System.Linq.Expressions;
using Blog.Domain.Communication.Entities;
using Blog.Infrastructure.Shared.Interfaces;

namespace Blog.Infrastructure.Communication.Interfaces;

public interface INotificationMessageRepository : IGenericRepository<NotificationMessage, Guid>
{
    Task<IReadOnlyList<NotificationMessage>> SearchAsync(Expression<Func<NotificationMessage, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<IReadOnlyList<NotificationMessage>> GetUserNotificationPagedReponseAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<int> CountNotificationByUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<IList<Guid>> AddUsersNotificationAsync(IList<Guid> userIds, NotificationMessage notificationMessage, CancellationToken cancellationToken);
    Task<IList<Guid>> AddGroupNotificationAsync(IList<Guid> userIds, NotificationMessage notificationMessage, CancellationToken cancellationToken);
    Task<IReadOnlyList<NotificationMessage>> GetTopNotificationUnreadByUser(Guid userId, CancellationToken cancellationToken = default, int quantity = 10);
    Task<int> CountNotificationUnreadByUser(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<NotificationMessage>> GetUserNotificationByIds(Guid userId, IList<Guid> notificationIds, CancellationToken cancellationToken = default, bool asNoTracking = true);
}
