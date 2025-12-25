using System;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Application.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Application.Repositories;

public class CategoryRepository : GenericRepositoryAsync<Category, Guid>, ICategoryRepository
{
    private readonly DbContext _dbContext;
    public CategoryRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Category> GetByCategorySlugAsync(string slug, CancellationToken cancellationToken, bool includedDeleted = false)
    {
        if (includedDeleted)
        {
            return await _dbContext.Set<Category>().Where(x => x.Slug == slug).FirstOrDefaultAsync();
        }
        return await _dbContext.Set<Category>().Where(x => !x.IsDeleted && x.Slug == slug).FirstOrDefaultAsync();
    }
}
