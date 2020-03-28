using Microsoft.Extensions.Logging;

namespace DataAccess.UnitOfWork.TransactionScopeUnitOfWork
{
    public class TransactionScopeUnitOfWorkFactory : IScopedUnitOfWorkFactory
    {
        private readonly ILogger<TransactionScopeUnitOfWork> logger;

        // logger factory could be used if more loggers were needed with different generics
        public TransactionScopeUnitOfWorkFactory(ILogger<TransactionScopeUnitOfWork> logger)
        {
            this.logger = logger;
        }
        
        public IScopedUnitOfWork Create()
        {
            return new TransactionScopeUnitOfWork(logger);
        }
    }
}
