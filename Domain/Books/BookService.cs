using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Domain.Books;
using DataAccess;
using DataAccess.Repositories;

namespace Domain.Books
{
    public class BookService
    {
        private readonly BookRepository bookRepository;

        public BookService(BookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Summary>> GetBooks()
        {
            return await bookRepository.GetAsync();
        }
    }
}
