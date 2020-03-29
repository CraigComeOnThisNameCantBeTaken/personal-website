using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Tests.EntityFactories;
using DataAccess.Tests.EntityFramework;
using DataAccess.Tests.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DataAccess.Tests.Repositories
{
    public class BookRepositoryTest
    {
        private readonly IDatabase<Book> database;
        private readonly BookRepository sut;
        private readonly BookFactory bookFactory = new BookFactory();

        public BookRepositoryTest()
        {
            var efDatabase = new EntityFrameworkDatabase<Book>();
            database = efDatabase;

            var mockLogger = new Mock<ILogger<BookRepository>>();
            sut = new BookRepository(efDatabase.dataContext, mockLogger.Object);
        }

        [Fact]
        public async Task AddAsync_Should_InsertNewRecordToDatabase()
        {
            var newBook = bookFactory.Create();
            await sut.AddAsync(newBook);

            var state = database.Get();
            Assert.Contains(newBook, state);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowWhen_InsertDuplicateRecordToDatabase()
        {
            var newBook = bookFactory.Create();
            database.AddData(newBook);

            await Assert.ThrowsAsync<ArgumentException>(() => sut.AddAsync(newBook));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(20)]
        public async Task AddRangeAsync_Should_InsertNewRecordsToDatabase(int amount)
        {
            var newBooks = bookFactory.CreateMany(amount);
            await sut.AddRangeAsync(newBooks);

            var insertedBooks = database.Get();
            Assert.All(newBooks, (bookToInsert) => insertedBooks.Contains(bookToInsert));
        }

        [Fact]
        public async Task AddRangeAsync_ShouldThrowWhen_InsertDuplicateRecordToDatabase()
        {
            var newBooks = bookFactory.CreateMany(2);
            var addedRecord = database.AddData(newBooks.First());
            var insertable = newBooks.CloneAndReplaceAt(0, addedRecord);

            await Assert.ThrowsAsync<ArgumentException>(() => sut.AddRangeAsync(insertable));
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteRecordFromDatabase()
        {
            var newBooks = bookFactory.CreateMany(2);
            var inserted = database.AddData(newBooks);

            await sut.DeleteAsync(inserted.ElementAt(0));

            var finalState = database.Get();
            Assert.Single(finalState);
            Assert.Equal(finalState.First().Id, inserted.ElementAt(1).Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowWhen_DeleteNonExistentRecordFromDatabase()
        {
            var newBook = bookFactory.Create();
            newBook.Id = Guid.NewGuid();

            await Assert.ThrowsAsync<ArgumentException>(() => sut.DeleteAsync(newBook));
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdateRecord()
        {
            var bookToUpdate = database.AddData(bookFactory.Create());

            bookToUpdate.Name = "a new name";
            bookToUpdate.PurchaseLink = "a new purchase link";
            bookToUpdate.Review.LearningRating = bookToUpdate.Review.LearningRating++;
            bookToUpdate.Review.ReadabilityRating = bookToUpdate.Review.ReadabilityRating++;
            bookToUpdate.Review.Text = "some new text";

            await sut.UpdateAsync(bookToUpdate);
            var state = database.Get().First();
            
            Assert.Equal(bookToUpdate, state);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowWhen_RecordDoesNotExist()
        {
            var book = bookFactory.Create();
            book.Id = Guid.NewGuid();
            await Assert.ThrowsAsync<ArgumentException>(() => sut.UpdateAsync(book));
        }

        [Fact]
        public async Task GetAsync_Should_ReturnAllRecordsAsSummaries()
        {
            var insertedBooks = database.AddData(bookFactory.CreateMany(2));

            var summaries = await sut.GetAsync();

            Assert.Equal(2, summaries.Count());
            foreach (var summary in summaries)
            {
                var match = insertedBooks.First(inserted => inserted.Id == summary.Id);
                Assert.Equal(match.Name, summary.Name);
            }
        }

        [Fact]
        public async Task GetSummaryByIdAsync_Should_ReturnRecordAsSummary()
        {
            var insertedBook = database.AddData(bookFactory.Create());

            var summary = await sut.GetSummaryByIdAsync(insertedBook.Id);
            Assert.Equal(insertedBook.Name, summary.Name);
        }

        [Fact]
        public async Task GetSummaryByIdAsync_Should_ReturnNullWhenNoRecord()
        {
            var summary = await sut.GetSummaryByIdAsync(Guid.NewGuid());
            Assert.Null(summary);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnRecord()
        {
            var insertedBook = database.AddData(bookFactory.Create());

            var record = await sut.GetByIdAsync(insertedBook.Id);
            Assert.Equal(insertedBook, record);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnNullWhenNoRecord()
        {
            var record = await sut.GetByIdAsync(Guid.NewGuid());
            Assert.Null(record);
        }
    }
}
