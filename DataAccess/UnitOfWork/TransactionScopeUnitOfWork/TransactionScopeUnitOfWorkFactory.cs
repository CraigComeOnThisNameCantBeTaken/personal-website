namespace DataAccess.UnitOfWork.TransactionScopeUnitOfWork
{
    public class TransactionScopeUnitOfWorkFactory : IScopedUnitOfWorkFactory
    {
        public IScopedUnitOfWork Create()
        {
            return new TransactionScopeUnitOfWork();
        }
    }
}
