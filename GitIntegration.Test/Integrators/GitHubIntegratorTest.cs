using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DataAccess.Entities;
using GitIntegration.GitHub;
using GitIntegration.OnDemand.GitHub.Responses;
using GitIntegration.Resolvers;
using GitIntegration.Tests.Fixtures;
using Moq;
using Xunit;

namespace GitIntegration.Tests.Integrators
{
    public class GitHubIntegratorTest
    {
        private readonly GitHubIntegrator sut;
        private readonly StubbedHttpClientFixture httpFixture;

        private const string endpoint = "http://githubapifake.com";
        private const string userName = "TestUserName";

        public GitHubIntegratorTest()
        {
            httpFixture = new StubbedHttpClientFixture();

            var optionResolver = new Mock<IOptionResolver>();
            optionResolver.Setup(or => or.Resolve(It.IsAny<string>()))
                .Returns(new GitIntegrationOption
                {
                    IntegrationName = "GitHub",
                    Endpoint = endpoint,
                    UserName = userName
                });

            sut = new GitHubIntegrator(httpFixture.HttpClient, optionResolver.Object);
        }

        [Fact]
        public async Task GetBasicRepoDataAsync_NoData_ShouldThrow()
        {
            httpFixture.WithResponse($"{endpoint}/users/{userName}/repos", HttpStatusCode.NotFound);
            await Assert.ThrowsAsync<Exception>(() => sut.GetReposAsync());
        }

        [Fact]
        public async Task GetReposAsync_RepoWithoutCommitsData_ShouldReturnRepos()
        {
            var stubbedResponseData = new List<RepoData> {
                new RepoData
                {
                    Description = "A description",
                    Name = "A name",
                    HtmlUrl = "a url"
                },
                 new RepoData
                {
                    Description = "A description 2",
                    Name = "A name 2",
                    HtmlUrl = "a url 2"
                }
            };

            httpFixture.WithResponse($"{endpoint}/users/{userName}/repos", stubbedResponseData, HttpMethod.Get);
            httpFixture.WithResponse($"{endpoint}/repos/{userName}/{stubbedResponseData.First().Name}/commits", Enumerable.Empty<CommitData>(), HttpMethod.Get);
            httpFixture.WithResponse($"{endpoint}/repos/{userName}/{stubbedResponseData.Last().Name}/commits", Enumerable.Empty<CommitData>(), HttpMethod.Get);
            
            var result = await sut.GetReposAsync();

            Assert.Equal(stubbedResponseData.First().Name, result.First().Name);
            Assert.Equal(stubbedResponseData.First().Description, result.First().Description);
            Assert.Equal(stubbedResponseData.First().HtmlUrl, result.First().Url);

            Assert.Equal(stubbedResponseData.Last().Name, result.Last().Name);
            Assert.Equal(stubbedResponseData.Last().Description, result.Last().Description);
            Assert.Equal(stubbedResponseData.Last().HtmlUrl, result.Last().Url);
        }

        [Fact]
        public async Task GetReposAsync_RepoWithCommitsData_ShouldReturnRepos()
        {
            var stubbedRepoResponseData = new List<RepoData> {
                new RepoData
                {
                    Description = "A description",
                    Name = "A name",
                    HtmlUrl = "a url"
                },
                 new RepoData
                {
                    Description = "A description 2",
                    Name = "A name 2",
                    HtmlUrl = "a url 2"
                }
            };
            httpFixture.WithResponse($"{endpoint}/users/{userName}/repos", stubbedRepoResponseData, HttpMethod.Get);

            var stubbedRepoOneCommits = new List<CommitData> {
                new CommitData
                {
                    Sha = "aSha",
                    Commit = new CommitBody
                    {
                        Message = "a message"
                    }
                }
            };
            httpFixture.WithResponse($"{endpoint}/repos/{userName}/{stubbedRepoResponseData.First().Name}/commits", stubbedRepoOneCommits, HttpMethod.Get);

            var stubbedRepoTwoCommits = new List<CommitData> {
                new CommitData
                {
                    Sha = "aShaTwo",
                    Commit = new CommitBody
                    {
                        Message = "a message two"
                    }
                }
            };
            httpFixture.WithResponse($"{endpoint}/repos/{userName}/{stubbedRepoResponseData.Last().Name}/commits", stubbedRepoTwoCommits, HttpMethod.Get);
            
            var result = await sut.GetReposAsync();
            Assert.Equal(stubbedRepoOneCommits.First().Commit.Message, result.First().Commits.First().Message);
            Assert.Equal(stubbedRepoTwoCommits.First().Commit.Message, result.Last().Commits.First().Message);
        }
    }
}
