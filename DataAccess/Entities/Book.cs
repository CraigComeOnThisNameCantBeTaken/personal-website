using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Entities
{
    public class Book : ValueEntity
    {
        public string PurchaseLink { get; set; }
        public virtual Review Review { get; set; }
    }

    internal class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder
                .Property(b => b.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(b => b.PurchaseLink)
                .HasMaxLength(100);

            builder
                .HasOne(b => b.Review);
        }
    }
}
