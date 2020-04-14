using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DataAccess.Entities;
using GitIntegration.OnDemand;
using GitIntegration.OnDemand.GitHub.Responses;
using GitIntegration.Resolvers;
using Newtonsoft.Json;

namespace GitIntegration.GitHub
{
    internal class GitHubIntegrator : IGitIntegrator
    {
        private readonly GitIntegrationOption option;
        private readonly HttpClient client;

        public GitHubIntegrator(
            IHttpClientFactory httpClientFactory,
            IOptionResolver optionResolver)
        {
            client = httpClientFactory.CreateClient();
            option = optionResolver.Resolve(SupportedIntegrations.GitHub);
        }

        public async Task<IEnumerable<GitRepo>> GetReposAsync()
        {
            var repos = (await GetBasicRepoDataAsync()).ToList();

            foreach (var repo in repos)
            {
                var commitData = await EnrichRepoWithCommitsAsync(repo);
                repo.Commits = commitData;
            }

            return repos;
        }

        private async Task<IEnumerable<GitRepo>> GetBasicRepoDataAsync()
        {
            var response = await client.GetAsync($"{option.Endpoint}/users/{option.UserName}/repos");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Unable to fetch github repositories");
            }

            var body = await response.Content.ReadAsStringAsync();
            var repoData = JsonConvert.DeserializeObject<IEnumerable<RepoData>>(body);
            return repoData.Select(data => new GitRepo
            {
                Description = data.Description,
                Url = data.HtmlUrl,
                Name = data.Name,
                Commits = new List<GitCommit>()
            });
        }

        private async Task<ICollection<GitCommit>> EnrichRepoWithCommitsAsync(GitRepo repo)
        {
            var url = $"{option.Endpoint}/repos/{option.UserName}/{repo.Name}/commits";
            var response = await client
                .GetAsync($"{option.Endpoint}/repos/{option.UserName}/{repo.Name}/commits");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Unable to fetch commits for" + repo.Name);
            }

            var body = await response.Content.ReadAsStringAsync();
            var commitData = JsonConvert.DeserializeObject<IEnumerable<CommitData>>(body);
            return commitData
                .Select(data => new GitCommit
                {
                    Message = data.Commit.Message,
                    Sha = data.Sha
                })
                .ToList();
        }
    }
}
