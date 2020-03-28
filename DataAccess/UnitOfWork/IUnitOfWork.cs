using System.Threading.Tasks;

namespace DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
