using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Repositories;

namespace DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
