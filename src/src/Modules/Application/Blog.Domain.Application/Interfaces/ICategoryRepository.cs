using System;
using System.Linq.Expressions;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Shared.Interfaces;

namespace Blog.Domain.Application.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category, Guid>
{
    Task<IReadOnlyList<Category>> SearchAsync(Expression<Func<Category, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken);
}
