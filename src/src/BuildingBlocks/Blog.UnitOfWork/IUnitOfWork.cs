using System;
using System.Data;

namespace Blog.UnitOfWork;

public interface IUnitOfWork
{
    IDbConnection Connection { get; }
    Task BeginTransactionAsync(IsolationLevel level = IsolationLevel.ReadCommitted);
    Task CommitAsync();
    Task RollbackAsync();
}
