using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync();

        Task<T> GetByIdAsync(Guid id);

        void Add(T data);

        void Delete(T data);

        void Update(T data);

        Task<int> CommitAsync();
    }
}
