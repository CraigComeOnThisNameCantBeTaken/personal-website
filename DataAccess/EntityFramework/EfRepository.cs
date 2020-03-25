using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFramework
{
    internal class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly DataContext dataContext;
        protected readonly DbSet<T> DbSet;

        public EfRepository(DataContext dataContext)
        {
            this.DbSet = dataContext.Set<T>();
            this.dataContext = dataContext;
        }

        public void Add(T data)
        {
            DbSet.Add(data);
        }

        public Task<int> CommitAsync()
        {
            return dataContext.SaveChangesAsync();
        }

        public void Delete(T data)
        {
            DbSet.Remove(data);
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await DbSet.ToListAsync();
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return DbSet.FirstOrDefaultAsync();
        }

        public void Update(T data)
        {
            DbSet.Update(data);
        }
    }
}
