using System;
using System.Linq.Expressions;
using Blog.Domain.Shared.Common;

namespace Blog.Infrastructure.Shared.Interfaces;

public interface IGenericRepository<TEntity, TKey> : ISearchRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool includedDeleted = false);

    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<IReadOnlyList<TEntity>> GetPagedReponseAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken, bool hasTransaction = false);
    Task<bool> AddRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken, bool hasTransaction = false);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool hasTransaction = false);
    Task UpdateRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken, bool hasTransaction = false);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool hasTransaction = false);
    Task DeleteRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken, bool hasTransaction = false);
    Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken, bool hasTransaction = false);
    Task SoftDeleteRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken, bool hasTransaction = false);

    Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> predicate, CancellationToken cancellationToken);
    Task<TResult> MinAsync<TResult>(Expression<Func<TEntity, TResult>> predicate, CancellationToken cancellationToken);
}
