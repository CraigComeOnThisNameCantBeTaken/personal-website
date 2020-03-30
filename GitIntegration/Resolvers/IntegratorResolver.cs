using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
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

        public IGitIntegrator Resolve(string providerName)
        {
            switch (providerName)
            {
                case SupportedIntegrations.GitHub:
                    return provider.GetRequiredService<GitHubIntegrator>();
                default:
                    throw new ConfigurationErrorsException($"Cannot resolve an integration service for {providerName}");
            }
        }
    }
}
