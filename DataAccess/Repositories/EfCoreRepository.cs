using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class EfCoreRepository<T, TRepositoryType> : IRepository<T> where T : ValueEntity
    {
        protected readonly DataContext dataContext;
        protected readonly DbSet<T> dbSet;
        private readonly ILogger<TRepositoryType> logger;

        public EfCoreRepository(DataContext dataContext,
            ILogger<TRepositoryType> logger)
        {
            this.dataContext = dataContext;
            this.dbSet = dataContext.Set<T>();
            this.logger = logger;
        }

        public async Task AddAsync(T data)
        {
            try
            {
                await dbSet.AddAsync(data);
                await dataContext.SaveChangesAsync();
                logger.LogInformation("Added new " + typeof(T).Name + " with id: {ID}", data.Id);
            }
            catch
            {
                logger.LogError("Failed to add new " + typeof(T).Name + " with id: {ID}", data.Id);
                throw;
            }

        }

        public async Task AddRangeAsync(IEnumerable<T> data)
        {
            try
            {
                await dbSet.AddRangeAsync(data);
                await dataContext.SaveChangesAsync();
                logger.LogInformation("Added muliple new " + typeof(T).Name + "s with ids: {IDS}",
                    String.Join(", ", data.Select(d => d.Id.ToString()))
                );
            }
            catch(Exception e)
            {
                logger.LogError(
                    "Failed to add new " + typeof(T).Name + "s with ids: {IDS}",
                    String.Join(", ", data.Select(d => d.Id.ToString()))
                );
                throw;
            }
        }

        public async Task DeleteAsync(T data)
        {
            try
            {
                dbSet.Remove(data);
                await dataContext.SaveChangesAsync();
                logger.LogInformation("Deleted " + typeof(T).Name + " with id: {ID}", data.Id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                logger.LogError("Attempt to delete a " + typeof(T).Name + " failed for id: {ID}", data.Id);
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<T> data)
        {
            try
            {
                dbSet.RemoveRange(data);
                await dataContext.SaveChangesAsync();
                logger.LogInformation("Deleted muliple new " + typeof(T).Name + "s with ids: {IDS}",
                    String.Join(", ", data.Select(d => d.Id.ToString()))
                );
            }
            catch
            {
                logger.LogError(
                    "Failed to delete " + typeof(T).Name + "s with ids: {IDS}",
                    String.Join(", ", data.Select(d => d.Id.ToString()))
                );
                throw;
            }
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T data)
        {
            try
            {
                dataContext.Update(data);
                await dataContext.SaveChangesAsync();
                logger.LogInformation("Updated " + typeof(T).Name + " with id: {ID}", data.Id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                logger.LogError("Attempt to update a " + typeof(T).Name + " failed for id: {ID}", data.Id);
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task UpdateRangeAsync(IEnumerable<T> data)
        {
            dataContext.UpdateRange(data);
            await dataContext.SaveChangesAsync();
            logger.LogInformation("Updated muliple new " + typeof(T).Name + "s with ids: {IDS}",
                String.Join(", ", data.Select(d => d.Id.ToString()))
            );
        }

        public async Task<IEnumerable<Summary>> GetAsync()
        {
            return await dbSet
                .Select(b => new Summary
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .ToListAsync();
        }

        public async Task<Summary> GetSummaryByIdAsync(Guid id)
        {
            return await dbSet
                .Select(b => new Summary
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
