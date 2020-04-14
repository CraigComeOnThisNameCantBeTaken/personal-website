using System.Threading.Tasks;
using Dapper;
using DataAccess.Dapper.ConnectionProvider;
using DataAccess.Queries;

namespace DataAccess.Dapper
{
    public class ProfileSummaryQuery : IProfileSummaryQuery
    {
        private readonly IConnectionFactory connectionFactory;

        public ProfileSummaryQuery(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<ProfileSummary> GetAsync()
        {
            using(var connection = connectionFactory.GetConnection())
            {
                connection.Open();
                
                return await connection
                    .QueryFirstAsync<ProfileSummary>(@"SELECT 
	                    (SELECT COUNT(Id) FROM dbo.GitRepos) AS ProjectNum,
	                    (SELECT COUNT(Id) FROM dbo.GitCommits) AS CommitsNum,
	                    (SELECT COUNT(Id) FROM dbo.Reviews) AS BookReviewsNum");
            }
        }
    }
}
