using System;
using GitIntegration.GitHub;
using GitIntegration.OnDemand;
using GitIntegration.Resolvers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GitIntegration
{
    public static class GitIntegrationExtensions
    {
        public static IServiceCollection AddGitIntegration(
            this IServiceCollection serviceCollection,
            IConfigurationSection configSection
        )
        {
            if (configSection == null)
            {
                throw new ArgumentNullException(nameof(IConfigurationSection),
                    @"Please provide options for Git Integration.");
            }

            serviceCollection.Configure<GitIntegrationOption>(
                "GitHub",
                (options) => {
                    configSection.GetSection("GitHub").Bind(options);
                }
            );

            serviceCollection.AddSingleton<OptionResolver>();
            serviceCollection.AddSingleton<IntegratorResolver>();
            serviceCollection.AddSingleton<IntegrationAggregator>();
            serviceCollection.AddSingleton<IGitIntegrator, GitHubIntegrator>();

            return serviceCollection;
        }
    }
}
