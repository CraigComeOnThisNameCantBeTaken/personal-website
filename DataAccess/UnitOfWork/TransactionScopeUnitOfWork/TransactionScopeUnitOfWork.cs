using System;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Logging;

namespace DataAccess.UnitOfWork.TransactionScopeUnitOfWork
{
    public class TransactionScopeUnitOfWork : IScopedUnitOfWork
    {
        private bool disposed = false;

        private readonly TransactionScope transactionScope;
        private readonly ILogger<TransactionScopeUnitOfWork> logger;

        public TransactionScopeUnitOfWork(ILogger<TransactionScopeUnitOfWork> logger)
        {
            this.transactionScope = new TransactionScope(
                    TransactionScopeOption.RequiresNew,
                    new TransactionOptions
                    {
                        IsolationLevel = IsolationLevel.ReadCommitted,
                        Timeout = TransactionManager.MaximumTimeout
                    },
                    TransactionScopeAsyncFlowOption.Enabled);
            this.logger = logger;
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
            return Task.Run(() =>
            {
                try
                {
                    this.transactionScope.Complete();
                    logger.LogDebug("Transaction completed");
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, "Unit of work failed to commit");
                    throw;
                }
            });
        }
    }
}
