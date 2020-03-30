using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace GitIntegration.OnDemand
{
    public interface IGitIntegrator
    {
        Task<IEnumerable<GitRepo>> GetReposAsync();
    }
}
