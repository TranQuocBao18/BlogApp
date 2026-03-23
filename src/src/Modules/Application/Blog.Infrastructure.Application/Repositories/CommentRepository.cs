using System;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Application.Context;
using Blog.Domain.Application.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Blog.Infrastructure.Application.Repositories;

public class CommentRepository : GenericRepositoryAsync<Comment, Guid>, ICommentRepository
{
    private readonly ApplicationDbContext _dbContext;
    public CommentRepository(ApplicationDbContext dbContext) : base(dbContext)
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
                RepliesCount = c.ChildComments.Count(),
                IsLiked = currentUserId.HasValue &&
                    c.Likes.Any(l => l.UserId == currentUserId.Value)
            })
            .ToListAsync(cancellationToken);

        var res = new List<(Comment, int, bool)>();
        foreach (var item in result)
        {
            res.Add((item.CommentEntity!, item.RepliesCount, item.IsLiked));
        }

        return res;
    }

    public async Task<IReadOnlyList<(Comment Comment, int ReplyCount, bool isLiked)>> GetRepliesByParentIdAsync(Guid parentId, Guid blogId, Guid? currentUserId, CancellationToken cancellationToken)
    {
        var query = Query()
            .AsNoTracking()
            .Where(c => c.BlogId == blogId && c.ParentId == parentId);

        var result = await query
            .OrderBy(c => c.Created)
            .Select(c => new
            {
                CommentEntity = c,
                RepliesCount = c.ChildComments.Count(),
                IsLiked = currentUserId.HasValue &&
                    c.Likes.Any(l => l.UserId == currentUserId.Value)
            })
            .ToListAsync(cancellationToken);

        var res = new List<(Comment, int, bool)>();
        foreach (var item in result)
        {
            res.Add((item.CommentEntity!, item.RepliesCount, item.IsLiked));
        }
        return res;
    }

    public async Task<IReadOnlyList<Comment>> SearchAsync(Expression<Func<Comment, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await Query()
            .Where(predicate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .OrderByDescending(x => x.Created)
            .ToListAsync(cancellationToken);
    }
}
