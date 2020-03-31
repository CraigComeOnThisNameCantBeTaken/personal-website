using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using GitIntegration.Resolvers;

namespace GitIntegration.OnDemand
{
    internal class IntegrationAggregator : IGitIntegrator
    {
        private readonly IntegratorResolver integrationResolver;

        public IntegrationAggregator(IntegratorResolver integrationResolver)
        {
            this.integrationResolver = integrationResolver;
        }

        public async Task<IEnumerable<GitRepo>> GetReposAsync()
        {
            var type = typeof(IGitIntegrator);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .Where(p => p != typeof(IntegrationAggregator));

            var services = types
                .Select(type => integrationResolver.Resolve(type));

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
