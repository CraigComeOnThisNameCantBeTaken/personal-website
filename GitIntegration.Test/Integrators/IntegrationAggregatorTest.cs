using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using GitIntegration.OnDemand;
using Moq;
using Xunit;

namespace GitIntegration.Tests.Integrators
{
    public class IntegrationAggregatorTest
    {
        [Fact]
        public async Task GetReposAsync_Should_ReturnDataFromAllIntegrators()
        {
            var integratorOne = new Mock<IGitIntegrator>();
            var integratorTwo = new Mock<IGitIntegrator>();

            var gitIntegrators = new List<IGitIntegrator>
            {
                integratorOne.Object,
                integratorTwo.Object
            };

            var integratorOneResults = new List<GitRepo>{
                new GitRepo {
                    Description = "description one",
                    Url = "Url one",
                    Commits = new List<GitCommit>()
                }
            };
            var integratorTwoResults = new List<GitRepo>{
                new GitRepo {
                    Description = "description two",
                    Url = "Url two",
                    Commits = new List<GitCommit>()
                }
            };

            integratorOne
                .Setup(i => i.GetReposAsync())
                .ReturnsAsync(integratorOneResults);
            integratorTwo
                .Setup(i => i.GetReposAsync())
                .ReturnsAsync(integratorTwoResults);

            var sut = new IntegrationAggregator(gitIntegrators);

            var expected = new List<GitRepo>();
            expected.AddRange(integratorOneResults);
            expected.AddRange(integratorTwoResults);

            var results = await sut.GetReposAsync();
            Assert.Equal(expected, results);
        }
    }
}
