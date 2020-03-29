using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
    public abstract class BaseRepositoryTest<TEntity, TRepository> where TEntity : ValueEntity
    {
        protected abstract IDatabase<TEntity> Database { get; }

        protected abstract IRepository<TEntity> Sut { get; }

        protected abstract Mock<ILogger<TRepository>> MockLogger { get; }

        protected abstract IEntityFactory<TEntity> EntityFactory { get; }

        #region AddAsync
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

        [Fact]
        public async Task AddAsync_ShouldLogErrorWhen_Throws()
        {
            var newEntity = EntityFactory.Create();
            Database.AddData(newEntity);

            try
            {
                await Sut.AddAsync(newEntity);
            }
            catch {
                string pattern = $"Failed to add new {typeof(TEntity).Name} with id: {newEntity.Id}";
                MockLogger.VerifyLog(LogLevel.Error, pattern, Times.Once());
            }
        }

        [Fact]
        public async Task AddAsync_ShouldLogInformationWhen_Succeeds()
        {
            var newEntity = EntityFactory.Create();
            await Sut.AddAsync(newEntity);

            string pattern = $"Added new {typeof(TEntity).Name} with id: {newEntity.Id}";
            MockLogger.VerifyLog(LogLevel.Information, pattern, Times.Once());
        }
        #endregion

        #region AddRangeAsync
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
        public async Task AddRangeAsync_ShouldLogInformationWhen_Succeeds()
        {
            var newEntities = EntityFactory.CreateMany(3);
            await Sut.AddRangeAsync(newEntities);

            var ids = string.Join(", ", newEntities.Select(d => d.Id.ToString()));
            string message = $@"Added muliple new books with ids: {ids}";
            MockLogger.VerifyLog(LogLevel.Information, message, Times.Once());
        }

        [Fact]
        public async Task AddRangeAsync_ShouldLogErrorWhen_Throws()
        {
            var newEntities = EntityFactory.CreateMany(2);
            var addedRecord = Database.AddData(newEntities.First());
            var insertable = newEntities.CloneAndReplaceAt(0, addedRecord);

            try
            {
                await Sut.AddRangeAsync(insertable);
            }
            catch
            {
                string ids = string.Join(", ", insertable.Select(d => d.Id.ToString()));
                string message = $@"Failed to add new Books with ids: {ids}";
                MockLogger.VerifyLog(LogLevel.Error, message, Times.Once());
            }
        }
        #endregion

        #region DeleteAsync
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

        [Fact]
        public async Task DeleteAsync_ShouldLogInformationWhen_Succeeds()
        {
            var newEntity = EntityFactory.Create();
            var inserted = Database.AddData(newEntity);

            await Sut.DeleteAsync(inserted);
            var message = $"Deleted {typeof(TEntity).Name} with id: {inserted.Id}";
            MockLogger.VerifyLog(LogLevel.Information, message, Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_ShouldLogErrorWhen_Throws()
        {
            var newEntity = EntityFactory.Create();
            newEntity.Id = Guid.NewGuid();

            try
            {
                await Sut.DeleteAsync(newEntity);
            }
            catch
            {
                string message = $"Attempt to delete a {typeof(TEntity).Name} failed for id: {newEntity.Id}";
                MockLogger.VerifyLog(LogLevel.Error, message, Times.Once());
            }
        }
        #endregion

        #region UpdateAsync
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
        public abstract Task UpdateAsync_ShouldLogInformationWhen_Succeeds();

        [Fact]
        public async Task UpdateAsync_ShouldLogErrorWhen_Throws()
        {
            var entity = EntityFactory.Create();
            entity.Id = Guid.NewGuid();

            try
            {
                await Sut.UpdateAsync(entity);
            }
            catch
            {
                string message = $"Attempt to update a {typeof(TEntity).Name} failed for id: {entity.Id}";
                MockLogger.VerifyLog(LogLevel.Error, message, Times.Once());
            }
        }
        #endregion

        #region GetAync
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
        #endregion

        #region GetSummaryByIdAsync
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
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GetByIdAsync_Should_ReturnNullWhenNoRecord()
        {
            var record = await Sut.GetByIdAsync(Guid.NewGuid());
            Assert.Null(record);
        }
        #endregion
    }
}
