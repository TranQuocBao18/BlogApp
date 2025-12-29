using System;
using System.Linq.Expressions;
using Blog.Domain.Application.Entities;
using Blog.Domain.Application.Enum;
using Blog.Infrastructure.Application.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Application.Repositories;

public class BlogRepository : GenericRepositoryAsync<BlogEntity, Guid>, IBlogRepository
{
    private readonly DbContext _dbContext;
    public BlogRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BlogEntity?> GetByBlogSlugAsync(string slug, CancellationToken cancellationToken, bool includedDeleted = false)
    {
        return await Query(includedDeleted)
            .FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);
    }

    public async Task<IReadOnlyList<BlogEntity>> SearchAsync(Expression<Func<BlogEntity, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await Query()
            .Where(predicate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .OrderByDescending(x => x.Created)
            .ToListAsync(cancellationToken);
    }

    // public async Task<BlogEntity?> GetBySlugWithRelationsAsync(
    //     string slug,
    //     CancellationToken cancellationToken = default)
    // {
    //     return await Query()
    //         .Include(b => b.Category)
    //         .Include(b => b.Banner)
    //         .Include(b => b.BlogTags)
    //             .ThenInclude(bt => bt.Tag)
    //         .AsSplitQuery()
    //         .FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);
    // }

    public async Task<(BlogEntity? blog, int likeCount, bool isLiked)> GetBySlugWithStatsAsync(
        string slug,
        Guid? currentUserId,
        CancellationToken cancellationToken = default)
    {
        // Single query với subqueries
        var result = await Query()
            .Where(b => b.Slug == slug)
            .Select(b => new
            {
                Blog = b,
                LikeCount = b.Likes.Count(),
                IsLiked = currentUserId.HasValue &&
                    b.Likes.Any(l => l.UserId == currentUserId.Value)
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
            return (null, 0, false);

        // Load relationships
        await _dbContext.Entry(result.Blog)
            .Reference(b => b.Category)
            .LoadAsync(cancellationToken);

        await _dbContext.Entry(result.Blog)
            .Reference(b => b.Banner)
            .LoadAsync(cancellationToken);

        await _dbContext.Entry(result.Blog)
            .Collection(b => b.BlogTags)
            .Query()
            .Include(bt => bt.Tag)
            .LoadAsync(cancellationToken);

        return (result.Blog, result.LikeCount, result.IsLiked);
    }

    public async Task<List<BlogEntity>> GetPublishedBlogsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(b => b.Category)
            .Include(b => b.Banner)
            .Where(b => b.Status == BlogStatus.Published)
            .OrderByDescending(b => b.Created)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<List<BlogEntity>> GetBlogsByCategoryAsync(
        Guid categoryId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(b => b.Banner)
            .Where(b => b.CategoryId == categoryId && b.Status == BlogStatus.Published)
            .OrderByDescending(b => b.Created)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<List<BlogEntity>> GetBlogsByTagAsync(
        Guid tagId,
        CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(b => b.Banner)
            .Include(b => b.Category)
            .Where(b => b.BlogTags.Any(bt => bt.TagId == tagId) &&
                       b.Status == BlogStatus.Published)
            .OrderByDescending(b => b.Created)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetLikeCountAsync(
        Guid blogId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<BlogLike>()
            .CountAsync(l => l.BlogId == blogId, cancellationToken);
    }

    public async Task<bool> IsLikedByUserAsync(
        Guid blogId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<BlogLike>()
            .AnyAsync(l => l.BlogId == blogId && l.UserId == userId, cancellationToken);
    }
}
