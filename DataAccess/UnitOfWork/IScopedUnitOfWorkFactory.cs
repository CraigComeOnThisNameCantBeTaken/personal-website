
namespace DataAccess.UnitOfWork
{
    public interface IScopedUnitOfWorkFactory
    {
        IScopedUnitOfWork Create();
    }
}
