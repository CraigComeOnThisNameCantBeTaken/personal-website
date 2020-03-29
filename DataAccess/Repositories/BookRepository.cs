using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class BookRepository : EfCoreRepository<Book, BookRepository>, IBookRepository
    {
        public BookRepository(DataContext dataContext, ILogger<BookRepository> logger)
            : base(dataContext, logger)
        {
        }
    }
}
