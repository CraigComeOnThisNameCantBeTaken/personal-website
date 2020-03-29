using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Tests.EntityFactories;
using DataAccess.Tests.EntityFramework;
using DataAccess.Tests.Extensions;
using Moq;
using Xunit;

namespace DataAccess.Tests.Repositories
{
    public abstract class BaseRepositoryTest<T> where T : ValueEntity
    {
        protected abstract IDatabase<T> Database { get; }

        protected abstract IRepository<T> Sut { get; }

        protected abstract IEntityFactory<T> EntityFactory { get; }

        [Fact]
        public async Task AddAsync_Should_InsertNewRecordToDatabase()
        {
            var newEntity = EntityFactory.Create();
            await Sut.AddAsync(newEntity);

            var state = Database.Get();
            Assert.Contains(newEntity, state);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowWhen_InsertDuplicateRecordToDatabase()
        {
            var newEntity = EntityFactory.Create();
            Database.AddData(newEntity);

            await Assert.ThrowsAsync<ArgumentException>(() => Sut.AddAsync(newEntity));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(20)]
        public async Task AddRangeAsync_Should_InsertNewRecordsToDatabase(int amount)
        {
            var newEntities = EntityFactory.CreateMany(amount);
            await Sut.AddRangeAsync(newEntities);

            var insertedEntities = Database.Get();
            Assert.All(newEntities, (entityToInsert) => insertedEntities.Contains(entityToInsert));
        }

        [Fact]
        public async Task AddRangeAsync_ShouldThrowWhen_InsertDuplicateRecordToDatabase()
        {
            var newEntities = EntityFactory.CreateMany(2);
            var addedRecord = Database.AddData(newEntities.First());
            var insertable = newEntities.CloneAndReplaceAt(0, addedRecord);

            await Assert.ThrowsAsync<ArgumentException>(() => Sut.AddRangeAsync(insertable));
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteRecordFromDatabase()
        {
            var newEntities = EntityFactory.CreateMany(2);
            var inserted = Database.AddData(newEntities);

            await Sut.DeleteAsync(inserted.ElementAt(0));

            var finalState = Database.Get();
            Assert.Single(finalState);
            Assert.Equal(finalState.First().Id, inserted.ElementAt(1).Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowWhen_DeleteNonExistentRecordFromDatabase()
        {
            var newEntity = EntityFactory.Create();
            newEntity.Id = Guid.NewGuid();

            await Assert.ThrowsAsync<ArgumentException>(() => Sut.DeleteAsync(newEntity));
        }

        // this is abstract because we want to ensure the entire aggregate root tree is being updated
        // which is hard to do using reflection and therefore low confidence for a test
        [Fact]
        public abstract Task UpdateAsync_Should_UpdateRecord();

        [Fact]
        public async Task UpdateAsync_ShouldThrowWhen_RecordDoesNotExist()
        {
            var entity = EntityFactory.Create();
            entity.Id = Guid.NewGuid();
            await Assert.ThrowsAsync<ArgumentException>(() => Sut.UpdateAsync(entity));
        }

        [Fact]
        public async Task GetAsync_Should_ReturnAllRecordsAsSummaries()
        {
            var insertedEntities = Database.AddData(EntityFactory.CreateMany(2));

            var summaries = await Sut.GetAsync();

            Assert.Equal(2, summaries.Count());
            foreach (var summary in summaries)
            {
                Assert.NotEmpty(summary.Name);
            }
        }

        [Fact]
        public async Task GetSummaryByIdAsync_Should_ReturnRecordAsSummary()
        {
            var insertedEntity = Database.AddData(EntityFactory.Create());

            var summary = await Sut.GetSummaryByIdAsync(insertedEntity.Id);
            Assert.NotEmpty(summary.Name);
        }

        [Fact]
        public async Task GetSummaryByIdAsync_Should_ReturnNullWhenNoRecord()
        {
            var summary = await Sut.GetSummaryByIdAsync(Guid.NewGuid());
            Assert.Null(summary);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnRecord()
        {
            var insertedEntity = Database.AddData(EntityFactory.Create());

            var record = await Sut.GetByIdAsync(insertedEntity.Id);
            Assert.Equal(insertedEntity, record);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnNullWhenNoRecord()
        {
            var record = await Sut.GetByIdAsync(Guid.NewGuid());
            Assert.Null(record);
        }
    }
}
