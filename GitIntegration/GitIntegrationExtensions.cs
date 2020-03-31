﻿using System;
using GitIntegration.GitHub;
using GitIntegration.OnDemand;
using GitIntegration.Public;
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

            // resolvers for internal work
            serviceCollection.AddSingleton<IOptionResolver, OptionResolver>();

            // git provider integration services
            serviceCollection.AddSingleton<GitHubIntegrator>();
            serviceCollection.AddSingleton<IGitIntegrator>(c =>
                new IntegrationAggregator(
                    new IGitIntegrator[]
                    {
                        c.GetRequiredService<GitHubIntegrator>()
                    }
                )
            );

            // service for use external to the project
            serviceCollection.AddSingleton<IGitIntegrationService, GitIntegrationService>();

            return serviceCollection;
        }
    }
}
