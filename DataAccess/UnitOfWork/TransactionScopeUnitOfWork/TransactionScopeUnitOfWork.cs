using System;
using System.Threading.Tasks;
using System.Transactions;

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

        public Task CommitAsync()
        {
            return Task.Run(() => this.transactionScope.Complete());
        }
    }
}
