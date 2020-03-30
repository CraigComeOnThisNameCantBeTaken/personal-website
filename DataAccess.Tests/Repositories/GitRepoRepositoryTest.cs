using System.Linq;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Repositories.Books;
using DataAccess.Repositories.GitRepos;
using DataAccess.Tests.EntityFactories;
using DataAccess.Tests.EntityFramework;
using DataAccess.Tests.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DataAccess.Tests.Repositories
{
    public class GitRepoRepositoryTest : BaseRepositoryTest<GitRepo, GitRepoRepository>
    {
        private readonly IDatabase<GitRepo> _database;
        private readonly IGitRepoRepository _sut;
        private readonly IEntityFactory<GitRepo> _entityFactory;
        private readonly Mock<ILogger<GitRepoRepository>> _mockLogger;

        protected override IDatabase<GitRepo> Database { get { return _database; } }
        protected override IRepository<GitRepo> Sut { get { return _sut; } }
        protected override IEntityFactory<GitRepo> EntityFactory { get { return _entityFactory; } }
        protected override Mock<ILogger<GitRepoRepository>> MockLogger => _mockLogger;

        public GitRepoRepositoryTest()
        {
            var efDatabase = new EntityFrameworkDatabase<GitRepo>();
            _database = efDatabase;

            _mockLogger = new Mock<ILogger<GitRepoRepository>>();
            _sut = new GitRepoRepository(efDatabase.dataContext, _mockLogger.Object);
            _entityFactory = new GitRepoFactory();
        }

        public override async Task UpdateAsync_Should_UpdateRecord()
        {
            var toUpdate = Database.AddData(EntityFactory.Create());

            toUpdate.Name = "a new name";
            toUpdate.Url = "a new url";
            toUpdate.Commits.Add(new GitCommit
            {
                Message = "a message",
                Sha = "a sha code"
            });

            await Sut.UpdateAsync(toUpdate);
            var state = Database.Get().First();

            Assert.Equal(toUpdate, state);
            Assert.Equal(toUpdate.Commits.First(), toUpdate.Commits.First());
        }

        public override async Task UpdateAsync_ShouldLogInformationWhen_Succeeds()
        {
            var toUpdate = Database.AddData(EntityFactory.Create());
            await Sut.UpdateAsync(toUpdate);

            var message = $"Updated GitRepo with id: {toUpdate.Id}";
            MockLogger.VerifyLog(LogLevel.Information, message, Times.Once());
        }
    }
}
