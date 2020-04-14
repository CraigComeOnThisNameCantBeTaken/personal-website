using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Entities;
using GitIntegration.OnDemand;
using Moq;
using Xunit;

namespace GitIntegration.Tests
{
    public class GitIntegrationServiceTest
    {
        private readonly IGitIntegrationService sut;
        private readonly Mock<IGitIntegrator> dataProvider;

        public GitIntegrationServiceTest()
        {
            dataProvider = new Mock<IGitIntegrator>();
            sut = new GitIntegrationService(dataProvider.Object);
        }

        [Fact]
        public async Task GetGitRepoDataAsync_ShouldReturnDataFromInternalProvider()
        {
            var stubbedRepoData = new List<GitRepo>();
            dataProvider.Setup(dp => dp.GetReposAsync())
                .ReturnsAsync(stubbedRepoData);

            var result = await sut.GetGitRepoDataAsync();
            Assert.Equal(stubbedRepoData, result);
        }
    }
}
