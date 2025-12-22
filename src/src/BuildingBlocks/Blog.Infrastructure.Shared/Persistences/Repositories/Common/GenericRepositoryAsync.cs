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

    #region Read Methods

    #endregion

    #region Write Methods

    #endregion


    public virtual async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool includedDeleted = false)
    {
        if (includedDeleted)
        {
            return await _dbContext.Set<TEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        return await _dbContext.Set<TEntity>().Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetPagedReponseAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<TEntity>()
            .Where(x => !x.IsDeleted)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .OrderByDescending(x => x.Created)
            .ToListAsync();
    }

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

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Set<TEntity>().Where(x => !x.IsDeleted).ToListAsync();
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public async virtual Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<TEntity>().Where(x => !x.IsDeleted).AllAsync(predicate, cancellationToken);
    }

    public async virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<TEntity>().Where(x => !x.IsDeleted).AnyAsync(predicate, cancellationToken);
    }

    public async virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<TEntity>().Where(x => !x.IsDeleted).CountAsync(predicate, cancellationToken);
    }

    public async virtual Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> predicate, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<TEntity>().Where(x => !x.IsDeleted).MaxAsync(predicate, cancellationToken);
    }

    public async virtual Task<TResult> MinAsync<TResult>(Expression<Func<TEntity, TResult>> predicate, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<TEntity>().Where(x => !x.IsDeleted).MinAsync(predicate, cancellationToken);
    }

    public IQueryable<TEntity> AsQueryable()
    {
        throw new NotImplementedException();
    }
}
