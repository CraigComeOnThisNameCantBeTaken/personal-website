using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

// Used to run migrations
namespace DataAccess.EntityFramework
{
    internal class DesignTimeDataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DesignTimeDataContextFactory()
        {

        }

        public DataContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseSqlServer("Server=localhost;Database=AspNetAuth;Trusted_Connection=True");
            return new DataContext(builder.Options);
        }
    }
}
