using System;
using System.Linq.Expressions;
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
    public async Task<IReadOnlyList<Category>> SearchAsync(Expression<Func<Category, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await Query()
            .Where(predicate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .OrderByDescending(x => x.Created)
            .ToListAsync(cancellationToken);
    }
}
