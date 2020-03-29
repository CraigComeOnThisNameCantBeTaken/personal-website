using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Repositories.Books;

namespace Domain.Books
{
    public class BookService
    {
        private readonly IBookRepository bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Summary>> GetBooks()
        {
            return await bookRepository.GetAsync();
        }
    }
}
