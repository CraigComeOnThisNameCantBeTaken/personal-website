using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Entities;

namespace DataAccess.Tests.EntityFactories
{
    public class GitRepoFactory : IEntityFactory<GitRepo>
    {
        public GitRepo Create()
        {
            var randomIdentifier = Guid
                .NewGuid()
                .ToString();

            return new GitRepo
            {
                Name = $"test book ${randomIdentifier}",
                Description = $"a description ${randomIdentifier}",
                Url = $"a url ${randomIdentifier}",
                Commits = new List<GitCommit>()
            };
        }

        public IEnumerable<GitRepo> CreateMany(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                yield return Create();
            }
        }
    }
}
