using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using DataAccess.Repository;

namespace DataAccess.UnitOfWork.TransactionScopeUnitOfWork
{
    public class TransactionScopeUnitOfWork : IScopedUnitOfWork
    {
        private bool disposed = false;

        private readonly TransactionScope transactionScope;

        public TransactionScopeUnitOfWork()
        {
            this.transactionScope = new TransactionScope(
                    TransactionScopeOption.RequiresNew,
                    new TransactionOptions
                    {
                        IsolationLevel = IsolationLevel.ReadCommitted,
                        Timeout = TransactionManager.MaximumTimeout
                    });
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this.transactionScope.Dispose();
                }

                disposed = true;
            }
        }

        public async Task CommitAsync(IRepository<object> repository)
        {
            await repository.CommitAsync();
            this.transactionScope.Complete();
        }

        public async Task CommitAsync(IEnumerable<IRepository<object>> repositories)
        {
            await Task.WhenAll(repositories.Select(r => CommitAsync(r)));;
        }
    }
}
