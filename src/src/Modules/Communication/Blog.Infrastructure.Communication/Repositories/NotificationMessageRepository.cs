using System;
using System.Linq.Expressions;
using Blog.Domain.Communication.Entities;
using Blog.Infrastructure.Communication.Contexts;
using Blog.Domain.Communication.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Communication.Repositories;

public class NotificationMessageRepository : GenericRepositoryAsync<NotificationMessage, Guid>, INotificationMessageRepository
{
    private readonly CommunicationDbContext _dbContext;
    private readonly DbSet<NotificationUser> _notificationUserDbSet;

    public NotificationMessageRepository(CommunicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
        _notificationUserDbSet = dbContext.Set<NotificationUser>();
    }

    public async Task<IList<Guid>> AddGroupNotificationAsync(IList<Guid> userIds, NotificationMessage notificationMessage, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notificationMessage, nameof(notificationMessage));

        await this.AddAsync(notificationMessage, cancellationToken);

        var notificationUsers = userIds.Select(userId => new NotificationUser
        {
            UserId = userId,
            NotificationId = notificationMessage.Id,
            IsRead = false,
            Created = DateTime.UtcNow,
        }).ToList();

        await _notificationUserDbSet.AddRangeAsync(notificationUsers);

        return notificationUsers.Select(n => n.Id).ToList();
    }

    public async Task<IList<Guid>> AddUsersNotificationAsync(IList<Guid> userIds, NotificationMessage notificationMessage, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notificationMessage, nameof(notificationMessage));

        await this.AddAsync(notificationMessage, cancellationToken);

        var notificationUsers = userIds.Select(userId => new NotificationUser
        {
            UserId = userId,
            NotificationId = notificationMessage.Id,
            IsRead = false,
            Created = DateTime.UtcNow,
        }).ToList();

        await _notificationUserDbSet.AddRangeAsync(notificationUsers);

        return notificationUsers.Select(n => n.Id).ToList();
    }

    public async Task<int> CountNotificationByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<NotificationMessage>()
            .Include(x => x.NotificationUsers)
            .AsNoTracking()
            .CountAsync(notificationMessage => notificationMessage.NotificationUsers.Any(nu => nu.UserId == userId) && !notificationMessage.IsDeleted, cancellationToken);
    }

    public async Task<int> CountNotificationUnreadByUser(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<NotificationMessage>()
            .Include(x => x.NotificationUsers)
            .AsNoTracking()
            .CountAsync(x => !x.IsDeleted && x.NotificationUsers.Any(nu => nu.UserId == userId && nu.IsRead == false), cancellationToken);
    }

    public async Task<IReadOnlyList<NotificationMessage>> GetTopNotificationUnreadByUser(Guid userId, CancellationToken cancellationToken = default, int quantity = 10)
    {
        return await _dbContext
            .Set<NotificationMessage>()
            .Include(x => x.NotificationUsers)
            .AsNoTracking()
            .Where(x => !x.IsDeleted && x.NotificationUsers.Any(nu => nu.UserId == userId && nu.IsRead == false))
            .OrderByDescending(x => x.Created)
            .Take(quantity)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<NotificationMessage>> GetUserNotificationByIds(Guid userId, IList<Guid> notificationIds, CancellationToken cancellationToken = default, bool asNoTracking = true)
    {
        var query = Query();
        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }
        else
        {
            query = query.AsTracking();
        }

        return await query
            .Include(x => x.NotificationUsers)
            .Where(x => !x.IsDeleted && x.NotificationUsers.Any(nu => nu.UserId == userId) && notificationIds.Contains(x.Id))
            .OrderByDescending(x => x.Created)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<NotificationMessage>> GetUserNotificationPagedReponseAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<NotificationMessage>()
            .Include(x => x.NotificationUsers)
            .AsNoTracking()
            .Where(x => !x.IsDeleted && x.NotificationUsers.Any(nu => nu.UserId == userId))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .OrderByDescending(x => x.Created)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<NotificationMessage>> SearchAsync(Expression<Func<NotificationMessage, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<NotificationMessage>()
            .Include(x => x.NotificationUsers)
            .Where(predicate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .OrderByDescending(x => x.Created)
            .ToListAsync(cancellationToken);
    }
}
