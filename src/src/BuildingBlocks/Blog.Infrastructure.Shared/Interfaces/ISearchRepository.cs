using System;
using Blog.Domain.Shared.Common;

namespace Blog.Infrastructure.Shared.Interfaces;

public interface ISearchRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    IQueryable<TEntity> AsQueryable();
}
