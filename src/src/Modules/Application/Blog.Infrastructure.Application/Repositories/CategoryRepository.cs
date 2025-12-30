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
        return await Query(includedDeleted)
            .FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);
    }
}
