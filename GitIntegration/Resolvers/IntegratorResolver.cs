using System;
using System.Configuration;
using GitIntegration.GitHub;
using GitIntegration.OnDemand;
using Microsoft.Extensions.DependencyInjection;

namespace GitIntegration.Resolvers
{
    public class IntegratorResolver
    {
        private readonly IServiceProvider provider;

        public IntegratorResolver(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public IGitIntegrator Resolve<T>(T typeToProvide = null) where T : Type
        {
            if(typeToProvide == null)
            {
                return provider.GetRequiredService<IGitIntegrator>();
            }

            return provider.GetRequiredService(typeof(T)) as IGitIntegrator;
        }

        public IGitIntegrator Resolve()
        {
            return provider.GetRequiredService<IntegrationAggregator>();
        }
    }
}
