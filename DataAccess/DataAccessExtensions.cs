using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.EntityFramework;
using DataAccess.Repositories;
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
            serviceCollection.AddSingleton<BookRepository>();

            return serviceCollection;
        }
    }
}
