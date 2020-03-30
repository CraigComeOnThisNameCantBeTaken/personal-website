using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using GitIntegration.OnDemand;
using GitIntegration.OnDemand.GitHub.Responses;
using GitIntegration.Resolvers;
using Newtonsoft.Json;

namespace GitIntegration.GitHub
{
    public class GitHubIntegrator : IGitIntegrator
    {
        private readonly GitIntegrationOption option;
        private readonly HttpClient client = new HttpClient();

        public GitHubIntegrator(OptionResolver optionResolver)
        {
            option = optionResolver.Resolve(SupportedIntegrations.GitHub);
        }

        public async Task<IEnumerable<GitRepo>> GetReposAsync()
        {
            var repos = await GetBasicRepoDataAsync();

            foreach (var repo in repos)
            {
                await EnrichRepoWithCommitsAsync(repo);
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
                Commits = new List<GitCommit>()
            });
        }

        private async Task EnrichRepoWithCommitsAsync(GitRepo repo)
        {
            var response = await client
                .GetAsync($"{option.Endpoint}/repos/{option.UserName}/{repo.Name}/commits");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Unable to fetch commits for" + repo.Name);
            }

            var body = await response.Content.ReadAsStringAsync();
            var commitData = JsonConvert.DeserializeObject<IEnumerable<CommitData>>(body);
            repo.Commits = commitData
                .Select(data => new GitCommit
                {
                    Message = data.Commit.Message,
                    Sha = data.Sha
                })
                .ToList();
        }
    }
}
