using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Entities
{
    public class GitCommit : ValueEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Sha { get; set; }

        public Guid RepoId { get; set; }
        public virtual GitRepo Repo { get; set; }
    }

    internal class GitCommitConfiguration : IEntityTypeConfiguration<GitCommit>
    {
        public void Configure(EntityTypeBuilder<GitCommit> builder)
        {
            builder
                .Property(b => b.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder
             .Property(b => b.Description)
             .HasMaxLength(200)
             .IsRequired();

            builder
             .Property(b => b.Sha)
             .HasMaxLength(50) // should be only require 40 but just on safe side
             .IsRequired();

            builder
                .HasOne(c => c.Repo)
                .WithMany(r => r.Commits)
                .HasForeignKey(c => c.RepoId);
        }
    }
}
