using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace GitIntegration
{
    public interface IGitIntegrator
    {
        Task<IEnumerable<GitRepo>> GetReposAsync();
    }
}
