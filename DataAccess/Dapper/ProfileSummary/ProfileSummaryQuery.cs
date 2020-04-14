﻿using System.Threading.Tasks;
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
                    .QueryFirstAsync<ProfileSummary>(@"SELECT COUNT(r.Id) as BookReviewsNum,
                        COUNT(gr.Id) as ProjectNum,
                        COUNT(gc.Id) as CommitsNum
                        FROM dbo.Reviews r, dbo.GitRepos gr, dbo.GitCommits gc");
            }
        }
    }
}
