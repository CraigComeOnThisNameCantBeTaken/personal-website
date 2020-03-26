using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync();

        Task<T> GetByIdAsync(Guid id);

        Task AddAsync(T data);

        Task AddRangeAsync(IEnumerable<T> data);

        Task DeleteAsync(T data);

        Task DeleteRangeAsync(IEnumerable<T> data);

        Task UpdateAsync(T data);

        Task UpdateRangeAsync(IEnumerable<T> data);
    }
}
