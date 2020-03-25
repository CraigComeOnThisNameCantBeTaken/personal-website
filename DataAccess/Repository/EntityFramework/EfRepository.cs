using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository.EntityFramework
{
    internal class EfRepository<T> : IRepository<T> where T : class
    {
        protected readonly DataContext DataContext;
        protected readonly DbSet<T> DbSet;

        public EfRepository(DataContext dataContext)
        {
            this.DbSet = dataContext.Set<T>();
            this.DataContext = dataContext;
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await DbSet.ToListAsync();
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return DbSet.FirstOrDefaultAsync();
        }

        public async Task AddAsync(T data)
        {
            await DbSet.AddAsync(data);
            await DataContext.SaveChangesAsync();
        }

        public Task UpdateAsync(T data)
        {
            DbSet.Update(data);
            return DataContext.SaveChangesAsync();
        }

        public Task DeleteAsync(T data)
        {
            DbSet.Remove(data);
            return DataContext.SaveChangesAsync();
        }
    }
}
