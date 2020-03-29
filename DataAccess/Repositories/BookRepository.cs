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
    public class BookRepository : IRepository<Book>
    {
        private readonly DataContext dataContext;
        private readonly ILogger logger;

        public BookRepository(DataContext dataContext,
            ILogger<BookRepository> logger)
        {
            this.dataContext = dataContext;
            this.logger = logger;
        }

        public async Task AddAsync(Book data)
        {
            try
            {
                await dataContext.Books.AddAsync(data);
                await dataContext.SaveChangesAsync();
                logger.LogInformation("Added new Book with id: {ID}", data.Id);
            }
            catch(ArgumentException)
            {
                logger.LogError("Failed to add new Book with id: {ID}", data.Id);
                throw;
            }

        }

        public async Task AddRangeAsync(IEnumerable<Book> data)
        {
            try
            {
                await dataContext.Books.AddRangeAsync(data);
                await dataContext.SaveChangesAsync();
                logger.LogInformation("Added muliple new books with ids: {IDS}",
                    String.Join(", ", data.Select(d => d.Id.ToString()))
                );
            }
            catch
            {
                logger.LogError(
                    "Failed to add new Books with ids: {IDS}",
                    String.Join(", ", data.Select(d => d.Id.ToString()))
                );
                throw;
            }
        }

        public async Task DeleteAsync(Book data)
        {
            try
            {
                dataContext.Books.Remove(data);
                await dataContext.SaveChangesAsync();
                logger.LogInformation("Deleted Book with id: {ID}", data.Id);
            }
            catch(DbUpdateConcurrencyException ex)
            {
                logger.LogError("Attempt to delete a book failed for {ID}", data.Id);
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<Book> data)
        {
            try
            {
                dataContext.Books.RemoveRange(data);
                await dataContext.SaveChangesAsync();
                logger.LogInformation("Deleted muliple new books with ids: {IDS}",
                    String.Join(", ", data.Select(d => d.Id.ToString()))
                );
            }
            catch
            {
                logger.LogError(
                    "Failed to delete Books with ids: {IDS}",
                    String.Join(", ", data.Select(d => d.Id.ToString()))
                );
                throw;
            }
        }

        public async Task<Book> GetByIdAsync(Guid id)
        {
            return await dataContext.Books.FindAsync(id);
        }

        public async Task UpdateAsync(Book data)
        {
            try
            {
                dataContext.Update(data);
                await dataContext.SaveChangesAsync();
                logger.LogInformation("Updated Book with id: {ID}", data.Id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                logger.LogError("Attempt to update a book failed for {ID}", data.Id);
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task UpdateRangeAsync(IEnumerable<Book> data)
        {
            dataContext.UpdateRange(data);
            await dataContext.SaveChangesAsync();
            logger.LogInformation("Updated muliple new books with ids: {IDS}",
                String.Join(", ", data.Select(d => d.Id.ToString()))
            );
        }

        public virtual async Task<IEnumerable<Summary>> GetAsync()
        {
            return await dataContext.Books
                .Select(b => new Summary
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .ToListAsync();
        }

        public virtual async Task<Summary> GetSummaryByIdAsync(Guid id)
        {
            return await dataContext.Books
                .Select(b => new Summary
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
