using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Repository;

namespace DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
