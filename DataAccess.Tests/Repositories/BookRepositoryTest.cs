using System.Collections;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Tests.EntityFramework;
using Xunit;

namespace DataAccess.Tests.Repositories
{
    public class BookRepositoryTest
    {
        private readonly IDatabase<Book> database;
        private readonly BookRepository sut;

        public BookRepositoryTest()
        {
            var efDatabase = new EntityFrameworkDatabase<Book>();
            database = efDatabase;
            sut = new BookRepository(efDatabase.dataContext);
        }

        [Fact]
        public async Task AddAsync_Should_InsertNewRecordToDatabase()
        {
            var newBook = new Book
            {
                Name = "test book",
                PurchaseLink = "a purchase link",
                Review = new Review
                {
                    LearningRating = 1,
                    ReadabilityRating = 1,
                    Text = "test text"
                }
            };

            await sut.AddAsync(newBook);

            var state = database.Get();
            Assert.Contains(newBook, state);
        }
    }
}
