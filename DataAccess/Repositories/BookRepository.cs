using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        private readonly DataContext dataContext;

        public BookRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task AddAsync(Book data)
        {
            await dataContext.Books.AddAsync(data);
            await dataContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Book> data)
        {
            await dataContext.Books.AddRangeAsync(data);
            await dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Book data)
        {
            var record = await dataContext.Books.FindAsync(data.Id);
            if (record == null)
            {
                throw new ArgumentException($"Cannot find book to delete with id {data.Id}");
            }

            dataContext.Books.Remove(record);
            await dataContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<Book> data)
        {
            dataContext.Books.RemoveRange(data);
            await dataContext.SaveChangesAsync();
        }

        public async Task<Book> GetByIdAsync(Guid id)
        {
            var data = await dataContext.Books.FindAsync(id);
            return data;
        }

        public async Task UpdateAsync(Book data)
        {
            dataContext.Update(data);
            await dataContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<Book> data)
        {
            dataContext.UpdateRange(data);
            await dataContext.SaveChangesAsync();
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
