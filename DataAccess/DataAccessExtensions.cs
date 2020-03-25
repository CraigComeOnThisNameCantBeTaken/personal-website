using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Repository;
using DataAccess.Repository.EntityFramework;
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

            // although the intension is concrete repository instead of generic
            // this is still getting registered because in simple cases we may only need the basic methods
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            return serviceCollection;
        }
    }
}
