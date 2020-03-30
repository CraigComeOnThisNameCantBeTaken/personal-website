using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Entities;
using GitIntegration.OnDemand;

namespace GitIntegration.Public
{
    public class GitIntegrationService : IGitIntegrationService
    {
        private readonly IntegrationAggregator dataProvider;

        public GitIntegrationService(IntegrationAggregator dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        public Task<IEnumerable<GitRepo>> GetGitRepoDataAsync()
        {
            return dataProvider.GetReposAsync();
        }
    }
}
