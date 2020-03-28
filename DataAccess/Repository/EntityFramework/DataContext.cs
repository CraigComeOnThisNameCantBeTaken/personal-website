using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DataAccess.Repository.EntityFramework
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var EditedEntities = ChangeTracker.Entries()
                .Where(E => E.State == EntityState.Modified)
                .ToList();

            EditedEntities.ForEach(E =>
            {
                E.Property(nameof(ValueEntity.UpdatedAt)).CurrentValue = DateTime.UtcNow;
            });

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.ConfigureValueEntities(modelBuilder);

            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureValueEntities(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(ValueEntity).IsAssignableFrom(e.ClrType)))
            {
                modelBuilder
                    .Entity(entityType.ClrType)
                    .Property(nameof(ValueEntity.Id))
                    .HasDefaultValueSql("NEWSEQUENTIALID()")
                    .ValueGeneratedOnAdd();

                modelBuilder
                    .Entity(entityType.ClrType)
                    .Property(nameof(ValueEntity.CreatedAt))
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                modelBuilder
                    .Entity(entityType.ClrType)
                    .Property(nameof(ValueEntity.UpdatedAt));
            }
        }
    }
}
