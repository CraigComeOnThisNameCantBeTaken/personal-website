using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFramework
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<GitRepo> GitRepos { get; set; }

        public DbSet<GitCommit> GitCommits { get; set; }

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
            modelBuilder.ApplyConfiguration(new GitRepoConfiguration());
            modelBuilder.ApplyConfiguration(new GitCommitConfiguration());

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
