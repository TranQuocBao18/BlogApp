using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Blog.UnitOfWork;

public class BaseUnitOfWork : IUnitOfWork
{
    private readonly Func<IDbConnection> _connFactory;
        private IDbConnection? _dbConnection;
        private IDbContextTransaction? _transaction;
        protected DbContext Context { get; private set; }

        public BaseUnitOfWork(DbContext context) => Context = context;
        public BaseUnitOfWork(Func<IDbConnection> connFactory) => _connFactory = connFactory;

        public IDbConnection Connection
        {
            get
            {
                if (_dbConnection == null)
                {
                    _dbConnection = _connFactory();
                }
                return _dbConnection;
            }
        }

        public virtual async Task BeginTransactionAsync(IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            _transaction = await Context.Database.BeginTransactionAsync(level);
        }

        public virtual async Task CommitAsync()
        {
            Context.SaveChanges();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
            }
        }

        public virtual async Task RollbackAsync()
        {
            await _transaction?.RollbackAsync();
            await _transaction.DisposeAsync();
        }
}
