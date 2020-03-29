using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Entities;

namespace DataAccess.Tests.EntityFactories
{
    public class BookFactory
    {
        public Book Create()
        {
            var randomIdentifier = Guid
                .NewGuid()
                .ToString();

            return new Book
            {
                Name = $"test book ${randomIdentifier}",
                PurchaseLink = $"a purchase link ${randomIdentifier}",
                Review = new Review
                {
                    LearningRating = 1,
                    ReadabilityRating = 1,
                    Text = $"test text ${randomIdentifier}"
                }
            };
        }

        public IEnumerable<Book> CreateMany(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                yield return Create();
            }
        }
    }
}
