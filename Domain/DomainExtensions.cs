using Domain.Books;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddDomain(
            this IServiceCollection serviceCollection
        )
        {
            serviceCollection.AddScoped<IBookService, BookService>();
            return serviceCollection;
        }
    }
}
