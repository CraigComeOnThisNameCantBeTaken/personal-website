using System.Configuration;
using Microsoft.Extensions.Options;

namespace GitIntegration.Resolvers
{
    internal class OptionResolver : IOptionResolver
    {
        private readonly IOptionsSnapshot<GitIntegrationOption> options;

        public OptionResolver(IOptionsSnapshot<GitIntegrationOption> options)
        {
            this.options = options;
        }

        public GitIntegrationOption Resolve(string provider)
        {
            var option = options.Get(provider);
            if (option == null)
                throw new ConfigurationErrorsException("Unable to resolve options for " + provider);

            return option;
        }
    }
}
