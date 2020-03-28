using System;
using System.ComponentModel.DataAnnotations;

namespace Books.Domain.Books
{
    public class Book
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = String.Empty;

        [MaxLength(100)]
        public string PurchaseLink { get; set; } = String.Empty;

        [Required]
        public BookReview Review { get; set; }

        public class BookReview
        {
            public int LearningRating { get; set; }

            public int ReadabilityRating { get; set; }

            [Required]
            public string Text { get; set; } = String.Empty;
        }
    }
}
