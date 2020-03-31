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
        private readonly IEnumerable<IGitIntegrator> integrators;

        public IntegrationAggregator(IEnumerable<IGitIntegrator> _integrators)
        {
            integrators = _integrators;
        }

        public async Task<IEnumerable<GitRepo>> GetReposAsync()
        {
            var data = new List<GitRepo>();
            foreach (var integrator in integrators)
            {
                var repoData = await integrator.GetReposAsync();
                data.AddRange(repoData);
            }

            return data;
        }
    }
}
