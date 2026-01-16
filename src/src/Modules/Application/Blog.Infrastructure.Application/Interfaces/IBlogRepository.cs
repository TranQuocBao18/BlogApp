using System;
using System.Linq.Expressions;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Shared.Interfaces;

namespace Blog.Infrastructure.Application.Interfaces;

public interface IBlogRepository : IGenericRepository<BlogEntity, Guid>
{
    Task<IReadOnlyList<BlogEntity>> SearchAsync(Expression<Func<BlogEntity, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<BlogEntity?> GetByBlogSlugAsync(string slug, CancellationToken cancellationToken, bool includedDeleted = false);
    // Task<BlogEntity?> GetBySlugWithRelationsAsync(string slug, CancellationToken cancellationToken);
    Task<(BlogEntity? blog, int likeCount, bool isLiked)> GetByPredicateWithStatsAsync(Expression<Func<BlogEntity, bool>> predicate, Guid? currentUserId, CancellationToken cancellationToken = default);
    Task<List<BlogEntity>> GetPublishedBlogsAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<List<BlogEntity>> GetBlogsByCategoryAsync(Guid categoryId, int page, int pageSize, CancellationToken cancellationToken);
    Task<List<BlogEntity>> GetBlogsByTagAsync(Guid tagId, CancellationToken cancellationToken);

    // Statistics
    Task<int> GetLikeCountAsync(Guid blogId, CancellationToken cancellationToken);
    Task<bool> IsLikedByUserAsync(Guid blogId, Guid userId, CancellationToken cancellationToken);
}
