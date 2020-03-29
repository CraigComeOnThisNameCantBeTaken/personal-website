using DataAccess.Entities;
using DataAccess.EntityFramework;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories.GitRepos
{
    public class GitRepoRepository : EfCoreRepository<GitRepo, GitRepoRepository>, IGitRepoRepository
    {
        public GitRepoRepository(DataContext dataContext, ILogger<GitRepoRepository> logger)
            : base(dataContext, logger)
        {
        }
    }
}
