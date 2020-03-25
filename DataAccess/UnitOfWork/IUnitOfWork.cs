using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Repository;

namespace DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CommitAsync(IRepository<object> repository);
        Task CommitAsync(IEnumerable<IRepository<object>> repositories);
    }
}
