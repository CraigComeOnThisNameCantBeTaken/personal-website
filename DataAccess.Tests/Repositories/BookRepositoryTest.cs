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
    public class BookRepositoryTest : BaseRepositoryTest<Book>
    {
        private readonly IDatabase<Book> _database;
        private readonly IRepository<Book> _sut;
        private readonly IEntityFactory<Book> _entityFactory;

        protected override IDatabase<Book> Database { get { return _database; } }
        protected override IRepository<Book> Sut { get { return _sut; } }
        protected override IEntityFactory<Book> EntityFactory { get { return _entityFactory; } }

        public BookRepositoryTest()
        {
            var efDatabase = new EntityFrameworkDatabase<Book>();
            _database = efDatabase;

            var mockLogger = new Mock<ILogger<BookRepository>>();
            _sut = new BookRepository(efDatabase.dataContext, mockLogger.Object);
            _entityFactory = new BookFactory();
        }

        public override async Task UpdateAsync_Should_UpdateRecord()
        {
            var bookToUpdate = Database.AddData(EntityFactory.Create());

            bookToUpdate.Name = "a new name";
            bookToUpdate.PurchaseLink = "a new purchase link";
            bookToUpdate.Review.LearningRating = bookToUpdate.Review.LearningRating++;
            bookToUpdate.Review.ReadabilityRating = bookToUpdate.Review.ReadabilityRating++;
            bookToUpdate.Review.Text = "some new text";

            await Sut.UpdateAsync(bookToUpdate);
            var state = Database.Get().First();

            Assert.Equal(bookToUpdate, state);
        }
    }
}
