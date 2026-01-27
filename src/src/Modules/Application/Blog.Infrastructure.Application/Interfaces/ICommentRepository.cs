using System;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Shared.Interfaces;

namespace Blog.Infrastructure.Application.Interfaces;

public interface ICommentRepository : IGenericRepository<Comment, Guid>
{
    Task<IReadOnlyList<(Comment Comment, int ReplyCount, bool isLiked)>> GetCommentsByBlogIdAsync(
        Guid blogId,
        Guid? currentUserId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken
    );

    Task<IReadOnlyList<(Comment Comment, int ReplyCount, bool isLiked)>> GetRepliesByParentIdAsync(
        Guid parentId,
        Guid blogId,
        Guid? currentUserId,
        CancellationToken cancellationToken
    );
}
