using System.Threading.Tasks;
using DataAccess.Queries;

namespace DataAccess.Dapper
{
    public interface IProfileSummaryQuery
    {
        Task<ProfileSummary> GetAsync();
    }
}
