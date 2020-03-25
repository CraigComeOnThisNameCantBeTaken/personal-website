using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Entities
{
    public class Review : ValueEntity
    {
        public int LearningRating { get; set; }
        public int ReadabilityRating { get; set; }
        public string Text { get; set; }

        public Guid BookId { get; set; }
        public virtual Book Book { get; set; }
    }

    internal class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder
                .Property(nameof(Review.LearningRating))
                .IsRequired();

            builder
                .Property(nameof(Review.ReadabilityRating))
                .IsRequired();

            builder
                .Property(nameof(Review.Text))
                .IsRequired();

            builder
                .HasOne(r => r.Book)
                .WithOne(b => b.Review)
                .HasForeignKey<Review>(r => r.BookId);
        }
    }
}
