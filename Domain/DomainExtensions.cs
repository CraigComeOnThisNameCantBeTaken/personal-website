using Domain.Books;
using Domain.Summaries;
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
            serviceCollection.AddTransient<ProfileSummaryService>();

            return serviceCollection;
        }
    }
}
