using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;

namespace Domain.Books
{
    public interface IBookService
    {
        Task<IEnumerable<Summary>> GetBooksAsync();
    }
}
