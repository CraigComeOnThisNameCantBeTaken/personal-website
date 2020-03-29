using DataAccess.Entities;
using DataAccess.EntityFramework;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories.Books
{
    public class BookRepository : EfCoreRepository<Book, BookRepository>, IBookRepository
    {
        public BookRepository(DataContext dataContext, ILogger<BookRepository> logger)
            : base(dataContext, logger)
        {
        }
    }
}
