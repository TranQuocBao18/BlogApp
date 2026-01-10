using System;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Application.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Application.Repositories;

public class CommentRepository : GenericRepositoryAsync<Comment, Guid>, ICommentRepository
{
    private readonly DbContext _dbContext;
    public CommentRepository(DbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<(Comment Comment, int ReplyCount, bool isLiked)>> GetCommentsByBlogIdAsync(Guid blogId, Guid? currentUserId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = Query()
            .AsNoTracking()
            .Where(c => c.BlogId == blogId && c.ParentId == null);

        var result = await query
            .OrderByDescending(c => c.Created)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new
            {
                CommentEntity = c,
                UserEntity = c.User, // Cần select rõ User để tránh null
                RepliesCount = c.ChildComments.Count(), // Đây là đoạn SQL Count(*)
                IsLiked = currentUserId.HasValue &&
                    c.Likes.Any(l => l.UserId == currentUserId.Value)
            })
            .ToListAsync(cancellationToken);

        var res = new List<(Comment, int, bool)>();
        foreach (var item in result)
        {
            if (item.CommentEntity != null && item.UserEntity != null)
            {
                item.CommentEntity.User = item.UserEntity;
            }

            res.Add((item.CommentEntity!, item.RepliesCount, item.IsLiked));
        }

        return res;
    }

    public async Task<IReadOnlyList<(Comment Comment, int ReplyCount, bool isLiked)>> GetRepliesByParentIdAsync(Guid parentId, Guid? currentUserId, CancellationToken cancellationToken)
    {
        var query = Query()
            .AsNoTracking()
            .Where(c => c.ParentId == parentId);

        var result = await query
            .Include(c => c.User)
            .OrderBy(c => c.Created)
            .Select(c => new
            {
                CommentEntity = c,
                UserEntity = c.User, // Cần select rõ User để tránh null
                RepliesCount = c.ChildComments.Count(), // Đây là đoạn SQL Count(*)
                IsLiked = currentUserId.HasValue &&
                    c.Likes.Any(l => l.UserId == currentUserId.Value)
            })
            .ToListAsync(cancellationToken);

        var res = new List<(Comment, int, bool)>();
        foreach (var item in result)
        {
            if (item.CommentEntity != null && item.UserEntity != null)
            {
                item.CommentEntity.User = item.UserEntity;
            }

            res.Add((item.CommentEntity!, item.RepliesCount, item.IsLiked));
        }
        return res;
    }
}
