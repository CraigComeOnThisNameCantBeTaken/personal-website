using static Books.Domain.Books.Book;
using DbEntities = DataAccess.Entities;

namespace Books.Domain.Books
{
    internal static class BookExtensions
    {
        public static DbEntities.Book ToUpsertable(this Book data)
        {
            return new DbEntities.Book
            {
                Id = data.Id,
                Name = data.Name,
                PurchaseLink = data.PurchaseLink,
                Review = new DbEntities.Review
                {
                    LearningRating = data.Review.LearningRating,
                    ReadabilityRating = data.Review.ReadabilityRating,
                    Text = data.Review.Text
                }
            };
        }

        public static Book ToDomain(this DbEntities.Book data)
        {
            return new Book
            {
                Id = data.Id,
                Name = data.Name,
                PurchaseLink = data.PurchaseLink,
                Review = new BookReview
                {
                    LearningRating = data.Review.LearningRating,
                    ReadabilityRating = data.Review.ReadabilityRating,
                    Text = data.Review.Text
                }
            };
        }
    }
}
