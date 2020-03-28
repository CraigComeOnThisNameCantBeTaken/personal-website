using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository;
using DataAccess.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore;
using DbEntities = DataAccess.Entities;

namespace Books.Domain.Books
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
            await dataContext.Books.AddAsync(ConvertToUpsertableModel(data));
            await dataContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Book> data)
        {
            var insertable = data.Select(b => ConvertToUpsertableModel(b));
            await dataContext.Books.AddRangeAsync(insertable);
            await dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Book data)
        {
            var record = await dataContext.Books.FindAsync(data.Id);
            if(record == null)
            {
                throw new ArgumentException($"Cannot find book to delete with id {data.Id}");
            }

            dataContext.Books.Remove(record);
            await dataContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<Book> data)
        {
            var records = await dataContext.Books
                .Where(b => data.Any(d => d.Id == b.Id))
                .ToListAsync();
            dataContext.Books.RemoveRange(records);
            await dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetAsync()
        {
            return await dataContext.Books
                .Select(db => db.ToDomain())
                .ToListAsync();
        }

        public async Task<Book> GetByIdAsync(Guid id)
        {
            var data = await dataContext.Books.FindAsync(id);
            return data?.ToDomain();
        }

        public async Task UpdateAsync(Book data)
        {
            var toUpdate = data.ToUpsertable();
            dataContext.Update(toUpdate);
            await dataContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<Book> data)
        {
            var updatable = data.Select(d => d.ToUpsertable());
            dataContext.UpdateRange(updatable);
            await dataContext.SaveChangesAsync();
        }

        private DbEntities.Book ConvertToUpsertableModel(Book data)
        {
            return new DbEntities.Book
            {
                Id = data.Id,
                Name = data.Name,
                PurchaseLink = data.PurchaseLink,
                Review = new DbEntities.Review
                {
                    LearningRating = data.Review.LearningRating,
                    ReadabilityRating = data.Review.ReadabilityRating,
                    Text = data.Review.Text
                }
            };
        }
    }
}
