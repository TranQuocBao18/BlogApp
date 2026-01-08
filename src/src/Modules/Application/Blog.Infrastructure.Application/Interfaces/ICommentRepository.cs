using System;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Shared.Interfaces;

namespace Blog.Infrastructure.Application.Interfaces;

public interface ICommentRepository : IGenericRepository<Comment, Guid>
{
    Task<IReadOnlyList<(Comment Comment, int ReplyCount)>> GetCommentsByBlogIdAsync(
        Guid blogId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken
    );

    Task<IReadOnlyList<Comment>> GetRepliesByParentIdAsync(
        Guid parentId,
        CancellationToken cancellationToken
    );
}
