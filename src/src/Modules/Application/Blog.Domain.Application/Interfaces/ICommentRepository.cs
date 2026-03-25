using System;
using System.Linq.Expressions;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Shared.Interfaces;

namespace Blog.Domain.Application.Interfaces;

public interface ICommentRepository : IGenericRepository<Comment, Guid>
{
    Task<IReadOnlyList<(Comment Comment, int ReplyCount, bool isLiked)>> GetCommentsByBlogIdAsync(
        Guid blogId,
        Guid? currentUserId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken
    );

    Task<IReadOnlyList<(Comment Comment, int likeCount, int ReplyCount)>> GetAllCommentsAsync(
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
    Task<IReadOnlyList<Comment>> SearchAsync(Expression<Func<Comment, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<IReadOnlyList<Comment>> GetPagedReponseAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken
    );
}
