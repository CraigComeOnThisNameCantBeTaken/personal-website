using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Entities
{
    public class GitRepo : ValueEntity
    {
        public string Description { get; set; }

        public string Url { get; set; }

        public virtual ICollection<GitCommit> Commits { get; set; }
    }

    internal class GitRepoConfiguration : IEntityTypeConfiguration<GitRepo>
    {
        public void Configure(EntityTypeBuilder<GitRepo> builder)
        {
            builder
                .Property(gr => gr.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder
             .Property(gr => gr.Url)
             .HasMaxLength(100)
             .IsRequired();
        }
    }
}
