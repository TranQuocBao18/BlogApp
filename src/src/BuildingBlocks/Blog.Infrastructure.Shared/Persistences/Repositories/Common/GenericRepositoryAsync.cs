using System;
using System.Linq.Expressions;
using Blog.Domain.Shared.Common;
using Blog.Infrastructure.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Shared.Persistences.Repositories.Common;

public class GenericRepositoryAsync<TEntity, TKey> : IGenericRepository<TEntity, Guid> where TEntity : BaseEntityWithAudit
{
    private readonly DbContext _dbContext;

    public GenericRepositoryAsync(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public GenericRepositoryAsync()
    {
    }

    /// <summary>
    /// Get queryable for custom queries in derived repositories
    /// </summary>
    protected IQueryable<TEntity> Query(bool includeDeleted = false)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();

        if (!includeDeleted)
        {
            query = query.Where(x => !x.IsDeleted);
        }

        return query;
    }

    /// <summary>
    /// Get queryable with includes
    /// </summary>
    protected IQueryable<TEntity> QueryWithIncludes(
        bool includeDeleted = false,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = Query(includeDeleted);

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query;
    }

    #region Read Methods
    public virtual async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool includedDeleted = false)
    {
        return await Query(includedDeleted)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetPagedReponseAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await Query()
            .OrderByDescending(x => x.Created)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Query()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await Query()
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async virtual Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await Query()
            .AllAsync(predicate, cancellationToken);
    }

    public async virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await Query()
            .AnyAsync(predicate, cancellationToken);
    }

    public async virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await Query()
            .CountAsync(predicate, cancellationToken);
    }

    public async virtual Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken)
    {
        return await Query()
            .MaxAsync(selector, cancellationToken);
    }

    public async virtual Task<TResult> MinAsync<TResult>(Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken)
    {
        return await Query()
            .MinAsync(selector, cancellationToken);
    }

    #endregion

    #region Write Methods
    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken, bool hasTransaction = false)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        if (!hasTransaction)
        {
            await _dbContext.SaveChangesAsync();
        }
        return entity;
    }

    public virtual async Task<bool> AddRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken, bool hasTransaction = false)
    {
        await _dbContext.Set<TEntity>().AddRangeAsync(entities);
        if (!hasTransaction)
        {
            await _dbContext.SaveChangesAsync();
        }
        return true;
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool hasTransaction = false)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        if (!hasTransaction)
        {
            await _dbContext.SaveChangesAsync();
        }
    }

    public virtual async Task UpdateRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken, bool hasTransaction = false)
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);
        if (!hasTransaction)
        {
            await _dbContext.SaveChangesAsync();
        }
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool hasTransaction = false)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        if (!hasTransaction)
        {
            await _dbContext.SaveChangesAsync();
        }
    }

    public virtual async Task DeleteRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken, bool hasTransaction = false)
    {
        _dbContext.Set<TEntity>().RemoveRange(entities);
        if (!hasTransaction)
        {
            await _dbContext.SaveChangesAsync();
        }
    }

    public virtual async Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken, bool hasTransaction = false)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        entity.IsDeleted = true;
        if (!hasTransaction)
        {
            await _dbContext.SaveChangesAsync();
        }
    }

    public virtual async Task SoftDeleteRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken, bool hasTransaction = false)
    {
        foreach (var entity in entities)
        {
            _dbContext.Entry(entity).State = EntityState.Detached;
            entity.IsDeleted = true;
        }
        if (!hasTransaction)
        {
            await _dbContext.SaveChangesAsync();
        }
    }

    IQueryable<TEntity> IGenericRepository<TEntity, Guid>.Query(bool includeDeleted)
    {
        return Query(includeDeleted);
    }

    IQueryable<TEntity> IGenericRepository<TEntity, Guid>.QueryWithIncludes(bool includeDeleted, params Expression<Func<TEntity, object>>[] includes)
    {
        return QueryWithIncludes(includeDeleted, includes);
    }

    public IQueryable<TEntity> AsQueryable()
    {
        throw new NotImplementedException();
    }

    #endregion
}
