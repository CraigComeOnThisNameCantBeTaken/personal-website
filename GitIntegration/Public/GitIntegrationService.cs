using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Entities;
using GitIntegration.OnDemand;
using GitIntegration.Resolvers;

namespace GitIntegration.Public
{
    public class GitIntegrationService : IGitIntegrationService
    {
        private readonly IGitIntegrator dataProvider;

        public GitIntegrationService(IntegratorResolver resolver)
        {
            this.dataProvider = resolver.Resolve();
        }

        public Task<IEnumerable<GitRepo>> GetGitRepoDataAsync()
        {
            return dataProvider.GetReposAsync();
        }
    }
}
