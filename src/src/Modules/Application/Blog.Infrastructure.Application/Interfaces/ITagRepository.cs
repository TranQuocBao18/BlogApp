using System;
using System.Linq.Expressions;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Shared.Interfaces;

namespace Blog.Infrastructure.Application.Interfaces;

public interface ITagRepository : IGenericRepository<Tag, Guid>
{
    Task<IReadOnlyList<Tag>> SearchAsync(Expression<Func<Tag, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken);
}
