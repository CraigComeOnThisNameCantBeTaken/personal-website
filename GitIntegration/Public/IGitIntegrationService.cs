using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace GitIntegration.Public
{
    public interface IGitIntegrationService
    {
        Task<IEnumerable<GitRepo>> GetGitRepoDataAsync();
    }
}
