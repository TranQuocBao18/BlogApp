using System;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Shared.Interfaces;

namespace Blog.Infrastructure.Application.Interfaces;

public interface IBlogRepository : IGenericRepository<BlogEntity, Guid>
{
    Task<BlogEntity?> GetByBlogSlugAsync(string slug, CancellationToken cancellationToken, bool includedDeleted = false);
    // Task<BlogEntity?> GetBySlugWithRelationsAsync(string slug, CancellationToken cancellationToken);
    Task<(BlogEntity? blog, int likeCount, bool isLiked)> GetBySlugWithStatsAsync(string slug, Guid? currentUserId, CancellationToken cancellationToken = default);
    Task<List<BlogEntity>> GetPublishedBlogsAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<List<BlogEntity>> GetBlogsByCategoryAsync(Guid categoryId, int page, int pageSize, CancellationToken cancellationToken);
    Task<List<BlogEntity>> GetBlogsByTagAsync(Guid tagId, CancellationToken cancellationToken);

    // Statistics
    Task<int> GetLikeCountAsync(Guid blogId, CancellationToken cancellationToken);
    Task<bool> IsLikedByUserAsync(Guid blogId, Guid userId, CancellationToken cancellationToken);
}
