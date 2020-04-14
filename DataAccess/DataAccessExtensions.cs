using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Dapper;
using DataAccess.Dapper.ConnectionFactory;
using DataAccess.Dapper.ConnectionProvider;
using DataAccess.EntityFramework;
using DataAccess.Queries;
using DataAccess.Repositories;
using DataAccess.Repositories.Books;
using DataAccess.Repositories.GitRepos;
using DataAccess.UnitOfWork;
using DataAccess.UnitOfWork.TransactionScopeUnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddDataAccess(
            this IServiceCollection serviceCollection,
            DataAccessOptions contextOptions
        )
        {
            if (contextOptions == null)
            {
                throw new ArgumentNullException(nameof(contextOptions),
                    @"Please provide options for Data Access.");
            }

            serviceCollection.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(contextOptions.ConnectionString);
            });

            serviceCollection.AddSingleton<IScopedUnitOfWorkFactory, TransactionScopeUnitOfWorkFactory>();
            serviceCollection.AddScoped<IBookRepository, BookRepository>();
            serviceCollection.AddScoped<IGitRepoRepository, GitRepoRepository>();

            serviceCollection.AddSingleton<IConnectionFactory>(new ConnectionFactory(contextOptions.ConnectionString));
            serviceCollection.AddTransient<IProfileSummaryQuery, ProfileSummaryQuery>();

            return serviceCollection;
        }
    }
}
