using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using GitIntegration.Resolvers;

namespace GitIntegration
{
    public class IntegrationAggregator
    {
        private readonly IEnumerable<string> supportedIntegrations;
        private readonly IntegratorResolver integrationResolver;

        public IntegrationAggregator(IntegratorResolver integrationResolver)
        {
            this.supportedIntegrations = SupportedIntegrations.List;
            this.integrationResolver = integrationResolver;
        }

        public async Task<IEnumerable<GitRepo>> GetReposAsync()
        {
            var services = supportedIntegrations
                .Select(providerName => integrationResolver.Resolve(providerName));

            var data = new List<GitRepo>();
            foreach (var service in services)
            {
                var repos = await service.GetReposAsync();
                data.AddRange(repos);
            }

            return data;
        }
    }
}
