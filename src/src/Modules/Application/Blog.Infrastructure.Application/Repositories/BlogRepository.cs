using System;
using Blog.Domain.Application.Entities;
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

    public async Task<BlogEntity> GetByBlogSlugAsync(string slug, CancellationToken cancellationToken, bool includedDeleted = false)
    {
        if (includedDeleted)
        {
            return await _dbContext.Set<BlogEntity>().Where(x => x.Slug == slug).FirstOrDefaultAsync();
        }
        return await _dbContext.Set<BlogEntity>().Where(x => !x.IsDeleted && x.Slug == slug).FirstOrDefaultAsync();
    }
}
