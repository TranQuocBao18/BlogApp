using System;
using System.Linq.Expressions;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Application.Context;
using Blog.Domain.Application.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Application.Repositories;

public class TagRepository : GenericRepositoryAsync<Tag, Guid>, ITagRepository
{
    public TagRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IReadOnlyList<Tag>> SearchAsync(Expression<Func<Tag, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken)
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
