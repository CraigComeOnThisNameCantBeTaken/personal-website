using System.Threading.Tasks;
using DataAccess.Dapper;
using DataAccess.Queries;

namespace Domain.Summaries
{
    public class ProfileSummaryService
    {
        private readonly IProfileSummaryQuery profileSummaryProvider;

        public ProfileSummaryService(IProfileSummaryQuery profileSummaryProvider)
        {
            this.profileSummaryProvider = profileSummaryProvider;
        }

        public Task<ProfileSummary> GetProfileSummaryAsync()
        {
            return profileSummaryProvider.GetAsync();
        }
    }
}
