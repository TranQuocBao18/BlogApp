using System;
using System.Linq.Expressions;
using Blog.Domain.Application.Entities;
using Blog.Domain.Application.Enum;
using Blog.Infrastructure.Application.Context;
using Blog.Domain.Application.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Application.Repositories;

public class BlogRepository : GenericRepositoryAsync<BlogEntity, Guid>, IBlogRepository
{
    private readonly ApplicationDbContext _dbContext;
    public BlogRepository(ApplicationDbContext dbContext) : base(dbContext)
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

    public async Task<(BlogEntity? blog, int likeCount, bool isLiked)> GetByPredicateWithStatsAsync(
        Expression<Func<BlogEntity, bool>> predicate,
        Guid? currentUserId,
        CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Set<BlogEntity>()
            .Where(predicate)
            .Include(b => b.Category)
            .Include(b => b.Banner)
            .Include(b => b.BlogTags)
                .ThenInclude(bt => bt.Tag)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
            return (null, 0, false);

        var likeCount = await _dbContext.Set<BlogLike>()
            .Where(l => l.BlogId == result.Id && !l.IsDeleted)
            .CountAsync(cancellationToken);

        var isLiked = currentUserId.HasValue &&
            await _dbContext.Set<BlogLike>()
                .AnyAsync(l => l.BlogId == result.Id && l.UserId == currentUserId.Value && !l.IsDeleted, cancellationToken);

        return (result, likeCount, isLiked);
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
