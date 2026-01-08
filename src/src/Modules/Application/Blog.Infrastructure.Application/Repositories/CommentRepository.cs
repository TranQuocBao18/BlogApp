using System;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Application.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Application.Repositories;

public class CommentRepository : GenericRepositoryAsync<Comment, Guid>, ICommentRepository
{
    public CommentRepository(DbContext dbContext) : base(dbContext)
    {

    }

    public async Task<IReadOnlyList<(Comment Comment, int ReplyCount)>> GetCommentsByBlogIdAsync(Guid blogId, int pageNumber, int pageSize, CancellationToken cancellationToken)
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
                RepliesCount = c.ChildComments.Count() // Đây là đoạn SQL Count(*)
            })
            .ToListAsync(cancellationToken);

        var res = new List<(Comment, int)>();
        foreach (var item in result)
        {
            if (item.CommentEntity != null && item.UserEntity != null)
            {
                item.CommentEntity.User = item.UserEntity;
            }

            res.Add((item.CommentEntity!, item.RepliesCount));
        }

        return res;
    }

    public async Task<IReadOnlyList<Comment>> GetRepliesByParentIdAsync(Guid parentId, CancellationToken cancellationToken)
    {
        return await Query()
            .Include(c => c.User)
            .Where(c => c.ParentId == parentId)
            .OrderBy(c => c.Created)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
