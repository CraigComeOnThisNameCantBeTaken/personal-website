using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;

namespace GitIntegration.Resolvers
{
    internal class OptionResolver
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
