using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class Book
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = String.Empty;

        public string PurchaseLink { get; set; } = String.Empty;

        public BookReview? Review { get; set; }

        public class BookReview
        {
            public int LearningRating { get; set; }
            public int ReadabilityRating { get; set; }
            public string Text { get; set; } = String.Empty;
        }
    }
}
